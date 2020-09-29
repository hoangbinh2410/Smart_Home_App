using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Interfaces;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Core.Views.Camera.MonitoringCamera;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service.IService;
using BA_MobileGPS.Service.Service;
using LibVLCSharp.Shared;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services.Dialogs.Popups;
using Syncfusion.XlsIO.Parser.Biff_Records;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class CameraManagingPageViewModel : ViewModelBase
    {
        private readonly Stopwatch stopwatchCam1 = new Stopwatch();
        private readonly Stopwatch stopwatchCam2 = new Stopwatch();
        private readonly string playIconSource = "ic_play_arrow_white.png";
        private readonly string stopIconSource = "ic_stop_white.png";
        private readonly string volumeIconSource = "ic_volumespeaker";
        private readonly string muteIconSource = "ic_mute";
        private string videoUrl1 = string.Empty;
        private string videoUrl2 = string.Empty;
        private string videoUrl3 = string.Empty;
        private string videoUrl4 = string.Empty;
        private string currentXnCode { get; set; }
        private string currentVehiclePlate { get; set; }
        private long totalTimeCam1 { get; set; }
        private long totalTimeCam2 { get; set; }
        private long totalTimeCam3 { get; set; }
        private long totalTimeCam4 { get; set; }
        private const int maxTimeCameraRemain = 600000; //milisecond
        private const int pingRequestDelayTime = 30000; //milisecond : time between two request extra time for current video

        private List<CameraEnum> currentCamera { get; set; } = new List<CameraEnum>();

        private readonly IStreamCameraService _streamCameraService;

        public CameraManagingPageViewModel(INavigationService navigationService, IStreamCameraService streamCameraService) : base(navigationService)
        {
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
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            // Đóng busy indicator
            IsCam1Loaded = true;
            IsCam2Loaded = true;
            IsCam3Loaded = true;
            IsCam4Loaded = true;
            if (parameters.ContainsKey(ParameterKey.Vehicle) && parameters.GetValue<Vehicle>(ParameterKey.Vehicle) is Vehicle vehiclePlate)
            {
                if (!string.IsNullOrEmpty(VehicleSelectedPlate) || VehicleSelectedPlate != vehiclePlate.VehiclePlate)
                {
                    // reset selected cam
                    SelectedCamera = null;
                    var allCams = new List<CameraEnum>()
                        {
                            CameraEnum.CAM1,
                            CameraEnum.CAM2,
                            CameraEnum.CAM3,
                            CameraEnum.CAM4,
                        };
                    EventAggregator.GetEvent<HideVideoViewEvent>().Publish(allCams);
                    VehicleSelectedPlate = vehiclePlate.VehiclePlate;
                    GetCameraInfor("BACAM1409");
                    SelectedCamera = currentCamera.FirstOrDefault();

                    Cam1ReloadCount = 0;
                    Cam2ReloadCount = 0;
                    Cam3ReloadCount = 0;
                    Cam4ReloadCount = 0;
                }
            }
            if (parameters.ContainsKey(ParameterKey.RequestTime) && parameters.GetValue<int>(ParameterKey.RequestTime) is int time)
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

        private int cam1ReloadCount;

        public int Cam1ReloadCount // Time before video play
        {
            get { return cam1ReloadCount; }
            set
            {
                SetProperty(ref cam1ReloadCount, value);
                RaisePropertyChanged();
            }
        }

        private int cam2ReloadCount;

        public int Cam2ReloadCount // Time before video play
        {
            get { return cam2ReloadCount; }
            set
            {
                SetProperty(ref cam2ReloadCount, value);
                RaisePropertyChanged();
            }
        }

        private int cam3ReloadCount;

        public int Cam3ReloadCount // Time before video play
        {
            get { return cam3ReloadCount; }
            set
            {
                SetProperty(ref cam3ReloadCount, value);
                RaisePropertyChanged();
            }
        }

        private int cam4ReloadCount;

        public int Cam4ReloadCount // Time before video play
        {
            get { return cam4ReloadCount; }
            set
            {
                SetProperty(ref cam4ReloadCount, value);
                RaisePropertyChanged();
            }
        }

        public ICommand PlayTappedCommand { get; }

        private void PlayTapped()
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

        private void SetUpVlc()
        {
            LibVLCSharp.Shared.Core.Initialize();
            LibVLC = new LibVLC();
        }

        private void InitCamera1(string url)
        {
            IsCam1Loaded = false;
            MediaPlayerNo1 = null;
            var mediaNo1 = new Media(LibVLC, new Uri(url));
            MediaPlayerNo1 = new MediaPlayer(mediaNo1) { AspectRatio = "4:3", Scale = 0, Mute = true };
            MediaPlayerNo1.TimeChanged += MediaPlayerNo1_TimeChanged;
            MediaPlayerNo1.EncounteredError += MediaPlayerNo1_EncounteredError;
            MediaPlayerNo1.Play();
            stopwatchCam1.Start();
        }

        private void MediaPlayerNo1_EncounteredError(object sender, EventArgs e)
        {
            TryExecute(async () =>
            {
                stopwatchCam1.Stop();
                await Task.Delay(2000);
                MediaPlayerNo1.EncounteredError -= MediaPlayerNo1_EncounteredError;
                MediaPlayerNo1.TimeChanged -= MediaPlayerNo1_TimeChanged;
                InitCamera1(videoUrl1);
            });
            Cam1ReloadCount += 2;
        }

        private void MediaPlayerNo1_TimeChanged(object sender, MediaPlayerTimeChangedEventArgs e)
        {
            if (e.Time > 0 && !IsCam1Loaded)
            {
                PlayButtonIconSource = stopIconSource;
                IsCam1Loaded = true;
                ShowVideoView(CameraEnum.CAM1);
                stopwatchCam1.Stop();
                Cam1ReloadCount += (int)(stopwatchCam1.ElapsedMilliseconds / 1000);
                totalTimeCam1 = 3;
            }
            else
            {
                if (e.Time % pingRequestDelayTime == 0) // check ping 30000 ms per time
                {
                    var remain = totalTimeCam1 * 60 * 1000 - e.Time;
                    if (remain > maxTimeCameraRemain - pingRequestDelayTime)
                    {
                        TryExecute(async () =>
                        {
                            await SendRequestTime(600, 1);
                        });
                    }
                }
            }
        }

        private void InitCamera2(string url)
        {
            IsCam2Loaded = false;
            MediaPlayerNo2 = null;
            var mediaNo1 = new Media(LibVLC, new Uri(url));
            MediaPlayerNo2 = new MediaPlayer(mediaNo1) { AspectRatio = "4:3", Scale = 0, Mute = true };
            MediaPlayerNo2.TimeChanged += MediaPlayerNo2_TimeChanged;
            MediaPlayerNo2.EncounteredError += MediaPlayerNo2_EncounteredError;
            MediaPlayerNo2.Play();
            stopwatchCam2.Start();
        }

        private void MediaPlayerNo2_EncounteredError(object sender, EventArgs e)
        {
            TryExecute(async () =>
            {
                stopwatchCam2.Stop();
                await Task.Delay(2000);
                MediaPlayerNo2.EncounteredError -= MediaPlayerNo2_EncounteredError;
                MediaPlayerNo2.TimeChanged -= MediaPlayerNo2_TimeChanged;
                InitCamera2(videoUrl2);
            });
            Cam2ReloadCount += 2;
        }

        private void MediaPlayerNo2_TimeChanged(object sender, MediaPlayerTimeChangedEventArgs e)
        {
            if (e.Time > 0 && !IsCam2Loaded)
            {
                PlayButtonIconSource = stopIconSource;
                IsCam2Loaded = true;
                ShowVideoView(CameraEnum.CAM2);
                stopwatchCam2.Stop();
                Cam2ReloadCount += (int)(stopwatchCam1.ElapsedMilliseconds / 1000);
                totalTimeCam2 = 3;
            }
            else
            {
                if (e.Time % pingRequestDelayTime == 0) // check ping 30000 ms per time
                {
                    var remain = totalTimeCam2 * 60 * 1000 - e.Time;
                    if (remain > maxTimeCameraRemain - pingRequestDelayTime)
                    {
                        TryExecute(async () =>
                        {
                            await SendRequestTime(600, 2);
                        });
                    }
                }
            }
        }

        private void InitCamera3(string url)
        {
            IsCam3Loaded = false;
            MediaPlayerNo3 = null;
            var mediaNo1 = new Media(LibVLC, new Uri(url));
            MediaPlayerNo3 = new MediaPlayer(mediaNo1) { AspectRatio = "4:3", Scale = 0, Mute = true };
            MediaPlayerNo3.TimeChanged += MediaPlayerNo3_TimeChanged;
            MediaPlayerNo3.EncounteredError += MediaPlayerNo3_EncounteredError;
            MediaPlayerNo3.Play();
        }

        private void MediaPlayerNo3_EncounteredError(object sender, EventArgs e)
        {
            TryExecute(async () =>
            {
                await Task.Delay(2000);
                MediaPlayerNo3.EncounteredError -= MediaPlayerNo3_EncounteredError;
                MediaPlayerNo3.TimeChanged -= MediaPlayerNo3_TimeChanged;
                InitCamera3(videoUrl3);
            });
            Cam3ReloadCount += 2;
        }

        private void MediaPlayerNo3_TimeChanged(object sender, MediaPlayerTimeChangedEventArgs e)
        {
            if (e.Time > 0 && !IsCam3Loaded)
            {
                PlayButtonIconSource = stopIconSource;
                IsCam3Loaded = true;
                ShowVideoView(CameraEnum.CAM3);
                totalTimeCam3 = 3;
            }
            else
            {
                if (e.Time % pingRequestDelayTime == 0) // check ping 30000 ms per time
                {
                    var remain = totalTimeCam3 * 60 * 1000 - e.Time;
                    if (remain > maxTimeCameraRemain - pingRequestDelayTime)
                    {
                        TryExecute(async () =>
                        {
                            await SendRequestTime(600, 3);
                        });
                    }
                }
            }
        }

        private void InitCamera4(string url)
        {
            IsCam4Loaded = false;
            MediaPlayerNo4 = null;
            var mediaNo1 = new Media(LibVLC, new Uri(url));
            MediaPlayerNo4 = new MediaPlayer(mediaNo1) { AspectRatio = "4:3", Scale = 0, Mute = true };
            MediaPlayerNo4.TimeChanged += MediaPlayerNo4_TimeChanged;
            MediaPlayerNo4.EncounteredError += MediaPlayerNo4_EncounteredError;
            MediaPlayerNo4.Play();
        }

        private void MediaPlayerNo4_EncounteredError(object sender, EventArgs e)
        {
            TryExecute(async () =>
            {
                await Task.Delay(2000);
                MediaPlayerNo4.EncounteredError -= MediaPlayerNo4_EncounteredError;
                MediaPlayerNo4.TimeChanged -= MediaPlayerNo4_TimeChanged;
                InitCamera4(videoUrl4);
            });
            Cam4ReloadCount += 2;
        }

        private void MediaPlayerNo4_TimeChanged(object sender, MediaPlayerTimeChangedEventArgs e)
        {
            if (e.Time > 0 && !IsCam4Loaded)
            {
                PlayButtonIconSource = stopIconSource;
                IsCam4Loaded = true;
                ShowVideoView(CameraEnum.CAM4);
                totalTimeCam4 = 3;
            }
            else
            {
                if (e.Time % pingRequestDelayTime == 0) // check ping 30000 ms per time
                {
                    var remain = totalTimeCam4 * 60 * 1000 - e.Time;
                    if (remain > maxTimeCameraRemain - pingRequestDelayTime)
                    {
                        TryExecute(async () =>
                        {
                            await SendRequestTime(600, 4);
                        });                        
                    }                   
                }
            }
        }

        public ICommand VolumeChangedCommand { get; }

        private void VolumeChanged()
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

        public ICommand CameraFrameTappedCommand { get; }

        private void CameraFrameTapped(object obj)
        {
            var camObj = (CameraEnum)obj;
            if (currentCamera.Contains(camObj))
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
            }
        }

        public ICommand RequestTimeTappedCommand { get; set; }

        private void RequestTimeTapped()
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("RequestMoreTimePopup");
            });
        }

        public ICommand FullScreenTappedCommand { get; }

        private void FullScreenTapped()
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
                DependencyService.Get<IScreenOrientServices>().ForceLandscape();
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
            }
        }

        public ICommand ScreenShotTappedCommand { get; }
        private void ScreenShotTapped()
        {
            TryExecute(async () =>
           {
               var photoPermission = await PermissionHelper.CheckPhotoPermissions();
               var storagePermission = await PermissionHelper.CheckStoragePermissions();
               if (photoPermission && storagePermission)
               {
                   var folderPath = DependencyService.Get<ICameraSnapShotServices>().GetFolderPath();
                   var current = DateTime.Now.ToString("yyyyMMddHHmmss");
                   var fileName = Enum.GetName(typeof(CameraEnum), SelectedCamera) + current + ".jpg";
                   var filePath = Path.Combine(folderPath, fileName);
                   switch (SelectedCamera)
                   {
                       case CameraEnum.CAM1:
                           MediaPlayerNo1.TakeSnapshot(0, filePath, 0, 0);
                           bool doesExist1 = File.Exists(filePath);
                           break;

                       case CameraEnum.CAM2:

                           break;

                       case CameraEnum.CAM3:

                           break;

                       case CameraEnum.CAM4:

                           break;
                   }
               }


           });                                  
        }

        private void GetCameraInfor(string bks)
        {
            using (new HUDService())
            {
                TryExecute(async () =>
                {
                    var deviceResponse = await _streamCameraService.GetDevicesStatus(ConditionType.BKS, bks);
                    // only 1 data
                    var deviceResponseData = deviceResponse?.Data?.FirstOrDefault();
                    if (deviceResponseData != null)
                    {
                        currentXnCode = deviceResponseData.XnCode;
                        currentVehiclePlate = deviceResponseData.VehiclePlate;
                        // camrera active filter
                        var cameraActiveQuantity = deviceResponseData.CameraChannels?.Where(x => x.IsPlug).ToList();
                        foreach (var data in cameraActiveQuantity)
                        {
                            TryExecute(async () =>
                            {
                                var request = new StreamStartRequest()
                                {
                                    Channel = data.Channel,
                                    xnCode = deviceResponseData.XnCode,
                                    VehiclePlate = bks
                                };
                                var countRequest = 0;
                                var temp = true;
                                StreamStart camResponseData = null;
                                while (temp)
                                {
                                    countRequest += 1;
                                    var camResponse = await _streamCameraService.StartStream(request);

                                    var checkResult = await _streamCameraService.GetDevicesStatus(ConditionType.BKS, bks);
                                    if ((bool)checkResult?.Data?.FirstOrDefault()?.CameraChannels.FirstOrDefault(x => x.Channel == data.Channel).IsStreaming)
                                    {
                                        camResponseData = camResponse.Data.FirstOrDefault();
                                        temp = false;
                                    }
                                    // wait 1s
                                    await Task.Delay(1000);

                                    switch (data.Channel)
                                    {
                                        case 1:
                                            //Bật busy indicator
                                            IsCam1Loaded = false;
                                            Cam1ReloadCount += 1;
                                            break;

                                        case 2:
                                            IsCam2Loaded = false;
                                            Cam2ReloadCount += 1;
                                            break;

                                        case 3:
                                            IsCam3Loaded = false;
                                            Cam3ReloadCount += 1;
                                            break;

                                        case 4:
                                            IsCam4Loaded = false;
                                            Cam4ReloadCount += 1;
                                            break;

                                        default:
                                            break;
                                    }
                                }

                                if (camResponseData != null)
                                {
                                    switch (camResponseData.Channel)
                                    {
                                        case 1:
                                            videoUrl1 = camResponseData.Link;
                                            InitCamera1(videoUrl1);
                                            break;

                                        case 2:
                                            videoUrl2 = camResponseData.Link;
                                            InitCamera2(videoUrl2);
                                            break;

                                        case 3:
                                            videoUrl3 = camResponseData.Link;
                                            InitCamera3(videoUrl3);
                                            break;

                                        case 4:
                                            videoUrl4 = camResponseData.Link;
                                            InitCamera4(videoUrl4);
                                            break;

                                        default:
                                            break;
                                    }
                                }
                                else
                                {
                                    // data hav isssue
                                }
                            });
                        }
                    }
                    else
                    {
                        // not response vehicle data
                    }
                });
            }
        }

        private void ShowVideoView(CameraEnum camera)
        {
            EventAggregator.GetEvent<ShowVideoViewEvent>().Publish(new List<CameraEnum>() { camera });
            currentCamera.Add(camera);
        }

        private void RequestMoreTimeStream(int minutes)
        {
            if (SelectedCamera != null)
            {
                switch (SelectedCamera)
                {
                    case CameraEnum.CAM1:
                        totalTimeCam1 += minutes;
                        break;

                    case CameraEnum.CAM2:
                        totalTimeCam2 += minutes;
                        break;

                    case CameraEnum.CAM3:
                        totalTimeCam3 += minutes;
                        break;

                    case CameraEnum.CAM4:
                        totalTimeCam4 += minutes;
                        break;
                }
            }
        }

        private async Task SendRequestTime(int timeSecond, int chanel)
        {
            var response = await _streamCameraService.RequestMoreStreamTime(new StreamPingRequest()
            {
                xnCode = currentXnCode,
                Duration = timeSecond,
                VehiclePlate = currentVehiclePlate,
                Channel = chanel
            });
            if (!response.Data) // false : try request again
            {
                await SendRequestTime(timeSecond, chanel);
            }          
        }

        public override void OnDestroy()
        {
            DependencyService.Get<IScreenOrientServices>().ForcePortrait();
            var mediaPlayer1 = MediaPlayerNo1;
            MediaPlayerNo1 = null;
            mediaPlayer1?.Dispose();

            var mediaPlayer2 = MediaPlayerNo2;
            MediaPlayerNo2 = null;
            mediaPlayer2?.Dispose();

            var mediaPlayer3 = MediaPlayerNo3;
            MediaPlayerNo3 = null;
            mediaPlayer3?.Dispose();

            var mediaPlayer4 = MediaPlayerNo4;
            MediaPlayerNo4 = null;
            mediaPlayer4?.Dispose();

            LibVLC?.Dispose();
            LibVLC = null;
        }
    }
}