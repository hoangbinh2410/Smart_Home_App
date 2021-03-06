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
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;
using MOTO_MobileGPS.Constant;
using MOTO_MobileGPS.Extensions;
using MOTO_MobileGPS.ViewModels;
using Prism;
using Prism.Events;
using Prism.Ioc;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MOTO_MobileGPS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OnlinePage : ContentPage, IDestructible, INavigationAware
    {
        #region Contructor



        private readonly IEventAggregator eventAggregator;
        private readonly IGeocodeService geocodeService;
        private readonly IDisplayMessage displayMessage;
        private readonly IPageDialogService pageDialog;
        private readonly IMotoDetailService motoDetailService;
        private readonly IMotoPropertiesService motoPropertiesService;

        private bool boxInfoIsShown = false;
        private int pageWidth = 0;
        public OnlinePage()
        {
            InitializeComponent();
            googleMap.UiSettings.ZoomControlsEnabled = false;
            eventAggregator = PrismApplicationBase.Current.Container.Resolve<IEventAggregator>();
            geocodeService = PrismApplicationBase.Current.Container.Resolve<IGeocodeService>();
            displayMessage = PrismApplicationBase.Current.Container.Resolve<IDisplayMessage>();
            pageDialog = PrismApplicationBase.Current.Container.Resolve<IPageDialogService>();
            motoDetailService = PrismApplicationBase.Current.Container.Resolve<IMotoDetailService>();
            motoPropertiesService = PrismApplicationBase.Current.Container.Resolve<IMotoPropertiesService>();

            // Initialize the View Model Object
            vm = (OnlinePageViewModel)BindingContext;
            pageWidth = (int)Application.Current.MainPage.Width;
            boxStatusVehicle.TranslationX = pageWidth;
            boxInfo.TranslationY = 300;
            frVehicleInfo.TranslationX = -pageWidth;

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
            btnDirectvehicleOnline.IsVisible = false;

            this.eventAggregator.GetEvent<ReceiveSendCarEvent>().Subscribe(this.OnReceiveSendCarSignalR);
            this.eventAggregator.GetEvent<OnReloadVehicleOnline>().Subscribe(OnReLoadVehicleOnlineCarSignalR);
            this.eventAggregator.GetEvent<BackButtonEvent>().Subscribe(AndroidBackButton);
            this.eventAggregator.GetEvent<GetLocationVehicleEvent>().Subscribe(GetLocationVehicle);

            IsInitMarker = false;

            StartTimmerCaculatorStatus();

            entrySearch.Placeholder = MobileResource.Route_Label_SearchFishing;
            lblMotorStatus.Text = MobileResource.Moto_Label_Status;
            lblVelocity.Text = MobileResource.Moto_Label_Speed;
        }

        #endregion Contructor

        #region Lifecycle

        private bool viewHasAppeared = false;

        public void OnPageAppearingFirstTime()
        {
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
        }

        public void OnNavigatingTo(INavigationParameters parameters)
        {
        }

        public void Destroy()
        {
            timer.Stop();
            timer.Dispose();


            StopTimerIsOnline();

            this.eventAggregator.GetEvent<ReceiveSendCarEvent>().Unsubscribe(OnReceiveSendCarSignalR);
            this.eventAggregator.GetEvent<OnReloadVehicleOnline>().Unsubscribe(OnReLoadVehicleOnlineCarSignalR);
            this.eventAggregator.GetEvent<BackButtonEvent>().Unsubscribe(AndroidBackButton);
            this.eventAggregator.GetEvent<GetLocationVehicleEvent>().Unsubscribe(GetLocationVehicle);
        }

        #endregion Lifecycle

        #region Property

        private OnlinePageViewModel vm;


        private Timer timer;

        private Timer timerIsOnline;

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
        private bool IsInitMarker = false;

        private void StartTimmerIsOnline()
        {
            timerIsOnline = new Timer
            {
                Interval = 10000
            };
            timerIsOnline.Elapsed += TIsOnline_Elapsed;

            timerIsOnline.Start();
        }

        #endregion Property

        #region Private Method

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

        private void StopTimerIsOnline()
        {
            if (timerIsOnline != null)
            {
                timerIsOnline.Stop();
                timerIsOnline.Dispose();
            }
        }

        private void TIsOnline_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (vm.CarActive != null && vm.CarActive.VehicleId > 0 && GlobalResourcesMoto.Current.MotoDetail != null)
            {
                var isOffline = StateExtensionMoto.IsVehicleOffline(vm.CarActive.GPSTime);

                GlobalResourcesMoto.Current.MotoDetail.IsOnline = !isOffline;
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

                    googleMap.AnimateCamera(CameraUpdateFactory.NewPositionZoom(new Position(listPin[0].Lat, listPin[0].Lng), 5));
                }
                else
                {
                    googleMap.ClusteredPins.Clear();

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
                googleMap.ClusteredPins.Clear();

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
                    vm.EngineState = carInfo.StatusEngineer;
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
                                 Getaddress(carInfo.Lat.ToString(), carInfo.Lng.ToString(), carInfo.VehicleId);
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
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
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
        private void RenderMarkerClusterOnMap(DoubleMarker carinfo)
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
                        lstpin[1].Icon = BitmapDescriptorFactory.FromView(new MOTOPinInfowindowActiveView(carinfo.PrivateCode));
                    }
                    else
                    {
                        lstpin[1].Tag = carinfo.VehiclePlate + "Plate";

                        lstpin[1].Icon = BitmapDescriptorFactory.FromView(new MOTOPinInfowindowView(carinfo.PrivateCode));
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

                btnDirectvehicleOnline.IsVisible = true;

                GetMotoProperties(carInfo);

                vm.EngineState = StateVehicleExtension.EngineState(carInfo);

                Getaddress(carInfo.Lat.ToString(), carInfo.Lng.ToString(), carInfo.VehicleId);

                //update active xe mới
                UpdateBackgroundPinLable(carInfo, true);

                ShowBoxInfo();

                StartTimmerIsOnline();
            }
            else
            {
                pageDialog.DisplayAlertAsync(MobileResource.Common_Message_Warning, MobileResource.Online_Message_CarDebtMoney, MobileResource.Common_Label_Close);
            }
        }

        private void GetMotoProperties(VehicleOnline carinfo)
        {
            Task.Run(async () =>
            {
                return await motoPropertiesService.GetMotoProperties(StaticSettings.User.XNCode, carinfo.VehiclePlate);
            }).ContinueWith(task => Device.BeginInvokeOnMainThread(() =>
            {
                if (task.Status == TaskStatus.RanToCompletion)
                {
                    var temp = task.Result;
                    if (temp != null && temp.Data != null)
                    {
                        MotoStaticSettings.MotoProperties = temp.Data.GetXMPropertiesResult;

                        GetDetailMoto(carinfo);

                    }
                    else
                    {
                        MotoStaticSettings.MotoProperties = new MotoPropertiesViewModel();
                    }
                }
                else if (task.IsFaulted)
                {
                    Logger.WriteError(MethodBase.GetCurrentMethod().Name, "Error");
                }
            }));
        }

        private void GetDetailMoto(VehicleOnline carinfo)
        {
            Task.Run(async () =>
            {
                return await motoDetailService.GetMotoDetail(StaticSettings.User.XNCode, carinfo.VehiclePlate);
            }).ContinueWith(task => Device.BeginInvokeOnMainThread(() =>
            {
                if (task.Status == TaskStatus.RanToCompletion)
                {
                    var response = task.Result;
                    if (response != null && response.Data!= null)
                    {
                        GlobalResourcesMoto.Current.MotoDetail = response.Data.MotoDetailResult;
                        GlobalResourcesMoto.Current.MotoDetail.IsOnline = response.Data.MotoDetailResult.IsOnline;
                        vm.MotoDetail = response.Data.MotoDetailResult;
                        vm.MotoDetail.DevicePhone = MotoStaticSettings.MotoProperties.DevicePhoneNumber;
                        Getaddress(carinfo.Lat.ToString(), carinfo.Lng.ToString(), carinfo.VehicleId);
                    }
                    else
                    {
                        vm.MotoDetail = new MotoDetailViewModel();
                    }
                }
                else if (task.IsFaulted)
                {
                    Logger.WriteError(MethodBase.GetCurrentMethod().Name, "Error");
                }
            }));
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
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
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
            btnDirectvehicleOnline.IsVisible = false;

            StopTimerIsOnline();
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
            if (args.Pin != null)
            {
                args.Handled = true;
            }
            else
            {
                args.Handled = false;
            }

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
                    Action<double> frcallback = input => frVehicleInfo.TranslationX = input;
                    frVehicleInfo.Animate("animehicleInfo", frcallback, 0, -pageWidth, 16, 300, Easing.CubicInOut);


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
                    Action<double> frcallback = input => frVehicleInfo.TranslationX = input;
                    frVehicleInfo.Animate("animehicleInfo", frcallback, -pageWidth, 0, 16, 300, Easing.CubicInOut);
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

        private void ShowBoxStatus()
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
                default:
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
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
        }

        private void GetLocationVehicle()
        {
            if (!vm.MotoDetail.IsOnline)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var p = MotoConfigHelper.JoinPhoneNumberEx(MotoStaticSettings.MotoProperties.PhoneNumber1) + MotoConfigHelper.JoinPhoneNumberEx(MotoStaticSettings.MotoProperties.PhoneNumber2) + MotoConfigHelper.JoinPhoneNumberEx(MotoStaticSettings.MotoProperties.PhoneNumber3);
                    if (p.Length > 0)
                    {
                        var result = await DisplayAlert(MobileResource.Moto_Label_Confirm, MobileResource.Moto_Message_Send_Location, MobileResource.Common_Button_OK, MobileResource.Common_Message_Skip);
                        if (result)
                        {
                            SendActionHelper.SendSMS(MotoStaticSettings.MotoProperties.DevicePhoneNumber, MotoParameterKey.ValueSendLocation);
                        }
                    }
                    else
                    {
                        await DisplayAlert(MobileResource.Moto_Label_Alter, MobileResource.Moto_Message_Config_PhoneNumber, MobileResource.Moto_Label_Confirm);
                    }
                });
            }
            else
            {
                if (mCarActive != null)
                {
                    if (IsBusy)
                    {
                        return;
                    }
                    IsBusy = true;
                    try
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            googleMap.AnimateCamera(CameraUpdateFactory.NewPositionZoom(new Position(mCarActive.Lat, mCarActive.Lng), 17));
                        });
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
                    }
                    finally
                    {
                        IsBusy = false;
                    }
                }
            }
        }
        #endregion Private Method
    }
}