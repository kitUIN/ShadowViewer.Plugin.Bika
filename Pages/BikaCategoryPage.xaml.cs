using CustomExtensions.WinUI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using PicaComic;
using PicaComic.Utils;
using ShadowViewer.Enums;
using ShadowViewer.Plugin.Bika.Args;
using ShadowViewer.Plugin.Bika.ViewModels;
using ShadowViewer.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace ShadowViewer.Plugin.Bika.Pages
{

    public sealed partial class BikaCategoryPage : Page
    {
        private BikaCategoryViewModel ViewModel { get; }
        public BikaCategoryPage()
        {
            this.LoadComponent(ref _contentLoaded);
            ViewModel = new BikaCategoryViewModel();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var arg = e.Parameter as CategoryArg;
            if (arg != null)
            {
                ViewModel.CategoryTitle = arg.Category;
                ViewModel.Page = arg.Page - 1;
                ViewModel.SortRule = arg.SortRule;
                ViewModel.Refresh();
            }
        }

        private void GridV_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        

        private void LockButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Refresh();
        }

        private void GotoButton_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// �޸�����
        /// </summary>
        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SortRule = EnumHelper.GetEnum<SortRule>(((MenuFlyoutItem)sender).Tag.ToString());
            foreach (var item in SortFlyout.Items.Cast<MenuFlyoutItem>())
            {
                item.Icon = item.Text == SortButton.Label ? new FontIcon() { Glyph = "\uE7B3" } : null;
            }
        }
        private void SortButton_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var item in SortFlyout.Items.Cast<MenuFlyoutItem>())
            {
                item.Icon = item.Text == SortButton.Label ? new FontIcon() { Glyph = "\uE7B3" } : null;
            }
        }
    }
}
