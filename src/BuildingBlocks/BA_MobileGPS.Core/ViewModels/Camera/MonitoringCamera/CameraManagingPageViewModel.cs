using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Core.Interfaces;
using BA_MobileGPS.Core.Models;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Core.Views;
using BA_MobileGPS.Core.Views.Camera.MonitoringCamera;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using BA_MobileGPS.Service.IService;
using LibVLCSharp.Shared;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;

using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class CameraManagingPageViewModel : ViewModelBase
    {
        private Timer timer;
        private int counterRequestMoreTime = 15;
        private readonly int maxLoadingTime = 20; //second
        private readonly string playIconSource = "ic_play_arrow_white.png";
        private readonly string stopIconSource = "ic_stop_white.png";
        private readonly string volumeIconSource = "ic_volumespeaker";
        private readonly string muteIconSource = "ic_mute";
        private string currentXnCode { get; set; }
        private string currentIMEI { get; set; }
        private const int maxTimeCameraRemain = 600; //second
        private List<CameraEnum> currentCamera { get; set; }
        private readonly IGeocodeService _geocodeService;
        private readonly IStreamCameraService _streamCameraService;

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
            currentAddress = MobileResource.Camera_Label_Undefined;
            currentCamera = new List<CameraEnum>();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            // Đóng busy indicator
            Device.BeginInvokeOnMainThread(() =>
            {
                Cam1.IsLoaded = true;
                Cam2.IsLoaded = true;
                Cam3.IsLoaded = true;
                Cam4.IsLoaded = true;
            });
            //Check parameter key
            if (parameters.ContainsKey(ParameterKey.Vehicle) && parameters.GetValue<Vehicle>(ParameterKey.Vehicle) is Vehicle vehiclePlate)
            {
                VehicleSelectedPlate = vehiclePlate.VehiclePlate;
                ReLoadAllCamera(true);
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
        /// <summary>
        /// object management camera position cameraenum.Cam1
        /// </summary>
        private CameraManagement cam1;

        public CameraManagement Cam1
        {
            get { return cam1; }
            set
            {
                SetProperty(ref cam1, value);
                RaisePropertyChanged();
            }
        }

        private CameraManagement cam2;

        public CameraManagement Cam2
        {
            get { return cam2; }
            set
            {
                SetProperty(ref cam2, value);
                RaisePropertyChanged();
            }
        }

        private CameraManagement cam3;

        public CameraManagement Cam3
        {
            get { return cam3; }
            set
            {
                SetProperty(ref cam3, value);
                RaisePropertyChanged();
            }
        }

        private CameraManagement cam4;

        public CameraManagement Cam4
        {
            get { return cam4; }
            set
            {
                SetProperty(ref cam4, value);
                RaisePropertyChanged();
            }
        }

        private int totalTime;

        public int TotalTime
        {
            get { return totalTime; }
            set
            {
                SetProperty(ref totalTime, value);
                RaisePropertyChanged();
            }
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            LibVLCSharp.Shared.Core.Initialize();
            LibVLC = new LibVLC("--no-osd", "--rtsp-tcp");
            cam1 = new CameraManagement(maxLoadingTime, libVLC, CameraEnum.CAM1);
            cam2 = new CameraManagement(maxLoadingTime, libVLC, CameraEnum.CAM2);
            cam3 = new CameraManagement(maxLoadingTime, libVLC, CameraEnum.CAM3);
            cam4 = new CameraManagement(maxLoadingTime, libVLC, CameraEnum.CAM4);
            InitTimer();
        }

        private void InitTimer()
        {
            timer = new Timer()
            {
                Interval = 1000
            };
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private LibVLC libVLC;

        public LibVLC LibVLC
        {
            get { return libVLC; }
            set => SetProperty(ref libVLC, value);
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
        /// <summary>
        /// binding screen orient
        /// </summary>
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
        /// <summary>
        /// camera position on view 1-2-3-4
        /// </summary>
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
        /// <summary>
        /// iconsource volume on common playback control
        /// </summary>
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
        /// <summary>
        /// iconsource play on common playback control
        /// </summary>
        public string PlayButtonIconSource
        {
            get { return playButtonIconSource; }
            set
            {
                SetProperty(ref playButtonIconSource, value);
                RaisePropertyChanged();
            }
        }

        private bool autoAddTime;
        /// <summary>
        /// checkbox binding
        /// </summary>
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
        /// <summary>
        /// bínding address on detail vehicle
        /// </summary>
        public string CurrentAddress
        {
            get { return currentAddress; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    SetProperty(ref currentAddress, MobileResource.Camera_Label_Undefined);
                }
                else
                {
                    SetProperty(ref currentAddress, value);
                }
                RaisePropertyChanged();
            }
        }

        private DateTime? currentTime;
        /// <summary>
        /// bínding time on detail vehicle
        /// </summary>
        public DateTime? CurrentTime
        {
            get { return currentTime; }
            set
            {
                SetProperty(ref currentTime, value);
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// get camera management object at position
        /// </summary>
        /// <param name="camera">position</param>
        /// <returns></returns>
        private CameraManagement GetCamera(CameraEnum camera)
        {
            switch (camera)
            {
                case CameraEnum.CAM1:
                    return Cam1;

                case CameraEnum.CAM2:
                    return Cam2;

                case CameraEnum.CAM3:
                    return Cam3;

                case CameraEnum.CAM4:
                    return Cam4;

                default:
                    return null;
            }
        }
        /// <summary>
        /// get current camera selected
        /// </summary>
        /// <returns></returns>
        private CameraManagement GetSelectedCamera()
        {
            if (selectedCamera != null)
            {
                return GetCamera((CameraEnum)selectedCamera);
            }
            return null;
        }
        /// <summary>
        /// command raise while play is tapped
        /// </summary>
        public ICommand PlayTappedCommand { get; }

        private void PlayTapped()
        {
            var selectedCam = GetSelectedCamera();
            if (selectedCam != null && selectedCam.CanExcute())
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (selectedCam.MediaPlayer.IsPlaying)
                    {
                        selectedCam.MediaPlayer.Pause();
                        PlayButtonIconSource = playIconSource;
                    }
                    else
                    {
                        selectedCam.MediaPlayer.Play();
                        PlayButtonIconSource = stopIconSource;
                    }
                });
            }
        }
        /// <summary>
        /// command raise while volume is tapped
        /// </summary>
        public ICommand VolumeChangedCommand { get; }

        private void VolumeChanged()
        {
            var selectedCam = GetSelectedCamera();
            if (selectedCam != null && selectedCam.CanExcute())
            {
                if (selectedCam.MediaPlayer.Mute)
                {
                    selectedCam.MediaPlayer.Mute = false;
                }
                else selectedCam.MediaPlayer.Mute = true;

                VolumeButtonIconSource = selectedCam.MediaPlayer.Mute ? muteIconSource : volumeIconSource;
            }
        }
        /// <summary>
        /// command raise while a camera region is tapped
        /// </summary>
        public ICommand CameraFrameTappedCommand { get; }

        private void CameraFrameTapped(object obj)
        {
            if (obj != null)
            {
                var camObj = (CameraEnum)obj;
                if (currentCamera.Contains(camObj))
                {
                    var selected = GetCamera(camObj);
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        if (selected != null && !selected.IsError)
                        {
                            SelectedCamera = camObj;
                            PlayButtonIconSource = selected.MediaPlayer.IsPlaying ? stopIconSource : playIconSource;
                            VolumeButtonIconSource = selected.MediaPlayer.Mute ? muteIconSource : volumeIconSource;
                            TotalTime = selected.TotalTime;
                        }
                    });
                }
            }
        }
        /// <summary>
        /// command raise while add time is tapped
        /// </summary>
        public ICommand RequestTimeTappedCommand { get; set; }

        private void RequestTimeTapped()
        {
            var selected = GetSelectedCamera();
            if (selected != null && selected.TotalTime > 1)
            {
                SafeExecute(async () =>
                {
                    await NavigationService.NavigateAsync("RequestMoreTimePopup");
                });
            }
        }
        /// <summary>
        /// command raise while fullscreen is tapped
        /// </summary>
        public ICommand FullScreenTappedCommand { get; }

        private void FullScreenTapped()
        {
            var selected = GetSelectedCamera();
            if (selected != null && selected.CanExcute())
            {
                IsFullScreenOff = !IsFullScreenOff;
                if (IsFullScreenOff)
                {
                    EventAggregator.GetEvent<SwitchToNormalScreenEvent>().Publish();
                    DependencyService.Get<IScreenOrientServices>().ForcePortrait();
                }
                else
                {
                    SetLanscape();
                }
            }
        }
        /// <summary>
        /// set screen orient to lanscape
        /// </summary>
        private void SetLanscape()
        {
            DependencyService.Get<IScreenOrientServices>().ForceLandscape();
            var selected = GetSelectedCamera();
            if (selected != null && selected.CanExcute())
            {
                EventAggregator.GetEvent<SwitchToFullScreenEvent>().Publish((CameraEnum)selected.Position);
            }
        }
        /// <summary>
        /// command raise while snapshot is tapped
        /// </summary>
        public ICommand ScreenShotTappedCommand { get; }

        private void ScreenShotTapped()
        {
            TakeSnapShot();
        }

        private string TakeSnapShot()
        {
            try
            {
                var folderPath = DependencyService.Get<ICameraSnapShotServices>().GetFolderPath();
                var current = DateTime.Now.ToString("yyyyMMddHHmmss");
                var fileName = Enum.GetName(typeof(CameraEnum), SelectedCamera) + current + ".jpg";
                var filePath = Path.Combine(folderPath, fileName);
                var selected = GetSelectedCamera();
                if (selected != null)
                {
                    selected.MediaPlayer.TakeSnapshot(0, filePath, 0, 0);
                }
                if (File.Exists(filePath))
                {
                    DependencyService.Get<ICameraSnapShotServices>().SaveSnapShotToGalery(filePath);
                    return filePath;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return string.Empty;
        }
        /// <summary>
        /// command raise while share is tapped
        /// share : take a snapshot and share on other app like viber, fb,...
        /// </summary>
        public ICommand ShareTappedCommand { get; }

        private async void ShareTapped()
        {
            try
            {
                var filePath = TakeSnapShot();
                if (!string.IsNullOrEmpty(filePath))
                {
                    await Xamarin.Essentials.Share.RequestAsync(new Xamarin.Essentials.ShareFileRequest(new Xamarin.Essentials.ShareFile(filePath)));
                }
                else LoggerHelper.WriteLog(MethodBase.GetCurrentMethod().Name, "filePath error while snapshot");
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        public ICommand ReloadCommand { get; }

        /// <summary>
        /// reload khi bi loi hoac het thoi gian
        /// </summary>
        /// <param name="obj"> vi tri camera can reload</param>
        private void Reload(object obj)
        {
            if (obj != null)
            {
                var param = (CameraEnum)obj;
                var camera = GetCamera(param);
                if (camera != null)
                {
                    if (camera.Data != null)
                    {
                        camera.Clear();
                        RequestStartCam(camera.Data.Channel, (CameraEnum)camera.Position);
                    }
                }
            }
        }

        /// <summary>
        /// gui request start cho camera
        /// </summary>
        /// <param name="chanel"> kenh </param>
        /// <param name="position"> vi tri hien thi tren man hinh </param>
        private void RequestStartCam(int chanel, CameraEnum position)
        {
            var cameraAsign = GetCamera(position);
            cameraAsign.Position = position;
            if (cameraAsign != null)
            {
                TryExecute(async () =>
                {
                    var request = new StreamStartRequest()
                    {
                        Channel = chanel,
                        IMEI = currentIMEI,
                        VehiclePlate = vehicleSelectedPlate,
                        xnCode = currentXnCode
                    };

                    var camResponse = await _streamCameraService.StartStream(request);
                    var startResponse = camResponse?.Data?.FirstOrDefault();
                    if (startResponse != null)
                    {
                        cameraAsign.Data = startResponse;
                        cameraAsign.SetMedia(startResponse.Link);
                    }
                });
            }
        }

        /// <summary>
        /// Lay thong tin camera tren xe duoc chon
        /// </summary>
        /// <param name="bks"> bien so xe</param>
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
                else
                {
                    currentXnCode = deviceResponseData.XnCode;
                    currentIMEI = deviceResponseData.IMEI;
                    CurrentAddress = await _geocodeService.GetAddressByLatLng(deviceResponseData.Latitude.ToString(), deviceResponseData.Longitude.ToString());
                    CurrentTime = deviceResponseData.DeviceTime;
                    // camrera active filter => set layout 1 2 4
                    var cameraActive = deviceResponseData.CameraChannels?.Where(x => x.IsPlug).ToList();
                    SetLayoutDependCameraQuantity(cameraActive.Count);

                    for (int index = 0; index < cameraActive.Count; index++)
                    {
                        var position = (CameraEnum)index;
                        currentCamera.Add(position);
                        RequestStartCam(cameraActive[index].Channel, position);
                    }
                }
            });
        }

        /// <summary>
        /// layout 1-2-4 theo so luong camera tren xe
        /// </summary>
        /// <param name="quantity"></param>
        private void SetLayoutDependCameraQuantity(int quantity)
        {
            if (quantity <= 2)
            {
                EventAggregator.GetEvent<SetCameraLayoutEvent>().Publish(quantity);
            }
            else
            {
                EventAggregator.GetEvent<SetCameraLayoutEvent>().Publish(4);
            }
        }

        /// <summary>
        /// Gan lai gia tri thoi gian chay cho camera theo yeu cau tu nguoi dung
        /// </summary>
        /// <param name="minutes"></param>
        private void RequestMoreTimeStream(int minutes)
        {
            var cameraRequested = GetSelectedCamera();
            if (cameraRequested != null)
            {
                cameraRequested.TotalTime += minutes * 60;
                TotalTime = cameraRequested.TotalTime;
            }
        }

        public override void OnSleep()
        {
            foreach (var item in currentCamera)
            {
                var cam = GetCamera(item);
                cam.Clear();
            }

            if (timer != null && timer.Enabled)
            {
                timer.Stop();
            }
            base.OnSleep();
        }

        public override void OnResume()
        {
            base.OnResume();
            ReLoadAllCamera(false);
            DependencyService.Get<IScreenOrientServices>().ForcePortrait();
        }

        /// <summary>
        ///  reload khi thay doi xe or phone status change to sleep
        /// </summary>
        /// <param name="clearData"> clear current media in media player</param>
        private void ReLoadAllCamera(bool clearData)
        {
            try
            {
                if (clearData)
                {
                    foreach (var item in currentCamera)
                    {
                        var cam = GetCamera(item);
                        cam.Clear();
                    }
                    currentCamera.Clear();
                    SelectedCamera = null;
                }
                GetCameraInfor(VehicleSelectedPlate);
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteError("ReloadCamera", ex);
            }
        }

        /// <summary>
        /// GUi request gia han
        /// </summary>
        /// <param name="timeSecond"> so giay</param>
        /// <param name="chanel"> kenh gia han</param>
        /// <returns></returns>
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

        /// <summary>
        /// Cap nhat khung thong tin chi tiet:
        ///  - update each 10s
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!string.IsNullOrEmpty(vehicleSelectedPlate))
            {
                counterRequestMoreTime--;
                if (counterRequestMoreTime == 0)
                {
                    counterRequestMoreTime = 15;
                    UpdateTimeAndLocation();
                }
                foreach (var item in currentCamera)
                {
                    var cam = GetCamera(item);
                    if (AutoAddTime) // tu dong gia han
                    {
                        if (counterRequestMoreTime == 5)
                        {
                            TryExecute(async () =>
                            {
                                if (cam != null && !cam.IsError && cam.IsLoaded)
                                {
                                    await SendRequestTime(maxTimeCameraRemain, cam.Data.Channel);
                                }
                            });
                        }
                    }
                    else
                    {
                        if (TotalTime > 0)
                        {
                            TotalTime -= 1;
                        }
                        TryExecute(async () =>
                        {
                            if (cam != null && !cam.IsError && cam.IsLoaded)
                            {
                                if (cam.TotalTime > 0)
                                {
                                    if (cam.TotalTime % 10 == 0 && cam.TotalTime > maxTimeCameraRemain)
                                    {
                                        await SendRequestTime(maxTimeCameraRemain, cam.Data.Channel);
                                    }
                                }
                            }
                        });
                    }
                }
            }
        }

        /// <summary>
        /// lay thong tin chi tiet by request get device information
        /// </summary>
        private void UpdateTimeAndLocation()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    if (!string.IsNullOrEmpty(vehicleSelectedPlate))
                    {
                        var deviceResponse = await _streamCameraService.GetDevicesStatus(ConditionType.BKS, VehicleSelectedPlate);
                        var deviceResponseData = deviceResponse?.Data?.FirstOrDefault();
                        if (deviceResponseData != null)
                        {
                            CurrentAddress = await _geocodeService.GetAddressByLatLng(deviceResponseData.Latitude.ToString(), deviceResponseData.Longitude.ToString());
                            CurrentTime = deviceResponseData.DeviceTime;
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggerHelper.WriteError(MethodBase.GetCurrentMethod().Name, ex);
                }
            });
        }

        public override void OnDestroy()
        {
            DependencyService.Get<IScreenOrientServices>().ForcePortrait();
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