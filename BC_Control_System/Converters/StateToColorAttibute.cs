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
    public class StateToColorAttibute : IMultiValueConverter
    {
        public SolidColorBrush TrueColor { get; set; } = Brushes.Blue;
        public SolidColorBrush FalseColor { get; set; } = Brushes.Red;
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                string value = values[0].ToString();
                string stateAttrbute = values[1].ToString();
                if (string.IsNullOrEmpty(stateAttrbute))
                {
                    SolidColorBrush brushes = value == "0" ? FalseColor : TrueColor;
                    return brushes;
                }
                else
                {
                    bool b1 = int.TryParse(value, out int t);
                    SolidColorBrush brushes = b1 ? FalseColor : TrueColor;
                    return brushes;
                }
            }
            catch (Exception ee)
            {

                return FalseColor;
            }

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
