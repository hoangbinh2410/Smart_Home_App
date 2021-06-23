using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service.IService;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Extensions;

namespace BA_MobileGPS.Core.ViewModels
{
    public class MyVideoTabViewModel : ViewModelBase
    {
        public ICommand VideoItemTapCommand { get; set; }
        public ICommand SelectVehicleCameraCommand { get; }

        public ICommand SearchCommand { get; }
        private readonly IStreamCameraService _streamCameraService;

        public MyVideoTabViewModel(INavigationService navigationService, IStreamCameraService streamCameraService) : base(navigationService)
        {
            _streamCameraService = streamCameraService;
            VideoItemTapCommand = new DelegateCommand<VideoUploadInfo>(VideoSelectedChange);
            SelectVehicleCameraCommand = new DelegateCommand(SelectVehicleCamera);
            SearchCommand = new DelegateCommand(SearchData);
            vehicle = new CameraLookUpVehicleModel();
            EventAggregator.GetEvent<UploadVideoEvent>().Subscribe(UploadVideoRestream);
            EventAggregator.GetEvent<UploadFinishVideoEvent>().Subscribe(UploadFinishVideo);
        }

        #region Lifecycle

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            GetListVideoUpload();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey(ParameterKey.Vehicle) && parameters.GetValue<CameraLookUpVehicleModel>(ParameterKey.Vehicle) is CameraLookUpVehicleModel vehicle)
            {
                Vehicle = vehicle;
            }
        }

        public override void OnPageAppearingFirstTime()
        {
            base.OnPageAppearingFirstTime();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            EventAggregator.GetEvent<UploadVideoEvent>().Unsubscribe(UploadVideoRestream);
            EventAggregator.GetEvent<UploadFinishVideoEvent>().Unsubscribe(UploadFinishVideo);
        }

        public override void OnSleep()
        {
            base.OnSleep();
        }

        public override void OnResume()
        {
            base.OnResume();
        }

        #endregion Lifecycle

        #region Property

        private CameraLookUpVehicleModel vehicle = new CameraLookUpVehicleModel();
        public CameraLookUpVehicleModel Vehicle { get => vehicle; set => SetProperty(ref vehicle, value); }

        private DateTime dateStart = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 0, 0, 0);

        public DateTime DateStart
        {
            get => dateStart;
            set => SetProperty(ref dateStart, value);
        }

        private DateTime dateEnd = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 23, 59, 59);

        public DateTime DateEnd
        {
            get => dateEnd;
            set => SetProperty(ref dateEnd, value);
        }

        private ObservableCollection<VideoUpload> listVideoUpload;

        /// <summary>
        /// Source ảnh để chọn video
        /// </summary>
        public ObservableCollection<VideoUpload> ListVideoUpload { get => listVideoUpload; set => SetProperty(ref listVideoUpload, value); }

        private ObservableCollection<VideoUploadInfo> videoItemsSource;

        /// <summary>
        /// Source ảnh để chọn video
        /// </summary>
        public ObservableCollection<VideoUploadInfo> VideoItemsSource { get => videoItemsSource; set => SetProperty(ref videoItemsSource, value); }

        private VideoUploadInfo videoSlected;

        /// <summary>
        /// Ảnh được focus
        /// </summary>
        public VideoUploadInfo VideoSlected
        {
            get => videoSlected;
            set
            {
                SetProperty(ref videoSlected, value);
            }
        }

        #endregion Property

        #region PrivateMethod

        private bool ValidateInput()
        {
            if (dateStart > dateEnd)
            {
                DisplayMessage.ShowMessageInfo("Thời gian bắt đầu phải nhỏ hơn thời gian kết thúc");
                return false;
            }
            else if (Vehicle == null || Vehicle.VehicleId == 0)
            {
                DisplayMessage.ShowMessageInfo(" Vui lòng chọn phương tiện");
                return false;
            }
            else
            {
                return true;
            }
        }

        private void SelectVehicleCamera()
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("BaseNavigationPage/VehicleCameraLookup", null, useModalNavigation: true, animated: true);
            });
        }

        /// <summary>
        /// Lấy danh sách video từ server
        /// </summary>
        private void GetListVideoDataFrom()
        {
            try
            {
                if (ValidateInput())
                {
                    VideoItemsSource = new ObservableCollection<VideoUploadInfo>();
                    RunOnBackground(async () =>
                    {
                        return await _streamCameraService.GetListVideoOnCloud(new Entities.CameraRestreamRequest()
                        {
                            CustomerId = UserInfo.XNCode,
                            Date = DateStart.Date,
                            VehicleNames = Vehicle.VehiclePlate
                        });
                    }, (result) =>
                    {
                        if (result != null && result.Count > 0)
                        {
                            var lstvideoupload = new List<VideoUploadInfo>();
                            foreach (var item in result)
                            {
                                foreach (var item1 in item.Data)
                                {
                                    item1.Channel = item.Channel;
                                    item1.VehicleName = item.VehicleName;
                                    item1.EndTime = item1.StartTime.AddSeconds(item1.Duration);
                                }
                                lstvideoupload.AddRange(item.Data);
                            }
                            if (lstvideoupload != null && lstvideoupload.Count > 0)
                            {
                                var lstvideo = new List<VideoUploadInfo>();
                                var video = lstvideoupload.Where(x => x.StartTime >= DateStart && x.StartTime <= DateEnd).OrderBy(x => x.StartTime).ToList();
                                lstvideo.AddRange(video);
                                VideoItemsSource = lstvideo.ToObservableCollection();
                            }
                        }
                    }, showLoading: true);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        private void GetListVideoUpload()
        {
            if (StaticSettings.ListVideoUpload != null && StaticSettings.ListVideoUpload.Count > 0)
            {
                ListVideoUpload = new ObservableCollection<VideoUpload>(StaticSettings.ListVideoUpload);
            }
            else
            {
                ListVideoUpload = new ObservableCollection<VideoUpload>();
            }
        }

        private void SearchData()
        {
            GetListVideoDataFrom();
        }

        private void VideoSelectedChange(VideoUploadInfo obj)
        {
            if (obj != null)
            {
                SafeExecute(async () =>
                {
                    var parameters = new NavigationParameters
                    {
                        {   ParameterKey.VideoUploaded, obj
                        },
                        {
                            ParameterKey.LstVideoUploaded, VideoItemsSource.ToList()
                        }
                    };
                    _ = await NavigationService.NavigateAsync("BaseNavigationPage/ViewVideoUploadedPage", parameters, useModalNavigation: true, animated: true);
                });
            }
        }

        private void UploadVideoRestream(bool obj)
        {
            GetListVideoUpload();
            Device.StartTimer(TimeSpan.FromSeconds(5), () =>
            {
                //nếu ko còn video nào upload thì ngừng timmer
                if (StaticSettings.ListVideoUpload == null || StaticSettings.ListVideoUpload.Count == 0)
                {
                    return false;
                }
                else
                {
                    GetListVideoUpload();
                    return true;
                }
            });
        }

        private void UploadFinishVideo(bool obj)
        {
            GetListVideoUpload();
        }

        #endregion PrivateMethod
    }
}