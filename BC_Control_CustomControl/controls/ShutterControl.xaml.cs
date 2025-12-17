using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BC_Control_CustomControl.Controls
{
    /// <summary>
    /// ShutterControl.xaml 的交互逻辑
    /// </summary>
    public partial class ShutterControl : UserControl
    {
        public ShutterControl()
        {
            InitializeComponent();
        }
        public bool OpenSensor
        {
            get { return (bool)GetValue(ActPositionProperty); }
            set { SetValue(ActPositionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Radius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActPositionProperty = DependencyProperty.Register(
            "OpenSensor",
            typeof(bool),
            typeof(ShutterControl),
            new PropertyMetadata(false, OnSensorChanged)
        );
        public bool CloseSensor
        {
            get { return (bool)GetValue(CloseSensorProperty); }
            set { SetValue(CloseSensorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Radius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CloseSensorProperty = DependencyProperty.Register(
            "CloseSensor",
            typeof(bool),
            typeof(ShutterControl),
            new PropertyMetadata(true, OnSensorChanged)
        );

        private static void OnSensorChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e
        )
        {
            ShutterControl b = d as ShutterControl;
            b.Door1.Visibility = b.OpenSensor ? Visibility.Collapsed : Visibility.Visible;
            b.Door2.Visibility = b.CloseSensor ? Visibility.Visible : Visibility.Collapsed;



        }
    }
}
