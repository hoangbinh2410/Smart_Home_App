using BA_MobileGPS.Core;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;
using BA_MobileGPS.Utilities.Enums;
using Prism;
using Prism.Ioc;
using UNITEL_MobileGPS.Styles;
using Xamarin.Forms;

namespace UNITEL_MobileGPS
{
    public class UNITELApp : App
    {
        public UNITELApp(IPlatformInitializer initializer) : base(initializer)
        {
        }

        public override string OneSignalKey => base.OneSignalKey;

        protected async override void OnInitialized()
        {
            base.OnInitialized();
            ServerConfig.ServerIdentityHubType = ServerIdentityHubTypes.ServerUNITEL;
            ServerConfig.ServerVehicleOnlineHubType = ServerVehicleOnlineHubTypes.ServerUNITEL;
            ServerConfig.ServerAlertHubType = ServerAlertHubTypes.ServerUNITEL;
            ServerConfig.ServerUserBehaviorHubType = ServerUserBehaviorHubTypes.ServerUNITEL;
            ServerConfig.ApiEndpointTypes = ApiEndpointTypes.ServerUNITEL;

            Application.Current.Resources.MergedDictionaries.Add(new LightColor());
            Application.Current.Resources.MergedDictionaries.Add(new BA_MobileGPS.Core.Styles.Styles());

            //AppCenter.Start("ios=b9feff6c-5277-4e97-97e9-8a8e5c939eef;" +
            //       "android=db0089bc-c6e2-4df4-bead-0368ccef3cd6",
            //       typeof(Analytics), typeof(Crashes));

            //Nếu cài app lần đầu tiên hoặc có sự thay đổi dữ liệu trên server thì sẽ vào trang cập nhật thông tin vào localDB
            if (!Settings.IsFistInstallApp || Settings.IsChangeDataLocalDB)
            {
                _ = await NavigationService.NavigateAsync("InsertLocalDBPage");
            }
            else
            {
                _ = await NavigationService.NavigateAsync("LoginPage");
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterTypes(containerRegistry);
            AppType = BA_MobileGPS.Entities.AppType.Unitel;
            Settings.CurrentLanguage = CultureCountry.Laos;
            CurrentLanguage = CultureCountry.Laos;
            CultureHelper.SetCulture();
        }
    }
}