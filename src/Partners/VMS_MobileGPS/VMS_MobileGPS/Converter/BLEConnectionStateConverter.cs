using BA_MobileGPS.Core;

using System;
using System.Globalization;
using VMS_MobileGPS.Constant;
using VMS_MobileGPS.Service;

using Xamarin.Forms;

namespace VMS_MobileGPS.Converter
{
    public class BLEConnectionStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "KHÔNG KẾT NỐI";
            }

            var state = (BleConnectionState)value;

            switch (state)
            {
                case BleConnectionState.NO_CONNECTION:

                    return "KHÔNG KẾT NỐI";

                case BleConnectionState.CONNECTED:

                    return "ĐÃ KẾT NỐI";

                case BleConnectionState.PING_OK:
                    return "ĐÃ KẾT NỐI";

                default:
                    return "KHÔNG XÁC ĐỊNH";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class OfflineMapDeviceCodeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            string val = value.ToString();
            if (string.IsNullOrEmpty(val))
            {
                return string.Empty;
            }
            else
            {
                return val;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }


    public class BLENameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "KHÔNG XÁC ĐỊNH";

            string val = value.ToString();
            if (string.IsNullOrEmpty(val))
            {
                return "KHÔNG XÁC ĐỊNH";
            }
            else
            {
                return val;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class BLEConnectionStateColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = (Color)App.Current.Resources["PrimaryColor"];

            if (parameter != null)
            {
                color = (Color)parameter;
            }
            if (value == null)
            {
                return Color.FromHex("#e2e2e2");
            }

            var state = (BleConnectionState)value;

            switch (state)
            {
                case BleConnectionState.NO_CONNECTION:
                    return Color.FromHex("#e2e2e2");

                case BleConnectionState.CONNECTED:
                    return color;

                case BleConnectionState.PING_OK:

                    return color;

                default:
                    return Color.FromHex("#e2e2e2");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class BLEConnectionStateTextColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Color.FromHex("#898989");
            }

            var state = (BleConnectionState)value;

            switch (state)
            {
                case BleConnectionState.NO_CONNECTION:
                    return Color.FromHex("#898989");

                case BleConnectionState.CONNECTED:
                    return Color.White;

                case BleConnectionState.PING_OK:

                    return Color.White;

                default:
                    return Color.FromHex("#898989");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class SOSStateColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = (Color)App.Current.Resources["PrimaryColor"];

            if (parameter != null)
            {
                color = (Color)parameter;
            }
            if (value == null)
            {
                return color;
            }
            if ((bool)value)
            {
                return (Color)App.Current.Resources["DangerousColor"];
            }
            else
            {
                return color;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class SOSStateTextColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Color.FromHex("#898989");
            }
            if ((bool)value)
            {
                return Color.White;
            }
            else
            {
                return Color.FromHex("#898989");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    public class PermisisonTextColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Color.FromHex("#048BC5");
            }
            if ((bool)value)
            {
                return Color.White;
            }
            else
            {
                return Color.FromHex("#048BC5");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    public class PermissionBackgroundColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Color.White;
            }
            if ((bool)value)
            {
                return Color.FromHex("#048BC5"); ;
            }
            else
            {
                return Color.White;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    public class GrantPermissionEnableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return true;
            }
            if ((bool)value)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    public class OffMapBtnZoomInConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Color.White;
            if ((int)value >= GlobalResourcesVMS.Current.MaxOffMapZoom)
            {
                return Color.FromHex("26A1D9");
            }
            else return Color.White;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    public class OffMapBtnZoomInIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Color.FromHex("26A1D9");
            if ((int)value >= GlobalResourcesVMS.Current.MaxOffMapZoom)
            {
                return Color.White;
            }
            else return Color.FromHex("26A1D9");

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    public class OffMapBtnZoomOutConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Color.White;
            if ((int)value <= GlobalResourcesVMS.Current.MinOffMapZoom)
            {
                return Color.FromHex("26A1D9");
            }
            else return Color.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    public class OffMapBtnZoomOutIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Color.FromHex("26A1D9");
            if ((int)value <= GlobalResourcesVMS.Current.MinOffMapZoom)
            {
                return Color.White;
            }
            else return Color.FromHex("26A1D9");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}