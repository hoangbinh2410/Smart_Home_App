using BA_MobileGPS.Utilities;

using System;
using System.Globalization;

using Xamarin.Forms;
using Xamarin.Forms.Extensions;

namespace BA_MobileGPS.Core
{
    public class DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = string.Empty;

            if (value is DateTime date1)
            {
                return (string)parameter == "ToLocal" ? date1.ToLocalTime().ToString("HH:mm:ss dd/MM/yyyy") : date1.FormatDateTimeWithSecond();
            }

            if (value is DateTimeOffset date2)
            {
                return (string)parameter == "ToLocal" ? date2.DateTime.ToLocalTime().ToString("HH:mm:ss dd/MM/yyyy") : date2.DateTime.FormatDateTimeWithSecond();
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return default;
        }
    }

    public class DateTimeViewConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = string.Empty;

            if (value is DateTime date1)
            {
                return (string)parameter == "ToLocal" ? date1.ToLocalTime().ToString("HH:mm dd/MM/yyyy") : date1.FormatDateTime();
            }

            if (value is DateTimeOffset date2)
            {
                return (string)parameter == "ToLocal" ? date2.DateTime.ToLocalTime().ToString("HH:mm dd/MM/yyyy") : date2.DateTime.FormatDateTime();
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return default;
        }
    }

    public class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = string.Empty;

            if (!(value is DateTime) && !(value is DateTimeOffset))
            {
                return result;
            }

            DateTime dt = default;

            if (value is DateTime dateTime)
            {
                if (dateTime == DateTime.MinValue)
                {
                    return string.Empty;
                }
                dt = dateTime;
            }
            else if (value is DateTimeOffset dateTimeOffset)
            {
                dt = dateTimeOffset.DateTime;
            }

            result = (string)parameter == "ToLocal" ? dt.ToLocalTime().ToString("dd/MM/yyyy") : dt.FormatDate();

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return default;
        }
    }

    public class MonthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = string.Empty;

            if (!(value is DateTime) && !(value is DateTimeOffset))
            {
                return result;
            }

            DateTime dt = default;

            if (value is DateTime dateTime)
            {
                if (dateTime == DateTime.MinValue)
                {
                    return string.Empty;
                }
                dt = dateTime;
            }
            else if (value is DateTimeOffset dateTimeOffset)
            {
                dt = dateTimeOffset.DateTime;
            }

            result = dt.ToLocalTime().ToString("MM/yyyy");

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return default;
        }
    }

    public class TimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = string.Empty;

            if (!(value is DateTime) && !(value is DateTimeOffset))
            {
                return result;
            }

            DateTime dt = default;

            if (value is DateTime dateTime)
            {
                dt = dateTime;
            }
            else if (value is DateTimeOffset dateTimeOffset)
            {
                dt = dateTimeOffset.DateTime;
            }

            result = (string)parameter == "ToLocal" ? dt.ToLocalTime().FormatTime() : dt.FormatTime();

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return default;
        }
    }

    public class DateTimeOffsetConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTimeOffset dt)
            {
                return dt.ToLocalTime().DateTime;
            }
            return default;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dt)
            {
                return new DateTimeOffset(dt).ToUniversalTime();
            }
            return default;
        }
    }

    public class DateLocaleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = string.Empty;

            if (!(value is DateTime) && !(value is DateTimeOffset))
            {
                return result;
            }

            DateTime dt = default;

            if (value is DateTime dateTime)
            {
                dt = dateTime;
            }
            else if (value is DateTimeOffset dateTimeOffset)
            {
                dt = dateTimeOffset.DateTime.ToLocalTime();
            }

            if (dt.Date.Equals(DateTime.Today))
            {
                return "Hôm nay";
            }
            else if (dt.Date.Equals(DateTime.Today.Subtract(TimeSpan.FromDays(1))))
            {
                return "Hôm qua";
            }

            return dt.FormatDate();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return default;
        }
    }

    public class DateTimeLocaleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = string.Empty;

            if (!(value is DateTime) && !(value is DateTimeOffset))
            {
                return result;
            }

            DateTime dt = default;

            if (value is DateTime dateTime)
            {
                dt = dateTime;
            }
            else if (value is DateTimeOffset dateTimeOffset)
            {
                dt = dateTimeOffset.DateTime.ToLocalTime();
            }

            if (dt.Date.Equals(DateTime.Today))
            {
                return "Hôm nay";
            }
            else if (dt.Date.Equals(DateTime.Today.Subtract(TimeSpan.FromDays(1))))
            {
                return "Hôm qua";
            }

            return dt.FormatDateTime();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return default;
        }
    }

    public class TimeSecondConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = string.Empty;

            if (!(value is DateTime))
            {
                result = "";
            }
            else
            {
                result = ((DateTime)value).FormatTime(includeSecond: true);
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return default;
        }
    }

    public class DateLocalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = string.Empty;

            if (!(value is DateTime))
            {
                result = "";
            }
            else
            {
                var date = (DateTime)value;

                result = date.FormatOnlyDate();
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return default;
        }
    }

    public class TodayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = string.Empty;

            if (!(value is DateTime))
            {
                result = "";
            }
            else
            {
                var date = (DateTime)value;

                if (date.DayOfYear != DateTime.Now.DayOfYear)
                {
                    result = date.FormatDateTimeWithSecond();
                }
                else
                {
                    result = date.FormatTime(includeSecond: true);
                }
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return default;
        }
    }

    public class SecondsToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is int result))
            {
                return "";
            }
            else
            {
                return result.SecondsToString();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return default;
        }
    }

    public class SecondsToStringShortConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is int result))
            {
                return "";
            }
            else
            {
                return result.SecondsToStringShort();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return default;
        }
    }

    public class MinutesToStringShortConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is int result))
            {
                return "";
            }
            else
            {
                return result.MinutesToStringShort();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return default;
        }
    }

    public class NumberToTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var res = string.Empty;
            if (value == null)
            {
                return res;
            }
            else
            {
                try
                {
                    var totalSeconds = System.Convert.ToInt32(value);
                    int seconds = totalSeconds % 60;
                    int minutes = totalSeconds / 60;
                    string time = minutes + ":" + seconds;
                    res = time;
                }
                catch (Exception)
                {
                }
                return res;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return default;
        }
    }

    public class FormatTimeSpan24h : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var res = string.Empty;
            TimeSpan time = default;
            try
            {
                if (value is TimeSpan timeSpan)
                {
                    time = timeSpan;
                    if (time.Days > 0)
                    {
                        var hour = time.Hours;
                        hour += (time.Days * 24);
                        res = hour.ToString() + ":" + time.Minutes;
                    }
                    else
                    {
                        res = time.ToString(@"hh\:mm");
                    }
                }              
            }
            catch (Exception)
            {
                return string.Empty;
            }
            return res;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return default;
        }
    }
}