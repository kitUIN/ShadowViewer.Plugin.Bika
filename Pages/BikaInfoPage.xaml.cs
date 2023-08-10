using CustomExtensions.WinUI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using ShadowViewer.Plugin.Bika.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ShadowViewer.Plugin.Bika.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BikaInfoPage : Page
    {
        public BikaInfoViewModel ViewModel { get; } = new();
        public BikaInfoPage()
        {
            this.LoadComponent(ref _contentLoaded);
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is not string id) return;
            ViewModel.ComicId = id;
            ViewModel.Refresh();
        }

        private void Border_OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (sender is Border border)
            {
                border.Background = Application.Current.Resources["HyperlinkButtonBackgroundPointerOver"] as Brush;
            }
        }
        private void Border_OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (sender is Border border)
            {
                border.Background = new SolidColorBrush(Colors.Transparent);
            }
        }
    }
}
