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
    /// CondenserLidControl.xaml 的交互逻辑
    /// </summary>
    public partial class CondenserLidControl : UserControl
    {

        public CondenserLidControl()
        {
            InitializeComponent();
        }
        #region 依赖属性

        public bool IsOpen
        {
            get => (bool)GetValue(IsOpenProperty);
            set => SetValue(IsOpenProperty, value);
        }

        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register("IsOpen", typeof(bool), typeof(CondenserLidControl),
                new PropertyMetadata(false, OnIsOpenChanged));

        // 新增两个属性用于绑定打开和关闭的地址状态
        public bool OpenSignal
        {
            get => (bool)GetValue(OpenSignalProperty);
            set => SetValue(OpenSignalProperty, value);
        }

        public static readonly DependencyProperty OpenSignalProperty =
            DependencyProperty.Register("OpenSignal", typeof(bool), typeof(CondenserLidControl),
                new PropertyMetadata(false, OnSignalChanged));

        public bool CloseSignal
        {
            get => (bool)GetValue(CloseSignalProperty);
            set => SetValue(CloseSignalProperty, value);
        }

        public static readonly DependencyProperty CloseSignalProperty =
            DependencyProperty.Register("CloseSignal", typeof(bool), typeof(CondenserLidControl),
                new PropertyMetadata(false, OnSignalChanged));

        public double OpenAngle
        {
            get => (double)GetValue(OpenAngleProperty);
            set => SetValue(OpenAngleProperty, value);
        }

        public static readonly DependencyProperty OpenAngleProperty =
            DependencyProperty.Register("OpenAngle", typeof(double), typeof(CondenserLidControl),
                new PropertyMetadata(90.0));

        public double AnimationDuration
        {
            get => (double)GetValue(AnimationDurationProperty);
            set => SetValue(AnimationDurationProperty, value);
        }

        public static readonly DependencyProperty AnimationDurationProperty =
            DependencyProperty.Register("AnimationDuration", typeof(double), typeof(CondenserLidControl),
                new PropertyMetadata(1.0));

        #endregion

        private static void OnIsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CondenserLidControl control)
            {
                control.ToggleLid((bool)e.NewValue);
            }
        }

        private static void OnSignalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CondenserLidControl control)
            {
                // 当OpenSignal或CloseSignal变化时，更新IsOpen状态
                if (control.OpenSignal && !control.CloseSignal)
                {
                    control.IsOpen = true;
                }
                else if (!control.OpenSignal && control.CloseSignal)
                {
                    control.IsOpen = false;
                }
                // 如果两个信号都为true或false，保持当前状态不变
            }
        }

        private void ToggleLid(bool isOpen)
        {
            double angle = isOpen ? OpenAngle : 0;

            var leftAnimation = new DoubleAnimation
            {
                To = -angle,
                Duration = TimeSpan.FromSeconds(AnimationDuration),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };

            var rightAnimation = new DoubleAnimation
            {
                To = angle,
                Duration = TimeSpan.FromSeconds(AnimationDuration),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };

            leftLidRotate.BeginAnimation(RotateTransform.AngleProperty, leftAnimation);
            rightLidRotate.BeginAnimation(RotateTransform.AngleProperty, rightAnimation);
        }

        //鼠标双击打开关闭 不需要可以注释掉
        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            IsOpen = !IsOpen;
        }
    }
}

