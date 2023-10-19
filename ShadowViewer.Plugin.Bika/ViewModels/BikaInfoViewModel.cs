using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PicaComic;
using PicaComic.Models;
using ShadowViewer.Plugin.Bika.Args;
using ShadowViewer.Plugin.Bika.Enums;
using ShadowViewer.Plugin.Bika.Helpers;

namespace ShadowViewer.Plugin.Bika.ViewModels;

public partial class BikaInfoViewModel : ObservableObject
{
    public event EventHandler<ScrollToCommentEventArg>? ScrollToCommentEvent;
    private IPicaClient BikaClient { get; }

    public BikaInfoViewModel(IPicaClient client)
    {
        BikaClient = client;
    }

    public string ComicId { get; set; } = "";
    [ObservableProperty] private ComicInfo currentComic = new();
    [ObservableProperty] private Comment replyComment = new();
    [ObservableProperty] private string replyText = "";
    
    public ObservableCollection<string> Tags { get; } = new();
    public int CommentPage { get; set; }
    public ObservableCollection<Episode> Episodes { get; } = new();
    public ObservableCollection<CategoryComic> RecommendComics { get; } = new();
    public ObservableCollection<Comment> Comments { get; } = new();
    private int CurrentCommentPage { get; set; } = 1;
    private int TotalCommentPage { get; set; } = 1;
    private int CommentOrder { get; set; }
    private bool CommentLoading { get; set; }
    /// <summary> 
    /// 取消回复
    /// </summary>
    [RelayCommand]
    private void RemoveReply()
    {
        ReplyComment = new Comment();
    }

    /// <summary>
    /// 评论
    /// </summary>
    [RelayCommand]
    private async Task CommentAsync()
    {
        if (string.IsNullOrEmpty(ReplyText)) return;
        if (string.IsNullOrEmpty(ReplyComment.Id))
        {
            await BikaHttpHelper.TryRequestWithTip(this, BikaClient.SendComicComment(CurrentComic.Id, ReplyText), async _ =>
            {
                ReplyComment = new Comment();
                ReplyText = "";
                CommentOrder = 0;
                CurrentCommentPage = 1;
                Comments.Clear();
                await LoadComments();
            },$"[{BikaResourcesHelper.GetString(BikaResourceKey.SendComment)}]");
        }
        else
        {
            await BikaHttpHelper.TryRequestWithTip(this, BikaClient.SendCommentChildren(ReplyComment.Id, ReplyText),
                async _ =>
                {
                    var id = ReplyComment.Id;
                    ReplyComment = new Comment();
                    ReplyText = "";
                    await ReLoadComments();
                    var comment = Comments.FirstOrDefault(x => x.Id == id);
                    if (comment != null)
                    {
                        ScrollToComment(Comments.IndexOf(comment));
                        await RefreshCommentChildren(comment);
                    }
                },$"[{BikaResourcesHelper.GetString(BikaResourceKey.SendComment)}]");
        }
    }

    private void ScrollToComment(int index)
    {
        if (index > -1)
        {
            ScrollToCommentEvent?.Invoke(this,new ScrollToCommentEventArg(index));
        }
    }
    public void ScrollToComment(Comment comment)
    {
        if(string.IsNullOrEmpty(comment.Id)) return;
        if (Comments.FirstOrDefault(x => x.Id == comment.Id) is { } com)
        {
            ScrollToComment(Comments.IndexOf(com));
        }
    }
    /// <summary>
    /// 喜欢漫画
    /// </summary>
    [RelayCommand]
    private async Task LikeAsync()
    {
        await BikaHttpHelper.TryRequestWithTip(this, BikaClient.ComicLike(CurrentComic.Id), res =>
        {
            switch (res.Data.action)
            {
                case "like":
                    CurrentComic.LikesCount += 1;
                    CurrentComic.TotalLikes += 1;
                    CurrentComic.IsLiked = true;
                    break;
                case "unlike":
                    CurrentComic.LikesCount -= 1;
                    CurrentComic.TotalLikes -= 1;
                    CurrentComic.IsLiked = false;
                    break;
            }
        },BikaResourcesHelper.GetString(BikaResourceKey.Action));
    }

    /// <summary>
    /// 收藏漫画
    /// </summary>
    [RelayCommand]
    private async Task Favourite()
    {
        await BikaHttpHelper.TryRequestWithTip(this, BikaClient.ComicFavourite(CurrentComic.Id), res =>
        {
            CurrentComic.IsFavourite = res.Data.action switch
            {
                "favourite" => true,
                "un_favourite" => false,
                _ => CurrentComic.IsFavourite
            };
        },BikaResourcesHelper.GetString(BikaResourceKey.Action));
    }

