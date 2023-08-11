using PicaComic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DryIoc;
using Microsoft.Extensions.DependencyInjection;
using PicaComic.Models;
using ShadowViewer.Enums;
using ShadowViewer.Interfaces;
using ShadowViewer.Plugin.Bika.Models;
using SqlSugar;
using ShadowViewer.Plugin.Bika.Controls;

namespace ShadowViewer.Plugin.Bika.ViewModels
{
    public class ClassificationViewModel
    {
        public ObservableCollection<Category> Categories { get; } = new()
        {
            new Category
            {
                Title = BikaResourcesHelper.GetString(BikaResourceKey.Leaderboard),
                Thumb = new PicaComic.Models.Thumb
                {
                    FilePath = @"ms-appx:///ShadowViewer.Plugin.Bika/Assets/Picacgs/cat_leaderboard.jpg"
                }
            },
            new Category
            {
                Title = BikaResourcesHelper.GetString(BikaResourceKey.Random),
                Thumb = new PicaComic.Models.Thumb
                {
                    FilePath = @"ms-appx:///ShadowViewer.Plugin.Bika/Assets/Picacgs/cat_random.jpg"
                }
            },
            new Category
            {
                Title = BikaResourcesHelper.GetString(BikaResourceKey.Latest),
                Thumb = new PicaComic.Models.Thumb
                {
                    FilePath = @"ms-appx:///ShadowViewer.Plugin.Bika/Assets/Picacgs/cat_latest.jpg"
                }
            }
        };

        public static ClassificationViewModel Current { get; } = new();
        private ICallableToolKit Caller { get; } = DiFactory.Services.Resolve<ICallableToolKit>();

        public async Task GetClassification()
        {
            if (Categories.Count <= 5)
            {
                await BikaHttpHelper.TryRequest(this, PicaClient.Categories(), res =>
                {
                    foreach (var item in res.Data.Categories)
                    {
                        Categories.Add(item);
                    }
                });
            }
        }
    }
}