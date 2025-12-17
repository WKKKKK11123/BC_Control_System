using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;


namespace BC_Control_CustomControl.Controls
{
    /// <summary>
    /// WTRRobotControl.xaml 的交互逻辑
    /// </summary>
    public partial class WTRRobotControl : UserControl
    {
        public WTRRobotControl()
        {
            InitializeComponent();
            //DataContext = this;
        }
        public int ActPosition
        {
            get { return (int)GetValue(ActPositionProperty); }
            set { SetValue(ActPositionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Radius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActPositionProperty = DependencyProperty.Register(
            "ActPosition",
            typeof(int),
            typeof(WTRRobotControl),
            new PropertyMetadata(1, OnActPositionChanged)
        );
        public int SetPosition
        {
            get { return (int)GetValue(SetPositionProperty); }
            set { SetValue(SetPositionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Radius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SetPositionProperty = DependencyProperty.Register(
            "SetPosition",
            typeof(int),
            typeof(WTRRobotControl),
            new PropertyMetadata(1, OnActPositionChanged)
        );

        #region MyRegion

        public static readonly DependencyProperty StatusProperty =
           DependencyProperty.Register("Status", typeof(WaferStatus), typeof(WTRRobotControl),
               new PropertyMetadata(WaferStatus.Empty, OnStatusChanged));

        public static readonly DependencyProperty IsWaferVisibleProperty =
            DependencyProperty.Register("IsWaferVisible", typeof(bool), typeof(WTRRobotControl),
                new PropertyMetadata(true, OnIsWaferVisibleChanged));

        public static readonly DependencyProperty WaferColorProperty =
            DependencyProperty.Register("WaferColor", typeof(Brush), typeof(WTRRobotControl),
                new PropertyMetadata(Brushes.Transparent));
        public WaferStatus Status
        {
            get => (WaferStatus)GetValue(StatusProperty);
            set => SetValue(StatusProperty, value);
        }
        public bool IsWaferVisible
        {
            get => (bool)GetValue(IsWaferVisibleProperty);
            set => SetValue(IsWaferVisibleProperty, value);
        }
        public Brush WaferColor
        {
            get => (Brush)GetValue(WaferColorProperty);
            set => SetValue(WaferColorProperty, value);
        }
        #endregion

        private static void OnActPositionChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e
        )
        {
            WTRRobotControl b = d as WTRRobotControl;
            //b.Height111 = (double)e.NewValue;
            //b.bor1.Height = (b.ActualHeight - b.ActualHeight * 0.07) / 100.0 * (double)e.NewValue;
            double dd = b.bath1.ActualWidth * (int)e.NewValue / b.SetPosition;
            double time = Math.Abs(((int)e.NewValue - (int)e.OldValue)) * 2.5;
            DoubleAnimation animation = new DoubleAnimation
            {
                To = dd,   // 结束值
                Duration = new Duration(TimeSpan.FromSeconds(time))  // 动画持续时间
            };
            b.rectTransform.BeginAnimation(TranslateTransform.XProperty, animation);
            //b.bor1.BeginAnimation(Border.HeightProperty, animation);
        }
        private static void OnStatusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as WTRRobotControl;
            if (control != null)
            {
                control.WaferColor = control.GetStatusColor((WaferStatus)e.NewValue);
                //control.ww123.
            }
        }
        private static void OnIsWaferVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // 不需要额外处理，绑定会自动更新
        }
        private Brush GetStatusColor(WaferStatus status)
        {
            switch (status)
            {
                case WaferStatus.Processing: return Brushes.Cyan;
                case WaferStatus.Ready: return Brushes.Yellow;
                case WaferStatus.Completed: return Brushes.Red;
                case WaferStatus.Error: return Brushes.Red;
                default: return Brushes.Transparent;
            }
        }
    }
}
