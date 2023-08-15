using Windows.System;
using CustomExtensions.WinUI;
using DryIoc;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using PicaComic;
using ShadowViewer.Enums;
using ShadowViewer.Helpers;
using ShadowViewer.Interfaces;
using ShadowViewer.Plugin.Bika.Enums;
using ShadowViewer.Plugin.Bika.Helpers;
using ShadowViewer.Plugin.Bika.Models;
using ShadowViewer.Plugin.Bika.ViewModels;
using SqlSugar;
using Microsoft.UI.Xaml.Media.Imaging;

namespace ShadowViewer.Plugin.Bika.Controls;

public sealed partial class LoginTip : UserControl
{
    private LoginTipViewModel ViewModel { get; }

    public LoginTip()
    {
        this.LoadComponent(ref _contentLoaded);
        ViewModel = DiFactory.Services.Resolve<LoginTipViewModel>();

    }

    public void Show()
    {
        ViewModel.IsOpen = true;
    }

    public void Hide()
    {
        ViewModel.IsOpen = false;
    }
    private string Hi(string tx)
    {
        return tx;
    }
    private void Logo_Loaded(object sender, RoutedEventArgs e)
    {
        
    }
}