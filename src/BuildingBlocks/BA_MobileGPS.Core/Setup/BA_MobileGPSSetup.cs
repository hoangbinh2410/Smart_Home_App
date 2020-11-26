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
using DryIoc;
using Prism.Ioc;
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
            //containerRegistry.Register<INavigationService, PopupPageNavigationService>();

            // This updates INavigationService and registers PopupNavigation.Instance
            containerRegistry.RegisterPopupNavigationService();
            containerRegistry.RegisterSingleton<IMapper, MapperUtility>();
            containerRegistry.RegisterSingleton(typeof(IRealmBaseService<,>), typeof(RealmBaseService<,>));
            containerRegistry.RegisterSingleton<IRealmConnection, RealmConnection>();
            containerRegistry.RegisterSingleton<IBaseRepository, BaseRepository>();
            containerRegistry.RegisterSingleton<IRequestProvider, RequestProvider>();
            containerRegistry.RegisterSingleton<IPlacesAutocomplete, PlacesAutocomplete>();
            containerRegistry.RegisterSingleton<IPlacesGeocode, PlacesGeocode>();
            containerRegistry.RegisterSingleton<IVehicleOnlineHubService, VehicleOnlineHubService>();
            containerRegistry.RegisterSingleton<IAlertHubService, AlertHubService>();
            containerRegistry.RegisterSingleton<IIdentityHubService, IdentityHubService>();
            containerRegistry.RegisterSingleton<IAuthenticationService, AuthenticationService>();
            containerRegistry.RegisterSingleton<IHomeService, HomeService>();
            containerRegistry.RegisterSingleton<IResourceService, ResourceService>();
            containerRegistry.RegisterSingleton<IDBVersionService, DBVersionService>();
            containerRegistry.RegisterSingleton<ILanguageService, LanguageService>();
            containerRegistry.RegisterSingleton<ICategoryService, CategoryService>();
            containerRegistry.RegisterSingleton<IFeedbackService, FeedbackService>();
            containerRegistry.RegisterSingleton<IUserService, UserService>();
            containerRegistry.RegisterSingleton<IRegisterConsultService, RegisterConsultService>();
            containerRegistry.RegisterSingleton<IVehicleOnlineService, VehicleOnlineService>();
            containerRegistry.RegisterSingleton<IGeocodeService, GeocodeService>();
            containerRegistry.RegisterSingleton<IAlertService, AlertService>();
            containerRegistry.RegisterSingleton<IVehicleDebtMoneyService, VehicleDebtMoneyService>();
            containerRegistry.RegisterSingleton<IDetailVehicleService, DetailVehicleService>();
            containerRegistry.RegisterSingleton<IVehicleRouteService, VehicleRouteService>();
            containerRegistry.RegisterSingleton<IReportTemperatureService, ReportTemperatureService>();
            containerRegistry.RegisterSingleton<IMachineVehicleService, MachineVehicleService>();
            containerRegistry.RegisterSingleton<IFuelChartService, FuelChartService>();
            containerRegistry.RegisterSingleton<IActivityDetailsService, ActivityDetailsService>();
            containerRegistry.RegisterSingleton<IActivitySummariesService, ActivitySummariesService>();
            containerRegistry.RegisterSingleton<IFuelsSummariesService, FuelsSummariesService>();
            containerRegistry.RegisterSingleton<IFuelsSummariesTotalService, FuelsSummariesTotalService>();
            containerRegistry.RegisterSingleton<ISpeedOversService, SpeedOversService>();
            containerRegistry.RegisterSingleton<IStopParkingVehicleService, StopsParkingVehicleService>();
            containerRegistry.RegisterSingleton<IShowHideColumnService, ShowHideColumnService>();
            containerRegistry.RegisterSingleton<ICameraService, CameraService>();
            containerRegistry.RegisterSingleton<IGuideService, GuideService>();
            containerRegistry.RegisterSingleton<IHelperService, HelperService>();
            containerRegistry.RegisterSingleton<IAppDeviceService, AppDeviceService>();
            containerRegistry.RegisterSingleton<INotificationService, NotificationService>();
            containerRegistry.RegisterSingleton<ISendEngineControlService, SendEngineControlService>();
            containerRegistry.RegisterSingleton<IUserLandmarkGroupService, UserLandmarkGroupService>();
            containerRegistry.RegisterSingleton<IPingServerService, PingServerService>();
            containerRegistry.RegisterSingleton<IStreamCameraService, StreamCameraService>();
            containerRegistry.RegisterSingleton<IMobileSettingService, MobileSettingService>();
            containerRegistry.RegisterSingleton<IPopupServices, PopupServices>();
            containerRegistry.RegisterSingleton<IThemeServices, ThemeServices>();
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
            containerRegistry.RegisterForNavigation<SelectRangeDateTime, SelectRangeDateTimeViewModel>("SelectRangeDateTime");
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

            containerRegistry.RegisterForNavigation<Home, HomeViewModel>("Home");
            containerRegistry.RegisterForNavigation<ListVehiclePage, ListVehiclePageViewModel>("ListVehiclePage");
            containerRegistry.RegisterForNavigation<OnlinePage, OnlinePageViewModel>("OnlinePage");
            containerRegistry.RegisterForNavigation<OnlinePageNoCluster, OnlinePageViewModel>("OnlinePageNoCluster");
            containerRegistry.RegisterForNavigation<RoutePage, RoutePageViewModel>("RoutePage");
            containerRegistry.RegisterForNavigation<Account, AccountViewModel>("Account");

            containerRegistry.Register<ContentPage, ListVehiclePage>("ListVehiclePage");
            containerRegistry.Register<ContentPage, OnlinePage>("OnlinePage");
            containerRegistry.Register<ContentPage, OnlinePageNoCluster>("OnlinePageNoCluster");
            containerRegistry.Register<ContentPage, RoutePage>("RoutePage");

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

            containerRegistry.RegisterForNavigation<CameraRestreamOverview, CameraRestreamOverviewViewModel>("CameraRestreamOverview");
            containerRegistry.RegisterForNavigation<CameraRestream, CameraRestreamViewModel>("CameraRestream");
            containerRegistry.RegisterForNavigation<DeviceTab, DeviceTabViewModel>();
            containerRegistry.RegisterForNavigation<BACloudTab, BACloudTabViewModel>();
            containerRegistry.RegisterForNavigation<MyVideoTab, MyVideoTabViewModel>();
            containerRegistry.RegisterForNavigation<VehicleCameraLookup, VehicleCameraLookupViewModel>("VehicleCameraLookup");

        }
    }
}