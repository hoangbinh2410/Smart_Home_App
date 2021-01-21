using BA_MobileGPS;
using BA_MobileGPS.Core;
using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.GoogleMap.Behaviors;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Core.ViewModels.Base;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;
using Prism.Commands;
using Prism.Navigation;
using Prism.Navigation.TabbedPages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;


namespace VMS_MobileGPS.ViewModels
{
    public class OnlinePageViewModel : TabbedPageChildVMBase
    {
        #region Contructor
        private readonly IDetailVehicleService detailVehicleService;
        private readonly IUserLandmarkGroupService userLandmarkGroupService;
        public ICommand NavigateToSettingsCommand { get; private set; }
        public ICommand ChangeMapTypeCommand { get; private set; }
        public ICommand PushToRouterPageCommand { get; private set; }
        public ICommand PushDirectvehicleOnlineCommand { get; private set; }
        public DelegateCommand<CameraIdledEventArgs> CameraIdledCommand { get; private set; }
        public DelegateCommand PushToDetailPageCommand { get; private set; }
        public DelegateCommand GoDistancePageCommand { get; private set; }
        public DelegateCommand PushToServicePackHistoryPageCommand { get; private set; }
        public bool IsCheckShowLandmark { get; set; } = false;

        public OnlinePageViewModel(INavigationService navigationService,
            IUserLandmarkGroupService userLandmarkGroupService,
            IDetailVehicleService detailVehicleService)
            : base(navigationService)
        {
            this.userLandmarkGroupService = userLandmarkGroupService;
            this.detailVehicleService = detailVehicleService;
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
            PushDirectvehicleOnlineCommand = new DelegateCommand(PushDirectvehicleOnline);
            PushToServicePackHistoryPageCommand = new DelegateCommand(GoServicePackHistoryPage);
        }

        public override void OnPageAppearingFirstTime()
        {
            base.OnPageAppearingFirstTime();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.TryGetValue(ParameterKey.Landmark, out List<UserLandmarkGroupRespone> listLandmark))
            {
                ListLandmark = listLandmark;

                ShowLandmark();
            }
        }

