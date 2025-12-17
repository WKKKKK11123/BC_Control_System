using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BC_Control_System.Converters
{
    public class ArrayToStringConverter : IValueConverter
    {
        // 将数组转换为字符串
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // 检查是否为整数数组
            if (value is int[] array)
            {
                // 如果传入参数（如格式要求），按参数处理
                if (parameter is string format)
                {
                    switch (format)
                    {
                        case "time": // 自定义时间格式：2025-05-19 19:57
                            if (array.Length >= 5)
                            {
                                return $"{array[0]}-{array[1]:D2}-{array[2]:D2} {array[3]:D2}:{array[4]:D2}";
                            }
                            break;
                    }
                }
                // 默认返回逗号分隔的字符串
                return string.Join(", ", array);
            }
            return string.Empty; // 非数组类型返回空
        }

        // 反向转换（不需要实现）
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
