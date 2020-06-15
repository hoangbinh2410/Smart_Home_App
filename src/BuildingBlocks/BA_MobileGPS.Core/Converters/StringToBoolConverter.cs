using BA_MobileGPS.Utilities;

using System;
using System.Globalization;

using Xamarin.Forms;

namespace BA_MobileGPS.Core
{
    public class StringToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return false;
            }
            if (string.IsNullOrEmpty(value.ToString()))
                return false;
            else
                return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }
    }

    public class ShowOnOffToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return false;
            }

            string stringValue = value.ToString();

            if (stringValue.ToUpper().Equals("TRUE"))
            {
                if (Settings.CurrentLanguage == CultureCountry.Vietnamese)
                {
                    return "Mở";
                }
                else
                {
                    return "On";
                }
            }
            else
            {
                if (Settings.CurrentLanguage == CultureCountry.Vietnamese)
                {
                    return "Tắt";
                }
                else
                {
                    return "Off";
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}