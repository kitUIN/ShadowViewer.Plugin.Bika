using CommunityToolkit.Mvvm.ComponentModel;
using FluntIcon;
using Microsoft.UI;
using PicaComic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ShadowViewer.Controls;
using ShadowViewer.Enums;
using ShadowViewer.Interfaces;
using ShadowViewer.Extensions;
using System.Drawing;

namespace ShadowViewer.Plugin.Bika.ViewModels
{
    public partial class BikaSettingsViewModel:ObservableObject
    {
        [ObservableProperty]
        private bool pingShow;
        [ObservableProperty]
        private int apiShunt = PicaClient.AppChannel - 1;
        [ObservableProperty]
        private int picShunt = PicaClient.FileChannel - 1;
        [ObservableProperty]
        private string pingText;
        [ObservableProperty]
        private bool canTemporaryUnlock = BikaConfig.CanTemporaryUnlockComic;
        [ObservableProperty]
        private bool loadLockComic = BikaConfig.LoadLockComic;
        [ObservableProperty]
        private bool isIgnoreLockComic = BikaConfig.IsIgnoreLockComic;
        [ObservableProperty]
        private SolidColorBrush pingColor = new SolidColorBrush(Colors.Green);
        [ObservableProperty]
        private FluentIconSymbol pingIcon = FluentIconSymbol.CheckmarkCircleFilled;
        public static BikaSettingsViewModel Current { get; set; } = new BikaSettingsViewModel();
        private ICallableToolKit Caller { get; }
        public BikaSettingsViewModel()
        { 
            Caller = DiFactory.Current.Services.GetService<ICallableToolKit>();
            LoadLockComicShow = !BikaConfig.IsIgnoreLockComic;
            CanTemporaryUnlockShow = !BikaConfig.IsIgnoreLockComic && BikaConfig.LoadLockComic;
        }
        private bool loadLockComicShow  ;
        public bool LoadLockComicShow
        {
            get => loadLockComicShow;
            set => SetProperty(ref loadLockComicShow, value);
        }
        private bool canTemporaryUnlockShow;
        public bool CanTemporaryUnlockShow
        {
            get => canTemporaryUnlockShow;
            set => SetProperty(ref canTemporaryUnlockShow, value);
        }
    
        #region Changed
        partial void OnLoadLockComicChanged(bool oldValue, bool newValue)
        {
            if (oldValue != newValue)
            {
                BikaConfig.LoadLockComic = newValue;
                CanTemporaryUnlockShow = !IsIgnoreLockComic && LoadLockComic;
            }
        }
        partial void OnIsIgnoreLockComicChanged(bool oldValue, bool newValue)
        {
            if (oldValue != newValue)
            {
                BikaConfig.IsIgnoreLockComic = newValue;
                LoadLockComicShow = !IsIgnoreLockComic;
                CanTemporaryUnlockShow = !IsIgnoreLockComic && LoadLockComic;
            }
        }
        partial void OnCanTemporaryUnlockChanged(bool oldValue, bool newValue)
        {
            if (oldValue != newValue)
            {
                BikaConfig.CanTemporaryUnlockComic = newValue;
            }
        }
        partial void OnApiShuntChanged(int oldValue, int newValue)
        {
            if(oldValue != newValue)
            {
                PicaClient.AppChannel = newValue + 1;
                BikaConfigHelper.Set(BikaConfigKey.ApiShunt, PicaClient.AppChannel);
            }
        }
        partial void OnPicShuntChanged(int oldValue, int newValue)
        {
            if (oldValue != newValue)
            {
                PicaClient.FileChannel = newValue + 1;
                BikaConfigHelper.Set(BikaConfigKey.PicShunt, PicaClient.FileChannel);
            }
        }
        #endregion
        public async Task Ping()
        {
            PingShow = true;
            PingText = BikaResourcesHelper.GetString(BikaResourceKey.Pinging);
            PingIcon = FluntIcon.FluentIconSymbol.ArrowSyncCircleFilled;
            PingColor = new SolidColorBrush(Colors.Orange);
            try
            {
                var ms = await PicaClient.PingBaseUrl();
                
                PingText = $"{ms}ms";
                PingIcon = FluntIcon.FluentIconSymbol.CheckmarkCircleFilled;
                PingColor = new SolidColorBrush(Colors.Green);
            }
            catch (Exception)
            { 
                PingText = BikaResourcesHelper.GetString(BikaResourceKey.TimeOut);
                PingIcon = FluntIcon.FluentIconSymbol.DismissCircleFilled;
                PingColor = new SolidColorBrush(Colors.Red);
            }
        }

        public void SetProxy(string text)
        {
            try
            {
                var uri = new Uri(text);
                PicaClient.SetProxy(uri);
                BikaConfig.Proxy = text;
                Caller.TopGrid(this, new TipPopup(
                    $"[{BikaPlugin.Meta.Name}]{BikaResourcesHelper.GetString(BikaResourceKey.Proxy)}({text}){BikaResourcesHelper.GetString(BikaResourceKey.SetSuccess)}",
                    InfoBarSeverity.Success), TopGridMode.Tip);
            }
            catch (Exception)
            {
                Caller.TopGrid(this, new TipPopup(
                    $"[{BikaPlugin.Meta.Name}]{BikaResourcesHelper.GetString(BikaResourceKey.Proxy)}({text}){BikaResourcesHelper.GetString(BikaResourceKey.SetError)}",
                    InfoBarSeverity.Error), TopGridMode.Tip);
            }
        }
    }
}
