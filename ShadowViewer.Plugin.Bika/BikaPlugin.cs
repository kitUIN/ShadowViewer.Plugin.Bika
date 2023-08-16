using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.Storage;
using DryIoc;
using Microsoft.UI.Xaml.Controls;
using ShadowViewer.Plugin.Bika.Enums;
using PicaComic;
using PicaComic.Models;
using Serilog;
using ShadowViewer.Args;
using ShadowViewer.Controls;
using ShadowViewer.Enums;
using ShadowViewer.Helpers;
using ShadowViewer.Interfaces;
using ShadowViewer.Models;
using SqlSugar;
using ShadowViewer.Plugin.Bika.Models;
using ShadowViewer.Plugin.Bika.Controls;
using ShadowViewer.Plugin.Bika.Helpers;
using ShadowViewer.Plugin.Bika.Pages;
using ShadowViewer.Plugin.Bika.ViewModels;
using ShadowViewer.Plugins;
using ShadowViewer.Services;
using ShadowViewer.Extensions;
using ShadowViewer.Plugin.Bika.Args;
using ShadowViewer.ViewModels;
using CustomExtensions.WinUI;
using Microsoft.UI.Xaml;
using ShadowViewer.Plugin.Local.Services;
using ShadowViewer.Plugin.Local.ViewModels;

namespace ShadowViewer.Plugin.Bika;

[PluginMetaData("Bika",
    "ßÙßÇÂþ»­",
    "ßÙßÇÂþ»­ÊÊÅäÆ÷",
    "kitUIN", "0.1.0",
    "https://github.com/kitUIN/ShadowViewer.Plugin.Bika/",
    "ms-appx:///Assets/Icons/logo.png",
    20230808, 
    new []{"Local"},
    new []{"zh-CN"}
    )]
public partial class BikaPlugin : PluginBase
{
    /// <summary>
    /// Login Frame
    /// </summary>
    public static LoginTip MainLoginTip { get; set; }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override LocalTag AffiliationTag { get; } =
        new(BikaResourcesHelper.GetString(BikaResourceKey.BikaTag), "#000000", "#ef97b9");

    public override PluginMetaData MetaData { get; } = typeof(BikaPlugin).GetPluginMetaData();
    public  static readonly PluginMetaData Meta = typeof(BikaPlugin).GetPluginMetaData();
    private ILogger Logger { get; }
    private IPicaClient BikaClient { get; }
    private PicViewService PicViewService { get; }

