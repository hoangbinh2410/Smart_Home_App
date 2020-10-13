﻿using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Core.Interfaces;
using BA_MobileGPS.Core.Models;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Core.Views;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using BA_MobileGPS.Service.IService;
using LibVLCSharp.Shared;
using Prism.Commands;
using Prism.Navigation;
using Syncfusion.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private int counterRequestPing = 15;
        private readonly int maxLoadingTime = 20; //second
        private readonly string playIconSource = "ic_play_arrow_white.png";
        private readonly string stopIconSource = "ic_stop_white.png";
        private readonly string volumeIconSource = "ic_volumespeaker";
        private readonly string muteIconSource = "ic_mute";
        private string currentXnCode { get; set; }
        private string currentIMEI { get; set; }
        private const int maxTimeCameraRemain = 600; //second
        private readonly IGeocodeService _geocodeService;
        private readonly IStreamCameraService _streamCameraService;

        public CameraManagingPageViewModel(INavigationService navigationService, IStreamCameraService streamCameraService, IGeocodeService geocodeService) : base(navigationService)
        {
            _geocodeService = geocodeService;
            _streamCameraService = streamCameraService;
            PlayTappedCommand = new DelegateCommand(PlayTapped);
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
            itemsSource = new ObservableCollection<CameraManagement>();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            //Check parameter key
            if (parameters.ContainsKey(ParameterKey.Vehicle) && parameters.GetValue<Vehicle>(ParameterKey.Vehicle) is Vehicle vehiclePlate)
            {
                AutoAddTime = true;
                VehicleSelectedPlate = vehiclePlate.VehiclePlate;
                ReLoadAllCamera();
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
        public bool IsFullScreenOff
        {
            get { return isFullScreenOff; }
            set
            {
                SetProperty(ref isFullScreenOff, value);
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

        public DateTime? CurrentTime
        {
            get { return currentTime; }
            set
            {
                SetProperty(ref currentTime, value);
                RaisePropertyChanged();
            }
        }

        private CameraManagement selectedItem;

        public CameraManagement SelectedItem
        {
            get { return selectedItem; }
            set
            {
                if (value != null)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        PlayButtonIconSource = value.MediaPlayer.IsPlaying ? stopIconSource : playIconSource;
                        VolumeButtonIconSource = value.MediaPlayer.Mute ? muteIconSource : volumeIconSource;
                        TotalTime = value.TotalTime;
                        AutoAddTime = value.AutoRequestPing;
                    });
                }             
                SetProperty(ref selectedItem, value);
            }
        }

        public ICommand PlayTappedCommand { get; }

        private void PlayTapped()
        {
            if (selectedItem != null && selectedItem.CanExcute())
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (selectedItem.MediaPlayer.IsPlaying)
                    {
                        SelectedItem.MediaPlayer.Pause();
                        PlayButtonIconSource = playIconSource;
                    }
                    else
                    {
                        SelectedItem.MediaPlayer.Play();
                        PlayButtonIconSource = stopIconSource;
                    }
                });
            }
        }

        public ICommand VolumeChangedCommand { get; }

        private void VolumeChanged()
        {
            if (selectedItem != null && selectedItem.CanExcute())
            {
                if (selectedItem.MediaPlayer.Mute)
                {
                    SelectedItem.MediaPlayer.Mute = false;
                }
                else SelectedItem.MediaPlayer.Mute = true;

                VolumeButtonIconSource = selectedItem.MediaPlayer.Mute ? muteIconSource : volumeIconSource;
            }
        }

        public ICommand RequestTimeTappedCommand { get; set; }

        private void RequestTimeTapped()
        {
            if (selectedItem != null && selectedItem.TotalTime > 1)
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
            if (selectedItem != null && selectedItem.CanExcute())
            {
                IsFullScreenOff = !IsFullScreenOff;
                if (IsFullScreenOff)
                {
                    EventAggregator.GetEvent<SwitchToNormalScreenEvent>().Publish();
                    DependencyService.Get<IScreenOrientServices>().ForcePortrait();
                }
                else
                {
                    EventAggregator.GetEvent<SwitchToFullScreenEvent>().Publish();
                    DependencyService.Get<IScreenOrientServices>().ForceLandscape();
                }
            }
        }
       

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
                var fileName = current + ".jpg";
                var filePath = Path.Combine(folderPath, fileName);

                if (selectedItem != null)
                {
                    selectedItem.MediaPlayer.TakeSnapshot(0, filePath, 0, 0);
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

        private void Reload(object obj)
        {
        }

        private async Task<CameraManagement> RequestStartCam(int chanel)
        {
            CameraManagement result = null;

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
                result = new CameraManagement(maxLoadingTime, libVLC);
                result.Data = startResponse;
                result.SetMedia(startResponse.Link);
            }
            return result;
        }

        private void GetCameraInfor(string bks)
        {
            StreamDevicesResponse deviceResponse = null;
            TryExecute(async () =>
            {
                deviceResponse = await _streamCameraService.GetDevicesStatus(ConditionType.BKS, bks);
                // only 1 data
                var deviceResponseData = deviceResponse?.Data?.FirstOrDefault();
                if (deviceResponseData != null)
                {
                    currentXnCode = deviceResponseData.XnCode;
                    currentIMEI = deviceResponseData.IMEI;
                    CurrentAddress = await _geocodeService.GetAddressByLatLng(deviceResponseData.Latitude.ToString(), deviceResponseData.Longitude.ToString());
                    CurrentTime = deviceResponseData.DeviceTime;
                    var cameraActive = deviceResponseData.CameraChannels?.Where(x => x.IsPlug).ToList();
                    var listCam = new List<CameraManagement>();
                    foreach (var item in cameraActive)
                    {
                        var res = await RequestStartCam(item.Channel);
                        listCam.Add(res);
                    }
                    listCam.Add(new CameraManagement(maxLoadingTime, libVLC));
                    listCam.Add(new CameraManagement(maxLoadingTime, libVLC));
                    listCam.Add(new CameraManagement(maxLoadingTime, libVLC));
                    listCam.Add(new CameraManagement(maxLoadingTime, libVLC));
                    ItemsSource = listCam.ToObservableCollection();
                }           
            });
        }

        private ObservableCollection<CameraManagement> itemsSource;

        public ObservableCollection<CameraManagement> ItemsSource
        {
            get { return itemsSource; }
            set
            {
                SetProperty(ref itemsSource, value);
                RaisePropertyChanged();
            }
        }

        private void RequestMoreTimeStream(int minutes)
        {
            //var cameraRequested = GetSelectedCamera();
            //if (cameraRequested != null)
            //{
            //    cameraRequested.TotalTime += minutes * 60;
            //    TotalTime = cameraRequested.TotalTime;
            //}
        }

        public override void OnSleep()
        {
            ClearAllMediaPlayer();

            if (timer != null && timer.Enabled)
            {
                timer.Stop();
            }
            base.OnSleep();
        }

        public override void OnResume()
        {
            base.OnResume();
            ReLoadAllCamera();
            if (timer != null && !timer.Enabled)
            {
                timer.Start();
            }
            DependencyService.Get<IScreenOrientServices>().ForcePortrait();
        }

        private void ReLoadAllCamera()
        {
            try
            {
                ClearAllMediaPlayer();
                GetCameraInfor(VehicleSelectedPlate);
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteError("ReloadCamera", ex);
            }
        }

        private void ClearAllMediaPlayer()
        {
            foreach (var item in itemsSource)
            {
                item.Dispose();
            }
            ItemsSource.Clear();
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
            //if (!string.IsNullOrEmpty(vehicleSelectedPlate))
            //{
            //    if (TotalTime > 0)
            //    {
            //        Device.BeginInvokeOnMainThread(() =>
            //        {
            //            TotalTime -= 1;
            //        });
            //    }
            //    counterRequestPing--;
            //    if (counterRequestPing == 0)
            //    {
            //        counterRequestPing = 15;
            //        UpdateTimeAndLocation();
            //        foreach (var item in currentCamera)
            //        {
            //            var cam = GetCamera(item);
            //            if (AutoAddTime || cam.TotalTime > maxTimeCameraRemain)
            //            {
            //                TryExecute(async () =>
            //                {
            //                    if (cam != null && !cam.IsError && cam.IsLoaded)
            //                    {
            //                        await SendRequestTime(maxTimeCameraRemain, cam.Data.Channel);
            //                    }
            //                    if (AutoAddTime && cam.TotalTime < 600)
            //                    {
            //                        cam.TotalTime = 600;
            //                    }
            //                });
            //            }
            //        }
            //    }
            //}
        }

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