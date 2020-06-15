using System;
using System.Globalization;

using Xamarin.Forms;

namespace BA_MobileGPS.Converter
{
    public class SelectionBoolToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Assembly assembly = typeof(Selection).GetTypeInfo().Assembly;
#if COMMONSB
            if ((bool)value)
                return ImageSource.FromResource("SampleBrowser.Icons.Selected.png", assembly);
            else
                return ImageSource.FromResource("SampleBrowser.Icons.NotSelected.png", assembly);
#else
            if ((bool)value)
                return ImageSource.FromResource("selected.png");
            else
                return ImageSource.FromResource("notSelected.png");
#endif
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}