using BA_MobileGPS;
using BA_MobileGPS.Core;
using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Extensions;
using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Core.Resource;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.ModelViews;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;
using Prism;
using Prism.Ioc;
using Prism.Events;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using VMS_MobileGPS.ViewModels;
using Xamarin.Forms;

using Xamarin.Forms.Xaml;
using System.Threading;

namespace VMS_MobileGPS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OnlinePage : ContentView, IDestructible, INavigationAware
    {
        private enum States
        {
            ShowFilter,
            HideFilter,
            ShowStatus,
            HideStatus
        }

        private readonly IEventAggregator eventAggregator;
        private readonly IDisplayMessage displayMessage;
        private readonly IHelperAdvanceService helperAdvanceService;
        private readonly IPageDialogService pageDialog;
        private readonly IVehicleOnlineService vehicleOnlineService;

        private readonly BA_MobileGPS.Core.Animation _animations = new BA_MobileGPS.Core.Animation();

        private OnlinePageViewModel vm;

        private System.Timers.Timer timer;
        private CancellationTokenSource cts;

        public OnlinePage()
        {
            InitializeComponent();
            googleMap.UiSettings.ZoomControlsEnabled = false;

            //On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);

            eventAggregator = PrismApplicationBase.Current.Container.Resolve<IEventAggregator>();
            displayMessage = PrismApplicationBase.Current.Container.Resolve<IDisplayMessage>();
            helperAdvanceService = PrismApplicationBase.Current.Container.Resolve<IHelperAdvanceService>();
            pageDialog = PrismApplicationBase.Current.Container.Resolve<IPageDialogService>();
            vehicleOnlineService = PrismApplicationBase.Current.Container.Resolve<IVehicleOnlineService>();

            // Initialize the View Model Object
            vm = (OnlinePageViewModel)BindingContext;
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

                    googleMap.InitialCameraUpdate = CameraUpdateFactory.NewBounds(bounds, 60);
                }
                else
                {
                    googleMap.InitialCameraUpdate = CameraUpdateFactory.NewPositionZoom(new Position(StaticSettings.ListVehilceOnline[0].Lat, StaticSettings.ListVehilceOnline[0].Lng), MobileUserSettingHelper.Mapzoom);
                }
            }
            else
            {
                googleMap.InitialCameraUpdate = CameraUpdateFactory.NewPositionZoom(new Position(MobileUserSettingHelper.LatCurrentScreenMap, MobileUserSettingHelper.LngCurrentScreenMap), MobileUserSettingHelper.Mapzoom);
            }
            googleMap.IsUseCluster = true;
            googleMap.IsTrafficEnabled = false;

            googleMap.ClusterOptions.EnableBuckets = true;
            googleMap.ClusterOptions.Algorithm = ClusterAlgorithm.GridBased;

            googleMap.UiSettings.MapToolbarEnabled = false;
            googleMap.UiSettings.ZoomControlsEnabled = false;
            googleMap.UiSettings.MyLocationButtonEnabled = false;
            googleMap.UiSettings.RotateGesturesEnabled = false;

            googleMap.ClusterClicked += Map_ClusterClicked;
            googleMap.PinClicked += MapOnPinClicked;
            googleMap.MapClicked += Map_MapClicked;
            googleMap.CameraIdled += GoogleMap_CameraIdled;

            InitAnimation();

            mCarActive = new VehicleOnline();
            mCurrentVehicleList = new List<VehicleOnline>();

            this.eventAggregator.GetEvent<ReceiveSendCarEvent>().Subscribe(this.OnReceiveSendCarSignalR);

            IsInitMarker = false;

            StartTimmerCaculatorStatus();
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
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
                    }
                    else
                    {
                        displayMessage.ShowMessageInfo(MobileResource.Common_Message_NotFindYourCar);
                    }
                }
            }
            else if (parameters.ContainsKey(ParameterKey.Company) && parameters.GetValue<Company>(ParameterKey.Company) is Company company)
            {
                HideBoxStatus();

                HideBoxInfo();

                UpdateVehicleByCompany(company);
            }
            else if (parameters.ContainsKey(ParameterKey.VehicleGroups) && parameters.GetValue<int[]>(ParameterKey.VehicleGroups) is int[] vehiclegroup)
            {
                vm.VehicleGroups = vehiclegroup;

                HideBoxStatus();

                HideBoxInfo();

                UpdateVehicleByVehicleGroup(vehiclegroup);
            }
            else
            {
                InitOnline();
            }
        }

        public void OnNavigatingTo(INavigationParameters parameters)
        {
        }

        public void Destroy()
        {
            timer.Stop();
            timer.Dispose();
            eventAggregator.GetEvent<ReceiveSendCarEvent>().Unsubscribe(OnReceiveSendCarSignalR);
        }

        #region Propety

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

        #endregion Propety

        #region Private Method

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

        private async void InitAnimation()
        {
            try
            {
                if (_animations == null)
                {
                    return;
                }

                _animations.Add(States.ShowFilter, new[] {
                                                            new ViewTransition(boxInfo, AnimationType.TranslationY, 0, 300, delay: 300), // Active and visible
                                                new ViewTransition(boxInfo, AnimationType.Opacity, 1, 0), // Active and visible
                                                          });

                _animations.Add(States.HideFilter, new[] {
                                                            new ViewTransition(boxInfo, AnimationType.TranslationY, 300),
                                                            new ViewTransition(boxInfo, AnimationType.Opacity, 0),
                                                          });

                await _animations.Go(States.HideFilter, false);

                var pageWidth = Xamarin.Forms.Application.Current?.MainPage?.Width;

                if (pageWidth > 0)
                {
                    AbsoluteLayout.SetLayoutBounds(boxStatusVehicle, new Rectangle(1, 0, pageWidth.GetValueOrDefault(), 1));

                    _animations.Add(States.ShowStatus, new[] {
                                                            new ViewTransition(boxStatusVehicle, AnimationType.TranslationX, 0,  pageWidth.GetValueOrDefault(), delay: 200), // Active and visible
                                                            new ViewTransition(boxStatusVehicle, AnimationType.Opacity, 1, 0), // Active and visible
                                                          });

                    _animations.Add(States.HideStatus, new[] {
                                                            new ViewTransition(boxStatusVehicle, AnimationType.TranslationX, pageWidth.GetValueOrDefault()),
                                                            new ViewTransition(boxStatusVehicle, AnimationType.Opacity, 0),
                                                          });

                }



                await _animations.Go(States.HideStatus, false);
            }
            catch (Exception ex)
            {

                LoggerHelper.WriteError("InitAnimation", ex);
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
                        item.MarkerAnimation(carInfo.Lat, carInfo.Lng, () =>
                        {
                            if (carActive)
                            {
                                vm.CurrentAddress = string.Join(", ", GeoHelper.LatitudeToDergeeMinSec(carInfo.Lat), GeoHelper.LongitudeToDergeeMinSec(carInfo.Lng));
                            }
                        });
                        //di chuyển biển số xe
                        itemLable.MarkerAnimation(carInfo.Lat, carInfo.Lng, () => { });
                    });
                }
                else
                {
                    itemLable.Position = new Position(carInfo.Lat, carInfo.Lng);
                    item.Position = new Position(carInfo.Lat, carInfo.Lng);
                    if (carActive)
                    {
                        googleMap.AnimateCamera(CameraUpdateFactory.NewPosition(new Position(carInfo.Lat, carInfo.Lng)));
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
                            var list = StaticSettings.ListVehilceOnline;
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

        private void GetListVehicleOnline()
        {
            var userID = StaticSettings.User.UserId;
            if (Settings.CurrentCompany != null && Settings.CurrentCompany.FK_CompanyID > 0)
            {
                userID = Settings.CurrentCompany.UserId;
            }
            int vehicleGroup = 0;

            if (cts != null)
                cts.Cancel(true);

            cts = new CancellationTokenSource();

            Task.Run(async () =>
            {
                await Task.Delay(500, cts.Token);

                return await vehicleOnlineService.GetListVehicleOnline(userID, vehicleGroup);
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
                        InitOnline();
                    }
                    else
                    {
                        StaticSettings.ListVehilceOnline = new List<VehicleOnline>();
                    }
                }
            }));
        }



        /// <summary>
        /// khởi tạo danh sách trạng thái xe
        /// </summary>
        /// <param name="vehicleList"></param>
        private void InitVehicleStatus(List<VehicleOnline> vehicleList)
        {
            //txtCountVehicle.Text = vehicleList.Count().ToString();
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

                    //googleMap.Cluster();
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
                pageDialog.DisplayAlertAsync(MobileResource.Common_Message_Warning, MobileResource.Online_Message_CarDebtMoney, MobileResource.Common_Label_Close);
            }
        }

        /// <summary>
        /// ẩn thông tin xe đi và remove active xe
        /// </summary>
        /// <param name="carinfo"></param>
        private void HideBoxInfoCarActive(VehicleOnline carinfo)
        {
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
        public async void HideBoxInfo()
        {
            vm.HideBorder();
            eventAggregator.GetEvent<ShowTabItemEvent>().Publish(true);
            SetNoPaddingWithFooter();
            await _animations.Go(States.HideFilter, true);
        }

        /// <summary>
        /// Hiển thị box thông tin xe
        /// </summary>
        private async void ShowBoxInfo()
        {
            eventAggregator.GetEvent<ShowTabItemEvent>().Publish(false);
            SetPaddingWithFooter();
            await _animations.Go(States.ShowFilter, true);
        }

        /* Set padding map khi có thông tin xe ở footer - tracking */

        public void SetPaddingWithFooter()
        {
            double paddingMap = boxInfo.HeightRequest;
            googleMap.Padding = new Thickness(0, 0, 0, (int)paddingMap);
            BoxControls.Margin = new Thickness(20, 0, 20, (int)paddingMap + 25);
        }

        /* Set padding map khi có thông tin xe ở footer - tracking */

        public void SetNoPaddingWithFooter()
        {
            googleMap.Padding = new Thickness(0, 0, 0, 0);
            BoxControls.Margin = new Thickness(20);
        }

        private async void HideBoxStatus()
        {
            await _animations.Go(States.HideStatus, true);
            infoStatusIsShown = false;
        }

        private async void ShowBoxStatus()
        {
            await _animations.Go(States.ShowStatus, true);
            infoStatusIsShown = true;
        }

        private void TapStatusVehicel(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs args)
        {
            HideBoxStatus();

            HideBoxInfo();

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

        private async void FilterStatusCar(object sender, EventArgs e)
        {
            if (infoStatusIsShown)
            {
                await _animations.Go(States.HideStatus, true);
            }
            else
            {
                await _animations.Go(States.ShowStatus, true);
            }

            infoStatusIsShown = !infoStatusIsShown;

            //InitVehicleStatus(mVehicleList);
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

        #endregion Private Method
    }
}