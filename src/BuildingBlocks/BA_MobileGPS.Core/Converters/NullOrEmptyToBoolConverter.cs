using System;
using Xamarin.Forms;

namespace BA_MobileGPS.Core
{
    public class NullOrEmptyToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return false;

            string val = value.ToString();
            if (string.IsNullOrEmpty(val))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
