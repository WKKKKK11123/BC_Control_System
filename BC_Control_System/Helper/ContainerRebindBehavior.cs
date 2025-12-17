using BC_Control_CustomControl.Controls;
using BC_Control_Models;
using Microsoft.Xaml.Behaviors;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Threading;
using ValueConverters;
using System.Windows.Shapes;

namespace ZCControlSystem.Controls.Behaviors
{
    public class ContainerRebindBehavior : Behavior<Panel>
    {
        /// <summary>
        /// 所有阀信息
        /// </summary>
        public IEnumerable<IPLCValue> ValveInfos
        {
            get => (IEnumerable<IPLCValue>)GetValue(ValveInfosProperty);
            set => SetValue(ValveInfosProperty, value);
        }

        public static readonly DependencyProperty ValveInfosProperty = DependencyProperty.Register(
            nameof(ValveInfos),
            typeof(IEnumerable<IPLCValue>),
            typeof(ContainerRebindBehavior),
            new PropertyMetadata(null)
        );
        public bool RebindTrigger
        {
            get => (bool)GetValue(RebindTriggerProperty);
            set => SetValue(RebindTriggerProperty, value);
        }
        public static readonly DependencyProperty RebindTriggerProperty =
            DependencyProperty.Register(
                nameof(RebindTrigger),
                typeof(bool),
                typeof(ContainerRebindBehavior),
                new PropertyMetadata(false, OnRebindTriggerChanged)
            );

        protected override void OnAttached()
        {
            base.OnAttached();
        }

