using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using DryIoc;
using PicaComic;
using PicaComic.Models;
using ShadowViewer.Plugin.Bika.Enums;
using ShadowViewer.Plugin.Bika.Helpers;

namespace ShadowViewer.Plugin.Bika.ViewModels
{
    public partial class BikaInfoViewModel: ObservableObject
    {
        private IPicaClient BikaClient { get; }
        public BikaInfoViewModel(IPicaClient client)
        {
            BikaClient = client;
        }
        public string ComicId { get; set; }
        [ObservableProperty]
        private ComicInfo currentComic = new();
        [ObservableProperty]
        private bool recommendEmpty = true; 
        [ObservableProperty]
        private bool commentEmpty = true;
        private int order;
        public ObservableCollection<string> Tags { get;  }= new();
        public int CommentPage {get;set;}
        public ObservableCollection<Episode> Episodes { get;  }= new();
        public ObservableCollection<CategoryComic> RecommendComics { get;  }= new();
        public ObservableCollection<Comment> Comments { get;  }= new();
        [ObservableProperty] private string favouriteText = BikaResourcesHelper.GetString(BikaResourceKey.Favourite);
        [ObservableProperty] private string likeText = BikaResourcesHelper.GetString(BikaResourceKey.Like);
        
        
        
        public async void RefreshComments(int page = 1)
        {
            if (CommentEmpty && page==1)
            {
                await BikaHttpHelper.TryRequest(this, BikaClient.ComicComments(ComicId, page), res =>
                {
                    if (page == 1 && res.Data.TopComments.Count != 0)
                    {
                        foreach (var item in res.Data.TopComments)
                        {
                            Comments.Add(item);
                        }
                    }
                    foreach (var item in res.Data.Comments.Docs)
                    {
                        item.Order = order--;
                        Comments.Add(item);
                    }
                });
            }
            CommentEmpty = Comments.Count == 0;
        }        
        public async void RefreshRecommendation()
        {
            if (RecommendEmpty)
            {
                await BikaHttpHelper.TryRequest(this, BikaClient.Recommendation(ComicId), res =>
                {
                    foreach (var item in res.Data.Comics)
                    {
                        RecommendComics.Add(item);
                    }
                });
            }
            RecommendEmpty = RecommendComics.Count == 0;
        }
            
            public async void Refresh()
        {
            await BikaHttpHelper.TryRequest(this, BikaClient.ComicInfo(ComicId), res =>
            {
                CurrentComic = res.Data.Comic;
                order = CurrentComic.TotalComments;
                foreach (var item in CurrentComic.Tags)
                {
                    Tags.Add(item);
                }
            });
            var i = 1;
            var total =  1;
            await BikaHttpHelper.TryRequest(this, BikaClient.Episodes(ComicId, i), res =>
            {
                foreach (var item in res.Data.Episodes.Docs)
                {
                    Episodes.Add(item);
                }

                total = res.Data.Episodes.Pages;
            });
            for (i++; i <= total; i++)
            {
                await BikaHttpHelper.TryRequest(this, BikaClient.Episodes(ComicId, i), res =>
                {
                    foreach (var item in res.Data.Episodes.Docs)
                    {
                        Episodes.Add(item);
                    }
                });
            }
        }
    }
}
