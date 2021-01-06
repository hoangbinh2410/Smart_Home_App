using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service.IService;
using BA_MobileGPS.Utilities;
using Prism.Commands;
using Prism.Navigation;
using Syncfusion.ListView.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;

namespace BA_MobileGPS.Core.ViewModels
{
    public class CameraTimeChartViewModel : ViewModelBase
    {
        #region internal variable

        protected int pageIndex { get; set; } = 0;
        protected int pageCount { get; } = 5;
        private readonly IStreamCameraService cameraService;
        private List<RestreamChartData> ChartItemsSourceOrigin { get; set; } = new List<RestreamChartData>();

        #endregion internal variable

        public CameraTimeChartViewModel(INavigationService navigationService,
            IStreamCameraService cameraService) : base(navigationService)
        {
            this.cameraService = cameraService;
            LoadMoreItemsCommand = new DelegateCommand<object>(LoadMoreItems, CanLoadMoreItems);
            GotoResreamTabCommand = new DelegateCommand<object>(GotoResreamTab);
            chartItemsSource = new ObservableCollection<RestreamChartData>();
            PushToFromDatePageCommand = new DelegateCommand(ExecuteToFromDate);
            SelectVehicleCameraCommand = new DelegateCommand(SelectVehicleCamera);
            selectedDate = DateTime.Now.Date;
            MaxTime = selectedDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            MinTime = selectedDate.Date;
        }

        #region life cycle

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            EventAggregator.GetEvent<SelectDateEvent>().Subscribe(UpdateDateTime);
            IsBusy = true;
        }

        public override void OnDestroy()
        {
            EventAggregator.GetEvent<SelectDateEvent>().Unsubscribe(UpdateDateTime);
            base.OnDestroy();
        }

