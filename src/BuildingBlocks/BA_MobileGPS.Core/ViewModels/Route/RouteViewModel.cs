﻿using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Events;
using BA_MobileGPS.Core.GoogleMap.Behaviors;
using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Core.Models;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Core.Views;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;
using Prism.Commands;
using Prism.Navigation;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Forms.Extensions;

namespace BA_MobileGPS.Core.ViewModels
{
    public class RouteViewModel : ViewModelBase
    {
        #region Contructor

        private readonly IGeocodeService geocodeService;
        private readonly IVehicleRouteService vehicleRouteService;
        private readonly IDisplayMessage displayMessage;

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

        public RouteViewModel(INavigationService navigationService, IVehicleRouteService vehicleRouteService, IDisplayMessage displayMessage, IGeocodeService geocodeService)
           : base(navigationService)
        {
            this.vehicleRouteService = vehicleRouteService;
            this.displayMessage = displayMessage;
            this.geocodeService = geocodeService;

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

            TimeSelectedCommand = new Command<string>(TimeSelected);
            DateSelectedCommand = new Command<DateChangedEventArgs>(DateSelected);
            SearchVehicleCommand = new Command(SearchVehicle);
            GetVehicleRouteCommand = new Command(GetVehicleRoute);
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
            EventAggregator.GetEvent<TabItemSwitchEvent>().Subscribe(TabItemSwitch);
            EventAggregator.GetEvent<ThemeChangedEvent>().Subscribe(ThemeChanged);
            EventAggregator.GetEvent<TabSelectedChangedEvent>().Subscribe(TabSelectedChanged);
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

                    GetVehicleRoute();
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

                    GetVehicleRoute();
                }
            }
        }

        public override void OnDestroy()
        {
            if (ctsRouting != null)
                ctsRouting.Cancel();

            EventAggregator.GetEvent<TabItemSwitchEvent>().Unsubscribe(TabItemSwitch);
            EventAggregator.GetEvent<ThemeChangedEvent>().Unsubscribe(ThemeChanged);
            EventAggregator.GetEvent<TabSelectedChangedEvent>().Subscribe(TabSelectedChanged);
        }

        #endregion Lifecycle

        #region Property

        private CancellationTokenSource ctsRouting = new CancellationTokenSource();
        private CancellationTokenSource ctsAddress = new CancellationTokenSource();

        private Polyline RouteLine;

        private RouteHistoryResponse RouteHistory;

        private Pin PinCar, PinPlate;

        public List<VehicleRoute> ListRoute { get; set; } = new List<VehicleRoute>();

        private Vehicle vehicle = new Vehicle();
        public Vehicle Vehicle { get => vehicle; set => SetProperty(ref vehicle, value); }

        private float dateKm;
        public float DateKm { get => dateKm; set => SetProperty(ref dateKm, value); }

        private VehicleRoute currentRoute;
        public VehicleRoute CurrentRoute { get => currentRoute; set => SetProperty(ref currentRoute, value); }

        public ObservableCollection<Pin> Pins { get; set; } = new ObservableCollection<Pin>();

        public ObservableCollection<Polyline> Polylines { get; set; } = new ObservableCollection<Polyline>();

        public ObservableCollection<Polygon> Boundaries { get; set; } = new ObservableCollection<Polygon>();

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

        public int playSpeed = 4;
        public int PlaySpeed { get => playSpeed; set => SetProperty(ref playSpeed, value); }

        public int playMin = 0;
        public int PlayMin { get => playMin; set => SetProperty(ref playMin, value); }

        public int playMax = 99;
        public int PlayMax { get => playMax; set => SetProperty(ref playMax, value); }

        public int playCurrent = 0;
        public int PlayCurrent { get => playCurrent; set => SetProperty(ref playCurrent, value); }

        public bool playControlEnabled;
        public bool PlayControlEnabled { get => !playControlEnabled; set => SetProperty(ref playControlEnabled, value); }

        private readonly double SPEED_MAX = 8;
        private readonly double BASE_TIME = 250;

        private readonly double MARKER_ROTATE_RATE = 0.1;
        private double MARKER_ROTATE_STEP => 5;
        private double MARKER_ROTATE_TIME_STEP => MARKER_ROTATE_RATE * BASE_TIME / PlaySpeed / MARKER_ROTATE_STEP;

        private double MARKER_MOVE_RATE => 1 - MARKER_ROTATE_RATE;
        private double MARKER_MOVE_STEP => 128 / PlaySpeed;
        private double MARKER_MOVE_TIME_STEP => BASE_TIME / PlaySpeed / MARKER_MOVE_STEP;

        private bool lastPlayStatus;
        private bool isMarkerRotating = false;
        private bool isMarkerMoving = false;

        #endregion Property

        #region PrivateMethod

        private void TabItemSwitch(Tuple<ItemTabPageEnums, object> obj)
        {
            if (obj != null
              && obj.Item2 != null
              && obj.Item1 == ItemTabPageEnums.RoutePage
              && obj.Item2.GetType() == typeof(VehicleOnline))
            {
                var vehicleOnline = (VehicleOnline)obj.Item2;
                Vehicle = new Vehicle()
                {
                    GroupIDs = vehicleOnline.GroupIDs,
                    PrivateCode = vehicleOnline.PrivateCode,
                    VehicleId = vehicleOnline.VehicleId,
                    VehiclePlate = vehicleOnline.VehiclePlate
                };

                // Gán lại thời gian
                DateStart = DateTime.Now.Subtract(TimeSpan.FromHours(24));
                DateEnd = DateTime.Now;

                GetVehicleRoute();
            }
            else
            {
                StopRoute();
            }
        }

        private void TabSelectedChanged(int obj)
        {
            StopRoute();
        }

        private void TimeSelected(string args)
        {
            if (double.TryParse(args, out double length) && length > 0)
            {
                DateEnd = DateTime.Now;
                DateStart = DateEnd.Subtract(TimeSpan.FromHours(length));
                GetVehicleRoute();
            }
            else
            {
                DateStart = DateTime.Today.Date;
                DateEnd = DateTime.Now;
            }
        }

        private void DateSelected(DateChangedEventArgs args)
        {
            GetVehicleRoute();
        }

        private void ViewList()
        {
            if (ListRoute.Count <= 0)
            {
                displayMessage.ShowMessageWarning(MobileResource.Route_Label_RouteNotExist, 3000);
                return;
            }

            SafeExecute(async () =>
            {
                if (ctsRouting != null)
                    ctsRouting.Cancel();

                await NavigationService.NavigateAsync("BaseNavigationPage/RouteListPage", useModalNavigation: true, parameters: new NavigationParameters
                {
                    { ParameterKey.VehicleRoute, ListRoute }
                });
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
                await NavigationService.NavigateAsync("BaseNavigationPage/BoundaryPage", useModalNavigation: true);
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

                if (PinCar != null)
                {
                    if (Device.RuntimePlatform == Device.iOS)
                        MoveCameraRequest.MoveCamera(CameraUpdateFactory.NewCameraPosition(new CameraPosition(PinCar.Position, ZoomLevel)));
                    else
                        MoveCameraRequest.MoveCamera(CameraUpdateFactory.NewPosition(PinCar.Position));
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

                await NavigationService.NavigateAsync("BaseNavigationPage/VehicleLookUp", useModalNavigation: true, parameters: new NavigationParameters
                {
                    { ParameterKey.VehicleLookUpType, VehicleLookUpType.VehicleRoute }
                });
            });
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(Vehicle.VehiclePlate))
            {
                displayMessage.ShowMessageWarning(MobileResource.Route_Label_VehicleEmpty, 3000);
                return false;
            }

            return true;
        }

        private void GetVehicleRoute()
        {
            if (IsBusy || !IsConnected || !ValidateInput())
                return;

            if (ctsRouting != null)
                ctsRouting.Cancel();

            if (ctsAddress != null)
                ctsAddress.Cancel();

            ListRoute.Clear();

            //Polylines.Clear();

            foreach (var line in Polylines.ToList().FindAll(l => !"Boundary".Equals(l.Tag)))
            {
                Polylines.Remove(line);
            }

            //Pins.Clear();

            foreach (var pin in Pins.Where(p => !p.Tag.ToString().Contains("Boundary")).ToList())
            {
                Pins.Remove(pin);
            }

            CurrentRoute = null;
            IsPlaying = false;
            PlayCurrent = 0;
            PlayControlEnabled = false;
            DateKm = 0;

            DependencyService.Get<IHUDProvider>().DisplayProgress("");

            Task.Run(async () =>
            {
                var currentCompany = Settings.CurrentCompany;

                var result = await vehicleRouteService.ValidateUserConfigGetHistoryRoute(new ValidateUserConfigGetHistoryRouteRequest
                {
                    UserId = currentCompany?.UserId ?? UserInfo.UserId,
                    CompanyId = currentCompany?.FK_CompanyID ?? CurrentComanyID,
                    VehiclePlate = Vehicle.VehiclePlate,
                    FromDate = DateStart,
                    ToDate = DateEnd,
                    AppID = (int)App.AppType
                });

                if (result != null && result.Success && result.State == ValidatedHistoryRouteState.Success)
                {
                    return await vehicleRouteService.GetHistoryRoute(new RouteHistoryRequest
                    {
                        UserId = currentCompany?.UserId ?? UserInfo.UserId,
                        CompanyId = currentCompany?.FK_CompanyID ?? CurrentComanyID,
                        VehiclePlate = Vehicle.VehiclePlate,
                        FromDate = DateStart,
                        ToDate = DateEnd,
                    });
                }

                if (result != null)
                {
                    ProcessUserConfigGetHistoryRoute(result);
                }
                return null;
            }).ContinueWith(task => Device.BeginInvokeOnMainThread(() =>
            {
                if (task.Status == TaskStatus.RanToCompletion)
                {
                    try
                    {
                        if (task.Result == null)
                        {
                            DependencyService.Get<IHUDProvider>().Dismiss();
                            displayMessage.ShowMessageWarning(MobileResource.Route_Label_RouteNotFound, 3000);
                            return;
                        }

                        RouteHistory = task.Result;

                        DateKm = RouteHistory.DateKm / 1000;

                        InitRoute();

                        if (ListRoute.Count == 0)
                        {
                            DependencyService.Get<IHUDProvider>().Dismiss();
                            displayMessage.ShowMessageWarning(MobileResource.Route_Label_RouteNotFound, 3000);
                            return;
                        }

                        if (ListRoute.Count == 1)
                        {
                            ListRoute.Add(ListRoute[0].DeepCopy());
                        }

                        if (ListRoute.Count >= 5)
                        {
                            InitDirection();
                        }

                        DrawRoute();

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            CurrentRoute = ListRoute[0];
                            MoveCameraRequest.MoveCamera(CameraUpdateFactory.NewCameraPosition(new CameraPosition(new Position(ListRoute[0].Latitude, ListRoute[0].Longitude), ZoomLevel)));
                        });

                        PlayMax = ListRoute.Count - 1;
                        PlayControlEnabled = true;

                        RouteHistory = null;
                    }
                    catch (Exception ex)
                    {
                        PageDialog.DisplayAlertAsync("", "Không tải được dữ liệu", MobileResource.Common_Button_OK);
                        LoggerHelper.WriteError(MethodBase.GetCurrentMethod().Name, ex);
                    }
                }
                else if (task.IsFaulted)
                {
                    PageDialog.DisplayAlertAsync("", "Không tải được dữ liệu", MobileResource.Common_Button_OK);
                    LoggerHelper.WriteError(MethodBase.GetCurrentMethod().Name, task.Exception);
                }

                DependencyService.Get<IHUDProvider>().Dismiss();
            }));
        }

        private void ProcessUserConfigGetHistoryRoute(ValidateUserConfigGetHistoryRouteResponse result)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                switch (result.State)
                {
                    case ValidatedHistoryRouteState.OverTotalDateMobile:
                        PageDialog.DisplayAlertAsync("", MobileResource.Route_Label_TotalTimeLimit(result.TotalDayConfig), MobileResource.Common_Button_OK);
                        break;

                    case ValidatedHistoryRouteState.Expired:
                        PageDialog.DisplayAlertAsync("", MobileResource.Route_Label_AccountIsExpired, MobileResource.Common_Button_OK);
                        break;

                    case ValidatedHistoryRouteState.OverDateConfig:
                        if (result.MinDate != null && result.MaxDate != null)
                        {
                            if (result.MinDate > result.MaxDate)
                                PageDialog.DisplayAlertAsync("", MobileResource.Route_Label_ToDateFromDateLimit(result.MinDate.FormatDate(), result.MaxDate.FormatDate()), MobileResource.Common_Button_OK);
                            else
                                PageDialog.DisplayAlertAsync("", MobileResource.Route_Label_FromDateToDateLimit(result.MinDate.FormatDate(), result.MaxDate.FormatDate()), MobileResource.Common_Button_OK);
                        }
                        else if (result.MinDate != null)
                        {
                            PageDialog.DisplayAlertAsync("", MobileResource.Route_Label_FromDateLimit(result.MinDate.FormatDate()), MobileResource.Common_Button_OK);
                        }
                        else if (result.MaxDate != null)
                        {
                            PageDialog.DisplayAlertAsync("", MobileResource.Route_Label_ToDateLimit(result.MaxDate.FormatDate()), MobileResource.Common_Button_OK);
                        }
                        else
                        {
                            PageDialog.DisplayAlertAsync("", MobileResource.Route_Label_OverDateLimit, MobileResource.Common_Button_OK);
                        }
                        break;

                    case ValidatedHistoryRouteState.DateFuture:
                        PageDialog.DisplayAlertAsync("", MobileResource.Route_Label_EndDateLimit, MobileResource.Common_Button_OK);
                        break;

                    case ValidatedHistoryRouteState.FromDateOverToDate:
                        PageDialog.DisplayAlertAsync("", MobileResource.Route_Label_StartDateMustSmallerThanEndDate, MobileResource.Common_Button_OK);
                        break;
                }
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

                //var stopPoints = RouteHistory.StatePoints.FindAll(s => s.State == StateType.Stop);

                var startTime = RouteHistory.TimePoints.StartTime;

                void AddRoute(int index)
                {
                    var route = new VehicleRoute()
                    {
                        Index = index,
                        Latitude = listLatLng[index].Latitude,
                        Longitude = listLatLng[index].Longitude,
                        State = RouteHistory.StatePoints.FirstOrDefault(stp => stp.StartIndex <= index && index <= stp.EndIndex),
                        Velocity = RouteHistory.VelocityPoints[index],
                        Time = startTime
                    };

                    if (index == 0)
                    {
                        route.Heading1 = route.Heading2 = (float)GeoHelper.ComputeHeading(listLatLng[index].Latitude, listLatLng[index].Longitude, listLatLng[index + 1].Latitude, listLatLng[index + 1].Longitude);
                    }
                    else if (index <= listLatLng.Count - 1)
                    {
                        route.Heading2 = (float)GeoHelper.ComputeHeading(listLatLng[index - 1].Latitude, listLatLng[index - 1].Longitude, listLatLng[index].Latitude, listLatLng[index].Longitude);
                    }

                    if (ListRoute.Count > 0)
                    {
                        route.Heading1 = ListRoute[ListRoute.Count - 1].Heading2;
                        route.DeltaAngle = GeoHelper.GetRotaion(route.Heading1, route.Heading2);
                    }

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

                    if (GeoHelper.IsOriginLocation(listLatLng[i].Latitude, listLatLng[i].Longitude))
                        continue;

                    if (CalculateDistance(listLatLng[i - 1].Latitude, listLatLng[i - 1].Longitude, listLatLng[i].Latitude, listLatLng[i].Longitude) < 0.015
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

        private void InitDirection()
        {
            try
            {
                void AddDirection(int index)
                {
                    ListRoute[index].Direction = (float)GeoHelper.ComputeHeading(ListRoute[index].Latitude, ListRoute[index].Longitude, ListRoute[index + 1].Latitude, ListRoute[index + 1].Longitude);
                }

                for (int i = 1; i <= ListRoute.Count - 2; i++)
                {
                    if (!ListRoute.Exists(d => d.Direction != null && CalculateDistance(ListRoute[i].Latitude, ListRoute[i].Longitude, d.Latitude, d.Longitude) < 2))
                    {
                        AddDirection(i);
                    }
                    else
                    {
                        if (Math.Abs(ListRoute[i + 1].Heading2 - ListRoute[i].Heading2) >= 45
                            && !ListRoute.Exists(d => d.Direction != null && CalculateDistance(ListRoute[i].Latitude, ListRoute[i].Longitude, d.Latitude, d.Longitude) < 0.5))
                        {
                            AddDirection(i);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                PageDialog.DisplayAlertAsync("Init Direction Error", ex.Message, MobileResource.Common_Button_OK);
            }
        }

        private double CalculateDistance(double lat1, double lng1, double lat2, double lng2)
        {
            return Plugin.Geolocator.Abstractions.GeolocatorUtils.CalculateDistance(lat1, lng1, lat2, lng2, Plugin.Geolocator.Abstractions.GeolocatorUtils.DistanceUnits.Kilometers);
        }

        private void DrawRoute()
        {
            PinCar = new Pin()
            {
                Type = PinType.Place,
                Label = "pin_car",
                Anchor = new Point(.5, .5),
                Position = new Position(ListRoute[0].Latitude, ListRoute[0].Longitude),
                Rotation = (float)ListRoute[0].Heading2,
                Icon = BitmapDescriptorFactory.FromResource("car_blue.png"),
                ZIndex = 2,
                Tag = Vehicle.VehiclePlate,
                IsDraggable = false
            };
            Pins.Add(PinCar);

            PinPlate = new Pin()
            {
                Type = PinType.Place,
                Label = "pin_plate",
                Anchor = new Point(.5, .75),
                Position = new Position(ListRoute[0].Latitude, ListRoute[0].Longitude),
                Icon = BitmapDescriptorFactory.FromView(new PinInfowindowView(Vehicle.PrivateCode)),
                ZIndex = 2,
                Tag = Vehicle.VehiclePlate + "Plate",
                IsDraggable = false
            };

            Pins.Add(PinPlate);

            Pins.Add(new Pin()
            {
                Type = PinType.Place,
                Label = "",
                Position = new Position(ListRoute[0].Latitude, ListRoute[0].Longitude),
                Icon = BitmapDescriptorFactory.FromResource("ic_start.png"),
                ZIndex = 0,
                Tag = "pin_start",
                IsDraggable = false
            });

            Pins.Add(new Pin()
            {
                Type = PinType.Place,
                Label = "",
                Position = new Position(ListRoute[ListRoute.Count - 1].Latitude, ListRoute[ListRoute.Count - 1].Longitude),
                Icon = BitmapDescriptorFactory.FromResource("ic_end.png"),
                ZIndex = 0,
                Tag = "pin_end",
                IsDraggable = false
            });

            RouteLine = new Polyline
            {
                IsClickable = false,
                StrokeColor = Color.FromHex("#4285f4"),
                StrokeWidth = 3f,
                ZIndex = 1
            };

            RouteLine.Positions.Add(new Position(ListRoute[0].Latitude, ListRoute[0].Longitude));

            for (int i = 0; i < ListRoute.Count; i++)
            {
                RouteLine.Positions.Add(new Position(ListRoute[i].Latitude, ListRoute[i].Longitude));
                if (ListRoute[i].State != null && ListRoute[i].State.State == StateType.Stop)
                {
                    DrawStopPoint(ListRoute[i]);
                }

                if (ListRoute[i].Direction != null)
                {
                    DrawDirection(ListRoute[i]);
                }
            }

            Polylines.Add(RouteLine);

            StartRoute();
        }

        private string PinLabel(VehicleRoute vehicle)
        {
            if (Device.RuntimePlatform == Device.iOS)
                return string.Format("{0} {1}", vehicle.State.StartTime.FormatDateTimeWithSecond(), vehicle.State.Duration.SecondsToString());
            else
                return string.Format("{0} {1}: {2}", vehicle.State.StartTime.FormatDateTimeWithSecond(), MobileResource.Common_Label_Duration2, vehicle.State.Duration.SecondsToStringShort());
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
                Tag = "state_stop",
                ZIndex = 1,
                IsDraggable = false
            };

            Pins.Add(pin);
        }

        private void DrawDirection(VehicleRoute vehicle)
        {
            var pin = new Pin()
            {
                Type = PinType.Place,
                Label = vehicle.Time.FormatDateTimeWithSecond(),
                Anchor = new Point(.5, .5),
                Position = new Position(vehicle.Latitude, vehicle.Longitude),
                Rotation = vehicle.Direction ?? 0,
                Icon = BitmapDescriptorFactory.FromResource("ic_arrow_tracking.png"),
                Tag = "direction",
                ZIndex = 1,
                IsDraggable = false
            };

            Pins.Add(pin);
        }

        private void PinClicked(PinClickedEventArgs args)
        {
            if (!"state_stop".Equals(args.Pin.Tag) && !"direction".Equals(args.Pin.Tag))
            {
                args.Handled = true;
                return;
            }

            args.Handled = true;

            if (ctsAddress != null)
                ctsAddress.Cancel();

            ctsAddress = new CancellationTokenSource();

            Task.Run(async () =>
            {
                if (ctsAddress.IsCancellationRequested)
                    throw new Exception();
                return await geocodeService.GetAddressByLatLng(args.Pin.Position.Latitude.ToString(), args.Pin.Position.Longitude.ToString());
            }, ctsAddress.Token).ContinueWith(task => Device.BeginInvokeOnMainThread(() =>
            {
                if (task.Status == TaskStatus.RanToCompletion && !ctsAddress.IsCancellationRequested)
                {
                    args.Pin.Address = task.Result;
                }
            }));

            args.Handled = false;
        }

        private void StartRoute()
        {
            try
            {
                if (ctsRouting != null)
                    ctsRouting.Cancel();

                ctsRouting = new CancellationTokenSource();

                CurrentRoute = ListRoute[0];

                PinCar.Rotation = (float)GeoHelper.ComputeHeading(CurrentRoute.Latitude, CurrentRoute.Longitude, ListRoute[PlayCurrent + 1].Latitude, ListRoute[PlayCurrent + 1].Longitude);

                PinCar.Position = new Position(CurrentRoute.Latitude, CurrentRoute.Longitude);
                PinPlate.Position = PinCar.Position;

                MoveCameraRequest.MoveCamera(CameraUpdateFactory.NewCameraPosition(new CameraPosition(PinCar.Position, ZoomLevel)));

                SuperInteligent();

                IsPlaying = true;
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

                    SuperInteligent();

                    IsPlaying = true;
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

        private void SuperInteligent()
        {
            try
            {
                PlayCurrent++;

                CurrentRoute = ListRoute[PlayCurrent];
                var duration = 64 / PlaySpeed * 50;
                Device.BeginInvokeOnMainThread(() =>
                {
                    var durationRotate = duration * 10 / 100;
                    var durationmove = duration * 90 / 100;
                    RotateMarker((rotated) =>
                    {
                        isMarkerRotating = false;
                        MarkerAnimation(CurrentRoute.Latitude, CurrentRoute.Longitude, () =>
                            {
                                if (PlayCurrent + 1 > PlayMax || ctsRouting.IsCancellationRequested)
                                {
                                    IsPlaying = false;
                                    return;
                                }
                                SuperInteligent();
                            }, durationmove);
                    });
                });
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }
        public void MarkerAnimation(double latitude, double longitude, Action callback, int duration = 4000)
        {
            if (this.isMarkerMoving)
            {
                callback();
            }
            else
            {
                isMarkerMoving = true;
                //gán lại vòng quay
                double mMoveIndex = 0;
                double MAX_MOVE_STEP = duration / 10;
                var startPosition = PinCar.Position;

                var finalPosition = new Position(latitude, longitude);
                double elapsed = 0;
                double t;
                double v;

                while (mMoveIndex <= MAX_MOVE_STEP && !ctsRouting.IsCancellationRequested)
                {
                    // Calculate progress using interpolator
                    elapsed = elapsed + 10;
                    t = elapsed / duration;
                    v = GeoHelper.GetInterpolation(t);

                    var postionnew = GeoHelper.Interpolate(v,
                        new Position(startPosition.Latitude, startPosition.Longitude),
                        finalPosition);

                    mMoveIndex = mMoveIndex + 1;
                    PinCar.Position = new Position(postionnew.Latitude, postionnew.Longitude);
                    PinPlate.Position = new Position(postionnew.Latitude, postionnew.Longitude);
                    if (IsWatching && !ctsRouting.IsCancellationRequested)
                    {
                        _ = MoveCameraRequest.MoveCamera(CameraUpdateFactory.NewPosition(PinCar.Position));
                    }
                    if (mMoveIndex > MAX_MOVE_STEP)
                    {
                        isMarkerMoving = false;
                        callback();
                    }
                };
                isMarkerMoving = false;
            }
        }
        private async void RotateMarker(Action<bool> callback = null)
        {
            try
            {
                if (!isMarkerRotating)
                {
                    isMarkerRotating = true;
                    if (-10 <= CurrentRoute.DeltaAngle && CurrentRoute.DeltaAngle <= 10)
                    {
                        PinCar.Rotation = (float)CurrentRoute.Heading2;
                        callback?.Invoke(false);
                        return;
                    }

                    int rotateIndex = 1;

                    while (rotateIndex <= MARKER_ROTATE_STEP && !ctsRouting.IsCancellationRequested)
                    {
                        PinCar.Rotation = (float)GeoHelper.ComputeRotation(rotateIndex / MARKER_ROTATE_STEP, CurrentRoute.Heading1, CurrentRoute.DeltaAngle);

                        await Task.Delay(TimeSpan.FromMilliseconds(MARKER_ROTATE_TIME_STEP));

                        rotateIndex++;
                    }

                    callback?.Invoke(true);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteError(MethodBase.GetCurrentMethod().Name, ex);
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
                MoveToCurrent();

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

                //DrawToCurrent();

                if (PlayCurrent > 0)
                {
                    PinCar.Rotation = (float)GeoHelper.ComputeHeading(ListRoute[PlayCurrent - 1].Latitude, ListRoute[PlayCurrent - 1].Longitude, CurrentRoute.Latitude, CurrentRoute.Longitude);
                }
                else
                {
                    PinCar.Rotation = (float)GeoHelper.ComputeHeading(CurrentRoute.Latitude, CurrentRoute.Longitude, ListRoute[PlayCurrent + 1].Latitude, ListRoute[PlayCurrent + 1].Longitude);
                }

                PinCar.Position = new Position(CurrentRoute.Latitude, CurrentRoute.Longitude);
                PinPlate.Position = PinCar.Position;

                MoveCameraRequest.MoveCamera(CameraUpdateFactory.NewPosition(PinCar.Position));
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

        private void ThemeChanged()
        {
            ColorMapType = MapType == MapType.Street
                ? (Color)Prism.PrismApplicationBase.Current.Resources["PrimaryColor"]
                : (Color)Prism.PrismApplicationBase.Current.Resources["WhiteColor"];

            BackgroundMapType = MapType == MapType.Street
                ? (Color)Prism.PrismApplicationBase.Current.Resources["WhiteColor"]
                : (Color)Prism.PrismApplicationBase.Current.Resources["PrimaryColor"];
        }



        public void StartTimmerRoute()
        {
            //4 là 800ms
            var duration = 64 / PlaySpeed * 50;
            var countroute = ListRoute.Count;
            Device.StartTimer(TimeSpan.FromMilliseconds(duration), () =>
              {
                  if (ctsRouting.IsCancellationRequested || PlayCurrent - 1 >= countroute)
                  {
                      return false;
                  }
                  else
                  {
                      Stopwatch sw = new Stopwatch();
                      sw.Start();
                      PlayCurrent++;
                      CurrentRoute = ListRoute[PlayCurrent];
                      var durationRotate = duration * 10 / 100;
                      var durationmove = duration * 90 / 100;
                      Stopwatch sw1 = new Stopwatch();
                      sw1.Start();
                      //di chuyển xe
                      PinCar.Rotate(CurrentRoute.Latitude, CurrentRoute.Longitude, () =>
                      {
                          sw1.Stop();
                          Debug.WriteLine(string.Format("Rotate : {0}", sw1.ElapsedMilliseconds));
                          Stopwatch sw2 = new Stopwatch();
                          sw2.Start();
                          PinCar.MarkerAnimation(CurrentRoute.Latitude, CurrentRoute.Longitude, () =>
                          {
                              sw2.Stop();
                              Debug.WriteLine(string.Format("MarkerAnimation : {0}", sw2.ElapsedMilliseconds));
                              sw.Stop();
                              Debug.WriteLine(string.Format("RotateMarkerAnimation : {0}", sw.ElapsedMilliseconds));
                          }, durationmove);
                          //di chuyển biển số xe
                          PinPlate.MarkerAnimation(CurrentRoute.Latitude, CurrentRoute.Longitude, () => { }, durationmove);
                      }, durationRotate);
                  }
                  return true;
              });
        }
        private void TimerRouteStop()
        {
            if (IsPlaying)
            {
                if (ctsRouting != null)
                    Interlocked.Exchange(ref ctsRouting, new CancellationTokenSource()).Cancel();

                IsPlaying = false;
            }
        }
        #endregion PrivateMethod
    }
}