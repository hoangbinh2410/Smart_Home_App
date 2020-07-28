using BA_MobileGPS;
using BA_MobileGPS.Core;
using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Events;
using BA_MobileGPS.Core.Extensions;
using BA_MobileGPS.Core.GoogleMap.Behaviors;
using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Core.Models;
using BA_MobileGPS.Core.Resource;
using BA_MobileGPS.Core.ViewModels;
using BA_MobileGPS.Core.Views;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.RealmEntity;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;
using Microsoft.AppCenter;
using Prism.Commands;
using Prism.Common;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using Realms.Sync;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using VMS_MobileGPS.Views;
using Xamarin.Forms;

namespace VMS_MobileGPS.ViewModels
{
    public class OnlinePageViewModel : ViewModelBase
    {

        #region Contructor

        private readonly IUserService userService;
        private readonly IDetailVehicleService detailVehicleService;

        public ICommand NavigateToSettingsCommand { get; private set; }
        public ICommand ChangeMapTypeCommand { get; private set; }
        public ICommand PushToRouterPageCommand { get; private set; }
        public ICommand PushToFABPageCommand { get; private set; }
        public DelegateCommand<CameraIdledEventArgs> CameraIdledCommand { get; private set; }
        public DelegateCommand PushToDetailPageCommand { get; private set; }
        public DelegateCommand PushToServicePackHistoryPageCommand { get; private set; }
        public ICommand PushtoListVehicleOnlineCommand { get; private set; }
        public DelegateCommand GoDistancePageCommand { get; private set; }

        public OnlinePageViewModel(INavigationService navigationService,
            IUserService userService, IDetailVehicleService detailVehicleService)
            : base(navigationService)
        {
            this.userService = userService;
            this.detailVehicleService = detailVehicleService;

            if (Settings.CurrentCompany != null && Settings.CurrentCompany.FK_CompanyID > 0)
            {
                Title = Settings.CurrentCompany.CompanyName;
            }
            else
            {
                Title = MobileResource.Online_Label_TitlePage;
            }

            carActive = new VehicleOnline();
            selectedVehicleGroup = new List<int>();
            CarSearch = MobileResource.Online_Label_SeachVehicle2;

            if (MobileUserSettingHelper.MapType == 4 || MobileUserSettingHelper.MapType == 5)
            {
                mapType = MapType.Hybrid;
                ColorMapType = (Color)App.Current.Resources["GrayColor2"];
            }
            else
            {
                mapType = MapType.Street;
                ColorMapType = (Color)App.Current.Resources["PrimaryColor"];
            }

            zoomLevel = MobileUserSettingHelper.Mapzoom;

            NavigateToSettingsCommand = new DelegateCommand(NavigateToSettings);
            ChangeMapTypeCommand = new DelegateCommand(ChangeMapType);
            PushToRouterPageCommand = new DelegateCommand(PushtoRouterPage);
            PushToFABPageCommand = new DelegateCommand<object>(PushtoFABPage);
            PushToDetailPageCommand = new DelegateCommand(PushtoDetailPage);
            CameraIdledCommand = new DelegateCommand<CameraIdledEventArgs>(UpdateMapInfo);
            GoDistancePageCommand = new DelegateCommand(GoDistancePage);
            PushToServicePackHistoryPageCommand = new DelegateCommand(GoServicePackHistoryPage);
        }

        #endregion

        #region Property

        private string carSearch;
        public string CarSearch { get => carSearch; set => SetProperty(ref carSearch, value); }
        public ObservableCollection<Circle> Circles { get; set; } = new ObservableCollection<Circle>();
        public ObservableCollection<Polygon> Boundaries { get; set; } = new ObservableCollection<Polygon>();
        public ObservableCollection<Polyline> Borders { get; set; } = new ObservableCollection<Polyline>();

        public AnimateCameraRequest AnimateCameraRequest { get; } = new AnimateCameraRequest();

