using Microsoft.UI.Xaml.Controls;
using CustomExtensions.WinUI;

namespace ShadowViewer.Plugin.Bika.Controls
{
    public sealed partial class EmptyTip : UserControl
    {
        public EmptyTip()
        {
            this.LoadComponent(ref _contentLoaded);
        }
    }
}
