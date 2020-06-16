using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;

namespace BA_MobileGPS.Core.Helpers
{
    public static class LoggerHelper
    {
        public static void WriteLog(string name, string content)
        {
            var _dictionary = new Dictionary<string, string>(1);

            _dictionary[name] = $"[{DateTime.Now.ToString()}]:" + content;

            Analytics.TrackEvent(name, _dictionary);
        }

        public static void WriteError(string method, Exception exception)
        {
            var properties = new Dictionary<string, string> {
                { "METHOD", method },
                };
            Crashes.TrackError(exception, properties);
        }
    }
}