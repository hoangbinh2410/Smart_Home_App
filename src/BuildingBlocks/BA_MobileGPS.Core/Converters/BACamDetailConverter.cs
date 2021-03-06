using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Utilities.Enums;
using BA_MobileGPS.Utilities.Extensions;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace BA_MobileGPS.Core
{
    public class BoolToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return MobileResource.Common_Button_No;
            }
            if ((bool)value)
                return MobileResource.Common_Button_Yes;
            else
                return MobileResource.Common_Button_No;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return (Color)Application.Current.Resources["DangerousColor"];
            }
            if ((bool)value)
                return (Color)Application.Current.Resources["PrimaryColor"];
            else
                return (Color)Application.Current.Resources["DangerousColor"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class NetworkTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "";
            }
            return ((NetworkDataType)value).ToDescription();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class StorageTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "";
            }
            else
            {
                var storage = (int)value;
                switch (storage)
                {
                    case 0:
                        return "SDC";

                    case 1:
                        return "SSD";

                    case 2:
                        return "USB Stick";
                }
                return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class CSQIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "ic_ltesignal.png";
            }
            var data = (int)value;
            double percent = (data / 31f) * 100;
            if (percent >= 1 && percent <= 25)
            {
                return "ic_ltesignal1.png";
            }
            else if (percent > 25 && percent <= 50)
            {
                return "ic_ltesignal2.png";
            }
            else if (percent > 50 && percent <= 90)
            {
                return "ic_ltesignal3.png";
            }
            else if (percent > 90 && percent <= 100)
            {
                return "ic_ltesignal4.png";
            }

            return "ic_ltesignal.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}