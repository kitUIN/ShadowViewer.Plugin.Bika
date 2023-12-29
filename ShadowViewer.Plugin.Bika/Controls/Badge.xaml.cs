using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace ShadowViewer.Controls
{
    public sealed partial class Badge : UserControl
    {
        public Badge()
        {
            this.InitializeComponent();
            
        }
        DependencyProperty LeftTitleProperty = DependencyProperty.Register(
                nameof(LeftTitle),
                typeof(string),
                typeof(Badge),
                new PropertyMetadata(null, new PropertyChangedCallback(OnLeftTitleSoureChanged)));
        public string LeftTitle
        {
            get => (string)GetValue(LeftTitleProperty);
            set => SetValue(LeftTitleProperty, value);
        }
        private static void OnLeftTitleSoureChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Badge control = (Badge)d;
            control.LeftTitle = (string)e.NewValue;
        }
        DependencyProperty RightTitleProperty = DependencyProperty.Register(
                nameof(RightTitle),
                typeof(string),
                typeof(Badge),
                new PropertyMetadata(null, new PropertyChangedCallback(OnRightTitleSoureChanged)));
        public string RightTitle
        {
            get => (string)GetValue(RightTitleProperty);
            set => SetValue(RightTitleProperty, value);
        }
        private static void OnRightTitleSoureChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Badge control = (Badge)d;
            control.RightTitle = (string)e.NewValue;
        }
        DependencyProperty RightBackgroundProperty = DependencyProperty.Register(
                nameof(RightBackground),
                typeof(Brush),
                typeof(Badge),
                new PropertyMetadata(new SolidColorBrush(Colors.Transparent), new PropertyChangedCallback(OnRightBackgroundSoureChanged)));
        public Brush RightBackground
        {
            get => (Brush)GetValue(RightBackgroundProperty);
            set => SetValue(RightBackgroundProperty, value);
        }
        private static void OnRightBackgroundSoureChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Badge control = (Badge)d;
            control.RightBackground = (Brush)e.NewValue;
        }
        DependencyProperty LeftBackgroundProperty = DependencyProperty.Register(
                nameof(LeftBackground),
                typeof(Brush),
                typeof(Badge),
                new PropertyMetadata(new SolidColorBrush(Colors.Transparent), new PropertyChangedCallback(OnLeftBackgroundSoureChanged)));
        public Brush LeftBackground
        {
            get => (Brush)GetValue(LeftBackgroundProperty);
            set => SetValue(LeftBackgroundProperty, value);
        }
        private static void OnLeftBackgroundSoureChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Badge control = (Badge)d;
            control.LeftBackground = (Brush)e.NewValue;
        }

        DependencyProperty RightForegroundProperty = DependencyProperty.Register(
                nameof(RightForeground),
                typeof(Brush),
                typeof(Badge),
                new PropertyMetadata(new SolidColorBrush(Colors.White), new PropertyChangedCallback(OnRightForegroundSoureChanged)));
        public Brush RightForeground
        {
            get => (Brush)GetValue(RightForegroundProperty);
            set => SetValue(RightForegroundProperty, value);
        }
        private static void OnRightForegroundSoureChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Badge control = (Badge)d;
            control.RightForeground = (Brush)e.NewValue;
        }
        DependencyProperty LeftForegroundProperty = DependencyProperty.Register(
                nameof(LeftForeground),
                typeof(Brush),
                typeof(Badge),
                new PropertyMetadata(new SolidColorBrush(Colors.White), new PropertyChangedCallback(OnLeftForegroundSoureChanged)));
        public Brush LeftForeground
        {
            get => (Brush)GetValue(LeftForegroundProperty);
            set => SetValue(LeftForegroundProperty, value);
        }
        private static void OnLeftForegroundSoureChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Badge control = (Badge)d;
            control.LeftForeground = (Brush)e.NewValue;
        }
    }
}
