using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace BA_MobileGPS.Core
{
    public class BoolToColorCoverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return (Color)Application.Current.Resources["WhiteColor"];
            }
            if ((bool)value)
            {
                return (Color)Application.Current.Resources["OrangeColor"];
            }
            else
            {
                return (Color)Application.Current.Resources["WhiteColor"];
            }          
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
