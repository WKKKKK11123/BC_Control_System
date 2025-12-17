using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BC_Control_System.Converters
{
    public class StatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string statusCode)
            {
                switch (statusCode)
                {
                    case "0": return "Without";
                    case "1": return "Init";
                    case "2": return "Idle";
                    case "3": return "Ready";
                    case "4": return "NotReady"; 
                    case "5": return "Processing";
                    case "6": return "PreProcessing";
                    case "7": return "PostProcessing";
                    default: return "Unknown";
                }
            }
            return "Invalid";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException(); // 单向转换，不需要反向转换
        }
    }
}
