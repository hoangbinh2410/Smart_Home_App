using System;
using System.Globalization;

using Xamarin.Forms;

namespace BA_MobileGPS.Core
{
    public class UnitConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }

            return $"{value.ToString()} {parameter.ToString()}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return default;
        }
    }

    public class NMConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value == null)
                {
                    return string.Empty;
                }
                if (parameter == null || string.IsNullOrEmpty(parameter.ToString()))
                {
                    return Math.Round((System.Convert.ToSingle(value) / 1.852)).ToString();
                }

                return $"{Math.Round((System.Convert.ToSingle(value) / 1.852)).ToString()} {parameter.ToString()}";
            }
            catch
            {
                return $"{0} {parameter.ToString()}";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return default;
        }
    }
}