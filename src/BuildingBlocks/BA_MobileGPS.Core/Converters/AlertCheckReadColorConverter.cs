using BA_MobileGPS.Utilities.Extensions;

using System;
using System.Globalization;

using Xamarin.Forms;

namespace BA_MobileGPS.Converter
{
    public class AlertCheckReadColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
            {
                return Application.Current.Resources["TextSecondaryColor"];
            }
            else
            {
                return Application.Current.Resources["GreenColor"];
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class AlertStatusColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var colorDefault = Application.Current.Resources["GreenColor"];
            var color = colorDefault;
            try
            {
                int colorAlert = (int)value;
                color = colorAlert == 0 ? colorDefault : $"#{colorAlert.ConvertIntToHex()}";
            }
            catch
            {
            }
            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}