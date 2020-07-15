using Syncfusion.XForms.Buttons;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace BA_MobileGPS.Core
{
    public class SwitchStateChangedEventArgsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is SwitchStateChangedEventArgs args))
            {
                throw new ArgumentException("Expected value to be of type StateChangedEventArgs", nameof(value));
            }
            return args;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}