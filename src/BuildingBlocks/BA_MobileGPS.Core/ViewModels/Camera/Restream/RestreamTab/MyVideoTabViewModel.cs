using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Core.Interfaces;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service.IService;
using BA_MobileGPS.Utilities.Extensions;
using LibVLCSharp.Shared;
using Plugin.Permissions;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
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
            ScreenShotTappedCommand = new DelegateCommand(TakeSnapShot);
            DowloadVideoCommand = new DelegateCommand(DowloadVideo);
            DowloadVideoInListTappedCommand = new DelegateCommand<VideoUploadInfo>(DowloadVideoInListTapped);
            SelectVehicleCameraCommand = new DelegateCommand(SelectVehicleCamera);
            PreviousVideoCommand = new DelegateCommand(PreviousVideo);
            NextVideoCommand = new DelegateCommand(NextVideo);
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
            LibVLCSharp.Shared.Core.Initialize();
            LibVLC = new LibVLC();
            //InitVLC();
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
                    pageIndex = 0;
                    RunOnBackground(async () =>
                    {
                        return await streamCameraService.GetListVideoOnCloud(new Entities.CameraRestreamRequest()
                        {
                            CustomerId = UserInfo.XNCode,
                            Date = DateStart.Date,
                            VehicleNames = Vehicle.VehiclePlate
                        });
                    }, (result) =>
                    {
                        if (result != null && result.Count > 0)
                        {
                            var lstvideo = new List<VideoUploadInfo>();
                            foreach (var item in result)
                            {
                                foreach (var item1 in item.Data)
                                {
                                    item1.Channel = item.Channel;
                                }
                                lstvideo.AddRange(item.Data);
                            }
                            //var lstvideo = result.Where(x => x.VehicleName == Vehicle.VehiclePlate)?.Data;
                            if (lstvideo != null && lstvideo.Count > 0)
                            {
                                var video = lstvideo.Where(x => x.StartTime >= DateStart && x.StartTime <= DateEnd).OrderByDescending(x => x.StartTime).ToList();
                                VideoItemsSource = video.ToObservableCollection();
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

        private void SearchData()
        {
            GetListVideoDataFrom();
            CloseVideo();
        }

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
            if (VideoSlected != null && !string.IsNullOrEmpty(VideoSlected.Link))
            {
                isSelectPreOrNext = true;
                var index = VideoItemsSource.ToList().FindIndex(VideoSlected);
                if (index >= VideoItemsSource.Count - 1)
                {
                    return;
                }
                else
                {
                    VideoSlected = VideoItemsSource[index + 1];
                }
                InitVideoVideo(VideoSlected);

                isSelectPreOrNext = false;
            }
        }

        private void PreviousVideo()
        {
            if (VideoSlected != null && !string.IsNullOrEmpty(VideoSlected.Link))
            {
                isSelectPreOrNext = true;
                var index = VideoItemsSource.ToList().FindIndex(VideoSlected);
                if (index == 0)
                {
                    return;
                }
                else
                {
                    VideoSlected = VideoItemsSource[index - 1];
                }

                InitVideoVideo(VideoSlected);

                isSelectPreOrNext = false;
            }
        }

        private void InitVideoVideo(VideoUploadInfo obj)
        {
            if (MediaPlayerVisible)
            {
                CloseVideo();
            }
            foreach (var item in VideoItemsSource)
            {
                if (item.FileName == obj.FileName)
                {
                    item.IsSelected = true;
                }
                else { item.IsSelected = false; }
            }
            Device.BeginInvokeOnMainThread(() =>
            {
                IsError = false;
                BusyIndicatorActive = true;
                MediaPlayerVisible = true; // Bật layout media lên
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
                        var permissionStatus = await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>();
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