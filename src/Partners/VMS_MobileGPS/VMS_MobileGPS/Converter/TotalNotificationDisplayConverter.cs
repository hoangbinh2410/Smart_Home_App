using System;
using System.Globalization;

using Xamarin.Forms;

namespace VMS_MobileGPS.Converter
{
    public class TotalNotificationDisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }

            float val = int.Parse(value.ToString());

            if (val >= 100)
                return $"99+";
            else
                return val;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}