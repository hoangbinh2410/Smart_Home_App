using BA_MobileGPS.Core.Resource;

using System;
using System.Globalization;

using Xamarin.Forms;

namespace BA_MobileGPS.Converter
{
    public class ShowConfigFavoriteConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Assembly assembly = typeof(Selection).GetTypeInfo().Assembly;
            if (value == null)
            {
                return false;
            }

            if (value.ToString().Equals(MobileResource.Menu_Label_Favorite))
                return true;
            else
                return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}