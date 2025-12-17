using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace BC_Control_CustomControl
{
    public static class ActStateBackgroundBehavior
    {
        #region ActState 属性

        public static readonly DependencyProperty ActStateProperty =
            DependencyProperty.RegisterAttached(
                "ActState",
                typeof(bool),
                typeof(ActStateBackgroundBehavior),
                new PropertyMetadata(false, OnActStateChanged)
            );

        public static bool GetActState(DependencyObject obj) => (bool)obj.GetValue(ActStateProperty);
        public static void SetActState(DependencyObject obj, bool value) => obj.SetValue(ActStateProperty, value);

        #endregion

        #region 可配置的颜色属性

        public static readonly DependencyProperty ActiveBackgroundProperty =
            DependencyProperty.RegisterAttached(
                "ActiveBackground",
                typeof(Brush),
                typeof(ActStateBackgroundBehavior),
                new PropertyMetadata(Brushes.Green)
            );

        public static Brush GetActiveBackground(DependencyObject obj) => (Brush)obj.GetValue(ActiveBackgroundProperty);
        public static void SetActiveBackground(DependencyObject obj, Brush value) => obj.SetValue(ActiveBackgroundProperty, value);

        public static readonly DependencyProperty InactiveBackgroundProperty =
            DependencyProperty.RegisterAttached(
                "InactiveBackground",
                typeof(Brush),
                typeof(ActStateBackgroundBehavior),
                new PropertyMetadata(Brushes.Red)
            );

        public static Brush GetInactiveBackground(DependencyObject obj) => (Brush)obj.GetValue(InactiveBackgroundProperty);
        public static void SetInactiveBackground(DependencyObject obj, Brush value) => obj.SetValue(InactiveBackgroundProperty, value);

        #endregion

        #region 属性变更处理

        private static void OnActStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UpdateElementBackground(d);
        }

        private static void UpdateElementBackground(DependencyObject d)
        {
            var isActive = GetActState(d);
            var activeBrush = GetActiveBackground(d);
            var inactiveBrush = GetInactiveBackground(d);

            var brush = isActive ? activeBrush : inactiveBrush;

            switch (d)
            {
                case Control control:
                    control.Background = brush;
                    break;
                case Panel panel:
                    panel.Background = brush;
                    break;
                case Border border:
                    border.Background = brush;
                    break;
                case TextBlock textBlock:
                    textBlock.Background = brush;
                    break;
            }
        }

        #endregion
    }
}