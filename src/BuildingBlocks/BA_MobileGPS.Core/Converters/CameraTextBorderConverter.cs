using BA_MobileGPS.Core.Views.Camera.MonitoringCamera;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.Converters
{
    public class CameraTextBorderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //var param = (CameraSelectedEnum)parameter;
            Color unSelected = Color.FromHex("#DBFF00");
            Color selected = Color.FromHex("#03BE0B");
            if (value == null || parameter == null)
            {
                return unSelected;
            }
            if ((CameraSelectedEnum)value == (CameraSelectedEnum)parameter)
                return selected;
            else
                return unSelected;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }
    }
}
