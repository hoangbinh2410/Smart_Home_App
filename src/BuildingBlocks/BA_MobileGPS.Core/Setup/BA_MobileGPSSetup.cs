using BA_MobileGPS.Entities.Infrastructure.Repository;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities.Constant;

using Prism.Ioc;
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

            containerRegistry.RegisterSingleton(typeof(IRealmBaseService<,>), typeof(RealmBaseService<,>));

            containerRegistry.Register<IRealmConnection, RealmConnection>();
            containerRegistry.Register<IBaseRepository, BaseRepository>();

            containerRegistry.RegisterSingleton<IRequestProvider, RequestProvider>();
            containerRegistry.Register<IPlacesAutocomplete, PlacesAutocomplete>();
            containerRegistry.Register<IPlacesGeocode, PlacesGeocode>();
            containerRegistry.Register<ISignalRServices, SignalRService>();
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
            containerRegistry.Register<IHelperAdvanceService, HelperAdvanceService>();
            containerRegistry.Register<IAppDeviceService, AppDeviceService>();
            containerRegistry.Register<INotificationService, NotificationService>();
            containerRegistry.Register<ISendEngineControlService, SendEngineControlService>();
            containerRegistry.Register<IUserLandmarkGroupService, UserLandmarkGroupService>();
            containerRegistry.Register<IPingServerService, PingServerService>();
        }

        public static void RegisterPages(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            //containerRegistry.RegisterForNavigation<NetworkPage>("NetworkPage");
            //containerRegistry.RegisterForNavigation<RootPage, RootPageViewModel>("RootPage");
            //containerRegistry.RegisterForNavigation<BaseNavigationPage, BaseNavigationPageViewModel>("BaseNavigationPage");
            //containerRegistry.RegisterForNavigation<MenuNavigationPage, MenuNavigationPageViewModel>("MenuNavigationPage");
            //containerRegistry.RegisterForNavigation<CustomNavigationPage, BaseNavigationPageViewModel>("CustomNavigationPage");
            //containerRegistry.RegisterForNavigation<MasterDetailNavigationPage, MasterDetailNavigationPageViewModel>("MasterDetailNavigationPage");
            //containerRegistry.RegisterForNavigation<SplashScreenPage, SplashScreenPageViewModel>("SplashScreenPage");
            //containerRegistry.RegisterForNavigation<MenuPage, MenuPageViewModel>("MenuPage");
            //containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>("LoginPage");
            //containerRegistry.RegisterForNavigation<HomePage, HomePageViewModel>("HomePage");
            //containerRegistry.RegisterForNavigation<RegisterConsultPage, RegisterConsultPageViewModel>("RegisterConsultPage");
            //containerRegistry.RegisterForNavigation<ChangePasswordPage, ChangePasswordPageViewModel>("ChangePasswordPage");
            //containerRegistry.RegisterForNavigation<UserInfoPage, UserInfoPageViewModel>("UserInfoPage");
            //containerRegistry.RegisterForNavigation<ForgotPasswordPage, ForgotPasswordPageViewModel>("ForgotPasswordPage");
            //containerRegistry.RegisterForNavigation<VerifyCodeSMSPage, VerifyCodeSMSPageViewModel>("VerifyCodeSMSPage");
            //containerRegistry.RegisterForNavigation<ChangePasswordForgotPage, ChangePasswordForgotPageViewModel>("ChangePasswordForgotPage");
            //containerRegistry.RegisterForNavigation<FeedbackPage, FeedbackPageViewModel>("FeedbackPage");
            //containerRegistry.RegisterForNavigation<LanguagePage, LanguagePageViewModel>("LanguagePage");
            //containerRegistry.RegisterForNavigation<PhoneCountryCodePage, PhoneCountryCodePageViewModel>("PhoneCountryCodePage");
            //containerRegistry.RegisterForNavigation<ComboboxPage, ComboboxPageViewModel>("ComboboxPage");
            //containerRegistry.RegisterForNavigation<OnlinePage, OnlinePageViewModel>("OnlinePage");
            //containerRegistry.RegisterForNavigation<OnlinePageNoCluster, OnlinePageViewModel>("OnlinePageNoCluster");
            //containerRegistry.RegisterForNavigation<OnlineOneCar, OnlineOneCarViewModel>("OnlineOneCar");
            //containerRegistry.RegisterForNavigation<ImageEditorPage, ImageEditorViewModel>("ImageEditorPage");
            //containerRegistry.RegisterForNavigation<PopupMessagePage, PopupMessageViewModel>("PopupMessagePage");
            //containerRegistry.RegisterForNavigation<PopupHtmlPage, PopupHtmlPageViewModel>("PopupHtmlPage");
            //containerRegistry.RegisterForNavigation<InsertLocalDBPage, InsertLocalDBViewModel>("InsertLocalDBPage");
            //containerRegistry.RegisterForNavigation<ChangeLanguage, ChangeLanguageViewModel>("ChangeLanguage");
            //containerRegistry.RegisterForNavigation<FavoritesConfigurationsPage, FavoritesConfigurationsPageViewModel>("FavoritesConfigurationsPage");
            //containerRegistry.RegisterForNavigation<VehicleDetailPage, VehicleDetailViewModel>("DetailVehiclePage");
            //containerRegistry.RegisterForNavigation<VehicleDebtMoneyPage, VehicleDebtMoneyPageViewModel>("VehicleDebtMoneyPage");
            //containerRegistry.RegisterForNavigation<ListVehiclePage, ListVehiclePageViewModel>("ListVehiclePage");
            //containerRegistry.RegisterForNavigation<RoutePage, RouteViewModel>("RoutePage");
            //containerRegistry.RegisterForNavigation<RouteListPage, RouteListViewModel>("RouteListPage");
            //containerRegistry.RegisterForNavigation<AlertOnlinePage, AlertOnlinePageViewModel>("AlertOnlinePage");
            //containerRegistry.RegisterForNavigation<AlertHandlingPage, AlertHandlingPageViewModel>("AlertHandlingPage");
            //containerRegistry.RegisterForNavigation<ListVehiclePage, ListVehiclePageViewModel>("ListVehiclePage");
            //containerRegistry.RegisterForNavigation<ListVehicleHelpPage, ListVehicleHelpViewModel>("ListVehicleHelpPage");
            //containerRegistry.RegisterForNavigation<ReportTableTemperature, ReportTableTemperatureViewModel>("ReportTableTemperature");
            //containerRegistry.RegisterForNavigation<SelectDatePicker, SelectDatePickerViewModel>("SelectDatePicker");
            //containerRegistry.RegisterForNavigation<SelectTimePicker, SelectTimePickerViewModel>("SelectTimePicker");
            //containerRegistry.RegisterForNavigation<SelectDateTimeCalendar, SelectDateTimeCalendarViewModel>("SelectDateTimeCalendar");
            //containerRegistry.RegisterForNavigation<SelectDateCalendar, SelectDateCalendarViewModel>("SelectDateCalendar");
            //containerRegistry.RegisterForNavigation<SelectDateTimeCalendarPopup, SelectDateTimeCalendarPopupViewModel>("SelectDateTimeCalendarPopup");
            //containerRegistry.RegisterForNavigation<ReportDetailTemperaturePage, ReportDetailTemperaturePageViewModel>("ReportDetailTemperaturePage");
            //containerRegistry.RegisterForNavigation<MachineVehicleReport, MachineVehicleReportViewModel>("MachineVehicleReport");
            //containerRegistry.RegisterForNavigation<DetailMachineVehicleReport, DetailMachineVehicleReportViewModel>("DetailMachineVehicleReport");
            //containerRegistry.RegisterForNavigation<CompanyLookUp, CompanyLookUpViewModel>("CompanyLookUp");
            //containerRegistry.RegisterForNavigation<VehicleGroupLookUp, VehicleGroupLookUpViewModel>("VehicleGroupLookUp");
            //containerRegistry.RegisterForNavigation<VehicleLookUp, VehicleLookUpViewModel>("VehicleLookUp");
            //containerRegistry.RegisterForNavigation<ListCameraVehicle, ListCameraVehicleViewModel>("ListCameraVehicle");
            //containerRegistry.RegisterForNavigation<CameraDetail, CameraDetailViewModel>("CameraDetail");
            //containerRegistry.RegisterForNavigation<TutorialPage, TutorialPageViewModel>("TutorialPage");
            //containerRegistry.RegisterForNavigation<HelperPage, HeplerViewModel>("HeplerPage");
            //containerRegistry.RegisterForNavigation<ChartFuelReportPage, ChartFuelReportViewModel>("ChartFuelReportPage");
            //containerRegistry.RegisterForNavigation<PourFuelReportPage, PourFuelViewModel>("PourFuelReportPage");
            //containerRegistry.RegisterForNavigation<PourFuelDetailReportPage, PourFuelDetailViewModel>("PourFuelDetailReport");
            //containerRegistry.RegisterForNavigation<UpdateVersion, UpdateVersionViewModel>("UpdateVersion");
            //containerRegistry.RegisterForNavigation<SettingsPage, SettingsViewModel>("SettingsPage");
            //containerRegistry.RegisterForNavigation<MyLocationSettingPage, MyLocationSettingPageViewModel>("MyLocationSettingPage");
            //containerRegistry.RegisterForNavigation<VerifyCodeOtpPage, VerifyCodeOtpPageViewModel>("VerifyCodeOtpPage");
            //containerRegistry.RegisterForNavigation<SendVerifyOtpPage, SendVerifyOtpPageViewModel>("SendVerifyOtpPage");
            //containerRegistry.RegisterForNavigation<SpeedOversReportPage, SpeedOversViewModel>("SpeedOversReportPage");
            //containerRegistry.RegisterForNavigation<SpeedOversDetailReportPage, SpeedOversDetailViewModel>("SpeedOversDetailReportPage");
            //containerRegistry.RegisterForNavigation<StopParkingVehicleReportPage, StopParkingVehicleViewModel>("StopParkingVehicleReportPage");
            //containerRegistry.RegisterForNavigation<StopParkingVehicleDetailReportPage, StopParkingVehicleDetailViewModel>("StopParkingVehicleDetailReportPage");
            //containerRegistry.RegisterForNavigation<ActivityDetailsReportPage, ActivityDetailsViewModel>("ActivityDetailsReportPage");
            //containerRegistry.RegisterForNavigation<ActivityDetailsDetailReportPage, ActivityDetailsDetailViewModel>("ActivityDetailsDetailReportPage");
            //containerRegistry.RegisterForNavigation<ActivitySummariesReportPage, ActivitySummariesViewModel>("ActivitySummariesReportPage");
            //containerRegistry.RegisterForNavigation<ActivitySummariesDetailReportPage, ActivitySummariesDetailViewModel>("ActivitySummariesDetailReportPage");
            //containerRegistry.RegisterForNavigation<FuelsSummariesReportPage, FuelsSummariesViewModel>("FuelsSummariesReportPage");
            //containerRegistry.RegisterForNavigation<FuelsSummariesDetailReportPage, FuelsSummariesDetailViewModel>("FuelsSummariesDetailReportPage");
            //containerRegistry.RegisterForNavigation<FuelsSummariesTotalReportPage, FuelsSummariesTotalViewModel>("FuelsSummariesTotalReportPage");
            //containerRegistry.RegisterForNavigation<FuelsSummariesTotalDetailReportPage, FuelsSummariesTotalDetailViewModel>("FuelsSummariesTotalDetailReportPage");
            //containerRegistry.RegisterForNavigation<NotificationPage, NotificationPageViewModel>("NotificationPage");
            //containerRegistry.RegisterForNavigation<NotificationPopup, NotificationPopupViewModel>("NotificationPopup");
            //containerRegistry.RegisterForNavigation<NotificationPopupWhenLogin, NotificationPopupWhenLoginViewModel>("NotificationPopupWhenLogin");
            //containerRegistry.RegisterForNavigation<NotificationPopupAfterLogin, NotificationPopupAfterLoginViewModel>("NotificationPopupAfterLogin");
            //containerRegistry.RegisterForNavigation<NotificationDetailPage, NotificationDetailPageViewModel>("NotificationDetailPage");
            //containerRegistry.RegisterForNavigation<SendEngineControlPage, SendEngineControlViewModel>("SendEngineControlPage");
            //containerRegistry.RegisterForNavigation<BoundaryPage, BoundaryViewModel>("BoundaryPage");
        }
    }
}