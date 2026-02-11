using DryIoc;
using Microsoft.UI.Xaml.Controls;
using PicaComic;
using ShadowPluginLoader.Attributes;
using ShadowPluginLoader.WinUI;
using ShadowViewer.Plugin.Bika.Configs;
using ShadowViewer.Plugin.Bika.Controls;
using ShadowViewer.Plugin.Bika.Helpers;
using ShadowViewer.Plugin.Bika.I18n;
using ShadowViewer.Plugin.Bika.Models;
using ShadowViewer.Plugin.Bika.ViewModels;
using ShadowViewer.Sdk.Plugins;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShadowViewer.Sdk.Helpers;

namespace ShadowViewer.Plugin.Bika;

/// <summary>
/// 
/// </summary>
[MainPlugin]
[CheckAutowired]
public partial class BikaPlugin : AShadowViewerPlugin
{
    /// <summary>
    /// Config
    /// </summary>
    public BikaPluginConfig Config { get; } = DiFactory.Services.Resolve<BikaPluginConfig>();

    /// <summary>
    /// Login Frame
    /// </summary>
    public static LoginTip? MainLoginTip { get; set; }

    /// <summary>
    /// 
    /// </summary>
    partial void ConstructorInit()
    {
        DiFactory.Services.Register<IPicaClient, PicaClient>(Reuse.Singleton);
        DiFactory.Services.Register<BikaSettingsViewModel>(Reuse.Singleton);
        DiFactory.Services.Register<ClassificationViewModel>(Reuse.Singleton);
        DiFactory.Services.Register<BikaInfoViewModel>(Reuse.Transient);
        DiFactory.Services.Register<BikaCategoryViewModel>(Reuse.Transient);
        DiFactory.Services.Register<LoginTipViewModel>(Reuse.Transient);
        Db.CodeFirst.InitTables<BikaUser>();
        IPicaClient.AppChannel = Config.ApiShunt;
        IPicaClient.FileChannel = Config.PicShunt;
        if (!string.IsNullOrEmpty(Config.Proxy))
            DiFactory.Services.Resolve<IPicaClient>().SetProxy(new Uri(Config.Proxy));
        VersionHelper.Init(MetaData);
        Caller.AppLoadedEvent += (_, _) =>
        {
            if (PluginService.IsEnabled(MetaData.Id) == true) Enabled();
        };
    }

    private void PluginEventServiceOnPluginLoaded()
    {
        if (MainLoginTip != null) return;
        WindowHelper.GetDispatcherQueue().TryEnqueue(() =>
        {
            MainLoginTip = new LoginTip();
            Caller.CreateTopLevelControl(MainLoginTip);
            Logger.Information("触发CreateTopLevelControl");
            if (PluginService.IsEnabled(MetaData.Id) == true) Enabled();
        });
    }


    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override IEnumerable<string> ResourceDictionaries { get; } =
        ["ms-plugin://ShadowViewer.Plugin.Bika/Themes/BikaTheme.xaml"];


    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override string DisplayName => I18N.Title;


    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override void Enabled()
    {
        CheckLock();
        CheckToken();
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override void Loaded()
    {
        PluginEventServiceOnPluginLoaded();
        Enabled();
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override void Disabled()
    {
        // Close Login Frame
        MainLoginTip?.Hide();
    }

    /// <summary>
    /// Auto Login
    /// </summary>
    private async Task<bool> TryAutoLogin()
    {
        var bikaClient = DiFactory.Services.Resolve<IPicaClient>();
        var user = Config.LastBikaUser;
        if (await Db.Queryable<BikaUser>().FirstAsync(x => x.Email == user) is not { } bikaUser) return false;
        bikaClient.SetToken(bikaUser.Token);
        try
        {
            var res = await bikaClient.Profile();
            BikaData.Current.CurrentUser = res.Data.User;
        }
        catch (Exception)
        {
            Notifier.NotifyTip(this,
                $"[{MetaData.Name}]{I18N.AutoLoginFail}",
                InfoBarSeverity.Error);
            return false;
        }

        Notifier.NotifyTip(this,
            $"[{MetaData.Name}]{I18N.AutoLoginSuccess}",
            InfoBarSeverity.Success);
        return true;
    }

    /// <summary>
    /// Show Login Frame On Main Window
    /// 展示登录窗口
    /// </summary>
    public static void ShowLoginFrame() => MainLoginTip?.Show();

    /// <summary>
    /// Check Token
    /// 检查用户是否在线(token)
    /// </summary>
    private async void CheckToken()
    {
        // if no token then load token
        var bikaClient = DiFactory.Services.Resolve<IPicaClient>();
        if (bikaClient.HasToken) return;
        var b = false;
        if (Config.AutoLogin) b = await TryAutoLogin();
        if (!b) ShowLoginFrame();
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
        var lockConfig = DiFactory.Services.Resolve<BikaPluginLockConfig>();
        foreach (var item in BikaData.Categories)
            if (lockConfig.Locks.TryGetValue(item, out var configLock))
                BikaData.Current.Locks.Add(new BikaLock(item, configLock));
            else
                lockConfig.Locks[item] = true;
        lockConfig.Save();
    }
}