using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Core.Interfaces;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service.IService;
using LibVLCSharp.Shared;
using Plugin.Permissions;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Input;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class ViewVideoUploadedPageViewModel : RestreamChildVMBase
    {
        public ICommand ScreenShotTappedCommand { get; }

        /// <summary>
        /// Chụp ảnh màn hình
        /// </summary>
        public ICommand DowloadVideoCommand { get; }

        public ICommand DowloadVideoInListTappedCommand { get; }

        public ICommand PreviousVideoCommand { get; }

        public ICommand NextVideoCommand { get; }

        private readonly IDownloadVideoService _downloadService;

        public ViewVideoUploadedPageViewModel(INavigationService navigationService,
          IStreamCameraService cameraService,
          IScreenOrientServices screenOrientServices, IDownloadVideoService downloadService) : base(navigationService, cameraService, screenOrientServices)
        {
            _downloadService = downloadService;
            ScreenShotTappedCommand = new DelegateCommand(TakeSnapShot);
            DowloadVideoCommand = new DelegateCommand(DowloadVideo);
            DowloadVideoInListTappedCommand = new DelegateCommand<VideoUploadInfo>(DowloadVideoInListTapped);
            PreviousVideoCommand = new DelegateCommand(PreviousVideo);
            NextVideoCommand = new DelegateCommand(NextVideo);
        }

        #region Lifecycle

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
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
            InitVideoVideo(new VideoUploadInfo()
            {
                Channel = 1,
                FileName = "29H123456_CH1_000000323.mp4",
                Link = "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ElephantsDream.mp4",
                StartTime = DateTime.Now,
                Status = VideoUploadStatus.Uploaded
            });
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            DisposeVLC();
        }

        public override void OnSleep()
        {
            if (MediaPlayer != null)
            {
                MediaPlayer.Pause();
            }
            base.OnSleep();
        }

        public override void OnResume()
        {
            base.OnResume();
            DependencyService.Get<IScreenOrientServices>().ForcePortrait();
        }

        #endregion Lifecycle

        #region Property

        public string BusyIndicatorText { get; set; }

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

        private bool _autoSwitch = true;

        public bool AutoSwitch
        {
            get { return _autoSwitch; }
            set { SetProperty(ref _autoSwitch, value); }
        }

        private double _seekBarValue;

        public double SeekBarValue
        {
            get { return _seekBarValue; }
            set { SetProperty(ref _seekBarValue, value); }
        }

        #endregion Property

        #region PrivateMethod

        private void InitVLC(string url)
        {
            try
            {
                MediaPlayer = new MediaPlayer(LibVLC);
                MediaPlayer.TimeChanged += Media_TimeChanged;
                MediaPlayer.EndReached += Media_EndReached;
                MediaPlayer.EncounteredError += Media_EncounteredError;
                MediaPlayer.SnapshotTaken += MediaPlayer_SnapshotTaken;
                MediaPlayer.Media = new Media(libVLC, new Uri(url));
                MediaPlayer.Play();
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        private void MediaPlayer_SnapshotTaken(object sender, MediaPlayerSnapshotTakenEventArgs e)
        {
            if (File.Exists(e.Filename))
            {
                DependencyService.Get<ICameraSnapShotServices>().SaveSnapShotToGalery(e.Filename);
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
                    MediaPlayer.SnapshotTaken -= MediaPlayer_SnapshotTaken;

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

        public void SeekBarValueChanged(double value)
        {
            if (value <= 0)
            {
                if (AutoSwitch && isSelectPreOrNext == false)
                {
                    NextVideo();
                    isSelectPreOrNext = false;
                }
                else
                {
                    isSelectPreOrNext = false;
                }
            }
            else
            {
                isSelectPreOrNext = false;
            }
        }

        private bool isSelectPreOrNext = false;

        private void NextVideo()
        {
            //if (VideoSlected != null && !string.IsNullOrEmpty(VideoSlected.Link))
            //{
            //    isSelectPreOrNext = true;
            //    var index = VideoItemsSource.ToList().FindIndex(VideoSlected);
            //    if (index >= VideoItemsSource.Count - 1)
            //    {
            //        return;
            //    }
            //    else
            //    {
            //        VideoSlected = VideoItemsSource[index + 1];
            //    }
            //    InitVideoVideo(VideoSlected);

            //    isSelectPreOrNext = false;
            //}
        }

        private void PreviousVideo()
        {
            //if (VideoSlected != null && !string.IsNullOrEmpty(VideoSlected.Link))
            //{
            //    isSelectPreOrNext = true;
            //    var index = VideoItemsSource.ToList().FindIndex(VideoSlected);
            //    if (index == 0)
            //    {
            //        return;
            //    }
            //    else
            //    {
            //        VideoSlected = VideoItemsSource[index - 1];
            //    }

            //    InitVideoVideo(VideoSlected);

            //    isSelectPreOrNext = false;
            //}
        }

        private void InitVideoVideo(VideoUploadInfo obj)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                IsError = false;
                BusyIndicatorActive = true;
            });
            // init ở đây :
            InitVLC(obj.Link);

            VideoSlected = obj; // Set màu select cho item
        }

        /// <summary>
        /// Đóng video : Đóng khi current tab thay đổi
        /// Chức năng trên player : không làm (đã confirm)
        /// </summary>
        private void CloseVideo()
        {
            //can remove media plaer de tranh loi man hinh den
            DisposeVLC();
        }

        private void VideoSelectedChange(VideoUploadInfo obj)
        {
            if (obj != null)
            {
                SafeExecute(() =>
                {
                    isSelectPreOrNext = true;
                    InitVideoVideo(obj);
                });
            }
        }

        private void TakeSnapShot()
        {
            try
            {
                if (VideoSlected != null)
                {
                    var folderPath = DependencyService.Get<ICameraSnapShotServices>().GetFolderPath();
                    var current = DateTime.Now.ToString("yyyyMMddHHmmss");
                    var fileName = current + ".jpg";
                    var filePath = Path.Combine(folderPath, fileName);
                    bool result = MediaPlayer.TakeSnapshot(0, filePath, 0, 0);
                    //if (File.Exists(filePath) && result)
                    //{
                    //    DependencyService.Get<ICameraSnapShotServices>().SaveSnapShotToGalery(filePath);
                    //}
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
                        var permissionStatus = await CrossPermissions.Current.RequestPermissionAsync<Plugin.Permissions.StoragePermission>();
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

        private void DowloadVideoInListTapped(VideoUploadInfo obj)
        {
            SafeExecute(async () =>
            {
                if (obj != null && !string.IsNullOrEmpty(obj.Link))
                {
                    var action = await PageDialog.DisplayAlertAsync("Thông báo", "Bạn có muốn tải video này về điện thoại không ?", "Đồng ý", "Bỏ qua");
                    if (action)
                    {
                        var progressIndicator = new Progress<double>(ReportProgress);
                        var cts = new CancellationTokenSource();
                        IsDownloading = true;
                        var permissionStatus = await CrossPermissions.Current.RequestPermissionAsync<Plugin.Permissions.StoragePermission>();
                        if (permissionStatus == Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                        {
                            await _downloadService.DownloadFileAsync(obj.Link, progressIndicator, cts.Token);
                        }
                    }
                }
            });
        }

        #endregion PrivateMethod
    }
}