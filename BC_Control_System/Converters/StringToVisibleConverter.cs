using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace BC_Control_System.Converters
{
    public class StringToVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                bool result;
                if (!bool.TryParse(value.ToString(), out result))
                {
                    return Visibility.Collapsed;
                }
                return result == true ? Visibility.Visible : Visibility.Collapsed;
            }
            catch (Exception ee)
            {

                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