        public override void OnPageAppearingFirstTime()
        {
            base.OnPageAppearingFirstTime();
            GetAllChartData();
            DisplayMessage.ShowMessageInfo("Thiết bị không ghi video khi phương tiện tắt máy.");
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey(ParameterKey.ListVehicleSelected)
                           && parameters.GetValue<List<CameraLookUpVehicleModel>>(ParameterKey.ListVehicleSelected) is List<CameraLookUpVehicleModel> list)
            {
                var listVehiclePlate = new List<string>();
                foreach (var item in list)
                {
                    listVehiclePlate.Add(item.VehiclePlate);
                }
                SelectedVehiclePlates = string.Join(", ", listVehiclePlate);
                GetChartData(selectedVehiclePlates);
            }
        }

        #endregion life cycle

        #region Binding

        public ICommand SelectVehicleCameraCommand { get; }
        public ICommand LoadMoreItemsCommand { get; }
        public ICommand GotoResreamTabCommand { get; }
        public ICommand PushToFromDatePageCommand { get; }

        private string selectedVehiclePlates;
        /// <summary>
        /// Danh sách xe được chọn ở entry tìm kiếm xe
        /// </summary>
        public string SelectedVehiclePlates
        {
            get { return selectedVehiclePlates; }
            set { SetProperty(ref selectedVehiclePlates, value); }
        }

        private ObservableCollection<RestreamChartData> chartItemsSource;
        /// <summary>
        /// Source danh sách biểu đồ
        /// </summary>
        public ObservableCollection<RestreamChartData> ChartItemsSource
        {
            get { return chartItemsSource; }
            set { SetProperty(ref chartItemsSource, value); }
        }

        private DateTime selectedDate;
        /// <summary>
        /// Ngày được chọn ở ô chọn ngày
        /// </summary>
        public virtual DateTime SelectedDate { get => selectedDate; set => SetProperty(ref selectedDate, value); }

        private DateTime maxTime;
        /// <summary>
        /// Thời gian tối đa của dữ liệu trên 1 biểu đồ,
        /// hỗ trợ hiển thị theo interval trục y
        /// </summary>
        public DateTime MaxTime
        {
            get { return maxTime; }
            set { SetProperty(ref maxTime, value); }
        }

        private DateTime minTime;
        /// <summary>
        /// Thời gian tối thiểu của dữ liệu trên 1 biểu đồ,
        /// hỗ trợ hiển thị theo interval trục y
        /// </summary>
        public DateTime MinTime
        {
            get { return minTime; }
            set { SetProperty(ref minTime, value); }
        }

        #endregion Binding

        #region function
        /// <summary>
        /// Load dữ liệu tất cả biểu dồ của xe có trên hệ thống, infinite scroll
        /// </summary>
        private void GetAllChartData()
        {
            if (StaticSettings.ListVehilceCamera != null && StaticSettings.ListVehilceCamera.Count > 0)
            {
                var vehicleString = GetVehiclesHaveCamera(StaticSettings.ListVehilceCamera);

                GetChartData(vehicleString);
            }
            else
            {
                RunOnBackground(async () =>
                {
                    return await cameraService.GetListVehicleCamera(UserInfo.XNCode);
                },
                (lst) =>
                {
                    if (lst?.Data?.Count > 0)
                    {
                        StaticSettings.ListVehilceCamera = lst.Data;
                        var vehicleString = GetVehiclesHaveCamera(lst.Data);
                        GetChartData(vehicleString);
                    }
                });
            }
        }
        /// <summary>
        /// Kiểm tra xe có cam, dựa vào so sánh với pnc
        /// </summary>
        /// <param name="lstcamera">danh sách xe từ pnc</param>
        /// StaticSettings.ListVehilceOnline : danh sách xe online, init từ trang online (code behind)
        /// <returns></returns>
        private string GetVehiclesHaveCamera(List<StreamDevices> lstcamera)
        {
            var listVehicles = (from a in lstcamera
                                join b in StaticSettings.ListVehilceOnline on a.VehiclePlate.ToUpper() equals b.VehiclePlate.ToUpper()
                                select b.VehiclePlate).ToList();

            return string.Join(",", listVehicles);
        }
        /// <summary>
        /// Lấy dữ liệu biểu đồ theo chuỗi biển số
        /// </summary>
        /// <param name="vehicleString">chuỗi biển số join(',')</param>
        private void GetChartData(string vehicleString)
        {
            ChartItemsSourceOrigin.Clear();

            vehicleString = vehicleString.Replace(" ", string.Empty);
            if (!IsBusy)
            {
                IsBusy = true;
            }
            if (ChartItemsSource.Count > 0)
            {
                ChartItemsSource.Clear();
            }

            pageIndex = 0;

            var request = new CameraRestreamRequest()
            {
                CustomerId = UserInfo.XNCode,
                VehicleNames = vehicleString,
                Date = selectedDate
            };
            RunOnBackground(async () =>
            {
                return await cameraService.GetVehiclesChartDataByDate(request);
            }, (res) =>
            {
                if (res != null && res.Count > 0)
                {
                    foreach (var item in res)
                    {
                        if (item.DeviceTimes == null || item.DeviceTimes.Count == 0)
                        {
                            item.DeviceTimes = FixEmptyData();
                        }
                        if (item.CloudTimes == null || item.CloudTimes.Count == 0)
                        {
                            item.CloudTimes = FixEmptyData();
                        }
                        item.DeviceTimes.Sort((y, x) => x.Channel.CompareTo(y.Channel));
                        item.CloudTimes.Sort((y, x) => x.Channel.CompareTo(y.Channel));
                    }
                    res.Sort((x, y) => string.Compare(x.VehiclePlate, y.VehiclePlate));
                    ChartItemsSourceOrigin = res;
                    LoadMore();
                }
                IsBusy = false;
            });

        }
        /// <summary>
        /// Lỗi init UI biểu đồ nếu trục X không có giá trị
        /// </summary>
        /// <returns></returns>
        private List<AppVideoTimeInfor> FixEmptyData()
        {
            return new List<AppVideoTimeInfor>()
            {
                new AppVideoTimeInfor()
                {
                   Channel = 1
                },
                 new AppVideoTimeInfor()
                {
                   Channel = 2
                }
            };
        }
        /// <summary>
        /// infinite scroll
        /// </summary>
        /// <param name="obj">listview</param>
        private void LoadMoreItems(object obj)
        {
            if (IsBusy)
            {
                return;
            }
            var listview = obj as Syncfusion.ListView.XForms.SfListView;
            listview.IsBusy = true;
            try
            {
                LoadMore();
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            finally
            {
                listview.IsBusy = false;
                IsBusy = false;
            }
        }
        /// <summary>
        /// Command canexcute
        /// </summary>
        /// <param name="obj">listview</param>
        /// <returns></returns>
        private bool CanLoadMoreItems(object obj)
        {
            if (ChartItemsSourceOrigin.Count <= pageIndex * pageCount)
                return false;
            return true;
        }

        private void LoadMore()
        {
            try
            {
                var source = ChartItemsSourceOrigin.Skip(pageIndex * pageCount).Take(pageCount).ToList();
                pageIndex++;
                if (source != null && source.Count() > 0)
                {
                    for (int i = 0; i < source.Count; i++)
                    {
                        ChartItemsSource.Add(source[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }

        }
        /// <summary>
        /// Mở popup chọn ngày
        /// </summary>
        public virtual void ExecuteToFromDate()
        {
            TryExecute(async () =>
            {
                var parameters = new NavigationParameters
                {
                    { "DataPicker", SelectedDate },
                    { "PickerType", ComboboxType.First }
                };
                await NavigationService.NavigateAsync("SelectDateCalendar", parameters);
            });
        }
        /// <summary>
        /// Dữ liệu ngày chọn trả về
        /// </summary>
        /// <param name="param">Ngày chọn</param>
        public virtual void UpdateDateTime(PickerDateResponse param)
        {
            try
            {
                if (param != null)
                {
                    SelectedDate = param.Value;
                    MaxTime = selectedDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
                    MinTime = selectedDate.Date;
                    if (!string.IsNullOrEmpty(SelectedVehiclePlates))
                    {
                        GetChartData(SelectedVehiclePlates);
                    }
                    else
                    {
                        if (StaticSettings.ListVehilceCamera != null && StaticSettings.ListVehilceCamera.Count > 0)
                        {
                            var vehicleString = GetVehiclesHaveCamera(StaticSettings.ListVehilceCamera);

                            GetChartData(vehicleString);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }
        /// <summary>
        /// CHọn xe từ dánh sách, chọn nhiều
        /// </summary>
        private void SelectVehicleCamera()
        {
            var param = new NavigationParameters();
            param.Add(ParameterKey.ListVehicleSelected, SelectedVehiclePlates);
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("BaseNavigationPage/VehicleCameraMultiSelect", param, useModalNavigation: true, animated: true);
            });
        }
        /// <summary>
        /// Chuyển qua trang xem lại
        /// </summary>
        /// <param name="obj">dữ liệu row ở listview</param>
        private void GotoResreamTab(object obj)
        {
            try
            {
                if (obj is ItemTappedEventArgs even)
                {
                    var item = (RestreamChartData)even.ItemData;
                    var vehicle = StaticSettings.ListVehilceOnline.FirstOrDefault(x => x.VehiclePlate == item.VehiclePlate);
                    if (vehicle != null)
                    {
                        var chanels = StaticSettings.ListVehilceCamera
                                                    .FirstOrDefault(x => x.VehiclePlate == item.VehiclePlate)?
                                                    .CameraChannels?
                                                    .Select(y => y.Channel)
                                                    .ToList();

                        var vehicleModel = new CameraLookUpVehicleModel()
                        {
                            VehiclePlate = item.VehiclePlate,
                            VehicleId = vehicle.VehicleId,
                            PrivateCode = vehicle.PrivateCode,
                            CameraChannels = chanels != null ? chanels : new List<int>()
                        };
                        var param = new NavigationParameters()
                {
                    {ParameterKey.SelectDate,selectedDate },
                    {ParameterKey.VehiclePlate,vehicleModel }
                };
                        SafeExecute(async () =>
                        {
                            var a = await NavigationService.NavigateAsync("CameraRestream", param);
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }

        }


        #endregion function
    }
}