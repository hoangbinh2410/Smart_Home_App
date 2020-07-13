﻿using AutoMapper;

using BA_MobileGPS.Core;
using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Extensions;
using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Core.Interfaces;
using BA_MobileGPS.Core.Models;
using BA_MobileGPS.Core.Resource;
using BA_MobileGPS.Core.ViewModels;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.ModelViews;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;

using Prism.Commands;
using Prism.Navigation;

using Syncfusion.Data.Extensions;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace VMS_MobileGPS.ViewModels
{
    public enum SortOrder
    {
        PrivateCodeASC,
        PrivateCodeDES,
        TimeASC,
        TimeDES,
        DefaultASC,
        DefaultDES
    }

    public class ListVehiclePageViewModel : ViewModelBase
    {
        private CancellationTokenSource cts;



        public ICommand ShowHelpCommand { get; private set; }
        public ICommand ChangeSortCommand { get; private set; }
        public ICommand SearchVehicleCommand { get; private set; }
        public ICommand TapListVehicleCommand { get; private set; }
        public ICommand SelectStatusVehicleCommand { get; private set; }

        private readonly IMapper _mapper;
        private readonly IVehicleOnlineService vehicleOnlineService;
        private readonly IPopupServices popupServices;
        public ListVehiclePageViewModel(INavigationService navigationService, IMapper mapper, IVehicleOnlineService vehicleOnlineService, IPopupServices popupServices)
            : base(navigationService)
        {
            this._mapper = mapper;
            this.vehicleOnlineService = vehicleOnlineService;
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

            if (parameters?.GetValue<Company>(ParameterKey.Company) is Company _)
            {
                UpdateVehicleByCompany();
            }
            else if (parameters?.GetValue<int[]>(ParameterKey.VehicleGroups) is int[] vehiclegroup)
            {
                VehicleGroups = vehiclegroup;

                UpdateVehicleByVehicleGroup(vehiclegroup);
            }
            else if (companyChanged)
            {
                UpdateVehicleByCompany();

                companyChanged = false;
            }
        }
        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            GetListVehicleOnline();

        }
        public override void OnDestroy()
        {
            EventAggregator.GetEvent<ReceiveSendCarEvent>().Unsubscribe(OnReceiveSendCarSignalR);
            EventAggregator.GetEvent<OnReloadVehicleOnline>().Unsubscribe(OnReLoadVehicleOnlineCarSignalR);
            EventAggregator.GetEvent<SelectedCompanyEvent>().Unsubscribe(OnCompanyChanged);
        }
        #endregion

        #region Property
        public SortOrder sortOrder = SortOrder.DefaultDES;
        public SortOrder SortOrder { get => sortOrder; set => SetProperty(ref sortOrder, value); }

        // Danh sách xe gốc chưa lọc
        private List<VMSVehicleOnlineViewModel> ListVehicleOrigin
        {
            get
            {
                if (StaticSettings.ListVehilceOnline != null)
                {
                    if (SortOrder == SortOrder.PrivateCodeASC)
                        return _mapper.Map<List<VMSVehicleOnlineViewModel>>(StaticSettings.ListVehilceOnline.OrderByDescending(x => x.SortOrder).OrderBy(x => x.PrivateCode));
                    else if (SortOrder == SortOrder.PrivateCodeDES)
                        return _mapper.Map<List<VMSVehicleOnlineViewModel>>(StaticSettings.ListVehilceOnline.OrderByDescending(x => x.SortOrder).OrderByDescending(x => x.PrivateCode));
                    else if (SortOrder == SortOrder.TimeASC)
                        return _mapper.Map<List<VMSVehicleOnlineViewModel>>(StaticSettings.ListVehilceOnline.OrderByDescending(x => x.SortOrder).OrderBy(x => x.VehicleTime));
                    else if (SortOrder == SortOrder.TimeDES)
                        return _mapper.Map<List<VMSVehicleOnlineViewModel>>(StaticSettings.ListVehilceOnline.OrderByDescending(x => x.SortOrder).OrderByDescending(x => x.VehicleTime));
                    else if (SortOrder == SortOrder.DefaultASC)
                        return _mapper.Map<List<VMSVehicleOnlineViewModel>>(StaticSettings.ListVehilceOnline.OrderBy(x => x.SortOrder));
                    else
                        return _mapper.Map<List<VMSVehicleOnlineViewModel>>(StaticSettings.ListVehilceOnline.OrderByDescending(x => x.SortOrder));
                }
                else
                {
                    return new List<VMSVehicleOnlineViewModel>();
                }
            }
        }

        // Danh sách xe đã được lọc theo nhóm, công ty, trạng thái
        private List<VMSVehicleOnlineViewModel> ListVehicleByGroup
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
                    return new List<VMSVehicleOnlineViewModel>();
                }
            }
        }

        private List<VMSVehicleOnlineViewModel> ListVehicleByStatus;

        private bool companyChanged;

        public int countVehicle;
        public int CountVehicle { get => countVehicle; set => SetProperty(ref countVehicle, value); }

        private VehicleStatusViewModel selectedStatus;
        public VehicleStatusViewModel SelectedStatus { get => selectedStatus; set => SetProperty(ref selectedStatus, value); }

        private ObservableCollection<VehicleStatusViewModel> listVehilceStatus;
        public ObservableCollection<VehicleStatusViewModel> ListVehilceStatus { get => listVehilceStatus; set => SetProperty(ref listVehilceStatus, value); }

        private ObservableCollection<VMSVehicleOnlineViewModel> listVehicle = new ObservableCollection<VMSVehicleOnlineViewModel>();
        public ObservableCollection<VMSVehicleOnlineViewModel> ListVehicle { get => listVehicle; set => SetProperty(ref listVehicle, value); }

        public string searchedText;
        public string SearchedText { get => searchedText; set => SetProperty(ref searchedText, value); }

        #endregion



        #region Private Method

        private void OnReLoadVehicleOnlineCarSignalR(bool arg)
        {
            if (arg)
            {
                using (new HUDService())
                {
                    InitVehicleList();
                }
            }
        }

        private void OnCompanyChanged(int e)
        {
            companyChanged = true;
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

            // Chạy lại hàm tính toán trạng thái xe
            InitVehicleStatus();
        }

        private void InitVehicleList()
        {
            try
            {
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
            var vehicleList = _mapper.Map<List<VehicleOnline>>(ListVehicle);

            CountVehicle = vehicleList.Count();

            // Lấy trạng thái xe
            List<VehicleStatusViewModel> listStatus = (new VehicleStatusHelper()).DictVehicleStatus.Values.Where(x => x.IsEnable).ToList();
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
                vehicle.StatusEngineer = StateVehicleExtension.EngineState(carInfo);
                vehicle.IconImage = IconCodeHelper.GetMarkerResource(carInfo);
                vehicle.Lat = carInfo.Lat;
                vehicle.Lng = carInfo.Lng;
            }
        }

        private void ShowHelp()
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("ListVehicleHelpPage", useModalNavigation: true);
            });

        }

        private async void ChangeSort()
        {
            var sort = await PageDialog.DisplayActionSheetAsync(MobileResource.ListVehicle_Label_SortBy, MobileResource.Common_Button_Cancel, null,
                MobileResource.ListVehicle_Label_ByVehiclePlate, MobileResource.ListVehicle_Label_ByTime, MobileResource.ListVehicle_Label_Default);

            if (MobileResource.ListVehicle_Label_ByVehiclePlate.Equals(sort))
            {
                if (SortOrder == SortOrder.PrivateCodeASC)
                    SortOrder = SortOrder.PrivateCodeDES;
                else
                    SortOrder = SortOrder.PrivateCodeASC;
            }
            else if (MobileResource.ListVehicle_Label_ByTime.Equals(sort))
            {
                if (SortOrder == SortOrder.TimeDES)
                    SortOrder = SortOrder.TimeASC;
                else
                    SortOrder = SortOrder.TimeDES;
            }
            else if (MobileResource.ListVehicle_Label_Default.Equals(sort))
            {
                if (SortOrder == SortOrder.DefaultDES)
                    SortOrder = SortOrder.DefaultASC;
                else
                    SortOrder = SortOrder.DefaultDES;
            }
            else
            {
                return;
            }

            if (SortOrder == SortOrder.PrivateCodeASC)
                ListVehicle = ListVehicle.OrderByDescending(x => x.SortOrder).OrderBy(x => x.PrivateCode).ToObservableCollection();
            else if (SortOrder == SortOrder.PrivateCodeDES)
                ListVehicle = ListVehicle.OrderByDescending(x => x.SortOrder).OrderByDescending(x => x.PrivateCode).ToObservableCollection();
            else if (SortOrder == SortOrder.TimeASC)
                ListVehicle = ListVehicle.OrderByDescending(x => x.SortOrder).OrderBy(x => x.VehicleTime).ToObservableCollection();
            else if (SortOrder == SortOrder.TimeDES)
                ListVehicle = ListVehicle.OrderByDescending(x => x.SortOrder).OrderByDescending(x => x.VehicleTime).ToObservableCollection();
            else if (SortOrder == SortOrder.DefaultASC)
                ListVehicle = ListVehicle.OrderBy(x => x.SortOrder).ToObservableCollection();
            else
                ListVehicle = ListVehicle.OrderByDescending(x => x.SortOrder).ToObservableCollection();
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
                    ListVehicle = new ObservableCollection<VMSVehicleOnlineViewModel>(task.Result);
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
                List<VehicleOnline> listVehicle = _mapper.Map<List<VehicleOnline>>(ListVehicleByGroup);
                var listFilter = StateVehicleExtension.GetVehicleCarByStatus(listVehicle, (VehicleStatusGroup)selected.ID);

                if (listFilter != null)
                {
                    listFilter.ForEach(x =>
                    {
                        x.IconImage = IconCodeHelper.GetMarkerResource(x);
                    });
                }

                ListVehicleByStatus = _mapper.Map<List<VMSVehicleOnlineViewModel>>(listFilter);
                ListVehicle = _mapper.Map<List<VMSVehicleOnlineViewModel>>(listFilter).ToObservableCollection();
            }
        }

        private void TapListVehicle(Syncfusion.ListView.XForms.ItemTappedEventArgs args)
        {
            TryExecute(async () =>
            {
                if (args.ItemData is VMSVehicleOnlineViewModel selected)
                {
                    //Nếu messageId = 2 hoặc 3 là xe phải thu phí
                    if (StateVehicleExtension.IsVehicleStopService(selected.MessageId))
                    {
                        ShowInfoMessageDetailBAP(selected.MessageDetailBAP);
                        return;
                    }
                    var action = await PageDialog.DisplayActionSheetAsync(MobileResource.ListVehicle_Label_SelectCommand, MobileResource.Common_Button_Close, null,
                           MobileResource.DetailVehicle_Label_TilePage, MobileResource.Online_Label_TitlePage, MobileResource.Route_Label_TitleVMS, MobileResource.Route_Label_DistanceTitle);

                    if (action == MobileResource.DetailVehicle_Label_TilePage)
                    {
                        GoDetailPage(selected);
                    }
                    else if (action == MobileResource.Online_Label_TitlePage)
                    {
                        GoOnlinePage(selected);
                    }
                    else if (action == MobileResource.Route_Label_TitleVMS)
                    {
                        GoRoutePage(selected);
                    }
                    else if (action == MobileResource.Route_Label_DistanceTitle)
                    {
                        GoDistancePage(selected);
                    }
                }
            });
        }

        public void CacularVehicleStatus()
        {
            var vehicleList = _mapper.Map<List<VehicleOnline>>(ListVehicleByGroup);

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
            RunOnBackground(async () =>
            {
                var userID = StaticSettings.User.UserId;
                if (Settings.CurrentCompany != null && Settings.CurrentCompany.FK_CompanyID > 0)
                {
                    userID = Settings.CurrentCompany.UserId;
                }
                int vehicleGroup = 0;
                return await vehicleOnlineService.GetListVehicleOnline(userID, vehicleGroup);
            },
           (result) =>
           {
               if (result != null && result.Count > 0)
               {
                   result.ForEach(x =>
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
                   StaticSettings.ListVehilceOnline = result;

                   InitVehicleList();
               }
               else
               {
                   StaticSettings.ListVehilceOnline = new List<VehicleOnline>();
               }
           });
        }


        #endregion Private Method

        #region Navigation

        public void GoDetailPage(VMSVehicleOnlineViewModel selected)
        {
            SafeExecute(async () =>
            {
                var parameters = new NavigationParameters
                {
                    { ParameterKey.CarDetail, selected }
                };

                await NavigationService.NavigateAsync("BaseNavigationPage/VehicleDetailPage", parameters, true);
            });
        }

        public void GoRoutePage(VMSVehicleOnlineViewModel selected)
        {
            SafeExecute(async () =>
            {
                var param = _mapper.Map<VehicleOnline>(selected);

                await NavigationService.NavigateAsync("RoutePage", new NavigationParameters
                {
                    { ParameterKey.VehicleOnline, param }
                }, false);
            });
        }

        public void GoOnlinePage(VMSVehicleOnlineViewModel selected)
        {
            SafeExecute(async () =>
            {
                var param = _mapper.Map<VehicleOnline>(selected);

                await NavigationService.NavigateAsync("OnlineOneCar", new NavigationParameters
                {
                    { ParameterKey.VehicleOnline, param }
                }, false);
            });
        }

        public void GoDistancePage(VMSVehicleOnlineViewModel selected)
        {
            SafeExecute(async () =>
            {
                var parameters = new NavigationParameters
                {
                    { ParameterKey.VehicleOnline, selected }
                };

                await NavigationService.NavigateAsync("BaseNavigationPage/DistancePage", parameters, true);
            });
        }

        private void ShowInfoMessageDetailBAP(string content)
        {
            SafeExecute(async () =>
            {
                var title = MobileResource.Common_Message_Warning;
                await popupServices.ShowNotificatonPopup(title, content);
            });
        }

        #endregion Navigation
    }
}