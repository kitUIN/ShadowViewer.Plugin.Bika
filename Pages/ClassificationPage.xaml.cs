using System;
using CustomExtensions.WinUI;
using DryIoc;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using PicaComic.Models;
using ShadowViewer.Extensions;
using ShadowViewer.Plugin.Bika.Args;
using ShadowViewer.Plugin.Bika.Enums;
using ShadowViewer.Plugin.Bika.Helpers;
using ShadowViewer.Plugin.Bika.ViewModels;

namespace ShadowViewer.Plugin.Bika.Pages;

public sealed partial class ClassificationPage : Page
{
    private ClassificationViewModel ViewModel { get; }

    public ClassificationPage()
    {
        this.LoadComponent(ref _contentLoaded);
        ViewModel = DiFactory.Services.Resolve<ClassificationViewModel>();
    }

    protected override async void OnNavigatedTo(NavigationEventArgs e)
    {
        await ViewModel.GetClassification();
    }

    /// <summary>
    /// µã»÷·ÖÇø
    /// </summary>
    private void GridV_OnItemClick(object sender, ItemClickEventArgs e)
    {
        var category = (Category)e.ClickedItem;
        if (category.Title == BikaResourcesHelper.GetString(BikaResourceKey.Random))
        {
            Frame.Navigate(typeof(BikaCategoryPage), new CategoryArg
                {
                    Category = category.Title,
                    Mode = CategoryMode.Random
                },
                new SlideNavigationTransitionInfo { Effect = SlideNavigationTransitionEffect.FromRight });
        }
        else if (category.Title == BikaResourcesHelper.GetString(BikaResourceKey.Leaderboard))
        {
        }
        else
        {
            switch (category.IsWeb)
            {
                case false:
                    Frame.Navigate(typeof(BikaCategoryPage), new CategoryArg { Category = category.Title },
                        new SlideNavigationTransitionInfo { Effect = SlideNavigationTransitionEffect.FromRight });
                    break;
                case true when category.Link != null:
                    new Uri(category.Link).LaunchUriAsync();
                    break;
            }
        }
    }
}