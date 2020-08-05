using BA_MobileGPS.Core;
using VIVIEW_MobileGPS.Styles;
using BA_MobileGPS.Utilities.Constant;
using BA_MobileGPS.Utilities.Enums;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Prism;
using Prism.Ioc;

namespace VIVIEW_MobileGPS
{
    public class VIVIEWApp : App
    {
        public VIVIEWApp(IPlatformInitializer initializer) : base(initializer)
        {
        }

        public override string OneSignalKey => base.OneSignalKey;

        protected async override void OnInitialized()
        {
            Resources.MergedDictionaries.Add(new DarkColor());
            Resources.MergedDictionaries.Add(new LightColor());
            Resources.MergedDictionaries.Add(new Colors());

            base.OnInitialized();

            ServerConfig.ServerIdentityHubType = ServerIdentityHubTypes.ServerVIVIEW;
            ServerConfig.ServerVehicleOnlineHubType = ServerVehicleOnlineHubTypes.ServerVIVIEW;
            ServerConfig.ServerAlertHubType = ServerAlertHubTypes.ServerVIVIEW;
            ServerConfig.ApiEndpointTypes = ApiEndpointTypes.ServerVIVIEW;

            AppCenter.Start("ios=26e01862-0464-4767-994a-ccb280c938fe;" +
             "android=52f713d7-5e8f-4769-8341-f36243ab690c",
                typeof(Analytics), typeof(Crashes));

            //Nếu cài app lần đầu tiên hoặc có sự thay đổi dữ liệu trên server thì sẽ vào trang cập nhật thông tin vào localDB
            if (!Settings.IsFistInstallApp || Settings.IsChangeDataLocalDB)
            {
                _ = await NavigationService.NavigateAsync("LoginPage");
            }
            else
            {
                _ = await NavigationService.NavigateAsync("LoginPage");
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterTypes(containerRegistry);

            AppType = BA_MobileGPS.Entities.AppType.CNN;

            // Đăng ký config automapper
            AutoMapperConfig.RegisterMappings(containerRegistry);
        }
    }
}