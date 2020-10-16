using System;
using System.Globalization;
using Xamarin.Forms;

namespace MOTO_MobileGPS.Converter
{
    public class VoltageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }

            float val = float.Parse(value.ToString());

            return $"{val} mV";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
