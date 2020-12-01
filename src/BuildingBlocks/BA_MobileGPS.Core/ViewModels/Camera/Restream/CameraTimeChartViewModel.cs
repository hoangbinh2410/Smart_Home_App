using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service.IService;
using BA_MobileGPS.Utilities;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms.Extensions;

namespace BA_MobileGPS.Core.ViewModels
{
    public class CameraTimeChartViewModel : ViewModelBase
    {
        protected int pageIndex { get; set; } = 0;
        protected int pageCount { get; } = 7;
        private readonly IStreamCameraService cameraService;
        public CameraTimeChartViewModel(INavigationService navigationService,
            IStreamCameraService cameraService) : base(navigationService)
        {
            this.cameraService = cameraService;
            LoadMoreItemsCommand = new DelegateCommand<object>(LoadMoreItems, CanLoadMoreItems);
            GotoResreamTabCommand = new DelegateCommand<object>(GotoResreamTab);
            chartItemsSource = new ObservableCollection<RestreamChartData>();
            PushToFromDatePageCommand = new DelegateCommand(ExecuteToFromDate);
            SelectVehicleCameraCommand = new DelegateCommand(SelectVehicleCamera);
            selectedDate = DateTime.Today;
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            EventAggregator.GetEvent<SelectDateTimeEvent>().Subscribe(UpdateDateTime);
            GetAllChartData();
        }
        public override void OnDestroy()
        {
            EventAggregator.GetEvent<SelectDateTimeEvent>().Unsubscribe(UpdateDateTime);
            base.OnDestroy();
        }

        public override void OnPageAppearingFirstTime()
        {
            base.OnPageAppearingFirstTime();

        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey(ParameterKey.Vehicle) && parameters.GetValue<CameraLookUpVehicleModel>(ParameterKey.Vehicle) is CameraLookUpVehicleModel vehicle)
            {
                Vehicle = vehicle;
                GetChartData(vehicle.VehiclePlate);
            }
        }

        public ICommand SelectVehicleCameraCommand { get; }
        public ICommand LoadMoreItemsCommand { get; }
        public ICommand GotoResreamTabCommand { get; }
        public ICommand PushToFromDatePageCommand { get; }

        private CameraLookUpVehicleModel vehicle = new CameraLookUpVehicleModel();
        public CameraLookUpVehicleModel Vehicle { get => vehicle; set => SetProperty(ref vehicle, value); }
        private List<RestreamChartData> ChartItemsSourceOrigin { get; set; } = new List<RestreamChartData>();
        private ObservableCollection<RestreamChartData> chartItemsSource;
        public ObservableCollection<RestreamChartData> ChartItemsSource
        {
            get { return chartItemsSource; }
            set { SetProperty(ref chartItemsSource, value); }
        }
        private DateTime selectedDate;
        public virtual DateTime SelectedDate { get => selectedDate; set => SetProperty(ref selectedDate, value); }

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
                }
                ChartItemsSourceOrigin = res;
                ChartItemsSource = ChartItemsSourceOrigin.Skip(pageIndex * pageCount).Take(pageCount).ToObservableCollection();
                pageIndex++;
            });
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
            if (ChartItemsSourceOrigin.Count < pageIndex * pageCount)
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
            SafeExecute(async () =>
            {
                var parameters = new NavigationParameters
                {
                    { "DataPicker", SelectedDate },
                    { "PickerType", ComboboxType.First }
                };
                await NavigationService.NavigateAsync("SelectDateCalendar", parameters);
            });
        }

        public virtual void UpdateDateTime(PickerDateTimeResponse param)
        {
            if (param != null)
            {
                SelectedDate = param.Value;
                if (vehicle == null)
                {
                    GetAllChartData();
                }
                else
                {
                    GetChartData(vehicle.VehiclePlate);
                }
            }
        }
        private void SelectVehicleCamera()
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("BaseNavigationPage/VehicleCameraLookup", null, useModalNavigation: true, animated: true);
            });
        }

        private void GotoResreamTab(object obj)
        {

        }
    }
}
