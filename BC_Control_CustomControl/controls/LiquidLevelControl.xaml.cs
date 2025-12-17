using System;
using System.Collections.Generic;
using System.Configuration;
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
    /// LiquidLevelControl.xaml 的交互逻辑
    /// </summary>
    public partial class LiquidLevelControl : UserControl
    {
        public LiquidLevelControl()
        {
            InitializeComponent();
        }
        public double SettingLevel
        {
            get { return (double)GetValue(SettingProperty); }
            set { SetValue(SettingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Radius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SettingProperty = DependencyProperty.Register(
            "SettingLevel",
            typeof(double),
            typeof(LiquidLevelControl),
            new PropertyMetadata(5.0, OnActLevelChanged)


        );
        public double ActLevel
        {
            get { return (double)GetValue(ActLevelProperty); }
            set { SetValue(ActLevelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Radius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActLevelProperty = DependencyProperty.Register(
            "ActLevel",
            typeof(double),
            typeof(LiquidLevelControl),
            new PropertyMetadata(3.0, OnActLevelChanged)
        );
        public double RotationAngle
        {
            get { return (double)GetValue(RotationAngleProperty); }
            set { SetValue(RotationAngleProperty, value); }
        }
        public static readonly DependencyProperty RotationAngleProperty = DependencyProperty.Register(
            "RotationAngle",
            typeof(double),
            typeof(LiquidLevelControl),
            new PropertyMetadata(2.0, OnRotationChange)
        );
        public double ScaleFactor
        {
            get { return (double)GetValue(ScaleFactorProperty); }
            set { SetValue(ScaleFactorProperty, value); }
        }



        public static readonly DependencyProperty ScaleFactorProperty = DependencyProperty.Register(
            "ScaleFactor",
            typeof(double),
            typeof(LiquidLevelControl),
            new PropertyMetadata(1.0, OnScaleChange)
        );
        public static void OnRotationChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LiquidLevelControl LiquidLevelControl = d as LiquidLevelControl;
            LiquidLevelControl.rectRotateTransform.Angle = (double)e.NewValue;
        }
        public static void OnScaleChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LiquidLevelControl LiquidLevelControl = d as LiquidLevelControl;
            LiquidLevelControl.rectScaleTransform.ScaleX = 1.0;
            LiquidLevelControl.rectScaleTransform.ScaleY = (double)e.NewValue;
        }
        private static void OnActLevelChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e
        )
        {
            LiquidLevelControl b = d as LiquidLevelControl;
            b.ScaleFactor = (double)b.ActLevel / b.SettingLevel;
        }
    }
}
