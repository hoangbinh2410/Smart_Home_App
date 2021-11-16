using BA_MobileGPS.Core.Themes;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Enums;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace BA_MobileGPS.Core
{
    public static class Settings
    {
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
        private static readonly int CurrentThemeDefault = (int)Theme.Light;

        private const string FavoritesVehicleImageKey = "FavoritesVehicleImageKey";
        private static readonly string FavoritesVehicleImageDefault = string.Empty;

        private const string FavoritesVehicleOnlineKey = "FavoritesVehicleOnlineKey";
        private static readonly string FavoritesVehicleOnlineDefault = string.Empty;

        private const string FavoritesIssueKey = "FavoritesIssueKey";
        private static readonly string FavoritesIssueDefault = string.Empty;

        private const string ShowViewVehicleImageKey = "ShowViewVehicleImageKey";
        private static readonly int ShowViewVehicleImageDefault = 10;

        private const string MapTypeKey = "MapTypeKey";
        private static readonly int MapTypeDefault = 2;

        public static float Latitude
        {
            get => Preferences.Get(IdLatitude, LatitudeDefault);
            set => Preferences.Set(IdLatitude, value);
        }

        public static float Longitude
        {
            get => Preferences.Get(IdLongitude, LongitudeDefault);
            set => Preferences.Set(IdLongitude, value);
        }

        /// <summary>
        /// Lưu thông tin đăng nhập
        /// </summary>
        public static string UserName
        {
            get => Preferences.Get(UserNameKey, UserNameDefault);
            set => Preferences.Set(UserNameKey, value);
        }

        /// <summary>
        /// Lưu thông tin mật khẩu
        /// </summary>
        public static string Password
        {
            get => Preferences.Get(PasswordKey, PasswordDefault);
            set => Preferences.Set(PasswordKey, value);
        }

        /// <summary>
        /// Lưu thông tin ghi nho mat khau
        /// </summary>
        public static bool Rememberme
        {
            get => Preferences.Get(RemembermeKey, RemembermeDefault);
            set => Preferences.Set(RemembermeKey, value);
        }

        /// <summary>
        /// ngôn ngữ hiện tại được chọn
        /// </summary>
        public static string CurrentLanguage
        {
            get => Preferences.Get(CurrentLanguageKey, CurrentLanguageDefault);
            set => Preferences.Set(CurrentLanguageKey, value);
        }

        /// <summary>
        /// công ty hiện tại
        /// </summary>
        public static Company CurrentCompany
        {
            get
            {
                Company company = null;
                string value = Preferences.Get(CurrentCompanyKey, CurrentCompanyDefault);
                if (!string.IsNullOrWhiteSpace(value))
                {
                    company = JsonConvert.DeserializeObject<Company>(value);
                }
                return company;
            }
            set => Preferences.Set(CurrentCompanyKey, JsonConvert.SerializeObject(value));
        }

        /// <summary>
        /// Đường dẫn download AppKH trên AppStore
        /// </summary>
        public static string AppLinkDownload
        {
            get => Preferences.Get(AppLinkDownloadKey, AppLinkDownloadDefault);
            set => Preferences.Set(AppLinkDownloadKey, value);
        }

        public static string AppVersionDB
        {
            get => Preferences.Get(AppVersionDBKey, AppVersionDBDefault);
            set => Preferences.Set(AppVersionDBKey, value);
        }

        /// <summary>
        ///Đánh dấu là lần đầu cài app
        /// </summary>
        public static bool IsFistInstallApp
        {
            get => Preferences.Get(IsFistInstallAppKey, IsFistInstallAppDefault);
            set => Preferences.Set(IsFistInstallAppKey, value);
        }

        /// <summary>
        ///Đánh dấu là có sự thay đổi dữ liệu trên database local cần cập nhập lại vào app
        /// </summary>
        public static bool IsChangeDataLocalDB
        {
            get => Preferences.Get(IsChangeDataLocalDBKey, IsChangeDataLocalDBDefault);
            set => Preferences.Set(IsChangeDataLocalDBKey, value);
        }

        /// <summary>
        /// Lưu thông tin Token hiện tại của Firebase
        /// </summary>
        public static string CurrentFirebaseToken
        {
            get => Preferences.Get(FirebaseToken, FirebaseTokenDefault);
            set => Preferences.Set(FirebaseToken, value);
        }

        public static string ReceivedNotificationType
        {
            get => Preferences.Get(ReceivedNotificationTypeKey, ReceivedNotificationTypeDefault);
            set => Preferences.Set(ReceivedNotificationTypeKey, value);
        }

        public static string ReceivedNotificationValue
        {
            get => Preferences.Get(ReceivedNotificationValueKey, ReceivedNotificationValueDefault);
            set => Preferences.Set(ReceivedNotificationValueKey, value);
        }

        public static string ReceivedNotificationTitle
        {
            get => Preferences.Get(ReceivedNotificationTitleKey, ReceivedNotificationTitleDefault);
            set => Preferences.Set(ReceivedNotificationTitleKey, value);
        }

        public static string LastImeiVMS
        {
            get => Preferences.Get(LastImeiVMSKey, LastImeiVMSDefault);
            set => Preferences.Set(LastImeiVMSKey, value);
        }

        public static string LastDeviceVMS
        {
            get => Preferences.Get(LastDeviceVMSKey, LastDeviceVMSDefault);
            set => Preferences.Set(LastDeviceVMSKey, value);
        }

        public static int NoticeIdWhenLogin
        {
            get => Preferences.Get(NoticeIdWhenLoginKey, NoticeIdWhenLoginDefault);
            set => Preferences.Set(NoticeIdWhenLoginKey, value);
        }

        public static int NoticeIdAfterLogin
        {
            get => Preferences.Get(NoticeIdAfterLoginKey, NoticeIdAfterLoginDefault);
            set => Preferences.Set(NoticeIdAfterLoginKey, value);
        }

        /// <summary>
        /// Load Map lan dau
        /// </summary>
        public static bool IsLoadedMap
        {
            get => Preferences.Get(LoadedMap, IsLoadedMapDefault);
            set => Preferences.Set(LoadedMap, value);
        }

        public static bool IsFirstPopup
        {
            get => Preferences.Get(FistInstallPopup, FistInstallPopupDefault);
            set => Preferences.Set(FistInstallPopup, value);
        }

        public static string TempVersionName
        {
            get => Preferences.Get(TempVersionNameKey, TempVersionNameKeyDefault);
            set => Preferences.Set(TempVersionNameKey, value);
        }

        public static int SortOrder
        {
            get => Preferences.Get(SortOrderKey, SortOrderKeyDefault);
            set => Preferences.Set(SortOrderKey, value);
        }

        public static int CurrentTheme
        {
            get => Preferences.Get(CurrentThemeKey, CurrentThemeDefault);
            set => Preferences.Set(CurrentThemeKey, value);
        }

        public static string FavoritesVehicleImage
        {
            get => Preferences.Get(FavoritesVehicleImageKey, FavoritesVehicleImageDefault);
            set => Preferences.Set(FavoritesVehicleImageKey, value);
        }

        public static string FavoritesVehicleOnline
        {
            get => Preferences.Get(FavoritesVehicleOnlineKey, FavoritesVehicleOnlineDefault);
            set => Preferences.Set(FavoritesVehicleOnlineKey, value);
        }

        public static string FavoritesIssue
        {
            get => Preferences.Get(FavoritesIssueKey, FavoritesIssueDefault);
            set => Preferences.Set(FavoritesIssueKey, value);
        }

        public static int ShowViewVehicleImage
        {
            get => Preferences.Get(ShowViewVehicleImageKey, ShowViewVehicleImageDefault);
            set => Preferences.Set(ShowViewVehicleImageKey, value);
        }

        public static int MapType
        {
            get => Preferences.Get(MapTypeKey, MobileUserSettingHelper.MapType == 4 || MobileUserSettingHelper.MapType == 5 ? 2 : 0);
            set => Preferences.Set(MapTypeKey, value);
        }
    }
}