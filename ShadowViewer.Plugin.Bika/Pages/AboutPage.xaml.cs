using CustomExtensions.WinUI;
using DryIoc;
using Microsoft.UI.Xaml.Controls;
using ShadowPluginLoader.WinUI;
using ShadowViewer.Plugin.Bika.Configs;

namespace ShadowViewer.Plugin.Bika.Pages;

/// <summary>
/// About Page
/// </summary>
public sealed partial class AboutPage : Page
{
    /// <summary>
    /// Config
    /// </summary>
    public BikaPluginConfig Config { get; } = DiFactory.Services.Resolve<BikaPluginConfig>();

    /// <summary>
    /// 
    /// </summary>
    public AboutPage()
    {
        this.LoadComponent(ref _contentLoaded);
        
    }
}