using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.Storage;
using DryIoc;
using Microsoft.UI.Xaml.Controls;
using ShadowViewer.Plugin.Bika.Enums;
using PicaComic;
using Serilog;
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

namespace ShadowViewer.Plugin.Bika;

public class BikaPlugin : PluginBase
{
    private static readonly ILogger Logger = Log.ForContext<BikaPlugin>();

    /// <summary>
    /// µÇÂ¼´°Ìå
    /// </summary>
    public static LoginTip MainLoginTip = new();

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override PluginMetaData MetaData { get; }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override LocalTag AffiliationTag { get; } =
        new(BikaResourcesHelper.GetString(BikaResourceKey.Tag), "#000000", "#ef97b9");

    public static readonly PluginMetaData Meta = new(
        "Bika",
        "ßÙßÇÂþ»­",
        "ßÙßÇÂþ»­ÊÊÅäÆ÷",
        "kitUIN", "0.1.0",
        new Uri("https://github.com/kitUIN/ShadowViewer.Plugin.Bika/"),
        new Uri("ms-appx:///ShadowViewer.Plugin.Bika/Assets/Icons/logo.png"),
        20230808);
    public IPicaClient BikaClient { get; }
    public BikaPlugin(ICallableService callableService, ISqlSugarClient sqlSugarClient,
        CompressService compressService, IPluginService pluginService) :
        base(callableService, sqlSugarClient, compressService, pluginService)
    {
        DiFactory.Services.Register<IPicaClient, PicaClient>(Reuse.Singleton);
        BikaClient = DiFactory.Services.Resolve<IPicaClient>();
        DiFactory.Services.Register<BikaSettingsViewModel>(reuse: Reuse.Singleton);
        DiFactory.Services.Register<ClassificationViewModel>(reuse: Reuse.Singleton);
        DiFactory.Services.Register<BikaInfoViewModel>(reuse: Reuse.Transient);
        DiFactory.Services.Register<BikaCategoryViewModel>(reuse: Reuse.Transient);
        MetaData = Meta;
    }

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
    public override Type SettingsPage => typeof(BikaSettingsPage);

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override IList<ShadowNavigationItem> NavigationViewMenuItems => new List<ShadowNavigationItem>()
    {
        new()
        {
            Content = BikaResourcesHelper.GetString(BikaResourceKey.Title),
            Icon = XamlHelper.CreateImageIcon(MetaData.Logo),
            Id = MetaData.Id
        }
    };

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override IList<ShadowNavigationItem> NavigationViewFooterItems => new List<ShadowNavigationItem>();

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override void NavigationViewItemInvokedHandler(object tag, ref Type page, ref object parameter)
    {
        if (BikaClient.HasToken)
        {
            page = typeof(ClassificationPage);
            parameter = null;
        }
        else
        {
            MainLoginTip = new LoginTip();
            Caller.TopGrid(this, MainLoginTip, TopGridMode.Dialog);
            MainLoginTip.Show();
        }
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
        // ¹Ø±ÕµÇÂ¼´°Ìå
        MainLoginTip.Hide();
    }

    /// <summary>
    /// ×Ô¶¯µÇÂ¼
    /// </summary>
    private bool TryAutoLogin()
    {
        var user = BikaConfig.LastBikaUser;
        if (Db.Queryable<BikaUser>().First(x => x.Email == user) is { } bikaUser)
        {
            BikaClient.SetToken(bikaUser.Token);
            Caller.TopGrid(this, new TipPopup(
                $"[{Meta.Name}]{BikaResourcesHelper.GetString(BikaResourceKey.AutoLoginSuccess)}",
                InfoBarSeverity.Success), TopGridMode.Tip);
            return true;
        }

        return false;
    }

    /// <summary>
    /// ¼ì²éµÇÂ¼Æ¾Ö¤
    /// </summary>
    private async void CheckToken()
    {
        // Î´¼ÓÔØµÇÂ¼Æ¾Ö¤Ôò¼ÓÔØ
        if (!BikaClient.HasToken)
        {
            var b = false;
            if (BikaConfig.AutoLogin) b = TryAutoLogin();

            if (!b)
            {
                MainLoginTip = new LoginTip();
                Caller.TopGrid(this, MainLoginTip, TopGridMode.Dialog);
                MainLoginTip.Show();
            }
            else
            {
                await BikaHttpHelper.Profile(this);
                await BikaHttpHelper.PunchIn(this);
                await BikaHttpHelper.Keywords();
            }
        }
    }

    /// <summary>
    /// ¼ì²é·âÓ¡
    /// </summary>
    private void CheckLock()
    {
        // Î´¼ÓÔØ·âÓ¡Ôò¼ÓÔØ
        if (BikaData.Current.Locks.Count == 0)
            foreach (var item in BikaData.Categories)
                if (ConfigHelper.Contains(item))
                    BikaData.Current.Locks.Add(
                        new BikaLock(item,
                            ConfigHelper.GetBoolean(item))
                    );
                else
                    ConfigHelper.Set(item, true);
    }
}