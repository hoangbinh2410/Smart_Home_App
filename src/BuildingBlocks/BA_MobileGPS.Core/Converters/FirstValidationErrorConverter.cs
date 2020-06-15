using BA_MobileGPS.Utilities;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

using Xamarin.Forms;

namespace BA_MobileGPS.Core
{
    public class FirstValidationErrorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ICollection<string> errors = value as ICollection<string>;
            return errors != null && errors.Count > 0 ? errors.ElementAt(0) : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Logger.WriteError(MethodInfo.GetCurrentMethod().Name, new NotImplementedException());
            throw new NotImplementedException();
        }
    }
}