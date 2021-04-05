using BA_MobileGPS;
using BA_MobileGPS.Core;
using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.GoogleMap.Behaviors;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Core.ViewModels.Base;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using Prism.Commands;
using Prism.Navigation;
using Prism.Navigation.TabbedPages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using VMS_MobileGPS.Views;
using Xamarin.Forms;

namespace VMS_MobileGPS.ViewModels
{
    public class OnlinePageViewModel : TabbedPageChildVMBase
    {
        #region Contructor

        private readonly IDetailVehicleService detailVehicleService;
        private readonly IVehicleOnlineService vehicleOnlineService;
        public ICommand NavigateToSettingsCommand { get; private set; }
        public ICommand ChangeMapTypeCommand { get; private set; }
        public ICommand PushToRouterPageCommand { get; private set; }
        public DelegateCommand<CameraIdledEventArgs> CameraIdledCommand { get; private set; }
        public DelegateCommand PushToDetailPageCommand { get; private set; }
        public DelegateCommand GoDistancePageCommand { get; private set; }
        public DelegateCommand PushToServicePackHistoryPageCommand { get; private set; }

        public OnlinePageViewModel(INavigationService navigationService, IVehicleOnlineService vehicleOnlineService,
            IDetailVehicleService detailVehicleService)
            : base(navigationService)
        {
            this.detailVehicleService = detailVehicleService;
            this.vehicleOnlineService = vehicleOnlineService;
            carActive = new VehicleOnline();
            selectedVehicleGroup = new List<int>();
            CarSearch = string.Empty;

            if (Settings.MapType == (int)MapType.Hybrid)
            {
                mapType = MapType.Hybrid;
                ColorMapType = (Color)App.Current.Resources["WhiteColor"];
                BackgroundMapType = (Color)App.Current.Resources["PrimaryColor"];
            }
            else
            {
                mapType = MapType.Street;
                ColorMapType = (Color)App.Current.Resources["PrimaryColor"];
                BackgroundMapType = (Color)App.Current.Resources["WhiteColor"];
            }

            zoomLevel = MobileUserSettingHelper.Mapzoom;
            NavigateToSettingsCommand = new DelegateCommand(NavigateToSettings);
            ChangeMapTypeCommand = new DelegateCommand(ChangeMapType);
            PushToRouterPageCommand = new DelegateCommand(PushtoRouterPage);
            PushToDetailPageCommand = new DelegateCommand(PushtoDetailPage);
            CameraIdledCommand = new DelegateCommand<CameraIdledEventArgs>(UpdateMapInfo);
            GoDistancePageCommand = new DelegateCommand(GoDistancePage);
            PushToServicePackHistoryPageCommand = new DelegateCommand(GoServicePackHistoryPage);
        }

        public override void OnPageAppearingFirstTime()
        {
            base.OnPageAppearingFirstTime();

            GetAllIslandVN();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
        }

        public override void OnIsActiveChanged(object sender, EventArgs e)
        {
            base.OnIsActiveChanged(sender, e);
            if (!IsActive)
            {
                //EventAggregator.GetEvent<ShowHideTabEvent>().Publish(true);
            }
        }

        #endregion Contructor

        #region Property

        public ObservableCollection<Circle> Circles { get; set; } = new ObservableCollection<Circle>();
        public ObservableCollection<Polygon> Boundaries { get; set; } = new ObservableCollection<Polygon>();
        public ObservableCollection<Polyline> Borders { get; set; } = new ObservableCollection<Polyline>();
        private string carSearch;
        public string CarSearch { get => carSearch; set => SetProperty(ref carSearch, value); }

        public AnimateCameraRequest AnimateCameraRequest { get; } = new AnimateCameraRequest();

        private MapType mapType;
        public MapType MapType { get => mapType; set => SetProperty(ref mapType, value); }

        private MapSpan visibleRegion;
        public MapSpan VisibleRegion { get => visibleRegion; set => SetProperty(ref visibleRegion, value); }

        private Color colorMapType;
        public Color ColorMapType { get => colorMapType; set => SetProperty(ref colorMapType, value); }

