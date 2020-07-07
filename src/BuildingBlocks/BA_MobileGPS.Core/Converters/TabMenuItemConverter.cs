using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace BA_MobileGPS.Core
{
    public class TabItemTextColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color unSelected = (Color)Application.Current.Resources["TextSecondaryColor"];
            Color selected = (Color)Application.Current.Resources["BlueDarkColor"];
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

    public class TabItemIconColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color unSelected = (Color)Application.Current.Resources["TextSecondaryColor"];
            Color selectedBackground = (Color)Application.Current.Resources["WhiteColor"];
            if (value == null)
            {
                return unSelected;
            }
            if ((bool)value)
                return selectedBackground;
            else
                return unSelected;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }
    }

    public class TabItemIconBackgroundColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color unSelected = Color.Transparent;
            Color selectedBackground = (Color)Application.Current.Resources["BlueDarkColor"];
            if (value == null)
            {
                return unSelected;
            }
            if ((bool)value)
                return selectedBackground;
            else
                return unSelected;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }
    }

}
