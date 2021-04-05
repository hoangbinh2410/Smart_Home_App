using BA_MobileGPS;
using BA_MobileGPS.Core;
using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Extensions;
using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Core.Views;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Timers;
using VMS_MobileGPS.ViewModels;
using Xamarin.Forms;

namespace VMS_MobileGPS.Views
{
    public partial class OnlinePageNoCluster : ContentPage, INavigationAware, IDestructible
    {
        #region Contructor

        private readonly IRealmBaseService<BoundaryRealm, LandmarkResponse> boundaryRepository;
        private readonly IEventAggregator eventAggregator;
        private readonly IGeocodeService geocodeService;
        private readonly IDisplayMessage displayMessage;
        private readonly IPageDialogService pageDialog;

        public OnlinePageNoCluster()
        {
            InitializeComponent();
            eventAggregator = PrismApplicationBase.Current.Container.Resolve<IEventAggregator>();
            geocodeService = PrismApplicationBase.Current.Container.Resolve<IGeocodeService>();
            displayMessage = PrismApplicationBase.Current.Container.Resolve<IDisplayMessage>();
            pageDialog = PrismApplicationBase.Current.Container.Resolve<IPageDialogService>();
            pageWidth = (int)Application.Current.MainPage.Width;
            boxStatusVehicle.TranslationX = pageWidth;
            boxInfo.TranslationY = 300;
            // Initialize the View Model Object
            vm = (OnlinePageViewModel)BindingContext;
            googleMap.IsUseCluster = false;
            googleMap.IsTrafficEnabled = false;
            googleMap.UiSettings.MapToolbarEnabled = false;
            googleMap.UiSettings.ZoomControlsEnabled = false;
            googleMap.UiSettings.MyLocationButtonEnabled = false;
            googleMap.UiSettings.RotateGesturesEnabled = false;
            googleMap.PinClicked += MapOnPinClicked;
            googleMap.MapClicked += Map_MapClicked;
            mCarActive = new VehicleOnline();
            mCurrentVehicleList = new List<VehicleOnline>();
            IsInitMarker = false;
            entrySearch.Placeholder = MobileResource.Route_Label_SearchFishing;
            boundaryRepository = PrismApplicationBase.Current.Container.Resolve<IRealmBaseService<BoundaryRealm, LandmarkResponse>>();
        }

        #endregion Contructor

        #region Lifecycle

