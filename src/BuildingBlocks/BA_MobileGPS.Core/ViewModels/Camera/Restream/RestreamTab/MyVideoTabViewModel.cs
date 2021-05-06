using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Core.Interfaces;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service.IService;
using LibVLCSharp.Shared;
using Plugin.Permissions;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Extensions;

namespace BA_MobileGPS.Core.ViewModels
{
    public class MyVideoTabViewModel : RestreamChildVMBase
    {
        public ICommand ScreenShotTappedCommand { get; }

        /// <summary>
        /// Chụp ảnh màn hình
        /// </summary>
        public ICommand DowloadVideoCommand { get; }

        private readonly IDownloadVideoService _downloadService;

        public MyVideoTabViewModel(INavigationService navigationService,
          IStreamCameraService cameraService,
          IScreenOrientServices screenOrientServices, IDownloadVideoService downloadService) : base(navigationService, cameraService, screenOrientServices)
        {
            _downloadService = downloadService;
            VideoItemTapCommand = new DelegateCommand<VideoUploadInfo>(VideoSelectedChange);
            ScreenShotTappedCommand = new DelegateCommand(TakeSnapShot);
            DowloadVideoCommand = new DelegateCommand(DowloadVideo);
        }

        #region Lifecycle

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            GetListVideoDataFrom();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
        }

        public override void OnPageAppearingFirstTime()
        {
            base.OnPageAppearingFirstTime();
            LibVLCSharp.Shared.Core.Initialize();
            LibVLC = new LibVLC();
            InitVLC();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            DisposeVLC();
        }

        public override void OnSleep()
        {
            base.OnSleep();
        }

        #endregion Lifecycle

        #region Property

        public string BusyIndicatorText { get; set; }

        private ObservableCollection<VideoUploadInfo> videoItemsSource;

        /// <summary>
        /// Source ảnh để chọn video
        /// </summary>
        public ObservableCollection<VideoUploadInfo> VideoItemsSource { get => videoItemsSource; set => SetProperty(ref videoItemsSource, value); }

        private bool mediaPlayerVisible;

        public bool MediaPlayerVisible
        {
            get => mediaPlayerVisible; set => SetProperty(ref mediaPlayerVisible, value);
        }

        private LibVLC libVLC;

        public LibVLC LibVLC
        {
            get { return libVLC; }
            set { SetProperty(ref libVLC, value); }
        }

        private MediaPlayer mediaPlayer;

        public MediaPlayer MediaPlayer
        {
            get => mediaPlayer; set => SetProperty(ref mediaPlayer, value);
        }

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

        private double _progressValue;

        public double ProgressValue
        {
            get { return _progressValue; }
            set { SetProperty(ref _progressValue, value); }
        }

        private bool _isDownloading;

        public bool IsDownloading
        {
            get { return _isDownloading; }
            set { SetProperty(ref _isDownloading, value); }
        }

        #endregion Property

        #region PrivateMethod

