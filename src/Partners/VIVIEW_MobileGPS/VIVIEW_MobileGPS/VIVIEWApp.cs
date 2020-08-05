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
            try
            {
                Resources.MergedDictionaries.Add(new DarkColor());
                Resources.MergedDictionaries.Add(new LightColor());
                Resources.MergedDictionaries.Add(new Colors());

                base.OnInitialized();

                ServerConfig.ServerIdentityHubType = ServerIdentityHubTypes.ServerVIVIEW;
                ServerConfig.ServerVehicleOnlineHubType = ServerVehicleOnlineHubTypes.ServerVIVIEW;
                ServerConfig.ServerAlertHubType = ServerAlertHubTypes.ServerVIVIEW;
                ServerConfig.ApiEndpointTypes = ApiEndpointTypes.ServerVIVIEW;

                AppCenter.Start("ios=b9feff6c-5277-4e97-97e9-8a8e5c939eef;" +
                       "android=db0089bc-c6e2-4df4-bead-0368ccef3cd6",
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
            catch (System.Exception ex)
            {

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