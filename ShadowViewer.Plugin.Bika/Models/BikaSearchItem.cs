using CommunityToolkit.WinUI.Helpers;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using ShadowViewer.Interfaces;
using ShadowViewer.Plugin.Bika.Enums;
using ShadowViewer.Plugin.Bika.Helpers;

namespace ShadowViewer.Plugin.Bika.Models;


public class BikaSearchItem : IShadowSearchItem
{
    public string Title { get; set; }
    public string SubTitle { get; set; }
    public string Id { get; set; }
    public string ComicId { get; set; }
    public IconSource Icon { get; set; }
    public BikaSearchMode Mode { get; set; }

    public BikaSearchItem(string title, string id, BikaSearchMode mode)
    {
        Title = title;
        Mode = mode;
        switch (mode)
        {
            case BikaSearchMode.Search:
                SubTitle = BikaResourcesHelper.GetString(BikaResourceKey.BikaSearch);
                Icon = new FontIconSource() { Glyph = "\uE773" ,Foreground = new SolidColorBrush("#FFE480A7".ToColor())};
                break;
            case BikaSearchMode.History:
                SubTitle = BikaResourcesHelper.GetString(BikaResourceKey.History);
                break;
            default:
                break;
        }
        Id = id;
    }
}