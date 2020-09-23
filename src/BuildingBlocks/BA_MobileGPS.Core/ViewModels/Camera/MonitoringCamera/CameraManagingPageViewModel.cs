using BA_MobileGPS.Core.Constant;
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
        private readonly string videoUrl1 = "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ForBiggerJoyrides.mp4";
        private readonly string videoUrl2 = "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/Sintel.mp4";
        private readonly string videoUrl3 = "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/SubaruOutbackOnStreetAndDirt.mp4";
        private readonly string videoUrl4 = "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/TearsOfSteel.mp4";

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
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey(ParameterKey.Vehicle) && parameters.GetValue<Vehicle>(ParameterKey.Vehicle) is Vehicle vehiclePlate)
            {
                try
                {
                    if (!string.IsNullOrEmpty(VehicleSelectedPlate) || VehicleSelectedPlate != vehiclePlate.VehiclePlate)
                    {
                        VehicleSelectedPlate = vehiclePlate.VehiclePlate;
                        //GetCameraInfor("BACAM1409");
                        var allCams = new List<CameraEnum>()
                        {
                            CameraEnum.FirstCamera,
                            CameraEnum.SecondCamera,
                            CameraEnum.ThirdCamera,
                            CameraEnum.FourthCamera,
                        };
                        EventAggregator.GetEvent<HideVideoViewEvent>().Publish(allCams);
                                               
                        if (vehiclePlate.VehiclePlate == "98B00048")
                        {
                            InitCamera1(videoUrl1);
                            InitCamera2(videoUrl2);
                            InitCamera3(videoUrl3);
                            InitCamera4(videoUrl4);
                            currentCamera = new List<CameraEnum>()
                            {
                                CameraEnum.FirstCamera,
                                CameraEnum.SecondCamera,
                                CameraEnum.ThirdCamera,
                                CameraEnum.FourthCamera,
                            };
                            EventAggregator.GetEvent<ShowVideoViewEvent>().Publish(currentCamera);
                        }
                        else if (vehiclePlate.VehiclePlate == "98B00821")
                        {
                            InitCamera1(videoUrl3);
                            InitCamera2(videoUrl3);
                            InitCamera3(videoUrl3);
                            currentCamera = new List<CameraEnum>()
                            {
                                CameraEnum.FirstCamera,
                                CameraEnum.SecondCamera,
                                CameraEnum.ThirdCamera,
                            };
                            EventAggregator.GetEvent<ShowVideoViewEvent>().Publish(currentCamera);
                        }
                        else if (vehiclePlate.VehiclePlate == "98B00874")
                        {
                            InitCamera3(videoUrl2);
                            InitCamera4(videoUrl1);
                            currentCamera = new List<CameraEnum>()
                            {
                                CameraEnum.ThirdCamera,
                                CameraEnum.FourthCamera
                            };
                            EventAggregator.GetEvent<ShowVideoViewEvent>().Publish(currentCamera);
                        }
                        else
                        {
                            InitCamera4(videoUrl4);
                            currentCamera = new List<CameraEnum>()
                            {
                                CameraEnum.FourthCamera
                            };
                            EventAggregator.GetEvent<ShowVideoViewEvent>().Publish(currentCamera);
                        }
                        SelectedCamera = currentCamera.First();
                        PlayButtonIconSource = stopIconSource;
                    }
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
            else
            {
                // Đóng busy indicator
                IsCam1Loaded = true;
                IsCam2Loaded = true;
                IsCam3Loaded = true;
                IsCam4Loaded = true;
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
            MediaPlayerNo1.Play();
        }

        private void MediaPlayerNo1_TimeChanged(object sender, MediaPlayerTimeChangedEventArgs e)
        {
            if (e.Time > 0)
            {
                IsCam1Loaded = true;
                MediaPlayerNo1.TimeChanged -= MediaPlayerNo1_TimeChanged;
            }
        }
        private void InitCamera2(string url)
        {
            IsCam2Loaded = false;
            var mediaNo1 = new Media(LibVLC, new Uri(url));
            MediaPlayerNo2 = new MediaPlayer(mediaNo1) { AspectRatio = "4:3", Scale = 0, Mute = true };
            MediaPlayerNo2.TimeChanged += MediaPlayerNo2_TimeChanged;
            MediaPlayerNo2.Play();
        }
        private void MediaPlayerNo2_TimeChanged(object sender, MediaPlayerTimeChangedEventArgs e)
        {
            if (e.Time > 0)
            {
                IsCam2Loaded = true;
                MediaPlayerNo2.TimeChanged -= MediaPlayerNo2_TimeChanged;
            }
        }
        private void InitCamera3(string url)
        {
            IsCam3Loaded = false;
            var mediaNo1 = new Media(LibVLC, new Uri(url));
            MediaPlayerNo3 = new MediaPlayer(mediaNo1) { AspectRatio = "4:3", Scale = 0, Mute = true };
            MediaPlayerNo3.TimeChanged += MediaPlayerNo3_TimeChanged;
            MediaPlayerNo3.Play();
        }
        private void MediaPlayerNo3_TimeChanged(object sender, MediaPlayerTimeChangedEventArgs e)
        {
            if (e.Time > 0)
            {
                IsCam3Loaded = true;
                MediaPlayerNo3.TimeChanged -= MediaPlayerNo3_TimeChanged;
            }
        }

        private void InitCamera4(string url)
        {
            IsCam4Loaded = false;
            var mediaNo1 = new Media(LibVLC, new Uri(url));
            MediaPlayerNo4 = new MediaPlayer(mediaNo1) { AspectRatio = "4:3", Scale = 0, Mute = true };
            MediaPlayerNo4.TimeChanged += MediaPlayerNo4_TimeChanged;
            MediaPlayerNo4.Play();
        }

        private void MediaPlayerNo4_TimeChanged(object sender, MediaPlayerTimeChangedEventArgs e)
        {
            if (e.Time > 0)
            {
                IsCam4Loaded = true;
                MediaPlayerNo4.TimeChanged -= MediaPlayerNo4_TimeChanged;
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

        //private void GetCameraInfor(string bks)
        //{
        //    using (new HUDService())
        //    {
        //        TryExecute(async () =>
        //        {
        //            var deviceResponse = await _streamCameraService.GetDevicesStatus(ConditionType.BKS, bks);
        //            // only 1 data
        //            var deviceResponseData = deviceResponse?.Data?.FirstOrDefault();
        //            if (deviceResponseData != null)
        //            {
        //                var cameraActiveQuantity = deviceResponseData.CameraChannels?.Where(x => x.IsPlug).ToList();

        //                foreach (var data in cameraActiveQuantity)
        //                {
        //                    var request = new StreamStartRequest()
        //                    {
        //                        Channel = data.Channel,
        //                        xnCode = deviceResponseData.XnCode,
        //                        VehiclePlate = bks
        //                    };
        //                    var camResponse = await _streamCameraService.StartStream(request);
        //                    var camResponseData = camResponse?.Data?.FirstOrDefault();
        //                    if (camResponseData != null)
        //                    {
        //                        Device.BeginInvokeOnMainThread(() =>
        //                        {
        //                            switch (camResponseData.Channel)
        //                            {
        //                                case 1:
        //                                    InitCamera1(camResponseData.Link);
        //                                    break;
        //                                case 2:
        //                                    InitCamera2(camResponseData.Link);
        //                                    break;
        //                                case 3:
        //                                    InitCamera3(camResponseData.Link);
        //                                    break;
        //                                case 4:
        //                                    InitCamera4(camResponseData.Link);
        //                                    break;
        //                                default:
        //                                    break;
        //                            }
        //                        });
        //                    }
        //                }
        //            }
        //        });
        //    }
        //}
        public override void OnDestroy()
        {
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
