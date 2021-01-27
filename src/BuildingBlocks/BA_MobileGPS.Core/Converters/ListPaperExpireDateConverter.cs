using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.Converters
{
    public class ListPaperExpireDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var expireDate = (DateTime)value;
            var day = (expireDate.Date - DateTime.Now).Days;
            if (day <= 0)
            {
                return "<font color=#F80A0A>Còn 0 ngày</font>";
            }
            else
            {
                var temp = (expireDate - new TimeSpan(CompanyConfigurationHelper.DayAllowRegister, 0, 0, 0)).Date;
                if (DateTime.Now.Date < temp)
                {
                    return string.Format("<font color=#1CB6E8>Còn {0} ngày</font>",day);
                }
                else
                {
                    return string.Format("<font color=#FF6C3E>Còn {0} ngày</font>",day);
                }
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
