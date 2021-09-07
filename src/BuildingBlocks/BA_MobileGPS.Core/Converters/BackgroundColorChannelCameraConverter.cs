using BA_MobileGPS.Core.Models;
using BA_MobileGPS.Core.Resources;
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

    public class UploadStatusIsEnableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return true;
            }
            else
            {
                var status = (VideoUploadStatus)value;
                switch (status)
                {
                    case VideoUploadStatus.NotUpload:
                        return true;

                    case VideoUploadStatus.Uploaded:
                        return false;

                    case VideoUploadStatus.Uploading:
                        return false;

                    case VideoUploadStatus.WaitingUpload:
                        return false;

                    case VideoUploadStatus.UploadErrorTimeout:
                    case VideoUploadStatus.UploadErrorDevice:
                    case VideoUploadStatus.UploadErrorCancel:
                        return true;

                    default:
                        return true;
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class UploadStatusIsVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return false;
            }
            else
            {
                var status = (VideoUploadStatus)value;
                switch (status)
                {
                    case VideoUploadStatus.NotUpload:
                        return false;

                    case VideoUploadStatus.Uploaded:
                        return true;

                    case VideoUploadStatus.Uploading:
                        return true;

                    case VideoUploadStatus.WaitingUpload:
                        return true;

                    case VideoUploadStatus.UploadErrorTimeout:
                    case VideoUploadStatus.UploadErrorDevice:
                    case VideoUploadStatus.UploadErrorCancel:
                        return true;

                    default:
                        return false;
                }
            }
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
                    case VideoUploadStatus.NotUpload:
                        return "";

                    case VideoUploadStatus.Uploaded:
                        return "ic_dowload.png";

                    case VideoUploadStatus.Uploading:
                        return "ic_cloud_upload.png";

                    case VideoUploadStatus.WaitingUpload:
                        return "ic_time_black";

                    case VideoUploadStatus.UploadErrorTimeout:
                    case VideoUploadStatus.UploadErrorDevice:
                    case VideoUploadStatus.UploadErrorCancel:
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

    public class UploadStatusIconUploadConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "ic_uploadsucces.png";
            }
            else
            {
                var status = (VideoUploadStatus)value;
                switch (status)
                {
                    case VideoUploadStatus.NotUpload:
                        return "";

                    case VideoUploadStatus.Uploaded:
                        return "ic_uploadsucces.png";

                    case VideoUploadStatus.Uploading:
                        return "ic_cloud_upload.png";

                    case VideoUploadStatus.WaitingUpload:
                        return "ic_time_black";

                    case VideoUploadStatus.UploadErrorTimeout:
                    case VideoUploadStatus.UploadErrorDevice:
                    case VideoUploadStatus.UploadErrorCancel:
                        return "ic_info_outline_white.png";

                    default:
                        return "ic_uploadsucces.png";
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class UploadStatusColorUploadConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Color.FromHex("#8BC34A");
            }
            else
            {
                var status = (VideoUploadStatus)value;
                switch (status)
                {
                    case VideoUploadStatus.Uploaded:
                        return Color.FromHex("#8BC34A");

                    case VideoUploadStatus.Uploading:
                        return Color.FromHex("#00ADE5");

                    case VideoUploadStatus.WaitingUpload:
                        return (Color)Application.Current.Resources["TextSecondaryColor"];

                    case VideoUploadStatus.UploadErrorTimeout:
                    case VideoUploadStatus.UploadErrorDevice:
                    case VideoUploadStatus.UploadErrorCancel:
                        return (Color)Application.Current.Resources["DangerousColor"];

                    default:
                        return Color.FromHex("#8BC34A");
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

                    case VideoUploadStatus.UploadErrorTimeout:
                    case VideoUploadStatus.UploadErrorDevice:
                    case VideoUploadStatus.UploadErrorCancel:
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

    public class UploadTextStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return MobileResource.Camera_Status_Downloaded;
            }
            else
            {
                var status = (VideoUploadStatus)value;
                switch (status)
                {
                    case VideoUploadStatus.Uploaded:
                        return MobileResource.Camera_Status_Downloaded;

                    case VideoUploadStatus.Uploading:
                        return MobileResource.Camera_Status_Downloading;

                    case VideoUploadStatus.WaitingUpload:
                        return MobileResource.Camera_Status_WaitingUpload;

                    case VideoUploadStatus.UploadErrorTimeout:
                    case VideoUploadStatus.UploadErrorDevice:
                    case VideoUploadStatus.UploadErrorCancel:
                        return MobileResource.Camera_Status_DownloadError;

                    default:
                        return MobileResource.Camera_Status_Downloaded;
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}