    /// <summary>
    /// 喜欢评论
    /// </summary>
    public async Task LikeComment(Comment comment)
    {
        comment.IsLiked = !comment.IsLiked;
        await BikaHttpHelper.TryRequestWithTip(this, BikaClient.CommentLike(comment.Id), res =>
        {
            switch (res.Data.action)
            {
                case "like":
                    comment.LikesCount += 1;
                    comment.IsLiked = true;
                    break;
                case "unlike":
                    comment.LikesCount -= 1;
                    comment.IsLiked = false;
                    break;
            }
        },BikaResourcesHelper.GetString(BikaResourceKey.Action));
    }

    /// <summary>
    /// 点击回复
    /// </summary>
    public void ReplyCommentOnClick(Comment comment)
    {
        ReplyComment = comment;
    }
    
    /// <summary>
    /// 加载子评论
    /// </summary>
    /// <param name="comment"></param>
    public async Task RefreshCommentChildren(Comment comment)
    {
        if (comment.TotalComments == 0) return;
        comment.IsShowChildren = !comment.IsShowChildren;
        if (comment.TotalComments == comment.Children.Count) return;
        comment.Children.Clear();
        var order = comment.TotalComments;
        var i = 1;
        var total = 1;
        await BikaHttpHelper.TryRequestWithTip(this, BikaClient.CommentChildren(comment.Id, i), res =>
        {
            foreach (var item in res.Data.Comments.Docs)
            {
                item.Order = order--;
                comment.Children.Add(item);
            }
            total = res.Data.Comments.Pages;
        },isSendSuccess:false);
        for (i++; i <= total; i++)
            await BikaHttpHelper.TryRequestWithTip(this, BikaClient.CommentChildren(comment.Id, i), res =>
            {
                foreach (var item in res.Data.Comments.Docs)
                {
                    item.Order = order--;
                    comment.Children.Add(item);
                }
            },isSendSuccess:false);
    }

    private async Task ReLoadComments()
    {
        CommentOrder = 0;
        Comments.Clear(); 
        await LoadComments();
        if (CurrentCommentPage != 1)
        {
            for (var page = 2; page <= CurrentCommentPage; page++)
            {
                await LoadComments(page);
            }
        }
    }

    public async Task LoadNextComments()
    {
        if (!CommentLoading)
        {
            CommentLoading = true;
            await LoadComments(CurrentCommentPage + 1);
            CommentLoading = false;
        }
    }
    public async Task LoadComments(int page = 1)
    {
        if (TotalCommentPage < page) return;
        await BikaHttpHelper.TryRequestWithTip(this, BikaClient.ComicComments(ComicId, page), res =>
        {
            if (CommentOrder == 0)
            {
                CommentOrder = res.Data.Comments.Total;
            }

            TotalCommentPage = res.Data.Comments.Pages;
            if (page == 1 && res.Data.TopComments.Count != 0)
                foreach (var item in res.Data.TopComments)
                    Comments.Add(item);
            foreach (var item in res.Data.Comments.Docs)
            {
                item.Order = CommentOrder--;
                if (item is { IsTop: false, IsHide: false }) Comments.Add(item);
            }

            if (CommentLoading)
            {
                CurrentCommentPage = res.Data.Comments.Page;
            }
        },isSendSuccess:false);
    }

    public async void RefreshRecommendation()
    {
        if (RecommendComics.Count == 0)
            await BikaHttpHelper.TryRequestWithTip(this, BikaClient.Recommendation(ComicId), res =>
            {
                foreach (var item in res.Data.Comics) RecommendComics.Add(item);
            },isSendSuccess:false);
    }

    public async void Refresh()
    {
        await BikaHttpHelper.TryRequest(this, BikaClient.ComicInfo(ComicId), res =>
        {
            CurrentComic = res.Data.Comic;
            BikaHistoryHelper.Add(res.Data.Comic);
            foreach (var item in CurrentComic.Tags) Tags.Add(item);
        });
        var i = 1;
        var total = 1;
        await BikaHttpHelper.TryRequest(this, BikaClient.Episodes(ComicId, i), res =>
        {
            foreach (var item in res.Data.Episodes.Docs) Episodes.Add(item);

            total = res.Data.Episodes.Pages;
        });
        for (i++; i <= total; i++)
            await BikaHttpHelper.TryRequest(this, BikaClient.Episodes(ComicId, i), res =>
            {
                foreach (var item in res.Data.Episodes.Docs) Episodes.Add(item);
            });
    }
}