        private static void OnRebindTriggerChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e
        )
        {
            var behavior = d as ContainerRebindBehavior;
            if (behavior == null) return;

            // 确保行为已经附加
            if (behavior.AssociatedObject == null)
            {
                Debug.WriteLine("Warning: Behavior not attached yet. Delaying rebind...");

                // 延迟执行，等待行为附加完成
                behavior.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (behavior.AssociatedObject != null && (bool)e.NewValue)
                    {
                        behavior.RebindAllControls();
                    }
                }), DispatcherPriority.Loaded);
                return;
            }

            if ((bool)e.NewValue)
            {
                behavior.RebindAllControls();
            }
        }

        private void RebindAllControls()
        {
            if (AssociatedObject == null)
                return;

            foreach (var child in AssociatedObject.Children.OfType<FrameworkElement>())
            {
                ApplyRebindings(child);
            }
        }

        private void ApplyRebindings(FrameworkElement control)
        {
            // 清除现有绑定
            //BindingOperations.ClearAllBindings(control);

            // 根据控件类型应用新绑定
            switch (control)
            {
                case ValveControl valve:
                    ApplyValveBindings(valve);
                    break;
                case UtypePhotoelectricSwitch utypePhotoelectricSwitch:
                    UtypePhotoelectricSwitchBindings(utypePhotoelectricSwitch);
                    break;
                case LiquidLevelControl liquidLevelControl:
                    ApplyLiquidLevelControlBindings(liquidLevelControl);
                    break;
                case Border border:
                    ApplyBorderControl(border);
                    break;
                case PumpControl pumpControl:
                    PumpControlBindings(pumpControl);
                    break;
                case ContentControl contentControl:
                    ApplyContentControlBindings(contentControl);
                    break;              
                case Ellipse ellipse:
                    ApplyEllipseBindings(ellipse);
                    break;
            }
        }
        private void UtypePhotoelectricSwitchBindings(UtypePhotoelectricSwitch valve)
        {
            if (string.IsNullOrEmpty(valve.Name))
            {
                return;
            }
            var info = ValveInfos?.Where(v => v.ParameterName == valve.Name).FirstOrDefault();
            if (info == null)
                info = ValveInfos?.Where(v => v.ParameterName == valve.Tag.ToString()).FirstOrDefault();
            if (info == null)
                return;
            if (
                !string.IsNullOrEmpty(info.ValueAddress)
                && info.Value != null
            )
            {
                valve.SetBinding(
                    UtypePhotoelectricSwitch.ActStateProperty,
                    new Binding(nameof(info.Value))
                    {
                        Source = info,
                        Mode = BindingMode.TwoWay,
                    }
                );
            }           
        }
        private void ApplyEllipseBindings(Ellipse ellipse)
        {
            if (string.IsNullOrEmpty(ellipse.Name))
            {
                return;
            }
            Brush TrueColor = Brushes.Red;
            Brush FalseColor = Brushes.Transparent;
            IValueConverter valueConverter = null;
            var info = ValveInfos?.Where(v => v.ParameterName == ellipse.Name).FirstOrDefault();
            if (info == null)
                info = ValveInfos?.Where(v => v.ParameterName == ellipse.Tag.ToString()).FirstOrDefault();
            if (info == null) return;
            if (true)
            {
                ValueConverterGroup var1 = new ValueConverterGroup();
                var1.Converters.Add(new StringToBoolConverter()
                {
                    TrueValue = "True",
                });
                var1.Converters.Add(new BoolToBrushConverter()
                {
                    TrueValue = Brushes.Yellow,
                    FalseValue = Brushes.LightGray
                });
                valueConverter = var1;
            }
            if (
                !string.IsNullOrEmpty(info.ValueAddress)
                && info.Value != null
            )
            {
                ellipse.SetBinding(
                    Ellipse.FillProperty,
                    new Binding(nameof(info.Value))
                    {
                        Source = info,
                        Mode = BindingMode.TwoWay,
                        Converter = valueConverter
                    }
                );
            }
        }
        private void ApplyContentControlBindings(ContentControl valve)
        {
            if (string.IsNullOrEmpty(valve.Name))
            {
                return;
            }
            Brush TrueColor = Brushes.Red;
            Brush FalseColor = Brushes.Transparent;
            IValueConverter valueConverter = null;
            var info = ValveInfos?.Where(v => v.ParameterName == valve.Name).FirstOrDefault();
            if (info == null)
                info = ValveInfos?.Where(v => v.ParameterName == valve.Tag.ToString()).FirstOrDefault();
            if (info == null) return;          
            if (valve.Style == (Style)Application.Current.Resources["HeaterStyle"])
            {
                ValueConverterGroup var1 = new ValueConverterGroup();
                var1.Converters.Add(new StringToBoolConverter()
                {
                    TrueValue = "True",
                });
                var1.Converters.Add(new BoolToBrushConverter()
                {
                    TrueValue = Brushes.Red,
                    FalseValue = Brushes.LightGray
                });
                valueConverter = var1;
            }
            if (
                !string.IsNullOrEmpty(info.ValueAddress)
                && info.Value != null
            )
            {

                valve.SetBinding(
                    Control.BackgroundProperty,
                    new Binding(nameof(info.Value))
                    {
                        Source = info,
                        Mode = BindingMode.TwoWay,
                        Converter = valueConverter
                    }
                );
            }
        }
        private void PumpControlBindings(PumpControl valve)
        {
            if (string.IsNullOrEmpty(valve.Name))
            {
                return;
            }
            var info = ValveInfos?.Where(v => v.ParameterName == valve.Name).FirstOrDefault();
            if (info == null)
                info = ValveInfos?.Where(v => v.ParameterName == valve.Tag.ToString()).FirstOrDefault();
            if (info == null)
                return;
            if (
                !string.IsNullOrEmpty(info.ValueAddress)
                && info.Value != null
            )
            {
                valve.SetBinding(
                    PumpControl.ActStateProperty,
                    new Binding(nameof(info.Value))
                    {
                        Source = info,
                        Mode = BindingMode.TwoWay,
                    }
                );
            }            
        }
        private void ApplyLiquidLevelControlBindings(LiquidLevelControl valve)
        {
            if (string.IsNullOrEmpty(valve.Name))
            {
                return;
            }
            var info = ValveInfos?.Where(v => v.ParameterName == valve.Name).FirstOrDefault();
            if (info == null)
                info = ValveInfos?.Where(v => v.ParameterName == valve.Tag.ToString()).FirstOrDefault();
            if (info == null)
                return;
            if (
                !string.IsNullOrEmpty(info.ValueAddress)
                && info.Value != null
            )
            {
                valve.SetBinding(
                    LiquidLevelControl.ActLevelProperty,
                    new Binding(nameof(info.Value))
                    {
                        Source = info,
                        Mode = BindingMode.TwoWay,
                    }
                );
            }
        }
        private void ApplyValveBindings(ValveControl valve)
        {
            if (string.IsNullOrEmpty(valve.Name))
            {
                return;
            }
            var info = ValveInfos?.Where(v => v.ParameterName == valve.Name).FirstOrDefault();
            if (info == null)
                info = ValveInfos?.Where(v => v.ParameterName == valve.Tag.ToString()).FirstOrDefault();
            if (info == null)
                return;
            if (
                !string.IsNullOrEmpty(info.ValueAddress)
                && info.Value != null
            )
            {
                valve.SetBinding(
                    ValveControl.ActStateProperty,
                    new Binding(nameof(info.Value))
                    {
                        Source = info,
                        Mode = BindingMode.TwoWay,
                    }
                );
            }
        }
        private void ApplyBorderControl(Border control)
        {
            if (string.IsNullOrEmpty(control.Name))
            {
                return;
            }
            var info = ValveInfos?.Where(v => v.ParameterName == control.Name).FirstOrDefault();
            if (info == null)
                info = ValveInfos?.Where(v => v.ParameterName == control.Tag.ToString()).FirstOrDefault();
            if (info == null)
                return;
            IValueConverter valueConverter = null;
            if (true)
            {
                ValueConverterGroup var1 = new ValueConverterGroup();
                var1.Converters.Add(new StringToBoolConverter()
                {
                    TrueValue = "True",
                });
                var1.Converters.Add(new BoolToBrushConverter()
                {
                    TrueValue = Brushes.Yellow,
                    FalseValue = Brushes.LightGray
                });
                valueConverter = var1;
            }
            if (
              !string.IsNullOrEmpty(info.ValueAddress)
              && info.Value != null
          )
            {

                control.SetBinding(
                    Control.BackgroundProperty,
                    new Binding(nameof(info.Value))
                    {
                        Source = info,
                        Mode = BindingMode.TwoWay,
                        Converter = valueConverter
                    }
                );
            }
        }
    }
}
