using BA_MobileGPS.Core;
using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.GoogleMap.Behaviors;
using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Core.Models;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Core.ViewModels.Base;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.RealmEntity;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;
using Prism.Commands;
using Prism.Navigation;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

using VMS_MobileGPS.Views;

using Xamarin.Forms;
using Xamarin.Forms.Extensions;

namespace VMS_MobileGPS.ViewModels
{
    public class RoutePageViewModel : TabbedPageChildVMBase
    {
        #region Contructor

        private readonly IGeocodeService geocodeService;
        private readonly IVehicleRouteService vehicleRouteService;
        private readonly IRealmBaseService<BoundaryRealm, LandmarkResponse> boundaryRepository;
        public ICommand TimeSelectedCommand { get; }
        public ICommand DateSelectedCommand { get; }
        public ICommand SearchVehicleCommand { get; }
        public ICommand GetVehicleRouteCommand { get; }
        public ICommand ViewListCommand { get; }
        public ICommand ChangeMapTypeCommand { get; }
        public ICommand NavigateToSettingsCommand { get; }
        public ICommand WatchVehicleCommand { get; }
        public ICommand PinClickedCommand { get; }
        public ICommand DragStartedCommand { get; }
        public ICommand DragCompletedCommand { get; }
        public ICommand PlayStopCommand { get; }
        public ICommand IncreaseSpeedCommand { get; }
        public ICommand DecreaseSpeedCommand { get; }
        public ICommand FastStartCommand { get; }
        public ICommand FastEndCommand { get; }
        public ICommand ChangeSpeedCommand { get; }

        public RoutePageViewModel(INavigationService navigationService, IVehicleRouteService vehicleRouteService,
            IGeocodeService geocodeService, IRealmBaseService<BoundaryRealm, LandmarkResponse> realmBaseService)
           : base(navigationService)
        {
            this.vehicleRouteService = vehicleRouteService;
            this.geocodeService = geocodeService;
            boundaryRepository = realmBaseService;
            if (MobileUserSettingHelper.MapType == 4 || MobileUserSettingHelper.MapType == 5)
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
            view = new StackLayout();
            TimeSelectedCommand = new Command<string>(TimeSelected);
            DateSelectedCommand = new Command<DateChangedEventArgs>(DateSelected);
            SearchVehicleCommand = new Command(SearchVehicle);
            GetVehicleRouteCommand = new Command(ValidateUserConfigGetHistoryRoute);
            ViewListCommand = new Command(ViewList);
            ChangeMapTypeCommand = new Command(ChangeMapType);
            NavigateToSettingsCommand = new Command(NavigateToSettings);
            WatchVehicleCommand = new Command(WatchVehicle);
            PinClickedCommand = new Command<PinClickedEventArgs>(PinClicked);
            DragStartedCommand = new Command(DragStarted);
            DragCompletedCommand = new Command(DragCompleted);
            PlayStopCommand = new Command(PlayStop);
            IncreaseSpeedCommand = new DelegateCommand(IncreaseSpeed);
            DecreaseSpeedCommand = new Command(DecreaseSpeed);
            FastStartCommand = new Command(FastStart);
            FastEndCommand = new Command(FastEnd);
            ChangeSpeedCommand = new DelegateCommand(ChangeSpeed);
        }

        #endregion Contructor

