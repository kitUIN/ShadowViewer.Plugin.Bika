using System.Linq;
using CustomExtensions.WinUI;
using DryIoc;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using PicaComic.Models;
using PicaComic.Utils;
using ShadowViewer.Helpers;
using ShadowViewer.Plugin.Bika.Args;
using ShadowViewer.Plugin.Bika.Enums;
using ShadowViewer.Plugin.Bika.Helpers;
using ShadowViewer.Plugin.Bika.ViewModels;

namespace ShadowViewer.Plugin.Bika.Pages
{
    public sealed partial class BikaCategoryPage : Page
    {
        private BikaCategoryViewModel ViewModel { get; }
        private CategoryArg Arg { get; set; }

        public BikaCategoryPage()
        {
            this.LoadComponent(ref _contentLoaded);
            ViewModel = DiFactory.Services.Resolve<BikaCategoryViewModel>();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is CategoryArg arg && (Arg == null || arg.Category != Arg.Category))
            {
                ViewModel.Sort = arg.SortRule;
                ViewModel.Page = arg.Page;
                ViewModel.CategoryTitle = arg.Category;
                ViewModel.Mode = arg.Mode;
                 ViewModel.Refresh();
                Arg = arg;
                MainScrollViewer.ScrollToVerticalOffset(0);
            }
            else
            {
                 ViewModel.Refresh();
            }

            LockTip.LockChangedEvenet += ViewModel.CheckAllCategoryComicLock;
        }

        private void GridV_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is CategoryComic { IsLocked: false } comic)
            {
                Frame.Navigate(typeof(BikaInfoPage), comic.Id);
            }
        }

        private void LockButton_Click(object sender, RoutedEventArgs e)
        {
            LockTip.Show();
        }

        /// <summary>
        /// ÐÞ¸ÄÅÅÐò
        /// </summary>
        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Sort = EnumHelper.GetEnum<SortRule>(((MenuFlyoutItem)sender).Tag.ToString());
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

        private void Lock_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (BikaConfig.CanTemporaryUnlockComic)
            {
                if (sender is Grid grid && grid.Children[0] is StackPanel stackPanel)
                {
                    var icon = stackPanel.Children[0] as FontIcon;
                    var text1 = stackPanel.Children[1] as TextBlock;
                    icon.Glyph = "\uE785";
                    text1.Text = BikaResourcesHelper.GetString(BikaResourceKey.ClickOpenLock);
                }
            }
        }

        private void Lock_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (BikaConfig.CanTemporaryUnlockComic)
            {
                if (sender is Grid grid && grid.Children[0] is StackPanel stackPanel)
                {
                    var icon = stackPanel.Children[0] as FontIcon;
                    var text1 = stackPanel.Children[1] as TextBlock;
                    icon.Glyph = "\uE72E";
                    text1.Text = BikaResourcesHelper.GetString(BikaResourceKey.Locked);
                }
            }
        }

        private void UnLock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (BikaConfig.CanTemporaryUnlockComic)
            {
                if (sender is Grid grid && grid.Tag is CategoryComic category)
                {
                    category.IsLocked = false;
                }
            }
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(BikaSettingsPage), null,
                new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
        }
    }
}