using Syncfusion.SfChart.XForms;

using System;
using System.Globalization;

using Xamarin.Forms;

namespace BA_MobileGPS.Core
{
    public class ChartTrackballCreatedEventArgsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is ChartTrackballCreatedEventArgs args))
            {
                throw new ArgumentException("Expected value to be of type ChartTrackballCreatedEventArgs", nameof(value));
            }
            return args;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}