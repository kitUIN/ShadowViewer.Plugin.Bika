using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using ShadowViewer.Plugin.Local.Models.Interfaces;


namespace ShadowViewer.Plugin.Bika.Models;
 
public partial class BikaPicture : ObservableObject, IUiPicture
{
    [ObservableProperty] private int index;
    [ObservableProperty] private ImageSource source;


    public BikaPicture(int index, BitmapImage image)
    {
        Index = index;
        Source = image;
    }

    public BikaPicture(int index, Uri uri) : this(index, new BitmapImage() { UriSource = uri })
    {
    }

    public BikaPicture(int index, string uri) : this(index, new BitmapImage() { UriSource = new Uri(uri) })
    {
    }

    public object Tag { get; set; }
}