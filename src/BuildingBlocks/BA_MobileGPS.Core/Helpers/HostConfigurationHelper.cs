using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;

using System;
using System.ComponentModel;
using System.Reflection;

namespace BA_MobileGPS.Core.Helpers
{
    public class HostConfigurationHelper
    {
        /// <summary>
        /// Gets the host setting.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static string Get(HostConfigurationNames key)
        {
            return Get(key, string.Empty);
        }

        public static T Get<T>(HostConfigurationNames key, T defaultValue) where T : IConvertible
        {
            var val = defaultValue;
            try
            {
                if (StaticSettings.User != null && StaticSettings.User.HostConfigurations.Count > 0)
                {
                    var data = StaticSettings.User.HostConfigurations;

                    var setting = data.Find(x => x.Name == key.ToString());
                    if (setting != null)
                    {
                        var converter = TypeDescriptor.GetConverter(typeof(T));
                        if (converter != null)
                        {
                            // this will throw an exception when conversion is not possible
                            val = (T)converter.ConvertFromString(setting.Value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, string.Format("{0} with Key = {1} has an Exception: {2}", MethodInfo.GetCurrentMethod().Name, key.ToString(), ex));
            }

            return val;
        }

        public static string FilterWord => Get(HostConfigurationNames.FilterWord, string.Empty);

        public static int FeedbackNumber => Get(HostConfigurationNames.FeedbackNumber, 10);
    }
}