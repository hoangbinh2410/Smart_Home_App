using BA_MobileGPS.Core;
using BA_MobileGPS.Styles;
using BA_MobileGPS.Utilities.Constant;
using BA_MobileGPS.Utilities.Enums;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Prism;
using Prism.Ioc;
using Xamarin.Forms;

namespace BA_MobileGPS
{
    public class BAGPSApp : App
    {
        public BAGPSApp(IPlatformInitializer initializer) : base(initializer)
        {
        }

        public override string OneSignalKey => base.OneSignalKey;

        protected async override void OnInitialized()
        {
            base.OnInitialized();
            ServerConfig.ServerIdentityHubType = ServerIdentityHubTypes.ServerThat;
            ServerConfig.ServerVehicleOnlineHubType = ServerVehicleOnlineHubTypes.ServerThat;
            ServerConfig.ServerAlertHubType = ServerAlertHubTypes.ServerThat;
            ServerConfig.ServerUserBehaviorHubType = ServerUserBehaviorHubTypes.ServerThat;
            ServerConfig.ApiEndpointTypes = ApiEndpointTypes.ServerTest;

            Application.Current.Resources.MergedDictionaries.Add(new LightColor());
            Application.Current.Resources.MergedDictionaries.Add(new BA_MobileGPS.Core.Styles.Styles());

            AppCenter.Start("ios=b9feff6c-5277-4e97-97e9-8a8e5c939eef;" +
                   "android=db0089bc-c6e2-4df4-bead-0368ccef3cd6",
                   typeof(Analytics), typeof(Crashes));

            //Nếu cài app lần đầu tiên hoặc có sự thay đổi dữ liệu trên server thì sẽ vào trang cập nhật thông tin vào localDB
            if (!Settings.IsFistInstallApp || Settings.IsChangeDataLocalDB)
            {
                _ = await NavigationService.NavigateAsync("InsertLocalDBPage");
            }
            else
            {
                _ = await NavigationService.NavigateAsync("/SupportClientPage");
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterTypes(containerRegistry);
            AppType = BA_MobileGPS.Entities.AppType.BinhAnh;
        }
    }
}