using System;
using System.Collections.Generic;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using PicaComic;
using ShadowViewer.Plugin.Bika.Args;
using ShadowViewer.Plugin.Bika.Enums;
using ShadowViewer.Plugin.Bika.Models;
using ShadowViewer.Plugin.Bika.Pages;
using CustomExtensions.WinUI;
using ShadowPluginLoader.Attributes;
using ShadowViewer.Sdk;
using ShadowViewer.Sdk.Models.Interfaces;
using ShadowViewer.Sdk.Plugins;
using ShadowViewer.Sdk.Responders;
using ShadowViewer.Sdk.Utils;
using ShadowViewer.Plugin.Bika.I18n;

namespace ShadowViewer.Plugin.Bika.Responders;

[EntryPoint(Name = nameof(PluginResponder.NavigationResponder))]
public partial class BikaNavigationResponder : AbstractNavigationResponder
{
    [Autowired] private IPicaClient Client { get; }
    [Autowired] private PluginLoader PluginService { get; }

    public override IEnumerable<IShadowNavigationItem> NavigationViewMenuItems { get; } =
        new List<IShadowNavigationItem>
        {
            new BikaNavigationItem()
            {
                Content = I18N.Title,
                Icon = new ImageIcon()
                {
                    Source = new BitmapImage(new Uri("ms-plugin://ShadowViewer.Plugin.Bika/Assets/Icons/logo.png".PluginPath()))
                },
                Id = "Bika.Classification"
            }
        };

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override ShadowNavigation? NavigationViewItemInvokedHandler(IShadowNavigationItem item)
    {
        //if (Client.HasToken)
        //{
        //    page = typeof(ClassificationPage);
        //    parameter = null;
        //}
        //else
        //{
        //    (PluginService.GetEnabledPlugin(Id) as BikaPlugin)?.ShowLoginFrame();
        //}
        return item.Id switch
        {
            "Bika.Classification" => new ShadowNavigation(typeof(ClassificationPage)),
            _ => null
        };
    }

    public override ShadowNavigation? Navigate(Uri uri, string[] urls)
    {
        if (urls.Length == 0) return null;
        switch (urls[0])
        {
            case "comic":
                return urls.Length == 2 ? new ShadowNavigation(typeof(BikaInfoPage), urls[1]) : null;
            case "classification":
                if (!Client.HasToken) BikaPlugin.ShowLoginFrame();
                return new ShadowNavigation(typeof(ClassificationPage), null);
            case "settings":
                return new ShadowNavigation(typeof(BikaSettingsPage), null);
            case "user":
                // new ShadowNavigation(NavigateMode.Page,typeof());
                break;
            case "category":
                return urls.Length == 2
                    ? new ShadowNavigation(typeof(BikaCategoryPage), new CategoryArg { Category = urls[1] })
                    : null;
            case "random":
                return new ShadowNavigation(typeof(BikaCategoryPage),
                    new CategoryArg
                    {
                        Category = ResourcesHelper.GetString(ResourceKey.Random), Mode = CategoryMode.Random
                    });
        }

        return null;
    }
}