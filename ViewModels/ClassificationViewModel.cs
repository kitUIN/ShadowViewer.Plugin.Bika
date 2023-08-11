using System.Collections.ObjectModel;
using System.Threading.Tasks;
using PicaComic;
using PicaComic.Models;
using PicaComic.Responses;
using ShadowViewer.Interfaces;
using ShadowViewer.Plugin.Bika.Enums;
using ShadowViewer.Plugin.Bika.Helpers;
using Thumb = PicaComic.Models.Thumb;

namespace ShadowViewer.Plugin.Bika.ViewModels;

public class ClassificationViewModel
{
    private IPicaClient BikaClient { get; }
    public ClassificationViewModel(ICallableService caller,IPicaClient client)
    {
        Caller = caller;
        BikaClient = client;
    }

    public ObservableCollection<Category> Categories { get; } = new()
    {
        new Category
        {
            Title = BikaResourcesHelper.GetString(BikaResourceKey.Leaderboard),
            Thumb = new Thumb
            {
                FilePath = @"ms-appx:///ShadowViewer.Plugin.Bika/Assets/Picacgs/cat_leaderboard.jpg"
            }
        },
        new Category
        {
            Title = BikaResourcesHelper.GetString(BikaResourceKey.Random),
            Thumb = new Thumb
            {
                FilePath = @"ms-appx:///ShadowViewer.Plugin.Bika/Assets/Picacgs/cat_random.jpg"
            }
        },
        new Category
        {
            Title = BikaResourcesHelper.GetString(BikaResourceKey.Latest),
            Thumb = new Thumb
            {
                FilePath = @"ms-appx:///ShadowViewer.Plugin.Bika/Assets/Picacgs/cat_latest.jpg"
            }
        }
    };

    private ICallableService Caller { get; }

    public async Task GetClassification()
    {
        if (Categories.Count <= 5)
            await BikaHttpHelper.TryRequest(this, BikaClient.Categories(), res =>
            {
                foreach (var item in res.Data.Categories) Categories.Add(item);
            });
    }
}