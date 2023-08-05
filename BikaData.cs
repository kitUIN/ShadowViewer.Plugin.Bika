using System.Collections.ObjectModel;
using PicaComic.Models;

namespace ShadowViewer.Plugin.Bika;

public class BikaData
{
    public Profile CurrentUser { get; set; }
    public ObservableCollection<string> Keywords { get; } = new ObservableCollection<string>();
    public static BikaData Current { get; set; }
}