        private MapType mapType;
        public MapType MapType { get => mapType; set => SetProperty(ref mapType, value); }

        private MapSpan visibleRegion;
        public MapSpan VisibleRegion { get => visibleRegion; set => SetProperty(ref visibleRegion, value); }

        private Color colorMapType;
        public Color ColorMapType { get => colorMapType; set => SetProperty(ref colorMapType, value); }

        private List<int> selectedVehicleGroup;
        public List<int> SelectedVehicleGroup { get => selectedVehicleGroup; set => SetProperty(ref selectedVehicleGroup, value); }

        private VehicleOnline carActive;

        public VehicleOnline CarActive
        {
            get { return carActive; }
            set
            {
                carActive = value;
                RaisePropertyChanged();
            }
        }

        private ShipDetailRespone shipDetailRespone;

        public ShipDetailRespone ShipDetailRespone
        {
            get { return shipDetailRespone; }
            set
            {
                shipDetailRespone = value;

                RaisePropertyChanged();
            }
        }

        private double zoomLevel;

        public double ZoomLevel { get => zoomLevel; set => SetProperty(ref zoomLevel, value); }

        public string currentAddress = string.Empty;
        public string CurrentAddress { get => currentAddress; set => SetProperty(ref currentAddress, value); }

        private bool isShowCircle;
        public bool IsShowCircle
        {
            get { return isShowCircle; }
            set {
                if (value)
                {
                    ShowBorder();
                }
                else HideBorder();
                SetProperty(ref isShowCircle, value);
            }
        }

        #endregion Property

        #region Private Method

        private void PushtoRouterPage()
        {
            SafeExecute(() =>
            {
                if (CheckPermision((int)PermissionKeyNames.ViewModuleRoute))
                {
                    EventAggregator.GetEvent<TabItemSwitchEvent>().Publish(new Tuple<ItemTabPageEnums, object>(ItemTabPageEnums.RoutePage, carActive));
                }
                else
                {
                    PageDialog.DisplayAlertAsync(MobileResource.Common_Label_Notification, MobileResource.Common_Message_NotPermission, MobileResource.Common_Button_Close);
                }
                   
            });
        }

        public void ShowBorder()
        {
            Circles.Clear();

            Circles.Add(new Circle
            {
                StrokeWidth = 2,
                StrokeColor = (Color)App.Current.Resources["PrimaryColor"],
                FillColor = Color.Transparent,
                Radius = Distance.FromKilometers(10 * 1.852),
                Center = new Position(CarActive.Lat, CarActive.Lng)
            });

            Circles.Add(new Circle
            {
                StrokeWidth = 2,
                StrokeColor = (Color)App.Current.Resources["PrimaryColor"],
                FillColor = Color.Transparent,
                Radius = Distance.FromKilometers(20 * 1.852),
                Center = new Position(CarActive.Lat, CarActive.Lng)
            });

            Circles.Add(new Circle
            {
                StrokeWidth = 2,
                StrokeColor = (Color)App.Current.Resources["PrimaryColor"],
                FillColor = Color.Transparent,
                Radius = Distance.FromKilometers(30 * 1.852),
                Center = new Position(CarActive.Lat, CarActive.Lng)
            });

            RaisePropertyChanged(nameof(Circles));
        }

        public void HideBorder()
        {
            Circles.Clear();

            RaisePropertyChanged(nameof(Circles));
        }

