using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Linq;
using Xamarin.Forms;

namespace BA_MobileGPS.Core
{
    public class FavoritesColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color unFavorites = (Color)Application.Current.Resources["WhiteColor"];
            Color favorites = (Color)Application.Current.Resources["YellowColor"];

            if (value == null)
            {
                return unFavorites;
            }

            if ((bool)value)
            {
                return favorites;
            }
            else
            {
                return unFavorites;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }
    }

    public class FavoritesBoolColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return false;
            }

            if (Settings.FavoritesVehicleImage.Contains((string)value))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }
    }
}
