using BA_MobileGPS.Core;
using BA_MobileGPS.Utilities.Constant;
using BA_MobileGPS.Utilities.Enums;
using GISVIET_MobileGPS.Styles;
using GISVIET_MobileGPS.ViewModels;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Prism;
using Prism.Ioc;
using Xamarin.Forms;

namespace GISVIET_MobileGPS
{
    public class GISVIETApp : App
    {
        public GISVIETApp(IPlatformInitializer initializer) : base(initializer)
        {
        }

        public override string OneSignalKey => Config.OneSignalKey_VIVIEW;

        protected async override void OnInitialized()
        {
            base.OnInitialized();

            ServerConfig.ServerIdentityHubType = ServerIdentityHubTypes.ServerVIVIEW;
            ServerConfig.ServerVehicleOnlineHubType = ServerVehicleOnlineHubTypes.ServerVIVIEW;
            ServerConfig.ServerAlertHubType = ServerAlertHubTypes.ServerVIVIEW;
            ServerConfig.ApiEndpointTypes = ApiEndpointTypes.ServerVIVIEW;

            Application.Current.Resources.MergedDictionaries.Add(new LightColor());
            Application.Current.Resources.MergedDictionaries.Add(new BA_MobileGPS.Core.Styles.Styles());

            AppCenter.Start("ios=26e01862-0464-4767-994a-ccb280c938fe;" +
             "android=52f713d7-5e8f-4769-8341-f36243ab690c",
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

            AppType = BA_MobileGPS.Entities.AppType.Viview;

            containerRegistry.RegisterForNavigation<BA_MobileGPS.Core.Views.HelperPage, HeplerViewModel>("HelperPage");

            //containerRegistry.Register<ResourceDictionary, LightColor>(Theme.Light.ToString());
            //containerRegistry.Register<ResourceDictionary, DarkColor>(Theme.Dark.ToString());
            //containerRegistry.Register<ResourceDictionary, VIVIEW_MobileGPS.Styles.Custom>(Theme.Custom.ToString());

        }
    }
}