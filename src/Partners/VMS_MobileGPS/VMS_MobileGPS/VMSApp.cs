using BA_MobileGPS.Core;
using BA_MobileGPS.Core.Themes;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;
using BA_MobileGPS.Utilities.Enums;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Prism;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Mvvm;
using VMS_MobileGPS.Styles;
using VMS_MobileGPS.ViewModels;
using VMS_MobileGPS.Views;
using Xamarin.Forms;

namespace VMS_MobileGPS
{
    public class VMSApp : App
    {
        public VMSApp(IPlatformInitializer initializer) : base(initializer)
        {
        }

        public override string OneSignalKey => Config.OneSignalKey_BASAT;

        protected override IContainerExtension CreateContainerExtension()
        {
            return ContainerLocator.Current;
        }

        protected async override void OnInitialized()
        {         
            base.OnInitialized();

            ServerConfig.ServerIdentityHubType = ServerIdentityHubTypes.ServerVMS;
            ServerConfig.ServerVehicleOnlineHubType = ServerVehicleOnlineHubTypes.ServerVMS;
            ServerConfig.ServerAlertHubType = ServerAlertHubTypes.ServerVMS;
            ServerConfig.ApiEndpointTypes = ApiEndpointTypes.ServerVMS;
            Application.Current.Resources.MergedDictionaries.Add(new LightColor());
            Application.Current.Resources.MergedDictionaries.Add(new BA_MobileGPS.Core.Styles.Styles());

            AppCenter.Start("ios=9a0650ec-057e-4e5a-b8de-4c3fd1fae415;" +
                "android=28d78b27-4b62-42e5-8db5-8e2d50de6a3a",
                typeof(Analytics), typeof(Crashes));

            AppManager.Init();

            await NavigationService.NavigateAsync("/NavigationPage/OfflinePage");
        }
      
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterTypes(containerRegistry);

            AppType = BA_MobileGPS.Entities.AppType.VMS;
          
            containerRegistry.Register<IMessageService, MessageService>();
            containerRegistry.Register<IFishShipService, FishShipService>();
            containerRegistry.Register<ISOSHistoryService, SOSHistoryService>();
            containerRegistry.Register<IVehicleDebtBlockService, VehicleDebtBlockService>();

            containerRegistry.Register<IServicePackageHistoryService, ServicePackageHistoryService>();

            containerRegistry.RegisterForNavigation<OfflinePage, OfflinePageViewModel>("OfflinePage");
            containerRegistry.RegisterForNavigation<FishQuantityInputPage, FishQuantityInputViewModel>("FishQuantityInputPage");
            containerRegistry.RegisterForNavigation<FishQuantityDetailPage, FishQuantityDetailViewModel>("FishQuantityDetailPage");
            containerRegistry.RegisterForNavigation<AddFishQuantityPage, AddFishQuantityViewModel>("AddFishQuantityPage");
            containerRegistry.RegisterForNavigation<LocationDergeeInputPage, LocationDergeeInputViewModel>("LocationDergeeInputPage");
            containerRegistry.RegisterForNavigation<SOSPage, SOSViewModel>(PageNames.SOSPage.ToString());
            containerRegistry.RegisterForNavigation<BluetoothPage, BluetoothViewModel>(PageNames.BluetoothPage.ToString());
            containerRegistry.RegisterForNavigation<MessagesPage, MessagesViewModel>(PageNames.MessagesPage.ToString());
            containerRegistry.RegisterForNavigation<MessageDetailPage, MessageDetailViewModel>(PageNames.MessageDetailPage.ToString());
            containerRegistry.RegisterForNavigation<MessagesOnlinePage, MessagesOnlineViewModel>(PageNames.MessagesOnlinePage.ToString());
            containerRegistry.RegisterForNavigation<MessageOnlineDetailPage, MessageOnlineDetailViewModel>(PageNames.MessageOnlineDetailPage.ToString());
            containerRegistry.RegisterForNavigation<NotificationMessagePage, NotificationMessageViewModel>(PageNames.NotificationMessagePage.ToString());
            containerRegistry.RegisterForNavigation<OfflineMap, OfflineMapViewModel>("OffMap");

            containerRegistry.RegisterForNavigation<BoundaryPage, BoundaryViewModel>("BoundaryPage");
            containerRegistry.RegisterForNavigation<DistancePage, DistancePageViewModel>("DistancePage");
            containerRegistry.RegisterForNavigation<VehicleDetailPage, VehicleDetailViewModel>("VehicleDetailPage");
            containerRegistry.RegisterForNavigation<ListVehicleHelpPage, ListVehicleHelpViewModel>("ListVehicleHelpPage");
            containerRegistry.RegisterForNavigation<RouteListPage, RouteListViewModel>("RouteListPage");
            containerRegistry.RegisterForNavigation<ServicePackHistoryPage, ServicePackHistoryViewModel>("ServicePackHistoryPage");
            containerRegistry.RegisterForNavigation<PackageInfosPage, PackageInfosViewModel>("PackageInfosPage");

            containerRegistry.RegisterForNavigation<BA_MobileGPS.Core.Views.HelperPage, HeplerViewModel>("HelperPage");

            ViewModelLocationProvider.Register<OnlinePage, OnlinePageViewModel>();
            ViewModelLocationProvider.Register<ListVehiclePage, ListVehiclePageViewModel>();
            ViewModelLocationProvider.Register<RoutePage, RoutePageViewModel>();

            containerRegistry.Register<ContentView, OnlinePage>("OnlineTab");
            containerRegistry.Register<ContentView, ListVehiclePage>("ListVehicleTab");
            containerRegistry.Register<ContentView, RoutePage>("RouteTab");
            containerRegistry.RegisterForNavigation<DetailVehiclePopup,DetailVehiclePopupViewModel>("DetailVehiclePopup");

            //containerRegistry.Register<ResourceDictionary, LightColor>(Theme.Light.ToString());
            //containerRegistry.Register<ResourceDictionary, DarkColor>(Theme.Dark.ToString());
            //containerRegistry.Register<ResourceDictionary, VMS_MobileGPS.Styles.Custom>(Theme.Custom.ToString());

        }
    }
}