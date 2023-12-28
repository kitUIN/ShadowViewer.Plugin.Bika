using System.Threading.Tasks;
using Windows.System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DryIoc;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using PicaComic;
using Serilog;
using ShadowViewer.Helpers;
using ShadowViewer.Plugin.Bika.Enums;
using ShadowViewer.Plugin.Bika.Helpers;
using ShadowViewer.Plugin.Bika.Models;
using SqlSugar;

namespace ShadowViewer.Plugin.Bika.ViewModels;

public partial class LoginTipViewModel : ObservableObject
{
    private ILogger Logger { get; }
    private IPicaClient Client { get; }
    private ISqlSugarClient Db { get; }

    public LoginTipViewModel(ILogger logger, IPicaClient picaClient,ISqlSugarClient db)
    {
        Logger = logger;
        Client = picaClient;
        Db = db;
        if (BikaConfigHelper.Contains(BikaConfigKey.LastBikaUser))
        {
            var user = BikaConfig.LastBikaUser;
            if (Db.Queryable<BikaUser>().First(x => x.Email == user) is BikaUser bikaUser)
                if (RememberMe)
                {
                    Email= bikaUser.Email;
                    Password= bikaUser.Password;
                    Logger.Information("自动加载上次登录用户");
                }
        }
    }

    [ObservableProperty] private bool rememberMe = BikaConfig.RememberMe;
    [ObservableProperty] private bool autoLogin = BikaConfig.AutoLogin;
    [ObservableProperty] private bool isOpen;
    [ObservableProperty] private bool canLogin = true;
    [ObservableProperty] private string email = "";
    [ObservableProperty] private string password = "";

    /// <summary>
    /// 密码栏回车
    /// </summary>
    public void Password_OnKeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == VirtualKey.Enter) LoginCommand.Execute(null);
    }
    partial void OnAutoLoginChanged(bool oldValue, bool newValue)
    {
        if (oldValue != newValue)
        {
            BikaConfig.AutoLogin = AutoLogin;
            if (AutoLogin)
                RememberMe = true;
        }
    }
    partial void OnRememberMeChanged(bool oldValue, bool newValue)
    {
        if (oldValue != newValue)
            BikaConfig.RememberMe = RememberMe;
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
            NotificationHelper.Dialog(this, ContentDialogHelper.CreateHttpDialog(BikaHttpStatus.Unknown,
                BikaResourcesHelper.GetString(BikaResourceKey.BlankLogin)));
            CanLogin = true;
            return;
        }

        await BikaHttpHelper.TryRequest(this, Client.SignIn(Email, Password),
            async res =>
            {
                var db = DiFactory.Services.Resolve<ISqlSugarClient>();
                db.Storageable(new BikaUser()
                {
                    Email = Email,
                    Password = Password,
                    Token = res.Data.Token
                }).ExecuteCommand();
                IsOpen = false;
                BikaConfig.LastBikaUser = Email;
                await BikaHttpHelper.TryRequest(this, Client.Profile(), res =>
                {
                    BikaData.Current.CurrentUser = res.Data.User;
                    NotificationHelper.Notify(this,
                        $"[{BikaPlugin.Meta.Name}]{BikaResourcesHelper.GetString(BikaResourceKey.LoginSuccess)}:{BikaData.Current.CurrentUser.Name}",
                        InfoBarSeverity.Success);
                });
                await BikaHttpHelper.PunchIn(this);
                await BikaHttpHelper.Keywords(this);
            });
        CanLogin = true;
    }
}