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

namespace ShadowViewer.Plugin.Bika.Pages
{
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

        private async void LikeComment_OnClick(object sender, RoutedEventArgs e)
        {
            if(sender is FrameworkElement { Tag: Comment comment })
            {
                await ViewModel.LikeComment(comment);
            }
        }

        private void CommentChild_OnClick(object sender, RoutedEventArgs e)
        {
            if(sender is FrameworkElement { Tag: Comment comment })
            {
                ViewModel.RefreshCommentChildren(comment);
            }
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