        /// <summary>
        /// Lấy danh sách video từ server
        /// </summary>
        private void GetListVideoDataFrom()
        {
            try
            {
                VideoItemsSource = new ObservableCollection<VideoUploadInfo>();
                pageIndex = 0;
                RunOnBackground(async () =>
                {
                    return await streamCameraService.GetListVideoOnCloud(new Entities.CameraRestreamRequest()
                    {
                        CustomerId = 1010,
                        Date = Convert.ToDateTime("2021-05-05"),
                        VehicleNames = "CAMTEST3"
                    });
                }, (result) =>
                {
                    if (result != null && result.Count > 0)
                    {
                        VideoItemsSource = (result.FirstOrDefault(x => x.VehicleName == "CAMTEST3")?.Data).ToObservableCollection();
                    }
                }, showLoading: true);
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        private void InitVLC()
        {
            try
            {
                MediaPlayer = new MediaPlayer(LibVLC) { EnableHardwareDecoding = true };
                MediaPlayer.TimeChanged += Media_TimeChanged;
                MediaPlayer.EndReached += Media_EndReached;
                MediaPlayer.EncounteredError += Media_EncounteredError;
                MediaPlayer.Play();
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        private void DisposeVLC()
        {
            try
            {
                if (MediaPlayer != null)
                {
                    MediaPlayer.TimeChanged -= Media_TimeChanged;
                    MediaPlayer.EndReached -= Media_EndReached;
                    MediaPlayer.EncounteredError -= Media_EncounteredError;

                    if (MediaPlayer.Media != null)
                    {
                        MediaPlayer.Media?.Dispose();
                        MediaPlayer.Media = null;
                    }

                    var media = MediaPlayer;
                    MediaPlayer = null;
                    media?.Dispose();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        /// <summary>
        /// Err : Fail connect server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///
        private void Media_EncounteredError(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Err : BỊ abort sau 10s không nhận tín hiệu từ server
        /// hoặc hết video hoặc video bị gọi đóng từ thiết bị khác
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Media_EndReached(object sender, EventArgs e)
        {
        }

        private void Media_TimeChanged(object sender, MediaPlayerTimeChangedEventArgs e)
        {
            if (MediaPlayer.Time > 1 && BusyIndicatorActive)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    BusyIndicatorActive = false;
                });
            }
        }

        /// <summary>
        /// Đóng video : Đóng khi current tab thay đổi
        /// Chức năng trên player : không làm (đã confirm)
        /// </summary>
        private void CloseVideo()
        {
            if (MediaPlayerVisible)
            {
                MediaPlayerVisible = false;
            }
            //can remove media plaer de tranh loi man hinh den
            DisposeVLC();
        }

        private void VideoSelectedChange(VideoUploadInfo obj)
        {
            if (obj != null)
            {
                SafeExecute(() =>
                {
                    if (MediaPlayerVisible)
                    {
                        CloseVideo();
                    }

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        IsError = false;
                        BusyIndicatorActive = true;
                        MediaPlayerVisible = true; // Bật layout media lên
                    });
                    // init ở đây :
                    InitVLC();
                    MediaPlayer.Media = new Media(libVLC, new Uri(obj.Link));
                    MediaPlayer.Play();
                    VideoSlected = obj; // Set màu select cho item
                });
            }
        }

        private void TakeSnapShot()
        {
            try
            {
                var folderPath = DependencyService.Get<ICameraSnapShotServices>().GetFolderPath();
                var current = DateTime.Now.ToString("yyyyMMddHHmmss");
                var fileName = current + ".jpg";
                var filePath = Path.Combine(folderPath, fileName);

                if (VideoSlected != null)
                {
                    MediaPlayer.TakeSnapshot(0, filePath, 0, 0);
                }
                if (File.Exists(filePath))
                {
                    DependencyService.Get<ICameraSnapShotServices>().SaveSnapShotToGalery(filePath);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        private void DowloadVideo()
        {
            SafeExecute(async () =>
            {
                if (VideoSlected != null && !string.IsNullOrEmpty(VideoSlected.Link))
                {
                    var action = await PageDialog.DisplayAlertAsync("Thông báo", "Bạn có muốn tải video này về điện thoại không ?", "Đồng ý", "Bỏ qua");
                    if (action)
                    {
                        var progressIndicator = new Progress<double>(ReportProgress);
                        var cts = new CancellationTokenSource();
                        IsDownloading = true;
                        var permissionStatus = await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>();
                        if (permissionStatus == Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                        {
                            await _downloadService.DownloadFileAsync(VideoSlected.Link, progressIndicator, cts.Token);
                        }
                    }
                }
            });
        }

        internal void ReportProgress(double value)
        {
            ProgressValue = value;
            if (value == 100)
            {
                DisplayMessage.ShowMessageInfo("Đã tải video thành công");
                IsDownloading = false;
            }
        }

        #endregion PrivateMethod
    }
}