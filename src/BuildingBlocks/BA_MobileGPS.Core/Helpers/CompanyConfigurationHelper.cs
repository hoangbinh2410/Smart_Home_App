using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;

using System;
using System.ComponentModel;
using System.Reflection;

namespace BA_MobileGPS.Core
{
    public class CompanyConfigurationHelper
    {
        public static string Get(CompanyConfigurationNames key)
        {
            return Get(key, string.Empty);
        }

        public static T Get<T>(CompanyConfigurationNames key, T defaultValue) where T : IConvertible
        {
            var val = defaultValue;
            try
            {
                if (StaticSettings.User != null && StaticSettings.User.CompanyConfigurations != null && StaticSettings.User.CompanyConfigurations.Count > 0)
                {
                    var data = StaticSettings.User.CompanyConfigurations;

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
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, string.Format("{0} with Key = {1} has an Exception: {2}", MethodBase.GetCurrentMethod().Name, key.ToString(), ex));
            }

            return val;
        }

        public static int DefaultMinTimeLossGPS => Get(CompanyConfigurationNames.DefaultMinTimeLossGPS, App.AppType == AppType.VMS ? MobileSettingHelper.DefaultMinTimeLossGPS : 5);

        public static int DefaultMaxTimeLossGPS => Get(CompanyConfigurationNames.DefaultMaxTimeLossGPS, App.AppType == AppType.VMS ? MobileSettingHelper.DefaultMaxTimeLossGPS : 150);

        public static int DefaultMaxVelocityBlue => Get(CompanyConfigurationNames.DefaultMaxVelocityBlue, 80);

        public static int DefaultMaxVelocityOrange => Get(CompanyConfigurationNames.DefaultMaxVelocityOrange, 100);

        public static int DefaultTimeLossConnect => Get(CompanyConfigurationNames.DefaultTimeLossConnect, App.AppType == AppType.VMS ? MobileSettingHelper.DefaultTimeLossConnect : 150);

        public static int Vmin => Get(CompanyConfigurationNames.Vmin, 3);

        public static bool VehicleOnlineAddressEnabled => Get(CompanyConfigurationNames.VehicleOnlineAddressEnabled, false);

        public static int AlertMinBlockSMS => Get(CompanyConfigurationNames.AlertMinBlockSMS, 10);

        public static int CountDateOfPayment => Get(CompanyConfigurationNames.CountDateOfPayment, 60);

        public static bool IsShowConfigLanmark => Get(CompanyConfigurationNames.IsShowConfigLanmark, false);

        public static bool IsDisplayPopupSendEngineControl => Get(CompanyConfigurationNames.IsDisplayPopupSendEngineControl, false);
    }
}