        #region Lifecycle

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters != null)
            {
                if (parameters.ContainsKey(ParameterKey.VehicleRoute) && parameters.GetValue<Vehicle>(ParameterKey.VehicleRoute) is Vehicle vehicle)
                {
                    Vehicle = vehicle;

                    ValidateUserConfigGetHistoryRoute();
                }
                else if (parameters.ContainsKey(ParameterKey.VehicleOnline) && parameters.GetValue<VehicleOnline>(ParameterKey.VehicleOnline) is VehicleOnline vehicleOnline)
                {
                    Vehicle = new Vehicle()
                    {
                        GroupIDs = vehicleOnline.GroupIDs,
                        PrivateCode = vehicleOnline.PrivateCode,
                        VehicleId = vehicleOnline.VehicleId,
                        VehiclePlate = vehicleOnline.VehiclePlate
                    };

                    ValidateUserConfigGetHistoryRoute();
                }
            }
        }

        public override void OnDestroy()
        {
            //base.OnDestroy();
            if (ctsRouting != null)
                ctsRouting.Cancel();
        }

        public override void OnIsActiveChanged(object sender, EventArgs e)
        {
            base.OnIsActiveChanged(sender, e);
            if (!IsActive)
            {
                StopRoute();
            }
            else
            {
                Device.StartTimer(TimeSpan.FromMilliseconds(1000), () =>
                {
                    GoogleMapAddBoundary();
                    GoogleMapAddName();
                    return false;
                });
            }
        }

        #endregion Lifecycle

        #region Property

        public ObservableCollection<Polygon> Boundaries { get; set; } = new ObservableCollection<Polygon>();
        private View view;
        private CancellationTokenSource ctsRouting = new CancellationTokenSource();

        private RouteHistoryResponse RouteHistory;

        public List<VehicleRoute> ListRoute { get; set; } = new List<VehicleRoute>();

        private Vehicle vehicle = new Vehicle();
        public Vehicle Vehicle { get => vehicle; set => SetProperty(ref vehicle, value); }

        private float dateKm;
        public float DateKm { get => dateKm; set => SetProperty(ref dateKm, value); }

        private VehicleRoute currentRoute;
        public VehicleRoute CurrentRoute { get => currentRoute; set => SetProperty(ref currentRoute, value); }

        public ObservableCollection<Pin> Pins { get; set; } = new ObservableCollection<Pin>();

        public ObservableCollection<Polyline> Polylines { get; set; } = new ObservableCollection<Polyline>();

        public MoveCameraRequest MoveCameraRequest { get; } = new MoveCameraRequest();

        public AnimateCameraRequest AnimateCameraRequest { get; } = new AnimateCameraRequest();

        public double ZoomLevel { get; set; } = 14d;

        private MapType mapType;
        public MapType MapType { get => mapType; set => SetProperty(ref mapType, value); }

        private Color colorMapType = (Color)Prism.PrismApplicationBase.Current.Resources["WhiteColor"];
        public Color ColorMapType { get => colorMapType; set => SetProperty(ref colorMapType, value); }

        private Color backgroundMapType = (Color)Prism.PrismApplicationBase.Current.Resources["PrimaryColor"];
        public Color BackgroundMapType { get => backgroundMapType; set => SetProperty(ref backgroundMapType, value); }

        private Color colorTrackingCar = (Color)Prism.PrismApplicationBase.Current.Resources["WhiteColor"];
        public Color ColorTrackingCar { get => colorTrackingCar; set => SetProperty(ref colorTrackingCar, value); }

        private Color backgroundTrackingCar = (Color)Prism.PrismApplicationBase.Current.Resources["PrimaryColor"];
        public Color BackgroundTrackingCar { get => backgroundTrackingCar; set => SetProperty(ref backgroundTrackingCar, value); }

        private DateTime dateStart = DateTime.Now.Subtract(TimeSpan.FromHours(24));
        public DateTime DateStart { get => dateStart; set => SetProperty(ref dateStart, value); }

        private DateTime dateEnd = DateTime.Now;
        public DateTime DateEnd { get => dateEnd; set => SetProperty(ref dateEnd, value); }

        public bool isWatching = true;
        public bool IsWatching { get => isWatching; set => SetProperty(ref isWatching, value); }

        public bool isPlaying;
        public bool IsPlaying { get => isPlaying; set => SetProperty(ref isPlaying, value, relatedProperty: nameof(PlayStopImage)); }

        public string PlayStopImage => IsPlaying ? "ic_stop_white" : "ic_play";

        public int playSpeed = 2;
        public int PlaySpeed { get => playSpeed; set => SetProperty(ref playSpeed, value); }

        public int playMin = 0;
        public int PlayMin { get => playMin; set => SetProperty(ref playMin, value); }

        public int playMax = 99;
        public int PlayMax { get => playMax; set => SetProperty(ref playMax, value); }

        public int playCurrent = 0;
        public int PlayCurrent { get => playCurrent; set => SetProperty(ref playCurrent, value); }

        public bool playControlEnabled;
        public bool PlayControlEnabled { get => !playControlEnabled; set => SetProperty(ref playControlEnabled, value); }
        private double SPEED_MAX = 8;
        private int BaseTimeMoving = 800;
        private int BaseTimeRotating = 250;

        private bool lastPlayStatus;

        #endregion Property

        #region PrivateMethod

        private void TimeSelected(string args)
        {
            SafeExecute(() =>
            {
                if (double.TryParse(args, out double length) && length > 0)
                {
                    DateEnd = DateTime.Now;
                    DateStart = DateEnd.Subtract(TimeSpan.FromHours(length));
                    if (Vehicle != null && Vehicle.VehicleId > 0)
                    {
                        ValidateUserConfigGetHistoryRoute();
                    }
                }
                else
                {
                    DateStart = DateTime.Today.Date;
                    DateEnd = DateTime.Now;
                }
            });
        }

        private void DateSelected(DateChangedEventArgs args)
        {
            ValidateUserConfigGetHistoryRoute();
        }

        private void ViewList()
        {
            if (ListRoute.Count <= 0)
            {
                DisplayMessage.ShowMessageWarning(MobileResource.Route_Label_RouteNotExistVMS, 3000);
                return;
            }

            SafeExecute(async () =>
            {
                if (ctsRouting != null)
                    ctsRouting.Cancel();

                await NavigationService.NavigateAsync("BaseNavigationPage/RouteListPage", parameters: new NavigationParameters
                {
                    { ParameterKey.VehicleRoute, ListRoute }
                }, true, true);
            });
        }

        private void ChangeMapType()
        {
            if (MapType == MapType.Street)
            {
                MapType = MapType.Hybrid;
                ColorMapType = (Color)Prism.PrismApplicationBase.Current.Resources["WhiteColor"];
                BackgroundMapType = (Color)Prism.PrismApplicationBase.Current.Resources["PrimaryColor"];
            }
            else
            {
                ColorMapType = (Color)Prism.PrismApplicationBase.Current.Resources["PrimaryColor"];
                BackgroundMapType = (Color)Prism.PrismApplicationBase.Current.Resources["WhiteColor"];
                MapType = MapType.Street;
            }
        }

        public void NavigateToSettings()
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("BaseNavigationPage/BoundaryPage", null, useModalNavigation: true, true);
            });
        }

        private void WatchVehicle()
        {
            if (IsWatching)
            {
                BackgroundTrackingCar = (Color)Prism.PrismApplicationBase.Current.Resources["WhiteColor"];
                ColorTrackingCar = (Color)Prism.PrismApplicationBase.Current.Resources["PrimaryColor"];
                IsWatching = false;
            }
            else
            {
                BackgroundTrackingCar = (Color)Prism.PrismApplicationBase.Current.Resources["PrimaryColor"];
                ColorTrackingCar = (Color)Prism.PrismApplicationBase.Current.Resources["WhiteColor"];
                IsWatching = true;

                if (CurrentRoute != null)
                {
                    if (Device.RuntimePlatform == Device.iOS)
                        MoveCameraRequest.MoveCamera(CameraUpdateFactory.NewCameraPosition(new CameraPosition(new Position(CurrentRoute.Latitude, CurrentRoute.Longitude), ZoomLevel)));
                    else
                        MoveCameraRequest.MoveCamera(CameraUpdateFactory.NewPosition(new Position(CurrentRoute.Latitude, CurrentRoute.Longitude)));
                }
            }
        }

        public void StopWatchVehicle()
        {
            if (IsWatching)
            {
                BackgroundTrackingCar = (Color)Prism.PrismApplicationBase.Current.Resources["WhiteColor"];
                ColorTrackingCar = (Color)Prism.PrismApplicationBase.Current.Resources["PrimaryColor"];
                IsWatching = false;
            }
        }

        private void SearchVehicle()
        {
            SafeExecute(async () =>
            {
                if (ctsRouting != null)
                    ctsRouting.Cancel();

                await NavigationService.NavigateAsync("BaseNavigationPage/VehicleLookUp", parameters: new NavigationParameters
                {
                    { ParameterKey.VehicleLookUpType, VehicleLookUpType.VehicleRoute }
                }, true, true);
            });
        }

        private void ValidateUserConfigGetHistoryRoute()
        {
            var currentCompany = Settings.CurrentCompany;
            RunOnBackground(async () =>
            {
                return await vehicleRouteService.ValidateUserConfigGetHistoryRoute(new ValidateUserConfigGetHistoryRouteRequest
                {
                    UserId = currentCompany?.UserId ?? UserInfo.UserId,
                    CompanyId = currentCompany?.FK_CompanyID ?? CurrentComanyID,
                    VehiclePlate = Vehicle.VehiclePlate,
                    FromDate = DateStart,
                    ToDate = DateEnd,
                    AppID = (int)App.AppType
                });
            }, (result) =>
            {
                if (result != null && result.Success && result.State == ValidatedHistoryRouteState.Success)
                {
                    if (ctsRouting != null)
                        ctsRouting.Cancel();
                    IsPlaying = false;
                   
                    GetHistoryRoute();
                }
                else
                {
                    ClearRoute();

                    ProcessUserConfigGetHistoryRoute(result);
                }
            });
        }

        private void ProcessUserConfigGetHistoryRoute(ValidateUserConfigGetHistoryRouteResponse result)
        {
            if (result != null && result.State != ValidatedHistoryRouteState.Success)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    switch (result.State)
                    {
                        case ValidatedHistoryRouteState.OverTotalDateMobile:
                            PageDialog.DisplayAlertAsync(MobileResource.Common_Label_Notification, string.Format(MobileResource.Route_Label_TotalTimeLimit, result.TotalDayConfig), MobileResource.Common_Button_OK);
                            break;

                        case ValidatedHistoryRouteState.Expired:
                            PageDialog.DisplayAlertAsync(MobileResource.Common_Label_Notification, MobileResource.Route_Label_AccountIsExpired, MobileResource.Common_Button_OK);
                            break;

                        case ValidatedHistoryRouteState.OverDateConfig:
                            if (result.MinDate != null && result.MaxDate != null)
                            {
                                PageDialog.DisplayAlertAsync(MobileResource.Common_Label_Notification, string.Format(MobileResource.Route_Label_FromDateToDateLimit, result.MinDate.FormatDate(), result.MaxDate.FormatDate()), MobileResource.Common_Button_OK);
                            }
                            else if (result.MinDate != null)
                            {
                                PageDialog.DisplayAlertAsync(MobileResource.Common_Label_Notification, string.Format(MobileResource.Route_Label_FromDateLimit, result.MinDate.FormatDate()), MobileResource.Common_Button_OK);
                            }
                            else if (result.MaxDate != null)
                            {
                                PageDialog.DisplayAlertAsync(MobileResource.Common_Label_Notification, string.Format(MobileResource.Route_Label_ToDateLimit, result.MaxDate.FormatDate()), MobileResource.Common_Button_OK);
                            }
                            else
                            {
                                PageDialog.DisplayAlertAsync(MobileResource.Common_Label_Notification, MobileResource.Route_Label_OverDateLimit, MobileResource.Common_Button_OK);
                            }
                            break;

                        case ValidatedHistoryRouteState.DateFuture:
                            PageDialog.DisplayAlertAsync(MobileResource.Common_Label_Notification, MobileResource.Route_Label_EndDateLimit, MobileResource.Common_Button_OK);
                            break;

                        case ValidatedHistoryRouteState.FromDateOverToDate:
                            PageDialog.DisplayAlertAsync(MobileResource.Common_Label_Notification, MobileResource.Route_Label_StartDateMustSmallerThanEndDate, MobileResource.Common_Button_OK);
                            break;
                    }
                });
            }
        }

        private void GetHistoryRoute()
        {
            var currentCompany = Settings.CurrentCompany;
            Xamarin.Forms.DependencyService.Get<IHUDProvider>().DisplayProgress("Đang tải dữ liệu...");
            RunOnBackground(async () =>
            {
                return await vehicleRouteService.GetHistoryRoute(new RouteHistoryRequest
                {
                    XnCode = StaticSettings.User.XNCode,
                    UserId = currentCompany?.UserId ?? UserInfo.UserId,
                    CompanyId = currentCompany?.FK_CompanyID ?? CurrentComanyID,
                    VehiclePlate = Vehicle.VehiclePlate,
                    FromDate = DateStart,
                    ToDate = DateEnd,
                });
            }, (result) =>
            {
                if (result != null)
                {
                    try
                    {
                        ClearRoute();

                        RouteHistory = result;

                        DateKm = RouteHistory.DateKm / 1000;

                        InitRoute();

                        if (ListRoute.Count == 0)
                        {
                            DisplayMessage.ShowMessageWarning(MobileResource.Route_Label_RouteNotFoundVMS, 3000);
                            Xamarin.Forms.DependencyService.Get<IHUDProvider>().Dismiss();
                            return;
                        }

                        if (ListRoute.Count == 1)
                        {
                            ListRoute.Add(ListRoute[0].DeepCopy());
                        }

                        DrawRoute();

                        PlayMax = ListRoute.Count - 1;

                        PlayControlEnabled = true;

                        RouteHistory = null;
                    }
                    catch (Exception)
                    {
                        PageDialog.DisplayAlertAsync("", "Không tải được dữ liệu", MobileResource.Common_Button_OK);
                    }
                }
                else
                {
                    PageDialog.DisplayAlertAsync("", "Không tải được dữ liệu", MobileResource.Common_Button_OK);
                }
                Xamarin.Forms.DependencyService.Get<IHUDProvider>().Dismiss();
            });
        }

        private void InitRoute()
        {
            try
            {
                if (RouteHistory.DirectionDetail == null)
                    return;

                var listLatLng = GeoHelper.DecodePoly(RouteHistory.DirectionDetail);

                if (listLatLng == null || listLatLng.Count == 0)
                {
                    return;
                }

                if (listLatLng.Count == 1)
                {
                    listLatLng.Add(new Position(listLatLng[0].Latitude, listLatLng[0].Longitude));
                    RouteHistory.TimePoints.AddedTimes.Add(RouteHistory.TimePoints.AddedTimes[0]);
                }

                var startTime = RouteHistory.TimePoints.StartTime;

                void AddRoute(int index)
                {
                    var route = new VehicleRoute()
                    {
                        Index = index,
                        Latitude = listLatLng[index].Latitude,
                        Longitude = listLatLng[index].Longitude,
                        State = RouteHistory.StateGPSPoints[index],
                        Velocity = RouteHistory.VelocityPoints[index],
                        Time = startTime
                    };

                    ListRoute.Add(route);
                }

                AddRoute(0);

                startTime = startTime.AddSeconds(RouteHistory.TimePoints.AddedTimes[0]);

                for (int i = 1; i < listLatLng.Count; i++)
                {
                    startTime = startTime.AddSeconds(RouteHistory.TimePoints.AddedTimes[i]);

                    if (listLatLng[i - 1].Latitude == listLatLng[i].Latitude
                        && listLatLng[i - 1].Longitude == listLatLng[i].Longitude
                        && !RouteHistory.StatePoints.Any(stp => stp.StartIndex == i && i == stp.EndIndex))
                        continue;

                    AddRoute(i);
                }
            }
            catch (Exception ex)
            {
                PageDialog.DisplayAlertAsync("Init Route Error", ex.Message, MobileResource.Common_Button_OK);
            }
        }

        private void DrawRoute()
        {
            DrawMarkerCar();
            DrawMarkerStartEnd();
            DrawDiretionMarker();
            DrawLine();
            StartRoute();
        }

        private string PinLabel(VehicleRoute vehicle)
        {
            if (Device.RuntimePlatform == Device.iOS)
                return string.Format("{0} {1}", vehicle.StateType.StartTime.FormatDateTimeWithSecond(), vehicle.StateType.Duration.SecondsToString());
            else
                return string.Format("{0} {1}: {2}", vehicle.StateType.StartTime.FormatDateTimeWithSecond(), MobileResource.Common_Label_Duration2, vehicle.StateType.Duration.SecondsToStringShort());
        }

        private void DrawStopPoint(VehicleRoute vehicle)
        {
            var pin = new Pin()
            {
                Type = PinType.Place,
                Label = PinLabel(vehicle),
                Anchor = new Point(.5, .5),
                Position = new Position(vehicle.Latitude, vehicle.Longitude),
                Icon = BitmapDescriptorFactory.FromResource("ic_stop.png"),
                Tag = "state_stop_route",
                ZIndex = 1,
                IsDraggable = false
            };

            Pins.Add(pin);
        }

        private void DrawMarkerCar()
        {
            var MarkerCar = new DoubleMarkerRoute().InitDoubleMarkerRoute(
                        ListRoute[0].Latitude, ListRoute[0].Longitude, ListRoute[1].Latitude, ListRoute[1].Longitude, Vehicle.PrivateCode);
            MarkerCar.DrawMarker(ListRoute[0]);
            Pins.Add(MarkerCar.Car);
            Pins.Add(MarkerCar.Plate);
        }

        private void DrawMarkerStartEnd()
        {
            Pins.Add(new Pin()
            {
                Type = PinType.Place,
                Label = "Bắt đầu",
                Position = new Position(ListRoute[0].Latitude, ListRoute[0].Longitude),
                Icon = BitmapDescriptorFactory.FromResource("ic_start.png"),
                ZIndex = 0,
                Tag = "pin_start_route",
                IsDraggable = false
            });

            Pins.Add(new Pin()
            {
                Type = PinType.Place,
                Label = "Kết thúc",
                Position = new Position(ListRoute[ListRoute.Count - 1].Latitude, ListRoute[ListRoute.Count - 1].Longitude),
                Icon = BitmapDescriptorFactory.FromResource("ic_end.png"),
                ZIndex = 0,
                Tag = "pin_end_route",
                IsDraggable = false
            });
        }

        private void DrawLine()
        {
            var line = new Polyline
            {
                IsClickable = false,
                StrokeColor = Color.FromHex("#4285f4"),
                StrokeWidth = 3f,
                ZIndex = 1
            };
            line.Positions.Add(new Position(ListRoute[0].Latitude, ListRoute[0].Longitude));

            for (int i = 0; i < ListRoute.Count; i++)
            {
                line.Positions.Add(new Position(ListRoute[i].Latitude, ListRoute[i].Longitude));
                if (ListRoute[i].StateType != null && ListRoute[i].StateType.State == StateType.Stop)
                {
                    DrawStopPoint(ListRoute[i]);
                }
            }
            Polylines.Add(line);
        }

        /* Vẽ hướng cho lộ trình */

        private void DrawDiretionMarker()
        {
            List<DirectionMarker> diretionList = new List<DirectionMarker>();
            for (int i = 0; i < ListRoute.Count() - 1; i++)
            {
                diretionList.Add(new DirectionMarker().InitDirectionPoint(
                        ListRoute[i].Latitude,
                        ListRoute[i].Longitude,
                        ListRoute[i + 1].Latitude,
                        ListRoute[i + 1].Longitude,
                        ListRoute[i].Time.FormatDateTimeWithSecond()));
            }
            // Lưu giá trị index được vẽ marker
            int indexDraw = 0;
            for (int i = 0; i < diretionList.Count() - 1; i++)
            {
                // Tính hướng tối thiểu
                float phi = Math.Abs(diretionList[i].Direction
                        - diretionList[i + 1].Direction) % 360;
                float directionMin = phi > 180 ? 360 - phi : phi;
                // Tính khoản cách tối thiểu
                float distance = (float)GeoHelper.ComputeDistanceBetween(
                        diretionList[indexDraw].Position.Latitude, diretionList[indexDraw].Position.Longitude,
                        diretionList[indexDraw + 1].Position.Latitude, diretionList[indexDraw + 1].Position.Longitude);
                if (directionMin > 30 || distance > 1200)
                {
                    var pin = diretionList[indexDraw].DrawPointDiretion();
                    Pins.Add(pin);
                    indexDraw = i + 1;
                }
            }
        }

        private void PinClicked(PinClickedEventArgs args)
        {
            if (!"state_stop_route".Equals(args.Pin.Tag) && !"direction_route".Equals(args.Pin.Tag))
            {
                args.Handled = true;
                return;
            }

            args.Handled = false;
            StopWatchVehicle();
            args.Pin.Address = string.Join(", ", GeoHelper.LatitudeToDergeeMinSec(args.Pin.Position.Latitude), GeoHelper.LongitudeToDergeeMinSec(args.Pin.Position.Longitude));

            args.Handled = false;
        }

        private void ClearRoute()
        {
            if (ctsRouting != null)
                ctsRouting.Cancel();
            IsPlaying = false;
            

            if (ListRoute != null)
                ListRoute.Clear();
            if (Polylines != null)
                Polylines.Clear();
            if (Pins != null)
                Pins.Clear();
            PlayCurrent = 0;
            PlayControlEnabled = false;
            DateKm = 0;
            CurrentRoute = null;
        }

        private void StartRoute()
        {
            try
            {
                if (ctsRouting != null)
                    ctsRouting.Cancel();

                ctsRouting = new CancellationTokenSource();

                CurrentRoute = ListRoute[0];
                var doubeMarker = Pins.Where(x => x.Label == Vehicle.PrivateCode).ToList();
                if (doubeMarker != null && doubeMarker.Count > 1)
                {
                    MoveCameraRequest.MoveCamera(CameraUpdateFactory.NewCameraPosition(new CameraPosition(doubeMarker[0].Position, ZoomLevel)));

                    SuperInteligent(doubeMarker[0], doubeMarker[1]);

                    IsPlaying = true;
                }
            }
            catch (Exception ex)
            {
                if (ctsRouting != null)
                    ctsRouting.Cancel();

                IsPlaying = false;

                PageDialog.DisplayAlertAsync("", ex.Message, MobileResource.Common_Button_OK);
            }
        }

        private void PlayStop()
        {
            try
            {
                if (!IsPlaying)
                {
                    if (PlayCurrent >= PlayMax)
                        return;

                    if (ctsRouting != null)
                        ctsRouting.Cancel();

                    ctsRouting = new CancellationTokenSource();

                    MoveToCurrent();
                }
                else
                {
                    StopRoute();

                    IsPlaying = false;
                }
            }
            catch (Exception ex)
            {
                if (ctsRouting != null)
                    ctsRouting.Cancel();

                IsPlaying = false;

                PageDialog.DisplayAlertAsync("", ex.Message, MobileResource.Common_Button_OK);
            }
        }

        private void StopRoute()
        {
            if (IsPlaying)
            {
                if (ctsRouting != null)
                    ctsRouting.Cancel();

                IsPlaying = false;
            }
        }

        private void SuperInteligent(Pin item, Pin itemLable)
        {
            try
            {
                PlayCurrent++;

                CurrentRoute = ListRoute[PlayCurrent];

                Device.BeginInvokeOnMainThread(() =>
                {
                    if (CurrentRoute == null)
                        return;
                    RotateMarker(item, CurrentRoute.Latitude, CurrentRoute.Longitude, () =>
                    {
                        if (CurrentRoute == null)
                            return;
                        MarkerAnimation(item, itemLable, CurrentRoute.Latitude, CurrentRoute.Longitude, () =>
                        {
                            if (PlayCurrent + 1 > PlayMax || ctsRouting.IsCancellationRequested)
                            {
                                IsPlaying = false;
                                return;
                            }

                            SuperInteligent(item, itemLable);
                        });
                    });
                });
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        public bool IsRunning = false;

        public void RotateMarker(Pin item, double latitude,
           double longitude,
           Action callback)
        {
            // * tính góc quay giữa 2 điểm location
            var angle = GeoHelper.ComputeHeading(item.Position.Latitude, item.Position.Longitude, latitude, longitude);
            if (angle == 0)
            {
                callback();
                return;
            }
            var startRotaion = item.Rotation;
            //tính lại độ lệch góc
            var deltaAngle = GeoHelper.GetRotaion(startRotaion, angle);
            void callbackanimate(double input)
            {
                var fractionAngle = GeoHelper.ComputeRotation(
                                     input,
                                      startRotaion,
                                      deltaAngle);

                item.Rotation = (float)fractionAngle;
            }
            view.Animate(
                "rotateCarRoute",
                animation: new Animation(callbackanimate),
                length: (uint)(BaseTimeRotating / PlaySpeed),

                finished: (val, b) =>
                {
                    callback();
                }
                );
        }

        public void MarkerAnimation(Pin item, Pin itemLable, double latitude, double longitude, Action callback)
        {
            if (this.IsRunning)
            {
                callback();
            }
            else
            {
                IsRunning = true;
                var startPosition = new Position(item.Position.Latitude, item.Position.Longitude);
                var finalPosition = new Position(latitude, longitude);
                void callbackanimate(double input)
                {
                    var postionnew = GeoHelper.LinearInterpolator(input,
                        startPosition,
                        finalPosition);
                    itemLable.Position = postionnew;
                    item.Position = postionnew;
                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        if (IsWatching && !ctsRouting.IsCancellationRequested)
                        {
                            _ = AnimateCameraRequest.AnimateCamera(CameraUpdateFactory.NewPosition(postionnew), TimeSpan.FromMilliseconds(1000 / PlaySpeed));
                        }
                    }
                }
                view.Animate(
                "moveCarRoute",
                animation: new Animation(callbackanimate),
                length: (uint)(BaseTimeMoving / PlaySpeed),
                finished: (val, b) =>
                {
                    if (Device.RuntimePlatform == Device.Android)
                    {
                        if (IsWatching && !ctsRouting.IsCancellationRequested)
                        {
                            _ = AnimateCameraRequest.AnimateCamera(CameraUpdateFactory.NewPosition(item.Position), TimeSpan.FromMilliseconds(300));
                        }
                    }
                    IsRunning = false;
                    callback();
                }
                );
            }
        }

        private void DragStarted()
        {
            lastPlayStatus = IsPlaying;

            if (ctsRouting != null)
                ctsRouting.Cancel();
        }

        private void DragCompleted()
        {
            try
            {
                if (lastPlayStatus)
                {
                    PlayStop();
                    lastPlayStatus = false;
                }
            }
            catch (Exception ex)
            {
                PageDialog.DisplayAlertAsync("", ex.Message, MobileResource.Common_Button_OK);
            }
        }

        private void MoveToCurrent()
        {
            try
            {
                CurrentRoute = ListRoute[PlayCurrent];
                var doubeMarker = Pins.Where(x => x.Label == Vehicle.PrivateCode).ToList();
                if (doubeMarker != null && doubeMarker.Count > 1)
                {
                    if (PlayCurrent > 0)
                    {
                        doubeMarker[0].Rotation = (float)GeoHelper.ComputeHeading(ListRoute[PlayCurrent - 1].Latitude, ListRoute[PlayCurrent - 1].Longitude, CurrentRoute.Latitude, CurrentRoute.Longitude);
                    }
                    else
                    {
                        doubeMarker[0].Rotation = (float)GeoHelper.ComputeHeading(CurrentRoute.Latitude, CurrentRoute.Longitude, ListRoute[PlayCurrent + 1].Latitude, ListRoute[PlayCurrent + 1].Longitude);
                    }
                    doubeMarker[0].Position = new Position(CurrentRoute.Latitude, CurrentRoute.Longitude);
                    doubeMarker[1].Position = doubeMarker[0].Position;

                    MoveCameraRequest.MoveCamera(CameraUpdateFactory.NewPosition(doubeMarker[0].Position));

                    SuperInteligent(doubeMarker[0], doubeMarker[1]);

                    IsPlaying = true;
                }
            }
            catch (Exception ex)
            {
                PageDialog.DisplayAlertAsync("", ex.Message, MobileResource.Common_Button_OK);
            }
        }

        private void IncreaseSpeed()
        {
            if (PlaySpeed >= SPEED_MAX)
                return;

            PlaySpeed *= 2;
        }

        private void DecreaseSpeed()
        {
            if (PlaySpeed <= 1)
                return;

            PlaySpeed /= 2;
        }

        private void FastStart()
        {
            PlayCurrent = PlayMin;

            MoveToCurrent();
        }

        private void FastEnd()
        {
            PlayCurrent = PlayMax;

            MoveToCurrent();
        }

        private async void ChangeSpeed()
        {
            if (PlaySpeed >= SPEED_MAX)
            {
                PlaySpeed = 4;
                await Task.Delay(1000);
                PlaySpeed = 2;
                await Task.Delay(1000);
                PlaySpeed = 1;
            }
            else
            {
                PlaySpeed *= 2;
            }
        }

        private void GoogleMapAddBoundary()
        {
            Boundaries.Clear();
            var temp = Polylines?.Where(l => l.Tag != null && l.Tag.ToString() == "POLYGON")?.ToList();
            foreach (var line in temp)
            {
                Polylines.Remove(line);
            }

            var listBoudary = boundaryRepository.Find(b => b.IsShowBoudary);

            foreach (var item in listBoudary)
            {
                AddBoundary(item);
            }
        }

        private void AddBoundary(LandmarkResponse boundary)
        {
            try
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

                    Boundaries.Add(polygon);
                }
                else
                {
                    var polyline = new Polyline
                    {
                        IsClickable = false,
                        StrokeColor = color,
                        StrokeWidth = 2f,
                        Tag = "POLYGON"
                    };

                    for (int i = 0; i < result.Length; i += 2)
                    {
                        polyline.Positions.Add(new Position(FormatHelper.ConvertToDouble(result[i + 1], 6), FormatHelper.ConvertToDouble(result[i], 6)));
                    }

                    Polylines.Add(polyline);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        private string ConvertIntToHex(int value)
        {
            return value.ToString("X").PadLeft(6, '0');
        }

        private void GoogleMapAddName()
        {
            try
            {
                var listName = boundaryRepository.Find(b => b.IsShowName);

                foreach (var pin in Pins.Where(p => p.Tag.ToString().Contains("Boundary")).ToList())
                {
                    Pins.Remove(pin);
                }

                foreach (var item in listName)
                {
                    AddName(item);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        private void AddName(LandmarkResponse name)
        {
            try
            {
                Pins.Add(new Pin
                {
                    Label = name.Name,
                    Position = new Position(name.Latitude, name.Longitude),
                    Icon = BitmapDescriptorFactory.FromView(new BoundaryNameInfoWindow(name.Name) { WidthRequest = name.Name.Length < 20 ? 6 * name.Name.Length : 110, HeightRequest = 18 * ((name.Name.Length / 20) + 1) }),
                    Tag = "Boundary" + name.Name
                });
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        #endregion PrivateMethod
    }
}