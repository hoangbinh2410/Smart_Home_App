using BA_MobileGPS.Entities.Enums;
using BA_MobileGPS.Utilities.Extensions;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace BA_MobileGPS.Core
{
    public class StatusIssueColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Color.FromHex("#00ADE5");
            }
            switch ((IssuesStatusEnums)value)
            {
                case IssuesStatusEnums.SendRequestIssue:
                    return Color.FromHex("#00ADE5");

                case IssuesStatusEnums.CSKHInReceived:
                    return Color.FromHex("#FFA502");

                case IssuesStatusEnums.EngineeringIsInprogress:
                    return Color.FromHex("#FFA502");

                case IssuesStatusEnums.Finish:
                    return Color.FromHex("#8BC34A");

                default:
                    return Color.FromHex("#00ADE5");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }
    }

    public class StatusIssueNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "";
            }
            return ((IssuesStatusEnums)value).ToDescription();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }
    }

    public class TimelineIssueColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Color.FromHex("#CED6E0");
            }
            if ((bool)value)
            {
                return Color.FromHex("#A2E8FF");
            }
            else
            {
                return Color.FromHex("#CED6E0");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }
    }


    public class IsNotFinishIssueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return false;
            }
            if ((IssuesStatusEnums)value == IssuesStatusEnums.Finish)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }
    }
}