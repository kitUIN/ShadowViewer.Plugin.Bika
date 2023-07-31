using CustomExtensions.WinUI;

namespace ShadowViewer.Plugin.Bika.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BikaSettingsPage : Page
    {
        public BikaSettingsPage()
        {
            this.LoadComponent(ref _contentLoaded);
        }
    }
}
