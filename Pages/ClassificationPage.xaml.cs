using CustomExtensions.WinUI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using ShadowViewer.Plugin.Bika.ViewModels;
using PicaComic.Models;
using Microsoft.UI.Xaml.Media.Animation;
using ShadowViewer.Plugin.Bika.Args;
using PicaComic;
using ShadowViewer.Enums;
using ShadowViewer.Plugin.Bika.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ShadowViewer.Plugin.Bika.Pages
{
 
    public sealed partial class ClassificationPage : Page
    {
        private ClassificationViewModel ViewModel { get; }
        public ClassificationPage()
        {
            this.LoadComponent(ref _contentLoaded);
            ClassificationViewModel.Current ??= new ClassificationViewModel();
            ViewModel = ClassificationViewModel.Current;
        }
    
        private void GridV_OnLoaded(object sender, RoutedEventArgs e)
        { 
            
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await ViewModel.GetClassification();
            
           
        }
        private void GridV_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var category = (Category)e.ClickedItem;
            if (!category.IsWeb)
            {
                Frame.Navigate(typeof(BikaCategoryPage), new CategoryArg()
                {
                    Category = category.Title,

                },
                    new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
            }
        }
    }
}
