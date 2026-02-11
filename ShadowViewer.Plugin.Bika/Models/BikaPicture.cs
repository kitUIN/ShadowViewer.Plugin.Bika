using CommunityToolkit.Mvvm.ComponentModel;
using ShadowViewer.Plugin.Local.Models.Interfaces;


namespace ShadowViewer.Plugin.Bika.Models;

public partial class BikaPicture : ObservableObject, IUiPicture
{
    [ObservableProperty] public partial int Index { get; set; }

    [ObservableProperty] public partial object Tag { get; set; }
    [ObservableProperty] public partial string SourcePath { get; set; }


    public BikaPicture(int index, string uri)
    {
        SourcePath = uri;
        Index = index;
    }
}