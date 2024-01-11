using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using CommunityToolkit.Mvvm.ComponentModel;
using ShadowViewer.Controls;
using Microsoft.UI.Xaml.Hosting;
using CustomExtensions.WinUI;
using ShadowViewer.Plugin.Bika.Helpers;
using ShadowViewer.Plugin.Bika.Enums;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ShadowViewer.Plugin.Bika.Controls
{
    public sealed partial class LockTip : UserControl
    {
        public event EventHandler LockChangedEvenet;

        public string LockText
        {
            get { return (string)GetValue(LockTextProperty); }
            set { SetValue(LockTextProperty, value); }
        }

        public static readonly DependencyProperty LockTextProperty =
            DependencyProperty.Register(nameof(LockText), typeof(string), typeof(LockTip), new PropertyMetadata(string.Empty));
        private int current = 0;
        private int total = BikaData.Categories.Count;
        public LockTip()
        {
            this.LoadComponent(ref _contentLoaded);
        }
        private void SetLockText()
        {
            if(BikaData.Current!=null)
            {
                current = BikaData.Current.Locks.Where(x => !x.IsOpened).Count();
                total = BikaData.Current.Locks.Count;
                LockText = $"{ResourcesHelper.GetString(ResourceKey.LockCounts)}:{current}/{total}";
            }
        }
        public void Show()
        {
            Lock.IsOpen = true;
            SetLockText();
        }
        public void Hide() 
        { 
            Lock.IsOpen = false; 
        }
        private void OnElementPrepared(ItemsRepeater sender, ItemsRepeaterElementPreparedEventArgs args)
        {
            var item = ElementCompositionPreview.GetElementVisual(args.Element);
            var svVisual = ElementCompositionPreview.GetElementVisual(Animated_ScrollViewer);
            var scrollProperties = ElementCompositionPreview.GetScrollViewerManipulationPropertySet(Animated_ScrollViewer);
            var scaleExpresion = scrollProperties.Compositor.CreateExpressionAnimation();
            scaleExpresion.SetReferenceParameter("svVisual", svVisual);
            scaleExpresion.SetReferenceParameter("scrollProperties", scrollProperties);
            scaleExpresion.SetReferenceParameter("item", item);
            // Scale the item based on the distance of the item relative to the center of the viewport.
            scaleExpresion.Expression = "1 - abs((svVisual.Size.Y/2 - scrollProperties.Translation.Y) - (item.Offset.Y + item.Size.Y / 2))*(.25 / (svVisual.Size.Y / 2))";
            // Animate the item based on its distance to the center of the viewport.
            item.StartAnimation("Scale.X", scaleExpresion);
            item.StartAnimation("Scale.Y", scaleExpresion);
            var centerPointExpression = scrollProperties.Compositor.CreateExpressionAnimation();
            centerPointExpression.SetReferenceParameter("item", item);
            centerPointExpression.Expression = "Vector3(item.Size.X/2, item.Size.Y/2, 0)";
            item.StartAnimation("CenterPoint", centerPointExpression);
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            SetLockText();
            if (BikaConfig.LoadLockComic)
            {
                LockChangedEvenet?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
