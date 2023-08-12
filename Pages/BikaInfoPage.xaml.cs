using CustomExtensions.WinUI;
using DryIoc;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using PicaComic.Models;
using ShadowViewer.Plugin.Bika.ViewModels;

namespace ShadowViewer.Plugin.Bika.Pages;

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
        if (RightGrid.SelectedIndex == 0)
            ViewModel.RefreshRecommendation();
        else
            ViewModel.RefreshComments();
    }

    private void LeftGrid_SizeChanged(object sender, SizeChangedEventArgs e)
    {
    }

    private async void LikeComment_OnClick(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement { Tag: Comment comment }) await ViewModel.LikeComment(comment);
    }

    private void CommentChild_OnClick(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement { Tag: Comment comment }) ViewModel.RefreshCommentChildren(comment);
    }

    private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        if (sender is Page page)
        {
            if (MainGrid.RowDefinitions[1].ActualHeight == 0)
            {
                LeftScrollViewer.Height = page.ActualHeight - 20;
                RightScrollViewer.Height = page.ActualHeight - 20;
            }
            else
            {
                LeftScrollViewer.Height = LeftGrid.ActualHeight;
                if (RightGrid.SelectedIndex == 0)
                {
                    // RightScrollViewer.Height;
                }
                else
                {
                }
            }
        }
    }

    private void Reply_OnClick(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement { Tag: Comment comment }) ViewModel.ReplyCommentOnClick(comment);
    }
}