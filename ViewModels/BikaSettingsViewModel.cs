using CommunityToolkit.Mvvm.ComponentModel;
using FluntIcon;
using Microsoft.UI;
using PicaComic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private SolidColorBrush pingColor = new SolidColorBrush(Colors.Green);
        [ObservableProperty]
        private FluentIconSymbol pingIcon = FluentIconSymbol.CheckmarkCircleFilled;
        public static BikaSettingsViewModel Current { get; set; }
        public BikaSettingsViewModel() { 
        }
        partial void OnApiShuntChanged(int oldValue, int newValue)
        {
            if(oldValue != newValue)
            {
                PicaClient.AppChannel = newValue + 1;
                ConfigHelper.Set(BikaSettingName.ApiShunt.ToString(), PicaClient.AppChannel);
            }
        }
        partial void OnPicShuntChanged(int oldValue, int newValue)
        {
            if (oldValue != newValue)
            {
                PicaClient.FileChannel = newValue + 1;
                ConfigHelper.Set(BikaSettingName.PicShunt.ToString(), PicaClient.FileChannel);
            }
        }
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
    }
}
