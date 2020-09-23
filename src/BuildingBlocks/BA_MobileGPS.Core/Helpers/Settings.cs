using BA_MobileGPS.Core.Themes;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Enums;
using Newtonsoft.Json;

using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace BA_MobileGPS.Core
{
    public static class Settings
    {
        private static ISettings AppSettings => CrossSettings.Current;

        private const string IdLatitude = "latitude";
        private static readonly float LatitudeDefault = 20.973993f;

        private const string IdLongitude = "longitude";
        private static readonly float LongitudeDefault = 105.846421f;

        private const string UserNameKey = "user_name_key";
        private static readonly string UserNameDefault = string.Empty;

        private const string PasswordKey = "password_key";
        private static readonly string PasswordDefault = string.Empty;

        private const string RemembermeKey = "remember_me_key";
        private static readonly bool RemembermeDefault = false;

        private const string CurrentLanguageKey = "current_language_key";
        private static readonly string CurrentLanguageDefault = CultureCountry.Vietnamese;

        private const string CurrentCompanyKey = "current_company_key";
        private static readonly string CurrentCompanyDefault = string.Empty;

        private const string AppVersionDBKey = "AppVersionDBKey";
        private static readonly string AppVersionDBDefault = string.Empty;

        private const string AppLinkDownloadKey = "AppLinkDownloadKey";
        private static readonly string AppLinkDownloadDefault = string.Empty;

        private const string IsFistInstallAppKey = "IsFistInstallAppKey";
        private static readonly bool IsFistInstallAppDefault = false;

        private const string IsChangeDataLocalDBKey = "IsChangeDataLocalDBKey";
        private static readonly bool IsChangeDataLocalDBDefault = false;

        private const string LastImeiVMSKey = "LastImeiVMSKey";
        private static readonly string LastImeiVMSDefault = string.Empty;

        private const string LastDeviceVMSKey = "LastDeviceVMSKey";
        private static readonly string LastDeviceVMSDefault = string.Empty;

        private const string FirebaseToken = "firebase_token";
        private static readonly string FirebaseTokenDefault = string.Empty;

        private const string ReceivedNotificationTypeKey = "ReceivedNotificationTypeKey";
        private static readonly string ReceivedNotificationTypeDefault = string.Empty;

        private const string ReceivedNotificationValueKey = "ReceivedNotificationValueKey";
        private static readonly string ReceivedNotificationValueDefault = string.Empty;

        private const string ReceivedNotificationTitleKey = "ReceivedNotificationTitleKey";
        private static readonly string ReceivedNotificationTitleDefault = string.Empty;

        private const string NoticeIdWhenLoginKey = "NoticeIdWhenLoginKey";
        private static readonly int NoticeIdWhenLoginDefault = 0;

        private const string NoticeIdAfterLoginKey = "NoticeIdAfterLoginKey";
        private static readonly int NoticeIdAfterLoginDefault = 0;

        private const string LoadedMap = "LoadMap";
        private static readonly bool IsLoadedMapDefault = false;

        private const string FistInstallPopup = "FisrtPopup";
        private static readonly bool FistInstallPopupDefault = true;

        private const string TempVersionNameKey = "TempVersionNameKey";
        private static readonly string TempVersionNameKeyDefault = string.Empty;

        private const string SortOrderKey = "SortOrderKey";
        private static readonly int SortOrderKeyDefault = (int)SortOrderType.DefaultDES;

        private const string CurrentThemeKey = "CurrentThemeKey";
        private static readonly string CurrentThemeDefault = Theme.Light.ToString();

        private const string LastViewVehicleImageKey = "LastViewVehicleImageKey";
        private static readonly string LastViewVehicleImageDefault = string.Empty;

        public static float Latitude
        {
            get => AppSettings.GetValueOrDefault(IdLatitude, LatitudeDefault);
            set => AppSettings.AddOrUpdateValue(IdLatitude, value);
        }

        public static float Longitude
        {
            get => AppSettings.GetValueOrDefault(IdLongitude, LongitudeDefault);
            set => AppSettings.AddOrUpdateValue(IdLongitude, value);
        }

        /// <summary>
        /// Lưu thông tin đăng nhập
        /// </summary>
        public static string UserName
        {
            get => AppSettings.GetValueOrDefault(UserNameKey, UserNameDefault);
            set => AppSettings.AddOrUpdateValue(UserNameKey, value);
        }

        /// <summary>
        /// Lưu thông tin mật khẩu
        /// </summary>
        public static string Password
        {
            get => AppSettings.GetValueOrDefault(PasswordKey, PasswordDefault);
            set => AppSettings.AddOrUpdateValue(PasswordKey, value);
        }

        /// <summary>
        /// Lưu thông tin ghi nho mat khau
        /// </summary>
        public static bool Rememberme
        {
            get => AppSettings.GetValueOrDefault(RemembermeKey, RemembermeDefault);
            set => AppSettings.AddOrUpdateValue(RemembermeKey, value);
        }

        /// <summary>
        /// ngôn ngữ hiện tại được chọn
        /// </summary>
        public static string CurrentLanguage
        {
            get => AppSettings.GetValueOrDefault(CurrentLanguageKey, CurrentLanguageDefault);
            set => AppSettings.AddOrUpdateValue(CurrentLanguageKey, value);
        }

        /// <summary>
        /// công ty hiện tại
        /// </summary>
        public static Company CurrentCompany
        {
            get
            {
                Company company = null;
                string value = AppSettings.GetValueOrDefault(CurrentCompanyKey, CurrentCompanyDefault);
                if (!string.IsNullOrWhiteSpace(value))
                {
                    company = JsonConvert.DeserializeObject<Company>(value);
                }
                return company;
            }
            set => AppSettings.AddOrUpdateValue(CurrentCompanyKey, JsonConvert.SerializeObject(value));
        }

        /// <summary>
        /// Đường dẫn download AppKH trên AppStore
        /// </summary>
        public static string AppLinkDownload
        {
            get => AppSettings.GetValueOrDefault(AppLinkDownloadKey, AppLinkDownloadDefault);
            set => AppSettings.AddOrUpdateValue(AppLinkDownloadKey, value);
        }

        public static string AppVersionDB
        {
            get => AppSettings.GetValueOrDefault(AppVersionDBKey, AppVersionDBDefault);
            set => AppSettings.AddOrUpdateValue(AppVersionDBKey, value);
        }

        /// <summary>
        ///Đánh dấu là lần đầu cài app
        /// </summary>
        public static bool IsFistInstallApp
        {
            get => AppSettings.GetValueOrDefault(IsFistInstallAppKey, IsFistInstallAppDefault);
            set => AppSettings.AddOrUpdateValue(IsFistInstallAppKey, value);
        }

        /// <summary>
        ///Đánh dấu là có sự thay đổi dữ liệu trên database local cần cập nhập lại vào app
        /// </summary>
        public static bool IsChangeDataLocalDB
        {
            get => AppSettings.GetValueOrDefault(IsChangeDataLocalDBKey, IsChangeDataLocalDBDefault);
            set => AppSettings.AddOrUpdateValue(IsChangeDataLocalDBKey, value);
        }

        /// <summary>
        /// Lưu thông tin Token hiện tại của Firebase
        /// </summary>
        public static string CurrentFirebaseToken
        {
            get => AppSettings.GetValueOrDefault(FirebaseToken, FirebaseTokenDefault);
            set => AppSettings.AddOrUpdateValue(FirebaseToken, value);
        }

        public static string ReceivedNotificationType
        {
            get => AppSettings.GetValueOrDefault(ReceivedNotificationTypeKey, ReceivedNotificationTypeDefault);
            set => AppSettings.AddOrUpdateValue(ReceivedNotificationTypeKey, value);
        }

        public static string ReceivedNotificationValue
        {
            get => AppSettings.GetValueOrDefault(ReceivedNotificationValueKey, ReceivedNotificationValueDefault);
            set => AppSettings.AddOrUpdateValue(ReceivedNotificationValueKey, value);
        }

        public static string ReceivedNotificationTitle
        {
            get => AppSettings.GetValueOrDefault(ReceivedNotificationTitleKey, ReceivedNotificationTitleDefault);
            set => AppSettings.AddOrUpdateValue(ReceivedNotificationTitleKey, value);
        }

        public static string LastImeiVMS
        {
            get => AppSettings.GetValueOrDefault(LastImeiVMSKey, LastImeiVMSDefault);
            set => AppSettings.AddOrUpdateValue(LastImeiVMSKey, value);
        }

        public static string LastDeviceVMS
        {
            get => AppSettings.GetValueOrDefault(LastDeviceVMSKey, LastDeviceVMSDefault);
            set => AppSettings.AddOrUpdateValue(LastDeviceVMSKey, value);
        }

        public static int NoticeIdWhenLogin
        {
            get => AppSettings.GetValueOrDefault(NoticeIdWhenLoginKey, NoticeIdWhenLoginDefault);
            set => AppSettings.AddOrUpdateValue(NoticeIdWhenLoginKey, value);
        }

        public static int NoticeIdAfterLogin
        {
            get => AppSettings.GetValueOrDefault(NoticeIdAfterLoginKey, NoticeIdAfterLoginDefault);
            set => AppSettings.AddOrUpdateValue(NoticeIdAfterLoginKey, value);
        }

        /// <summary>
        /// Load Map lan dau
        /// </summary>
        public static bool IsLoadedMap
        {
            get => AppSettings.GetValueOrDefault(LoadedMap, IsLoadedMapDefault);
            set => AppSettings.AddOrUpdateValue(LoadedMap, value);
        }

        public static bool IsFirstPopup
        {
            get => AppSettings.GetValueOrDefault(FistInstallPopup, FistInstallPopupDefault);
            set => AppSettings.AddOrUpdateValue(FistInstallPopup, value);
        }

        public static string TempVersionName
        {
            get => AppSettings.GetValueOrDefault(TempVersionNameKey, TempVersionNameKeyDefault);
            set => AppSettings.AddOrUpdateValue(TempVersionNameKey, value);
        }

        public static int SortOrder
        {
            get => AppSettings.GetValueOrDefault(SortOrderKey, SortOrderKeyDefault);
            set => AppSettings.AddOrUpdateValue(SortOrderKey, value);
        }

        public static string CurrentTheme
        {
            get => AppSettings.GetValueOrDefault(CurrentThemeKey, CurrentThemeDefault);
            set => AppSettings.AddOrUpdateValue(CurrentThemeKey, value);
        }

        public static string LastViewVehicleImage
        {
            get => AppSettings.GetValueOrDefault(LastViewVehicleImageKey, LastViewVehicleImageDefault);
            set => AppSettings.AddOrUpdateValue(LastViewVehicleImageKey, value);
        }
    }
}