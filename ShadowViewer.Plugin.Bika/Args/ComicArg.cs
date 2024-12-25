using System.Collections.ObjectModel;
using PicaComic.Models;

namespace ShadowViewer.Plugin.Bika.Args;

public class ComicArg
{
    public ComicArg(ComicInfo comicInfo, int currentEpisode, ObservableCollection<Episode> episodes)
    {
        ComicInfo = comicInfo;
        CurrentEpisode = currentEpisode;
        Episodes = episodes;
    }

    public ComicInfo ComicInfo { get; init; }
    public int CurrentEpisode { get; init; }
    public ObservableCollection<Episode> Episodes { get; init; }
}