        public void NavigateToSettings()
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("BaseNavigationPage/BoundaryPage", useModalNavigation: true);
            });
        }

        private void ChangeMapType()
        {
            SafeExecute(async () =>
            {
                if (MapType == MapType.Street)
                {
                    MapType = MapType.Hybrid;
                    ColorMapType = (Color)App.Current.Resources["GrayColor2"];
                }
                else
                {
                    ColorMapType = (Color)App.Current.Resources["PrimaryColor"];
                    MapType = MapType.Street;
                }
                byte maptype = 1;
                if (MapType == MapType.Hybrid)
                {
                    maptype = 4;
                }
                var result = await userService.SetAdminUserSettings(new AdminUserConfiguration()
                {
                    FK_UserID = UserInfo.UserId,
                    Latitude = (float)MobileUserSettingHelper.LatCurrent,
                    Longitude = (float)MobileUserSettingHelper.LngCurrent,
                    MapType = maptype,
                    MapZoom = (byte)MobileUserSettingHelper.Mapzoom
                });
                MobileUserSettingHelper.Set(MobileUserConfigurationNames.MBMapType, maptype);
            });
        }

        private void PushtoFABPage(object index)
        {
            SafeExecute(async () =>
            {
                switch ((int)index)
                {
                    case 1:
                        await NavigationService.NavigateAsync("ListVehiclePage", null, useModalNavigation: false);
                        break;

                    case 2:

                        StaticSettings.ClearStaticSettings();
                        GlobalResources.Current.TotalAlert = 0;

                        await NavigationService.NavigateAsync("/NavigationPage/LandingPage");

                        break;

                    case 3:
                        await NavigationService.NavigateAsync("MenuNavigationPage/HeplerPage", null, useModalNavigation: true);
                        break;
                }
            });
        }

        private void UpdateMapInfo(CameraIdledEventArgs args)
        {
            if (args != null && args.Position != null)
            {
                ZoomLevel = args.Position.Zoom;
            }
        }

        private void PushtoDetailPage()
        {
            SafeExecute(async () =>
            {
                var parameters = new NavigationParameters
                {
                    { ParameterKey.CarDetail, CarActive }
                };

                await NavigationService.NavigateAsync("BaseNavigationPage/VehicleDetailPage", parameters, true);
            });
        }

        public void GoDistancePage()
        {
            SafeExecute(async () =>
            {
                var parameters = new NavigationParameters
                {
                    { ParameterKey.VehicleOnline, CarActive }
                };

                await NavigationService.NavigateAsync("BaseNavigationPage/DistancePage", parameters, true);
            });
        }

        private void GoServicePackHistoryPage()
        {
            SafeExecute(async () =>
            {

                // gọi service để lấy dữ liệu trả về
                var input = new ShipDetailRequest()
                {
                    UserId = StaticSettings.User.UserId,
                    vehiclePlate = CarActive.VehiclePlate,
                };
                var response = await detailVehicleService.GetShipDetail(input);
                if (response != null)
                {
                    var model = new ShipDetailRespone()
                    {
                        Address = string.Join(", ", GeoHelper.LatitudeToDergeeMinSec(CarActive.Lat), GeoHelper.LongitudeToDergeeMinSec(CarActive.Lng)),
                        Latitude = CarActive.Lat,
                        Longtitude = CarActive.Lng,
                        Km = CarActive.TotalKm,
                        GPSTime = CarActive.GPSTime,
                        VelocityGPS = CarActive.Velocity,
                        IMEI = response.IMEI,
                        PortDeparture = response.PortDeparture,
                        ShipCaptainName = response.ShipCaptainName,
                        ShipCaptainPhoneNumber = response.ShipCaptainPhoneNumber,
                        ShipMembers = response.ShipMembers,
                        ShipOwnerName = response.ShipOwnerName,
                        ShipOwnerPhoneNumber = response.ShipOwnerPhoneNumber,
                        PrivateCode = response.PrivateCode
                    };

                    var parameters = new NavigationParameters
                    {
                        { ParameterKey.ShipDetail, model }
                    };

                    await NavigationService.NavigateAsync("NavigationPage/ServicePackHistoryPage", parameters, true);
                }
            });
        }

        //private void OpenDiscoreryBox()
        //{
        //    PopupNavigation.Instance.PushAsync(new OnlineCarInfo());
        //}
        #endregion Private Method
    }
}
