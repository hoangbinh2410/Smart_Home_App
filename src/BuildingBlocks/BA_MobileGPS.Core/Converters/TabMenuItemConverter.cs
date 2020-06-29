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
            if (true)
            {

            }
            if (value == null)
            {
                return (Color)Application.Current.Resources["DarkTabItemUnSelectedColor"];
            }
            if ((bool)value)
                return (Color)Application.Current.Resources["DarkTabItemSelectedColor"];
            else
                return (Color)Application.Current.Resources["DarkTabItemUnSelectedColor"];
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
