using CustomExtensions.WinUI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using PicaComic;
using ShadowViewer.Plugin.Bika.Args;
using ShadowViewer.Plugin.Bika.ViewModels;
using ShadowViewer.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace ShadowViewer.Plugin.Bika.Pages
{

    public sealed partial class BikaCategoryPage : Page
    {
        private BikaCategoryViewModel ViewModel { get; }
        public BikaCategoryPage()
        {
            this.LoadComponent(ref _contentLoaded);
            ViewModel = new BikaCategoryViewModel();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var arg = e.Parameter as CategoryArg;
            if (arg != null)
            {
                ViewModel.Init(arg);
            }
        }

        private void GridV_ItemClick(object sender, ItemClickEventArgs e)
        {

        }
    }
}
