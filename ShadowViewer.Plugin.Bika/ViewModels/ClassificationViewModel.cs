using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PicaComic;
using PicaComic.Models;
using PicaComic.Responses;
using ShadowViewer.Interfaces;
using ShadowViewer.Plugin.Bika.Enums;
using ShadowViewer.Plugin.Bika.Helpers;
using Thumb = PicaComic.Models.Thumb;
using CustomExtensions.WinUI;
using ShadowViewer.Services;
namespace ShadowViewer.Plugin.Bika.ViewModels;

public partial class ClassificationViewModel:ObservableObject
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
            Title = ResourcesHelper.GetString(ResourceKey.Leaderboard),
            Thumb = new Thumb
            {
                FilePath = "/Assets/Picacgs/cat_leaderboard.jpg".AssetPath(typeof(BikaPlugin)),
            }
        },
        new Category
        {
            Title = ResourcesHelper.GetString(ResourceKey.Random),
            Thumb = new Thumb
            {
                FilePath = "/Assets/Picacgs/cat_random.jpg".AssetPath(typeof(BikaPlugin)),
            }
        },
        new Category
        {
            Title = ResourcesHelper.GetString(ResourceKey.Latest),
            Thumb = new Thumb
            {
                FilePath = "/Assets/Picacgs/cat_latest.jpg".AssetPath(typeof(BikaPlugin)),
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