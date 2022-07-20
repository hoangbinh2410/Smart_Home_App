using BA_MobileGPS.Core.Interfaces;
using BA_MobileGPS.Core.Themes;
using BA_MobileGPS.Core.ViewModels;
using BA_MobileGPS.Core.ViewModels.Home;
using BA_MobileGPS.Core.Views;

using BA_MobileGPS.Entities.Infrastructure.Repository;
using BA_MobileGPS.Service;
using BA_MobileGPS.Service.IService;
using BA_MobileGPS.Service.IService.Support;
using BA_MobileGPS.Service.Report.Station;
using BA_MobileGPS.Service.Report.TransportBusiness;
using BA_MobileGPS.Service.Service;
using BA_MobileGPS.Service.Service.Expense;
using BA_MobileGPS.Service.Service.Report.Station;
using BA_MobileGPS.Service.Service.Report.TransportBusiness;
using BA_MobileGPS.Service.Service.Support;
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
            containerRegistry.RegisterSingleton<IUserBahaviorHubService, UserBahaviorHubService>();
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
            containerRegistry.RegisterSingleton<IDriverInforService, DriverInforService>();
            containerRegistry.RegisterSingleton<IPapersInforService, PapersInforService>();
            containerRegistry.RegisterSingleton<IIssueService, IssueService>();
            containerRegistry.RegisterSingleton<IReportQCVN31SpeedService, ReportQCVN31SpeedService>();
            containerRegistry.RegisterSingleton<ISupportCategoryService, SupportCategoryService>();
            containerRegistry.RegisterSingleton<IKPIDriverService, KPIDriverService>();
            containerRegistry.RegisterSingleton<IStationLocationService, StationLocationService>();
            containerRegistry.RegisterSingleton<IExpenseService, ExpenseService>();
            containerRegistry.RegisterSingleton<ITransportBusinessService, TransportBusinessService>();

            //SmartHome
            containerRegistry.RegisterSingleton<IControlSmartHomeService, ControlSmartHomeService>();
            containerRegistry.RegisterSingleton<IGetStatusService, GetStatusService>();
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
            containerRegistry.RegisterForNavigation<SelectMonthCalendar, SelectMonthCalendarViewModel>("SelectMonthCalendar");
            containerRegistry.RegisterForNavigation<SelectDateTimeCalendarPopup, SelectDateTimeCalendarPopupViewModel>("SelectDateTimeCalendarPopup");
            containerRegistry.RegisterForNavigation<SelectRangeDateTime, SelectRangeDateTimeViewModel>("SelectRangeDateTime");
            containerRegistry.RegisterForNavigation<ComboboxPage, ComboboxPageViewModel>("ComboboxPage");
            containerRegistry.RegisterForNavigation<LanguagePage, LanguagePageViewModel>("LanguagePage");
            containerRegistry.RegisterForNavigation<ChangeLanguage, ChangeLanguageViewModel>("ChangeLanguage");
            containerRegistry.RegisterForNavigation<InsertLocalDBPage, InsertLocalDBPageViewModel>("InsertLocalDBPage");            
            containerRegistry.RegisterForNavigation<CompanyLookUp, CompanyLookUpViewModel>("CompanyLookUp");
            containerRegistry.RegisterForNavigation<VehicleGroupLookUp, VehicleGroupLookUpViewModel>("VehicleGroupLookUp");
            containerRegistry.RegisterForNavigation<VehicleLookUp, VehicleLookUpViewModel>("VehicleLookUp");
            containerRegistry.RegisterForNavigation<UserInfoPage, UserInfoPageViewModel>("UserInfoPage");         
            containerRegistry.RegisterForNavigation<ComboboxPage, ComboboxPageViewModel>("ComboboxPage");
            containerRegistry.RegisterForNavigation<ImageEditorPage, ImageEditorViewModel>("ImageEditorPage");        
            containerRegistry.RegisterForNavigation<LoginPreviewFeaturesPage, LoginPreviewFeaturesPageViewModel>();
            containerRegistry.RegisterForNavigation<RouteListPage, RouteListViewModel>("RouteListPage");               
            containerRegistry.RegisterForNavigation<RoutePage, RoutePageViewModel>("RoutePage");
            containerRegistry.RegisterForNavigation<RouteReportPage, RoutePageViewModel>("RouteReportPage");
            containerRegistry.RegisterForNavigation<Account, AccountViewModel>("Account");          
            containerRegistry.Register<ContentPage, RoutePage>("RoutePage");
            containerRegistry.Register<ResourceDictionary, Dark>(Theme.Dark.ToString());
            containerRegistry.Register<ResourceDictionary, Light>(Theme.Light.ToString());
            containerRegistry.Register<ResourceDictionary, Custom>(Theme.Custom.ToString());            
            containerRegistry.RegisterForNavigation<RegisterConsultPage, RegisterConsultPageViewModel>("RegisterConsultPage");         
            containerRegistry.RegisterForNavigation<ReLoginPage, ReLoginPageViewModel>();          
            containerRegistry.RegisterForNavigation<VehicleCameraLookup, VehicleCameraLookupViewModel>("VehicleCameraLookup");          
            containerRegistry.RegisterForNavigation<VehicleCameraMultiSelect, VehicleCameraMultiSelectViewModel>("VehicleCameraMultiSelect");         
            containerRegistry.RegisterForNavigation<PopupHtmlPage, PopupHtmlPageViewModel>("PopupHtmlPage");
            containerRegistry.RegisterForNavigation<ForgotPasswordPage, ForgotPasswordPageViewModel>("ForgotPasswordPage");
            containerRegistry.RegisterForNavigation<VerifyCodeSMSPage, VerifyCodeSMSPageViewModel>();
            containerRegistry.RegisterForNavigation<ChangePasswordForgotPage, ChangePasswordForgotPageViewModel>();         
            containerRegistry.RegisterForNavigation<LoginFailedPopup, LoginFailedPopupViewModel>("LoginFailedPopup");                      
            containerRegistry.RegisterForNavigation<NumberPhoneLoginPage, NumberPhoneLoginPageViewModel>("NumberPhoneLoginPage");
            containerRegistry.RegisterForNavigation<VerifyOTPSmsPage, VerifyOTPSmsPageViewModel>("VerifyOTPSmsPage");

            //smart home
            containerRegistry.RegisterForNavigation<HomeViewPage, HomeViewModel>("HomeViewPage");
            containerRegistry.RegisterForNavigation<TurnHeaterView, TurnHeaterViewModel>("TurnHeaterView");
            containerRegistry.RegisterForNavigation<TurnLampView, TurnLampViewModel>("TurnLampView");


        }
    }
}