        private void ShowLandmark()
        {
            if (ZoomLevel >= 10 && IsShowConfigLanmark)
            {
                if (!IsCheckShowLandmark)
                {
                    if (ListLandmark != null)
                    {
                        if (ListLandmark.Count > 0)
                        {
                            Pins.Clear();
                            Polygons.Clear();
                            GroundOverlays.Clear();
                            Polylines.Clear();
                            GetLandmark(ListLandmark);
                            IsCheckShowLandmark = true;
                        }
                    }
                }
            }
            else
            {
                Pins.Clear();
                Polygons.Clear();
                GroundOverlays.Clear();
                Polylines.Clear();
                IsCheckShowLandmark = false;
            }
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

        private double zoomLevelFist;

        public double ZoomLevelFist { get => zoomLevelFist; set => SetProperty(ref zoomLevelFist, value); }

        private double zoomLevelLast;

        public double ZoomLevelLast { get => zoomLevelLast; set => SetProperty(ref zoomLevelLast, value); }

        public string currentAddress = string.Empty;
        public string CurrentAddress { get => currentAddress; set => SetProperty(ref currentAddress, value); }

        public string engineState;
        public string EngineState { get => engineState; set => SetProperty(ref engineState, value); }

        public bool isShowConfigLanmark;
        public bool IsShowConfigLanmark { get => isShowConfigLanmark; set => SetProperty(ref isShowConfigLanmark, value); }

        private List<UserLandmarkGroupRespone> listLandmark;
        public List<UserLandmarkGroupRespone> ListLandmark { get => listLandmark; set => SetProperty(ref listLandmark, value); }

        public ObservableCollection<GroundOverlay> GroundOverlays { get; set; } = new ObservableCollection<GroundOverlay>();

        public ObservableCollection<Pin> Pins { get; set; } = new ObservableCollection<Pin>();

        public ObservableCollection<Polygon> Polygons { get; set; } = new ObservableCollection<Polygon>();

        public ObservableCollection<Polyline> Polylines { get; set; } = new ObservableCollection<Polyline>();

        private bool isShowCircle;
        public bool IsShowCircle
        {
            get { return isShowCircle; }
            set
            {
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

        private void PushDirectvehicleOnline()
        {
            TryExecute(async () =>
            {
                // Namth: thêm đoạn check quyền location thì mới cho phép tiếp tục hoạt động.
                if (await PermissionHelper.CheckLocationPermissions())
                {
                    string map = string.Empty;
                    var mylocation = await LocationHelper.GetGpsLocation();
                    if (mylocation != null)
                    {
                        map = string.Format("https://www.google.com/maps/dir/{0};{1}/{2};{3}", mylocation.Latitude.ToString(), mylocation.Longitude.ToString(), carActive.Lat.ToString(), carActive.Lng.ToString());
                    }
                    else
                    {
                        map = string.Format("https://www.google.com/maps/dir/{0};{1}", carActive.Lat.ToString(), carActive.Lng.ToString());
                    }
                    await Launcher.OpenAsync(new Uri(ReplaceMapURL(map)));
                }
            });
        }

        public string ReplaceMapURL(string map)
        {
            return map.Replace(",", ".").Replace(";", ",");
        }

        public void GetLandmark(List<UserLandmarkGroupRespone> listmark)
        {
            SafeExecute(async () =>
            {
                var keygroup = string.Empty;
                var keycategory = string.Empty;
                foreach (var item in listmark)
                {
                    if (item.IsSystem)
                    {
                        keycategory += item.PK_LandmarksGroupID + ",";
                    }
                    else
                    {
                        keygroup += item.PK_LandmarksGroupID + ",";
                    }
                }
                keygroup = keygroup != string.Empty ? keygroup.Substring(0, keygroup.Length - 1) : string.Empty;
                keycategory = keycategory != string.Empty ? keycategory.Substring(0, keycategory.Length - 1) : string.Empty;

                if (keygroup != string.Empty)
                {
                    // Lấy thông tin các điểm theo nhóm điểm công ty
                    var list = await userLandmarkGroupService.GetDataLandmarkByGroupId(keygroup);

                    if (list != null && list.Count > 0)
                    {
                        GetLandmarkName(list);

                        GetLandmarkDisplayBound(list, listmark, false);

                        GetLandmarkDisplayName(list, listmark, false);
                    }
                }

                if (keycategory != string.Empty)
                {
                    // Lấy thông tin các điểm theo nhóm điểm hệ thống
                    var list = await userLandmarkGroupService.GetDataLandmarkByCategory(keycategory);

                    if (list != null && list.Count > 0)
                    {
                        GetLandmarkName(list);

                        GetLandmarkDisplayBound(list, listmark, true);

                        GetLandmarkDisplayName(list, listmark, true);
                    }
                }
            });
        }

        /// <summary>Danh sách các điểm</summary>
        /// <param name="list">The list.</param>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  2/18/2020   created
        /// </Modified>
        private void GetLandmarkName(List<UserLandmarkRespone> list)
        {
            TryExecute(() =>
            {
                foreach (var item in list)
                {
                    AddGroundOverlay(item);
                }
            });
        }

        /// <summary>Danh sách các điểm không được tích vùng bao</summary>
        /// <param name="list">The list.</param>
        /// <param name="listmark">The listmark.</param>
        /// <param name="IsSystem">if set to <c>true</c> [is system].</param>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  2/18/2020   created
        /// </Modified>
        private void GetLandmarkDisplayBound(List<UserLandmarkRespone> list, List<UserLandmarkGroupRespone> listmark, bool IsSystem)
        {
            TryExecute(() =>
            {
                var listBoundary = list.Where(x => x.Polygon != string.Empty).ToList();

                if (listBoundary != null && listBoundary.Count > 0)
                {
                    // Danh sách các điểm không được tích vùng bao

                    var respones = listmark.Where(l => l.IsSystem == IsSystem && !l.IsDisplayBound).ToList();

                    foreach (var item in respones)
                    {
                        listBoundary.RemoveAll(x => x.FK_LandmarksGroupID == item.PK_LandmarksGroupID);
                    }

                    foreach (var item in listBoundary)
                    {
                        AddBoundary(item);
                    }
                }
            });
        }

        private void AddBoundary(UserLandmarkRespone boundary)
        {
            TryExecute(() =>
            {
                var result = boundary.Polygon.Split(',');
                var color = Color.Blue;
                if (boundary.PK_LandmarkID == 376650)
                {
                    color = Color.Red;
                }
                else if (boundary.PK_LandmarkID == 376651)
                {
                    color = Color.Blue;
                }
                else if (boundary.PK_LandmarkID == 376652)
                {
                    color = Color.Green;
                }
                if (boundary.IsClosed)
                {
                    var polygon = new Polygon
                    {
                        IsClickable = true,
                        StrokeWidth = 1f,
                        StrokeColor = color.MultiplyAlpha(.5),
                        FillColor = color.MultiplyAlpha(.3),
                        Tag = "POLYGON"
                    };

                    for (int i = 0; i < result.Length; i += 2)
                    {
                        polygon.Positions.Add(new Position(FormatHelper.ConvertToDouble(result[i + 1], 6), FormatHelper.ConvertToDouble(result[i], 6)));
                    }

                    Polygons.Add(polygon);
                }
                else
                {
                    var polyline = new Polyline
                    {
                        IsClickable = false,
                        StrokeColor = color.MultiplyAlpha(.5),
                        StrokeWidth = 2f,
                        Tag = "POLYLINE"
                    };

                    for (int i = 0; i < result.Length; i += 2)
                    {
                        polyline.Positions.Add(new Position(FormatHelper.ConvertToDouble(result[i + 1], 6), FormatHelper.ConvertToDouble(result[i], 6)));
                    }

                    Polylines.Add(polyline);
                }
            });
        }

        private void AddGroundOverlay(UserLandmarkRespone boundary)
        {
            TryExecute(() =>
            {
                var icon = boundary.IconApp != null && boundary.IconApp != string.Empty ? BitmapDescriptorFactory.FromResource(boundary.IconApp) : BitmapDescriptorFactory.FromResource("ic_point_freeway.png");

                Pins.Add(new Pin()
                {
                    Position = new Position(boundary.Latitude, boundary.Longitude),
                    Label = boundary.Name,
                    Anchor = new Point(0.5, 0.5),
                    Icon = icon,
                    Tag = boundary.Name,
                });
            });
        }

        /// <summary>Danh sách các điểm không được tích tên điểm</summary>
        /// <param name="list">The list.</param>
        /// <param name="listmark">The listmark.</param>
        /// <param name="IsSystem">if set to <c>true</c> [is system].</param>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  2/18/2020   created
        /// </Modified>
        private void GetLandmarkDisplayName(List<UserLandmarkRespone> list, List<UserLandmarkGroupRespone> listmark, bool IsSystem)
        {
            TryExecute(() =>
            {
                var respones = listmark.Where(l => l.IsSystem == IsSystem && !l.IsDisplayName).ToList();

                foreach (var item in respones)
                {
                    list.RemoveAll(x => x.FK_LandmarksGroupID == item.PK_LandmarksGroupID);
                }

                foreach (var item in list)
                {
                    AddGroundOverlay(item);
                }
            });
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

                ShowLandmark();
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

        public void GotoCameraPage()
        {
            SafeExecute(async () =>
            {
                var param = new Vehicle()
                {
                    VehiclePlate = CarActive.VehiclePlate,
                    VehicleId = CarActive.VehicleId,
                    Imei = CarActive.Imei,
                    PrivateCode = CarActive.PrivateCode
                };
                var parameters = new NavigationParameters
                {
                    { ParameterKey.Vehicle, param }
                };

                await NavigationService.NavigateAsync("NavigationPage/ImageManagingPage", parameters, true, true);
            });
        }

        public void GotoVideoPage()
        {
            SafeExecute(async () =>
            {
                var photoPermission = await PermissionHelper.CheckPhotoPermissions();
                var storagePermission = await PermissionHelper.CheckStoragePermissions();
                if (photoPermission && storagePermission)
                {
                    var param = new Vehicle()
                    {
                        VehiclePlate = CarActive.VehiclePlate,
                        VehicleId = CarActive.VehicleId,
                        Imei = CarActive.Imei,
                        PrivateCode = CarActive.PrivateCode
                    };
                    var parameters = new NavigationParameters
                      {
                          { ParameterKey.Vehicle, param }
                     };

                    await NavigationService.NavigateAsync("NavigationPage/CameraManagingPage", parameters, true, true);
                }
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

        #endregion Private Method

    }
}
