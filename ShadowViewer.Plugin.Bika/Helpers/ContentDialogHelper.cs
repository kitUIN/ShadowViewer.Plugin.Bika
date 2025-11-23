using System;
using CustomExtensions.WinUI;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Serilog;
using ShadowViewer.Plugin.Bika.Enums;
using ShadowViewer.Plugin.Bika.I18n;

namespace ShadowViewer.Plugin.Bika.Helpers
{
    public static class ContentDialogHelper
    {
        public static ContentDialog CreateHttpDialog(BikaHttpStatus status, string messages)
        {
            Log.Error("{Status}:{Messages}", status, messages);
            var img = new Image
            {
                Height = 250,
                Width = 250,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            var stackPanel = new StackPanel()
            {
                Orientation = Orientation.Vertical,
            };
            var title = new TextBlock()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                FontWeight = FontWeights.Bold,
                TextWrapping = TextWrapping.Wrap,
            };
            stackPanel.Children.Add(title);
            stackPanel.Children.Add(img);
            var dialog = new ContentDialog()
            {
                DefaultButton = ContentDialogButton.Primary,
                Title = stackPanel,
                CloseButtonText = I18N.Confirm,
            };
            switch (status)
            {
                case BikaHttpStatus.TimeOut:
                    img.Source = new BitmapImage(new Uri("ms-plugin://ShadowViewer.Plugin.Bika/Assets/Picacgs/icon_unknown_error.png".PluginPath())) ;
                    title.Text = I18N.TimeOut;
                    var stack = new StackPanel()
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                    };
                    stack.Children.Add(new TextBlock
                    {
                        Text = I18N.TimeOutMessage1,
                        FontWeight = FontWeights.SemiBold,
                        HorizontalAlignment = HorizontalAlignment.Center,
                    });
                    stack.Children.Add(new TextBlock
                    {
                        Text = I18N.TimeOutMessage2,
                        FontWeight = FontWeights.SemiBold,
                        HorizontalAlignment = HorizontalAlignment.Center,
                    });
                    stack.Children.Add(new TextBlock
                    {
                        Text = I18N.TimeOutMessage3,
                        FontWeight = FontWeights.SemiBold,
                        HorizontalAlignment = HorizontalAlignment.Center,
                    });
                    dialog.Content = stack;
                    break;
                case BikaHttpStatus.NoAuth:
                    img.Source = new BitmapImage(new Uri("ms-plugin://ShadowViewer.Plugin.Bika/Assets/Picacgs/icon_exclamation_error.png".PluginPath()));
                    title.Text = I18N.NoAuth;
                    dialog.Content = new TextBlock
                    {
                        Text = I18N.NoAuthMessage,
                        FontWeight = FontWeights.Bold,
                        HorizontalAlignment = HorizontalAlignment.Center,
                    };
                    break;
                case BikaHttpStatus.Unknown:
                    title.Text = I18N.Unknown;
                    img.Source = new BitmapImage(new Uri("ms-plugin://ShadowViewer.Plugin.Bika/Assets/Picacgs/icon_unknown_error.png".PluginPath()));
                    dialog.Content = new TextBlock
                    {
                        TextWrapping = TextWrapping.Wrap,
                        Text = messages,
                        FontWeight = FontWeights.Bold,
                        HorizontalAlignment = HorizontalAlignment.Center,
                    };
                    break;
                default:
                    return null;
            }

            return dialog;
        }
    }
}