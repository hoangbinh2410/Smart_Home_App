using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Core.Interfaces;
using BA_MobileGPS.Core.Views;
using BA_MobileGPS.Core.Views.Camera.MonitoringCamera;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using BA_MobileGPS.Service.IService;
using LibVLCSharp.Shared;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Sharpnado.Presentation.Forms.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;

using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class CameraManagingPageViewModel : ViewModelBase
    {
        private System.Timers.Timer timer;
        private int counterRequestMoreTime = 15;
        private readonly int maxLoadingTime = 15000; //milisecond
        private readonly string playIconSource = "ic_play_arrow_white.png";
        private readonly string stopIconSource = "ic_stop_white.png";
        private readonly string volumeIconSource = "ic_volumespeaker";
        private readonly string muteIconSource = "ic_mute";
        private StreamStart videoUrl1 { get; set; }
        private StreamStart videoUrl2 { get; set; }
        private StreamStart videoUrl3 { get; set; }
        private StreamStart videoUrl4 { get; set; }
        private Stopwatch cam1LoadingTime = new Stopwatch();
        private Stopwatch cam2LoadingTime = new Stopwatch();
        private Stopwatch cam3LoadingTime = new Stopwatch();
        private Stopwatch cam4LoadingTime = new Stopwatch();
        private string currentXnCode { get; set; }
        private string currentIMEI { get; set; }
        private const int maxTimeCameraRemain = 598; //second
        private List<CameraEnum> currentCamera { get; set; } = new List<CameraEnum>();
        private readonly IGeocodeService _geocodeService;
        private readonly IStreamCameraService _streamCameraService;
        private bool stopLoad = true;
        public CameraManagingPageViewModel(INavigationService navigationService, IStreamCameraService streamCameraService, IGeocodeService geocodeService) : base(navigationService)
        {
            _geocodeService = geocodeService;
            _streamCameraService = streamCameraService;
            PlayTappedCommand = new DelegateCommand(PlayTapped);
            CameraFrameTappedCommand = new DelegateCommand<object>(CameraFrameTapped);
            VolumeChangedCommand = new DelegateCommand(VolumeChanged);
            playButtonIconSource = playIconSource;
            volumeButtonIconSource = muteIconSource;
            RequestTimeTappedCommand = new DelegateCommand(RequestTimeTapped);
            FullScreenTappedCommand = new DelegateCommand(FullScreenTapped);
            isFullScreenOff = true;
            ScreenShotTappedCommand = new DelegateCommand(ScreenShotTapped);
            ShareTappedCommand = new DelegateCommand(ShareTapped);
            AutoAddTime = true;
            ReloadCommand = new DelegateCommand<object>(Reload);
            currentAddress = "Không xác định";
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            // Đóng busy indicator
            Device.BeginInvokeOnMainThread(() =>
            {
                IsCam1Loaded = true;
                IsCam2Loaded = true;
                IsCam3Loaded = true;
                IsCam4Loaded = true;
            });

            if (parameters.ContainsKey(ParameterKey.Vehicle) && parameters.GetValue<Vehicle>(ParameterKey.Vehicle) is Vehicle vehiclePlate)
            {
                stopLoad = false;
                DisposeAllMediaPlayer();
                VehicleSelectedPlate = vehiclePlate.VehiclePlate;
                ReLoadCamera();

            }
            else if (parameters.ContainsKey(ParameterKey.VehicleGroups) && parameters.GetValue<int[]>(ParameterKey.VehicleGroups) is int[] vehiclegroup)
            {
                VehicleGroups = vehiclegroup;
            }
            else if (parameters.ContainsKey(ParameterKey.RequestTime) && parameters.GetValue<int>(ParameterKey.RequestTime) is int time)
            {
                RequestMoreTimeStream(time);
            }

            base.OnNavigatedTo(parameters);
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            SetUpVlc();
        }

        private int totalTimeCam1;// second

        public int TotalTimeCam1
        {
            get { return totalTimeCam1; }
            set
            {
                SetProperty(ref totalTimeCam1, value);
                RaisePropertyChanged();
            }
        }

        private int totalTimeCam2; // second

        public int TotalTimeCam2
        {
            get { return totalTimeCam2; }
            set
            {
                SetProperty(ref totalTimeCam2, value);
                RaisePropertyChanged();
            }
        }

        private int totalTimeCam3;// second

        public int TotalTimeCam3
        {
            get { return totalTimeCam3; }
            set
            {
                SetProperty(ref totalTimeCam3, value);
                RaisePropertyChanged();
            }
        }

        private int totalTimeCam4;// second

        public int TotalTimeCam4
        {
            get { return totalTimeCam4; }
            set
            {
                SetProperty(ref totalTimeCam4, value);
                RaisePropertyChanged();
            }
        }

        private LibVLC libVLC;

        public LibVLC LibVLC
        {
            get { return libVLC; }
            set
            {
                SetProperty(ref libVLC, value);
            }
        }

        private MediaPlayer mediaPlayerNo1;

        public MediaPlayer MediaPlayerNo1
        {
            get { return mediaPlayerNo1; }
            set
            {
                SetProperty(ref mediaPlayerNo1, value);
                RaisePropertyChanged();
            }
        }

        private bool isCam1Loaded;

        public bool IsCam1Loaded
        {
            get { return isCam1Loaded; }
            set
            {
                SetProperty(ref isCam1Loaded, value);
                RaisePropertyChanged();
            }
        }

        private MediaPlayer mediaPlayerNo2;

        public MediaPlayer MediaPlayerNo2
        {
            get { return mediaPlayerNo2; }
            set
            {
                SetProperty(ref mediaPlayerNo2, value);
                RaisePropertyChanged();
            }
        }

        private bool isCam2Loaded;

        public bool IsCam2Loaded
        {
            get { return isCam2Loaded; }
            set
            {
                SetProperty(ref isCam2Loaded, value);
                RaisePropertyChanged();
            }
        }

        private MediaPlayer mediaPlayerNo3;

        public MediaPlayer MediaPlayerNo3
        {
            get { return mediaPlayerNo3; }
            set
            {
                SetProperty(ref mediaPlayerNo3, value);
                RaisePropertyChanged();
            }
        }

        private bool isCam3Loaded;

        public bool IsCam3Loaded
        {
            get { return isCam3Loaded; }
            set
            {
                SetProperty(ref isCam3Loaded, value);
                RaisePropertyChanged();
            }
        }

        private MediaPlayer mediaPlayerNo4;

        public MediaPlayer MediaPlayerNo4
        {
            get { return mediaPlayerNo4; }
            set
            {
                SetProperty(ref mediaPlayerNo4, value);
                RaisePropertyChanged();
            }
        }

        private bool isCam4Loaded;

        public bool IsCam4Loaded
        {
            get { return isCam4Loaded; }
            set
            {
                SetProperty(ref isCam4Loaded, value);
                RaisePropertyChanged();
            }
        }

        private string vehicleSelectedPlate;

        public string VehicleSelectedPlate
        {
            get { return vehicleSelectedPlate; }
            set
            {
                SetProperty(ref vehicleSelectedPlate, value);
                RaisePropertyChanged();
            }
        }

        private bool isFullScreenOff;

        public bool IsFullScreenOff
        {
            get { return isFullScreenOff; }
            set
            {
                SetProperty(ref isFullScreenOff, value);
                RaisePropertyChanged();
            }
        }

        private CameraEnum? selectedCamera;

        public CameraEnum? SelectedCamera
        {
            get { return selectedCamera; }
            set
            {
                SetProperty(ref selectedCamera, value);
                RaisePropertyChanged();
            }
        }

        private string volumeButtonIconSource;

        public string VolumeButtonIconSource
        {
            get { return volumeButtonIconSource; }
            set
            {
                SetProperty(ref volumeButtonIconSource, value);
                RaisePropertyChanged();
            }
        }

        private string playButtonIconSource;

        public string PlayButtonIconSource
        {
            get { return playButtonIconSource; }
            set
            {
                SetProperty(ref playButtonIconSource, value);
                RaisePropertyChanged();
            }
        }

        private bool isCAM1Error;

        public bool IsCAM1Error
        {
            get { return isCAM1Error; }
            set
            {
                SetProperty(ref isCAM1Error, value);
                RaisePropertyChanged();
            }
        }

        private bool isCAM2Error;

        public bool IsCAM2Error
        {
            get { return isCAM2Error; }
            set
            {
                SetProperty(ref isCAM2Error, value);
                RaisePropertyChanged();
            }
        }

        private bool isCAM3Error;

        public bool IsCAM3Error
        {
            get { return isCAM3Error; }
            set
            {
                SetProperty(ref isCAM3Error, value);
                RaisePropertyChanged();
            }
        }

        private bool isCAM4Error;

        public bool IsCAM4Error
        {
            get { return isCAM4Error; }
            set
            {
                SetProperty(ref isCAM4Error, value);
                RaisePropertyChanged();
            }
        }

        private bool autoAddTime;

        public bool AutoAddTime
        {
            get { return autoAddTime; }
            set
            {
                SetProperty(ref autoAddTime, value);
                RaisePropertyChanged();
            }
        }

        private string currentAddress;
        public string CurrentAddress
        {
            get { return currentAddress; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    SetProperty(ref currentAddress, "Không xác định");
                }
                else
                {
                    SetProperty(ref currentAddress, value);
                }

                RaisePropertyChanged();
            }
        }

        private DateTime? currentTime;
        public DateTime? CurrentTime
        {
            get { return currentTime; }
            set
            {
                SetProperty(ref currentTime, value);
                RaisePropertyChanged();
            }
        }

        public ICommand PlayTappedCommand { get; }

        private void PlayTapped()
        {
            if (CanExcute())
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    switch (SelectedCamera)
                    {
                        case CameraEnum.CAM1:
                            if (IsCam1Loaded)
                            {
                                if (MediaPlayerNo1.IsPlaying)
                                {
                                    MediaPlayerNo1.Pause();
                                    PlayButtonIconSource = playIconSource;
                                }
                                else
                                {
                                    MediaPlayerNo1.Play();
                                    PlayButtonIconSource = stopIconSource;
                                }
                            }
                            break;

                        case CameraEnum.CAM2:
                            if (IsCam2Loaded)
                            {
                                if (MediaPlayerNo2.IsPlaying)
                                {
                                    MediaPlayerNo2.Pause();
                                    PlayButtonIconSource = playIconSource;
                                }
                                else
                                {
                                    MediaPlayerNo2.Play();
                                    PlayButtonIconSource = stopIconSource;
                                }
                            }
                            break;

                        case CameraEnum.CAM3:
                            if (IsCam3Loaded)
                            {
                                if (MediaPlayerNo3.IsPlaying)
                                {
                                    MediaPlayerNo3.Pause();
                                    PlayButtonIconSource = playIconSource;
                                }
                                else
                                {
                                    MediaPlayerNo3.Play();
                                    PlayButtonIconSource = stopIconSource;
                                }
                            }
                            break;

                        case CameraEnum.CAM4:
                            if (IsCam4Loaded)
                            {
                                if (MediaPlayerNo4.IsPlaying)
                                {
                                    MediaPlayerNo4.Pause();
                                    PlayButtonIconSource = playIconSource;
                                }
                                else
                                {
                                    MediaPlayerNo4.Play();
                                    PlayButtonIconSource = stopIconSource;
                                }
                            }
                            break;

                        default:
                            break;
                    }
                });
            }
        }

        private void SetUpVlc()
        {
            LibVLCSharp.Shared.Core.Initialize();
            LibVLC = new LibVLC("--no-osd", "--rtsp-tcp");
        }

        private void InitCamera1(string url)
        {
            try
            {
                if (IsCam1Loaded)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        IsCam1Loaded = false;
                    });
                }
                var mediaNo1 = new Media(LibVLC, new Uri(url));
                MediaPlayerNo1 = new MediaPlayer(mediaNo1) { AspectRatio = "4:3", Scale = 0, Mute = true };
                MediaPlayerNo1.TimeChanged += MediaPlayerNo1_TimeChanged;
                MediaPlayerNo1.EncounteredError += MediaPlayerNo1_EncounteredError;
                MediaPlayerNo1.Play();
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteError("InitCamera1", ex);
            }

        }

        private void MediaPlayerNo1_EncounteredError(object sender, EventArgs e)
        {
            TryExecute(() =>
            {
                if (cam1LoadingTime.ElapsedMilliseconds <= maxLoadingTime)
                {
                    MediaPlayerNo1.TimeChanged -= MediaPlayerNo1_TimeChanged;
                    MediaPlayerNo1.EncounteredError -= MediaPlayerNo1_EncounteredError;
                    InitCamera1(videoUrl1.Link);
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        cam1LoadingTime.Reset();
                        IsCam1Loaded = true;
                        IsCAM1Error = true;
                    });
                }
            });
        }

        private void MediaPlayerNo1_TimeChanged(object sender, MediaPlayerTimeChangedEventArgs e)
        {
            try
            {
                if (e.Time > 0)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        PlayButtonIconSource = stopIconSource;
                        IsCam1Loaded = true;
                        TotalTimeCam1 = 180;
                    });

                    ShowVideoView(CameraEnum.CAM1);
                    cam1LoadingTime.Reset();
                    MediaPlayerNo1.TimeChanged -= MediaPlayerNo1_TimeChanged;
                }
            }
            catch (Exception ex)
            {


            }

        }

        private void InitCamera2(string url)
        {
            try
            {
                if (IsCam2Loaded)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        IsCam2Loaded = false;
                    });
                }
                var mediaNo1 = new Media(LibVLC, new Uri(url));
                MediaPlayerNo2 = new MediaPlayer(mediaNo1) { AspectRatio = "4:3", Scale = 0, Mute = true };
                MediaPlayerNo2.TimeChanged += MediaPlayerNo2_TimeChanged;
                MediaPlayerNo2.EncounteredError += MediaPlayerNo2_EncounteredError;
                MediaPlayerNo2.Play();
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteError("InitCamera2", ex);
            }

        }

        private void MediaPlayerNo2_EncounteredError(object sender, EventArgs e)
        {
            TryExecute(() =>
            {
                if (cam2LoadingTime.ElapsedMilliseconds <= maxLoadingTime)
                {
                    MediaPlayerNo2.TimeChanged -= MediaPlayerNo2_TimeChanged;
                    MediaPlayerNo2.EncounteredError -= MediaPlayerNo2_EncounteredError;
                    InitCamera2(videoUrl2.Link);
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        cam2LoadingTime.Reset();
                        IsCam2Loaded = true;
                        IsCAM2Error = true;
                    });
                }
            });
        }

        private void MediaPlayerNo2_TimeChanged(object sender, MediaPlayerTimeChangedEventArgs e)
        {
            try
            {
                if (e.Time > 0 && !IsCam2Loaded)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        PlayButtonIconSource = stopIconSource;
                        IsCam2Loaded = true;
                        TotalTimeCam2 = 180;
                    });
                    ShowVideoView(CameraEnum.CAM2);
                    cam2LoadingTime.Reset();
                    MediaPlayerNo2.TimeChanged -= MediaPlayerNo2_TimeChanged;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteError("MediaPlayerNo2_TimeChanged", ex);
            }
        }

        private void InitCamera3(string url)
        {
            try
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsCam3Loaded = false;
                });

                var mediaNo1 = new Media(LibVLC, new Uri(url));
                MediaPlayerNo3 = new MediaPlayer(mediaNo1) { AspectRatio = "4:3", Scale = 0, Mute = true };
                MediaPlayerNo3.TimeChanged += MediaPlayerNo3_TimeChanged;
                MediaPlayerNo3.EncounteredError += MediaPlayerNo3_EncounteredError;
                MediaPlayerNo3.Play();
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteError("InitCamera3", ex);
            }

        }

        private void MediaPlayerNo3_EncounteredError(object sender, EventArgs e)
        {
            TryExecute(() =>
            {
                if (cam3LoadingTime.ElapsedMilliseconds < maxLoadingTime)
                {
                    MediaPlayerNo3.TimeChanged -= MediaPlayerNo3_TimeChanged;
                    MediaPlayerNo3.EncounteredError -= MediaPlayerNo3_EncounteredError;
                    InitCamera3(videoUrl3.Link);
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        cam3LoadingTime.Reset();
                        IsCam3Loaded = true;
                        IsCAM3Error = true;
                    });
                }
            });
        }

        private void MediaPlayerNo3_TimeChanged(object sender, MediaPlayerTimeChangedEventArgs e)
        {
            if (e.Time > 0 && !IsCam3Loaded)
            {
                PlayButtonIconSource = stopIconSource;
                IsCam3Loaded = true;
                ShowVideoView(CameraEnum.CAM3);
                TotalTimeCam3 = 180;
                cam3LoadingTime.Reset();
                MediaPlayerNo3.TimeChanged -= MediaPlayerNo3_TimeChanged;
            }
        }

        private void InitCamera4(string url)
        {
            try
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsCam4Loaded = false;
                });

                var mediaNo1 = new Media(LibVLC, new Uri(url));
                MediaPlayerNo4 = new MediaPlayer(mediaNo1) { AspectRatio = "4:3", Scale = 0, Mute = true };
                MediaPlayerNo4.TimeChanged += MediaPlayerNo4_TimeChanged;
                MediaPlayerNo4.EncounteredError += MediaPlayerNo4_EncounteredError;
                MediaPlayerNo4.Play();
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteError("InitCamera4", ex);
            }
        }

        private void MediaPlayerNo4_EncounteredError(object sender, EventArgs e)
        {
            TryExecute(() =>
            {
                if (cam4LoadingTime.ElapsedMilliseconds <= maxLoadingTime)
                {
                    MediaPlayerNo4.TimeChanged -= MediaPlayerNo4_TimeChanged;
                    MediaPlayerNo4.EncounteredError -= MediaPlayerNo4_EncounteredError;
                    InitCamera4(videoUrl4.Link);
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        IsCam4Loaded = true;
                        IsCAM4Error = true;
                        cam4LoadingTime.Reset();
                    });
                }
            });
        }

        private void MediaPlayerNo4_TimeChanged(object sender, MediaPlayerTimeChangedEventArgs e)
        {
            if (e.Time > 0 && !IsCam4Loaded)
            {
                PlayButtonIconSource = stopIconSource;
                IsCam4Loaded = true;
                ShowVideoView(CameraEnum.CAM4);
                TotalTimeCam4 = 180;
                cam4LoadingTime.Reset();
                MediaPlayerNo4.TimeChanged -= MediaPlayerNo4_TimeChanged;
            }
        }

        public ICommand VolumeChangedCommand { get; }

        private void VolumeChanged()
        {
            if (CanExcute())
            {
                switch (SelectedCamera)
                {
                    case CameraEnum.CAM1:
                        MediaPlayerNo1.Mute = !MediaPlayerNo1.Mute;
                        VolumeButtonIconSource = MediaPlayerNo1.Mute ? muteIconSource : volumeIconSource;
                        break;

                    case CameraEnum.CAM2:
                        MediaPlayerNo2.Mute = !MediaPlayerNo2.Mute;
                        VolumeButtonIconSource = MediaPlayerNo2.Mute ? muteIconSource : volumeIconSource;
                        break;

                    case CameraEnum.CAM3:
                        MediaPlayerNo3.Mute = !MediaPlayerNo3.Mute;
                        VolumeButtonIconSource = MediaPlayerNo3.Mute ? muteIconSource : volumeIconSource;
                        break;

                    case CameraEnum.CAM4:
                        MediaPlayerNo4.Mute = !MediaPlayerNo4.Mute;
                        VolumeButtonIconSource = MediaPlayerNo4.Mute ? muteIconSource : volumeIconSource;
                        break;
                }
            }
        }

        public ICommand CameraFrameTappedCommand { get; }

        private void CameraFrameTapped(object obj)
        {
            var camObj = (CameraEnum)obj;
            if (currentCamera.Contains(camObj))
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    SelectedCamera = camObj;
                    switch (camObj)
                    {
                        case CameraEnum.CAM1:
                            PlayButtonIconSource = MediaPlayerNo1.IsPlaying ? stopIconSource : playIconSource;
                            VolumeButtonIconSource = MediaPlayerNo1.Mute ? muteIconSource : volumeIconSource;
                            break;

                        case CameraEnum.CAM2:
                            PlayButtonIconSource = MediaPlayerNo2.IsPlaying ? stopIconSource : playIconSource;
                            VolumeButtonIconSource = MediaPlayerNo2.Mute ? muteIconSource : volumeIconSource;
                            break;

                        case CameraEnum.CAM3:
                            PlayButtonIconSource = MediaPlayerNo3.IsPlaying ? stopIconSource : playIconSource;
                            VolumeButtonIconSource = MediaPlayerNo3.Mute ? muteIconSource : volumeIconSource;
                            break;

                        case CameraEnum.CAM4:
                            PlayButtonIconSource = MediaPlayerNo4.IsPlaying ? stopIconSource : playIconSource;
                            VolumeButtonIconSource = MediaPlayerNo4.Mute ? muteIconSource : volumeIconSource;
                            break;
                    }
                });
            }
        }

        public ICommand RequestTimeTappedCommand { get; set; }

        private void RequestTimeTapped()
        {
            if (CanExcute())
            {
                var canTap = false;
                switch (SelectedCamera)
                {
                    case CameraEnum.CAM1:
                        if (TotalTimeCam1 > 0)
                        {
                            canTap = true;
                        }
                        break;

                    case CameraEnum.CAM2:
                        if (TotalTimeCam1 > 0)
                        {
                            canTap = true;
                        }
                        break;

                    case CameraEnum.CAM3:
                        if (TotalTimeCam1 > 0)
                        {
                            canTap = true;
                        }
                        break;

                    case CameraEnum.CAM4:
                        if (TotalTimeCam1 > 0)
                        {
                            canTap = true;
                        }
                        break;
                }
                if (canTap)
                {
                    SafeExecute(async () =>
                    {
                        await NavigationService.NavigateAsync("RequestMoreTimePopup");
                    });
                }
            }
        }

        public ICommand FullScreenTappedCommand { get; }

        private void FullScreenTapped()
        {
            if (CanExcute())
            {
                IsFullScreenOff = !IsFullScreenOff;
                if (IsFullScreenOff)
                {
                    EventAggregator.GetEvent<SwitchToNormalScreenEvent>().Publish();
                    DependencyService.Get<IScreenOrientServices>().ForcePortrait();
                    if (MediaPlayerNo1 != null)
                    {
                        MediaPlayerNo1.AspectRatio = "4:3";
                    }
                    if (MediaPlayerNo2 != null)
                    {
                        MediaPlayerNo2.AspectRatio = "4:3";
                    }
                    if (MediaPlayerNo3 != null)
                    {
                        MediaPlayerNo3.AspectRatio = "4:3";
                    }
                    if (MediaPlayerNo4 != null)
                    {
                        MediaPlayerNo4.AspectRatio = "4:3";
                    }
                }
                else
                {
                    SetLanscape();
                }
            }
        }

        private void SetLanscape()
        {
            DependencyService.Get<IScreenOrientServices>().ForceLandscape();
            Device.BeginInvokeOnMainThread(() =>
            {
                switch (SelectedCamera)
                {
                    case CameraEnum.CAM1:
                        MediaPlayerNo1.AspectRatio = "16:9";
                        EventAggregator.GetEvent<SwitchToFullScreenEvent>().Publish(CameraEnum.CAM1);
                        break;

                    case CameraEnum.CAM2:
                        MediaPlayerNo2.AspectRatio = "16:9";
                        EventAggregator.GetEvent<SwitchToFullScreenEvent>().Publish(CameraEnum.CAM2);
                        break;

                    case CameraEnum.CAM3:
                        MediaPlayerNo3.AspectRatio = "16:9";
                        EventAggregator.GetEvent<SwitchToFullScreenEvent>().Publish(CameraEnum.CAM3);
                        break;

                    case CameraEnum.CAM4:
                        MediaPlayerNo4.AspectRatio = "16:9";
                        EventAggregator.GetEvent<SwitchToFullScreenEvent>().Publish(CameraEnum.CAM4);
                        break;
                }
            });
        }

        public ICommand ScreenShotTappedCommand { get; }

        private async void ScreenShotTapped()
        {
            await TakeSnapShot();
        }

        private async Task<string> TakeSnapShot()
        {
            try
            {
                var photoPermission = await PermissionHelper.CheckPhotoPermissions();
                var storagePermission = await PermissionHelper.CheckStoragePermissions();
                if (photoPermission && storagePermission)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        SetLanscape();
                    });

                    var folderPath = DependencyService.Get<ICameraSnapShotServices>().GetFolderPath();
                    var current = DateTime.Now.ToString("yyyyMMddHHmmss");
                    var fileName = Enum.GetName(typeof(CameraEnum), SelectedCamera) + current + ".jpg";
                    var filePath = Path.Combine(folderPath, fileName);
                    switch (SelectedCamera)
                    {
                        case CameraEnum.CAM1:
                            MediaPlayerNo1.TakeSnapshot(0, filePath, 0, 0);
                            break;

                        case CameraEnum.CAM2:
                            MediaPlayerNo2.TakeSnapshot(0, filePath, 0, 0);
                            break;

                        case CameraEnum.CAM3:
                            MediaPlayerNo3.TakeSnapshot(0, filePath, 0, 0);
                            break;

                        case CameraEnum.CAM4:
                            MediaPlayerNo4.TakeSnapshot(0, filePath, 0, 0);
                            break;
                    }
                    if (File.Exists(filePath))
                    {
                        DependencyService.Get<ICameraSnapShotServices>().SaveSnapShotToGalery(filePath);
                        return filePath;
                    }
                    return string.Empty;
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        SetLanscape();
                    });
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteError(MethodBase.GetCurrentMethod().Name, ex);
                return string.Empty;
            }
        }

        public ICommand ShareTappedCommand { get; }

        private async void ShareTapped()
        {
            var filePath = await TakeSnapShot();
            await Xamarin.Essentials.Share.RequestAsync(new Xamarin.Essentials.ShareFileRequest(new Xamarin.Essentials.ShareFile(filePath)));
        }

        public ICommand ReloadCommand { get; }
        private void Reload(object obj)
        {
            var param = (CameraEnum)obj;
            //cam1LoadingTime = 0 when video was played or error
            if (SelectedCamera != null)
            {
                switch (param)
                {
                    case CameraEnum.CAM1:
                        DisposeMediaPlayer(CameraEnum.CAM1);
                        TotalTimeCam1 = 1;
                        IsCAM1Error = false;
                        RequestStartCam(videoUrl1.Channel, CameraEnum.CAM1);
                        break;

                    case CameraEnum.CAM2:
                        DisposeMediaPlayer(CameraEnum.CAM2);
                        TotalTimeCam2 = 1;
                        IsCAM2Error = false;
                        RequestStartCam(videoUrl2.Channel, CameraEnum.CAM2);
                        break;

                    case CameraEnum.CAM3:
                        DisposeMediaPlayer(CameraEnum.CAM3);
                        TotalTimeCam3 = 1;
                        IsCAM3Error = false;
                        RequestStartCam(videoUrl3.Channel, CameraEnum.CAM3);
                        break;

                    case CameraEnum.CAM4:
                        DisposeMediaPlayer(CameraEnum.CAM4);
                        TotalTimeCam4 = 1;
                        IsCAM4Error = false;
                        RequestStartCam(videoUrl4.Channel, CameraEnum.CAM4);
                        break;
                }

            }
        }


        private void RequestStartCam(int chanel, CameraEnum position)
        {
            TryExecute(async () =>
            {
                var request = new StreamStartRequest()
                {
                    Channel = chanel,
                    xnCode = currentXnCode,
                    VehiclePlate = VehicleSelectedPlate,
                    IMEI = currentIMEI
                };

                StreamStart camResponseData = null;
                while (stopLoad)
                {
                    switch (position)
                    {
                        case CameraEnum.CAM1:
                            if (!cam1LoadingTime.IsRunning)
                            {
                                cam1LoadingTime.Start();
                            }
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                IsCam1Loaded = false;
                                if (cam1LoadingTime.ElapsedMilliseconds >= maxLoadingTime)
                                {
                                    stopLoad = false;
                                    IsCam1Loaded = true;
                                    IsCAM1Error = true;

                                }
                            });

                            break;

                        case CameraEnum.CAM2:
                            if (!cam2LoadingTime.IsRunning)
                            {
                                cam2LoadingTime.Start();
                            }
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                IsCam2Loaded = false;
                                if (cam2LoadingTime.ElapsedMilliseconds >= maxLoadingTime)
                                {
                                    stopLoad = false;
                                    IsCam2Loaded = true;
                                    IsCAM2Error = true;
                                }
                            });
                            break;

                        case CameraEnum.CAM3:
                            if (!cam3LoadingTime.IsRunning)
                            {
                                cam3LoadingTime.Start();
                            }
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                IsCam3Loaded = false;
                                if (cam3LoadingTime.ElapsedMilliseconds >= maxLoadingTime)
                                {
                                    stopLoad = false;
                                    IsCam3Loaded = true;
                                    IsCAM3Error = true;
                                }
                            });
                            break;

                        case CameraEnum.CAM4:
                            if (!cam4LoadingTime.IsRunning)
                            {
                                cam4LoadingTime.Start();
                            }
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                IsCam4Loaded = false;
                                if (cam4LoadingTime.ElapsedMilliseconds >= maxLoadingTime)
                                {
                                    stopLoad = false;
                                    IsCam4Loaded = true;
                                    IsCAM4Error = true;
                                }
                            });
                            break;
                    }
                    var camResponse = await _streamCameraService.StartStream(request);
                    var checkResult = await _streamCameraService.GetDevicesStatus(ConditionType.BKS, VehicleSelectedPlate);
                    var checkStatus = checkResult?.Data?.FirstOrDefault()?.CameraChannels.FirstOrDefault(x => x.Channel == chanel);
                    if (checkStatus != null && checkStatus.IsStreaming)
                    {
                        camResponseData = camResponse.Data.FirstOrDefault();
                        stopLoad = false;
                        if (camResponseData != null)
                        {
                            switch (position)
                            {
                                case CameraEnum.CAM1:
                                    videoUrl1 = camResponseData;
                                    InitCamera1(camResponseData.Link);
                                    break;

                                case CameraEnum.CAM2:
                                    videoUrl2 = camResponseData;
                                    InitCamera2(camResponseData.Link);
                                    break;

                                case CameraEnum.CAM3:
                                    videoUrl3 = camResponseData;
                                    InitCamera3(camResponseData.Link);
                                    break;

                                case CameraEnum.CAM4:
                                    videoUrl4 = camResponseData;
                                    InitCamera4(camResponseData.Link);
                                    break;

                                default:
                                    break;
                            }
                        }
                    }

                }
            });
        }

        private void GetCameraInfor(string bks)
        {
            StreamDevicesResponse deviceResponse = null;
            TryExecute(async () =>
            {
                deviceResponse = await _streamCameraService.GetDevicesStatus(ConditionType.BKS, bks);
                // only 1 data
                var deviceResponseData = deviceResponse?.Data?.FirstOrDefault();
                if (deviceResponseData == null)
                {
                    EventAggregator.GetEvent<SetCameraLayoutEvent>().Publish(0);
                }
                if (deviceResponseData != null)
                {
                    currentXnCode = deviceResponseData.XnCode;
                    currentIMEI = deviceResponseData.IMEI;
                    CurrentAddress = await _geocodeService.GetAddressByLatLng(deviceResponseData.Latitude.ToString(), deviceResponseData.Longitude.ToString());
                    CurrentTime = deviceResponseData.DeviceTime;
                    // camrera active filter => set layout 1 2 4
                    var cameraActiveQuantity = deviceResponseData.CameraChannels?.Where(x => x.IsPlug).ToList();
                    SetLayoutDependCameraQuantity(cameraActiveQuantity);

                    for (int index = 0; index < cameraActiveQuantity.Count; index++)
                    {
                        var data = cameraActiveQuantity[index];
                        RequestStartCam(data.Channel, (CameraEnum)index);
                    }
                }
            });
        }

        private void SetLayoutDependCameraQuantity(List<CameraChannel> cameraActiveQuantity)
        {
            if (cameraActiveQuantity != null && cameraActiveQuantity.Count > 0)
            {
                if (cameraActiveQuantity.Count == 1)
                {
                    EventAggregator.GetEvent<SetCameraLayoutEvent>().Publish(1);
                }
                else if (cameraActiveQuantity.Count == 2)
                {
                    EventAggregator.GetEvent<SetCameraLayoutEvent>().Publish(2);
                }
                else if (cameraActiveQuantity.Count == 3 || cameraActiveQuantity.Count == 4)
                {
                    EventAggregator.GetEvent<SetCameraLayoutEvent>().Publish(4);
                }
            }
            else
            {
                EventAggregator.GetEvent<SetCameraLayoutEvent>().Publish(0);
            }
        }

        private void ShowVideoView(CameraEnum camera)
        {
            currentCamera.Add(camera);
            if (timer == null)
            {
                timer = new System.Timers.Timer();
                timer.Interval = 1000;
                timer.Elapsed += Timer_Elapsed;
                timer.Start();
            }
        }

        private void RequestMoreTimeStream(int minutes)
        {
            if (SelectedCamera != null)
            {
                switch (SelectedCamera)
                {
                    case CameraEnum.CAM1:
                        TotalTimeCam1 += minutes * 60;
                        break;

                    case CameraEnum.CAM2:
                        TotalTimeCam2 += minutes * 60;
                        break;

                    case CameraEnum.CAM3:
                        TotalTimeCam3 += minutes * 60;
                        break;

                    case CameraEnum.CAM4:
                        TotalTimeCam4 += minutes * 60;
                        break;
                }
            }
        }

        public override void OnResume()
        {
            base.OnResume();
            DisposeAllMediaPlayer();
            ReLoadCamera();
            DependencyService.Get<IScreenOrientServices>().ForcePortrait();
        }

        private void ReLoadCamera()
        {
            try
            {
                currentCamera.Clear();
                SelectedCamera = null;

                TotalTimeCam1 = 1;
                TotalTimeCam2 = 1;
                TotalTimeCam3 = 1;
                TotalTimeCam4 = 1;

                cam1LoadingTime.Reset();
                cam2LoadingTime.Reset();
                cam3LoadingTime.Reset();
                cam4LoadingTime.Reset();

                IsCAM1Error = false;
                IsCAM2Error = false;
                IsCAM3Error = false;
                IsCAM4Error = false;

                stopLoad = true;
                GetCameraInfor(VehicleSelectedPlate);

                SelectedCamera = currentCamera.FirstOrDefault();
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteError("ReloadCamera", ex);
            }
        }

        private async Task SendRequestTime(int timeSecond, int chanel)
        {
            var response = await _streamCameraService.RequestMoreStreamTime(new StreamPingRequest()
            {
                xnCode = currentXnCode,
                Duration = timeSecond,
                VehiclePlate = VehicleSelectedPlate,
                Channel = chanel
            });
            if (!response.Data) // false : try request again
            {
                await SendRequestTime(timeSecond, chanel);
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            counterRequestMoreTime--;
            if (counterRequestMoreTime == 0)
            {
                counterRequestMoreTime = 15;
                UpdateTimeAndLocation();
            }
            if (AutoAddTime)
            {
                if (counterRequestMoreTime == 5)
                {
                    TryExecute(async () =>
                    {
                        foreach (var item in currentCamera)
                        {
                            var chanel = (int)item + 1;
                            await SendRequestTime(600, chanel);
                        }
                    });
                }
            }
            else
            {
                foreach (var item in currentCamera)
                {
                    Task.Run(async () =>
                    {
                        switch (item)
                        {
                            case CameraEnum.CAM1:
                                if (TotalTimeCam1 == 0)
                                {
                                    currentCamera.Remove(item);
                                }
                                else
                                {
                                    TotalTimeCam1 -= 1;
                                }
                                var a = TotalTimeCam1;
                                if (TotalTimeCam1 % 10 == 0 && TotalTimeCam1 > maxTimeCameraRemain)
                                {
                                    await SendRequestTime(600, 1);
                                }
                                break;

                            case CameraEnum.CAM2:
                                if (TotalTimeCam2 == 0)
                                {
                                    currentCamera.Remove(item);
                                }
                                else
                                {
                                    TotalTimeCam2 -= 1;
                                }

                                if (TotalTimeCam2 % 10 == 0 && TotalTimeCam2 > maxTimeCameraRemain)
                                {
                                    await SendRequestTime(600, 2);
                                }
                                break;

                            case CameraEnum.CAM3:
                                if (TotalTimeCam3 == 0)
                                {
                                    currentCamera.Remove(item);
                                }
                                else
                                {
                                    TotalTimeCam3 -= 1;
                                }

                                if (TotalTimeCam3 % 10 == 0 && TotalTimeCam3 > maxTimeCameraRemain)
                                {
                                    await SendRequestTime(600, 3);
                                }
                                break;

                            case CameraEnum.CAM4:
                                if (TotalTimeCam4 == 0)
                                {
                                    currentCamera.Remove(item);
                                }
                                else
                                {
                                    TotalTimeCam4 -= 1;
                                }

                                if (TotalTimeCam4 % 10 == 0 && TotalTimeCam4 > maxTimeCameraRemain)
                                {
                                    await SendRequestTime(600, 4);
                                }
                                break;
                        }
                    });
                }
            }
        }
        private void UpdateTimeAndLocation()
        {

            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    var deviceResponse = await _streamCameraService.GetDevicesStatus(ConditionType.BKS, VehicleSelectedPlate);
                    var deviceResponseData = deviceResponse?.Data?.FirstOrDefault();
                    if (deviceResponseData !=null)
                    {
                        CurrentAddress = await _geocodeService.GetAddressByLatLng(deviceResponseData.Latitude.ToString(), deviceResponseData.Longitude.ToString());
                        CurrentTime = deviceResponseData.DeviceTime;
                    }                   
                }
                catch (Exception ex)
                {

                    
                }

            });
        }

        private bool CanExcute()
        {
            switch (SelectedCamera)
            {
                case CameraEnum.CAM1:
                    if (MediaPlayerNo1 != null && MediaPlayerNo1.Time > 0)
                    {
                        return true;
                    }
                    return false;

                case CameraEnum.CAM2:
                    if (MediaPlayerNo2 != null && MediaPlayerNo2.Time > 0)
                    {
                        return true;
                    }
                    return false;

                case CameraEnum.CAM3:
                    if (MediaPlayerNo3 != null && MediaPlayerNo3.Time > 0)
                    {
                        return true;
                    }
                    return false;

                case CameraEnum.CAM4:
                    if (MediaPlayerNo4 != null && MediaPlayerNo4.Time > 0)
                    {
                        return true;
                    }
                    return false;

                default:
                    return false;
            }
        }

        private void DisposeMediaPlayer(CameraEnum cam)
        {
            try
            {
                switch (cam)
                {
                    case CameraEnum.CAM1:
                        if (MediaPlayerNo1 != null)
                        {
                            var mediaPlayer1 = MediaPlayerNo1;
                            MediaPlayerNo1 = null;
                            mediaPlayer1?.Dispose();
                        }
                        break;
                    case CameraEnum.CAM2:
                        if (MediaPlayerNo2 != null)
                        {
                            var mediaPlayer2 = MediaPlayerNo2;
                            MediaPlayerNo2 = null;
                            mediaPlayer2?.Dispose();
                        }

                        break;
                    case CameraEnum.CAM3:
                        if (MediaPlayerNo3 != null)
                        {
                            var mediaPlayer3 = MediaPlayerNo3;
                            MediaPlayerNo3 = null;
                            mediaPlayer3?.Dispose();
                        }

                        break;
                    case CameraEnum.CAM4:
                        if (MediaPlayerNo4 != null)
                        {
                            var mediaPlayer4 = MediaPlayerNo4;
                            MediaPlayerNo4 = null;
                            mediaPlayer4?.Dispose();
                        }

                        break;
                }
            }
            catch (Exception ex)
            {


            }

        }
        private void DisposeAllMediaPlayer()
        {
            stopLoad = false;
            DisposeMediaPlayer(CameraEnum.CAM1);
            DisposeMediaPlayer(CameraEnum.CAM2);
            DisposeMediaPlayer(CameraEnum.CAM3);
            DisposeMediaPlayer(CameraEnum.CAM4);
        }
        public override void OnDestroy()
        {

            DependencyService.Get<IScreenOrientServices>().ForcePortrait();
            DisposeAllMediaPlayer();
            LibVLC?.Dispose();
            LibVLC = null;

            if (timer != null)
            {
                timer.Elapsed -= Timer_Elapsed;
                timer.Stop();
                timer.Dispose();
            }

        }
    }

    public class CameraManagement : BindableBase
    {
        private long totalTime;
        public long TotalTime
        {
            get { return totalTime; }
            set
            {
                SetProperty(ref totalTime, value);
                RaisePropertyChanged();
            }
        }

        private bool isError;
        public bool IsError
        {
            get { return isError; }
            set
            {
                SetProperty(ref isError, value);
                RaisePropertyChanged();
            }
        }
        private StreamStart data;
        public StreamStart Data
        {
            get { return data; }
            set
            {
                SetProperty(ref data, value);
                RaisePropertyChanged();
            }
        }

        private CameraEnum currentPosition;
        public CameraEnum CurrentPosition
        {
            get { return currentPosition; }
            set
            {
                SetProperty(ref currentPosition, value);
                RaisePropertyChanged();
            }
        }

        private double loadingTime;
        public double LoadingTime
        {
            get { return loadingTime; }
            set { SetProperty(ref loadingTime, value); }
        }

        private MediaPlayer mediaPlayer;
        public MediaPlayer MediaPlayer
        {
            get { return mediaPlayer; }
            set
            {
                SetProperty(ref mediaPlayer, value);
                RaisePropertyChanged();
            }
        }

    }
}