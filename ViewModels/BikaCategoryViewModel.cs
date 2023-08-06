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
using CommunityToolkit.Mvvm.ComponentModel;

namespace ShadowViewer.Plugin.Bika.ViewModels
{
    public partial class BikaCategoryViewModel: ObservableObject
    {
        [ObservableProperty]
        private int pages = 1;
        [ObservableProperty]
        private int page = 1;
        public int Index
        {
            get { return Page - 1; }
            set
            {
                Page = value + 1;
                // SetProperty(ref page,value + 1,nameof(Index));
            }
        }
        [ObservableProperty]
        private string categoryTitle;
        [ObservableProperty]
        private string currentPageString;
        [ObservableProperty]
        private string sortRuleText;
        [ObservableProperty]
        private SortRule sortRule;
        public ObservableCollection<CategoryComic> CategoryComics { get;  } = new ObservableCollection<CategoryComic>();
        public BikaCategoryViewModel()
        { 
            
        }
        public async void Refresh()
        {
            await BikaHttpHelper.TryRequest(this, PicaClient.Category(CategoryTitle, Page, SortRule), res =>
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
        private void SetCurrentPageString()
        {
            CurrentPageString = BikaResourcesHelper.GetString(BikaResourceKey.Number) + $"{Page}/{Pages}" + BikaResourcesHelper.GetString(BikaResourceKey.Page);
        }
        partial void OnPageChanged(int oldValue, int newValue)
        {
            if(oldValue != newValue)
            {
                Log.Warning("{o}-{n}", oldValue, newValue);
                SetCurrentPageString();
                Refresh();
            }
        }
        partial void OnPagesChanged(int oldValue, int newValue)
        {
            if (oldValue != newValue)
            {
                SetCurrentPageString();
            }
        }
        partial void OnSortRuleChanged(SortRule oldValue, SortRule newValue)
        {
            if (oldValue != newValue)
            {
                SortRuleText = BikaResourcesHelper.GetString(newValue.ToString().ToUpper());
                Refresh();
            }
        }
    }
}
