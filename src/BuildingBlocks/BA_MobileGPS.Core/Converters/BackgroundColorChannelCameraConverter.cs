using BA_MobileGPS.Core.Models;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace BA_MobileGPS.Core
{
    public class BackgroundColorChannelCameraConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Color.FromHex("#E63C2B");
            }
            switch ((ChannelCameraStatus)value)
            {
                case ChannelCameraStatus.Selected:
                    return Color.FromHex("#18A0FB");
                case ChannelCameraStatus.UnSelected:
                    return Color.FromHex("#C4C4C4");
                case ChannelCameraStatus.Error:
                    return Color.FromHex("#E63C2B");
            }
            return Color.FromHex("#18A0FB");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class BackgroundSelectedVideoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return (Color)Application.Current.Resources["GrayColor"];
            }
            if ((bool)value)
            {
                return (Color)Application.Current.Resources["PrimaryColor"];
            }
            return (Color)Application.Current.Resources["GrayColor"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}