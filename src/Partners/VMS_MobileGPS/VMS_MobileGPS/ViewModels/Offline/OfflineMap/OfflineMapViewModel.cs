using BA_MobileGPS.Core;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.RealmEntity;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Syncfusion.SfMaps.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Timers;
using System.Windows.Input;
using VMS_MobileGPS.Constant;
using VMS_MobileGPS.Events;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Extensions;
using MapMarker = Syncfusion.SfMaps.XForms.MapMarker;
using Point = Xamarin.Forms.Point;

namespace VMS_MobileGPS.ViewModels
{
    public class OfflineMapViewModel : VMSBaseViewModel
    {
        private Timer timer;
        private readonly IVehicleOnlineService _vehicleOnlineService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IRealmBaseService<BoundaryRealm, LandmarkResponse> _boundaryRepository;

        public OfflineMapViewModel(INavigationService navigationService,
            IEventAggregator ea,
            IRealmBaseService<BoundaryRealm, LandmarkResponse> boundaryRepository,
            IVehicleOnlineService vehicleOnlineService) : base(navigationService)
        {
            _vehicleOnlineService = vehicleOnlineService;
            _boundaryRepository = boundaryRepository;
            _eventAggregator = ea;
            _eventAggregator.GetEvent<RecieveLocationEvent>().Subscribe(this.RecieveLocationEventSuccess);
            if (!(GlobalResourcesVMS.Current.DeviceManager.State == Service.BleConnectionState.NO_CONNECTION))
            {
                StartTimmerSendLocation();
            }

            _locationMoved = new Point(16.32080478, 110.54221004);

            Speed = 0;
            _dateTimeN = DateTime.Now;

            CallSOSCommand = new DelegateCommand(CallSOS);
            OpenSettingCommand = new DelegateCommand(OpenSetting);
            SwitchToMyLocationCommand = new DelegateCommand(SwitchToMyLocation);
            ZoomInCommand = new DelegateCommand(ZoomIn);
            ZoomOutCommand = new DelegateCommand(ZoomOut);

            _markers = new ObservableCollection<MapMarker>();

            _osmMapLayers = new ObservableCollection<ShapeFileLayer>();
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            ZoomSupprt();
            GetListLandmark();

            MarkerPosition = "Cần quyền truy cập vị trí hoặc kết nối thiết bị";
        }

        public override void OnPageAppearingFirstTime()
        {
            base.OnPageAppearingFirstTime();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            BluetoothStatus = GlobalResourcesVMS.Current.DeviceManager.State == Service.BleConnectionState.NO_CONNECTION ? "CHƯA KẾT NỐI" : "ĐÃ KẾT NỐI";
            if (Settings.IsLoadedMap)
            {
                ShinyManage();
            }

            BugSupport();
        }

        private void InitMarker()
        {
            TryExecute(async () =>
            {
                if (GlobalResourcesVMS.Current.DeviceManager.State == Service.BleConnectionState.NO_CONNECTION)
                {
                    await PermissionHelper.CheckLocationAsync(async () =>
                    {
                        var location = await Geolocation.GetLastKnownLocationAsync();

                        if (location != null)
                        {
                            MyLocation = new Shiny.Position(location.Latitude, location.Longitude);
                            if (Markers.Count == 0)
                            {
                                Markers.Add(new CustomMarker() { Latitude = location.Latitude.ToString(), Longitude = location.Longitude.ToString() });
                            }
                            else
                            {
                                Markers[0].Latitude = location.Latitude.ToString();
                                Markers[0].Longitude = location.Longitude.ToString();
                            }
                            ConfigDetail();
                        }
                    });
                }
            });
        }

        public override void OnResume()
        {
            base.OnResume();
            if (Settings.IsLoadedMap)
            {
                InitMarker();
                // ShinyManage();
            }
            BugSupport();
        }

        //Colelction lưu location từ shinny
        public Shiny.Position MyLocation { get; set; }

