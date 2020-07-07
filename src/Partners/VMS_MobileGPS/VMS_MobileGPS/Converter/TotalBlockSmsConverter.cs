using System;
using System.Globalization;

using Xamarin.Forms;

namespace VMS_MobileGPS.Converter
{
    public class TotalBlockSmsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return 0;
            }

            int val = int.Parse(value.ToString()) / 10;

            return val;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}