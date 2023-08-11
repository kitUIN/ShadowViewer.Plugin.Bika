using CustomExtensions.WinUI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using CommunityToolkit.WinUI;
using DryIoc;
using Microsoft.Extensions.DependencyInjection;
using PicaComic;
using PicaComic.Responses;
using ShadowViewer.Controls;
using ShadowViewer.Enums;
using ShadowViewer.Interfaces;
using ShadowViewer.Plugin.Bika.Enums;
using ShadowViewer.Plugin.Bika.Helpers;
using ShadowViewer.Plugin.Bika.Models;
using SqlSugar;

namespace ShadowViewer.Plugin.Bika.Controls
{
    public sealed partial class LoginTip : UserControl
    {
        public LoginTip()
        {
            this.LoadComponent(ref _contentLoaded);
        }

        public void Show()
        {
            Login.IsOpen = true;
        }
        public void Hide()
        {
            Login.IsOpen = false;
        }

        /// <summary>
        /// 登录
        /// </summary>
        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            var caller = DiFactory.Services.Resolve<ICallableService>();
            if (string.IsNullOrEmpty(Email.Text) || string.IsNullOrEmpty(Password.Password))
            {
                caller.TopGrid(this, ContentDialogHelper.CreateHttpDialog(BikaHttpStatus.Unknown,
                    BikaResourcesHelper.GetString(BikaResourceKey.BlankLogin)), TopGridMode.ContentDialog);
                return;
            }

            await BikaHttpHelper.TryRequest(this, PicaClient.SignIn(Email.Text, Password.Password),
                res =>
                {
                    DispatcherQueue.TryEnqueue(() =>
                        {
                            var db = DiFactory.Services.Resolve<ISqlSugarClient>();
                            db.Storageable(new BikaUser()
                            {
                                Email = Email.Text,
                                Password = Password.Password,
                                Token = res.Data.Token,
                            }).ExecuteCommand();
                            Login.IsOpen = false;
                            BikaConfig.LastBikaUser = Email.Text;
                        }
                    );
                });
            await BikaHttpHelper.TryRequest(this, PicaClient.Profile(), res =>
            {
                BikaData.Current.CurrentUser = res.Data.User;
                caller.TopGrid(this, new TipPopup(
                    $"[{BikaPlugin.Meta.Name}]{BikaResourcesHelper.GetString(BikaResourceKey.LoginSuccess)}:{BikaData.Current.CurrentUser.Name}",
                    InfoBarSeverity.Success), TopGridMode.Tip);
            });
            await BikaHttpHelper.PunchIn(this);
            await BikaHttpHelper.Keywords();
        }

        /// <summary>
        /// 记住我复选框
        /// </summary>
        private void RememberMe_OnChecked(object sender, RoutedEventArgs e)
        {
            BikaConfig.RememberMe = RememberMeBox.IsChecked ?? false;
        }

        /// <summary>
        /// 自动登录复选框
        /// </summary>
        private void AutoLogin_OnChecked(object sender, RoutedEventArgs e)
        {
            if (AutoLoginBox.IsChecked ?? false) RememberMeBox.IsChecked = true;
            BikaConfig.AutoLogin = AutoLoginBox.IsChecked ?? false;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void FrameworkElement_OnLoaded(object sender, RoutedEventArgs e)
        {
            RememberMeBox.IsChecked = BikaConfig.RememberMe;
            AutoLoginBox.IsChecked = BikaConfig.AutoLogin;
            if (BikaConfigHelper.Contains(BikaConfigKey.LastBikaUser))
            {
                var user = BikaConfig.LastBikaUser;
                var db = DiFactory.Services.Resolve<ISqlSugarClient>();
                if (db.Queryable<BikaUser>().First(x => x.Email == user) is BikaUser bikaUser)
                {
                    if (RememberMeBox.IsChecked ?? false)
                    {
                        Email.Text = bikaUser.Email;
                        Password.Password = bikaUser.Password;
                    }
                }
            }
        }

        /// <summary>
        /// 密码栏回车
        /// </summary>
        private void Password_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                Login_Click(null, null);
            }
        }
    }
}