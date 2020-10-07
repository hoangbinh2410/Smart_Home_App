using BA_MobileGPS.Core.Interfaces;
using BA_MobileGPS.Core.Themes;
using BA_MobileGPS.Core.ViewModels;
using BA_MobileGPS.Core.Views;
using BA_MobileGPS.Entities.Infrastructure.Repository;
using BA_MobileGPS.Service;
using BA_MobileGPS.Service.IService;
using BA_MobileGPS.Service.Service;
using BA_MobileGPS.Service.Utilities;
using BA_MobileGPS.Utilities.Constant;

using Prism.Ioc;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Plugin.Popups;

using Xamarin.Forms;

namespace BA_MobileGPS.Core
{
    public static class BA_MobileGPSSetup
    {
        public static void Initialize()
        {
            //Đăng kí sử dụng Syncfusion
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(Config.SyncfusionKey);

            CultureHelper.SetCulture();
        }

        public static void RegisterServices(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<INavigationService, PopupPageNavigationService>();

            // This updates INavigationService and registers PopupNavigation.Instance
            containerRegistry.RegisterPopupNavigationService();
            containerRegistry.Register<IMapper, MapperUtility>();
            containerRegistry.RegisterSingleton(typeof(IRealmBaseService<,>), typeof(RealmBaseService<,>));

            containerRegistry.Register<IRealmConnection, RealmConnection>();
            containerRegistry.Register<IBaseRepository, BaseRepository>();

            containerRegistry.RegisterSingleton<IRequestProvider, RequestProvider>();
            containerRegistry.Register<IPlacesAutocomplete, PlacesAutocomplete>();
            containerRegistry.Register<IPlacesGeocode, PlacesGeocode>();
            containerRegistry.Register<IVehicleOnlineHubService, VehicleOnlineHubService>();
            containerRegistry.Register<IAlertHubService, AlertHubService>();
            containerRegistry.Register<IIdentityHubService, IdentityHubService>();
            containerRegistry.Register<IAuthenticationService, AuthenticationService>();
            containerRegistry.Register<IHomeService, HomeService>();
            containerRegistry.Register<IResourceService, ResourceService>();
            containerRegistry.Register<IDBVersionService, DBVersionService>();
            containerRegistry.Register<ILanguageService, LanguageService>();
            containerRegistry.Register<ICategoryService, CategoryService>();
            containerRegistry.Register<IFeedbackService, FeedbackService>();
            containerRegistry.Register<IUserService, UserService>();
            containerRegistry.Register<IRegisterConsultService, RegisterConsultService>();
            containerRegistry.Register<IVehicleOnlineService, VehicleOnlineService>();
            containerRegistry.Register<IGeocodeService, GeocodeService>();
            containerRegistry.Register<IAlertService, AlertService>();
            containerRegistry.Register<IVehicleDebtMoneyService, VehicleDebtMoneyService>();
            containerRegistry.Register<IDetailVehicleService, DetailVehicleService>();
            containerRegistry.Register<IVehicleRouteService, VehicleRouteService>();
            containerRegistry.Register<IReportTemperatureService, ReportTemperatureService>();
            containerRegistry.Register<IMachineVehicleService, MachineVehicleService>();
            containerRegistry.Register<IFuelChartService, FuelChartService>();
            containerRegistry.Register<IActivityDetailsService, ActivityDetailsService>();
            containerRegistry.Register<IActivitySummariesService, ActivitySummariesService>();
            containerRegistry.Register<IFuelsSummariesService, FuelsSummariesService>();
            containerRegistry.Register<IFuelsSummariesTotalService, FuelsSummariesTotalService>();
            containerRegistry.Register<ISpeedOversService, SpeedOversService>();
            containerRegistry.Register<IStopParkingVehicleService, StopsParkingVehicleService>();
            containerRegistry.Register<IShowHideColumnService, ShowHideColumnService>();
            containerRegistry.Register<ICameraService, CameraService>();
            containerRegistry.Register<IGuideService, GuideService>();
            containerRegistry.Register<IHelperService, HelperService>();
            containerRegistry.Register<IAppDeviceService, AppDeviceService>();
            containerRegistry.Register<INotificationService, NotificationService>();
            containerRegistry.Register<ISendEngineControlService, SendEngineControlService>();
            containerRegistry.Register<IUserLandmarkGroupService, UserLandmarkGroupService>();
            containerRegistry.Register<IPingServerService, PingServerService>();
            containerRegistry.Register<IStreamCameraService, StreamCameraService>();

            containerRegistry.Register<IPopupServices, PopupServices>();
            containerRegistry.Register<IThemeServices, ThemeServices>();
        }

