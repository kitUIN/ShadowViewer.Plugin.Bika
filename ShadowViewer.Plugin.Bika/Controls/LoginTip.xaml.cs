using CustomExtensions.WinUI;
using DryIoc;
using Microsoft.UI.Xaml.Controls;
using ShadowViewer.Plugin.Bika.ViewModels;
using ShadowPluginLoader.WinUI;

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
}