        //HÀM NHẬN location
        private void RecieveLocationEventSuccess(RecieveLocation obj)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (Markers.Count > 0 && MyLocation != null)
                {
                    // tinh van toc trong 60s gan nhat
                    var lastDistance = CalculateDistanceHelper.CalculateDistance(obj.Lng, obj.Lat, MyLocation.Longitude, MyLocation.Latitude);
                    if (obj.Velocity == 0)
                    {
                        Speed = Convert.ToInt32(CalculateDistanceHelper.ConvertDistanceMToKm(lastDistance, 1) * 72);
                    }
                    else
                        Speed = obj.Velocity;

                    DateTimeN = obj.GPSTime;
                    MyLocation = new Shiny.Position(obj.Lat, obj.Lng);

                    var temp = Markers.First();

                    var startLat = Convert.ToDouble(temp.Latitude.Replace(",", "."), CultureInfo.InvariantCulture);
                    var startLng = Convert.ToDouble(temp.Longitude.Replace(",", "."), CultureInfo.InvariantCulture);
                    var lastRotation = (temp as CustomMarker).Rotation;
                    Rotate(startLat, startLng, obj.Lat, obj.Lng,
                        lastRotation, () =>
                        {
                            MarkerAnimation(startLat, startLng, obj.Lat, obj.Lng, () =>
                            {
                            });
                        });
                }
                else
                {
                    MyLocation = new Shiny.Position(obj.Lat, obj.Lng);
                    if (Markers.Count == 0)
                    {
                        Markers.Add(new CustomMarker() { Latitude = obj.Lat.ToString(), Longitude = obj.Lng.ToString() });
                    }
                    else
                    {
                        Markers[0].Latitude = obj.Lat.ToString();
                        Markers[0].Longitude = obj.Lng.ToString();
                    }
                }
                ConfigDetail();
            });
        }

        private void ConfigDetail()
        {
            if (Markers != null && Markers.Count > 0)
            {
                var lat = Convert.ToDouble(Markers[0].Latitude.Replace(",", "."), CultureInfo.InvariantCulture);
                var lng = Convert.ToDouble(Markers[0].Longitude.Replace(",", "."), CultureInfo.InvariantCulture);
                MarkerPosition = string.Join(", ", GeoHelper.LatitudeToDergeeMinSec(lat), GeoHelper.LongitudeToDergeeMinSec(lng));
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            timer.Stop();
            timer.Dispose();
            _eventAggregator.GetEvent<RecieveLocationEvent>().Unsubscribe(this.RecieveLocationEventSuccess);
        }

        public ICommand CallSOSCommand { get; }
        public ICommand OpenSettingCommand { get; }
        public ICommand SwitchToMyLocationCommand { get; }
        public ICommand ZoomInCommand { get; }
        public ICommand ZoomOutCommand { get; }

        private ObservableCollection<ShapeFileLayer> _osmMapLayers;

        public ObservableCollection<ShapeFileLayer> OsmMapLayers
        {
            get { return _osmMapLayers; }
            set { SetProperty(ref _osmMapLayers, value); }
        }

        private Point _locationMoved;

        public Point LocationMoved
        {
            get { return _locationMoved; }
            set
            {
                SetProperty(ref _locationMoved, value);
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<MapMarker> _markers;

        public ObservableCollection<MapMarker> Markers
        {
            get { return _markers; }
            set
            {
                SetProperty(ref _markers, value);
                RaisePropertyChanged();
            }
        }

        private DateTime _dateTimeN;

        public DateTime DateTimeN
        {
            get { return _dateTimeN; }
            set
            {
                SetProperty(ref _dateTimeN, value);
                RaisePropertyChanged();
            }
        }

        private string _markerPosition;

        public string MarkerPosition
        {
            get { return _markerPosition; }
            set
            {
                SetProperty(ref _markerPosition, value);
                RaisePropertyChanged();
            }
        }

        private int _speed;

        public int Speed
        {
            get { return _speed; }
            set
            {
                SetProperty(ref _speed, value);
                RaisePropertyChanged();
            }
        }

        private string _bluetoothStatus;

        public string BluetoothStatus
        {
            get { return _bluetoothStatus; }
            set
            {
                SetProperty(ref _bluetoothStatus, value);
                RaisePropertyChanged();
            }
        }

        private void AddBoundary(IList<LandmarkResponse> list)
        {
            var listBoudary = _boundaryRepository.All().ToList();
            for (int i = 0; i < listBoudary.Count; i++)
            {
                var pointstring = listBoudary[i].Polygon.Split(',');
                var points = new List<Point>();
                for (int j = 0; j < pointstring.Length; j += 2)
                {
                    points.Add(new Point(FormatHelper.ConvertToDouble(pointstring[j + 1], 6), FormatHelper.ConvertToDouble(pointstring[j], 6)));
                }
                var shapeLayerSetting = new ShapeSetting
                {
                    ShapeFill = Color.FromHex("#80CC0000"),
                    ShapeStrokeThickness = 1
                };
                var layer = new ShapeFileLayer
                {
                    ShapeType = ShapeType.Polyline,
                    Points = points.ToObservableCollection(),
                    ShapeSettings = shapeLayerSetting
                };
                OsmMapLayers.Add(layer);
            }
        }

        private async void CallSOS()
        {
            if (GlobalResourcesVMS.Current.DeviceManager.State == Service.BleConnectionState.NO_CONNECTION)
            {
                if (!await PageDialog.DisplayAlertAsync("Cảnh báo", "Bạn chưa kết nối thiết bị. Bạn có muốn kết nối thiết bị?", "ĐỒNG Ý", "BỎ QUA"))
                {
                    return;
                }

                await NavigationService.NavigateAsync(PageNames.BluetoothPage.ToString());
            }
            else
            {
                await NavigationService.NavigateAsync("SOSPage");
            }
        }

        private void OpenSetting()
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("FishQuantityInputPage");
            });
        }

        private void SwitchToMyLocation()
        {
            TryExecute(() =>
            {
                ShinyManage();

                SwitchLocationSupport();
            });
        }

        public void SwitchLocationSupport()
        {
            if (MyLocation != null)
            {
                LocationMoved = new Point(11.125264, 112.219407);
                LocationMoved = new Point(MyLocation.Latitude, MyLocation.Longitude);
            }
        }

        private void BugSupport()
        {
            //RPoints = new ObservableCollection<Point>();
        }

        public void MarkerAnimation(double startlatitude,
           double startlongitude, double endlatitude,
           double endlongitude, Action callback)
        {
            //gán lại vòng quay
            double mMoveIndex = 0;
            double MAX_MOVE_STEP = 40;
            double elapsed = 0;
            double t;
            double v;

            Device.StartTimer(TimeSpan.FromMilliseconds(300), () =>
            {
                // Calculate progress using interpolator
                elapsed = elapsed + 300;
                t = elapsed / 15000;
                v = GeoHelper.GetInterpolation(t);

                var postionnew = GeoHelper.Interpolate(v,
                    new BA_MobileGPS.Core.Position(startlatitude, startlongitude),
                    new BA_MobileGPS.Core.Position(endlatitude, endlongitude));

                mMoveIndex = mMoveIndex + 1;

                if (Markers != null && Markers.Count > 0)
                {
                    Markers[0].Latitude = postionnew.Latitude.ToString();
                    Markers[0].Longitude = postionnew.Longitude.ToString();
                }

                if (mMoveIndex > MAX_MOVE_STEP)
                {
                    callback();
                    return false;
                }

                return true;
            });
        }

        public void Rotate(double oldlatitude,
    double oldlongitude, double newlatitude,
    double newlongitude, double roatation,
    Action callback)
        {
            //gán lại vòng quay
            var mRotateIndex = 0;

            // * tính góc quay giữa 2 điểm location
            var angle = GeoHelper.ComputeHeading(oldlatitude, oldlongitude, newlatitude, newlongitude);
            if (angle == 0)
            {
                callback();
                return;
            }

            //tính lại độ lệch góc
            var deltaAngle = GeoHelper.GetRotaion(roatation, angle);

            var startRotaion = roatation;

            Device.StartTimer(TimeSpan.FromMilliseconds(Constants.MARKER_ROTATE_TIME_STEP), () =>
            {
                //góc quay tiếp theo
                var fractionAngle = GeoHelper.ComputeRotation(
                                  mRotateIndex / Constants.MAX_ROTATE_STEP,
                                  startRotaion,
                                  deltaAngle);
                mRotateIndex = mRotateIndex + 1;

                if (Markers != null && Markers.Count > 0)
                {
                    (Markers[0] as CustomMarker).Rotation = (float)fractionAngle;
                }

                if (mRotateIndex > Constants.MAX_ROTATE_STEP)
                {
                    callback();
                    return false;
                }

                return true;
            });
        }

        private void StartTimmerSendLocation()
        {
            timer = new Timer
            {
                Interval = 60000
            };
            timer.Elapsed += SendLocation;

            timer.Start();

            SendRequestLocationDevice();
        }

        private void SendLocation(object sender, ElapsedEventArgs e)
        {
            SendRequestLocationDevice();
        }

        private async void SendRequestLocationDevice()
        {
            string str = string.Format("GCFG,{0}", "997");

            Debug.WriteLine(str);

            var ret = await AppManager.BluetoothService.Send(str);
        }

        private void GetListLandmark()
        {
            TryExecute(() =>
            {
                //lấy từ local DB
                var list = _boundaryRepository.All()?.ToList();
                if (list != null && list.Count > 0)
                {
                    AddBoundary(list);
                }
                else
                {
                    RunOnBackground(async () =>
                    {
                        return await _vehicleOnlineService.GetListBoundary();
                    },
                   (respones) =>
                   {
                       if (respones != null && respones.Count > 0)
                       {
                           foreach (var item in respones)
                           {
                               //thêm dữ liệu vào local database với bẳng là LanmarkReal
                               _boundaryRepository.Add(item);
                           }
                           AddBoundary(respones);
                       }
                   });
                }
            });
        }

        private void ZoomSupprt()
        {
            var shapeSetting = new ShapeSetting() { ShapeFill = Color.FromHex("AAD3DF"), ShapeStroke = Color.FromHex("AAD3DF"), ShapeStrokeThickness = 3 };
            var listSp = new List<ObservableCollection<Point>>();
            var v1Points = new ObservableCollection<Point>() {
                    new Point(16.362310,111.888266),
                     new Point(16.335954,112.075158),
                      new Point(16.267414,112.080655),
                       new Point(16.267414,111.877272)
                    };
            listSp.Add(v1Points);
            var v2Points = new ObservableCollection<Point>() {
                    new Point(9.619706, 112.947379),
                     new Point(9.623768, 112.994102),
                      new Point(9.602104, 112.994102),
                       new Point(9.602104, 112.943256)
                    };
            listSp.Add(v2Points);
            var v3Points = new ObservableCollection<Point>() {
                new Point(16.720385, 111.944297),
                     new Point(16.741428, 112.713853),
                      new Point(16.467695, 112.735840),
                       new Point(16.446622, 111.900322)
                    };
            listSp.Add(v3Points);
            var v4Points = new ObservableCollection<Point>() {
                new Point(9.449062, 112.514940),
                     new Point(9.449062, 113.240522),
                      new Point(9.188870, 113.240522),
                       new Point(9.123792, 112.514940)
                    };
            listSp.Add(v4Points);
            var v5Points = new ObservableCollection<Point>() {
                new Point(16.794024, 112.112234),
                     new Point(16.783506, 112.497012),
                    new Point(16.636192, 112.540987),
                      new Point(16.604610, 112.090246)};
            listSp.Add(v5Points);
            var v6Points = new ObservableCollection<Point>()
             {
                        new Point(9.481572, 112.694274),
                     new Point(9.503244, 113.079053),
                      new Point(9.362353, 113.090046),
                       new Point(9.329831, 112.672287)
                    };
            listSp.Add(v6Points);
            var v8Points = new ObservableCollection<Point>() {
                          new Point(16.951724, 112.108495),
                     new Point(16.914939, 112.564732),
                      new Point(16.657244, 112.526254),
                       new Point(16.667769, 112.092005)};
            listSp.Add(v8Points);
            var v9Points = new ObservableCollection<Point>() {
                        new Point(9.568251, 112.778445),
                     new Point(9.568251, 112.998318),
                      new Point(9.486990, 112.992821),
                       new Point(9.492408, 112.772948)
                    };
            listSp.Add(v9Points);
            var v10Points = new ObservableCollection<Point>()
            {
                          new Point(16.843976, 112.271230),
                     new Point(16.849234, 112.397657),
                      new Point(16.799282, 112.394909),
                       new Point(16.794024, 112.265733)};
            listSp.Add(v10Points);
            var v11Points = new ObservableCollection<Point>() {
                            new Point(9.557417, 112.820530),
                         new Point(9.554709, 112.952454),
                          new Point(9.511370, 112.955202),
                           new Point(9.508662, 112.820530)
                        };
            listSp.Add(v11Points);

            foreach (var item in listSp)
            {
                OsmMapLayers.Add(new ShapeFileLayer() { Points = item, ShapeType = ShapeType.Polygon, ShapeSettings = shapeSetting });
            }
        }

        private void ShinyManage()
        {
            TryExecute(() =>
            {
                if (GlobalResourcesVMS.Current.DeviceManager.State == Service.BleConnectionState.NO_CONNECTION)
                {
                    _eventAggregator.GetEvent<StartShinnyEvent>().Publish();
                }
                else
                {
                    _eventAggregator.GetEvent<StopShinyEvent>().Publish();
                }
            });
        }

        private void ZoomBugSupport()
        {
            OsmMapLayers[0].Points = new ObservableCollection<Point>();
            OsmMapLayers[0].Points = new ObservableCollection<Point>() {
                    new Point(16.362310,111.888266),
                     new Point(16.335954,112.075158),
                      new Point(16.267414,112.080655),
                       new Point(16.267414,111.877272)
                    };
            if (MyLocation != null)
            {
                Markers[0].Latitude = MyLocation.Latitude.ToString();
                Markers[0].Longitude = MyLocation.Longitude.ToString();
            }
        }

        private void ZoomIn()
        {
            if (GlobalResourcesVMS.Current.OffMapZoomLevel < GlobalResourcesVMS.Current.MaxOffMapZoom)
            {
                SwitchLocationSupport();
                GlobalResourcesVMS.Current.OffMapZoomLevel += 1;
            }
            if (Device.RuntimePlatform == Device.iOS)
            {
                ZoomBugSupport();
            }
        }

        private void ZoomOut()
        {
            if (GlobalResourcesVMS.Current.OffMapZoomLevel > GlobalResourcesVMS.Current.MinOffMapZoom)
            {
                SwitchLocationSupport();
                GlobalResourcesVMS.Current.OffMapZoomLevel -= 1;
            }
            if (Device.RuntimePlatform == Device.iOS)
            {
                ZoomBugSupport();
            }
        }
    }

    public class CustomMarker : MapMarker
    {
        public ImageSource ImageName { get; set; }
        private double _rotation;

        public double Rotation
        {
            get { return _rotation; }
            set
            {
                _rotation = value;
                OnPropertyChanged(nameof(Rotation));
            }
        }

        public CustomMarker()
        {
            ImageName = ImageSource.FromResource("car_blue.png", typeof(CustomMarker).GetTypeInfo().Assembly);
        }
    }
}