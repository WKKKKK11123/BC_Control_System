using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BC_Control_System.Converters
{
    public class DoubleToTwoDecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            culture = culture ?? CultureInfo.InvariantCulture; // 确保使用 . 作为小数点


            // 处理 float 类型
            if (value is float floatValue)
                return floatValue.ToString("0.00", CultureInfo.InvariantCulture);

            // 处理 double 类型（保持原有逻辑）
            if (value is double doubleValue)
                return doubleValue.ToString("0.00", CultureInfo.InvariantCulture);

            // 其他类型尝试转换（如 int/decimal/string）
            if (value != null && double.TryParse(value.ToString(), out var parsed))
                return parsed.ToString("0.00", culture);

            // 其他类型直接返回（如 int、decimal 等）
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
