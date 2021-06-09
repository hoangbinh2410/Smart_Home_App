using BA_MobileGPS.Core.Models;
using BA_MobileGPS.Entities;
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

    public class UploadStatusIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "ic_dowload.png";
            }
            else
            {
                var status = (VideoUploadStatus)value;
                switch (status)
                {
                    case VideoUploadStatus.Uploaded:
                        return "ic_dowload.png";

                    case VideoUploadStatus.Uploading:
                        return "ic_cloud_upload.png";

                    case VideoUploadStatus.WaitingUpload:
                        return "ic_time_black";

                    case VideoUploadStatus.UploadError:
                        return "ic_info_outline_white.png";

                    default:
                        return "ic_dowload.png";
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class UploadStatusColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return (Color)Application.Current.Resources["PrimaryColor"];
            }
            else
            {
                var status = (VideoUploadStatus)value;
                switch (status)
                {
                    case VideoUploadStatus.Uploaded:
                        return (Color)Application.Current.Resources["PrimaryColor"];

                    case VideoUploadStatus.Uploading:
                        return Color.FromHex("#00ADE5");

                    case VideoUploadStatus.WaitingUpload:
                        return (Color)Application.Current.Resources["TextSecondaryColor"];

                    case VideoUploadStatus.UploadError:
                        return (Color)Application.Current.Resources["DangerousColor"];

                    default:
                        return (Color)Application.Current.Resources["PrimaryColor"];
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class UploadIconPlacehoderColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Color.FromHex("#00ADE5");
            }
            else
            {
                var status = (VideoUploadStatus)value;
                switch (status)
                {
                    case VideoUploadStatus.Uploaded:
                        return Color.FromHex("#00ADE5");

                    case VideoUploadStatus.Uploading:
                        return Color.FromHex("#CED6E0");

                    case VideoUploadStatus.WaitingUpload:
                        return Color.FromHex("#CED6E0");

                    case VideoUploadStatus.UploadError:
                        return Color.FromHex("#CED6E0");

                    default:
                        return Color.FromHex("#00ADE5");
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}