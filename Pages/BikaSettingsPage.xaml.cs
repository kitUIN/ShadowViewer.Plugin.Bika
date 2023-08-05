using CustomExtensions.WinUI;
using ShadowViewer.Extensions;
using PicaComic;
using Microsoft.UI;
using ShadowViewer.Plugin.Bika.ViewModels;

namespace ShadowViewer.Plugin.Bika.Pages
{

    public sealed partial class BikaSettingsPage : Page
    {
        private BikaSettingsViewModel ViewModel { get; }
        public BikaSettingsPage()
        {
            this.LoadComponent(ref _contentLoaded);
            BikaSettingsViewModel.Current ??= new BikaSettingsViewModel();
            ViewModel = BikaSettingsViewModel.Current;
        }
        private void Uri_Click(object sender, RoutedEventArgs e)
        {
            var source = sender as FrameworkElement;
            if (source == null || source.Tag.ToString() is not { } tag) return;
            var uri = new Uri(tag);
            uri.LaunchUriAsync();
        } 
        private async void SettingsExpander_Loaded(object sender, RoutedEventArgs e)
        {
            if (BikaSettingsHelper.Contains(BikaSettingName.Proxy))
            {
                var uri = BikaSettingsHelper.GetString(BikaSettingName.Proxy);
                ProxyBox.Text = uri;
                PicaClient.SetProxy(new Uri(uri));
            }
            await ViewModel.Ping();
        }

        private async void Ping_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.Ping();
        }

        private void ProxyBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            ViewModel.SetProxy(ProxyBox.Text);
        }

        private void ResetButton_OnClick(object sender, RoutedEventArgs e)
        {
            PicaClient.ResetProxy();
            ProxyBox.Text = "";
        }
    }
}
