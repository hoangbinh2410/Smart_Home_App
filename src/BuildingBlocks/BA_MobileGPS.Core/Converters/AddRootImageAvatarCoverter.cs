using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace BA_MobileGPS.Core
{
    public class AddRootImageAvatarCoverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "avatar_default.png";
            }

            if (string.IsNullOrEmpty(value.ToString()))
                return "avatar_default.png";
            else
                if (!value.ToString().Contains("/"))
            {
                return value.ToString();
            }
            else
            {
                return $"{value.ToString()}";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
