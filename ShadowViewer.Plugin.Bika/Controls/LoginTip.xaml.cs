using CustomExtensions.WinUI;
using DryIoc;
using Microsoft.UI.Xaml.Controls;
using ShadowViewer.Plugin.Bika.ViewModels;
using ShadowPluginLoader.WinUI;

namespace ShadowViewer.Plugin.Bika.Controls;

/// <summary>
/// 
/// </summary>
public sealed partial class LoginTip : UserControl
{
    /// <summary>
    /// 
    /// </summary>
    private LoginTipViewModel ViewModel { get; } = DiFactory.Services.Resolve<LoginTipViewModel>();

    /// <summary>
    /// 
    /// </summary>
    public LoginTip()
    {
        this.LoadComponent(ref _contentLoaded);
    }

    /// <summary>
    /// 
    /// </summary>
    public void Show() => ViewModel.IsOpen = true;

    /// <summary>
    /// 
    /// </summary>
    public void Hide() => ViewModel.IsOpen = false;
}