using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DryIoc;
using Microsoft.UI.Xaml.Input;
using PicaComic;
using Serilog;
using ShadowPluginLoader.Attributes;
using ShadowPluginLoader.WinUI;
using ShadowViewer.Plugin.Bika.Args;
using ShadowViewer.Plugin.Bika.Configs;
using ShadowViewer.Plugin.Bika.Helpers;
using ShadowViewer.Plugin.Bika.Models;
using SqlSugar;
using System.Threading.Tasks;
using Windows.System;

namespace ShadowViewer.Plugin.Bika.ViewModels;

/// <summary>
/// 
/// </summary>
[CheckAutowired]
public partial class LoginTipViewModel : ObservableObject
{
    /// <summary>
    /// 登录成功事件
    /// </summary>
    public static event EventHandler<SignInEventArgs>? SignInEvent;

    /// <summary>
    /// Config
    /// </summary>
    [Autowired]
    public BikaPluginConfig Config { get; }
    [Autowired]
    private ILogger Logger { get; }
    [Autowired]
    private IPicaClient Client { get; }
    [Autowired]
    private ISqlSugarClient Db { get; }

    /// <summary>
    /// 
    /// </summary>
    [ObservableProperty] private bool isOpen;
    /// <summary>
    /// 
    /// </summary>
    [ObservableProperty] private bool canLogin = true;
    /// <summary>
    /// 
    /// </summary>
    [ObservableProperty] private string email = "";
    /// <summary>
    /// 
    /// </summary>
    [ObservableProperty] private string password = "";

    /// <summary>
    /// 密码栏回车
    /// </summary>
    public void Password_OnKeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == VirtualKey.Enter) LoginCommand.Execute(null);
    }

     

    /// <summary>
    /// 登录
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanLogin))]
    private async Task Login()
    {
        CanLogin = false;
        if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
        {
            // NotificationHelper.Dialog(this, ContentDialogHelper.CreateHttpDialog(BikaHttpStatus.Unknown,
            //     ResourcesHelper.GetString(ResourceKey.BlankLogin)));
            CanLogin = true;
            return;
        }

        await BikaHttpHelper.TryRequest(this, Client.SignIn(Email, Password),
            async res =>
            {
                var token = res.Data.Token;
                var db = DiFactory.Services.Resolve<ISqlSugarClient>();
                await db.Storageable(new BikaUser()
                {
                    Email = Email,
                    Password = Password,
                    Token = token
                }).ExecuteCommandAsync();
                IsOpen = false;
                Config.LastBikaUser = Email;
                SignInEvent?.Invoke(this, new SignInEventArgs(Email, token));
            });
        CanLogin = true;
    }

    partial void ConstructorInit()
    {
        SignInEvent += async (_, _) =>
        {
            await BikaHttpHelper.TryRequest(this, Client.Profile(), res =>
            {
                BikaData.Current.CurrentUser = res.Data.User;
                // 触发登录成功事件
                //NotificationHelper.Notify(this,
                //    $"[{BikaPlugin.Meta.Name}]{ResourcesHelper.GetString(ResourceKey.LoginSuccess)}:{BikaData.Current.CurrentUser.Name}",
                //    InfoBarSeverity.Success);
            });
            await BikaHttpHelper.PunchIn(this);
            await BikaHttpHelper.Keywords(this);
        };
        if (string.IsNullOrEmpty(Config.LastBikaUser)) return;
        var user = Config.LastBikaUser;
        if (Db.Queryable<BikaUser>().First(x => x.Email == user) is not { } bikaUser) return;
        if (!Config.RememberMe) return;
        Email = bikaUser.Email;
        Password = bikaUser.Password;
        Logger.Information("自动加载上次登录用户: {User}", user);
    }
}