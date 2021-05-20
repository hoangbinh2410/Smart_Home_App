using BA_MobileGPS.Entities.Enums;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace BA_MobileGPS.Core
{
    public class StatusIssueColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Color.FromHex("#CED6E0");
            }
            if (!string.IsNullOrEmpty(value.ToString()))
            {
                return Color.FromHex("#00ADE5");
            }
            else
            {
                return Color.FromHex("#CED6E0");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }
    }
}