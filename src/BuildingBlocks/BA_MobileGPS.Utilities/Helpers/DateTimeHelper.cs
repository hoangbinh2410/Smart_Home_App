using System;
using System.Globalization;
using System.Threading;

namespace BA_MobileGPS.Utilities
{
    /// <summary>
    /// Class contain DateTime utility
    /// </summary>
    /// <Modified>
    /// Name     Date         Comments
    /// Namth  2/20/2012   created
    /// </Modified>
    public static class DateTimeHelper
    {
        /// <summary>MM_dd_yyyy: "MM/dd/yyyy"</summary>
        public const string MmDdYyyy = "MM/dd/yyyy";

        /// <summary>yyyy_MM_dd: "yyyy/MM/dd"</summary>
        public const string YyyyMmDd = "yyyy/MM/dd";

        /// <summary>dd_MM_yyyy: "dd/MM/yyyy"</summary>
        public const string DdMmYyyy = "dd/MM/yyyy";

        /// <summary>12 Hour: "h:mm tt"</summary>
        public const string HMmTt = "h:mm tt";

        /// <summary>12 Hour: "h:mm:ss tt"</summary>
        public const string HMmSsTt = "h:mm:ss tt";

        /// <summary>24 Hour: "HH:mm"</summary>
        public const string HhMm = "HH:mm";

        /// <summary>24 Hour: "HH:mm:ss"</summary>
        public const string HhMmSs = "HH:mm:ss";

        /// <summary>HH_mm_ss_dd_MM_yyyy: "10:30:01 16/12/2013"</summary>
        public const string HhMmSsDdMmYyyy = "HH:mm:ss dd/MM/yyyy";

        public const string HhMmSsMmDdYyyy = "HH:mm:ss MM/dd/yyyy";

        /// <summary>HH_mm_ss_dd_MM_yyyy: "10:30:01 16/12/2013"</summary>
        public const string HhMmDdMmYyyy = "HH:mm dd/MM/yyyy";

        public const string HHmmMMddyyyy = "HH:mm MM/dd/yyyy";

        /// <summary>
        /// Gets the min system date.
        /// </summary>
        /// <Modified>
        /// Name     Date         Comments
        /// Namth  6/4/2012   created
        /// </Modified>
        public static DateTime MinSystemDate => new DateTime(2007, 1, 1);

        /// <summary>
        /// Check a string is a Date Type
        /// </summary>
        /// <param name="date"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static DateTime? ParseDate(string date, CultureInfo culture)
        {
            try { return DateTime.ParseExact(date, culture.DateTimeFormat.ShortDatePattern, culture); }
            catch { return null; }
        }

        /// <summary>
        /// Check a string is a Date Type in Vietnamese culture.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime? ParseDate(string date)
        {
            CultureInfo culture = Thread.CurrentThread.CurrentCulture;
            return ParseDate(date, culture);
        }

        /// <summary>
        /// Parse ngay thang theo format dau vao.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="format">The format.</param>
        /// <param name="provider">The provider.</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// Namth  28/7/2012   created
        /// </Modified>
        public static DateTime? ParseExactDateTime(string date, string format, IFormatProvider provider)
        {
            try { return DateTime.ParseExact(date, format, provider); }
            catch(Exception) { return null; }
        }

