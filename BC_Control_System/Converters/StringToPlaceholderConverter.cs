using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BC_Control_System.Converters
{
    public class StringToPlaceholderConverter:IValueConverter
    {
        public object Convert(object value, Type type, object p, CultureInfo c)
        {
            if (string.IsNullOrEmpty(value as string)) return "--------";
            if (double.TryParse(value as string, out double tempdvalue) && value.ToString().Contains('.')) return tempdvalue.ToString("F2");
            return value;
        } 

        public object ConvertBack(object v, Type t, object p, CultureInfo c)
            => throw new NotSupportedException();
    }
}
