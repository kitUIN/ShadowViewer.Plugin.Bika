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
            }
        }
        [ObservableProperty]
        private string categoryTitle;
        [ObservableProperty]
        private string currentPageString;
        [ObservableProperty]
        private string sortRuleText;
        [ObservableProperty]
        private SortRule sort = SortRule.dd;
        public ObservableCollection<CategoryComic> CategoryComics { get;  } = new ObservableCollection<CategoryComic>();
        
        public BikaCategoryViewModel()
        {
            SortRuleText = BikaResourcesHelper.GetString(Sort.ToString().ToUpper());
        }
        public void CheckAllCategoryComicLock()
        {
            
            foreach (var comic in CategoryComics.ToList())
            {
                CheckCategoryLock(comic);
                if (comic.IsLocked && BikaConfig.IsIgnoreLockComic)
                {
                    CategoryComics.Remove(comic);
                }
            }
        }
        public async void Refresh()
        {
            await BikaHttpHelper.TryRequest(this, PicaClient.Category(CategoryTitle, Page, Sort), res =>
            {
                Pages = res.Data.Comics.Pages;
                Page = res.Data.Comics.Page;
                CategoryComics.Clear();
                foreach (var comic in res.Data.Comics.Docs)
                {
                    CheckCategoryLock(comic);
                    if (!(comic.IsLocked && BikaConfig.IsIgnoreLockComic))
                    {
                        CategoryComics.Add(comic);
                    }
                }
            });
        }
        public void CheckCategoryLock(CategoryComic comic)
        {
            comic.LockCategories = comic.Categories.Where(x => BikaData.Current.Locks.Any(y => y.Title == x && !y.IsOpened)).ToList();
            if (comic.LockCategories.Count > 0)
            {
                comic.IsLocked = true;
            }
        }
        private void SetCurrentPageString()
        {
            CurrentPageString = BikaResourcesHelper.GetString(BikaResourceKey.Number) + $"{Page}/{Pages}" + BikaResourcesHelper.GetString(BikaResourceKey.Page);
        }
        partial void OnPageChanged(int oldValue, int newValue)
        {
            if(oldValue != newValue)
            {
                SetCurrentPageString();
                if (!string.IsNullOrEmpty(SortRuleText))
                {
                    Refresh();
                }
            }
        }
        partial void OnPagesChanged(int oldValue, int newValue)
        {
            if (oldValue != newValue)
            {
                SetCurrentPageString();
            }
        }
        partial void OnSortChanged(SortRule oldValue, SortRule newValue)
        {
            if (oldValue != newValue)
            {
                SortRuleText = BikaResourcesHelper.GetString(newValue.ToString().ToUpper());
                if (!string.IsNullOrEmpty(SortRuleText))
                {
                    Refresh();
                }
            }
        }
    }
}