        /// <summary>
        /// Parse ngay thang theo format dau vao.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// Namth  26/9/2012   created
        /// </Modified>
        public static DateTime? ParseExactDateTime(string date, string format)
        {
            return ParseExactDateTime(date, format, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Check time and date valid
        /// </summary>
        /// <param name="time">Time</param>
        /// <param name="date">Date</param>
        /// <param name="culture">Culture vi-VN or en-US</param>
        /// <returns></returns>
        public static DateTime? ParseDateTime(string time, string date, CultureInfo culture)
        {
            try
            {
                var dt = string.IsNullOrEmpty(time) ? date : $"{time} {date}";
                return DateTime.Parse(dt, culture, DateTimeStyles.NoCurrentDateDefault);
            }
            catch
            {
                return null;
            }
        }

        public static DateTime ToDateTime(this string datetime, char dateSpliter = '-', char timeSpliter = ':', char millisecondSpliter = ',')
        {
            try
            {
                datetime = datetime.Trim();
                datetime = datetime.Replace("  ", " ");
                string[] body = datetime.Split(' ');
                string[] date = body[0].Split(dateSpliter);
                int year = int.Parse(date[2]);
                int month = int.Parse(date[1]);
                int day = int.Parse(date[0]);
                int hour = 0, minute = 0, second = 0, millisecond = 0;
                if (body.Length == 2)
                {
                    string[] tpart = body[1].Split(millisecondSpliter);
                    string[] time = tpart[0].Split(timeSpliter);
                    hour = int.Parse(time[0]);
                    minute = int.Parse(time[1]);
                    if (time.Length == 3) second = int.Parse(time[2]);
                    if (tpart.Length == 2) millisecond = int.Parse(tpart[1]);
                }
                return new DateTime(year, month, day, hour, minute, second, millisecond);
            }
            catch
            {
                return DateTime.Now;
            }
        }

        /// <summary>
        /// Parses the date time.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// Namth  2/7/2012   created
        /// </Modified>
        public static DateTime? ParseDateTime(string time, string date)
        {
            CultureInfo culture = Thread.CurrentThread.CurrentCulture;
            return ParseDateTime(time, date, culture);
        }

        /// <summary>
        /// Doi so phut ve dang gio:phut.
        /// </summary>
        /// <param name="minutes">The minutes.</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// Namth  7/7/2012   created
        /// </Modified>
        public static string ConvertTime(object minutes)
        {
            string ret = string.Empty;
            try
            {
                if (minutes == null) return ret;

                int minute = Convert.ToInt32(minutes);

                ret = $"{(minute / 60)}h:{(minute % 60)}";
            }
            catch
            {
                ret = string.Empty;
            }
            return ret;
        }

        /// <summary>
        /// Doi so phut ve dang gio:phut.
        /// </summary>
        /// <param name="minutes">The minutes.</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// sonnl  21/03/2015   created
        /// </Modified>
        public static string ConvertTimeQcvn(object minutes)
        {
            string ret = string.Empty;
            try
            {
                if (minutes == null) return ret;

                int minute = Convert.ToInt32(minutes);

                int hour = minute / 60;
                var hourStr = hour.ToString();
                if (hour < 10)
                {
                    hourStr = "0" + hourStr;
                }

                minute = minute % 60;
                var minuteStr = minute.ToString();

                if (minute < 10)
                {
                    minuteStr = "0" + minuteStr;
                }

                ret = $"{hourStr}:{minuteStr}";
            }
            catch
            {
                ret = string.Empty;
            }
            return ret;
        }

        /// <summary>
        /// Doi so giay ve dang gio:phut:giay.
        /// </summary>
        /// <param name="secondObj">The seconds.</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// sonnl  21/03/2015   created
        /// </Modified>
        public static string ConvertTimeQcvnFromSeconds(object secondObj)
        {
            string ret = string.Empty;
            try
            {
                if (secondObj == null) return ret;

                int seconds = Convert.ToInt32(secondObj);

                int minutes = seconds / 60;
                seconds = seconds % 60;

                int hour = minutes / 60;
                minutes = minutes % 60;

                var hourStr = hour.ToString();
                if (hour < 10)
                {
                    hourStr = "0" + hourStr;
                }

                var minuteStr = minutes.ToString();
                if (minutes < 10)
                {
                    minuteStr = "0" + minuteStr;
                }

                var secondStr = seconds.ToString();
                if (seconds < 10)
                {
                    secondStr = "0" + secondStr;
                }

                ret = $"{hourStr}:{minuteStr}:{secondStr}";
            }
            catch
            {
                ret = string.Empty;
            }
            return ret;
        }

        /// <summary>
        /// Dinh dang lai ngay thang ( so sanh 2 ngay khac nhau)
        /// Neu trong ngay -> Hien thi HH:mm
        /// Neu khac ngay  -> Hien thi HH:mm dd/MM/yyyy
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <param name="fromDate">The date time.</param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// Namth  10/8/2012   created
        /// </Modified>
        public static string DisplayDateTime(object dateTime, DateTime fromDate, DateTime toDate)
        {
            string ret = string.Empty;
            try
            {
                DateTime date = DateTime.Parse(dateTime.ToString());

                if (date <= MinSystemDate) return ret;

                string formattime;
                string formatdate = string.Empty;

                CultureInfo culture = Thread.CurrentThread.CurrentCulture;

                //Neu 2 ngay so sanh bang nhau -> Hien thi HH:mm
                if (fromDate.Date.Equals(toDate.Date))
                {
                    formattime = HhMm;
                }
                else //Neu 2 ngay so sanh khac nhau hien thi HH:mm dd/MM/yyyy
                {
                    if (culture.TwoLetterISOLanguageName.Equals("vi", StringComparison.InvariantCultureIgnoreCase))
                    {
                        formattime = HhMm;
                        formatdate = DdMmYyyy;
                    }
                    else
                    {
                        formattime = HhMm;
                        formatdate = MmDdYyyy;
                    }
                }

                string formatString = (!string.IsNullOrEmpty(formatdate)) ? $"{formattime} {formatdate}" : formattime;

                ret = date.ToString(formatString);
            }
            catch
            {
                // ignored
            }
            return ret;
        }

        /// <summary>
        /// Truyen vao date time, neu bang null
        /// </summary>
        /// <param name="dt">The date.</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// Namth  8/9/2012   created
        /// </Modified>
        public static string FormatDate(this object dt)
        {
            return FormatDate(dt, string.Empty);
        }

        /// <summary>
        /// Formats the date.
        /// </summary>
        /// <Modified>
        /// Name     Date         Comments
        /// Namth  22/6/2012   created
        /// </Modified>
        public static string FormatDate(object dt, string defaultValue)
        {
            string ret;
            try
            {
                CultureInfo culture = Thread.CurrentThread.CurrentCulture;
                string formatRegion = MmDdYyyy;     //Định dạng ở VN
                if (culture.Name == "vi" || culture.Name == "vi-VN")
                    formatRegion = DdMmYyyy;
                DateTime date = DateTime.Parse(dt.ToString());
                ret = (date == DateTime.MinValue || date == MinSystemDate) ? defaultValue : date.ToString(formatRegion);
            }
            catch
            {
                ret = string.Empty;
            }
            return ret;
        }

        /// <summary>
        /// Định dạng ngày tháng
        /// HH:mm dd/MM/yyyy
        /// </summary>
        /// <param name="dt">The date.</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// Namth  22/6/2012   created
        /// </Modified>
        public static string FormatDateTime(this object dt)
        {
            string ret;
            try
            {
                DateTime date = DateTime.Parse(dt.ToString());
                CultureInfo culture = Thread.CurrentThread.CurrentCulture;
                string formatRegion = HHmmMMddyyyy;
                //Định dạng ở VN
                if (culture.Name == "vi" || culture.Name == "vi-VN")
                    formatRegion = HhMmDdMmYyyy;
                ret = (date == DateTime.MinValue || date == MinSystemDate || date.Date == DateTime.MaxValue.Date) ? string.Empty : date.ToString(formatRegion);
            }
            catch
            {
                ret = string.Empty;
            }
            return ret;
        }

        /// <summary>
        /// Định dạng ngày tháng
        /// HH:mm dd/MM/yyyy
        /// </summary>
        /// <param name="dt">The date.</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// Namth  22/6/2012   created
        /// </Modified>
        public static string FormatOnlyDate(this object dt)
        {
            string ret;
            try
            {
                DateTime date = DateTime.Parse(dt.ToString());
                CultureInfo culture = Thread.CurrentThread.CurrentCulture;
                string formatRegion = MmDdYyyy;
                if (culture.Name == "vi" || culture.Name == "vi-VN")
                    formatRegion = DdMmYyyy; //Định dạng ở VN
                ret = (date == DateTime.MinValue || date == MinSystemDate || date.Date == DateTime.MaxValue.Date) ? string.Empty : date.ToString(formatRegion);
            }
            catch
            {
                ret = string.Empty;
            }
            return ret;
        }

        public static string FormatDateTimeWithSecond(this object dt)
        {
            string ret;
            try
            {
                DateTime date = DateTime.Parse(dt.ToString());
                CultureInfo culture = Thread.CurrentThread.CurrentCulture;
                string formatRegion = HhMmSsMmDdYyyy;
                if (culture.Name == "vi" || culture.Name == "vi-VN")
                    formatRegion = HhMmSsDdMmYyyy;
                ret = (date == DateTime.MinValue || date == MinSystemDate || date.Date == DateTime.MaxValue.Date) ? string.Empty : date.ToString(formatRegion);
            }
            catch
            {
                ret = string.Empty;
            }
            return ret;
        }

        /// <summary>
        /// Định dạng ngày tháng
        /// HH:mm dd/MM/yyyy
        /// </summary>
        /// <Modified>
        /// Name     Date         Comments
        /// Namth  22/6/2012   created
        /// </Modified>
        public static string FormatDateTimeWithoutSecond(object dt)
        {
            string ret;
            try
            {
                DateTime date = DateTime.Parse(dt.ToString());

                ret = (date == DateTime.MinValue || date == MinSystemDate || date.Date == DateTime.MaxValue.Date)
                    ? string.Empty : date.ToString(HhMmDdMmYyyy);
            }
            catch
            {
                ret = string.Empty;
            }
            return ret;
        }

        /// <summary>
        /// Chuyển unix time to DateTime
        /// </summary>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// quochv  15/12/2014   created
        /// </Modified>
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        /// <summary>
        /// Chuyển DateTime sang unix time
        /// </summary>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// quochv  15/12/2014   created
        /// </Modified>
        public static double DateTimeToUnixTimestamp(DateTime dateTime)
        {
            return (dateTime - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds;
        }

        public static int DateTimeToUnixTimestampUtc(DateTime dateTime)
        {
            var span = (dateTime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));
            double unixTime = span.TotalSeconds;
            return (int)unixTime;
        }

        /// <summary>
        /// Formats the time.
        /// </summary>
        /// <param name="isRestrictMinDate">The date.</param>
        /// <param name="dt"></param>
        /// <param name="includeSecond"></param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// sonnl  25/03/2017   created
        /// </Modified>
        public static string FormatTime(this object dt, bool isRestrictMinDate = true, bool includeSecond = false)
        {
            try
            {
                string ret = string.Empty;
                try
                {
                    DateTime date = DateTime.Parse(dt.ToString());
                    if (isRestrictMinDate)
                    {
                        if (date <= new DateTime(2007, 1, 1)) return ret;
                    }
                    ret = date.ToString(!includeSecond ? HhMm : HhMmSs);
                }
                catch
                {
                    ret = string.Empty;
                }
                return ret;
            }
            catch (Exception)
            {
                // ignored
            }
            return string.Empty;
        }

        /// <summary>
        /// Convert dạng datetimeoffset sang datetime
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// hoangdt  3/30/2019   created
        /// </Modified>
        public static DateTimeOffset ConvertDatetimeOffsetToDatetime(DateTime date)
        {
            try
            {
                var result = DateTimeOffset.Parse(date.ToString(), CultureInfo.CurrentCulture);
                return result;
            }
            catch (Exception)
            {
                return new DateTimeOffset();
            }
        }

        /// <summary>
        /// Convert dạng datetime sang datetimeoffset
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// hoangdt  3/30/2019   created
        /// </Modified>
        public static DateTime ConvertDatetimeToDatetimeOffset(DateTimeOffset date)
        {
            try
            {
                var result = DateTime.Parse(date.ToString(), CultureInfo.CurrentCulture);
                return result;
            }
            catch (Exception)
            {
                return new DateTime();
            }
        }

        /// <summary>
        /// Dinh dang lai ngay thang ( so sanh 2 ngay khac nhau)
        /// Neu trong ngay -> Hien thi HH:mm - HH:mm dd/MM/yyyy
        /// Neu khac ngay  -> Hien thi HH:mm dd/MM/yyyy - HH:mm dd/MM/yyyy
        /// </summary>
        /// <Modified>
        /// Name     Date         Comments
        /// Hoangdt  10/8/2012   created
        /// </Modified>
        public static string DisplayDateTimeVer2(DateTime fromDate, DateTime? toDate, string culture)
        {
            string ret = string.Empty;
            try
            {
                var ToDate = toDate == null ? DateTime.Now : (DateTime)toDate;
                var checklanguage = culture == CultureCountry.Vietnamese ? true : false;

                // chỉnh lại định dạng ngày
                string formattime = HhMm;
                string formatdate = DisplayFormatDate(culture);
                //Neu 2 ngay so sanh bang nhau -> Hien thi HH:mm
                if (fromDate.Date.Equals(ToDate.Date))
                {
                    ret = string.Format("{0} - {1} {2}", fromDate.ToString(formattime), ToDate.ToString(formattime), fromDate.ToString(formatdate));
                }
                else //Neu 2 ngay so sanh khac nhau hien thi HH:mm dd/MM/yyyy
                {
                    ret = string.Format("{0} {1} - {2} {3}", fromDate.ToString(formattime), fromDate.ToString(formatdate), ToDate.ToString(formattime), ToDate.ToString(formatdate));
                }
            }
            catch
            {
                // ignored
            }
            return ret;
        }

        private static string DisplayFormatDateTime(string culture)
        {
            var result = string.Empty;
            string formattime = string.Empty;
            string formatdate = string.Empty;
            var checklanguage = culture == CultureCountry.Vietnamese ? true : false;
            if (checklanguage)
            {
                formattime = HhMm;
                formatdate = DdMmYyyy;
            }
            else
            {
                formattime = HhMm;
                formatdate = MmDdYyyy;
            }
            result = string.Format("{0} {1}", formatdate, formattime);
            return result;
        }

        private static string DisplayFormatDate(string culture)
        {
            string formatdate = string.Empty;
            var checklanguage = culture == CultureCountry.Vietnamese ? true : false;
            if (checklanguage)
            {
                formatdate = DdMmYyyy;
            }
            else
            {
                formatdate = MmDdYyyy;
            }
            return formatdate;
        }

        public static string FormatTimeSpan24h(TimeSpan time)
        {
            try
            {
                if (time.Days > 0)
                {
                    var hour = time.Hours;
                    hour += (time.Days * 24);
                    return hour.ToString() + ":" + time.Minutes;
                }
                else
                {
                    return time.ToString(@"hh\:mm");
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }

        }
    }
}