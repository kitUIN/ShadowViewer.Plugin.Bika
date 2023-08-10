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
        public ObservableCollection<string> Tags { get;  }= new();
        private List<Episode> OriginEpisodes { get; set; } 
        public ObservableCollection<Episode> Episodes { get;  }= new();
        [ObservableProperty] private string favouriteText = BikaResourcesHelper.GetString(BikaResourceKey.Favourite);
        [ObservableProperty] private string likeText = BikaResourcesHelper.GetString(BikaResourceKey.Like);
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
