using System;
using System.Collections;
using System.Globalization;

using Xamarin.Forms;

namespace BA_MobileGPS.Core
{
    public class HasDataConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if null then not visible
            if (value == null)
                return false;

            //if empty string then not visible
            if (value is string)
                return !string.IsNullOrWhiteSpace((string)value);

            //if blank list not visible
            if (value is IList)
                return ((IList)value).Count > 0;

            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    public class HasDataBackgroundColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if null then not visible
            if (value == null)
                return (Color)Prism.PrismApplicationBase.Current.Resources["WhiteColor"]; ;

            //if empty string then not visible
            if (value is string)
                if (!string.IsNullOrWhiteSpace((string)value))
                {
                    return (Color)Prism.PrismApplicationBase.Current.Resources["PrimaryColor"];
                };

            //if blank list not visible
            if (value is IList)
                if (((IList)value).Count > 0)
                {
                    return (Color)Prism.PrismApplicationBase.Current.Resources["PrimaryColor"];
                };

            return (Color)Prism.PrismApplicationBase.Current.Resources["WhiteColor"];

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    public class HasDataIconColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if null then not visible
            if (value == null)
                return (Color)Prism.PrismApplicationBase.Current.Resources["PrimaryColor"]; ;

            //if empty string then not visible
            if (value is string)
                if (!string.IsNullOrWhiteSpace((string)value))
                {
                    return (Color)Prism.PrismApplicationBase.Current.Resources["WhiteColor"];
                };

            //if blank list not visible
            if (value is IList)
                if (((IList)value).Count > 0)
                {
                    return (Color)Prism.PrismApplicationBase.Current.Resources["WhiteColor"];
                };

            return (Color)Prism.PrismApplicationBase.Current.Resources["PrimaryColor"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}