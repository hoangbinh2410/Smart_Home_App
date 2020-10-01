using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Interfaces;
using BA_MobileGPS.Core.Views;
using BA_MobileGPS.Core.Views.Camera.MonitoringCamera;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service.IService;
using LibVLCSharp.Shared;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;

using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class CameraManagingPageViewModel : ViewModelBase
    {
        private Timer timer;
        private readonly int maxLoadingTime = 28;
        private readonly string playIconSource = "ic_play_arrow_white.png";
        private readonly string stopIconSource = "ic_stop_white.png";
        private readonly string volumeIconSource = "ic_volumespeaker";
        private readonly string muteIconSource = "ic_mute";
        private string videoUrl1 = string.Empty;
        private string videoUrl2 = string.Empty;
        private string videoUrl3 = string.Empty;
        private string videoUrl4 = string.Empty;
        private double cam1LoadingTime = 0;
        private double cam2LoadingTime = 0;
        private double cam3LoadingTime = 0;
        private double cam4LoadingTime = 0;
        private string currentXnCode { get; set; }
        private string currentVehiclePlate { get; set; }
       
        private const int maxTimeCameraRemain = 598; //second       

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
            ShareTappedCommand = new DelegateCommand(ShareTapped);
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
                    currentCamera.Clear();
                    // reset selected cam
                    SelectedCamera = null;
                    var allCams = new List<CameraEnum>()
                        {
                            CameraEnum.CAM1,
                            CameraEnum.CAM2,
                            CameraEnum.CAM3,
                            CameraEnum.CAM4,
                        };
                    TotalTimeCam1 = 0;
                    TotalTimeCam2 = 0;
                    TotalTimeCam3 = 0;
                    TotalTimeCam4 = 0;

                    cam1LoadingTime = 0;
                    cam2LoadingTime = 0;
                    cam3LoadingTime = 0;
                    cam4LoadingTime = 0;

                    IsCAM1Error = false;
                    IsCAM2Error = false;
                    IsCAM3Error = false;
                    IsCAM4Error = false;
                    EventAggregator.GetEvent<HideVideoViewEvent>().Publish(allCams);
                    //VehicleSelectedPlate = vehiclePlate.VehiclePlate;
                    //GetCameraInfor("CAMTEST1");
                    if (vehiclePlate.VehiclePlate == "98B00048")
                    {
                        VehicleSelectedPlate = "CAMTEST1";
                        GetCameraInfor("CAMTEST1");
                    }
                    else if(vehiclePlate.VehiclePlate == "98B00562")
                    {
                        VehicleSelectedPlate = "BACAM1409";
                        GetCameraInfor("BACAM1409");
                    }
                    else
                    {
                        VehicleSelectedPlate = "QATEST1";
                        GetCameraInfor("QATEST1");
                    }
                    SelectedCamera = currentCamera.FirstOrDefault();                  
                    
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
            set { SetProperty(ref isCAM1Error, value);
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

        }

        private void MediaPlayerNo1_EncounteredError(object sender, EventArgs e)
        {
            TryExecute(async () =>
            {
                MediaPlayerNo1.EncounteredError -= MediaPlayerNo1_EncounteredError;
              
                if (cam1LoadingTime <= maxLoadingTime)
                {                  
                    await Task.Delay(200);
                    cam1LoadingTime += 0.2;
                    InitCamera1(videoUrl1);
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        EventAggregator.GetEvent<HideVideoViewEvent>().Publish(new List<CameraEnum>() { CameraEnum.CAM1 });
                        IsCam1Loaded = true;
                        IsCAM1Error = true;
                    });
                }
            });

        }

        private void MediaPlayerNo1_TimeChanged(object sender, MediaPlayerTimeChangedEventArgs e)
        {
            if (e.Time > 0 && !IsCam1Loaded)
            {
                PlayButtonIconSource = stopIconSource;
                IsCam1Loaded = true;
                ShowVideoView(CameraEnum.CAM1);
                TotalTimeCam1 = 180;
                MediaPlayerNo1.TimeChanged -= MediaPlayerNo1_TimeChanged;
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
        }

        private void MediaPlayerNo2_EncounteredError(object sender, EventArgs e)
        {
            TryExecute(async () =>
            {
                MediaPlayerNo2.EncounteredError -= MediaPlayerNo2_EncounteredError;
               
                if (cam2LoadingTime <= maxLoadingTime)
                {                 
                    await Task.Delay(200);
                    cam2LoadingTime += 0.2;
                    InitCamera2(videoUrl2);
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        EventAggregator.GetEvent<HideVideoViewEvent>().Publish(new List<CameraEnum>() { CameraEnum.CAM2 });
                        IsCam2Loaded = true;
                        IsCAM2Error = true;
                    });                  
                }
            });
        }

        private void MediaPlayerNo2_TimeChanged(object sender, MediaPlayerTimeChangedEventArgs e)
        {
            if (e.Time > 0 && !IsCam2Loaded)
            {
                PlayButtonIconSource = stopIconSource;
                IsCam2Loaded = true;
                ShowVideoView(CameraEnum.CAM2);
                TotalTimeCam2 = 180;
                MediaPlayerNo2.TimeChanged -= MediaPlayerNo2_TimeChanged;
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
                MediaPlayerNo3.EncounteredError -= MediaPlayerNo3_EncounteredError;               
                if (cam3LoadingTime < maxLoadingTime)
                {                 
                    await Task.Delay(200);
                    cam3LoadingTime += 0.2;
                    InitCamera3(videoUrl3);
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        EventAggregator.GetEvent<HideVideoViewEvent>().Publish(new List<CameraEnum>() { CameraEnum.CAM3 });
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
                MediaPlayerNo3.TimeChanged -= MediaPlayerNo3_TimeChanged;
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
                MediaPlayerNo4.EncounteredError -= MediaPlayerNo4_EncounteredError;              
                if (cam4LoadingTime <= maxLoadingTime)
                {                 
                    await Task.Delay(200);
                    cam4LoadingTime += 0.2;                  
                    InitCamera4(videoUrl4);
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        EventAggregator.GetEvent<HideVideoViewEvent>().Publish(new List<CameraEnum>() { CameraEnum.CAM3 });
                        IsCam4Loaded = true;
                        IsCAM4Error = true;
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
            if (CanExcute())
            {
                SafeExecute(async () =>
                {
                    await NavigationService.NavigateAsync("RequestMoreTimePopup");
                });
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
        private void ScreenShotTapped()
        {
            TryExecute(async () =>
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
                    }
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        SetLanscape();
                    });
                }
            });
           
        }

        public ICommand ShareTappedCommand { get; }
        private void ShareTapped()
        {
            //ScreenShotTapped();
            //Xamarin.Essentials.Share.RequestAsync(new Xamarin.Essentials.ShareFileRequest(new Xamarin.Essentials.ShareFile(filePath)));
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
                    if (deviceResponseData == null)
                    {
                        EventAggregator.GetEvent<SetCameraLayoutEvent>().Publish(0);
                    }
                    if (deviceResponseData != null)
                    {
                        currentXnCode = deviceResponseData.XnCode;
                        currentVehiclePlate = deviceResponseData.VehiclePlate;
                        // camrera active filter
                        var cameraActiveQuantity = deviceResponseData.CameraChannels?.Where(x => x.IsPlug).ToList();
                        if (cameraActiveQuantity != null && cameraActiveQuantity.Count >0)
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

                        for (int i = 0; i < cameraActiveQuantity.Count; i++)
                        {
                            CameraChannel data = cameraActiveQuantity[i];
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

                                    switch (i+1)
                                    {
                                        case 1:
                                            //Bật busy indicator
                                            IsCam1Loaded = false;
                                            cam1LoadingTime += 1;
                                            break;

                                        case 2:
                                            IsCam2Loaded = false;
                                            cam2LoadingTime += 1;
                                            break;

                                        case 3:
                                            IsCam3Loaded = false;
                                            cam3LoadingTime += 1;
                                            break;

                                        case 4:
                                            IsCam4Loaded = false;
                                            cam4LoadingTime += 1;
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
            if (timer == null)
            {
                timer = new Timer();
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

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
           
            foreach (var item in currentCamera)
            {
                TryExecute(async () =>
                {
                    switch (item)
                    {
                        case CameraEnum.CAM1:
                            if (TotalTimeCam1 > 0)
                            {
                                TotalTimeCam1 -= 1;
                            }
                            
                            if (TotalTimeCam1 % 10 == 0 && TotalTimeCam1 > maxTimeCameraRemain)
                            {
                                await SendRequestTime(600, 1);
                            }
                            break;
                        case CameraEnum.CAM2:
                            if (TotalTimeCam2 > 0)
                            {
                                TotalTimeCam2 -= 1;
                            }
                           
                            if (TotalTimeCam2 % 10 == 0 && TotalTimeCam2 > maxTimeCameraRemain)
                            {
                                await SendRequestTime(600, 2);
                            }
                            break;
                        case CameraEnum.CAM3:
                            if (TotalTimeCam3 > 0)
                            {
                                TotalTimeCam3 -= 1;
                            }
                                
                            if (TotalTimeCam3 % 10 == 0 && TotalTimeCam3 > maxTimeCameraRemain)
                            {
                                await SendRequestTime(600, 3);
                            }
                            break;
                        case CameraEnum.CAM4:
                            if (TotalTimeCam4 > 0)
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

        private bool CanExcute()
        {
           switch(SelectedCamera)
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

            if (timer != null)
            {
                timer.Elapsed -= Timer_Elapsed;
                timer.Stop();
                timer.Dispose();
            }
        }
    }
}