        public static void RegisterPages(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<BaseNavigationPage, BaseNavigationPageViewModel>("BaseNavigationPage");
            containerRegistry.RegisterForNavigation<NetworkPage>("NetworkPage");
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<ChangePasswordPage, ChangePasswordPageViewModel>("ChangePasswordPage");
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<UpdateVersion, UpdateVersionViewModel>("UpdateVersion");

            containerRegistry.RegisterForNavigation<SelectDatePicker, SelectDatePickerViewModel>("SelectDatePicker");
            containerRegistry.RegisterForNavigation<SelectTimePicker, SelectTimePickerViewModel>("SelectTimePicker");
            containerRegistry.RegisterForNavigation<SelectDateTimeCalendar, SelectDateTimeCalendarViewModel>("SelectDateTimeCalendar");
            containerRegistry.RegisterForNavigation<SelectDateCalendar, SelectDateCalendarViewModel>("SelectDateCalendar");
            containerRegistry.RegisterForNavigation<SelectDateTimeCalendarPopup, SelectDateTimeCalendarPopupViewModel>("SelectDateTimeCalendarPopup");
            containerRegistry.RegisterForNavigation<ComboboxPage, ComboboxPageViewModel>("ComboboxPage");

            containerRegistry.RegisterForNavigation<LanguagePage, LanguagePageViewModel>("LanguagePage");
            containerRegistry.RegisterForNavigation<ChangeLanguage, ChangeLanguageViewModel>("ChangeLanguage");
            containerRegistry.RegisterForNavigation<InsertLocalDBPage, InsertLocalDBPageViewModel>("InsertLocalDBPage");
            containerRegistry.RegisterForNavigation<NotificationPopupWhenLogin, NotificationPopupWhenLoginViewModel>("NotificationPopupWhenLogin");
            containerRegistry.RegisterForNavigation<FavoritesConfigurationsPage, FavoritesConfigurationsPageViewModel>("FavoritesConfigurationsPage");

            containerRegistry.RegisterForNavigation<ActivityDetailsDetailReportPage, ActivityDetailsDetailViewModel>("ActivityDetailsDetailReportPage");
            containerRegistry.RegisterForNavigation<ActivityDetailsReportPage, ActivityDetailsViewModel>("ActivityDetailsReportPage");
            containerRegistry.RegisterForNavigation<ActivitySummariesDetailReportPage, ActivitySummariesDetailViewModel>("ActivitySummariesDetailReportPage");
            containerRegistry.RegisterForNavigation<ActivitySummariesReportPage, ActivitySummariesViewModel>("ActivitySummariesReportPage");
            containerRegistry.RegisterForNavigation<ChartFuelReportPage, ChartFuelReportViewModel>("ChartFuelReportPage");
            containerRegistry.RegisterForNavigation<FuelsSummariesDetailReportPage, FuelsSummariesDetailViewModel>("FuelsSummariesDetailReportPage");
            containerRegistry.RegisterForNavigation<FuelsSummariesReportPage, FuelsSummariesViewModel>("FuelsSummariesReportPage");
            containerRegistry.RegisterForNavigation<FuelsSummariesTotalDetailReportPage, FuelsSummariesTotalDetailViewModel>("FuelsSummariesTotalDetailReportPage");
            containerRegistry.RegisterForNavigation<FuelsSummariesTotalReportPage, FuelsSummariesTotalViewModel>("FuelsSummariesTotalReportPage");
            containerRegistry.RegisterForNavigation<MachineDetailVehicleReport, MachineDetailVehicleReportViewModel>("MachineDetailVehicleReport");
            containerRegistry.RegisterForNavigation<MachineVehicleReport, MachineVehicleReportViewModel>("MachineVehicleReport");
            containerRegistry.RegisterForNavigation<PourFuelDetailReportPage, PourFuelDetailViewModel>("PourFuelDetailReportPage");
            containerRegistry.RegisterForNavigation<PourFuelReportPage, PourFuelViewModel>("PourFuelReportPage");
            containerRegistry.RegisterForNavigation<SignalLossReportDetailPage, SignalLossDetailViewModel>("SignalLossReportDetailPage");
            containerRegistry.RegisterForNavigation<SignalLossReportPage, SignalLossViewModel>("SignalLossReportPage");
            containerRegistry.RegisterForNavigation<SpeedOversDetailReportPage, SpeedOversDetailViewModel>("SpeedOversDetailReportPage");
            containerRegistry.RegisterForNavigation<SpeedOversReportPage, SpeedOversViewModel>("SpeedOversReportPage");
            containerRegistry.RegisterForNavigation<StopParkingVehicleDetailReportPage, StopParkingVehicleDetailViewModel>("StopParkingVehicleDetailReportPage");
            containerRegistry.RegisterForNavigation<StopParkingVehicleReportPage, StopParkingVehicleViewModel>("StopParkingVehicleReportPage");
            containerRegistry.RegisterForNavigation<ReportDetailTemperaturePage, ReportDetailTemperaturePageViewModel>("ReportDetailTemperaturePage");
            containerRegistry.RegisterForNavigation<ReportTableTemperature, ReportTableTemperatureViewModel>("ReportTableTemperature");

            containerRegistry.RegisterForNavigation<CompanyLookUp, CompanyLookUpViewModel>("CompanyLookUp");
            containerRegistry.RegisterForNavigation<VehicleGroupLookUp, VehicleGroupLookUpViewModel>("VehicleGroupLookUp");
            containerRegistry.RegisterForNavigation<VehicleLookUp, VehicleLookUpViewModel>("VehicleLookUp");
            containerRegistry.RegisterForNavigation<UserInfoPage, UserInfoPageViewModel>("UserInfoPage");
            containerRegistry.RegisterForNavigation<SettingsPage, SettingsViewModel>("SettingsPage");
            containerRegistry.RegisterForNavigation<MyLocationSettingPage, MyLocationSettingPageViewModel>("MyLocationSettingPage");
            containerRegistry.RegisterForNavigation<AlertConfigSettingPage, AlertConfigSettingPageViewModel>("AlertConfigSettingPage");
            containerRegistry.RegisterForNavigation<AlertVehicleSettingPage, AlertVehicleSettingPageViewModel>("AlertVehicleSettingPage");
            containerRegistry.RegisterForNavigation<AlertTimeSettingPage, AlertTimeSettingPageViewModel>("AlertTimeSettingPage");
            containerRegistry.RegisterForNavigation<HelperPage, HeplerViewModel>("HelperPage");
            containerRegistry.RegisterForNavigation<TutorialPage, TutorialPageViewModel>("TutorialPage");
            containerRegistry.RegisterForNavigation<AlertOnlinePage, AlertOnlinePageViewModel>("AlertOnlinePage");
            containerRegistry.RegisterForNavigation<AlertHandlingPage, AlertHandlingPageViewModel>("AlertHandlingPage");

            containerRegistry.RegisterForNavigation<ComboboxPage, ComboboxPageViewModel>("ComboboxPage");
            containerRegistry.RegisterForNavigation<ImageEditorPage, ImageEditorViewModel>("ImageEditorPage");

            containerRegistry.RegisterForNavigation<NotificationPage, NotificationPageViewModel>("NotificationPage");
            containerRegistry.RegisterForNavigation<NotificationPopup, NotificationPopupViewModel>("NotificationPopup");
            containerRegistry.RegisterForNavigation<NotificationPopupWhenLogin, NotificationPopupWhenLoginViewModel>("NotificationPopupWhenLogin");
            containerRegistry.RegisterForNavigation<NotificationPopupAfterLogin, NotificationPopupAfterLoginViewModel>("NotificationPopupAfterLogin");
            containerRegistry.RegisterForNavigation<NotificationDetailPage, NotificationDetailPageViewModel>("NotificationDetailPage");

            containerRegistry.RegisterForNavigation<VehicleDebtMoneyPage, VehicleDebtMoneyPageViewModel>("VehicleDebtMoneyPage");
            containerRegistry.RegisterForNavigation<VehicleDetailPage, VehicleDetailViewModel>("VehicleDetailPage");
            containerRegistry.RegisterForNavigation<LoginPreviewFeaturesPage, LoginPreviewFeaturesPageViewModel>();

            containerRegistry.RegisterForNavigation<RouteListPage, RouteListViewModel>("RouteListPage");

            containerRegistry.RegisterForNavigation<ListCameraVehicle, ListCameraVehicleViewModel>("ListCameraVehicle");
            containerRegistry.RegisterForNavigation<CameraDetail, CameraDetailViewModel>("CameraDetail");

            containerRegistry.RegisterForNavigation<SendEngineControlPage, SendEngineControlViewModel>("SendEngineControlPage");

            containerRegistry.RegisterForNavigation<BoundaryPage, BoundaryViewModel>("BoundaryPage");

            containerRegistry.RegisterForNavigation<ListVehicleHelpPage, ListVehicleHelpViewModel>("ListVehicleHelpPage");
            containerRegistry.RegisterForNavigation<DetailVehiclePopup, DetailVehiclePopupViewModel>("DetailVehiclePopup");

            ViewModelLocationProvider.Register<Home, HomeViewModel>();
            ViewModelLocationProvider.Register<ListVehiclePage, ListVehiclePageViewModel>();
            ViewModelLocationProvider.Register<OnlinePage, OnlinePageViewModel>();
            ViewModelLocationProvider.Register<OnlinePageNoCluster, OnlinePageViewModel>();
            ViewModelLocationProvider.Register<RoutePage, RouteViewModel>();
            ViewModelLocationProvider.Register<Account, AccountViewModel>();

            //containerRegistry.Register<ContentView, Home>("HomeTab");
            containerRegistry.Register<ContentView, ListVehiclePage>("ListVehicleTab");
            containerRegistry.Register<ContentView, OnlinePage>("OnlineTab");
            containerRegistry.Register<ContentView, OnlinePageNoCluster>("OnlineTabNoCluster");
            containerRegistry.Register<ContentView, RoutePage>("RouteTab");
           // containerRegistry.Register<ContentView, Account>("AccountTab");

            containerRegistry.Register<ResourceDictionary, Dark>(Theme.Dark.ToString());
            containerRegistry.Register<ResourceDictionary, Light>(Theme.Light.ToString());
            containerRegistry.Register<ResourceDictionary, Custom>(Theme.Custom.ToString());
            containerRegistry.RegisterForNavigation<SettingThemePage, SettingThemePageViewModel>("SettingThemePage");

            containerRegistry.RegisterForNavigation<RegisterConsultPage, RegisterConsultPageViewModel>("RegisterConsultPage");        

            containerRegistry.RegisterForNavigation<CameraManagingPage, CameraManagingPageViewModel>("CameraManagingPage");

            containerRegistry.RegisterForNavigation<ImageManagingPage, ImageManagingPageViewModel>("ImageManagingPage");
            containerRegistry.RegisterForNavigation<ImageDetailPage, ImageDetailViewModel>("ImageDetailPage");

            containerRegistry.RegisterForNavigation<ReLoginPage, ReLoginPageViewModel>();
            containerRegistry.RegisterForNavigation<RequestMoreTimePopup, RequestMoreTimePopupViewModel>();
        }
    }
}
