using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace BA_MobileGPS.Core
{
    public class TabItemColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color unSelected;
            Color selected;
            if (Application.Current.RequestedTheme == OSAppTheme.Light || Application.Current.RequestedTheme == OSAppTheme.Unspecified)
            {
                unSelected = (Color)Application.Current.Resources["LightTabItemUnSelectedColor"];
                selected = (Color)Application.Current.Resources["LightTabItemSelectedColor"];
            }
            else
            {
                unSelected = (Color)Application.Current.Resources["DarkTabItemUnSelectedColor"];
                selected = (Color)Application.Current.Resources["DarkTabItemSelectedColor"];
            }
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

    public class TabItemSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return (double)Application.Current.Resources["DarkTabItemNormalSize"];
            }
            if ((bool)value)
                return (double)Application.Current.Resources["DarkTabItemClickedSize"];
            else
                return (double)Application.Current.Resources["DarkTabItemNormalSize"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }
    }
}
