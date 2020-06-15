using Syncfusion.SfImageEditor.XForms;

using System;
using System.Globalization;

using Xamarin.Forms;

namespace BA_MobileGPS.Converter
{
    public class ItemImageSavedEventArgsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var itemTappedEventArgs = value as ImageSavedEventArgs;
            if (itemTappedEventArgs == null)
            {
                throw new ArgumentException("Expected value to be of type ItemTappedEventArgs", nameof(value));
            }
            return itemTappedEventArgs;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}