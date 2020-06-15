using System;
using System.Globalization;

using Xamarin.Forms;

namespace BA_MobileGPS.Core
{
    public class FormatDistanceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Math.Round((double)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}