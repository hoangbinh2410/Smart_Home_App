using System;
using System.Globalization;
using Xamarin.Forms;

namespace BA_MobileGPS.Core
{
    public class TabItemColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color unSelected = (Color)Application.Current.Resources["MainTabUnSelectedColor"];
            Color selected = (Color)Application.Current.Resources["MainTabSelectedColor"];
            if (value == null)
            {
                return unSelected;
            }
            if ((bool)value)
                return selected;
            else
                return unSelected;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }
    }

    public class TabItemTextColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color unSelected = (Color)Application.Current.Resources["MainTabUnSelectedTextColor"];
            Color selected = (Color)Application.Current.Resources["MainTabSelectedTextColor"];
            if (value == null)
            {
                return unSelected;
            }
            if ((bool)value)
                return selected;
            else
                return unSelected;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }
    }
}