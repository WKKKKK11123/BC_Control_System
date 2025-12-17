using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using ZC_Control_EFAM;

namespace BC_Control_System.Converters
{
    public class StringIsBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value == null) return false;
                bool result;
                if (!bool.TryParse(value.ToString(), out result))
                {
                    return false;
                }
                return result;
            }
            catch (Exception EX)
            {
                return false;
            }
            
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture
        )
        {
            throw new NotImplementedException();
        }
    }

    public class BoolToVisibilityConverter1 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture
        )
        {
            throw new NotImplementedException();
        }
    }

    public class EnumListToIntStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IEnumerable<WaferMapStation> enumList)
            {
                // 将枚举 List 转换为 int 并用逗号连接
                return string.Join(",", enumList.Cast<Enum>().Select(e => (int)(object)e));
            }
            return string.Empty; // 如果不是枚举 List，返回空字符串
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture
        )
        {
            throw new NotImplementedException(); // 单向转换，不需要反向转换
        }
    }

    public class InverseBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
                return !boolValue;
            return true; // 默认启用（如果绑定值不是 bool）
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture
        )
        {
            if (value is bool boolValue)
                return !boolValue;
            return false;
        }
    }
}
