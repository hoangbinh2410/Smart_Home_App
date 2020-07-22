using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;

using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace BA_MobileGPS.Core
{
    public class MobileUserSettingHelper
    {
        public static string Get(MobileUserConfigurationNames key)
        {
            return Get(key, string.Empty);
        }

        public static T Get<T>(MobileUserConfigurationNames key, T defaultValue) where T : IConvertible
        {
            var val = defaultValue;
            try
            {
                if (StaticSettings.User != null && StaticSettings.User.MobileUserSetting.Count > 0)
                {
                    var setting = StaticSettings.User.MobileUserSetting.Find(x => x.Name == key.ToString());
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
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, string.Format("{0} with Key = {1} has an Exception: {2}", MethodBase.GetCurrentMethod().Name, key.ToString(), ex));
            }

            return val;
        }

        public static bool Set<T>(MobileUserConfigurationNames key, T value) where T : IConvertible
        {
            return Set(key.ToString(), value);
        }

        public static bool Set<T>(string key, T value) where T : IConvertible
        {
            var val = string.Empty;
            try
            {
                if (StaticSettings.User != null && StaticSettings.User.MobileUserSetting.Count > 0)
                {
                    var converter = TypeDescriptor.GetConverter(typeof(T));
                    if (converter != null)
                    {
                        // this will throw an exception when conversion is not possible
                        val = converter.ConvertToString(value);
                    }

                    var setting = StaticSettings.User.MobileUserSetting.FindIndex(x => x.Name == key);
                    if (setting >= 0)
                    {
                        StaticSettings.User.MobileUserSetting[setting].Value = val;
                    }
                    else
                    {
                        StaticSettings.User.MobileUserSetting.Add(new MobileUserSetting
                        {
                            Name = key,
                            Value = val
                        });
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, string.Format("{0} with Key = {1} has an Exception: {2}", MethodBase.GetCurrentMethod().Name, key.ToString(), ex));
                return false;
            }
        }

        public static int DefautLanguage => Get(MobileUserConfigurationNames.DefautLanguage, 1);

        public static string MenuFavorite => Get(MobileUserConfigurationNames.MenuFavorite, "1,6,61");

        public static string MenuReport => Get(MobileUserConfigurationNames.MenuReport, "");

        public static bool EnableShowCluster => Get(MobileUserConfigurationNames.EnableShowCluster, true);

        public static bool ShowNotification => Get(MobileUserConfigurationNames.ShowNotification, true);

        public static double LatCurrent
        {
            get
            {
                string config = Get(MobileUserConfigurationNames.MBLatitude);
                if (!string.IsNullOrEmpty(config))
                {
                    return FormatHelper.ConvertToDouble(config);
                }
                else
                {
                    return 0;
                }
            }
        }

        public static double LngCurrent
        {
            get
            {
                string config = Get(MobileUserConfigurationNames.MBLongitude);
                if (!string.IsNullOrEmpty(config))
                {
                    return FormatHelper.ConvertToDouble(config);
                }
                else
                {
                    return 0;
                }
            }
        }

        public static double LatCurrentScreenMap
        {
            get
            {
                string config = Get(MobileUserConfigurationNames.MBLatitude);
                if (!string.IsNullOrEmpty(config) && (config != "20.9735" && config != "20,9735"))
                {
                    return FormatHelper.ConvertToDouble(config);
                }
                else
                {
                    var item = StaticSettings.ListVehilceOnline?.FirstOrDefault(x => x.SortOrder == 1);
                    if (item != null)
                    {
                        return item.Lat;
                    }
                    else
                    {
                        if (StaticSettings.ListVehilceOnline != null && StaticSettings.ListVehilceOnline.Count > 0)
                        {
                            return StaticSettings.ListVehilceOnline[0].Lat;
                        }
                        else
                        {
                            return 20.973501;
                        }
                    }
                }
            }
        }

        public static double LngCurrentScreenMap
        {
            get
            {
                string config = Get(MobileUserConfigurationNames.MBLongitude);
                if (!string.IsNullOrEmpty(config) && (config != "105.847" && config != "105,847"))
                {
                    return FormatHelper.ConvertToDouble(config);
                }
                else
                {
                    var item = StaticSettings.ListVehilceOnline?.FirstOrDefault(x => x.SortOrder == 1);
                    if (item != null)
                    {
                        return item.Lng;
                    }
                    else
                    {
                        if (StaticSettings.ListVehilceOnline != null && StaticSettings.ListVehilceOnline.Count > 0)
                        {
                            return StaticSettings.ListVehilceOnline[0].Lng;
                        }
                        else
                        {
                            return 105.847021;
                        }
                    }
                }
            }
        }

        public static int MapType => Get(MobileUserConfigurationNames.MBMapType, 1);

        public static double Mapzoom => Get(MobileUserConfigurationNames.MBMapZoom, 7d);
    }
}