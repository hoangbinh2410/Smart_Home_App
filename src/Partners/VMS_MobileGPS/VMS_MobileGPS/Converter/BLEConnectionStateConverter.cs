using BA_MobileGPS.Core;
using Remotion.Linq.Clauses.StreamedData;
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
                return "Kết nối thiết bị";

            string val = value.ToString();
            if (string.IsNullOrEmpty(val))
            {
                return "Kết nối thiết bị";
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

    public class DeviceStateColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = (Color)App.Current.Resources["TextPrimaryColor"];

            if (parameter != null)
            {
                color = (Color)parameter;
            }
            if (value == null)
            {
                return Color.FromHex("#e2e2e2");
            }
            if (!string.IsNullOrEmpty(value.ToString()))
            {
                var msg = value.ToString();
                if (msg == "Mất tín hiệu" || msg == "Mất sóng GPS" || msg == "Mất sóng vệ tinh")
                {
                    color = (Color)App.Current.Resources["DangerousColor"];
                }
                else if (msg == "Tín hiệu yếu" || msg == "Sóng GPS yếu" || msg == "Sóng vệ tinh yếu")
                {
                    color = (Color)App.Current.Resources["WarningColor"];
                }
                else if (msg == "Đã kết nối")
                {
                    color = (Color)App.Current.Resources["GreenColor"];
                }
            }
            else
            {
                return color;
            }
            return color;
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
                return (Color)App.Current.Resources["TextPrimaryColor"];
            }
            if ((bool)value)
            {
                return (Color)App.Current.Resources["DangerousColor"];
            }
            else
            {
                return (Color)App.Current.Resources["TextPrimaryColor"];
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class SOSBackgroundColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Color.White;
            }
            if ((bool)value)
            {
                return Color.FromHex("#FFD8D8");
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
    public class PermisisonTextColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return (Color)App.Current.Resources["PrimaryColor"];
            }
            if ((bool)value)
            {
                return Color.White;
            }
            else
            {
                return (Color)App.Current.Resources["PrimaryColor"];
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
                return (Color)App.Current.Resources["PrimaryColor"];
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

    public class SOSLabelTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "Cảnh báo SOS";
            }
            if ((bool)value)
            {
                return "SOS ĐÃ BẬT";
            }
            else
            {
                return "Cảnh báo SOS";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class BLEConnectionStateImagesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "ic_vmsdevice_off.png";
            }

            var state = (BleConnectionState)value;

            switch (state)
            {
                case BleConnectionState.NO_CONNECTION:

                    return "ic_vmsdevice_off.png";

                case BleConnectionState.CONNECTED:

                    return "ic_vmsdevice.png";

                case BleConnectionState.PING_OK:
                    return "ic_vmsdevice.png";

                default:
                    return "ic_vmsdevice_off.png";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class BLEConnectionStateBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return false;
            }

            var state = (BleConnectionState)value;

            switch (state)
            {
                case BleConnectionState.NO_CONNECTION:

                    return false;

                case BleConnectionState.CONNECTED:

                    return true;

                case BleConnectionState.PING_OK:
                    return true;

                default:
                    return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}