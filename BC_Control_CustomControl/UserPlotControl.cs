using ScottPlot;
using ScottPlot.Plottables;
using ScottPlot.WPF;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using BC_Control_CustomControl.Common;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.Input;

namespace BC_Control_CustomControl
{
    public class UserPlotControl : Control
    {
        private WpfPlot _plot;

        private List<object> Lines;

        private DateTime[] DateTimeLins;

        private List<Scatter> scatters;

        private Type type;

        private List<CheckBase> upLoadCheck;

        Dictionary<string, double[]> YLines = new Dictionary<string, double[]>();

        Crosshair CH;
        public static readonly DependencyProperty RefreshCommandProperty =
    DependencyProperty.Register(
        "RefreshCommand",
        typeof(ICommand),
        typeof(UserPlotControl),
        new PropertyMetadata(null));

        public ICommand RefreshCommand
        {
            get { return (ICommand)GetValue(RefreshCommandProperty); }
            set { SetValue(RefreshCommandProperty, value); }
        }

        // 构造函数中设置默认命令
        public UserPlotControl()
        {
            RefreshCommand = new RelayCommand(RefreshPlot);
        }
        static UserPlotControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UserPlotControl), new FrameworkPropertyMetadata(typeof(UserPlotControl)));
        }
        public List<CheckBase> CheckList
        {
            get { return (List<CheckBase>)GetValue(CheckListProperty); }
            set { SetValue(CheckListProperty, value); }
        }

        public static readonly DependencyProperty CheckListProperty =
           DependencyProperty.Register("CheckList", typeof(List<CheckBase>), typeof(UserPlotControl), new PropertyMetadata(null));
        public Type LinesType
        {
            get { return (Type)GetValue(LinesTypeProperty); }
            set { SetValue(LinesTypeProperty, value); }
        }

        public static readonly DependencyProperty LinesTypeProperty =
           DependencyProperty.Register("LinesType", typeof(Type), typeof(UserPlotControl), new PropertyMetadata(null, OnLinesTypeChanged));

        private static void OnLinesTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                var tempuserPlotControl = d as UserPlotControl;
                tempuserPlotControl.type = tempuserPlotControl.LinesType;
                var type = tempuserPlotControl.type;
                if (type == null)
                {
                    return;
                }
                int linscount = type.GetProperties().Length;
                tempuserPlotControl.CheckList = type.GetProperties()
                                .Where(prop => tempuserPlotControl.GetPropertyDescription(prop) != null)
                                .Select((prop, index) => new CheckBase()
                                {
                                    Description = tempuserPlotControl.GetPropertyDescription(prop),
                                    IsChecked = false,
                                    BackColor = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString(tempuserPlotControl.GenerateHexColor(index + 1, linscount)))
                                }
                                ).ToList();
                //tempuserPlotControl._plot.MouseMove += (s1, e1) =>
                //{
                //    try
                //    {
                //        //if (btn_1.IsEnabled)
                //        //{
                //        var mousePosition = e1.GetPosition(tempuserPlotControl._plot);
                //        Pixel mousePixel = new Pixel(mousePosition.X, mousePosition.Y);
                //        Coordinates mouseCoordinates = tempuserPlotControl._plot.Plot.GetCoordinates(mousePixel);
                //        tempuserPlotControl.CH.Position = mouseCoordinates;
                //        tempuserPlotControl.CH.VerticalLine.Text =
                //            $"{DateTime.FromOADate(mouseCoordinates.X).ToString()}";
                //        tempuserPlotControl.CH.HorizontalLine.Text = $"{mouseCoordinates.Y:N3}";
                //        tempuserPlotControl._plot.Refresh();
                //        //}
                //    }
                //    catch (Exception ee)
                //    {

                //    }


                //};
                //tempuserPlotControl._plot.MouseDown += (s2, e2) =>
                //{
                //    try
                //    {
                //        var mousePosition = e2.GetPosition(tempuserPlotControl._plot);
                //        Pixel mousePixel = new Pixel(mousePosition.X, mousePosition.Y);
                //        Coordinates mouseCoordinates = tempuserPlotControl._plot.Plot.GetCoordinates(mousePixel);
                //    }
                //    catch (Exception ee)
                //    {

                //        //throw;
                //    }
                //};
            }
            catch (Exception ee)
            {

                //throw;
            }

        }
        public List<object> ValueLines
        {
            get { return (List<object>)GetValue(ValueLinesProperty); }
            set { SetValue(ValueLinesProperty, value); }
        }

        public static readonly DependencyProperty ValueLinesProperty =
            DependencyProperty.Register("ValueLines", typeof(List<object>), typeof(UserPlotControl), new PropertyMetadata(null, OnValueLinesChanged));
        private static void OnValueLinesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                var tempuserPlotControl = d as UserPlotControl;
                var tempplot = tempuserPlotControl._plot;
                //tempplot2.Scatter = new Scatter[5];
                var tempScatter = tempuserPlotControl.scatters;
                Type type = tempuserPlotControl.type;
                if (type == null)
                {
                    return;
                }
                PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                tempuserPlotControl.DateTimeLins = tempuserPlotControl.ValueLines
                                .SelectMany(obj => obj.GetType().GetProperties()
                                .Where(prop => prop.PropertyType == typeof(DateTime))
                                .Select(prop => (DateTime?)prop.GetValue(obj))) // 使用 Nullable 处理 null 值
                                .Where(dt => dt.HasValue) // 过滤 null 值
                                .Select(dt => DateTime.FromOADate((dt.Value - new DateTime(1899, 12, 30)).TotalDays))
                                .ToArray();
                tempuserPlotControl.YLines = tempuserPlotControl.ValueLines
                                .SelectMany(obj => obj.GetType().GetProperties()
                                .Where(prop => tempuserPlotControl.GetPropertyDescription(prop) != null && prop != null)
                                .Select(prop => new
                                {
                                    Name = tempuserPlotControl.GetPropertyDescription(prop),
                                    Value = tempuserPlotControl.TryConvertToDouble(prop?.GetValue(obj)?.ToString()) // NULL值均转化为0
                                }))
                                //.Where(x => x.Value.HasValue) // 过滤 null 值
                                .GroupBy(x => x.Name)

                                .ToDictionary(
                                    g => g.Key,
                                    g => g.Select(x => x.Value).ToArray()
                                );
                int i = 0;
                int indexcount = tempuserPlotControl.YLines.Count();
                //this.plt.Plot.Clear();
                if (tempuserPlotControl.YLines.Count() == 0)
                {
                    foreach (var item in tempuserPlotControl.scatters)
                    {
                        tempplot.Plot.Remove(item);
                    }
                }
                foreach (var item in tempuserPlotControl.YLines)
                {
                    if (tempuserPlotControl.scatters.Count() > i)
                    {
                        tempplot.Plot.Remove(tempScatter[i]);
                        tempScatter[i] = tempplot.Plot.Add.Scatter(tempuserPlotControl.DateTimeLins, item.Value, ScottPlot.Color.FromHex(tempuserPlotControl.GenerateHexColor(i + 1, indexcount)));

                    }
                    else
                    {
                        //tempScatter[i] = new Scatter();
                        tempScatter.Add(tempplot.Plot.Add.Scatter(tempuserPlotControl.DateTimeLins, item.Value, ScottPlot.Color.FromHex(tempuserPlotControl.GenerateHexColor(i + 1, indexcount))));
                    }
                    //scatters[i].Label = item.Key;
                    tempScatter[i].IsVisible = tempuserPlotControl.CheckList[i].IsChecked;
                    i++;

                }

                //tempplot.Plot.Axes.DateTimeTicksBottom();
                tempplot.Plot.Legend.IsVisible = true;
                tempplot.Plot.Legend.OutlineStyle.Color = ScottPlot.Colors.Navy;
                tempplot.Plot.Legend.OutlineStyle.Width = 2;
                //tempuserPlotControl.ArrangeOverride();
                tempuserPlotControl._plot.MouseMove += (s1, e1) =>
                {
                    try
                    {
                        //if (btn_1.IsEnabled)
                        //{
                        var mousePosition = e1.GetPosition(tempuserPlotControl._plot);
                        Pixel mousePixel = new Pixel(mousePosition.X, mousePosition.Y);
                        Coordinates mouseCoordinates = tempuserPlotControl._plot.Plot.GetCoordinates((float)mousePosition.X, (float)mousePosition.Y);
                        tempuserPlotControl.CH.Position = mouseCoordinates;
                        tempuserPlotControl.CH.HorizontalLine.Text = $"{mouseCoordinates.Y:N3}";
                        tempuserPlotControl.CH.VerticalLine.Text = DateTime.FromOADate(mouseCoordinates.X).ToString();
                        tempplot.Refresh();

                        //}
                    }
                    catch (Exception ee)
                    {

                    }


                };
                tempuserPlotControl._plot.MouseDown += (s2, e2) =>
                {
                    try
                    {
                        var mousePosition = e2.GetPosition(tempuserPlotControl._plot);
                        Pixel mousePixel = new Pixel(mousePosition.X, mousePosition.Y);
                        Coordinates mouseCoordinates = tempuserPlotControl._plot.Plot.GetCoordinates(mousePixel);
                    }
                    catch (Exception ee)
                    {

                        //throw;
                    }
                };
                tempplot.Refresh();
            }
            catch (Exception EE)
            {

                throw;
            }

        }
        private string GetPropertyDescription(PropertyInfo prop)
        {
            var descriptionAttribute = prop.GetCustomAttribute<DescriptionAttribute>();
            return descriptionAttribute?.Description;
        }

        private double TryConvertToDouble(string str)
        {
            if (double.TryParse(str, out var result))
            {
                return result;
            }
            return 0;
        }

        private string GenerateHexColor(int index, int totalCurves)
        {
            double ratio = (double)index / totalCurves;

            // 根据比例生成RGB值
            int r = (int)(255 * ratio);          // 红色分量随曲线索引增加
            int g = (int)(255 * (1 - ratio));    // 绿色分量随曲线索引减少
            int b = (int)(255 * Math.Abs(Math.Sin(Math.PI * ratio))); // 蓝色分量有波动

            // 将RGB值转换为16进制，并返回颜色代码
            return $"#{r:X2}{g:X2}{b:X2}";
        }
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            scatters = new List<Scatter>();
            upLoadCheck = new List<CheckBase>();
            // 其他初始化逻辑
        }
        public override void OnApplyTemplate()
        {
            //  获取Xaml中的图表对象
            _plot = GetTemplateChild("PART_Plot") as WpfPlot;

            if (_plot == null)
            {
                _plot = new WpfPlot();
                AddVisualChild(_plot); // 添加到控件树
            }
            _plot.Plot.Axes.DateTimeTicksBottom();
            CH = _plot.Plot.Add.Crosshair(0, 0);
            CH.TextColor = ScottPlot.Colors.White;
            CH.TextBackgroundColor = CH.HorizontalLine.Color;
            base.OnApplyTemplate();
        }
        public void RefreshPlot()
        {
            try
            {
                var tempuserPlotControl = this;
                var tempplot = tempuserPlotControl._plot;
                //tempplot2.Scatter = new Scatter[5];
                var tempScatter = tempuserPlotControl.scatters;
                Type type = tempuserPlotControl.type;
                if (type == null)
                {
                    return;
                }
                PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                tempuserPlotControl.DateTimeLins = tempuserPlotControl.ValueLines
                                .SelectMany(obj => obj.GetType().GetProperties()
                                .Where(prop => prop.PropertyType == typeof(DateTime))
                                .Select(prop => (DateTime?)prop.GetValue(obj))) // 使用 Nullable 处理 null 值
                                .Where(dt => dt.HasValue) // 过滤 null 值
                                .Select(dt => DateTime.FromOADate((dt.Value - new DateTime(1899, 12, 30)).TotalDays))
                                .ToArray();
                tempuserPlotControl.YLines = tempuserPlotControl.ValueLines
                                .SelectMany(obj => obj.GetType().GetProperties()
                                .Where(prop => tempuserPlotControl.GetPropertyDescription(prop) != null && prop != null)                                
                                .Select(prop => new
                                {
                                    Name = tempuserPlotControl.GetPropertyDescription(prop),
                                    Value = tempuserPlotControl.TryConvertToDouble(prop?.GetValue(obj)?.ToString()) // NULL值均转化为0
                                }))
                                //.Where(x => x.Value.HasValue) // 过滤 null 值
                                .GroupBy(x => x.Name)

                                .ToDictionary(
                                    g => g.Key,
                                    g => g.Select(x => x.Value).ToArray()
                                );
                int i = 0;
                int indexcount = tempuserPlotControl.YLines.Count();
                //this.plt.Plot.Clear();
                if (tempuserPlotControl.YLines.Count() == 0)
                {
                    foreach (var item in tempuserPlotControl.scatters)
                    {
                        tempplot.Plot.Remove(item);
                    }
                }
                foreach (var item in tempuserPlotControl.YLines)
                {
                    if (tempuserPlotControl.scatters.Count() > i)
                    {
                        tempplot.Plot.Remove(tempScatter[i]);
                        tempScatter[i] = tempplot.Plot.Add.Scatter(tempuserPlotControl.DateTimeLins, item.Value, ScottPlot.Color.FromHex(tempuserPlotControl.GenerateHexColor(i + 1, indexcount)));

                    }
                    else
                    {
                        //tempScatter[i] = new Scatter();
                        tempScatter.Add(tempplot.Plot.Add.Scatter(tempuserPlotControl.DateTimeLins, item.Value, ScottPlot.Color.FromHex(tempuserPlotControl.GenerateHexColor(i + 1, indexcount))));
                    }
                    //scatters[i].Label = item.Key;
                    tempScatter[i].IsVisible = tempuserPlotControl.CheckList[i].IsChecked;
                    i++;

                }

                //tempplot.Plot.Axes.DateTimeTicksBottom();
                tempplot.Plot.Legend.IsVisible = true;
                tempplot.Plot.Legend.OutlineStyle.Color = ScottPlot.Colors.Navy;
                tempplot.Plot.Legend.OutlineStyle.Width = 2;
                //tempuserPlotControl.ArrangeOverride();
                tempuserPlotControl._plot.MouseMove += (s1, e1) =>
                {
                    try
                    {
                        //if (btn_1.IsEnabled)
                        //{
                        var mousePosition = e1.GetPosition(tempuserPlotControl._plot);
                        Pixel mousePixel = new Pixel(mousePosition.X, mousePosition.Y);
                        Coordinates mouseCoordinates = tempuserPlotControl._plot.Plot.GetCoordinates((float)mousePosition.X, (float)mousePosition.Y);
                        tempuserPlotControl.CH.Position = mouseCoordinates;
                        tempuserPlotControl.CH.HorizontalLine.Text = $"{mouseCoordinates.Y:N3}";
                        tempuserPlotControl.CH.VerticalLine.Text = DateTime.FromOADate(mouseCoordinates.X).ToString();
                        tempplot.Refresh();

                        //}
                    }
                    catch (Exception ee)
                    {

                    }


                };
                tempuserPlotControl._plot.MouseDown += (s2, e2) =>
                {
                    try
                    {
                        var mousePosition = e2.GetPosition(tempuserPlotControl._plot);
                        Pixel mousePixel = new Pixel(mousePosition.X, mousePosition.Y);
                        Coordinates mouseCoordinates = tempuserPlotControl._plot.Plot.GetCoordinates(mousePixel);
                    }
                    catch (Exception ee)
                    {

                        //throw;
                    }
                };
                tempplot.Refresh();
            }
            catch (Exception EE)
            {

                throw;
            }
        }
    }
}
