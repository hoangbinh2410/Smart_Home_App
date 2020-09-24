using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Interfaces;
using BA_MobileGPS.Core.Views.Camera.MonitoringCamera;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service.IService;
using BA_MobileGPS.Service.Service;
using LibVLCSharp.Shared;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class CameraManagingPageViewModel : ViewModelBase
    {
        private readonly string playIconSource = "ic_play_arrow_white.png";
        private readonly string stopIconSource = "ic_stop_white.png";
        private readonly string volumeIconSource = "ic_volumespeaker";
        private readonly string muteIconSource = "ic_mute";
        private string videoUrl1 = "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ForBiggerJoyrides.mp4";
        private string videoUrl2 = "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/Sintel.mp4";
        private string videoUrl3 = "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/SubaruOutbackOnStreetAndDirt.mp4";
        private string videoUrl4 = "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/TearsOfSteel.mp4";

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
            SettingTappedCommand = new DelegateCommand(SettingTapped);
            FullScreenTappedCommand = new DelegateCommand(FullScreenTapped);
            isFullScreenOff = true;
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
                            CameraEnum.FirstCamera,
                            CameraEnum.SecondCamera,
                            CameraEnum.ThirdCamera,
                            CameraEnum.FourthCamera,
                        };
                    EventAggregator.GetEvent<HideVideoViewEvent>().Publish(allCams);
                    VehicleSelectedPlate = vehiclePlate.VehiclePlate;
                    GetCameraInfor("TESTTBI");
                    SelectedCamera = currentCamera.FirstOrDefault();

                    PlayButtonIconSource = stopIconSource;
                    cam1ReloadCount = 0;
                    cam2ReloadCount = 0;
                    cam3ReloadCount = 0;
                    cam4ReloadCount = 0;

                }
            }
            //else
            //{                          
            //    PlayButtonIconSource = playIconSource;
            //}
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

        private bool isPlayBackControlEnable;
        public bool IsPlayBackControlEnable
        {
            get { return isPlayBackControlEnable; }
            set
            {
                SetProperty(ref isPlayBackControlEnable, value);
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
        public int Cam1ReloadCount
        {
            get { return cam1ReloadCount; }
            set
            {
                SetProperty(ref cam1ReloadCount, value);
                RaisePropertyChanged();
            }
        }
        private int cam2ReloadCount;
        public int Cam2ReloadCount
        {
            get { return cam2ReloadCount; }
            set
            {
                SetProperty(ref cam2ReloadCount, value);
                RaisePropertyChanged();
            }
        }
        private int cam3ReloadCount;
        public int Cam3ReloadCount
        {
            get { return cam3ReloadCount; }
            set
            {
                SetProperty(ref cam3ReloadCount, value);
                RaisePropertyChanged();
            }
        }
        private int cam4ReloadCount;
        public int Cam4ReloadCount
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
                    case CameraEnum.FirstCamera:
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
                        break;
                    case CameraEnum.SecondCamera:
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
                        break;
                    case CameraEnum.ThirdCamera:
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
                        break;
                    case CameraEnum.FourthCamera:
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
            var mediaNo1 = new Media(LibVLC, new Uri(url));
            MediaPlayerNo1 = new MediaPlayer(mediaNo1) { AspectRatio = "4:3", Scale = 0, Mute = true };
            MediaPlayerNo1.TimeChanged += MediaPlayerNo1_TimeChanged;
            MediaPlayerNo1.EncounteredError += MediaPlayerNo1_EncounteredError;
            MediaPlayerNo1.Play();
        }

        private void MediaPlayerNo1_EncounteredError(object sender, EventArgs e)
        {
            TryExecute(async () =>
            {
                await Task.Delay(2000);
                InitCamera1(videoUrl1);
            });
            Cam1ReloadCount += 2;
        }

        private void MediaPlayerNo1_TimeChanged(object sender, MediaPlayerTimeChangedEventArgs e)
        {
            if (e.Time > 0)
            {
                IsCam1Loaded = true;
                MediaPlayerNo1.TimeChanged -= MediaPlayerNo1_TimeChanged;
                ShowVideoView(CameraEnum.FirstCamera);
            }
        }
        private void InitCamera2(string url)
        {
            IsCam2Loaded = false;
            var mediaNo1 = new Media(LibVLC, new Uri(url));
            MediaPlayerNo2 = new MediaPlayer(mediaNo1) { AspectRatio = "4:3", Scale = 0, Mute = true };
            MediaPlayerNo2.TimeChanged += MediaPlayerNo2_TimeChanged;
            MediaPlayerNo2.EncounteredError += MediaPlayerNo2_EncounteredError; ;
            MediaPlayerNo2.Play();
        }

        private void MediaPlayerNo2_EncounteredError(object sender, EventArgs e)
        {
            TryExecute(async () =>
            {
                await Task.Delay(2000);
                InitCamera2(videoUrl2);
            });
            Cam2ReloadCount += 2;
        }

        private void MediaPlayerNo2_TimeChanged(object sender, MediaPlayerTimeChangedEventArgs e)
        {
            if (e.Time > 0)
            {
                IsCam2Loaded = true;
                MediaPlayerNo2.TimeChanged -= MediaPlayerNo2_TimeChanged;
                ShowVideoView(CameraEnum.SecondCamera);
            }
        }
        private void InitCamera3(string url)
        {
            IsCam3Loaded = false;
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
                InitCamera3(videoUrl3);
            });
            Cam3ReloadCount += 2;
        }

        private void MediaPlayerNo3_TimeChanged(object sender, MediaPlayerTimeChangedEventArgs e)
        {
            if (e.Time > 0)
            {
                IsCam3Loaded = true;
                MediaPlayerNo3.TimeChanged -= MediaPlayerNo3_TimeChanged;
                ShowVideoView(CameraEnum.ThirdCamera);
            }
        }

        private void InitCamera4(string url)
        {
            IsCam4Loaded = false;
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
                InitCamera4(videoUrl4);
            });
            Cam4ReloadCount += 2;
        }

        private void MediaPlayerNo4_TimeChanged(object sender, MediaPlayerTimeChangedEventArgs e)
        {
            if (e.Time > 0)
            {
                IsCam4Loaded = true;
                MediaPlayerNo4.TimeChanged -= MediaPlayerNo4_TimeChanged;
                ShowVideoView(CameraEnum.FourthCamera);
            }
        }

        public ICommand VolumeChangedCommand { get; }
        private void VolumeChanged()
        {
            switch (SelectedCamera)
            {
                case CameraEnum.FirstCamera:
                    MediaPlayerNo1.Mute = !MediaPlayerNo1.Mute;
                    VolumeButtonIconSource = MediaPlayerNo1.Mute ? muteIconSource : volumeIconSource;
                    break;
                case CameraEnum.SecondCamera:
                    MediaPlayerNo2.Mute = !MediaPlayerNo2.Mute;
                    VolumeButtonIconSource = MediaPlayerNo2.Mute ? muteIconSource : volumeIconSource;
                    break;
                case CameraEnum.ThirdCamera:
                    MediaPlayerNo3.Mute = !MediaPlayerNo3.Mute;
                    VolumeButtonIconSource = MediaPlayerNo3.Mute ? muteIconSource : volumeIconSource;
                    break;
                case CameraEnum.FourthCamera:
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
                    case CameraEnum.FirstCamera:
                        PlayButtonIconSource = MediaPlayerNo1.IsPlaying ? stopIconSource : playIconSource;
                        VolumeButtonIconSource = MediaPlayerNo1.Mute ? muteIconSource : volumeIconSource;
                        break;
                    case CameraEnum.SecondCamera:
                        PlayButtonIconSource = MediaPlayerNo2.IsPlaying ? stopIconSource : playIconSource;
                        VolumeButtonIconSource = MediaPlayerNo2.Mute ? muteIconSource : volumeIconSource;
                        break;
                    case CameraEnum.ThirdCamera:
                        PlayButtonIconSource = MediaPlayerNo3.IsPlaying ? stopIconSource : playIconSource;
                        VolumeButtonIconSource = MediaPlayerNo3.Mute ? muteIconSource : volumeIconSource;
                        break;
                    case CameraEnum.FourthCamera:
                        PlayButtonIconSource = MediaPlayerNo4.IsPlaying ? stopIconSource : playIconSource;
                        VolumeButtonIconSource = MediaPlayerNo4.Mute ? muteIconSource : volumeIconSource;
                        break;
                }
            }
        }

        public ICommand SettingTappedCommand { get; set; }

        private void SettingTapped()
        {

        }

        public ICommand FullScreenTappedCommand { get; }
        private void FullScreenTapped()
        {
            IsFullScreenOff = !IsFullScreenOff;
            if (IsFullScreenOff)
            {
                EventAggregator.GetEvent<SwitchToNormalScreenEvent>().Publish();
                DependencyService.Get<IScreenOrientServices>().ForcePortrait();
            }
            else
            {
                DependencyService.Get<IScreenOrientServices>().ForceLandscape();
                switch (SelectedCamera)
                {
                    case CameraEnum.FirstCamera:
                        EventAggregator.GetEvent<SwitchToFullScreenEvent>().Publish(CameraEnum.FirstCamera);
                        break;
                    case CameraEnum.SecondCamera:
                        EventAggregator.GetEvent<SwitchToFullScreenEvent>().Publish(CameraEnum.SecondCamera);
                        break;
                    case CameraEnum.ThirdCamera:
                        EventAggregator.GetEvent<SwitchToFullScreenEvent>().Publish(CameraEnum.ThirdCamera);
                        break;
                    case CameraEnum.FourthCamera:
                        EventAggregator.GetEvent<SwitchToFullScreenEvent>().Publish(CameraEnum.FourthCamera);
                        break;
                }
            }
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
                        // camrera active filter
                        var cameraActiveQuantity = deviceResponseData.CameraChannels?.Where(x => x.IsPlug).ToList();

                        foreach (var data in cameraActiveQuantity)
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
                                await Task.Delay(1000);
                                // wait 1s then request get to check Chanel result
                                var checkResult = await _streamCameraService.GetDevicesStatus(ConditionType.BKS, bks);
                                if ((bool)checkResult?.Data?.FirstOrDefault()?.CameraChannels.FirstOrDefault(x => x.Channel == data.Channel).IsStreaming)
                                {
                                    camResponseData = camResponse.Data.FirstOrDefault();
                                    temp = false;
                                }
                                switch (data.Channel)
                                {
                                    case 1:
                                        Cam1ReloadCount += 1;
                                        break;
                                    case 2:
                                        Cam2ReloadCount += 1;
                                        break;
                                    case 3:
                                        Cam3ReloadCount += 1;
                                        break;
                                    case 4:
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
