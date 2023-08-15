using System;
using CommunityToolkit.Mvvm.ComponentModel;
using PicaComic;
using PicaComic.Models;
using PicaComic.Utils;
using System.Collections.ObjectModel;
using System.Linq;
using ShadowViewer.Plugin.Bika.Enums;
using ShadowViewer.Plugin.Bika.Helpers;
using CommunityToolkit.Mvvm.Input;

namespace ShadowViewer.Plugin.Bika.ViewModels
{
    public partial class BikaCategoryViewModel: ObservableObject
    {
        [ObservableProperty]
        private int pages = 1;
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Index))]
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
        public CategoryMode Mode { get; set; }
        public IPicaClient BikaClient { get; }
        public BikaCategoryViewModel(IPicaClient bikaClient)
        {
            BikaClient = bikaClient;
            SortRuleText = BikaResourcesHelper.GetString(Sort.ToString().ToUpper());
        }
        public void CheckAllCategoryComicLock(object sender, EventArgs e)
        {
            foreach (var comic in CategoryComics)
            {
                CheckCategoryLock(comic);
            }
        }
        [RelayCommand]
        private void NextPage()
        {
            if (Page + 1 <= Pages)
            {
                Page += 1;
            }
        }

        [RelayCommand]
        private void PreviousPage()
        {
            if (Page - 1 > 0)
            {
                Page -= 1;
            }
        }
        public async void Refresh()
        {
            CategoryComics.Clear();
            switch (Mode)
            {
                case CategoryMode.Category:
                    await BikaHttpHelper.TryRequest(this, BikaClient.Category(CategoryTitle, Page, Sort), res =>
                    {
                        Pages = res.Data.Comics.Pages;
                        Page = res.Data.Comics.Page;
                        
                        foreach (var comic in res.Data.Comics.Docs)
                        {
                            CheckCategoryLock(comic);
                            if (!(comic.IsLocked && BikaConfig.IsIgnoreLockComic))
                            {
                                CategoryComics.Add(comic);
                            }
                        }
                    });
                    break;
                case CategoryMode.Random:
                    await BikaHttpHelper.TryRequest(this, BikaClient.ComicRandom(), res =>
                    {
                        Pages = 1;
                        Page = 1;
                        foreach (var comic in res.Data.Comics)
                        {
                            CheckCategoryLock(comic);
                            if (!(comic.IsLocked && BikaConfig.IsIgnoreLockComic))
                            {
                                CategoryComics.Add(comic);
                            }
                        }
                    });
                    break;
                case CategoryMode.Search:
                    await BikaHttpHelper.TryRequest(this, BikaClient.AdvancedSearch(CategoryTitle,Page, Sort), res =>
                    {
                        Pages = res.Data.Comics.Pages;
                        Page = res.Data.Comics.Page;
                        foreach (var comic in res.Data.Comics.Docs)
                        {
                            CheckCategoryLock(comic);
                            if (!(comic.IsLocked && BikaConfig.IsIgnoreLockComic))
                            {
                                CategoryComics.Add(comic);
                            }
                        }
                    });
                    break;
            }

        }

        private static void CheckCategoryLock(Comic comic)
        {
            comic.LockCategories = comic.Categories.Where(x => BikaData.Current.Locks.Any(y => y.Title == x && !y.IsOpened)).ToList();
            comic.IsLocked = comic.LockCategories.Count > 0;
        }
        private void SetCurrentPageString()
        {
            CurrentPageString = BikaResourcesHelper.GetString(BikaResourceKey.Number) + $"{Page}/{Pages}" + BikaResourcesHelper.GetString(BikaResourceKey.Page);
        }
        partial void OnPageChanged(int oldValue, int newValue)
        {
            if (!string.IsNullOrEmpty(SortRuleText))
            {
                Refresh();
            }
        }
        partial void OnPagesChanged(int oldValue, int newValue)
        {
            SetCurrentPageString();
        }
        partial void OnSortChanged(SortRule oldValue, SortRule newValue)
        {
            if (oldValue == newValue) return;
            SortRuleText = BikaResourcesHelper.GetString(newValue.ToString().ToUpper());
            if (!string.IsNullOrEmpty(SortRuleText))
            {
                Refresh();
            }
        }
    }
}
