using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BC_Control_CustomControl.Controls
{
    /// <summary>
    /// TankWaferControl.xaml 的交互逻辑
    /// </summary>
    public partial class TankWaferControl : UserControl
    {
        public TankWaferControl()
        {
            InitializeComponent();
        }


        #region MyRegion


        public static readonly DependencyProperty Status2Property =
           DependencyProperty.Register("Status2", typeof(WaferStatus), typeof(TankWaferControl),
               new PropertyMetadata(WaferStatus.Empty, OnStatusChanged));

        public static readonly DependencyProperty IsWaferVisibleProperty =
            DependencyProperty.Register("IsWaferVisible2", typeof(bool), typeof(TankWaferControl),
                new PropertyMetadata(true, OnIsWaferVisibleChanged));

        public static readonly DependencyProperty WaferColorProperty =
            DependencyProperty.Register("WaferColor2", typeof(Brush), typeof(TankWaferControl),
                new PropertyMetadata(Brushes.Transparent));

        public WaferStatus Status2
        {
            get => (WaferStatus)GetValue(Status2Property);
            set => SetValue(Status2Property, value);
        }

        public bool IsWaferVisible2
        {
            get => (bool)GetValue(IsWaferVisibleProperty);
            set => SetValue(IsWaferVisibleProperty, value);
        }

        public Brush WaferColor2
        {
            get => (Brush)GetValue(WaferColorProperty);
            set => SetValue(WaferColorProperty, value);
        }
        #endregion


        private static void OnStatusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as TankWaferControl;
            if (control != null)
            {
                control.WaferColor2 = control.GetStatusColor2((WaferStatus)e.NewValue);

                //control.ww123.
            }
        }

        private static void OnIsWaferVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // 不需要额外处理，绑定会自动更新
        }

        private Brush GetStatusColor2(WaferStatus status2)
        {
            switch (status2)
            {
                case WaferStatus.Processing:

                    return Brushes.Cyan;
                case WaferStatus.Ready: return Brushes.Yellow;
                case WaferStatus.Completed: return Brushes.Red;
                case WaferStatus.Error: return Brushes.Red;
                default:
                    return Brushes.Transparent;
            }
        }
    }
}
