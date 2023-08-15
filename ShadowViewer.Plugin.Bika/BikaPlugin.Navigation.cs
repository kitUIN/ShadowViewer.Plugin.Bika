using System;
using System.Collections.Generic;
using CustomExtensions.WinUI;
using ShadowViewer.Enums;
using ShadowViewer.Helpers;
using ShadowViewer.Models;
using ShadowViewer.Plugin.Bika.Args;
using ShadowViewer.Plugin.Bika.Enums;
using ShadowViewer.Plugin.Bika.Helpers;
using ShadowViewer.Plugin.Bika.Pages;

namespace ShadowViewer.Plugin.Bika;

public partial class BikaPlugin
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override Type SettingsPage => typeof(BikaSettingsPage);

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override IEnumerable<ShadowNavigationItem> NavigationViewMenuItems => new List<ShadowNavigationItem>()
    {
        new()
        {
            Content = BikaResourcesHelper.GetString(BikaResourceKey.Title),
            Icon = XamlHelper.CreateImageIcon(MetaData.Logo),
            Id = MetaData.Id
        }
    };

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override IEnumerable<ShadowNavigationItem> NavigationViewFooterItems => new List<ShadowNavigationItem>();

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override void NavigationViewItemInvokedHandler(object tag, ref Type page, ref object parameter)
    {
        if (BikaClient.HasToken)
        {
            page = typeof(ClassificationPage);
            parameter = null;
        }
        else
        {
            ShowLoginFrame();
        }
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override void Navigate(Uri uri, string[] urls)
    {
        if (urls.Length == 0) return;
        switch (urls[0])
        {
            case "comic":
                if (urls.Length == 2) Caller.NavigateTo(typeof(BikaInfoPage), urls[1]);
                break;
            case "classification":
                if (BikaClient.HasToken)
                    Caller.NavigateTo(typeof(ClassificationPage), null);
                else
                    ShowLoginFrame();
                break;
            case "settings":
                Caller.NavigateTo(typeof(BikaSettingsPage), null);
                break;
            case "user":
                // Caller.NavigateTo(NavigateMode.Page,typeof());
                break;
            case "category":
                if (urls.Length == 2)
                    Caller.NavigateTo(typeof(BikaCategoryPage), new CategoryArg { Category = urls[1] });
                break;
            case "random":
                Caller.NavigateTo(typeof(BikaCategoryPage),
                    new CategoryArg
                    {
                        Category = BikaResourcesHelper.GetString(BikaResourceKey.Random), Mode = CategoryMode.Random
                    });
                break;
        }
    }
}