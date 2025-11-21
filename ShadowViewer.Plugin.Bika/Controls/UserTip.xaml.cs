using CustomExtensions.WinUI;
using DryIoc;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PicaComic.Models;
using ShadowPluginLoader.WinUI;
using ShadowViewer.Sdk.Services;
using ShadowViewer.Plugin.Bika.Args;
using ShadowViewer.Plugin.Bika.Enums;
using ShadowViewer.Plugin.Bika.Pages;

namespace ShadowViewer.Plugin.Bika.Controls
{
    public sealed partial class UserTip : UserControl
    {
        public UserTip()
        {
            this.LoadComponent(ref _contentLoaded);
        }
        public void Show()
        {
            CurrentUserTip.IsOpen = true;
        }
        public void Hide()
        {
            CurrentUserTip.IsOpen = false;
        }
        public Creater CurrentUser
        {
            get { return (Creater)GetValue(CurrentUserProperty); }
            set { SetValue(CurrentUserProperty, value); }
        }

        public static readonly DependencyProperty CurrentUserProperty =
            DependencyProperty.Register(nameof(CurrentUser), typeof(Creater), typeof(UserTip), new PropertyMetadata(default, new PropertyChangedCallback(OnCurrentUserChanged)));
        private static void OnCurrentUserChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UserTip control = (UserTip)d;
            control.CurrentUser = (Creater)e.NewValue;
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is HyperlinkButton { Content: string tag })
                DiFactory.Services.Resolve<INavigateService>().Navigate(typeof(BikaCategoryPage),
                    new CategoryArg
                    {
                        Category = tag,
                        Mode = CategoryMode.Search
                    });
        }
    }
}