    public BikaPlugin(ICallableService callableService, ISqlSugarClient sqlSugarClient,
        CompressService compressService, IPluginService pluginService, ILogger logger,PicViewService picViewService) :
        base(callableService, sqlSugarClient, compressService, pluginService)
    {
        PicViewService = picViewService;
        Logger = logger;
        BikaClient = new PicaClient();
        DiFactory.Services.RegisterInstance<IPicaClient>(BikaClient);
        DiFactory.Services.Register<BikaSettingsViewModel>(Reuse.Singleton);
        DiFactory.Services.Register<ClassificationViewModel>(Reuse.Singleton);
        DiFactory.Services.Register<BikaInfoViewModel>(Reuse.Transient);
        DiFactory.Services.Register<BikaCategoryViewModel>(Reuse.Transient);
        DiFactory.Services.Register<LoginTipViewModel>(Reuse.Transient);
        BikaConfig.Init();
    }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override IEnumerable<ResourceDictionary> ResourceDictionaries => new List<ResourceDictionary>
    {
        new ResourceDictionary() { Source ="/Themes/BikaTheme.xaml".AssetUri<BikaPlugin>() }
    };
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override void Loaded(bool isEnabled)
    {
        base.Loaded(isEnabled);
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override void PluginEnabled()
    {
        PicViewService.PicturesLoadStartingEvent += CallerOnPicturesLoadStartingEvent;
        PicViewService.CurrentEpisodeIndexChangedEvent += CallerOnCurrentEpisodeIndexChangedEvent;
        Db.CodeFirst.InitTables<BikaUser>();
        CheckLock();
        CheckToken();
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override void PluginDisabled()
    {
        PicViewService.PicturesLoadStartingEvent -= CallerOnPicturesLoadStartingEvent;
        PicViewService.CurrentEpisodeIndexChangedEvent -= CallerOnCurrentEpisodeIndexChangedEvent;
        // Close Login Frame
        if (MainLoginTip != null) MainLoginTip.Hide();
    }

    /// <summary>
    /// »° ±ä»¯
    /// </summary>
    private async void CallerOnCurrentEpisodeIndexChangedEvent(object sender, CurrentEpisodeIndexChangedEventArg e)
    {
        if (sender is not PicViewModel viewModel) return;
        if (e.OldValue == e.NewValue) return;
        if (viewModel.Affiliation != MetaData.Id) return;
        viewModel.Images.Clear();
        var index = 0;
        if (viewModel.Episodes.Count > 0 && viewModel.Episodes[e.NewValue] is BikaEpisode episode)
        {
            var pages = 1;
            var page = 1;
            await BikaHttpHelper.TryRequest(this, BikaClient.Pictures(episode.ComicId, episode.Order, page), res =>
            {
                pages = res.Data.Pages.Pages;
                foreach (var item in res.Data.Pages.Docs)
                {
                    viewModel.Images.Add(new BikaPicture(++index, item.Media.FilePath));
                }
            });
            for (var i = 2; i <= pages; i++)
            {
                await BikaHttpHelper.TryRequest(this, BikaClient.Pictures(episode.ComicId, episode.Order, page), res =>
                {
                    foreach (var item in res.Data.Pages.Docs)
                    {
                        viewModel.Images.Add(new BikaPicture(++index, item.Media.FilePath));
                    }
                });
            }
        }
    }

    /// <summary>
    /// ³õÊ¼»¯ Âþ»­ Í¼Æ¬
    /// </summary>
    private void CallerOnPicturesLoadStartingEvent(object sender, PicViewArg e)
    {
        if (sender is not PicViewModel viewModel) return;
        if (e.Affiliation != MetaData.Id || e.Parameter is not ComicArg arg) return;
        var orders = new List<int>();
        foreach (var episode in arg.Episodes.Reverse())
        {
            orders.Add(episode.Order);
            viewModel.Episodes.Add(
                new BikaEpisode(episode,arg.ComicInfo.Id));
        }

        if (viewModel.CurrentEpisodeIndex == -1 && orders.Count > 0)
            viewModel.CurrentEpisodeIndex = orders.IndexOf(arg.CurrentEpisode);
    }

    /// <summary>
    /// Auto Login
    /// </summary>
    private bool TryAutoLogin()
    {
        var user = BikaConfig.LastBikaUser;
        if (Db.Queryable<BikaUser>().First(x => x.Email == user) is { } bikaUser)
        {
            BikaClient.SetToken(bikaUser.Token);
            NotificationHelper.Notify(this,
                $"[{MetaData.Name}]{BikaResourcesHelper.GetString(BikaResourceKey.AutoLoginSuccess)}",
                InfoBarSeverity.Success);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Show Login Frame On Main Window
    /// </summary>
    private void ShowLoginFrame()
    {
        MainLoginTip = new LoginTip();
        Caller.TopGrid(this, MainLoginTip, TopGridMode.Dialog);
        MainLoginTip.Show();
    }

    /// <summary>
    /// Check Token
    /// </summary>
    private async void CheckToken()
    {
        // if no token then load token
        if (BikaClient.HasToken) return;
        var b = false;
        if (BikaConfig.AutoLogin) b = TryAutoLogin();
        if (!b)
        {
            ShowLoginFrame();
        }
        else
        {
            await BikaHttpHelper.Profile(this);
            await BikaHttpHelper.PunchIn(this);
            await BikaHttpHelper.Keywords(this);
        }
    }

    /// <summary>
    /// Check Locks
    /// </summary>
    private static void CheckLock()
    {
        // if no locks then load locks
        if (BikaData.Current.Locks.Count != 0) return;
        foreach (var item in BikaData.Categories)
            if (ConfigHelper.Contains(item))
                BikaData.Current.Locks.Add(new BikaLock(item, ConfigHelper.GetBoolean(item)));
            else
                ConfigHelper.Set(item, true);
    }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override IEnumerable<IShadowSearchItem> SearchTextChanged(AutoSuggestBox sender,
        AutoSuggestBoxTextChangedEventArgs args)
    {
        var res = new List<IShadowSearchItem>();
        if (!string.IsNullOrEmpty(sender.Text) && BikaClient.HasToken )
        {
            res.Add(new BikaSearchItem(sender.Text,MetaData.Id,BikaSearchMode.Search));
        }
        return res;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override void SearchSuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {
        
    }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override void SearchQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        BikaSearchItem item = null;
        if (args.ChosenSuggestion is BikaSearchItem item1)
        {
            item = item1;
        }
        else if(args.ChosenSuggestion == null && sender.Items[0] is BikaSearchItem item2)
        {
            item = item2;
        }
        if (item != null)
        {
            Caller.NavigateTo(typeof(BikaCategoryPage),
                new CategoryArg
                {
                    Category = item.Title,
                    Mode = CategoryMode.Search
                },true);
        }
    }
    
}