using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Extensions;
using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Core.Interfaces;
using BA_MobileGPS.Core.Models;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Core.ViewModels.Base;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.ModelViews;
using BA_MobileGPS.Service;
using BA_MobileGPS.Service.Utilities;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Enums;
using Prism.Commands;
using Prism.Navigation;
using Prism.Navigation.TabbedPages;
using Syncfusion.Data.Extensions;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class ListVehiclePageViewModel : TabbedPageChildVMBase
    {
        private CancellationTokenSource cts;

        public ICommand ShowHelpCommand { get; private set; }
        public ICommand ChangeSortCommand { get; private set; }
        public ICommand SearchVehicleCommand { get; private set; }
        public ICommand TapListVehicleCommand { get; private set; }
        public ICommand SelectStatusVehicleCommand { get; private set; }

        private readonly IMapper _mapper;
        private readonly IGeocodeService geocodeService;
        private readonly IPopupServices popupServices;

        public ListVehiclePageViewModel(INavigationService navigationService, IMapper mapper, IGeocodeService geocodeService, IPopupServices popupServices)
            : base(navigationService)
        {
            this._mapper = mapper;
            this.geocodeService = geocodeService;
            this.popupServices = popupServices;
            ShowHelpCommand = new DelegateCommand(ShowHelp);
            ChangeSortCommand = new DelegateCommand(ChangeSort);
            SearchVehicleCommand = new DelegateCommand<TextChangedEventArgs>(SearchVehiclewithText);
            TapListVehicleCommand = new DelegateCommand<Syncfusion.ListView.XForms.ItemTappedEventArgs>(TapListVehicle);
            SelectStatusVehicleCommand = new DelegateCommand<Syncfusion.ListView.XForms.ItemTappedEventArgs>(SelectStatusVehicle);

            EventAggregator.GetEvent<ReceiveSendCarEvent>().Subscribe(OnReceiveSendCarSignalR);
            EventAggregator.GetEvent<OnReloadVehicleOnline>().Subscribe(OnReLoadVehicleOnlineCarSignalR);
            EventAggregator.GetEvent<SelectedCompanyEvent>().Subscribe(OnCompanyChanged);
        }

        #region Lifecycle

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters?.GetValue<int[]>(ParameterKey.VehicleGroups) is int[] vehiclegroup)
            {
                VehicleGroups = vehiclegroup;

                UpdateVehicleByVehicleGroup(vehiclegroup);
            }
            else if (parameters.ContainsKey("FavoriteVehicle") && parameters?.GetValue<bool>("FavoriteVehicle") is bool isfavorites)
            {
                SetFavoritesVehicle(currentVehicle, isfavorites);
            }
            else if (parameters.ContainsKey("MenuPageItem") && parameters?.GetValue<MenuKeyType>("MenuPageItem") is MenuKeyType action)
            {
                GoToPageAction(action);
            }
            else if (parameters.ContainsKey("pagetoNavigation") && parameters?.GetValue<string>("pagetoNavigation") is string menukey)
            {
                OnNavigateMenu(menukey);
            }
        }

        public override void OnDestroy()
        {
            EventAggregator.GetEvent<ReceiveSendCarEvent>().Unsubscribe(OnReceiveSendCarSignalR);
            EventAggregator.GetEvent<OnReloadVehicleOnline>().Unsubscribe(OnReLoadVehicleOnlineCarSignalR);
            EventAggregator.GetEvent<SelectedCompanyEvent>().Unsubscribe(OnCompanyChanged);
        }

        #endregion Lifecycle

        #region Property

        // Danh sách xe gốc chưa lọc
        private List<VehicleOnlineViewModel> ListVehicleOrigin
        {
            get
            {
                if (StaticSettings.ListVehilceOnline != null)
                {
                    List<VehicleOnlineViewModel> result = new List<VehicleOnlineViewModel>();
                    var lstmap = _mapper.MapListProperties<VehicleOnlineViewModel>(StaticSettings.ListVehilceOnline).ToList();
                    lstmap.ForEach(x =>
                    {
                        if (FavoritesVehicleHelper.IsFavoritesVehicleOnline(x.VehiclePlate))
                        {
                            x.IsFavorite = true;
                        }
                    });
                    if (Settings.SortOrder == (int)SortOrderType.PrivateCodeASC)
                        result = lstmap.OrderByDescending(x => x.IsFavorite == true)
                            .ThenByDescending(x => x.SortOrder).ThenBy(x => x.PrivateCode).ToList();
                    else if (Settings.SortOrder == (int)SortOrderType.PrivateCodeDES)
                        result = lstmap.OrderByDescending(x => x.IsFavorite == true)
                            .ThenByDescending(x => x.SortOrder).ThenByDescending(x => x.PrivateCode).ToList();
                    else if (Settings.SortOrder == (int)SortOrderType.TimeASC)
                        result = lstmap.OrderByDescending(x => x.IsFavorite == true)
                            .ThenByDescending(x => x.SortOrder).ThenBy(x => x.VehicleTime).ToList();
                    else if (Settings.SortOrder == (int)SortOrderType.TimeDES)
                        result = lstmap.OrderByDescending(x => x.IsFavorite == true)
                            .ThenByDescending(x => x.SortOrder).ThenByDescending(x => x.VehicleTime).ToList();
                    else if (Settings.SortOrder == (int)SortOrderType.DefaultASC)
                        result = lstmap.OrderByDescending(x => x.IsFavorite == true)
                            .ThenBy(x => x.SortOrder).ToList();
                    else
                        result = lstmap.OrderByDescending(x => x.IsFavorite == true)
                            .ThenByDescending(x => x.SortOrder).ToList();

                    return result;
                }
                else
                {
                    return new List<VehicleOnlineViewModel>();
                }
            }
        }

        // Danh sách xe đã được lọc theo nhóm, công ty, trạng thái
        private List<VehicleOnlineViewModel> ListVehicleByGroup
        {
            get
            {
                if (StaticSettings.ListVehilceOnline != null)
                {
                    if (VehicleGroups != null && VehicleGroups.Length > 0)
                    {
                        return ListVehicleOrigin.FindAll(v => v.GroupIDs.Split(',').ToList().Exists(g => VehicleGroups.Contains(Convert.ToInt32(g))));
                    }
                    else
                    {
                        return ListVehicleOrigin;
                    }
                }
                else
                {
                    return new List<VehicleOnlineViewModel>();
                }
            }
        }

        private List<VehicleOnlineViewModel> ListVehicleByStatus = new List<VehicleOnlineViewModel>();

        public int countVehicle;
        public int CountVehicle { get => countVehicle; set => SetProperty(ref countVehicle, value); }

        private VehicleStatusViewModel selectedStatus;
        public VehicleStatusViewModel SelectedStatus { get => selectedStatus; set => SetProperty(ref selectedStatus, value); }

        private ObservableCollection<VehicleStatusViewModel> listVehilceStatus;
        public ObservableCollection<VehicleStatusViewModel> ListVehilceStatus { get => listVehilceStatus; set => SetProperty(ref listVehilceStatus, value); }

        private ObservableCollection<VehicleOnlineViewModel> listVehicle = new ObservableCollection<VehicleOnlineViewModel>();
        public ObservableCollection<VehicleOnlineViewModel> ListVehicle { get => listVehicle; set => SetProperty(ref listVehicle, value); }

        public string searchedText;
        public string SearchedText { get => searchedText; set => SetProperty(ref searchedText, value); }

        #endregion Property

        #region Private Method

        private void OnReLoadVehicleOnlineCarSignalR(bool arg)
        {
            if (arg)
            {
                InitVehicleList();
            }
            else
            {
                GetListVehicleOnline();
            }
        }

        private void OnCompanyChanged(int e)
        {
            UpdateVehicleByCompany();
        }

        public void UpdateVehicleByCompany()
        {
            SearchedText = null;

            //Update lai danh sach xe moi
            ListVehicle = ListVehicleOrigin.ToObservableCollection();

            // Chạy lại hàm tính toán trạng thái xe
            InitVehicleStatus();
        }

        public void UpdateVehicleByVehicleGroup(int[] vehiclegroup)
        {
            SearchedText = null;

            if (vehiclegroup.Length > 0)
            {
                ListVehicle = ListVehicleOrigin.FindAll(v => v.GroupIDs.Split(',').ToList().Exists(g => vehiclegroup.Contains(Convert.ToInt32(g)))).ToObservableCollection();
            }
            else
            {
                ListVehicle = ListVehicleOrigin.ToObservableCollection();
            }
            ListVehicleByStatus = ListVehicle.ToList();
            // Chạy lại hàm tính toán trạng thái xe
            InitVehicleStatus();
        }

        private void InitVehicleList()
        {
            try
            {
                SearchedText = null;
                if (StaticSettings.ListVehilceOnline != null && StaticSettings.ListVehilceOnline.Count > 0)
                {
                    //Nếu là công ty thường thì mặc định load xe của công ty lên bản đồ
                    if (!UserHelper.isCompanyPartner(UserInfo.CompanyType))
                    {
                        ListVehicle = ListVehicleOrigin.ToObservableCollection();
                    }
                    else
                    {
                        //nếu trước đó đã chọn 1 công ty nào đó rồi thì load danh sách xe của công ty đó
                        if (Settings.CurrentCompany != null && Settings.CurrentCompany.FK_CompanyID > 0)
                        {
                            UpdateVehicleByCompany();
                        }
                        else
                        {
                            DisplayMessage.ShowMessageInfo(MobileResource.Common_Message_SelectCompany);
                        }
                    }
                }
                else
                {
                    StaticSettings.ListVehilceOnline = new List<VehicleOnline>();
                    ListVehicle = ListVehicleOrigin.ToObservableCollection();
                }

                if (VehicleGroups != null && VehicleGroups.Length > 0)
                {
                    ListVehicle = ListVehicleOrigin.FindAll(v => v.GroupIDs.Split(',').ToList().Exists(g => VehicleGroups.Contains(Convert.ToInt32(g)))).ToObservableCollection();
                }
                else
                {
                    ListVehicle = ListVehicleOrigin.ToObservableCollection();
                }
                ListVehicleByStatus = ListVehicleOrigin;
                InitVehicleStatus();
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        public void InitVehicleStatus()
        {
            var vehicleList = _mapper.MapListProperties<VehicleOnline>(ListVehicle.ToList());
            VehicleStatusSelected = VehicleStatusGroup.All;
            CountVehicle = vehicleList.Count();
            List<VehicleStatusViewModel> listStatus = new List<VehicleStatusViewModel>();
            // Lấy trạng thái xe
            if (CompanyConfigurationHelper.UseNewSummaryIconOnline)
            {
                listStatus = (new VehicleStatusHelper()).DictVehicleStatusNew.Values.Where(x => x.IsEnable).ToList();
            }
            else
            {
                listStatus = (new VehicleStatusHelper()).DictVehicleStatus.Values.Where(x => x.IsEnable).ToList();
            }

            if (listStatus != null && listStatus.Count > 0)
            {
                listStatus.ForEach(x =>
                {
                    x.CountCar = StateVehicleExtension.GetCountCarByStatus(vehicleList, (VehicleStatusGroup)x.ID);
                });
                ListVehilceStatus = listStatus.ToObservableCollection();
                SelectedStatus = ListVehilceStatus.First();
            }
        }

        private void OnReceiveSendCarSignalR(VehicleOnline carInfo)
        {
            var vehicle = ListVehicle.FirstOrDefault(x => x.VehicleId == carInfo.VehicleId);

            if (vehicle == null)
            {
                return;
            }

            if (vehicle != null)
            {
                vehicle.Velocity = carInfo.Velocity;
                vehicle.GPSTime = carInfo.GPSTime;
                vehicle.VehicleTime = carInfo.VehicleTime;
                vehicle.StopTime = carInfo.StopTime;
                vehicle.IconCode = carInfo.IconCode;
                vehicle.State = carInfo.State;
                if (carInfo.TotalKm > 0)
                {
                    vehicle.TotalKm = carInfo.TotalKm;
                }
                vehicle.Temperature = carInfo.Temperature;
                vehicle.StatusEngineer = carInfo.StatusEngineer;
                vehicle.IconImage = IconCodeHelper.GetMarkerResource(carInfo);

                if (CompanyConfigurationHelper.VehicleOnlineAddressEnabled)
                {
                    if (StateVehicleExtension.IsMovingAndEngineON(carInfo) && !GeoHelper.IsBetweenLatlng(vehicle.Lat, vehicle.Lng, carInfo.Lat, carInfo.Lng))
                    {
                        Task.Run(async () =>
                        {
                            return await geocodeService.GetAddressByLatLng(CurrentComanyID, vehicle.Lat.ToString(), vehicle.Lng.ToString());
                        }).ContinueWith(task => Device.BeginInvokeOnMainThread(() =>
                        {
                            if (task.Status == TaskStatus.RanToCompletion)
                            {
                                TryExecute(() =>
                                {
                                    vehicle.CurrentAddress = task.Result;
                                });
                            }
                        }));
                    }
                }

                vehicle.Lat = carInfo.Lat;
                vehicle.Lng = carInfo.Lng;
            }
        }

        private void ShowHelp()
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("ListVehicleHelpPage", null, useModalNavigation: true, true);
            });
        }

        private async void ChangeSort()
        {
            var sort = await PageDialog.DisplayActionSheetAsync(MobileResource.ListVehicle_Label_SortBy, MobileResource.Common_Button_Cancel, null,
                MobileResource.ListVehicle_Label_ByVehiclePlate, MobileResource.ListVehicle_Label_ByTime, MobileResource.ListVehicle_Label_Default);

            if (MobileResource.ListVehicle_Label_ByVehiclePlate.Equals(sort))
            {
                if (Settings.SortOrder == (int)SortOrderType.PrivateCodeASC)
                    Settings.SortOrder = (int)SortOrderType.PrivateCodeDES;
                else if (Settings.SortOrder == (int)SortOrderType.PrivateCodeDES)
                    Settings.SortOrder = (int)SortOrderType.PrivateCodeASC;
                else
                    Settings.SortOrder = (int)SortOrderType.PrivateCodeDES;
            }
            else if (MobileResource.ListVehicle_Label_ByTime.Equals(sort))
            {
                if (Settings.SortOrder == (int)SortOrderType.TimeDES)
                    Settings.SortOrder = (int)SortOrderType.TimeASC;
                else
                    Settings.SortOrder = (int)SortOrderType.TimeDES;
            }
            else if (MobileResource.ListVehicle_Label_Default.Equals(sort))
            {
                if (Settings.SortOrder == (int)SortOrderType.DefaultDES)
                    Settings.SortOrder = (int)SortOrderType.DefaultASC;
                else
                    Settings.SortOrder = (int)SortOrderType.DefaultDES;
            }
            else
            {
                return;
            }

            SetSortOrder();
        }

        private void SetSortOrder(bool isUpdateStatus = true)
        {
            if (Settings.SortOrder == (int)SortOrderType.PrivateCodeASC)
                ListVehicle = ListVehicle.OrderByDescending(x => x.IsFavorite == true).ThenByDescending(x => x.SortOrder)
                    .ThenBy(x => x.PrivateCode).ToObservableCollection();
            else if (Settings.SortOrder == (int)SortOrderType.PrivateCodeDES)
                ListVehicle = ListVehicle.OrderByDescending(x => x.IsFavorite == true).ThenByDescending(x => x.SortOrder)
                    .ThenByDescending(x => x.PrivateCode).ToObservableCollection();
            else if (Settings.SortOrder == (int)SortOrderType.TimeASC)
                ListVehicle = ListVehicle.OrderByDescending(x => x.IsFavorite == true)
                    .ThenByDescending(x => x.SortOrder).ThenBy(x => x.VehicleTime).ToObservableCollection();
            else if (Settings.SortOrder == (int)SortOrderType.TimeDES)
                ListVehicle = ListVehicle.OrderByDescending(x => x.IsFavorite == true)
                    .ThenByDescending(x => x.SortOrder).ThenByDescending(x => x.VehicleTime).ToObservableCollection();
            else if (Settings.SortOrder == (int)SortOrderType.DefaultASC)
                ListVehicle = ListVehicle.OrderByDescending(x => x.IsFavorite == true)
                    .ThenBy(x => x.SortOrder).ToObservableCollection();
            else
                ListVehicle = ListVehicle.OrderByDescending(x => x.IsFavorite == true)
                    .ThenByDescending(x => x.SortOrder).ToObservableCollection();

            if (isUpdateStatus)
            {
                ListVehicleByStatus = ListVehicle.ToList();
            }
        }

        private void SearchVehiclewithText(TextChangedEventArgs args)
        {
            if (ListVehicleByGroup == null || ListVehicleByStatus == null || args.NewTextValue == null)
                return;

            if (cts != null)
                cts.Cancel(true);

            cts = new CancellationTokenSource();

            Task.Run(async () =>
            {
                await Task.Delay(500, cts.Token);

                if (string.IsNullOrWhiteSpace(args.NewTextValue))
                    return ListVehicleByStatus;
                return ListVehicleByStatus.FindAll(v => v.VehiclePlate.ToUpper().Contains(args.NewTextValue.ToUpper()) || v.PrivateCode.ToUpper().Contains(args.NewTextValue.ToUpper()));
            }, cts.Token).ContinueWith(task => Device.BeginInvokeOnMainThread(() =>
            {
                if (task.Status == TaskStatus.RanToCompletion)
                {
                    ListVehicle = new ObservableCollection<VehicleOnlineViewModel>(task.Result);

                    SetSortOrder(false);
                }
            }));
        }

        private void SelectStatusVehicle(Syncfusion.ListView.XForms.ItemTappedEventArgs args)
        {
            SafeExecute(() =>
            {
                if (args.ItemData is VehicleStatusViewModel item)
                {
                    SearchedText = null;

                    // Gọi hàm tìm kiếm xe
                    SearchVehicleWithStatus(item);
                }
            });
        }

        private void SearchVehicleWithStatus(VehicleStatusViewModel selected)
        {
            if (ListVehicleByGroup == null)
            {
                return;
            }

            // Lọc theo trạng thái xe
            if (selected != null)
            {
                VehicleStatusSelected = (VehicleStatusGroup)selected.ID;
                List<VehicleOnline> listVehicle = _mapper.MapListProperties<VehicleOnline>(ListVehicleByGroup);
                var listFilter = StateVehicleExtension.GetVehicleCarByStatus(listVehicle, (VehicleStatusGroup)selected.ID);

                if (listFilter != null)
                {
                    listFilter.ForEach(x =>
                    {
                        x.IconImage = IconCodeHelper.GetMarkerResource(x);
                    });
                    var lst = _mapper.MapListProperties<VehicleOnlineViewModel>(listFilter);
                    lst.ForEach(x =>
                    {
                        if (FavoritesVehicleHelper.IsFavoritesVehicleOnline(x.VehiclePlate))
                        {
                            x.IsFavorite = true;
                        }
                    });
                    ListVehicleByStatus = lst;

                    ListVehicle = lst.ToObservableCollection();

                    SetSortOrder();
                }
            }
        }

        private VehicleOnlineViewModel currentVehicle { get; set; }

        private void TapListVehicle(Syncfusion.ListView.XForms.ItemTappedEventArgs args)
        {
            SafeExecute(async () =>
            {
                if (args.ItemData is VehicleOnlineViewModel selected)
                {
                    //Nếu messageId = 2 hoặc 3 là xe phải thu phí
                    if (StateVehicleExtension.IsVehicleStopService(selected.MessageId) && MobileSettingHelper.IsUseVehicleDebtMoney)
                    {
                        var mes = string.IsNullOrEmpty(selected.MessageDetailBAP) ? selected.MessageBAP : selected.MessageDetailBAP;
                        ShowInfoMessageDetailBAP(mes);
                        return;
                    }
                    currentVehicle = selected;

                    await NavigationService.NavigateAsync("DetailVehiclePopup", parameters: new NavigationParameters
                        {
                            { "vehicleItem",  selected}
                        }, true, true);
                }
            });
        }

        public void CacularVehicleStatus()
        {
            var vehicleList = _mapper.MapListProperties<VehicleOnline>(ListVehicleByGroup);

            if (ListVehilceStatus != null && ListVehilceStatus.Count > 0)
            {
                ListVehilceStatus.ForEach(x =>
                {
                    x.CountCar = StateVehicleExtension.GetCountCarByStatus(vehicleList, (VehicleStatusGroup)x.ID);
                });
            }
        }

        private void GetListVehicleOnline()
        {
            if (StaticSettings.ListVehilceOnline != null && StaticSettings.ListVehilceOnline.Count > 0)
            {
                InitVehicleList();
            }
        }

        private void SetFavoritesVehicle(VehicleOnlineViewModel selected, bool isFavorites)
        {
            if (selected != null)
            {
                FavoritesVehicleHelper.UpdateFavoritesVehicleOnline(selected.VehiclePlate);
                var vehicle = ListVehicle.FirstOrDefault(x => x.VehicleId == selected.VehicleId);
                if (vehicle != null)
                {
                    vehicle.IsFavorite = isFavorites;
                    var item = ListVehicleByStatus.FirstOrDefault(x => x.VehicleId == selected.VehicleId);
                    if (item != null)
                    {
                        item.IsFavorite = isFavorites;
                    }
                    SetSortOrder(false);
                }
            }
        }

        #endregion Private Method

        #region Navigation

        public void GoDetailPage(VehicleOnlineViewModel selected)
        {
            SafeExecute(async () =>
            {
                var param = _mapper.MapProperties<VehicleOnline>(selected);
                var parameters = new NavigationParameters
                {
                    { ParameterKey.CarDetail, param }
                };
                if (StaticSettings.ListVehilceCamera != null && StaticSettings.ListVehilceCamera.Count > 0)
                {
                    var plate = selected.VehiclePlate.Contains("_C") ? selected.VehiclePlate : selected.VehiclePlate + "_C";
                    var model = StaticSettings.ListVehilceCamera.FirstOrDefault(x => x.VehiclePlate == plate);
                    if (model != null)
                    {
                        await NavigationService.NavigateAsync("NavigationPage/VehicleDetailCameraPage", parameters, true, true);
                    }
                    else
                    {
                        await NavigationService.NavigateAsync("BaseNavigationPage/VehicleDetailPage", parameters, true, true);
                    }
                }
                else
                {
                    await NavigationService.NavigateAsync("BaseNavigationPage/VehicleDetailPage", parameters, true, true);
                }
            });
        }

        public void GoRoutePage(VehicleOnlineViewModel selected)
        {
            SafeExecute(async () =>
            {
                var param = _mapper.MapProperties<VehicleOnline>(selected);
                var parameters = new NavigationParameters
                {
                    { ParameterKey.VehicleOnline, param }
                };

                await NavigationService.SelectTabAsync("RoutePage", parameters);
            });
        }

        public void GoOnlinePage(VehicleOnlineViewModel selected)
        {
            SafeExecute(async () =>
            {
                var param = _mapper.MapProperties<Vehicle>(selected);
                var parameters = new NavigationParameters
                {
                    { ParameterKey.Vehicle, param }
                };
                if (App.AppType == AppType.Moto)
                {
                    if (MobileUserSettingHelper.EnableShowCluster)
                    {
                        await NavigationService.SelectTabAsync("OnlinePageMoto", parameters);
                    }
                    else
                    {
                        await NavigationService.SelectTabAsync("OnlinePageNoClusterMoto", parameters);
                    }
                }
                else
                {
                    if (MobileUserSettingHelper.EnableShowCluster)
                    {
                        await NavigationService.SelectTabAsync("OnlinePage", parameters);
                    }
                    else
                    {
                        await NavigationService.SelectTabAsync("OnlinePageNoCluster", parameters);
                    }
                }
            });
        }

        private async void ShowInfoMessageDetailBAP(string content)
        {
            var title = MobileResource.Common_Message_Warning;
            await popupServices.ShowNotificatonPopup(title, content);
        }

        public void GotoCameraPage(VehicleOnlineViewModel selected)
        {
            SafeExecute(async () =>
            {
                if (CheckVehcleHasImage(selected.VehiclePlate) || CheckVehcleHasIsQcvn31(selected.VehiclePlate))
                {
                    var param = _mapper.MapProperties<Vehicle>(selected);
                    var parameters = new NavigationParameters
                    {
                        { ParameterKey.Vehicle, param }
                    };

                    await NavigationService.NavigateAsync("NavigationPage/ImageManagingPage", parameters, true, true);
                }
                else
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        var action = await PageDialog.DisplayAlertAsync("Thông báo",
                              string.Format("Tính năng này không được hỗ trợ. Vì Xe {0} sử dụng gói cước không tích hợp tính năng hình ảnh. \nQuý khách vui liên hệ tới số {1} để được hỗ trợ",
                              selected.PrivateCode, MobileSettingHelper.HotlineGps),
                              "Liên hệ", "Bỏ qua");
                        if (action)
                        {
                            PhoneDialer.Open(MobileSettingHelper.HotlineGps);
                        }
                    });
                }
            });
        }

        public void GotoVideoPage(VehicleOnlineViewModel selected)
        {
            SafeExecute(async () =>
            {
                if (CheckVehcleHasVideo(selected.VehiclePlate))
                {
                    var photoPermission = await PermissionHelper.CheckPhotoPermissions();
                    var storagePermission = await PermissionHelper.CheckStoragePermissions();
                    if (photoPermission && storagePermission)
                    {
                        var param = _mapper.MapProperties<CameraLookUpVehicleModel>(selected);
                        var parameters = new NavigationParameters
                      {
                          { ParameterKey.Vehicle, param }
                     };

                        var a = await NavigationService.NavigateAsync("NavigationPage/CameraManagingPage", parameters, true, true);
                    }
                }
                else
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        var action = await PageDialog.DisplayAlertAsync("Thông báo",
                              string.Format("Tính năng này không được hỗ trợ. Vì Xe {0} sử dụng gói cước không tích hợp tính năng video. \nQuý khách vui liên hệ tới số {1} để được hỗ trợ",
                              selected.PrivateCode, MobileSettingHelper.HotlineGps),
                              "Liên hệ", "Bỏ qua");
                        if (action)
                        {
                            PhoneDialer.Open(MobileSettingHelper.HotlineGps);
                        }
                    });
                }
            });
        }

        public void GotoVideoPlaybackPage(VehicleOnlineViewModel selected)
        {
            SafeExecute(async () =>
            {
                if (CheckVehcleHasVideo(selected.VehiclePlate))
                {
                    var photoPermission = await PermissionHelper.CheckPhotoPermissions();
                    var storagePermission = await PermissionHelper.CheckStoragePermissions();
                    if (photoPermission && storagePermission)
                    {
                        var param = _mapper.MapProperties<CameraLookUpVehicleModel>(selected);
                        var parameters = new NavigationParameters
                      {
                          { ParameterKey.Vehicle, param }
                     };

                        var a = await NavigationService.NavigateAsync("NavigationPage/CameraRestream", parameters, true, true);
                    }
                }
                else
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        var action = await PageDialog.DisplayAlertAsync("Thông báo",
                              string.Format("Tính năng này không được hỗ trợ. Vì Xe {0} sử dụng gói cước không tích hợp tính năng video. \nQuý khách vui liên hệ tới số {1} để được hỗ trợ",
                              selected.PrivateCode, MobileSettingHelper.HotlineGps),
                              "Liên hệ", "Bỏ qua");
                        if (action)
                        {
                            PhoneDialer.Open(MobileSettingHelper.HotlineGps);
                        }
                    });
                }
            });
        }

        public void GotoExportVideoPage(VehicleOnlineViewModel selected)
        {
            SafeExecute(async () =>
            {
                if (CheckVehcleHasVideo(selected.VehiclePlate))
                {
                    var param = _mapper.MapProperties<CameraLookUpVehicleModel>(selected);
                    var parameters = new NavigationParameters
                      {
                          { ParameterKey.Vehicle, param }
                     };

                    var a = await NavigationService.NavigateAsync("BaseNavigationPage/ExportVideoPage", parameters, true, true);
                }
                else
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        var action = await PageDialog.DisplayAlertAsync("Thông báo",
                              string.Format("Tính năng này không được hỗ trợ. Vì Xe {0} sử dụng gói cước không tích hợp tính năng video. \nQuý khách vui liên hệ tới số {1} để được hỗ trợ",
                              selected.PrivateCode, MobileSettingHelper.HotlineGps),
                              "Liên hệ", "Bỏ qua");
                        if (action)
                        {
                            PhoneDialer.Open(MobileSettingHelper.HotlineGps);
                        }
                    });
                }
            });
        }

        public void GoToPageAction(MenuKeyType menuTupe)
        {
            switch (menuTupe)
            {
                case MenuKeyType.Route:
                    GoRoutePage(currentVehicle);
                    break;

                case MenuKeyType.VehicleDetail:
                    GoDetailPage(currentVehicle);
                    break;

                case MenuKeyType.Images:
                    GotoCameraPage(currentVehicle);
                    break;

                case MenuKeyType.Video:
                    GotoVideoPage(currentVehicle);
                    break;

                case MenuKeyType.VideoPlayback:
                    GotoExportVideoPage(currentVehicle);
                    break;

                case MenuKeyType.Online:
                    GoOnlinePage(currentVehicle);
                    break;

                case MenuKeyType.HelpCustomer:
                    break;

                default:
                    break;
            }
        }

        public async void OnNavigateMenu(string menuKey)
        {
            switch (menuKey)
            {
                case "ListVehiclePage":
                    await NavigationService.SelectTabAsync("ListVehiclePage");
                    break;

                case "OnlinePage":
                    if (MobileUserSettingHelper.EnableShowCluster)
                    {
                        await NavigationService.SelectTabAsync("OnlinePage");
                    }
                    else
                    {
                        await NavigationService.SelectTabAsync("OnlinePageNoCluster");
                    }
                    break;

                case "RoutePage":
                    await NavigationService.SelectTabAsync("RoutePage");
                    break;

                case "CameraManagingPage":
                    GotoVideoPage(currentVehicle);
                    break;

                case "CameraRestream":
                    GotoVideoPlaybackPage(currentVehicle);
                    break;

                default:
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        SafeExecute(async () =>
                        {
                            using (new HUDService(MobileResource.Common_Message_Processing))
                            {
                                var param = _mapper.MapProperties<Vehicle>(currentVehicle);
                                var parameters = new NavigationParameters
                                  {
                                      { ParameterKey.VehicleRoute, param }
                                 };
                                var a = await NavigationService.NavigateAsync("NavigationPage/" + menuKey, parameters, useModalNavigation: true, true);
                            }
                        });
                    });
                    break;
            }
        }

        #endregion Navigation
    }
}