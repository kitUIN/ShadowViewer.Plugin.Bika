using CommunityToolkit.Mvvm.ComponentModel;
using PicaComic.Models;
using ShadowViewer.Interfaces;

namespace ShadowViewer.Plugin.Bika.Models;

public partial class BikaEpisode : ObservableObject, IShadowEpisode
{
    [ObservableProperty] private string title;
    public string ComicId { get; set; }
    public int Order { get; set; }
    public BikaEpisode(Episode episode,string comicId)
    {
        Title = episode.Title;
        Order = episode.Order;
        ComicId = comicId;
    }
}