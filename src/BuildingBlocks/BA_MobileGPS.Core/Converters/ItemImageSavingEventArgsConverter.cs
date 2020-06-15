using Syncfusion.SfImageEditor.XForms;

using System;
using System.Globalization;

using Xamarin.Forms;

namespace BA_MobileGPS.Converter
{
    public class ItemImageSavingEventArgsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is ImageSavingEventArgs itemTappedEventArgs))
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