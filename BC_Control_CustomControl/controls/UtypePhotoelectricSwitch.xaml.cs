using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BC_Control_CustomControl.Controls
{
    /// <summary>
    /// UtypePhotoelectricSwitch.xaml 的交互逻辑
    /// </summary>
    public partial class UtypePhotoelectricSwitch : UserControl
    {
        public UtypePhotoelectricSwitch()
        {
            InitializeComponent();
        }
        public bool ActState
        {
            get { return (bool)GetValue(ActStateProperty); }
            set { SetValue(ActStateProperty, value); }
        }
        public static readonly DependencyProperty ActStateProperty = DependencyProperty.Register(
            "ActState",
            typeof(bool),
            typeof(UtypePhotoelectricSwitch),
            new PropertyMetadata(false, OnActStateChange)
        );
        public double RotationAngle
        {
            get { return (double)GetValue(RotationAngleProperty); }
            set { SetValue(RotationAngleProperty, value); }
        }
        public static readonly DependencyProperty RotationAngleProperty = DependencyProperty.Register(
            "RotationAngle",
            typeof(double),
            typeof(UtypePhotoelectricSwitch),
            new PropertyMetadata(0.0, OnRotationChange)
        );
        public double ScaleFactor
        {
            get { return (double)GetValue(ScaleFactorProperty); }
            set { SetValue(ScaleFactorProperty, value); }
        }
        public static readonly DependencyProperty ScaleFactorProperty = DependencyProperty.Register(
            "ScaleFactor",
            typeof(double),
            typeof(UtypePhotoelectricSwitch),
            new PropertyMetadata(1.0, OnScaleChange)
        );
        public static void OnRotationChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UtypePhotoelectricSwitch utypePhotoelectricSwitch = d as UtypePhotoelectricSwitch;
            utypePhotoelectricSwitch.rectRotateTransform.Angle = (double)e.NewValue;
        }
        public static void OnScaleChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UtypePhotoelectricSwitch utypePhotoelectricSwitch = d as UtypePhotoelectricSwitch;
            utypePhotoelectricSwitch.rectScaleTransform.ScaleX = (double)e.NewValue;
            utypePhotoelectricSwitch.rectScaleTransform.ScaleY = (double)e.NewValue;
        }
        public static void OnActStateChange(DependencyObject d,DependencyPropertyChangedEventArgs e)
        {
            UtypePhotoelectricSwitch utypePhotoelectricSwitch=d as UtypePhotoelectricSwitch;
            utypePhotoelectricSwitch.SwitchPath.Fill = (bool)e.NewValue == true ? new SolidColorBrush(Colors.Red) : new SolidColorBrush(Colors.Gray);
        }
    }
}
