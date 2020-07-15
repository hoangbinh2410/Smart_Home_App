using System;
using System.Globalization;
using Xamarin.Forms;

namespace BA_MobileGPS.Core
{
    public class NoticeIsReadBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return (Color)Prism.PrismApplicationBase.Current.Resources["PrimaryColor"];
            }
            if ((bool)value)
                return (Color)Prism.PrismApplicationBase.Current.Resources["TextPrimaryColor"];
            else
                return (Color)Prism.PrismApplicationBase.Current.Resources["PrimaryColor"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}