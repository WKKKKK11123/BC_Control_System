using NPOI.SS.Formula.Functions;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using BC_Control_Models;

namespace BC_Control_System.Converters
{
    public class EnumDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is Type enumType && enumType.IsEnum)
            {
                if (value is int intValue && Enum.IsDefined(enumType, intValue))
                {
                    return GetEnumDescription(Enum.GetName(enumType, intValue));
                }
                //PlcEnum tempEnumValue;
                if (Enum.TryParse(value.ToString(), out PlcEnum tempEnumValue))
                {
                    return GetEnumDescription(tempEnumValue);
                }
            }
            return "Unknown";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Empty;
        }

        private string GetEnumDescription(object enumObj)
        {
            var fi = enumObj.GetType().GetField(enumObj.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return enumObj.ToString();
        }
    }
}