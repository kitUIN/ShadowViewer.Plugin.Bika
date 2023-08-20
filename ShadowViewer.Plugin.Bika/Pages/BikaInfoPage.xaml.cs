using CustomExtensions.WinUI;
using DryIoc;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Navigation;
using PicaComic.Models;
using ShadowViewer.Args;
using ShadowViewer.Interfaces;
using ShadowViewer.Plugin.Bika.Args;
using ShadowViewer.Plugin.Bika.Enums;
using ShadowViewer.Plugin.Bika.ViewModels;
using ShadowViewer.Plugin.Local.Pages;

namespace ShadowViewer.Plugin.Bika.Pages;

public sealed partial class BikaInfoPage : Page
{
    public BikaInfoViewModel ViewModel { get; }

    public BikaInfoPage()
    {
        this.LoadComponent(ref _contentLoaded);
        ViewModel = DiFactory.Services.Resolve<BikaInfoViewModel>();
        ViewModel.ScrollToCommentEvent += ViewModelOnScrollToCommentEvent;
    }

    private void GridV_ItemClick(object sender, ItemClickEventArgs e)
    {
        if (e.ClickedItem is CategoryComic { IsLocked: false } comic) Frame.Navigate(typeof(BikaInfoPage), comic.Id);
    }

    private void ViewModelOnScrollToCommentEvent(object? sender, ScrollToCommentEventArg e)
    {
        var element = CommentsItemsRepeater.GetOrCreateElement(e.Index);
        CommentsItemsRepeater.UpdateLayout();
        element.StartBringIntoView(new BringIntoViewOptions() { VerticalOffset = 0D, VerticalAlignmentRatio = 0.0f });
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        if (e.Parameter is not string id) return;
        ViewModel.ComicId = id;
        ViewModel.Refresh();
    }

    private void Author_OnClick(object sender, RoutedEventArgs e)
    {
        if (sender is HyperlinkButton { Content: string tag })
            DiFactory.Services.Resolve<ICallableService>().NavigateTo(typeof(BikaCategoryPage),
                new CategoryArg
                {
                    Category = tag, Mode = CategoryMode.Search
                }, true);
    }

    private void ChineseTeam_OnClick(object sender, RoutedEventArgs e)
    {
        if (sender is HyperlinkButton { Content: string tag })
            DiFactory.Services.Resolve<ICallableService>().NavigateTo(typeof(BikaCategoryPage),
                new CategoryArg
                {
                    Category = tag, Mode = CategoryMode.Search
                }, true);
    }

    private async void Segmented_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (RightGrid.SelectedIndex == 0)
            ViewModel.RefreshRecommendation();
        else if (ViewModel.Comments.Count == 0) await ViewModel.LoadComments();
    }

    private async void LikeComment_OnClick(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement { Tag: Comment comment }) await ViewModel.LikeComment(comment);
    }

    private async void CommentChild_OnClick(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement { Tag: Comment comment }) await ViewModel.RefreshCommentChildren(comment);
    }

    private void Page_SizeChanged(object sender, SizeChangedEventArgs? e)
    {
        if (sender is not Page page) return;
         
        if (page.ActualWidth >= 1140)
        {
            Grid.SetRow(RightScrollViewer,0);
            Grid.SetColumn(RightScrollViewer,1);
            LeftScrollViewer.Height = page.ActualHeight - 20;
            RightScrollViewer.Height = page.ActualHeight - 20;
            RightScrollViewer.MaxWidth = 475;
        }
        else
        {
            Grid.SetRow(RightScrollViewer,1);
            Grid.SetColumn(RightScrollViewer,0);
            RightScrollViewer.MaxWidth = LeftGrid.ActualWidth;
            LeftScrollViewer.Height = LeftGrid.ActualHeight;
        }
    }

    private void Reply_OnClick(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement { Tag: Comment comment }) ViewModel.ReplyCommentOnClick(comment);
    }

    private async void ScrollViewer_OnViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
    {
        if (sender is ScrollViewer viewer && viewer.VerticalOffset + viewer.ActualHeight + 2 >= viewer.ExtentHeight)
            await ViewModel.LoadNextComments();
    }

    private void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
    {
        ViewModel.ScrollToComment(ViewModel.ReplyComment);
    }

    private void Tag_OnClick(object sender, RoutedEventArgs e)
    {
        if (sender is Button { Content: string tag })
            DiFactory.Services.Resolve<ICallableService>().NavigateTo(typeof(BikaCategoryPage),
                new CategoryArg
                {
                    Category = tag, Mode = CategoryMode.Search
                });
    }

    private void EpisodesButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (sender is Button { Tag: Episode episode })
            Frame.Navigate(typeof(PicPage), new PicViewArg(BikaPlugin.Meta.Id, new ComicArg
                { ComicInfo = ViewModel.CurrentComic, CurrentEpisode = episode.Order, Episodes = ViewModel.Episodes }));
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        Page_SizeChanged(this, null); // 刚进入的时候不会响应更改
    }

    private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
    {
        if (sender is not FrameworkElement { Tag: Creater creator }) return;
        CreatorTip.CurrentUser = creator;
        CreatorTip.Show();
    }
 
}