        private bool viewHasAppeared = false;

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
                if (googleMap.Pins != null && googleMap.Pins.Count > 0)
                {
                    var clusterpin = googleMap.Pins.FirstOrDefault(x => x.Label == vehiclePlate.VehiclePlate);
                    if (clusterpin != null)
                    {
                        var vehicleselect = mVehicleList.FirstOrDefault(x => x.VehicleId == vehiclePlate.VehicleId);
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

        #endregion Lifecycle

        #region Property

        private int pageWidth = 0;

        private OnlinePageViewModel vm;

        private System.Timers.Timer timer;

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

        /* Danh sách xe online */
        private List<VehicleOnline> mCurrentVehicleList;

        private bool infoStatusIsShown = false;
        private bool boxInfoIsShown = false;
        private bool IsInitMarker = false;

        #endregion Property

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
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
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
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
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
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
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
            var lstpin = googleMap.Pins.Where(x => x.Label == carInfo.VehiclePlate).ToList();
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
                            var lstpin = googleMap.Pins.Where(x => x.Label == vehicle.VehiclePlate).ToList();
                            if (lstpin != null && lstpin.Count > 1)
                            {
                                googleMap.AnimateCamera(CameraUpdateFactory.NewPositionZoom(new Position(lstpin[0].Position.Latitude, lstpin[0].Position.Longitude), MobileSettingHelper.Mapzoom));
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

                    googleMap.AnimateCamera(CameraUpdateFactory.NewPositionZoom(new Position(listPin[0].Lat, listPin[0].Lng), 5));
                }
                else
                {
                    googleMap.Pins.Clear();

                    googleMap.AnimateCamera(CameraUpdateFactory.NewPositionZoom(new Position(Settings.Latitude, Settings.Longitude), 5));
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
                var listPositon = new List<Position>();
                listResult.ForEach(x =>
                {
                    listPositon.Add(new Position(x.Lat, x.Lng));
                });
                var bounds = GeoHelper.FromPositions(listPositon);

                googleMap.AnimateCamera(CameraUpdateFactory.NewBounds(bounds, 60));
            }
            else
            {
                vm.ListVehicleStatus = new List<VehicleOnline>();
                googleMap.Pins.Clear();

                googleMap.AnimateCamera(CameraUpdateFactory.NewPositionZoom(new Position(MobileUserSettingHelper.LatCurrentScreenMap, MobileUserSettingHelper.LngCurrentScreenMap), MobileUserSettingHelper.Mapzoom));
            }

            // Chạy lại hàm tính toán trạng thái xe
            InitVehicleStatus(listResult);
        }

        public void UpdateVehicleByStatus(List<VehicleOnline> mVehicleList, VehicleStatusGroup vehiclestategroup)
        {
            vm.VehicleStatusSelected = vehiclestategroup;
            var listFilter = StateVehicleExtension.GetVehicleCarByStatus(mVehicleList, vehiclestategroup);
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
            else
            {
                vm.ListVehicleStatus = new List<VehicleOnline>();
            }
        }

        private List<VehicleOnlineMarker> ConvertMarkerPin(List<VehicleOnline> lisVehicle)
        {
            var listmarker = new List<VehicleOnlineMarker>();
            vm.ListVehicleStatus = lisVehicle;
            lisVehicle.ForEach(x =>
            {
                listmarker.Add(new VehicleOnlineMarker()
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
                    DoubleMarker = new DoubleMarker().DrawMarker(x),
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
                        || GeoHelper.IsBetweenLatlng(item.Position.Latitude, item.Position.Longitude, carInfo.Lat, carInfo.Lng) || carInfo.Velocity == 0)
                {
                    item.Rotation = carInfo.Direction * 45;
                    itemLable.Position = new Position(carInfo.Lat, carInfo.Lng);
                    item.Position = new Position(carInfo.Lat, carInfo.Lng);
                    return;
                }

                //nếu xe nằm trong màn hình thì mới animation xoay và di chuyển
                if (IsInMapScreen(new Position(carInfo.Lat, carInfo.Lng)) && vm.IsActive)
                {
                    //di chuyển xe
                    item.Rotate(carInfo.Lat, carInfo.Lng, () =>
                    {
                        item.MarkerAnimation(itemLable, carInfo.Lat, carInfo.Lng, () =>
                        {
                            if (carActive)
                            {
                                vm.CurrentAddress = string.Join(", ", GeoHelper.LatitudeToDergeeMinSec(carInfo.Lat), GeoHelper.LongitudeToDergeeMinSec(carInfo.Lng));
                            }
                        });
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
                        vm.CurrentAddress = string.Join(", ", GeoHelper.LatitudeToDergeeMinSec(carInfo.Lat), GeoHelper.LongitudeToDergeeMinSec(carInfo.Lng));
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteError(MethodBase.GetCurrentMethod().Name, ex);
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
            catch (Exception ex)
            {
                LoggerHelper.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
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
        private void InitPinVehicle(List<VehicleOnlineMarker> lstVehicle)
        {
            try
            {
                IsInitMarker = true;
                googleMap.Pins.Clear();

                for (int i = 0; i < lstVehicle.Count; i++)
                {
                    RenderMarkerClusterOnMap(lstVehicle[i].DoubleMarker);
                }
                GetAllIslandVN();
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
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
        private void RenderMarkerClusterOnMap(DoubleMarker carinfo)
        {
            googleMap.Pins.Add(carinfo.Car);
            googleMap.Pins.Add(carinfo.Plate);
        }

        /// <summary>
        /// Vẽ lại icon biển số xe
        /// </summary>
        /// <param name="vehicle"></param>
        /// <param name="bacground"></param>
        private void UpdateBackgroundPinLable(VehicleOnline carinfo, bool isActive = false)
        {
            var lstpin = googleMap.Pins.Where(x => x.Label == carinfo.VehiclePlate).ToList();
            if (lstpin != null && lstpin.Count > 1)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (isActive)
                    {
                        lstpin[1].Tag = carinfo.VehiclePlate + "Active";
                        lstpin[1].Icon = BitmapDescriptorFactory.FromView(new PinInfowindowActiveView(carinfo.PrivateCode));
                    }
                    else
                    {
                        lstpin[1].Tag = carinfo.VehiclePlate + "Plate";

                        lstpin[1].Icon = BitmapDescriptorFactory.FromView(new PinInfowindowView(carinfo.PrivateCode));
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
                vm.CurrentAddress = string.Join(", ", GeoHelper.LatitudeToDergeeMinSec(carInfo.Lat), GeoHelper.LongitudeToDergeeMinSec(carInfo.Lng));

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
                LoggerHelper.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
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

        private void ShowBoxStatus()
        {
            if (!infoStatusIsShown)
            {
                Action<double> callback = input => boxStatusVehicle.TranslationX = input;
                boxStatusVehicle.Animate("animboxStatusVehicle", callback, pageWidth, 0, 16, 300, Easing.CubicInOut);
                infoStatusIsShown = true;
            }
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

        private void TapStatusVehicel(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs args)
        {
            HideBoxStatus();

            HideBoxInfo();

            vm.CarSearch = string.Empty;

            if ((args as Syncfusion.ListView.XForms.ItemTappedEventArgs).ItemData is VehicleStatusViewModel item)
            {
                Device.StartTimer(TimeSpan.FromMilliseconds(300), () =>
                {
                    UpdateVehicleByStatus(mCurrentVehicleList, (VehicleStatusGroup)item.ID);
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
            if ((latlng.Latitude <= googleMap?.Region?.FarLeft.Latitude && latlng.Longitude >= googleMap?.Region?.FarLeft.Longitude) &&
                (latlng.Latitude >= googleMap?.Region?.NearRight.Latitude && latlng.Longitude <= googleMap?.Region?.NearRight.Longitude))
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
                LoggerHelper.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
        }

        void IDestructible.Destroy()
        {
            timer.Stop();
            timer.Dispose();
            this.eventAggregator.GetEvent<ReceiveSendCarEvent>().Unsubscribe(OnReceiveSendCarSignalR);
            this.eventAggregator.GetEvent<OnReloadVehicleOnline>().Unsubscribe(OnReLoadVehicleOnlineCarSignalR);
            this.eventAggregator.GetEvent<BackButtonEvent>().Unsubscribe(AndroidBackButton);
        }

        private void GetAllIslandVN()
        {
            var ts = "TT. Trường Sa";
            var st = "X. Sinh Tồn";
            var stt = "X. Song Tử Tây";
            googleMap.Pins.Add(new Pin()
            {
                Position = new Position(7.54174, 113.79929),
                Label = "TT. Trường Sa",
                Icon = BitmapDescriptorFactory.FromView(new BoundaryNameInfoWindow(ts) { WidthRequest = ts.Length < 20 ? 6 * ts.Length : 110, HeightRequest = 18 * ((ts.Length / 20) + 1) }),
                Tag = "TT. Trường Sa" + "Island"
            });
            googleMap.Pins.Add(new Pin()
            {
                Position = new Position(8.81108, 116.32163),
                Label = "X. Sinh Tồn",
                Icon = BitmapDescriptorFactory.FromView(new BoundaryNameInfoWindow(st) { WidthRequest = st.Length < 20 ? 6 * st.Length : 110, HeightRequest = 18 * ((st.Length / 20) + 1) }),
                Tag = "X. Sinh Tồn" + "Island"
            });
            googleMap.Pins.Add(new Pin()
            {
                Position = new Position(9.35837, 115.91965),
                Label = "X. Song Tử Tây",
                Icon = BitmapDescriptorFactory.FromView(new BoundaryNameInfoWindow(stt) { WidthRequest = stt.Length < 20 ? 6 * stt.Length : 110, HeightRequest = 18 * ((stt.Length / 20) + 1) }),
                Tag = "X. Song Tử Tây" + "Island",
            });
        }

        #endregion Private Method
    }
}