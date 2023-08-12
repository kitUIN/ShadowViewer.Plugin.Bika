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
using ShadowViewer.Extensions;

namespace ShadowViewer.Plugin.Bika;
[PluginMetaData( "Bika",
    "ßÙßÇÂþ»­",
    "ßÙßÇÂþ»­ÊÊÅäÆ÷",
    "kitUIN", "0.1.0",
    "https://github.com/kitUIN/ShadowViewer.Plugin.Bika/",
    "ms-appx:///ShadowViewer.Plugin.Bika/Assets/Icons/logo.png",
    20230808)]
public class BikaPlugin : PluginBase
{
    /// <summary>
    /// Login Frame
    /// </summary>
    public static LoginTip MainLoginTip { get; set; }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override LocalTag AffiliationTag { get; } =
        new(BikaResourcesHelper.GetString(BikaResourceKey.Tag), "#000000", "#ef97b9");

    public new static readonly PluginMetaData MetaData = typeof(BikaPlugin).GetPluginMetaData();
    private ILogger Logger { get; } 
    private IPicaClient BikaClient { get; }
    public BikaPlugin(ICallableService callableService, ISqlSugarClient sqlSugarClient,
        CompressService compressService, IPluginService pluginService, ILogger logger) :
        base(callableService, sqlSugarClient, compressService, pluginService)
    {
        Logger = logger;
        BikaClient = new PicaClient();
        DiFactory.Services.RegisterInstance<IPicaClient>(BikaClient);
        DiFactory.Services.Register<BikaSettingsViewModel>(reuse: Reuse.Singleton);
        DiFactory.Services.Register<ClassificationViewModel>(reuse: Reuse.Singleton);
        DiFactory.Services.Register<BikaInfoViewModel>(reuse: Reuse.Transient);
        DiFactory.Services.Register<BikaCategoryViewModel>(reuse: Reuse.Transient);
        DiFactory.Services.Register<LoginTipViewModel>(reuse: Reuse.Transient);
        BikaConfig.Init();
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
            ShowLoginFrame();
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
        // Close Login Frame
        MainLoginTip.Hide();
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
                BikaData.Current.Locks.Add( new BikaLock(item, ConfigHelper.GetBoolean(item)));
            else
                ConfigHelper.Set(item, true);
    }
}