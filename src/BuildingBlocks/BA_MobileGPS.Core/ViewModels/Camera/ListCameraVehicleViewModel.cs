using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.RequestEntity;
using BA_MobileGPS.Entities.ResponeEntity.Camera;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;

using Prism.Commands;
using Prism.Navigation;

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
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

        private ObservableCollection<ImageCamera> listCameraImage = new ObservableCollection<ImageCamera>();

        public ObservableCollection<ImageCamera> ListCameraImage
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
            ViewCameraDetailCommand = new Command<ImageCamera>(ViewCameraDetail);
            LoadMoreItemsCommand = new DelegateCommand<object>(LoadMoreItems, CanLoadMoreItems);
        }

        public int TotalCount { get; set; } = 0;
        public int PageIndex { get; set; } = 1;

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey(ParameterKey.VehicleRoute) && parameters.GetValue<Vehicle>(ParameterKey.VehicleRoute) is Vehicle vehiclePlate)
            {
                VehiclePlate = vehiclePlate.VehiclePlate;
                VehicleSelected = vehiclePlate;

                GetListCamera(true);
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
            Task.Run(async () =>
        {
            return await cameraService.GetCamera(new CameraImageRequest
            {
                CompanyID = CurrentComanyID,
                VehiclePlate = VehiclePlate,
                XNCode = UserInfo.XNCode,
                DayBefore = DayBefore,
                TimeFrom = TimeFrom,
                PageIndex = PageIndex,
                PageSize = PageSize,
                TimeEnd = TimeEnd,
                DayOffset = 0
            });
        }).ContinueWith(task => Device.BeginInvokeOnMainThread(() =>
        {
            if (task.Status == TaskStatus.RanToCompletion && task.Result != null)
            {
                try
                {
                    if (isSearch)
                    {
                        DependencyService.Get<IHUDProvider>().Dismiss();
                    }
                    if (task.Result.State)
                    {
                        if (task.Result.ListCameraImage != null)
                        {
                            TotalCount = task.Result.TotalCount;

                            if (isSearch)
                            {
                                ListCameraImage = task.Result.ListCameraImage.ToObservableCollection();
                            }
                            else
                            {
                                ListCameraImage.AddRange(task.Result.ListCameraImage);
                            }
                        }
                        Total = task.Result.TotalCount;
                    }
                    else
                    {
                        switch (task.Result.ErrorCode)
                        {
                            case Utilities.Enums.ResponseEnum.Success:
                                break;

                            case Utilities.Enums.ResponseEnum.NotCreated:
                                break;

                            case Utilities.Enums.ResponseEnum.ParameterInvalid:
                                break;

                            case Utilities.Enums.ResponseEnum.NoData:
                                break;

                            case Utilities.Enums.ResponseEnum.BadRequest:
                                break;

                            case Utilities.Enums.ResponseEnum.Exception:
                                break;

                            case Utilities.Enums.ResponseEnum.LimitDate:
                                displayMessage.ShowMessageInfo(task.Result.Message);
                                break;

                            default:
                                break;
                        }
                        Total = 0;
                        ListCameraImage = new ObservableCollection<ImageCamera>();
                    }
                }
                catch (Exception ex)
                {
                    PageDialog.DisplayAlertAsync("", ex.Message, MobileResource.Common_Button_OK);
                    Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
                }
            }
            else if (task.IsFaulted)
            {
                PageDialog.DisplayAlertAsync("", task.Exception?.Message, MobileResource.Common_Button_OK);
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, task.Exception);
            }
        }));
        }

        private void SearchCamera()
        {
            PageIndex = 1;
            GetListCamera(true);
        }

        //Lấy chi tiết hình ảnh
        private void ViewCameraDetail(ImageCamera camera)
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
    }
}