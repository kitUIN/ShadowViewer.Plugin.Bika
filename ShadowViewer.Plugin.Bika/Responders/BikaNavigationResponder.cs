using CustomExtensions.WinUI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using PicaComic;
using ShadowPluginLoader.Attributes;
using ShadowViewer.Plugin.Bika.Constants;
using ShadowViewer.Plugin.Bika.I18n;
using ShadowViewer.Plugin.Bika.Pages;
using ShadowViewer.Sdk;
using ShadowViewer.Sdk.Models;
using ShadowViewer.Sdk.Models.Interfaces;
using ShadowViewer.Sdk.Navigation;
using ShadowViewer.Sdk.Plugins;
using ShadowViewer.Sdk.Responders;
using ShadowViewer.Sdk.Utils;
using System;
using System.Collections.Generic;

namespace ShadowViewer.Plugin.Bika.Responders;

[EntryPoint(Name = nameof(PluginResponder.NavigationResponder))]
public partial class BikaNavigationResponder : AbstractNavigationResponder
{
    [Autowired] private IPicaClient Client { get; }
    [Autowired] private PluginLoader PluginService { get; }

    public override IEnumerable<IShadowNavigationItem> NavigationViewMenuItems { get; } =
        new List<IShadowNavigationItem>
        {
            new ShadowNavigationItem(
                pluginId: PluginConstants.PluginId,
                id: "Bika.Classification",
                uri: ShadowUri.Parse("shadow://bika/classification"), 
                icon: new ImageIcon()
                {
                    Source = new BitmapImage(new Uri("ms-plugin://ShadowViewer.Plugin.Bika/Assets/Icons/logo.png"
                        .PluginPath()))
                }, content: I18N.Title
            )
        };


    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override void Register()
    {
        ShadowRouteRegistry.RegisterPage(new ShadowNavigation(typeof(ClassificationPage), SelectItemId: "Bika.Classification"),
            "bika", "classification");
        ShadowRouteRegistry.RegisterPage(new ShadowNavigation(typeof(BikaCategoryPage), SelectItemId: "Bika.Classification"),
            "bika", "category");
        ShadowRouteRegistry.RegisterPage(new ShadowNavigation(typeof(BikaInfoPage), SelectItemId: "Bika.Classification"),
            "bika", "comic");
        ShadowRouteRegistry.RegisterPage(new ShadowNavigation(typeof(BikaSettingsPage), SelectItemId: "Bika.Classification"),
            "bika", "settings");
    }
}