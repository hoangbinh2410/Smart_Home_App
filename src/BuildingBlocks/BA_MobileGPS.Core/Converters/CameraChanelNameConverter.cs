using BA_MobileGPS.Core.Resources;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace BA_MobileGPS.Core
{
    public class CameraChanelNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }
            var res = value.ToString();
            return string.Format("{0} {1}", MobileResource.Camera_Lable_Channel, res);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }
    }
}