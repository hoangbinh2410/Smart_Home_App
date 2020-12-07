using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service.IService;
using BA_MobileGPS.Utilities;
using Prism.Commands;
using Prism.Navigation;
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

        public string SelectedVehiclePlates
        {
            get { return selectedVehiclePlates; }
            set { SetProperty(ref selectedVehiclePlates, value); }
        }

        private ObservableCollection<RestreamChartData> chartItemsSource;

        public ObservableCollection<RestreamChartData> ChartItemsSource
        {
            get { return chartItemsSource; }
            set { SetProperty(ref chartItemsSource, value); }
        }

        private DateTime selectedDate;
        public virtual DateTime SelectedDate { get => selectedDate; set => SetProperty(ref selectedDate, value); }

        private DateTime maxTime;
        public DateTime MaxTime
        {
            get { return maxTime; }
            set { SetProperty(ref maxTime, value); }
        }

        private DateTime minTime;
        public DateTime MinTime
        {
            get { return minTime; }
            set { SetProperty(ref minTime, value); }
        }

        #endregion Binding

        #region function

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

        private string GetVehiclesHaveCamera(List<StreamDevices> lstcamera)
        {
            var listVehicles = (from a in lstcamera
                                join b in StaticSettings.ListVehilceOnline on a.VehiclePlate.ToUpper() equals b.VehiclePlate.ToUpper()
                                select b.VehiclePlate).ToList();

            return string.Join(",", listVehicles);
        }

        private void GetChartData(string vehicleString)
        {
            ChartItemsSourceOrigin.Clear();
            if (Validate())
            {
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
        }

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

        private void LoadMoreItems(object obj)
        {
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

        private bool CanLoadMoreItems(object obj)
        {
            if (ChartItemsSourceOrigin.Count <= pageIndex * pageCount)
                return false;
            return true;
        }

        private void LoadMore()
        {
            var source = ChartItemsSourceOrigin.Skip(pageIndex * pageCount).Take(pageCount);
            pageIndex++;
            if (source != null && source.Count() > 0)
            {
                foreach (var item in source)
                {
                    ChartItemsSource.Add(item);
                }
            }
        }

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

        public virtual void UpdateDateTime(PickerDateResponse param)
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

        private void SelectVehicleCamera()
        {
            var param = new NavigationParameters();
            param.Add(ParameterKey.ListVehicleSelected, SelectedVehiclePlates);
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("BaseNavigationPage/VehicleCameraMultiSelect", param, useModalNavigation: true, animated: true);
            });
        }

        private void GotoResreamTab(object obj)
        {
            var item = (RestreamChartData)obj;
            var vehicle = StaticSettings.ListVehilceOnline.FirstOrDefault(x => x.VehiclePlate == item.VehiclePlate);
            if (vehicle != null)
            {
                var vehicleModel = new CameraLookUpVehicleModel()
                {
                    VehiclePlate = item.VehiclePlate,
                    VehicleId = vehicle.VehicleId,
                    PrivateCode = vehicle.PrivateCode
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

        private bool Validate()
        {
            //Ngày
            if (selectedDate != null)
            {
                var maxDay = new TimeSpan(7, 0, 0, 0, 0);
                if (DateTime.Now.Date - selectedDate.Date <= maxDay)
                {
                    return true;
                }
                else
                {
                    DisplayMessage.ShowMessageInfo("Chỉ cho phép chọn từ ngày hiện tại lùi lại 7 ngày");
                }
            }
            return false;
        }

        #endregion function
    }
}