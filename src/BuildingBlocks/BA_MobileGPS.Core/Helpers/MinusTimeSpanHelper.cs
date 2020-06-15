using System;

namespace BA_MobileGPS.Core
{
    public static class MinusTimeSpanHelper
    {
        public static string MinusTimeSpan(DateTime endTime, DateTime startTime)
        {
            try
            {
                var time = endTime.Subtract(startTime);
                if (time.Days > 0)
                {
                    var hour = time.Hours;
                    hour += (time.Days * 24);
                    return hour.ToString() + ":" + time.ToString(@"mm\:ss");
                }
                else
                {
                    return time.ToString(@"hh\:mm\:ss");
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string MinusTimeSpan(TimeSpan time)
        {
            try
            {
                if (time.Days > 0)
                {
                    var hour = time.Hours;
                    hour += (time.Days * 24);
                    return hour.ToString() + ":" + time.ToString(@"mm\:ss");
                }
                else
                {
                    return time.ToString(@"hh\:mm\:ss");
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}