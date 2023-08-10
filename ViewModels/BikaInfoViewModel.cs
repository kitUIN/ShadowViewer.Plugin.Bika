using CommunityToolkit.Mvvm.ComponentModel;
using PicaComic;
using PicaComic.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace ShadowViewer.Plugin.Bika.ViewModels
{
    public partial class BikaInfoViewModel: ObservableObject
    {
        public string ComicId { get; set; }
        [ObservableProperty]
        private ComicInfo currentComic = new();
        [ObservableProperty]
        public bool recommendEmpty = false; 
        [ObservableProperty]
        public bool commentEmpty = false; 
        public ObservableCollection<string> Tags { get;  }= new();
 
        public ObservableCollection<Episode> Episodes { get;  }= new();
        public ObservableCollection<CategoryComic> RecommendComics { get;  }= new();
        [ObservableProperty] private string favouriteText = BikaResourcesHelper.GetString(BikaResourceKey.Favourite);
        [ObservableProperty] private string likeText = BikaResourcesHelper.GetString(BikaResourceKey.Like);
        public async void RefreshComments()
        {
            await BikaHttpHelper.TryRequest(this, PicaClient.ComicComments(ComicId,1), res =>
            {
                 
            });
            //CommentEmpty = RecommendComics.Count == 0;
        }        
        public async void RefreshRecommendation()
        {
            if (RecommendEmpty)
            {
                await BikaHttpHelper.TryRequest(this, PicaClient.Recommendation(ComicId), res =>
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
            await BikaHttpHelper.TryRequest(this, PicaClient.ComicInfo(ComicId), res =>
            { 
                CurrentComic = res.Data.Comic;
                foreach (var item in CurrentComic.Tags)
                {
                    Tags.Add(item);
                }
            });
            var i = 1;
            var total =  1;
            await BikaHttpHelper.TryRequest(this, PicaClient.Episodes(ComicId, i), res =>
            {
                foreach (var item in res.Data.Episodes.Docs)
                {
                    Episodes.Add(item);
                }

                total = res.Data.Episodes.Pages;
            });
            for (i++; i <= total; i++)
            {
                await BikaHttpHelper.TryRequest(this, PicaClient.Episodes(ComicId, i), res =>
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
