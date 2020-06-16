using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;

using Prism.Ioc;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;

using Unity;

namespace BA_MobileGPS.Core
{
    /// <summary>
    /// Load toàn bộ thông tin cấu hình của App thông qua API
    /// </summary>
    /// <Modified>
    /// Name     Date         Comments
    /// namth  18/02/2019    created
    /// </Modified>
    public class MobileSettingHelper
    {
        /// <summary>
        /// Gets the host setting.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static string Get(MobileConfigurationNames key)
        {
            return Get(key, string.Empty);
        }

        public static T Get<T>(MobileConfigurationNames key, T defaultValue) where T : IConvertible
        {
            var val = defaultValue;
            try
            {
                IDictionary<string, string> dicConfigs = DicMobileConfigurations;

                // Neu dictionary chua key nay thi moi lay ra, neu ko tra ve gia tri mac dinh
                if (dicConfigs != null && dicConfigs.Count > 0 && dicConfigs.ContainsKey(key.ToString()))
                {
                    var setting = dicConfigs[key.ToString()].ToString();

                    var converter = TypeDescriptor.GetConverter(typeof(T));
                    if (converter != null)
                    {
                        // this will throw an exception when conversion is not possible
                        val = (T)converter.ConvertFromString(setting);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, string.Format("{0} with Key = {1} has an Exception: {2}", MethodInfo.GetCurrentMethod().Name, key.ToString(), ex));
            }

            return val;
        }

        private static IDictionary<string, string> _DicMobileConfigurations = null;

        public static IDictionary<string, string> DicMobileConfigurations
        {
            get
            {
                if (_DicMobileConfigurations == null)
                {
                    try
                    {
                        Task.Run(async () =>
                        {
                            _DicMobileConfigurations = new Dictionary<string, string>();

                            // Đọc dữ liệu từ API
                            var service = Prism.PrismApplicationBase.Current.Container.Resolve<IResourceService>();
                            var data = await service.GetAllMobileConfigs(App.AppType);

                            if (data != null && data.Count > 0)
                            {
                                data.ForEach(item =>
                                {
                                    if (!_DicMobileConfigurations.ContainsKey(item.Name))
                                    {
                                        _DicMobileConfigurations.Add(item.Name, item.Value);
                                    }
                                });
                            }
                        }).Wait();
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
                    }
                }
                return _DicMobileConfigurations;
            }
        }

        public static string HotlineGps => Get(MobileConfigurationNames.HotlineGps, "");

        public static string EmailSupport => Get(MobileConfigurationNames.EmailSupport, "");

        public static string HotlineTeleSaleGps => Get(MobileConfigurationNames.HotlineTeleSaleGps, "");

        public static string WebGps => Get(MobileConfigurationNames.WebGps, "https://gps.binhanh.vn/");

        public static string LinkAppStore => Get(MobileConfigurationNames.LinkAppStore, "https://itunes.apple.com/app/id1332270275");

        public static string LinkCHPlay => Get(MobileConfigurationNames.LinkCHPlay, "https://play.google.com/store/apps/details?id=vn.vietnamcnn.gpsmobile");

        public static string LinkShareApp => Get(MobileConfigurationNames.LinkShareApp, "https://play.google.com/store/apps/details?id=vn.com.xechieuve.customer");

        public static string LinkExperience => Get(MobileConfigurationNames.LinkExperience, "http://binhanh.vn/");

        public static bool ConfigUseAccountKit => Get(MobileConfigurationNames.ConfigUseAccountKit, true);

        public static int NumberOfDigitVerifyCode => Get(MobileConfigurationNames.NumberOfDigitVerifyCode, 4);

        public static int AuthenticationSecond => Get(MobileConfigurationNames.AuthenticationSecond, 5);

        public static string LengthAndPrefixNumberPhone => Get(MobileConfigurationNames.LengthAndPrefixNumberPhone, "10-09,086,088,089,020,032,033,034,035,036,037,038,039,070,079,077,076,078,083,084,085,081,082,056,058,059");

        public static string ConfigDangerousCharTextBox => Get(MobileConfigurationNames.ConfigDangerousChar, "['\"<>/&]");

        public static int ConfigUnitDebtMoney => Get(MobileConfigurationNames.ConfigUnitDebtMoney, 1000000);

        public static int ConfigPageNextReport => Get(MobileConfigurationNames.ConfigPageNextReport, 0);

        public static int ConfigPageSizeReport => Get(MobileConfigurationNames.ConfigPageSizeReport, 20);

        public static int ConfigTotalCountDefault => Get(MobileConfigurationNames.ConfigTotalCountDefault, 0);

        public static bool ConfigIsShowDataAfterLoadFormReport => Get(MobileConfigurationNames.ConfigIsShowDataAfterLoadFormReport, false);

        public static string ConfigTitleDefaultReport => Get(MobileConfigurationNames.ConfigTitleDefaultReport, "ReportGPSMobile.xlsx");

        public static bool ConfigIsEnableButtonExport => Get(MobileConfigurationNames.ConfigIsEnableButtonExport, false);

        public static int ConfigCountMinutesShowMessageReport => Get(MobileConfigurationNames.ConfigCountMinutesShowMessageReport, 3000);

        public static int ViewReport => Get(MobileConfigurationNames.ViewReport, 15);

        public static string CopyRight => Get(MobileConfigurationNames.CopyRight, "@ Copyright by BinhAnh Electronic Co., Ltd");

        public static string UserNameToken => Get(MobileConfigurationNames.UserNameToken, "GetToken");

        public static string PasswordToken => Get(MobileConfigurationNames.PasswordToken, "xE*CV#Vn_%f%zZemV?FvtjumhGeV$ZJH6gPGh8x");

        public static bool IsUseExperience => Get(MobileConfigurationNames.IsUseExperience, true);

        public static bool IsUseForgotpassword => Get(MobileConfigurationNames.IsUseForgotpassword, true);

        public static bool IsUseVehicleDebtMoney => Get(MobileConfigurationNames.IsUseVehicleDebtMoney, true);

        public static bool IsStartOnlinePage => Get(MobileConfigurationNames.IsStartOnlinePage, true);

        public static int CountVehicleUsingCluster => Get(MobileConfigurationNames.CountVehicleUsingCluster, 500);

        public static int DefaultMinTimeLossGPS => Get(MobileConfigurationNames.DefaultMinTimeLossGPS, App.AppType == AppType.VMS ? 180 : 5);

        public static int DefaultMaxTimeLossGPS => Get(MobileConfigurationNames.DefaultMaxTimeLossGPS, App.AppType == AppType.VMS ? 240 : 150);

        public static int DefaultTimeLossConnect => Get(MobileConfigurationNames.DefaultTimeLossConnect, App.AppType == AppType.VMS ? 240 : 150);

        public static int TimeVehicleOffline => Get(MobileConfigurationNames.TimeVehicleOffline, 2);
    }
}