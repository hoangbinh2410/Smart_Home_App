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
            if (value == null)
            {
                return (Color)Application.Current.Resources["TabItemUnSelectedColor"];
            }
            if ((bool)value)
                return (Color)Application.Current.Resources["TabItemSelectedColor"];
            else
                return (Color)Application.Current.Resources["TabItemUnSelectedColor"];
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
                return (double)Application.Current.Resources["TabItemNormalSize"];
            }
            if ((bool)value)
                return (double)Application.Current.Resources["TabItemClickedSize"];
            else
                return (double)Application.Current.Resources["TabItemNormalSize"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }
    }
}
