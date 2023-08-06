using PicaComic.Models;
using PicaComic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PicaComic.Utils;
using ShadowViewer.Plugin.Bika.Args;

namespace ShadowViewer.Plugin.Bika.ViewModels
{
    public class BikaCategoryViewModel
    {
        public int Pages { get; set; }
        public int Page { get; set; }
        public CategoryArg Arg { get; set; }
        public ObservableCollection<CategoryComic> CategoryComics { get;  } = new ObservableCollection<CategoryComic>();
        public BikaCategoryViewModel()
        { 
        
        }
        public async void Init(CategoryArg arg)
        {
            Arg = arg;
            await BikaHttpHelper.TryRequest(this, PicaClient.Category(arg.Category, arg.Page, arg.SortRule), res =>
            {
                Pages = res.Data.Comics.Pages;
                Page = res.Data.Comics.Page;
                CategoryComics.Clear();
                foreach (var comic in res.Data.Comics.Docs)
                {
                    CategoryComics.Add(comic);
                }
            });
        }
    }
}
