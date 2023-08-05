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
using Microsoft.Extensions.DependencyInjection;
using PicaComic;
using ShadowViewer.Enums;
using ShadowViewer.Interfaces;
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

        public void Open()
        {
            Login.IsOpen = true;
        }
        /// <summary>
        /// 登录
        /// </summary>
        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            var caller = DiFactory.Current.Services.GetService<ICallableToolKit>();
            if (string.IsNullOrEmpty(Email.Text) || string.IsNullOrEmpty(Password.Password))
            {
                caller.TopGrid(this, ContentDialogHelper.CreateHttpDialog(BikaHttpStatus.Unknown,
                    BikaResourcesHelper.GetString(BikaResourceKey.BlankLogin)), TopGridMode.ContentDialog);
                return;
            }

            try
            {
                var res = await PicaClient.SignIn(Email.Text, Password.Password);
                if (res.Code != 200)
                {
                    if (res.Code == 401)
                    {
                        caller.TopGrid(this,
                            ContentDialogHelper.CreateHttpDialog(BikaHttpStatus.NoAuth, null),
                            TopGridMode.ContentDialog);
                    }
                    else
                    {
                        caller.TopGrid(this,
                            ContentDialogHelper.CreateHttpDialog(BikaHttpStatus.Unknown, res.Message),
                            TopGridMode.ContentDialog);
                    }
                }
                else
                {
                    var db = DiFactory.Current.Services.GetService<ISqlSugarClient>();
                    db.Storageable(new BikaUser()
                    {
                        Email = Email.Text,
                        Password = Password.Password,
                        Token = res.Data.Token,
                    }).ExecuteCommand();
                    Login.IsOpen = false;
                    BikaSettingsHelper.Set(BikaSettingName.LastBikaUser,Email.Text);
                }
            }
            catch (TaskCanceledException)
            {
                caller.TopGrid(this,
                    ContentDialogHelper.CreateHttpDialog(BikaHttpStatus.TimeOut, null),
                    TopGridMode.ContentDialog);
            }
            catch (Exception exception)
            {
                caller.TopGrid(this,
                    ContentDialogHelper.CreateHttpDialog(BikaHttpStatus.Unknown, exception.Message),
                    TopGridMode.ContentDialog);
            }
        }
        /// <summary>
        /// 记住我复选框
        /// </summary>
        private void RememberMe_OnChecked(object sender, RoutedEventArgs e)
        {
            BikaSettingsHelper.Set(BikaSettingName.RememberMe, RememberMeBox.IsChecked ?? false);
        }
        /// <summary>
        /// 自动登录复选框
        /// </summary>
        private void AutoLogin_OnChecked(object sender, RoutedEventArgs e)
        {
            if (AutoLoginBox.IsChecked ?? false) RememberMeBox.IsChecked = true;
            BikaSettingsHelper.Set(BikaSettingName.AutoLogin, AutoLoginBox.IsChecked?? false);
        }
        /// <summary>
        /// 初始化
        /// </summary>
        private void FrameworkElement_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (BikaSettingsHelper.Contains(BikaSettingName.RememberMe))
            {
                RememberMeBox.IsChecked = BikaSettingsHelper.GetBoolean(BikaSettingName.RememberMe);
            }
            if (BikaSettingsHelper.Contains(BikaSettingName.AutoLogin))
            {
                AutoLoginBox.IsChecked = BikaSettingsHelper.GetBoolean(BikaSettingName.AutoLogin);
            }
            if (BikaSettingsHelper.Contains(BikaSettingName.LastBikaUser))
            {
                var user = BikaSettingsHelper.GetString(BikaSettingName.LastBikaUser);
                var db = DiFactory.Current.Services.GetService<ISqlSugarClient>();
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