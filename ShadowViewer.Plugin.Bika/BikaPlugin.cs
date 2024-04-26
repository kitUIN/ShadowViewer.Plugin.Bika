using System;
using System.Collections.Generic;
using DryIoc;
using Microsoft.UI.Xaml.Controls;
using ShadowViewer.Plugin.Bika.Enums;
using PicaComic;
using Serilog;
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
using CustomExtensions.WinUI;
using Microsoft.UI.Xaml;
using System.Threading.Tasks;

namespace ShadowViewer.Plugin.Bika;

[AutoPluginMeta]
public partial class BikaPlugin : PluginBase
{
    /// <summary>
    /// Login Frame
    /// </summary>
    public static LoginTip? MainLoginTip { get; set; }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override Type? SettingsPage => typeof(BikaSettingsPage);
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override LocalTag AffiliationTag { get; } =
        new(ResourcesHelper.GetString(ResourceKey.BikaTag), "#000000", "#ef97b9");

    public BikaPlugin(ICallableService callableService, ISqlSugarClient sqlSugarClient, CompressService compressServices, IPluginService pluginService, ILogger logger) : base(callableService, sqlSugarClient, compressServices, pluginService, logger)
    {
        BikaClient = new PicaClient();
        DiFactory.Services.RegisterInstance<IPicaClient>(BikaClient);
        DiFactory.Services.Register<BikaSettingsViewModel>(Reuse.Singleton);
        DiFactory.Services.Register<ClassificationViewModel>(Reuse.Singleton);
        DiFactory.Services.Register<BikaInfoViewModel>(Reuse.Transient);
        DiFactory.Services.Register<BikaCategoryViewModel>(Reuse.Transient);
        DiFactory.Services.Register<LoginTipViewModel>(Reuse.Transient);
        BikaConfig.Init();
    }

    private IPicaClient BikaClient { get; }

    public override void PluginDeleting()
    {
        DiFactory.Services.Unregister<IPicaClient>();
        DiFactory.Services.Unregister<BikaSettingsViewModel>();
        DiFactory.Services.Unregister<ClassificationViewModel>();
        DiFactory.Services.Unregister<BikaInfoViewModel>();
        DiFactory.Services.Unregister<BikaCategoryViewModel>();
        DiFactory.Services.Unregister<LoginTipViewModel>();
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override IEnumerable<ResourceDictionary> ResourceDictionaries => new List<ResourceDictionary>
    {
        new() { Source ="/Themes/BikaTheme.xaml".AssetUri<BikaPlugin>() }
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
        Db.CodeFirst.InitTables<BikaUser>();
        CheckLock();
        CheckToken();
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override void PluginDisabled()
    {
        // Close Login Frame
        MainLoginTip?.Hide();
    }

    /// <summary>
    /// Auto Login
    /// </summary>
    private async Task<bool> TryAutoLogin()
    {
        var user = BikaConfig.LastBikaUser;
        if (Db.Queryable<BikaUser>().First(x => x.Email == user) is { } bikaUser)
        {
            BikaClient.SetToken(bikaUser.Token);
            try
            {
                var res = await BikaClient.Profile();
                BikaData.Current.CurrentUser = res.Data.User;
            }
            catch (Exception)
            {
                NotificationHelper.Notify(this,
                $"[{MetaData.Name}]{ResourcesHelper.GetString(ResourceKey.AutoLoginFail)}",
                InfoBarSeverity.Error);
                return false;
            }
            NotificationHelper.Notify(this,
                $"[{MetaData.Name}]{ResourcesHelper.GetString(ResourceKey.AutoLoginSuccess)}",
                InfoBarSeverity.Success);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Show Login Frame On Main Window
    /// </summary>
    public void ShowLoginFrame()
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
        if (BikaConfig.AutoLogin) b = await TryAutoLogin();
        if (!b)
        {
            ShowLoginFrame();
        }
        else
        {
            
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
        BikaSearchItem? item = null;
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