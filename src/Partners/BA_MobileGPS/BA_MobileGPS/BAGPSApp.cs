using BA_MobileGPS.Core;
using BA_MobileGPS.Styles;
using BA_MobileGPS.Utilities.Constant;
using BA_MobileGPS.Utilities.Enums;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Prism;
using Prism.Ioc;

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
            Resources.MergedDictionaries.Add(new Colors());

            base.OnInitialized();

            ServerConfig.ServerTypes = ServerTypes.ServerThat;
            ServerConfig.ApiEndpointTypes = ApiEndpointTypes.ServerThat;

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

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterTypes(containerRegistry);

            AppType = BA_MobileGPS.Entities.AppType.BinhAnh;

            // Đăng ký config automapper
            AutoMapperConfig.RegisterMappings(containerRegistry);
        }
    }
}