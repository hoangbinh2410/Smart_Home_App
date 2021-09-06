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

        public static int DefaultTimeLossConnectOffPower => Get(CompanyConfigurationNames.DefaultTimeLossConnectOffPower, 5);

        public static int Vmin => Get(CompanyConfigurationNames.Vmin, 3);

        public static bool VehicleOnlineAddressEnabled => Get(CompanyConfigurationNames.VehicleOnlineAddressEnabled, MobileSettingHelper.VehicleOnlineAddressEnabled);

        public static int AlertMinBlockSMS => Get(CompanyConfigurationNames.AlertMinBlockSMS, 10);

        public static int CountDateOfPayment => Get(CompanyConfigurationNames.CountDateOfPayment, 60);

        public static bool IsShowConfigLanmark => Get(CompanyConfigurationNames.IsShowConfigLanmark, false);

        public static bool IsDisplayPopupSendEngineControl => Get(CompanyConfigurationNames.IsDisplayPopupSendEngineControl, false);

        public static int TimeVehicleSync => Get(CompanyConfigurationNames.TimeVehicleSync, MobileSettingHelper.TimeVehicleSync);

        public static int TimmerVehicleSync => Get(CompanyConfigurationNames.TimmerVehicleSync, MobileSettingHelper.TimmerVehicleSync);

        public static bool IsShowCoordinates => Get(CompanyConfigurationNames.IsShowCoordinates, false);

        public static bool IsShowTemperatureOnline => Get(CompanyConfigurationNames.IsShowTemperatureOnline, false);

        /// <summary>
        /// cấu hình có hiển thị ngày đăng kiểm trên online hay ko
        /// </summary>
        public static bool IsShowDateOfRegistration => Get(CompanyConfigurationNames.IsShowDateOfRegistration, false);

        /// <summary>
        /// cấu hình có hiển thị số km tích lũy trong tháng
        /// </summary>
        public static bool ShowKmInMonth => Get(CompanyConfigurationNames.ShowKmInMonth, false);

        /// <summary>
        /// trungtq: Mức đồng bộ cho phần đồng bộ online
        /// Level1: Đồng bộ 1 phần như của Namth đang làm (hiện tại vẫn bị)
        /// Level2: Đồng bộ tất, giống như web đang làm, hơi tốn máu nhưng an toàn
        /// </summary>
        /// <Modified>
        /// Name     Date         Comments
        /// trungtq  3/10/2020   created
        /// </Modified>
        public static SynOnlineLevelTypes SynOnlineLevel => Get(CompanyConfigurationNames.SynOnlineLevel, MobileSettingHelper.SynOnlineLevel);

        /// <summary>
        /// trungtq: Công ty có cấu hình đồng bộ Request không? nếu không có trong DB thì lấy theo hệ thống
        /// </summary>
        /// <Modified>
        /// Name     Date         Comments
        /// trungtq  3/10/2020   created
        /// </Modified>
        public static bool EnableLongPoolRequest => Get(CompanyConfigurationNames.EnableLongPoolRequest, MobileSettingHelper.EnableLongPoolRequest);

        /// <summary>
        /// Cấu hình công ty về số ngay cho phép tạo mới giấy tờ xe ở 
        /// form giấy tờ, mặc định là 30
        /// </summary>
        public static int DayAllowRegister => Get(CompanyConfigurationNames.DayAllowRegister, 30);

        public static bool UseNewSummaryIconOnline => Get(CompanyConfigurationNames.UseNewSummaryIconOnline, false);
    }
}