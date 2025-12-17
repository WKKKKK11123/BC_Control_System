using System;
using System.Globalization;
using System.Windows.Data;

namespace BC_Control_System.Converters
{
    public class SecondsToMinutesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                int acttime = 0;
                bool condition = int.TryParse(value?.ToString(), out acttime);
                if (condition)
                {
                    int minutes = acttime / 60;
                    int seconds = acttime % 60;
                    return $"{minutes:D2}:{seconds:D2}";
                }
                return "00:00";
            }
            catch (Exception ee)
            {

                return "00:00";
            }
            
            return "00:00";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string timeString)
            {
                var parts = timeString.Split(':');
                if (parts.Length == 2 &&
                    int.TryParse(parts[0], out int minutes) &&
                    int.TryParse(parts[1], out int seconds))
                {
                    return minutes * 60 + seconds;
                }
            }
            return 0; // 默认返回0秒
        }
    }
}
