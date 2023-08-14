using CustomExtensions.WinUI;
using Microsoft.UI.Xaml.Controls;

namespace ShadowViewer.Plugin.Bika.Pages;

public sealed partial class AboutPage : Page
{
    public AboutPage()
    {
        this.LoadComponent(ref _contentLoaded);
    }
}