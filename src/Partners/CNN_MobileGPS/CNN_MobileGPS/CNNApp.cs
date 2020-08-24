using BA_MobileGPS.Core;
using BA_MobileGPS.Core.Themes;
using BA_MobileGPS.Utilities.Constant;
using BA_MobileGPS.Utilities.Enums;
using CNN_MobileGPS.Styles;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Prism;
using Prism.Ioc;
using Xamarin.Forms;

namespace CNN_MobileGPS
{
    public class CNNApp : App
    {
        public CNNApp(IPlatformInitializer initializer) : base(initializer)
        {
        }

        public override string OneSignalKey => base.OneSignalKey;

        protected async override void OnInitialized()
        {
            base.OnInitialized();

            ServerConfig.ServerIdentityHubType = ServerIdentityHubTypes.ServerCNN;
            ServerConfig.ServerVehicleOnlineHubType = ServerVehicleOnlineHubTypes.ServerCNN;
            ServerConfig.ServerAlertHubType = ServerAlertHubTypes.ServerCNN;
            ServerConfig.ApiEndpointTypes = ApiEndpointTypes.ServerCNN;

            AppCenter.Start("ios=0e61c7a5-94be-4d89-b27d-ee7831e019ea;" +
                 "android=53aace3b-928b-49f0-8531-a7dca14754a5",
                 typeof(Analytics), typeof(Crashes));

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

            AppType = BA_MobileGPS.Entities.AppType.CNN;

            // Đăng ký config automapper
            AutoMapperConfig.RegisterMappings(containerRegistry);

            containerRegistry.Register<ResourceDictionary, LightColor>(Theme.Light.ToString());
            containerRegistry.Register<ResourceDictionary, DarkColor>(Theme.Dark.ToString());
            containerRegistry.Register<ResourceDictionary, CNN_MobileGPS.Styles.Custom>(Theme.Custom.ToString());
        }
    }
}