        private Color backgroundMapType;
        public Color BackgroundMapType { get => backgroundMapType; set => SetProperty(ref backgroundMapType, value); }

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

        private double zoomLevel;

        public double ZoomLevel { get => zoomLevel; set => SetProperty(ref zoomLevel, value); }

        private double zoomLevelFist;

        public double ZoomLevelFist { get => zoomLevelFist; set => SetProperty(ref zoomLevelFist, value); }

        private double zoomLevelLast;

        public double ZoomLevelLast { get => zoomLevelLast; set => SetProperty(ref zoomLevelLast, value); }

        public string currentAddress = string.Empty;
        public string CurrentAddress { get => currentAddress; set => SetProperty(ref currentAddress, value); }

        public ObservableCollection<Pin> Pins { get; set; } = new ObservableCollection<Pin>();

        #endregion Property

        #region Private Method

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

        public string ReplaceMapURL(string map)
        {
            return map.Replace(",", ".").Replace(";", ",");
        }

        private void PushtoRouterPage()
        {
            SafeExecute(async () =>
            {
                if (CheckPermision((int)PermissionKeyNames.ViewModuleRoute))
                {
                    var parameters = new NavigationParameters
                    {
                        { ParameterKey.VehicleOnline, carActive }
                    };
                    EventAggregator.GetEvent<BackButtonEvent>().Publish(true);
                    var a = await NavigationService.SelectTabAsync("RoutePageVMS", parameters);
                }
                else
                {
                    await PageDialog.DisplayAlertAsync(MobileResource.Common_Label_Notification, MobileResource.Common_Message_NotPermission, MobileResource.Common_Button_Close);
                }
            });
        }

        public void NavigateToSettings()
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("BaseNavigationPage/BoundaryPage", null, useModalNavigation: true, true);
            });
        }

        private void ChangeMapType()
        {
            SafeExecute(() =>
            {
                if (MapType == MapType.Street)
                {
                    MapType = MapType.Hybrid;
                    ColorMapType = (Color)App.Current.Resources["WhiteColor"];
                    BackgroundMapType = (Color)App.Current.Resources["PrimaryColor"];
                }
                else
                {
                    ColorMapType = (Color)App.Current.Resources["PrimaryColor"];
                    BackgroundMapType = (Color)App.Current.Resources["WhiteColor"];
                    MapType = MapType.Street;
                }
                Settings.MapType = (int)MapType;
            });
        }

        private void UpdateMapInfo(CameraIdledEventArgs args)
        {
            if (args != null && args.Position != null)
            {
                if (ZoomLevelFist == 0 && args.Position.Zoom != MobileUserSettingHelper.Mapzoom)
                {
                    ZoomLevelFist = args.Position.Zoom;
                }

                if (ZoomLevelFist > 0 && args.Position.Zoom != ZoomLevelFist)
                {
                    ZoomLevelLast = args.Position.Zoom;
                }

                if (ZoomLevelLast > 0)
                {
                    ZoomLevel = args.Position.Zoom;
                }
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

                await NavigationService.NavigateAsync("BaseNavigationPage/VehicleDetailPage", parameters, true, true);
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

                await NavigationService.NavigateAsync("BaseNavigationPage/DistancePage", parameters, true, true);
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

                    await NavigationService.NavigateAsync("NavigationPage/ServicePackHistoryPage", parameters, true, true);
                }
            });
        }

        private void GetAllIslandVN()
        {
            //RunOnBackground(async () =>
            //{
            //    return await vehicleOnlineService.GetListParacelIslands();
            //},
            //      (respones) =>
            //      {
            //          if (respones != null && respones.Count > 0)
            //          {
            //              foreach (var item in respones)
            //              {
            //                  Pins.Add(new Pin()
            //                  {
            //                      Position = new Position(item.Latitude, item.Longitude),
            //                      Label = item.Name,
            //                      Icon = BitmapDescriptorFactory.FromView(new VMSPinInfowindowView(item.Name)),
            //                      Tag = item.Name + "Island"
            //                  });
            //              }
            //          }
            //      });
        }

        #endregion Private Method
    }
}