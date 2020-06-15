using System;
using System.Globalization;

using Xamarin.Forms;

namespace BA_MobileGPS.Core
{
    public class SelectedComboxEventArgsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Syncfusion.XForms.ComboBox.SelectionChangedEventArgs selectionChangedEventArgs))
            {
                throw new ArgumentException("Expected value to be of type SelectionChangedEventArgs", nameof(value));
            }
            return selectionChangedEventArgs;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ComboBoxSelectionChangingEventArgsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Syncfusion.XForms.ComboBox.SelectionChangingEventArgs selectionChangingEventArgs))
            {
                throw new ArgumentException("Expected value to be of type SelectionChangingEventArgs", nameof(value));
            }
            return selectionChangingEventArgs;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}