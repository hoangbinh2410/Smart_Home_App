using BA_MobileGPS.Entities;
using BA_MobileGPS.Service.IService;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Extensions;

namespace BA_MobileGPS.Core.ViewModels
{
    public class UploadVideoPageViewModel : ViewModelBase
    {
        private readonly IStreamCameraService streamCameraService;
        private CancellationTokenSource cts = new CancellationTokenSource();
        public ICommand UploadVideoCommand { get; }

        public UploadVideoPageViewModel(INavigationService navigationService, IStreamCameraService streamCameraService) : base(navigationService)
        {
            this.streamCameraService = streamCameraService;
            UploadVideoCommand = new Command(UploadVideo);
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters != null)
            {
                if (parameters.TryGetValue("UploadVideo", out CameraUploadRequest obj))
                {
                    RequestInfo = obj;
                    GetListVideoUploadActive();
                }
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        private CameraUploadRequest requestInfo;

        /// <summary>
        /// Ảnh được focus
        /// </summary>
        public CameraUploadRequest RequestInfo
        {
            get => requestInfo;
            set
            {
                SetProperty(ref requestInfo, value);
            }
        }

        private VideoRestreamInfo videoRestreamInfo = new VideoRestreamInfo();
        public VideoRestreamInfo VideoRestreamInfo { get => videoRestreamInfo; set => SetProperty(ref videoRestreamInfo, value); }
        private ObservableCollection<VideoUploadTimeInfo> listVideo = new ObservableCollection<VideoUploadTimeInfo>();
        public ObservableCollection<VideoUploadTimeInfo> ListVideo { get => listVideo; set => SetProperty(ref listVideo, value); }

        private void GetListVideoUploadActive()
        {
            RunOnBackground(async () =>
            {
                return await streamCameraService.GetListVideoNotUpload(RequestInfo);
            }, (result) =>
            {
                if (result != null && result.Data != null && result.Data.Count > 0)
                {
                    VideoRestreamInfo = result;
                    ListVideo = result.Data.ToObservableCollection();
                }
            }, showLoading: true);
        }

        private void UploadVideo(object obj)
        {
            SafeExecute(async () =>
            {
                var lstvideoSelected = ListVideo.Where(x => x.IsSelected == true).ToList();
                if (lstvideoSelected != null && lstvideoSelected.Count > 0)
                {
                    await PageDialog.DisplayAlertAsync("Thông báo", "Video đang được tải về server. Quý khách có thể xem các video đã tải trên tab Yêu cầu", "Đóng");
                    EventAggregator.GetEvent<UploadVideoEvent>().Publish(new VideoRestreamInfo()
                    {
                        Channel = VideoRestreamInfo.Channel,
                        Data = lstvideoSelected,
                        VehicleName = VideoRestreamInfo.VehicleName
                    });
                    await NavigationService.GoBackAsync();
                }
            });
        }
    }
}