﻿using AutoMapper;
using BA_MobileGPS.Core.DependencyServices;
using BA_MobileGPS.Core.Interfaces;
using BA_MobileGPS.Core.ViewModels;
using BA_MobileGPS.Core.Views;
using BA_MobileGPS.Entities.Infrastructure.Repository;
using BA_MobileGPS.Service;
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

            containerRegistry.RegisterSingleton(typeof(IRealmBaseService<,>), typeof(RealmBaseService<,>));

            containerRegistry.Register<IRealmConnection, RealmConnection>();
            containerRegistry.Register<IBaseRepository, BaseRepository>();

            containerRegistry.RegisterSingleton<IRequestProvider, RequestProvider>();
            containerRegistry.Register<IPlacesAutocomplete, PlacesAutocomplete>();
            containerRegistry.Register<IPlacesGeocode, PlacesGeocode>();
            containerRegistry.Register<IVehicleOnlineHubService, VehicleOnlineHubService>();
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
            containerRegistry.Register<IHelperAdvanceService, HelperAdvanceService>();
            containerRegistry.Register<IAppDeviceService, AppDeviceService>();
            containerRegistry.Register<INotificationService, NotificationService>();
            containerRegistry.Register<ISendEngineControlService, SendEngineControlService>();
            containerRegistry.Register<IUserLandmarkGroupService, UserLandmarkGroupService>();
            containerRegistry.Register<IPingServerService, PingServerService>();

            containerRegistry.Register<IPopupServices, PopupServices>();

            ViewModelLocationProvider.Register<Home, HomeViewModel>();
            ViewModelLocationProvider.Register<Account, AccountViewModel>();
            containerRegistry.Register<ContentView, Home>("HomeTab");
            containerRegistry.Register<ContentView, Account>("AccountTab");

        }

        public static void RegisterPages(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<BaseNavigationPage, BaseNavigationPageViewModel>("BaseNavigationPage");
            containerRegistry.RegisterForNavigation<NetworkPage>("NetworkPage");
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<ChangePasswordPage, ChangePasswordPageViewModel>("ChangePasswordPage");
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
           
            containerRegistry.RegisterForNavigation<SelectDatePicker, SelectDatePickerViewModel>("SelectDatePicker");
            containerRegistry.RegisterForNavigation<SelectTimePicker, SelectTimePickerViewModel>("SelectTimePicker");
            containerRegistry.RegisterForNavigation<SelectDateTimeCalendar, SelectDateTimeCalendarViewModel>("SelectDateTimeCalendar");
            containerRegistry.RegisterForNavigation<SelectDateCalendar, SelectDateCalendarViewModel>("SelectDateCalendar");
            containerRegistry.RegisterForNavigation<SelectDateTimeCalendarPopup, SelectDateTimeCalendarPopupViewModel>("SelectDateTimeCalendarPopup");

            containerRegistry.RegisterForNavigation<LanguagePage, LanguagePageViewModel>();
            containerRegistry.RegisterForNavigation<ChangeLanguage, ChangeLanguageViewModel>();
            containerRegistry.RegisterForNavigation<InsertLocalDBPage, InsertLocalDBPageViewModel>();
            containerRegistry.RegisterForNavigation<NotificationPopupWhenLogin, NotificationPopupWhenLoginViewModel>("NotificationPopupWhenLogin");
            containerRegistry.RegisterForNavigation<FavoritesConfigurationsPage, FavoritesConfigurationsPageViewModel>("FavoritesConfigurationsPage");

            containerRegistry.RegisterForNavigation<CompanyLookUp, CompanyLookUpViewModel>("CompanyLookUp");
            containerRegistry.RegisterForNavigation<VehicleGroupLookUp, VehicleGroupLookUpViewModel>("VehicleGroupLookUp");
            containerRegistry.RegisterForNavigation<VehicleLookUp, VehicleLookUpViewModel>("VehicleLookUp");
            containerRegistry.RegisterForNavigation<UserInfoPage, UserInfoPageViewModel>("UserInfoPage");
            containerRegistry.RegisterForNavigation<SettingsPage, SettingsViewModel>("SettingsPage");
            containerRegistry.RegisterForNavigation<MyLocationSettingPage, MyLocationSettingPageViewModel>("MyLocationSettingPage");
            containerRegistry.RegisterForNavigation<AlertConfigSettingPage, AlertConfigSettingPageViewModel>("AlertConfigSettingPage");
            containerRegistry.RegisterForNavigation<AlertVehicleSettingPage, AlertVehicleSettingPageViewModel>("AlertVehicleSettingPage");
            containerRegistry.RegisterForNavigation<AlertTimeSettingPage, AlertTimeSettingPageViewModel>("AlertTimeSettingPage");
            containerRegistry.RegisterForNavigation<HelperPage, HeplerViewModel>("HeplerPage");
            containerRegistry.RegisterForNavigation<TutorialPage, TutorialPageViewModel>("TutorialPage");

        }
    }
}