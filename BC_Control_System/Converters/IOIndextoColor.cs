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
    public class IOIndextoColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value == null)
                {
                    return new SolidColorBrush(Colors.LightGray);
                }
                int tempvalue = int.Parse(value.ToString());
                if (tempvalue % 2==1)
                {
                    return new SolidColorBrush(Colors.White);
                }
                else
                {
                    return new SolidColorBrush(Colors.LightGray);
                   
                }
            }
            catch (Exception ee)
            {

                return new SolidColorBrush(Colors.LightGray);
            }
           
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
