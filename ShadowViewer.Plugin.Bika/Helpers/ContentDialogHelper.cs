using CustomExtensions.WinUI;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Serilog;
using ShadowViewer.Enums;
using ShadowViewer.Helpers;
using ShadowViewer.Plugin.Bika.Enums;

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
                CloseButtonText = BikaResourcesHelper.GetString(BikaResourceKey.Confirm),
            };
            switch (status)
            {
                case BikaHttpStatus.TimeOut:
                    img.Source = "/Assets/Picacgs/icon_unknown_error.png".ImageSource(typeof(ContentDialogHelper));
                    title.Text = BikaResourcesHelper.GetString(BikaResourceKey.TimeOut);
                    var stack = new StackPanel()
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                    };
                    stack.Children.Add(new TextBlock
                    {
                        Text = BikaResourcesHelper.GetString(BikaResourceKey.TimeOutMessage1),
                        FontWeight = FontWeights.SemiBold,
                        HorizontalAlignment = HorizontalAlignment.Center,
                    });
                    stack.Children.Add(new TextBlock
                    {
                        Text = BikaResourcesHelper.GetString(BikaResourceKey.TimeOutMessage2),
                        FontWeight = FontWeights.SemiBold,
                        HorizontalAlignment = HorizontalAlignment.Center,
                    });
                    stack.Children.Add(new TextBlock
                    {
                        Text = BikaResourcesHelper.GetString(BikaResourceKey.TimeOutMessage3),
                        FontWeight = FontWeights.SemiBold,
                        HorizontalAlignment = HorizontalAlignment.Center,
                    });
                    dialog.Content = stack;
                    break;
                case BikaHttpStatus.NoAuth:
                    img.Source = "/Assets/Picacgs/icon_exclamation_error.png".ImageSource(typeof(ContentDialogHelper));
                    title.Text = BikaResourcesHelper.GetString(BikaResourceKey.NoAuth);
                    dialog.Content = new TextBlock
                    {
                        Text = BikaResourcesHelper.GetString(BikaResourceKey.NoAuthMessage),
                        FontWeight = FontWeights.Bold,
                        HorizontalAlignment = HorizontalAlignment.Center,
                    };
                    break;
                case BikaHttpStatus.Unknown:
                    title.Text = BikaResourcesHelper.GetString(BikaResourceKey.Unknown);
                    img.Source = "/Assets/Picacgs/icon_unknown_error.png".ImageSource(typeof(ContentDialogHelper)); 
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