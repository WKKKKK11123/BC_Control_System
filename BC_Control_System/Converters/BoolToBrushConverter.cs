using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace BC_Control_System.Converters
{
    public class BoolToBrushConverter : IValueConverter
    {
        public SolidColorBrush TrueColor { get; set; } = Brushes.Green;  // true 时的颜色
        public SolidColorBrush FalseColor { get; set; } = Brushes.Red; // false 时的颜色

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value==null)
                {
                    return Brushes.LightGray; // 默认颜色（非 bool 类型时）
                }
                bool value1 = bool.Parse(value.ToString());
                if (value1 is bool boolValue)
                {
                    return boolValue ? TrueColor : FalseColor;
                }
                return Brushes.LightGray; // 默认颜色（非 bool 类型时）
            }
            catch (Exception ex)
            {

                return Brushes.LightGray; // 默认颜色（非 bool 类型时）
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
