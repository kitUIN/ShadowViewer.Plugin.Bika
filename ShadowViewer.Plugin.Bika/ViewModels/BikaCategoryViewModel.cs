using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PicaComic;
using PicaComic.Models;
using PicaComic.Utils;
using ShadowViewer.Plugin.Bika.Enums;
using ShadowViewer.Plugin.Bika.Helpers;
using ShadowViewer.Plugin.Bika.I18n;

namespace ShadowViewer.Plugin.Bika.ViewModels
{
    public partial class BikaCategoryViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CurrentPageString))]
        [NotifyCanExecuteChangedFor(nameof(PreviousPageCommand))]
        [NotifyCanExecuteChangedFor(nameof(NextPageCommand))]
        private int pages = 1;

        [ObservableProperty] private bool isGotoOpen;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Index))]
        [NotifyPropertyChangedFor(nameof(CurrentPageString))]
        [NotifyCanExecuteChangedFor(nameof(PreviousPageCommand))]
        [NotifyCanExecuteChangedFor(nameof(NextPageCommand))]
        private int page = 1;

        public int Index
        {
            get => Page - 1;
            set => Page = value + 1;
        }

        [ObservableProperty] private string categoryTitle = "";

        public string CurrentPageString => I18N.Number + $"{Page}/{Pages}" + I18N.Page;
        [ObservableProperty] private string sortRuleText;
        [ObservableProperty] private SortRule sort = SortRule.dd;
        public ObservableCollection<CategoryComic> CategoryComics { get; } = new();
        public CategoryMode Mode { get; set; }
        private IPicaClient BikaClient { get; }

        public BikaCategoryViewModel(IPicaClient bikaClient)
        {
            BikaClient = bikaClient;
            SortRuleText = ResourcesHelper.GetString(Sort.ToString().ToUpper());
        }

        public void CheckAllCategoryComicLock(object? sender, EventArgs e)
        {
            foreach (var comic in CategoryComics)
            {
                CheckCategoryLock(comic);
            }
        }

        private bool CanNextPageExecute() => Page + 1 <= Pages;
        private bool CanPreviousPageExecute() => Page - 1 > 0;

        [RelayCommand(CanExecute = nameof(CanNextPageExecute))]
        private void NextPage()
        {
            if (Page + 1 <= Pages)
            {
                Page += 1;
            }
        }

        [RelayCommand]
        private void CurrentPage()
        {
            IsGotoOpen = true;
        }

        [RelayCommand]
        private void Goto(double go)
        {
            if (go <= Pages && go >= 1)
            {
                Page = (int)go;
                IsGotoOpen = false;
            }
            else
            {
                //NotificationHelper.Notify(this,ResourcesHelper.GetString(ResourceKey.WarnPage),
                //    InfoBarSeverity.Warning);
            }
        }

        [RelayCommand(CanExecute = nameof(CanPreviousPageExecute))]
        private void PreviousPage()
        {
            if (Page - 1 > 0)
            {
                Page -= 1;
            }
        }

        [RelayCommand]
        private void RefreshButton()
        {
            Refresh();
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
                            if (!(comic.IsLocked && BikaPlugin.Settings.IsIgnoreLockComic))
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
                            if (!(comic.IsLocked && BikaPlugin.Settings.IsIgnoreLockComic))
                            {
                                CategoryComics.Add(comic);
                            }
                        }
                    });
                    break;
                case CategoryMode.Search:
                    await BikaHttpHelper.TryRequest(this, BikaClient.AdvancedSearch(CategoryTitle, Page, Sort), res =>
                    {
                        Pages = res.Data.Comics.Pages;
                        Page = res.Data.Comics.Page;
                        foreach (var comic in res.Data.Comics.Docs)
                        {
                            CheckCategoryLock(comic);
                            if (!(comic.IsLocked && BikaPlugin.Settings.IsIgnoreLockComic))
                            {
                                CategoryComics.Add(comic);
                            }
                        }
                    });
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void CheckCategoryLock(Comic comic)
        {
            comic.LockCategories = comic.Categories
                .Where(x => BikaData.Current.Locks.Any(y => y.Title == x && !y.IsOpened)).ToList();
            comic.IsLocked = comic.LockCategories.Count > 0;
        }

        partial void OnPageChanged(int oldValue, int newValue)
        {
            if (!string.IsNullOrEmpty(SortRuleText))
            {
                Refresh();
            }
        }

        partial void OnSortChanged(SortRule oldValue, SortRule newValue)
        {
            if (oldValue == newValue) return;
            SortRuleText = ResourcesHelper.GetString(newValue.ToString().ToUpper());
            if (!string.IsNullOrEmpty(SortRuleText))
            {
                Refresh();
            }
        }
    }
}