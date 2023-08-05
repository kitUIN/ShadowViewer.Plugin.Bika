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
    
        private async void GridV_OnLoaded(object sender, RoutedEventArgs e)
        { 
            await ViewModel.GetClassification();
        }

        private void GridV_OnItemClick(object sender, ItemClickEventArgs e)
        {
            
        }
    }
}
