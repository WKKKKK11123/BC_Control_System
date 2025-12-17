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
    public class StorageStateToBrushConverter : IValueConverter
    {
        public SolidColorBrush TrueColor { get; set; } = Brushes.Blue;
        public SolidColorBrush FalseColor { get; set; } = Brushes.Red;
       
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                int value1=int.Parse(value.ToString());
               
                switch (value1)
                {
                    case 0:
                        return Brushes.LightGray;
                    case 1:
                        return Brushes.Cyan;
                    case 2:
                        return Brushes.Blue;
                    case 3:
                        return Brushes.Green;
                    default:
                        return Brushes.LightGray;
                }
            }
            catch (Exception EE)
            {

                return Brushes.LightGray;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
