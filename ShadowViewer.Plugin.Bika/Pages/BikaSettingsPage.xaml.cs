using System;
using CustomExtensions.WinUI;
using DryIoc;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using PicaComic;
using ShadowPluginLoader.WinUI;
using ShadowViewer.Sdk.Extensions;
using ShadowViewer.Plugin.Bika.ViewModels;

namespace ShadowViewer.Plugin.Bika.Pages;

public sealed partial class BikaSettingsPage : Page
{
    private BikaSettingsViewModel ViewModel { get; }

    public BikaSettingsPage()
    {
       this.LoadComponent(ref _contentLoaded);
        ViewModel = DiFactory.Services.Resolve<BikaSettingsViewModel>();
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
        if (!string.IsNullOrEmpty(ViewModel.Config.Proxy))
        {
            ProxyBox.Text = ViewModel.Config.Proxy;
            ViewModel.SetProxy(ViewModel.Config.Proxy);
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
        ViewModel.ResetProxy();
        ProxyBox.Text = "";
    }

    private void LockButton_Click(object sender, RoutedEventArgs e)
    {
        LockTip.Show();
    }

    private void AboutCard_Click(object sender, RoutedEventArgs e)
    {
        Frame.Navigate(typeof(AboutPage), null,
            new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
    }
}