using System;
using System.Collections.Generic;
using Microsoft.UI.Xaml.Controls;
using PicaComic;
using ShadowViewer.Plugin.Bika.Models;
using ShadowViewer.Plugin.Bika.Controls;
using ShadowViewer.Plugin.Bika.Helpers;
using ShadowViewer.Plugin.Bika.ViewModels;
using System.Threading.Tasks;
using DryIoc;
using ShadowPluginLoader.Attributes;
using ShadowPluginLoader.WinUI;
using ShadowViewer.Core.Helpers;
using ShadowViewer.Core.Plugins;
using ShadowViewer.Plugin.Bika.I18n;

namespace ShadowViewer.Plugin.Bika;

[MainPlugin]
[CheckAutowired]
public partial class BikaPlugin : AShadowViewerPlugin
{
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
        MainLoginTip ??= new LoginTip();
        Db.CodeFirst.InitTables<BikaUser>();
        Caller.AppLoadedEvent += (sender, args) =>
        {
            Logger.Information("触发CreateTopLevelControl");
            Caller.CreateTopLevelControl(MainLoginTip!);
            if (PluginService.IsEnabled(Meta.Id) == true) Enabled();
        };
    }


    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override IEnumerable<string> ResourceDictionaries => new List<string>()
    {
        "ms-plugin://ShadowViewer.Plugin.Bika/Themes/BikaTheme.xaml"
    };


    public override string DisplayName => "哔咔漫画";


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
        // Enabled();
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
        var user = BikaPlugin.Settings.LastBikaUser;
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
        if (Settings.AutoLogin) b = await TryAutoLogin();
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
        foreach (var item in BikaData.Categories)
            if (ConfigHelper.Contains(item))
                BikaData.Current.Locks.Add(new BikaLock(item, ConfigHelper.GetBoolean(item)));
            else
                ConfigHelper.Set(item, true);
    }
}