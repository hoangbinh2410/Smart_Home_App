using BA_MobileGPS;
using BA_MobileGPS.Core;
using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Events;
using BA_MobileGPS.Core.Extensions;
using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Core.Models;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.ModelViews;
using BA_MobileGPS.Entities.RealmEntity;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;
using Prism;
using Prism.Events;
using Prism.Ioc;
using Prism.Navigation;
using Prism.Services;
using Realms.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using VMS_MobileGPS.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VMS_MobileGPS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OnlinePage : ContentView, IDestructible, INavigationAware
    {
        #region Contructor
        private readonly IEventAggregator eventAggregator;
        private readonly IGeocodeService geocodeService;
        private readonly IDisplayMessage displayMessage;
        private readonly IPageDialogService pageDialog;

        private readonly IVehicleOnlineService vehicleOnlineService;
        private readonly IRealmBaseService<BoundaryRealm, LandmarkResponse> boundaryRepository;
        private bool viewHasAppeared = false;
        private int pageWidth = 0;
        private bool boxInfoIsShown = false;


        public OnlinePage()
        {
            InitializeComponent();
            googleMap.UiSettings.ZoomControlsEnabled = false;

            geocodeService = PrismApplicationBase.Current.Container.Resolve<IGeocodeService>();
            eventAggregator = PrismApplicationBase.Current.Container.Resolve<IEventAggregator>();
            displayMessage = PrismApplicationBase.Current.Container.Resolve<IDisplayMessage>();
            pageDialog = PrismApplicationBase.Current.Container.Resolve<IPageDialogService>();
            vehicleOnlineService = PrismApplicationBase.Current.Container.Resolve<IVehicleOnlineService>();
            boundaryRepository = PrismApplicationBase.Current.Container.Resolve<IRealmBaseService<BoundaryRealm, LandmarkResponse>>();
            // Initialize the View Model Object
            pageWidth = (int)Application.Current.MainPage.Width;
            vm = (OnlinePageViewModel)BindingContext;

            googleMap.IsUseCluster = true;
            googleMap.IsTrafficEnabled = false;

            googleMap.ClusterOptions.EnableBuckets = true;
            googleMap.ClusterOptions.Algorithm = ClusterAlgorithm.GridBased;

            googleMap.UiSettings.MapToolbarEnabled = false;
            googleMap.UiSettings.ZoomControlsEnabled = false;
            googleMap.UiSettings.MyLocationButtonEnabled = false;
            googleMap.UiSettings.RotateGesturesEnabled = false;

            if (Device.RuntimePlatform == Device.Android)
            {
                googleMap.ClusterOptions.RendererImage = BitmapDescriptorFactory.FromResource("ic_cluster.png");
            }

            googleMap.ClusterClicked += Map_ClusterClicked;
            googleMap.PinClicked += MapOnPinClicked;
            googleMap.MapClicked += Map_MapClicked;
            googleMap.CameraIdled += GoogleMap_CameraIdled;

            mCarActive = new VehicleOnline();
            mCurrentVehicleList = new List<VehicleOnline>();

            this.eventAggregator.GetEvent<ReceiveSendCarEvent>().Subscribe(this.OnReceiveSendCarSignalR);


            IsInitMarker = false;
            entrySearch.Placeholder = MobileResource.Route_Label_SearchFishing;
        }

        #endregion

        #region Lifecycle
        public void OnPageAppearingFirstTime()
        {
            this.eventAggregator.GetEvent<ReceiveSendCarEvent>().Subscribe(this.OnReceiveSendCarSignalR);
            this.eventAggregator.GetEvent<OnReloadVehicleOnline>().Subscribe(OnReLoadVehicleOnlineCarSignalR);
            this.eventAggregator.GetEvent<BackButtonEvent>().Subscribe(AndroidBackButton);
            googleMap.InitialCameraUpdate = CameraUpdateFactory.NewPositionZoom(new Position(MobileUserSettingHelper.LatCurrentScreenMap, MobileUserSettingHelper.LngCurrentScreenMap), MobileUserSettingHelper.Mapzoom);
        }
        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            if (!viewHasAppeared)
            {
                OnPageAppearingFirstTime();

                viewHasAppeared = true;
            }

            if (parameters.ContainsKey(ParameterKey.Vehicle) && parameters.GetValue<Vehicle>(ParameterKey.Vehicle) is Vehicle vehiclePlate)
            {
                if (googleMap.ClusteredPins != null && googleMap.ClusteredPins.Count > 0)
                {
                    var clusterpin = googleMap.ClusteredPins.FirstOrDefault(x => x.Label == vehiclePlate.VehiclePlate);
                    if (clusterpin != null)
                    {
                        var vehicleselect = mCurrentVehicleList.FirstOrDefault(x => x.VehiclePlate == vehiclePlate.VehiclePlate);
                        if (vehicleselect != null)
                        {
                            vm.CarSearch = vehicleselect.PrivateCode;
                            UpdateSelectVehicle(vehicleselect);
                        }
                        else
                        {
                            pageDialog.DisplayAlertAsync(MobileResource.Common_Message_Warning, MobileResource.Online_Message_CarStopService, MobileResource.Common_Label_Close);
                        }
                    }
                    else
                    {
                        displayMessage.ShowMessageInfo(MobileResource.Common_Message_NotFindYourCar);
                    }
                }
                else
                {
                    displayMessage.ShowMessageInfo(MobileResource.Common_Message_NotFindYourCar);
                }
            }
            else if (parameters.ContainsKey(ParameterKey.Company) && parameters.GetValue<Company>(ParameterKey.Company) is Company company)
            {
                vm.CarSearch = string.Empty;

                HideBoxStatus();

                HideBoxInfo();

                UpdateVehicleByCompany(company);
            }
            else if (parameters.ContainsKey(ParameterKey.VehicleGroups) && parameters.GetValue<int[]>(ParameterKey.VehicleGroups) is int[] vehiclegroup)
            {
                vm.CarSearch = string.Empty;

                vm.VehicleGroups = vehiclegroup;

                HideBoxStatus();

                HideBoxInfo();

                UpdateVehicleByVehicleGroup(vehiclegroup);
            }

            GoogleMapAddBoundary();
            GoogleMapAddName();
        }

        public void OnNavigatingTo(INavigationParameters parameters)
        {
        }

        public void Destroy()
        {
            timer.Stop();
            timer.Dispose();
            this.eventAggregator.GetEvent<ReceiveSendCarEvent>().Unsubscribe(OnReceiveSendCarSignalR);

        }

        #endregion

        #region Property

        private OnlinePageViewModel vm;

        private System.Timers.Timer timer;

        private CancellationTokenSource cts;


        /* Xe đang được chọn */
        private VehicleOnline mCarActive;

        /* Danh sách xe online */

        private List<VehicleOnline> mVehicleList
        {
            get
            {
                if (StaticSettings.ListVehilceOnline != null)
                {
                    //nếu khóa BAP rồi thì ko hiển thị trên Map nữa
                    return StaticSettings.ListVehilceOnline.Where(x => x.MessageId != 65 && x.MessageId != 254 && x.MessageId != 128).ToList();
                }
                else
                {
                    return new List<VehicleOnline>();
                }
            }
        }

        private void OnReLoadVehicleOnlineCarSignalR(bool arg)
        {
            if (arg)
            {
                if (mCarActive.VehicleId != -1 && !string.IsNullOrEmpty(mCarActive.VehiclePlate))
                {
                    var vehicleselect = mVehicleList.FirstOrDefault(x => x.VehiclePlate == mCarActive.VehiclePlate);
                    if (vehicleselect != null)
                    {
                        vm.CarSearch = vehicleselect.PrivateCode;
                        UpdateSelectVehicle(vehicleselect, true);
                    }
                }
            }
            else
            {
                IsInitMarker = false;
                InitOnline();
            }
        }

        /* Danh sách xe online */
        private List<VehicleOnline> mCurrentVehicleList;

        private bool infoStatusIsShown = false;
        private bool IsInitMarker = false;

        #endregion Propety

        #region Private Method

        private void GoogleMapAddBoundary()
        {
            vm.Boundaries.Clear();
            vm.Borders.Clear();

            var listBoudary = boundaryRepository.Find(b => b.IsShowBoudary);

            foreach (var item in listBoudary)
            {
                AddBoundary(item);
            }
        }

        private void AndroidBackButton(bool obj)
        {
            vm.CarSearch = string.Empty;
            if (mCarActive != null && mCarActive.VehicleId > 0)
            {
                HideBoxInfoCarActive(mCarActive);
            }
            else
            {
                HideBoxStatus();
            }
        }

        private void UpdateSelectVehicle(VehicleOnline vehicle, bool isReloadVehicle = false)
        {
            if (vehicle != null)
            {
                try
                {
                    if (vehicle.VehicleId != mCarActive.VehicleId || isReloadVehicle)
                    {
                        ShowBoxInfoCarActive(vehicle, vehicle.MessageId, vehicle.DataExt);

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            var lstpin = googleMap.ClusteredPins.Where(x => x.Label == vehicle.VehiclePlate).ToList();
                            if (lstpin != null && lstpin.Count > 1)
                            {
                                googleMap.AnimateCamera(CameraUpdateFactory.NewPositionZoom(new Position(lstpin[0].Position.Latitude, lstpin[0].Position.Longitude), MobileSettingHelper.ClusterMapzoom));
                            }
                        });
                    }
                }
                catch (Exception ex)
                {
                    LoggerHelper.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
                }
            }
        }

        private void AddBoundary(LandmarkResponse boundary)
        {
            try
            {

                var result = boundary.Polygon.Split(',');

                var color = Color.FromHex(ConvertIntToHex(boundary.Color));

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

                    polygon.Clicked += Polygon_Clicked;
                    vm.Boundaries.Add(polygon);
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

                    vm.Borders.Add(polyline);
                }


            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }

        }

        public static string ConvertIntToHex(int value)
        {
            return value.ToString("X").PadLeft(6, '0');
        }

        private void Polygon_Clicked(object sender, EventArgs e)
        {
            HideBoxInfo();
        }

        private void GoogleMapAddName()
        {
            try
            {
                var listName = boundaryRepository.Find(b => b.IsShowName);

                foreach (var pin in googleMap.Pins.Where(p => p.Tag.ToString().Contains("Boundary")).ToList())
                {
                    googleMap.Pins.Remove(pin);
                }

                foreach (var item in listName)
                {
                    AddName(item);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }

        }

        private void AddName(LandmarkResponse name)
        {
            try
            {
                googleMap.Pins.Add(new Pin
                {
                    Label = name.Name,
                    Position = new Position(name.Latitude, name.Longitude),
                    Icon = BitmapDescriptorFactory.FromView(new BoundaryNameInfoWindow(name.Name) { WidthRequest = name.Name.Length < 20 ? 6 * name.Name.Length : 110, HeightRequest = 18 * ((name.Name.Length / 20) + 1) }),
                    Tag = "Boundary" + name.Name
                });
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
        }



        private void StartTimmerCaculatorStatus()
        {
            timer = new System.Timers.Timer
            {
                Interval = 15000
            };
            timer.Elapsed += T_Elapsed;

            timer.Start();
        }

        private void T_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (infoStatusIsShown)
            {
                CacularVehicleStatus();
            }
        }


        /// <summary>
        /// Nhận dữ liệu xe online
        /// </summary>
        /// <param name="e"></param>
        private void OnReceiveSendCarSignalR(VehicleOnline carInfo)
        {
            var lstpin = googleMap.ClusteredPins.Where(x => x.Label == carInfo.VehiclePlate).ToList();
            if (lstpin != null && lstpin.Count > 1)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (mCarActive.VehicleId != -1 && carInfo.VehiclePlate.Equals(mCarActive.VehiclePlate))
                    {
                        UpdateVehicle(carInfo, lstpin[0], lstpin[1], true);
                    }
                    else
                    {
                        UpdateVehicle(carInfo, lstpin[0], lstpin[1]);
                    }
                });
            }
        }

        private void UpdateSelectVehicle(VehicleOnline vehicle)
        {
            if (vehicle != null)
            {
                try
                {
                    if (vehicle.VehiclePlate != mCarActive.VehiclePlate)
                    {
                        ShowBoxInfoCarActive(vehicle, vehicle.MessageId, vehicle.DataExt);

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            var lstpin = googleMap.ClusteredPins.Where(x => x.Label == vehicle.VehiclePlate).ToList();
                            if (lstpin != null && lstpin.Count > 1)
                            {
                                googleMap.AnimateCamera(CameraUpdateFactory.NewPositionZoom(new Position(lstpin[0].Position.Latitude, lstpin[0].Position.Longitude), 17));
                            }
                        });
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
                }
            }
        }

        public void UpdateVehicleByCompany(Company company)
        {
            using (new HUDService())
            {
                //Title = company.CompanyName;

                if (mVehicleList != null && mVehicleList.Count > 0)
                {
                    var listPin = ConvertMarkerPin(mVehicleList);

                    InitPinVehicle(listPin);

                    // Chạy lại hàm tính toán trạng thái xe
                    InitVehicleStatus(mVehicleList);
                }
                else
                {
                    googleMap.Pins.Clear();
                }
            }
        }

        public void UpdateVehicleByVehicleGroup(int[] vehiclegroup)
        {
            List<VehicleOnline> listResult;

            if (vehiclegroup.Length > 0)
            {
                listResult = mVehicleList.FindAll(v => v.GroupIDs.Split(',').ToList().Exists(g => vehiclegroup.Contains(Convert.ToInt32(g))));
            }
            else
            {
                listResult = mVehicleList;
            }

            if (listResult.Count > 0)
            {
                InitPinVehicle(ConvertMarkerPin(listResult));
            }
            else
            {
                googleMap.Pins.Clear();
            }

            // Chạy lại hàm tính toán trạng thái xe
            InitVehicleStatus(listResult);
        }

        private List<VMSVehicleOnlineMarker> ConvertMarkerPin(List<VehicleOnline> lisVehicle)
        {
            var listmarker = new List<VMSVehicleOnlineMarker>();

            lisVehicle.ForEach(x =>
            {
                listmarker.Add(new VMSVehicleOnlineMarker()
                {
                    VehicleId = x.VehicleId,
                    VehiclePlate = x.VehiclePlate,
                    Lat = x.Lat,
                    Lng = x.Lng,
                    State = x.State,
                    Velocity = x.Velocity,
                    GPSTime = x.GPSTime,
                    VehicleTime = x.VehicleTime,
                    IconCode = x.IconCode,
                    PrivateCode = x.PrivateCode,
                    IconImage = IconCodeHelper.GetMarkerResource(x),
                    DoubleMarker = new VMSDoubleMarker().DrawMarker(x),
                });
            });

            return listmarker;
        }

        private void UpdateVehicle(VehicleOnline carInfo, Pin item, Pin itemLable, bool carActive = false)
        {
            try
            {
                if (item == null || itemLable == null || mVehicleList == null || carInfo.MessageId == 128)
                {
                    return;
                }

                if (carActive)
                {
                    vm.CarActive = carInfo;
                }

                carInfo.IconImage = IconCodeHelper.GetMarkerResource(carInfo);
                item.Icon = BitmapDescriptorFactory.FromResource(carInfo.IconImage);

                // Các xe đang off hoặc khoảng cách quá ngắn thì bỏ qua và nằm trong màn hình
                if (StateVehicleExtension.IsStopAndEngineOff(carInfo)
                        && GeoHelper.IsBetweenLatlng(item.Position.Latitude, item.Position.Longitude, carInfo.Lat, carInfo.Lng))
                {
                    return;
                }

                //nếu xe nằm trong màn hình thì mới animation xoay và di chuyển
                if (IsInMapScreen(new Position(carInfo.Lat, carInfo.Lng)))
                {
                    //di chuyển xe
                    item.Rotate(carInfo.Lat, carInfo.Lng, () =>
                    {
                        item.MarkerAnimation(itemLable, carInfo.Lat, carInfo.Lng, () =>
                         {
                             if (carActive)
                             {
                                 Getaddress(carInfo.Lat.ToString(), carInfo.Lng.ToString(), carInfo.VehicleId);
                             }
                         });
                        //di chuyển biển số xe
                        //itemLable.MarkerAnimation(carInfo.Lat, carInfo.Lng, () => { });
                    });
                }
                else
                {
                    item.Rotation = carInfo.Direction * 45;
                    itemLable.Position = new Position(carInfo.Lat, carInfo.Lng);
                    item.Position = new Position(carInfo.Lat, carInfo.Lng);
                    if (carActive)
                    {
                        googleMap.AnimateCamera(CameraUpdateFactory.NewPosition(new Position(carInfo.Lat, carInfo.Lng)));
                        Getaddress(carInfo.Lat.ToString(), carInfo.Lng.ToString(), carInfo.VehicleId);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        /// <summary>
        /// Lấy thông tin danh sách xe online
        /// </summary>
        private void InitOnline()
        {
            try
            {
                if (StaticSettings.ListVehilceOnline != null && StaticSettings.ListVehilceOnline.Count > 0)
                {
                    if (!IsInitMarker)
                    {
                        using (new HUDService(MobileResource.Common_Message_Processing))
                        {
                            vm.VehicleGroups = null;
                            HideBoxInfoCarActive(new VehicleOnline() { VehicleId = 1 });
                            var list = StaticSettings.ListVehilceOnline.Where(x => x.MessageId != 65 && x.MessageId != 254 && x.MessageId != 128).ToList();
                            if (list != null && list.Count > 0)
                            {
                                //Nếu là công ty thường thì mặc định load xe của công ty lên bản đồ
                                if (!UserHelper.isCompanyPartner(StaticSettings.User))
                                {
                                    InitVehicleStatus(list);

                                    var listPin = ConvertMarkerPin(list);

                                    //Vẽ xe lên bản đồ
                                    InitPinVehicle(listPin);
                                }
                                else
                                {
                                    //nếu trước đó đã chọn 1 công ty nào đó rồi thì load danh sách xe của công ty đó
                                    if (Settings.CurrentCompany != null && Settings.CurrentCompany.FK_CompanyID > 0)
                                    {
                                        UpdateVehicleByCompany(Settings.CurrentCompany);
                                    }
                                    else
                                    {
                                        displayMessage.ShowMessageInfo(MobileResource.Common_Message_SelectCompany);
                                    }
                                }
                            }
                            InitialCameraUpdate();
                            StartTimmerCaculatorStatus();
                        }
                    }
                }
                else
                {
                    GetListVehicleOnline();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
        }

        private void Getaddress(string lat, string lng, long vehicleID)
        {
            try
            {
                vm.CurrentAddress = MobileResource.Online_Label_Determining;
                Task.Run(async () =>
                {
                    return await geocodeService.GetAddressByLatLng(lat, lng);
                }).ContinueWith(task => Device.BeginInvokeOnMainThread(() =>
                {
                    if (task.Status == TaskStatus.RanToCompletion)
                    {
                        if (!string.IsNullOrEmpty(task.Result))
                        {
                            if (vm.CarActive.VehicleId == vehicleID)
                            {
                                vm.CurrentAddress = task.Result;
                                vm.CarActive.CurrentAddress = task.Result;
                            }
                        }
                    }
                    else if (task.IsFaulted)
                    {
                        Logger.WriteError(MethodBase.GetCurrentMethod().Name, "Error");
                    }
                }));
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
        }

        private void GetListVehicleOnline()
        {
            var userID = StaticSettings.User.UserId;
            var companyID = StaticSettings.User.CompanyId;
            var xnCode = StaticSettings.User.XNCode;
            var userType = StaticSettings.User.UserType;
            var companyType = StaticSettings.User.CompanyType;

            if (Settings.CurrentCompany != null && Settings.CurrentCompany.FK_CompanyID > 0)
            {
                userID = Settings.CurrentCompany.UserId;
                companyID = Settings.CurrentCompany.FK_CompanyID;
                xnCode = Settings.CurrentCompany.XNCode;
                userType = Settings.CurrentCompany.UserType;
                companyType = Settings.CurrentCompany.CompanyType;
            }
            int vehicleGroup = 0;

            if (cts != null)
                cts.Cancel(true);

            cts = new CancellationTokenSource();

            Task.Run(async () =>
            {
                await Task.Delay(500, cts.Token);

                return await vehicleOnlineService.GetListVehicleOnline(userID, vehicleGroup, companyID, xnCode, userType, companyType);
            }, cts.Token).ContinueWith(task => Device.BeginInvokeOnMainThread(() =>
            {
                if (task.Status == TaskStatus.RanToCompletion)
                {
                    if (task.Result != null && task.Result.Count > 0)
                    {
                        task.Result.ForEach(x =>
                        {
                            x.IconImage = IconCodeHelper.GetMarkerResource(x);
                            x.StatusEngineer = StateVehicleExtension.EngineState(x);

                            if (!StateVehicleExtension.IsLostGPS(x.GPSTime, x.VehicleTime) && !StateVehicleExtension.IsLostGSM(x.VehicleTime))
                            {
                                x.SortOrder = 1;
                            }
                            else
                            {
                                x.SortOrder = 0;
                            }
                        });

                        StaticSettings.ListVehilceOnline = task.Result;
                        InitialCameraUpdate();
                        InitOnline();
                    }
                    else
                    {
                        StaticSettings.ListVehilceOnline = new List<VehicleOnline>();
                    }
                }
            }));
        }

        private void InitialCameraUpdate()
        {
            if (StaticSettings.ListVehilceOnline != null && StaticSettings.ListVehilceOnline.Count > 0)
            {
                if (StaticSettings.ListVehilceOnline.Count > 1)
                {
                    var listPositon = new List<Position>();
                    StaticSettings.ListVehilceOnline.ForEach(x =>
                    {
                        listPositon.Add(new Position(x.Lat, x.Lng));
                    });
                    var bounds = GeoHelper.FromPositions(listPositon);
                    googleMap.AnimateCamera(CameraUpdateFactory.NewBounds(bounds, 60));
                }
                else
                {
                    googleMap.AnimateCamera(CameraUpdateFactory.NewPositionZoom(new Position(StaticSettings.ListVehilceOnline[0].Lat, StaticSettings.ListVehilceOnline[0].Lng), MobileUserSettingHelper.Mapzoom));
                }
            }
            else
            {
                googleMap.AnimateCamera(CameraUpdateFactory.NewPositionZoom(new Position(MobileUserSettingHelper.LatCurrentScreenMap, MobileUserSettingHelper.LngCurrentScreenMap), MobileUserSettingHelper.Mapzoom));
            }
        }

        /// <summary>
        /// khởi tạo danh sách trạng thái xe
        /// </summary>
        /// <param name="vehicleList"></param>
        private void InitVehicleStatus(List<VehicleOnline> vehicleList)
        {
            //txtCountVehicle.Text = vehicleList.Count().ToString();
            vm.VehicleStatusSelected = VehicleStatusGroup.All;
            mCurrentVehicleList = vehicleList;
            // Lấy trạng thái xe
            List<VehicleStatusViewModel> listStatus = (new VehicleStatusHelper()).DictVehicleStatus.Values.Where(x => x.IsEnable).ToList();
            if (listStatus != null && listStatus.Count > 0)
            {
                listStatus.ForEach(x =>
                {
                    x.CountCar = StateVehicleExtension.GetCountCarByStatus(vehicleList, (VehicleStatusGroup)x.ID);
                });

                lvStatusCar.ItemsSource = listStatus;
            }
        }

        /// <summary>
        /// Khởi tạo xe trên mapp
        /// </summary>
        /// <param name="lstVehicle"></param>
        private void InitPinVehicle(List<VMSVehicleOnlineMarker> lstVehicle)
        {
            try
            {
                IsInitMarker = true;
                googleMap.ClusteredPins.Clear();

                for (int i = 0; i < lstVehicle.Count; i++)
                {
                    RenderMarkerClusterOnMap(lstVehicle[i].DoubleMarker);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
            finally
            {
                googleMap.Cluster();
            }
        }

        /// <summary>
        /// Vẽ xe lên map không có cluster
        /// </summary>
        /// <param name="carinfo"></param>
        private void RenderMarkerClusterOnMap(VMSDoubleMarker carinfo)
        {
            googleMap.ClusteredPins.Add(carinfo.Car);
            googleMap.ClusteredPins.Add(carinfo.Plate);
        }

        /// <summary>
        /// Vẽ lại icon biển số xe
        /// </summary>
        /// <param name="vehicle"></param>
        /// <param name="bacground"></param>
        private void UpdateBackgroundPinLable(VehicleOnline carinfo, bool isActive = false)
        {
            var lstpin = googleMap.ClusteredPins.Where(x => x.Label == carinfo.VehiclePlate).ToList();
            if (lstpin != null && lstpin.Count > 1)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (isActive)
                    {
                        lstpin[1].Tag = carinfo.VehiclePlate + "Active";
                        lstpin[1].Icon = BitmapDescriptorFactory.FromView(new VMSPinInfowindowActiveView(carinfo.PrivateCode));
                    }
                    else
                    {
                        lstpin[1].Tag = carinfo.VehiclePlate + "Plate";

                        lstpin[1].Icon = BitmapDescriptorFactory.FromView(new VMSPinInfowindowView(carinfo.PrivateCode));
                    }
                });
            }
        }

        private void ShowBoxInfoCarActive(VehicleOnline carInfo, int messageId, int dataExt)
        {
            //nếu messageId==128 thì là xe dừng dịch vụ
            if (messageId == 128)
            {
                pageDialog.DisplayAlertAsync(MobileResource.Common_Message_Warning, MobileResource.Online_Message_CarStopService, MobileResource.Common_Label_Close);

                return;
            }

            //Nếu messageId=2 hoặc 3 là xe phải thu phí
            if (!StateVehicleExtension.IsVehicleDebtMoney(messageId, dataExt))
            {
                //nếu đang có xe active thì xóa active xe ý đi
                if (mCarActive != null && mCarActive.VehicleId > 0)
                {
                    UpdateBackgroundPinLable(mCarActive);
                }

                mCarActive = carInfo;
                vm.CarActive = carInfo;
                Getaddress(carInfo.Lat.ToString(), carInfo.Lng.ToString(), carInfo.VehicleId);

                if (vm.Circles.Count > 0)
                {
                    vm.ShowBorder();
                }
                else
                {
                    vm.HideBorder();
                }

                //update active xe mới
                UpdateBackgroundPinLable(carInfo, true);

                ShowBoxInfo();
            }
            else
            {
                if (!string.IsNullOrEmpty(carInfo.MessageDetailBAP))
                {
                    pageDialog.DisplayAlertAsync(MobileResource.Common_Message_Warning, carInfo.MessageDetailBAP, MobileResource.Common_Label_Close);
                }
                else
                {
                    pageDialog.DisplayAlertAsync(MobileResource.Common_Message_Warning, MobileResource.Online_Message_CarDebtMoney, MobileResource.Common_Label_Close);
                }
            }
        }

        /// <summary>
        /// ẩn thông tin xe đi và remove active xe
        /// </summary>
        /// <param name="carinfo"></param>
        private void HideBoxInfoCarActive(VehicleOnline carinfo)
        {
            vm.CarSearch = string.Empty;
            HideBoxStatus(); // ẩn tạm chưa có box trạng thái

            HideBoxInfo();

            if (carinfo.VehicleId > 0)
            {
                UpdateBackgroundPinLable(carinfo);
            }

            vm.CarActive = new VehicleOnline();
            mCarActive = new VehicleOnline();
        }

        /// <summary>
        /// sự kiện khi di chuyển map
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoogleMap_CameraIdled(object sender, CameraIdledEventArgs e)
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                googleMap.Cluster();
            }
        }

        /// <summary>
        /// Sự kiện khi click vào map
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Map_MapClicked(object sender, MapClickedEventArgs e)
        {
            try
            {
                vm.CarSearch = string.Empty;
                if (mCarActive != null && mCarActive.VehicleId > 0)
                {
                    HideBoxInfoCarActive(mCarActive);
                }
                else
                {
                    HideBoxStatus();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
        }

        /// <summary>
        /// Sự kiện khi click vào Cluster
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Map_ClusterClicked(object sender, ClusterClickedEventArgs e)
        {
            e.Handled = true;
            if (e.Pins != null && e.Pins.Count() > 0)
            {
                var listPositon = new List<Position>();
                e.Pins.ToList().ForEach(x =>
                {
                    listPositon.Add(x.Position);
                });
                var bounds = GeoHelper.FromPositions(listPositon);

                googleMap.AnimateCamera(CameraUpdateFactory.NewBounds(bounds, 40));
            }
        }

        /// <summary>
        /// Sự kiện khi click vào Pin ko cluster
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void MapOnPinClicked(object sender, PinClickedEventArgs args)
        {
            args.Handled = true;

            try
            {
                if (args.Pin != null && args.Pin.Label != mCarActive.VehiclePlate)
                {
                    var car = mVehicleList.FirstOrDefault(x => x.VehiclePlate == args.Pin.Label);
                    if (car != null)
                    {
                        vm.CarSearch = car.PrivateCode;
                        ShowBoxInfoCarActive(car, car.MessageId, car.DataExt);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
        }

        /// <summary>
        /// ẩn box  thông tin  xe
        /// </summary>
        public void HideBoxInfo()
        {
            try
            {
                vm.CarActive = new VehicleOnline();
                mCarActive = new VehicleOnline();
                SetNoPaddingWithFooter();
                eventAggregator.GetEvent<ShowHideTabEvent>().Publish(true);
                if (boxInfoIsShown)
                {
                    Action<double> callback = input => boxInfo.TranslationY = input;
                    boxInfo.Animate("animBoxInfo", callback, 0, 300, 16, 300, Easing.CubicInOut);
                    boxInfoIsShown = false;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
        }

        /// <summary>
        /// Hiển thị box thông tin xe
        /// </summary>
        private void ShowBoxInfo()
        {
            try
            {
                SetPaddingWithFooter();
                Device.BeginInvokeOnMainThread(() =>
                {
                    eventAggregator.GetEvent<ShowHideTabEvent>().Publish(false);
                });
                if (!boxInfoIsShown)
                {
                    Action<double> callback = input => boxInfo.TranslationY = input;
                    boxInfo.Animate("animBoxInfo", callback, 300, 0, 16, 300, Easing.CubicInOut);
                    boxInfoIsShown = true;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
        }

        /* Set padding map khi có thông tin xe ở footer - tracking */

        public void SetPaddingWithFooter()
        {
            double paddingMap = boxInfo.HeightRequest;
            googleMap.Padding = new Thickness(0, 0, 0, (int)paddingMap);
            BoxControls.Margin = new Thickness(20, 0, 20, (int)paddingMap + 35);
        }

        /* Set padding map khi có thông tin xe ở footer - tracking */

        public void SetNoPaddingWithFooter()
        {
            googleMap.Padding = new Thickness(0, 0, 0, 0);
            BoxControls.Margin = new Thickness(20);
        }

        private void HideBoxStatus()
        {
            try
            {
                if (infoStatusIsShown)
                {
                    Action<double> callback = input => boxStatusVehicle.TranslationX = input;
                    boxStatusVehicle.Animate("animboxStatusVehicle", callback, 0, pageWidth, 16, 300, Easing.CubicInOut);
                    infoStatusIsShown = false;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteError("HideBoxStatus", ex);
            }
        }

        private async void ShowBoxStatus()
        {
            if (!infoStatusIsShown)
            {
                Action<double> callback = input => boxStatusVehicle.TranslationX = input;
                boxStatusVehicle.Animate("animboxStatusVehicle", callback, pageWidth, 0, 16, 300, Easing.CubicInOut);
                infoStatusIsShown = true;
            }
        }

        private void TapStatusVehicel(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs args)
        {
            HideBoxStatus();

            HideBoxInfo();

            vm.CarSearch = string.Empty;

            if ((args as Syncfusion.ListView.XForms.ItemTappedEventArgs).ItemData is VehicleStatusViewModel item)
            {
                Device.StartTimer(TimeSpan.FromMilliseconds(300), () =>
                {
                    var listFilter = StateVehicleExtension.GetVehicleCarByStatus(mCurrentVehicleList, (VehicleStatusGroup)item.ID);
                    if (listFilter != null)
                    {
                        var listPin = ConvertMarkerPin(listFilter);

                        //Vẽ xe lên bản đồ
                        InitPinVehicle(listPin);

                        if (listPin.Count > 0)
                        {
                            var listPositon = new List<Position>();
                            listPin.ForEach(x =>
                            {
                                listPositon.Add(new Position(x.Lat, x.Lng));
                            });
                            var bounds = GeoHelper.FromPositions(listPositon);

                            googleMap.AnimateCamera(CameraUpdateFactory.NewBounds(bounds, 40));
                        }
                    }
                    return false;
                });
            }
        }

        private void FilterStatusCar(object sender, EventArgs e)
        {
            if (infoStatusIsShown)
            {
                HideBoxStatus();
            }
            else
            {
                CacularVehicleStatus();
                ShowBoxStatus();
            }
        }

        private bool IsInMapScreen(Position latlng)
        {
            var result = false;
            //nhỏ hơn góc trái ở trên và lớn hơn góc phải ở dưới
            if ((latlng.Latitude <= googleMap.Region.FarLeft.Latitude && latlng.Longitude >= googleMap.Region.FarLeft.Longitude) &&
                (latlng.Latitude >= googleMap.Region.NearRight.Latitude && latlng.Longitude <= googleMap.Region.NearRight.Longitude))
                result = true;

            return result;
        }

        private void TappedHidenBoxStatus(object sender, EventArgs e)
        {
            HideBoxStatus();
        }

        private void SwipeGestureBoxStatus(object sender, SwipedEventArgs e)
        {
            switch (e.Direction)
            {
                case SwipeDirection.Right:
                    HideBoxStatus();
                    break;

                case SwipeDirection.Left:
                    break;

                case SwipeDirection.Up:
                    break;

                case SwipeDirection.Down:
                    break;
            }
        }

        private void CacularVehicleStatus()
        {
            if (lvStatusCar.ItemsSource != null && ((List<VehicleStatusViewModel>)(lvStatusCar.ItemsSource)).Count > 0)
            {
                ((List<VehicleStatusViewModel>)(lvStatusCar.ItemsSource)).ForEach(x =>
                {
                    x.CountCar = StateVehicleExtension.GetCountCarByStatus(mCurrentVehicleList, (VehicleStatusGroup)x.ID);
                });
            }
        }

        //private void SelectMenuFAB(object sender, XViewEventArgs e)
        //{
        //    vm.PushToFABPageCommand.Execute(e.EventIndex);
        //}

        public async void GetMylocation(object sender, EventArgs e)
        {
            try
            {
                var mylocation = await LocationHelper.GetGpsLocation();

                if (mylocation != null)
                {
                    if (!googleMap.MyLocationEnabled)
                    {
                        googleMap.MyLocationEnabled = true;
                    }
                    await googleMap.AnimateCamera(CameraUpdateFactory.NewPosition(new Position(mylocation.Latitude, mylocation.Longitude)), TimeSpan.FromMilliseconds(300));
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
        }

        #endregion Private Method
    }
}