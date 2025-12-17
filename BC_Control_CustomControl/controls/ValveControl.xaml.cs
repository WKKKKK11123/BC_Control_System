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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BC_Control_CustomControl.Controls
{
    /// <summary>
    /// ValveControl.xaml 的交互逻辑
    /// </summary>
    public partial class ValveControl : UserControl, ICommandSource
    {
        public ValveControl()
        {
            InitializeComponent();
        }
        #region 定义点击事件

        public event RoutedEventHandler Click;

        #endregion


       

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(ValveControl), new UIPropertyMetadata(null));
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(ValveControl));

        public IInputElement CommandTarget
        {
            get { return (IInputElement)GetValue(CommandTargetProperty); }
            set { SetValue(CommandTargetProperty, value); }
        }

        public static readonly DependencyProperty CommandTargetProperty =
            DependencyProperty.Register("CommandTarget", typeof(IInputElement), typeof(ValveControl));
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {

            base.OnMouseLeftButtonUp(e);
            //调用点击事件
            Click?.Invoke(e.Source, e);
            //调用Command
            ICommand command = Command;
            object parameter = CommandParameter;
            IInputElement target = CommandTarget;

            RoutedCommand routedCmd = command as RoutedCommand;
            if (routedCmd != null && routedCmd.CanExecute(parameter, target))
            {
                routedCmd.Execute(parameter, target);
            }
            else if (command != null && command.CanExecute(parameter))
            {
                command.Execute(parameter);
            }
        }


        public bool IsInverse { get; set; }


        public bool ActState
        {
            get { return (bool)GetValue(ActStateProperty); }
            set { SetValue(ActStateProperty, value); }
        }
        public static readonly DependencyProperty ActStateProperty = DependencyProperty.Register(
            "ActState",
            typeof(bool),
            typeof(ValveControl),
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
            typeof(ValveControl),
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
            typeof(ValveControl),
            new PropertyMetadata(1.0, OnScaleChange)
        );
        public static void OnRotationChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ValveControl ValveControl = d as ValveControl;
            ValveControl.rectRotateTransform.Angle = (double)e.NewValue;
        }
        public static void OnScaleChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ValveControl ValveControl = d as ValveControl;
            ValveControl.rectScaleTransform.ScaleX = (double)e.NewValue;
            ValveControl.rectScaleTransform.ScaleY = (double)e.NewValue;
        }
        public static void OnActStateChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ValveControl ValveControl = d as ValveControl;
            if (ValveControl.IsInverse)
            {
                ValveControl.SwitchPath.Fill = (bool)e.NewValue == false ? new SolidColorBrush(Colors.Cyan) : new SolidColorBrush(Colors.LightGray);
            }
            else

                ValveControl.SwitchPath.Fill = (bool)e.NewValue == true ? new SolidColorBrush(Colors.Cyan) : new SolidColorBrush(Colors.LightGray);
        }
    }
}
