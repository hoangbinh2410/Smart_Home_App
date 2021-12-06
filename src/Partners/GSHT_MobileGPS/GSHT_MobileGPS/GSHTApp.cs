using BA_MobileGPS.Core;
using BA_MobileGPS.Utilities.Constant;
using BA_MobileGPS.Utilities.Enums;
using GSHT_MobileGPS.Core.Themes;
using GSHT_MobileGPS.Styles;
using GSHT_MobileGPS.ViewModels;
using GSHT_MobileGPS.Views;
using Prism;
using Prism.Ioc;
using Xamarin.Forms;

namespace GSHT_MobileGPS
{
    public class GSHTApp : App
    {
        public GSHTApp(IPlatformInitializer initializer) : base(initializer)
        {
        }

        public override string OneSignalKey => Config.OneSignalKey_GSHT;

        protected override async void OnInitialized()
        {
            base.OnInitialized();
            ServerConfig.ServerIdentityHubType = ServerIdentityHubTypes.ServerGSHT;
            ServerConfig.ServerVehicleOnlineHubType = ServerVehicleOnlineHubTypes.ServerGSHT;
            ServerConfig.ServerAlertHubType = ServerAlertHubTypes.ServerGSHT;
            ServerConfig.ApiEndpointTypes = ApiEndpointTypes.ServerGSHT;

            SetTheme();

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
            AppType = BA_MobileGPS.Entities.AppType.GSHT;
            containerRegistry.RegisterSingleton<IThemeGSHTServices, ThemeServices>();
            containerRegistry.Register<ResourceDictionary, ThemeDefault>(ThemeGSHT.ThemeDefault.ToString());
            containerRegistry.Register<ResourceDictionary, Theme1>(ThemeGSHT.Theme1.ToString());
            containerRegistry.Register<ResourceDictionary, Theme2>(ThemeGSHT.Theme2.ToString());
            containerRegistry.Register<ResourceDictionary, Theme3>(ThemeGSHT.Theme3.ToString());
            containerRegistry.Register<ResourceDictionary, Theme4>(ThemeGSHT.Theme4.ToString());
            containerRegistry.Register<ResourceDictionary, Theme5>(ThemeGSHT.Theme5.ToString());
            containerRegistry.Register<ResourceDictionary, Theme6>(ThemeGSHT.Theme6.ToString());
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>("LoginPage");
        }

        private void SetTheme()
        {
            var themeServices = Current.Container.Resolve<IThemeGSHTServices>();
            themeServices.ChangeTheme((ThemeGSHT)Settings.CurrentTheme);
        }
    }
}