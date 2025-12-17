using BC_Control_Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BC_Control_System.Converters
{
    public class RightsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (!(value is Rights)) return false;
                int t=(int)Enum.Parse(typeof(Rights), value.ToString()!);
                return t > int.Parse(parameter.ToString()!);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
