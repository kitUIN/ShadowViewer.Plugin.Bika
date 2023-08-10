using CommunityToolkit.Mvvm.ComponentModel;
using PicaComic;
using PicaComic.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowViewer.Plugin.Bika.ViewModels
{
    public partial class BikaInfoViewModel: ObservableObject
    {
        public string ComicId { get; set; }
        [ObservableProperty]
        private ComicInfo currentComic = new();
        public ObservableCollection<string> Tags { get;  }= new();
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
        }
    }
}
