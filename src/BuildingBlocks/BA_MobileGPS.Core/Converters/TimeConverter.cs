using System;
using System.Globalization;

using Xamarin.Forms;

namespace BA_MobileGPS.Core
{
    //public class TimeConverter : IValueConverter
    //{
    //    public object Convert(object value,
    //                   Type targetType,
    //                   object parameter,
    //                   CultureInfo culture)
    //    {
    //        if (!(value is DateTime))
    //        {
    //            Logger.WriteError(MethodInfo.GetCurrentMethod().Name, "The target must be a DateTime");
    //        }

    //        var date = (DateTime)value;
    //        bool converToLocal = (string)parameter == "ToLocal";

    //        var result = converToLocal
    //                        ? date.ToLocalTime().ToString("HH:mm")
    //                        : date.ToString("HH:mm");
    //        return result;
    //    }

    //    public object ConvertBack(object value,
    //                              Type targetType,
    //                              object parameter,
    //                              CultureInfo culture)
    //    {
    //        return DateTime.Parse(value.ToString());
    //    }
    //}

    public class MinutesOfDrivingTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Format("{0} {1}", 0, "phút");
            }
            string stringValue = value.ToString();
            return string.Format("{0} {1}", stringValue, "phút");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}