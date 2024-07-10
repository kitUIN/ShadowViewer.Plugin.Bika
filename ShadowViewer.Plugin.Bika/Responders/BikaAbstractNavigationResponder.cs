using System;
using System.Collections.Generic;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using PicaComic;
using ShadowViewer.Helpers;
using ShadowViewer.Interfaces;
using ShadowViewer.Plugin.Bika.Args;
using ShadowViewer.Plugin.Bika.Enums;
using ShadowViewer.Plugin.Bika.Helpers;
using ShadowViewer.Plugin.Bika.Models;
using ShadowViewer.Plugin.Bika.Pages;
using ShadowViewer.Responders;
using ShadowViewer.Services;

using SqlSugar;

namespace ShadowViewer.Plugin.Bika.Responders;

public class BikaAbstractNavigationResponder : NavigationResponderBase
{
    public override IEnumerable<IShadowNavigationItem> NavigationViewMenuItems { get; } =
        new List<IShadowNavigationItem>
        {
            new BikaNavigationItem()
            {
                Content = ResourcesHelper.GetString(ResourceKey.Title),
                Icon =  new ImageIcon()
                {
                    Source = new BitmapImage(new System.Uri(BikaPlugin.Meta.Logo))
                },
                Id = BikaPlugin.Meta.Id
            }
        };

    public override void NavigationViewItemInvokedHandler(IShadowNavigationItem item, ref Type? page,
        ref object? parameter)
    {
        if (item.Id != BikaPlugin.Meta.Id) return;
        if (Client.HasToken)
        {
            page = typeof(ClassificationPage);
            parameter = null;
        }
        else
        {
            (PluginService.GetEnabledPlugin(Id) as BikaPlugin)?.ShowLoginFrame();
        }
    }

    public override void Navigate(Uri uri, string[] urls)
    {
        if (urls.Length == 0) return;
        switch (urls[0])
        {
            case "comic":
                if (urls.Length == 2) Caller.NavigateTo(typeof(BikaInfoPage), urls[1]);
                break;
            case "classification":
                if (Client.HasToken)
                    Caller.NavigateTo(typeof(ClassificationPage), null);
                else
                    (PluginService.GetEnabledPlugin(Id) as BikaPlugin)?.ShowLoginFrame();
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
                        Category = ResourcesHelper.GetString(ResourceKey.Random), Mode = CategoryMode.Random
                    });
                break;
        }
    }

    private IPicaClient Client { get; }

    public BikaAbstractNavigationResponder(ICallableService callableService, ISqlSugarClient sqlSugarClient,
        CompressService compressServices, IPluginService pluginService, IPicaClient picaClient, string id) : base(
        callableService,
        sqlSugarClient, compressServices, pluginService, id)
    {
        Client = picaClient;
    }
}