using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.RequestEntity;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;

using Prism.Commands;
using Prism.Navigation;

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Forms.Extensions;

namespace BA_MobileGPS.Core.ViewModels
{
    public class ListCameraVehicleViewModel : ViewModelBase
    {
        private readonly ICameraService cameraService;
        private readonly IDisplayMessage displayMessage;
        public DelegateCommand<object> LoadMoreItemsCommand { get; set; }

        public DelegateCommand SelectVehicleImageCommand { get; private set; }
        public DelegateCommand HelpImageCommand { get; private set; }

        //thời gian bắt đầu
        private TimeSpan timeFrom = TimeSpan.Zero;

        public TimeSpan TimeFrom { get => timeFrom; set => SetProperty(ref timeFrom, value); }

        //thời gian kết thúc
        private TimeSpan timeEnd = new TimeSpan(23, 59, 59);

        public TimeSpan TimeEnd { get => timeEnd; set => SetProperty(ref timeEnd, value); }

        //trong ngày
        private DateTime dayBefore = DateTime.Now;

        public DateTime DayBefore { get => dayBefore; set => SetProperty(ref dayBefore, value); }
        public virtual int CountMinutesShowMessageReport { get; set; } = MobileSettingHelper.ConfigCountMinutesShowMessageReport;
        //xe

        private int _pagedNext = MobileSettingHelper.ConfigPageNextReport;

        public virtual int PagedNext
        {
            get { return _pagedNext; }
            set { SetProperty(ref _pagedNext, value); }
        }

        public virtual int PageSize { get; set; } = MobileSettingHelper.ConfigPageSizeReport;
        private string vehiclePlate;
        public string VehiclePlate { get => vehiclePlate; set => SetProperty(ref vehiclePlate, value); }
        private Vehicle vehicleSelected;
        public Vehicle VehicleSelected { get => vehicleSelected; set => SetProperty(ref vehicleSelected, value); }

        private int toTal;
        public int Total { get => toTal; set => SetProperty(ref toTal, value); }

        private ObservableCollection<CaptureImageData> listCameraImage = new ObservableCollection<CaptureImageData>();

        public ObservableCollection<CaptureImageData> ListCameraImage
        {
            get { return listCameraImage; }
            set
            {
                listCameraImage = value;

                RaisePropertyChanged(() => ListCameraImage);
            }
        }

        public ICommand SearchCameraCommand { get; }
        public ICommand ViewCameraDetailCommand { get; }

        public ListCameraVehicleViewModel(INavigationService navigationService,
            ICameraService cameraService, IDisplayMessage displayMessage) : base(navigationService)
        {
            this.cameraService = cameraService;
            this.displayMessage = displayMessage;
            SearchCameraCommand = new Command(SearchCamera);
            ViewCameraDetailCommand = new Command<CaptureImageData>(ViewCameraDetail);
            LoadMoreItemsCommand = new DelegateCommand<object>(LoadMoreItems, CanLoadMoreItems);
            SelectVehicleImageCommand = new DelegateCommand(SelectVehicleImage);
            HelpImageCommand = new DelegateCommand(HelpImage);
        }

