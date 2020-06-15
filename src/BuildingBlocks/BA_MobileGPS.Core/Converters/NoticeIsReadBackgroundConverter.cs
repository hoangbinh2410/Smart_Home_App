using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace BA_MobileGPS.Core
{
    public class NoticeIsReadBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return (Color)Prism.PrismApplicationBase.Current.Resources["Color_Navigation"];
            }
            if ((bool)value)
                return (Color)Prism.PrismApplicationBase.Current.Resources["Color_Text"];
            else
                return (Color)Prism.PrismApplicationBase.Current.Resources["Color_Navigation"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
