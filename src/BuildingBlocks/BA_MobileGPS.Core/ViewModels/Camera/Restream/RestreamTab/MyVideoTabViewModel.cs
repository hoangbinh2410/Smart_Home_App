using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Core.Interfaces;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service.IService;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Extensions;

namespace BA_MobileGPS.Core.ViewModels
{
    public class MyVideoTabViewModel : RestreamChildVMBase
    {
        public ICommand ScreenShotTappedCommand { get; }

        public ICommand SelectVehicleCameraCommand { get; }

        public ICommand SearchCommand { get; }

        /// <summary>
        /// Chụp ảnh màn hình
        /// </summary>
        public ICommand DowloadVideoCommand { get; }

        public ICommand DowloadVideoInListTappedCommand { get; }

        public ICommand PreviousVideoCommand { get; }

        public ICommand NextVideoCommand { get; }

        private readonly IDownloadVideoService _downloadService;

        public MyVideoTabViewModel(INavigationService navigationService,
          IStreamCameraService cameraService,
          IScreenOrientServices screenOrientServices, IDownloadVideoService downloadService) : base(navigationService, cameraService, screenOrientServices)
        {
            _downloadService = downloadService;
            VideoItemTapCommand = new DelegateCommand<VideoUploadInfo>(VideoSelectedChange);
            SelectVehicleCameraCommand = new DelegateCommand(SelectVehicleCamera);
            SearchCommand = new DelegateCommand(SearchData);
            vehicle = new CameraLookUpVehicleModel();
        }

        #region Lifecycle

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
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
        }

        public override void OnSleep()
        {
            base.OnSleep();
        }

        public override void OnResume()
        {
            base.OnResume();
            DependencyService.Get<IScreenOrientServices>().ForcePortrait();

            //if (MediaPlayer != null)
            //{
            //    MediaPlayer.Play();
            //}
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
                //if (ValidateInput())
                //{
                //    VideoItemsSource = new ObservableCollection<VideoUploadInfo>();
                //    pageIndex = 0;
                //    RunOnBackground(async () =>
                //    {
                //        return await streamCameraService.GetListVideoOnCloud(new Entities.CameraRestreamRequest()
                //        {
                //            CustomerId = UserInfo.XNCode,
                //            Date = DateStart.Date,
                //            VehicleNames = Vehicle.VehiclePlate
                //        });
                //    }, (result) =>
                //    {
                //        if (result != null && result.Count > 0)
                //        {
                //            var lstvideo = new List<VideoUploadInfo>();
                //            foreach (var item in result)
                //            {
                //                foreach (var item1 in item.Data)
                //                {
                //                    item1.Channel = item.Channel;
                //                }
                //                lstvideo.AddRange(item.Data);
                //            }
                //            if (lstvideo != null && lstvideo.Count > 0)
                //            {
                //                var video = lstvideo.Where(x => x.StartTime >= DateStart && x.StartTime <= DateEnd).OrderBy(x => x.StartTime).ToList();
                //                VideoItemsSource = video.ToObservableCollection();
                //            }
                //        }
                //    }, showLoading: true);
                //}
                var video = new List<VideoUploadInfo>();
                video.Add(new VideoUploadInfo()
                {
                    Channel = 1,
                    FileName = "29H123456_CH1_000000323.mp4",
                    StartTime = DateTime.Now,
                    Status = VideoUploadStatus.Uploading
                });
                video.Add(new VideoUploadInfo()
                {
                    Channel = 1,
                    FileName = "29H123456_CH1_000000323.mp4",
                    StartTime = DateTime.Now,
                    Status = VideoUploadStatus.UploadError
                });
                video.Add(new VideoUploadInfo()
                {
                    Channel = 1,
                    FileName = "29H123456_CH1_000000323.mp4",
                    StartTime = DateTime.Now,
                    Status = VideoUploadStatus.WaitingUpload
                });
                video.Add(new VideoUploadInfo()
                {
                    Channel = 1,
                    FileName = "29H123456_CH1_000000323.mp4",
                    StartTime = DateTime.Now,
                    Status = VideoUploadStatus.Uploaded
                });
                video.Add(new VideoUploadInfo()
                {
                    Channel = 1,
                    FileName = "29H123456_CH1_000000323.mp4",
                    StartTime = DateTime.Now,
                    Status = VideoUploadStatus.Uploaded
                });
                video.Add(new VideoUploadInfo()
                {
                    Channel = 1,
                    FileName = "29H123456_CH1_000000323.mp4",
                    StartTime = DateTime.Now,
                    Status = VideoUploadStatus.Uploaded
                });
                video.Add(new VideoUploadInfo()
                {
                    Channel = 1,
                    FileName = "29H123456_CH1_000000323.mp4",
                    StartTime = DateTime.Now,
                    Status = VideoUploadStatus.Uploaded
                });
                video.Add(new VideoUploadInfo()
                {
                    Channel = 1,
                    FileName = "29H123456_CH1_000000323.mp4",
                    StartTime = DateTime.Now,
                    Status = VideoUploadStatus.Uploaded
                });
                video.Add(new VideoUploadInfo()
                {
                    Channel = 1,
                    FileName = "29H123456_CH1_000000323.mp4",
                    StartTime = DateTime.Now,
                    Status = VideoUploadStatus.Uploaded
                });
                VideoItemsSource = video.ToObservableCollection();
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteError(MethodBase.GetCurrentMethod().Name, ex);
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
                SafeExecute(() =>
                {
                });
            }
        }

        #endregion PrivateMethod
    }
}