        public int TotalCount { get; set; } = 0;
        public int PageIndex { get; set; } = 1;

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey(ParameterKey.Vehicle) && parameters.GetValue<Vehicle>(ParameterKey.Vehicle) is Vehicle vehiclePlate)
            {
                ValidateVehicleCamera(vehiclePlate);

                GetListCamera(true);
            }
        }

        private void ValidateVehicleCamera(Vehicle vehicle)
        {
            var listVehicleCamera = StaticSettings.ListVehilceCamera;
            if (listVehicleCamera != null)
            {
                var model = StaticSettings.ListVehilceCamera.FirstOrDefault(x => x.VehiclePlate == vehicle.VehiclePlate + "_C");
                if (model != null)
                {
                    VehiclePlate = model.VehiclePlate;
                    VehicleSelected = new Vehicle
                    {
                        VehicleId = model.VehicleId,
                        VehiclePlate = model.VehiclePlate,
                        PrivateCode = model.VehiclePlate,
                        Imei = model.Imei
                    };
                }
                else
                {
                    VehiclePlate = vehicle.VehiclePlate;
                    VehicleSelected = vehicle;
                }
            }
        }

        private bool CanLoadMoreItems(object obj)
        {
            if (ListCameraImage.Count >= TotalCount || ListCameraImage.Count < PageSize)
                return false;
            return true;
        }

        private void LoadMoreItems(object obj)
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;
            var listview = obj as Syncfusion.ListView.XForms.SfListView;
            listview.IsBusy = true;
            try
            {
                PageIndex++;
                GetListCamera();
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

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(VehiclePlate))
            {
                DisplayMessage.ShowMessageInfo(MobileResource.Common_Message_NoSelectVehiclePlate, CountMinutesShowMessageReport);
                return false;
            }
            if (TimeFrom > TimeEnd)
            {
                DisplayMessage.ShowMessageInfo(MobileResource.Common_Message_ErrorTimeFromToTimeEnd, CountMinutesShowMessageReport);
                return false;
            }

            return true;
        }

        //hàm lấy danh sách ảnh
        private void GetListCamera(bool isSearch = false)
        {
            if (!IsConnected || !ValidateInput())
            {
                return;
            }
            if (isSearch)
            {
                DependencyService.Get<IHUDProvider>().DisplayProgress("");
            }
            RunOnBackground(async () =>
            {
                var request = new CameraImageRequest
                {
                    VehiclePlate = VehiclePlate,
                    XNCode = UserInfo.XNCode,
                    PageIndex = PageIndex,
                    PageSize = PageSize,
                    FromDate = new DateTime(DayBefore.Year, DayBefore.Month, DayBefore.Day, TimeFrom.Hours, TimeFrom.Minutes, TimeFrom.Seconds),
                    ToDate = new DateTime(DayBefore.Year, DayBefore.Month, DayBefore.Day, TimeEnd.Hours, TimeEnd.Minutes, TimeEnd.Seconds)
                };
                return await cameraService.GetListCaptureImage(request);
            }, (result) =>
            {
                if (isSearch)
                {
                    DependencyService.Get<IHUDProvider>().Dismiss();
                }
                if (result != null && result.Count > 0)
                {
                    if (result != null)
                    {
                        TotalCount = result.Count();

                        if (isSearch)
                        {
                            ListCameraImage = result.ToObservableCollection();
                        }
                        else
                        {
                            ListCameraImage.AddRange(result);
                        }
                    }
                    Total = result.Count;
                }
                else
                {
                    Total = 0;
                    ListCameraImage = new ObservableCollection<CaptureImageData>();
                }
            });
        }

        private void SearchCamera()
        {
            PageIndex = 1;
            GetListCamera(true);
        }

        //Lấy chi tiết hình ảnh
        private void ViewCameraDetail(CaptureImageData camera)
        {
            SafeExecute(async () =>
            {
                var parameters = new NavigationParameters
                {
                    { ParameterKey.ListImageCamera, ListCameraImage.ToList() },
                    { ParameterKey.ImageCamera, camera },
                    { ParameterKey.VehiclePlate, VehiclePlate }
                };

                await NavigationService.NavigateAsync("CameraDetail", parameters, useModalNavigation: false, true);
            });
        }

        private void SelectVehicleImage()
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("BaseNavigationPage/VehicleLookUp", animated: true, useModalNavigation: true, parameters: new NavigationParameters
                        {
                            { ParameterKey.VehicleLookUpType, VehicleLookUpType.VehicleImage },
                            {  ParameterKey.VehicleGroupsSelected, VehicleGroups},
                            {  ParameterKey.VehicleStatusSelected, ListVehicleStatus}
                        });
            });
        }

        private void HelpImage()
        {
            SafeExecute(async () =>
            {
                await PageDialog.DisplayAlertAsync("Thông báo", "Các xe sử dụng gói cước không tích hợp tính năng xem hình ảnh sẽ không được hiển thị trên tính năng này", "Bỏ qua");
            });
        }
    }
}