using CustomExtensions.WinUI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using ShadowViewer.Plugin.Bika.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI;
using System.Diagnostics;
using Windows.UI.ViewManagement;
using DryIoc;
using PicaComic;
using PicaComic.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ShadowViewer.Plugin.Bika.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BikaInfoPage : Page
    {
        public BikaInfoViewModel ViewModel { get; }
        public BikaInfoPage()
        {
            this.LoadComponent(ref _contentLoaded);
            ViewModel = DiFactory.Services.Resolve<BikaInfoViewModel>();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is not string id) return;
            ViewModel.ComicId = id;
            ViewModel.Refresh();
        }

        private void Border_OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (sender is Border border)
            {
                border.Background = Application.Current.Resources["HyperlinkButtonBackgroundPointerOver"] as Brush;
            }
        }
        private void Border_OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (sender is Border border)
            {
                border.Background = new SolidColorBrush(Colors.Transparent);
            }
        }

        private void Author_OnClick(object sender, RoutedEventArgs e)
        {
             
        }

        private void ChineseTeam_OnClick(object sender, RoutedEventArgs e)
        {
             
        }

        private void Segmented_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(RightGrid.SelectedIndex == 0)
            {
                ViewModel.RefreshRecommendation();
            }
            else
            {
                ViewModel.RefreshComments();
            }
        }

        private void LeftGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            
        }

        private async void LikeComment_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if(sender is FrameworkElement { Tag: Comment comment })
            {
                await ViewModel.LikeComment(comment);
            }
        }

        private void CommentChild_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if(sender is Page page)
            {
                if (MainGrid.RowDefinitions[1].ActualHeight == 0)
                {
                    LeftScrollViewer.Height = page.ActualHeight - 20;
                    RightScrollViewer.Height = page.ActualHeight - 20;
                }
                else
                {
                    LeftScrollViewer.Height = LeftGrid.ActualHeight;
                    if(RightGrid.SelectedIndex == 0)
                    {
                        // RightScrollViewer.Height;
                    }
                    else
                    {
                        
                    }
                }
            }

        }
    }
}
