using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

using Xamarin.Forms;

namespace BA_MobileGPS.Core
{
    public class CurrencyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Decimal.Parse(value.ToString()).ToString("C");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string valueFromString = Regex.Replace(value.ToString(), @"\D", "");

            if (valueFromString.Length <= 0)
                return 0m;

            long valueLong;
            if (!long.TryParse(valueFromString, out valueLong))
                return 0m;

            if (valueLong <= 0)
                return 0m;

            return valueLong / 100m;
        }
    }

    public class FuelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }

            float val = float.Parse(value.ToString());

            if (val == 0)
                return $"0 L";
            else
                return $"{val} L";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class NumberLitsOfPerBottleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "0 L";
            }
            try
            {
                List<double> val = (List<double>)value;

                return string.Join("L, ", val);
            }
            catch
            {
                return "0 L";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class TemperatureConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }

            float val = float.Parse(value.ToString());

            if (val == 0)
                return $"0 °C";
            else
                return $"{val} °C";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}