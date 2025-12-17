using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media;
using static BC_Control_CustomControl.WTRRobot;

namespace BC_Control_CustomControl
{
    public enum WaferRobotXAction
    {
        X_pos0,
        X_pos1,
        X_pos2,
        X_pos3,
        X_pos4,
        X_pos5,
        X_pos6,
        X_pos7,
        X_pos8,
        X_pos9,
    }
    public class WTRRobot:Control
    {

        

        static WTRRobot()
        {
            // 设置默认样式
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WTRRobot), new FrameworkPropertyMetadata(typeof(WTRRobot)));
        }
        public static readonly DependencyProperty WaferProperty = DependencyProperty.Register("Wafer", typeof(int), typeof(WTRRobot));
        public int Wafer { get => (int)GetValue(WaferProperty); set => SetValue(WaferProperty, value); }

        public static readonly DependencyProperty RobotXActionProperty = DependencyProperty.Register(
            "RobotXAction",
            typeof(WaferRobotXAction),
            typeof(WTRRobot),
            new PropertyMetadata(WaferRobotXAction.X_pos0, RobotXActionPropertyChangedCallback));
        public WaferRobotXAction RobotXAction
        {
            get => (WaferRobotXAction)GetValue(RobotXActionProperty);
            set => SetValue(RobotXActionProperty, value);
        }

        private static void RobotXActionPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as WTRRobot;
            var oldAct = (double)e.OldValue;
            var newAct = (double)e.NewValue;
            if (oldAct!= newAct)
            {
                if (newAct != oldAct)
                {
                    VisualStateManager.GoToState(control, newAct.ToString(), true);
                }
            }
            //switch (newAct)
            //{
            //    case WaferRobotXAction.X_None:
            //        VisualStateManager.GoToState(control, newAct.ToString(), true);
            //        break;
            //    case WaferRobotXAction.X_Move:
            //        if (newAct != oldAct)
            //        {
            //            VisualStateManager.GoToState(control, newAct.ToString(), true);
            //        }
            //        break;
            //    default:
            //        break;
            //}
        }
        public WTRRobot()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WTRRobot), new FrameworkPropertyMetadata(typeof(WTRRobot)));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            VisualStateManager.GoToState(this, WaferRobotXAction.X_pos0.ToString(), true);
        }
        // 启动动画的方法

    }
}

