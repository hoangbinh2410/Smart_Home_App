using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;

using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;

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
        protected static string Get(MobileConfigurationNames key)
        {
            return Get(key, string.Empty);
        }

        protected static T Get<T>(MobileConfigurationNames key, T defaultValue) where T : IConvertible
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

        public static IDictionary<string, string> _DicMobileConfigurations = null;

        private static List<MobileConfiguration> _Configurations = null;

        /// <summary>
        /// Set dữ liệu cấu hình
        /// </summary>
        public static void SetData(List<MobileConfiguration> datas)
        {
            _Configurations = datas;
        }

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

                            // Nếu chưa set dữ liệu cấu hình  => Đọc Từ API, nếu có rồi thì thôi, tránh đọc lại API 2 lần
                            if (_Configurations == null)
                            {
                                // Đọc dữ liệu từ API
                                var service = Prism.PrismApplicationBase.Current.Container.Resolve<IMobileSettingService>();
                                _Configurations = await service.GetAllMobileConfigs(App.AppType);
                            }

                            if (_Configurations != null && _Configurations.Count > 0)
                            {
                                _Configurations.ForEach(item =>
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

        public static string WebGps => Get(MobileConfigurationNames.WebGps, "https://bagps.vn/");

        public static string Network => Get(MobileConfigurationNames.Network, "https://bagps.vn/mang-luoi");

        public static string LinkAppStore => Get(MobileConfigurationNames.LinkAppStore, "https://itunes.apple.com/app/id1332270275");

        public static string LinkCHPlay => Get(MobileConfigurationNames.LinkCHPlay, "https://play.google.com/store/apps/details?id=vn.vietnamcnn.gpsmobile");

        public static string LinkShareApp => Get(MobileConfigurationNames.LinkShareApp, "http://binhanh.vn/");

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

        public static bool IsUseNetwork => Get(MobileConfigurationNames.IsUseNetwork, true);

        public static bool IsUseBAGPSIntroduce => Get(MobileConfigurationNames.IsUseBAGPSIntroduce, true);

        public static bool IsUseRegisterSupport => Get(MobileConfigurationNames.IsUseRegisterSupport, true);

        public static bool IsUseForgotpassword => Get(MobileConfigurationNames.IsUseForgotpassword, false);

        public static bool IsUseVehicleDebtMoney => Get(MobileConfigurationNames.IsUseVehicleDebtMoney, true);

        public static bool IsStartOnlinePage => Get(MobileConfigurationNames.IsStartOnlinePage, true);

        public static int CountVehicleUsingCluster => Get(MobileConfigurationNames.CountVehicleUsingCluster, 500);

        public static int DefaultMinTimeLossGPS => Get(MobileConfigurationNames.DefaultMinTimeLossGPS, App.AppType == AppType.VMS ? 180 : 5);

        public static int DefaultMaxTimeLossGPS => Get(MobileConfigurationNames.DefaultMaxTimeLossGPS, App.AppType == AppType.VMS ? 240 : 150);

        public static int DefaultTimeLossConnect => Get(MobileConfigurationNames.DefaultTimeLossConnect, App.AppType == AppType.VMS ? 240 : 150);

        public static int TimeVehicleOffline => Get(MobileConfigurationNames.TimeVehicleOffline, 2);

        public static string LinkYoutube => Get(MobileConfigurationNames.LinkYoutube, "https://www.youtube.com/c/BAGPS");

        public static string LinkBAGPS => Get(MobileConfigurationNames.LinkBAGPS, "https://bagps.vn/");

        public static string LinkAdvertising => Get(MobileConfigurationNames.LinkAdvertising, "");

        public static int TimeVehicleSync => Get(MobileConfigurationNames.TimeVehicleSync, 2);

        public static int TimmerVehicleSync => Get(MobileConfigurationNames.TimmerVehicleSync, 60000);

        public static int TimeSleep => Get(MobileConfigurationNames.TimeSleep, 5);

        public static int Mapzoom => Get(MobileConfigurationNames.Mapzoom, 14);

        public static int ClusterMapzoom => Get(MobileConfigurationNames.ClusterMapzoom, 16);

        /// <summary>
        /// trungtq: Mức đồng bộ cho phần đồng bộ online
        /// Level0: không cần đồng bộ, chỉ cần mỗi Signalr là cân hết :)
        /// Level1: Đồng bộ 1 phần như của Namth đang làm (hiện tại vẫn bị)
        /// Level3: Đồng bộ tất, giống như web đang làm, hơi tốn máu nhưng an toàn
        /// </summary>
        /// <Modified>
        /// Name     Date         Comments
        /// trungtq  3/10/2020   created
        /// </Modified>
        public static SynOnlineLevelTypes SynOnlineLevel => Get(MobileConfigurationNames.SynOnlineLevel, SynOnlineLevelTypes.Level1);

        /// <summary>
        /// trungtq: có cần đồng bộ dạng request giống Web không? mặc định là có (true)
        /// </summary>
        /// <Modified>
        /// Name     Date         Comments
        /// trungtq  3/10/2020   created
        /// </Modified>
        public static bool EnableLongPoolRequest => Get(MobileConfigurationNames.EnableLongPoolRequest, true);

        public static bool VehicleOnlineAddressEnabled => Get(MobileConfigurationNames.VehicleOnlineAddressEnabled, true);

        public static bool UseHelper => Get(MobileConfigurationNames.UseHelper, true);

        public static bool UseUserBehavior => Get(MobileConfigurationNames.UseUserBehavior, true);

        public static int MinZoomLevelGoogleMap => Get(MobileConfigurationNames.MinZoomLevelGoogleMap, 5);

        public static bool UseCameraRTMP => Get(MobileConfigurationNames.UseUserBehavior, false);

        public static string LinkFacebook => Get(MobileConfigurationNames.LinkFacebook, "https://www.facebook.com/bagps.vn");

        public static string LinkZalo => Get(MobileConfigurationNames.LinkZalo, "https://zalo.me/1958838581480438876");

        public static string LinkTiktok => Get(MobileConfigurationNames.LinkTiktok, "https://www.tiktok.com/@bagps");

        public static string LinkExportVideoCamera => Get(MobileConfigurationNames.LinkExportVideoCamera, "http://192.168.43.1:8080");

        public static string LinkHelpExportVideoCamera => Get(MobileConfigurationNames.LinkHelpExportVideoCamera, "https://www.youtube.com/watch?v=3KHS015dexo");

    }
}