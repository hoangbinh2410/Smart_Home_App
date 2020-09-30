using BA_MobileGPS.Core.Views.Camera.MonitoringCamera;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace BA_MobileGPS.Core
{
    public class CameraTextBorderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color unSelected = Color.White;
            Color selected = Color.FromHex("#0C852E");
            if (value == null || parameter == null)
            {
                return unSelected;
            }
            if ((CameraEnum)value == (CameraEnum)parameter)
                return selected;
            else
                return unSelected;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }
    }
    public class TimeRemainVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
            {
                return false;
            }
            if ((CameraEnum)value == (CameraEnum)parameter)
                return true;
            else
                return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }
    }
}
