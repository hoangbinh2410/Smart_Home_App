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
        #region internal property
        private readonly string playIconSource = "ic_play_arrow_white.png";
        private readonly string stopIconSource = "ic_stop_white.png";
        private readonly string volumeIconSource = "ic_volumespeaker";
        private readonly string muteIconSource = "ic_mute";
        private Timer timer;
        private int counterRequestPing = 15;
        private string currentXnCode { get; set; }
        private string currentIMEI { get; set; }
        private const int maxTimeCameraRemain = 600; //second
        private readonly int maxLoadingTime = 20; //second
        private readonly IGeocodeService _geocodeService;
        private readonly IStreamCameraService _streamCameraService;

        #endregion internal property

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
            currentAddress = MobileResource.Camera_Label_Undefined;
            ReLoadCommand = new DelegateCommand<object>(Reload);
            itemsSource = new List<ChildStackSource>();
        }

        #region Life Cycle

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            //Check parameter key
            if (parameters.ContainsKey(ParameterKey.Vehicle) && parameters.GetValue<Vehicle>(ParameterKey.Vehicle) is Vehicle vehiclePlate)
            {
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

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            LibVLCSharp.Shared.Core.Initialize();
            LibVLC = new LibVLC("--no-osd", "--rtsp-tcp");
            InitTimer();
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

        public override void OnDestroy()
        {
            ClearAllMediaPlayer();
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

        private void InitTimer()
        {
            timer = new Timer()
            {
                Interval = 1000
            };
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        #endregion Life Cycle

        #region Property Binding

        private LibVLC libVLC;

        public LibVLC LibVLC
        {
            get { return libVLC; }
            set => SetProperty(ref libVLC, value);
        }

        private List<ChildStackSource> itemsSource;

        public List<ChildStackSource> ItemsSource
        {
            get { return itemsSource; }
            set
            {
                SetProperty(ref itemsSource, value);
                RaisePropertyChanged();
            }
        }

        private int totalTime;

        /// <summary>
        /// Thời gian hiển thị ở khung chi tiết
        /// </summary>
        public int TotalTime
        {
            get { return totalTime; }
            set
            {
                SetProperty(ref totalTime, value);
                RaisePropertyChanged();
            }
        }

        private string vehicleSelectedPlate;

        /// <summary>
        /// Biển số xe đang chọn
        /// </summary>
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
        /// Hướng màn hình
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

        private string volumeButtonIconSource;

        /// <summary>
        /// Trạng thái âm thanh của video được chọn
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

        /// <summary>
        /// Trạng thái của video được chọn
        /// </summary>
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

        /// <summary>
        /// Checkbox tự động gia hạn của video được chọn
        /// </summary>
        public bool AutoAddTime
        {
            get { return autoAddTime; }
            set
            {
                SetProperty(ref autoAddTime, value);
                if (selectedItem != null)
                {
                    selectedItem.AutoRequestPing = value;
                    TotalTime = selectedItem.TotalTime;
                }

                RaisePropertyChanged();
            }
        }

        private string currentAddress;

        /// <summary>
        /// Vị trí hiện tại ở khung chi tiết
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
        /// Thời gian cập nhật ở khung chi tiết
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

        private CameraManagement selectedItem;

        /// <summary>
        ///  Camera đang được chọn
        /// </summary>
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

        #endregion Property Binding

        #region ICommand & excute

        /// <summary>
        ///  Raise khi nút play được chạm
        /// </summary>
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

        /// <summary>
        ///  Raise khi nút volume được chạm
        /// </summary>
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

        /// <summary>
        /// Mở khung gia hạn thời gian
        /// </summary>
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

        /// <summary>
        /// Chuyển chế độ full màn hình
        /// </summary>
        public ICommand FullScreenTappedCommand { get; }

        private void FullScreenTapped()
        {
            if (selectedItem != null && selectedItem.CanExcute())
            {
                IsFullScreenOff = !IsFullScreenOff;
                if (IsFullScreenOff)
                {
                    DependencyService.Get<IScreenOrientServices>().ForcePortrait();
                }
                else
                {
                    DependencyService.Get<IScreenOrientServices>().ForceLandscape();
                }
            }
        }

        /// <summary>
        /// Reload video
        /// </summary>
        public ICommand ReLoadCommand { get; }

        private void Reload(object obj)
        {
            try
            {
                if (obj != null && obj is CameraManagement item)
                {
                    if (item.Data != null)
                    {
                        item.Clear();
                        RunOnBackground(async () =>
                        {
                            await RequestStartCam(item.Data.Channel, false);
                        });
                        item.SetMedia(item.Data.Link);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        /// <summary>
        /// Chụp ảnh màn hình
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

        /// <summary>
        /// Chụp và chia sẻ ảnh
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

        #endregion ICommand & excute

        #region Private method

        /// <summary>
        /// Gửi request start cho thiết bị
        /// </summary>
        /// <param name="chanel">kênh</param>
        /// <param name="initMedia">có khởi tạo lần đàu không?</param>
        /// <returns></returns>
        private async Task<CameraManagement> RequestStartCam(int chanel, bool initMedia = true)
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
                // Gửi 1 request ping để chắc chắn lúc đầu là 10p
                SendRequestTime(maxTimeCameraRemain, startResponse.Channel);
                if (initMedia)
                {
                    result = new CameraManagement(maxLoadingTime, libVLC);
                    if (result.Data != null)
                    {
                        result.Data = null;
                    }
                    result.Data = startResponse;
                }
            }
            return result;
        }

        /// <summary>
        /// Lấy thông tin camera trên xe
        /// </summary>
        /// <param name="bks">Biển số xe</param>
        private void GetCameraInfor(string bks)
        {
            StreamDevicesResponse deviceResponse = null;
            TryExecute(async () =>
            {
                using (new HUDService())
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
                            res.SetMedia(res.Data.Link);
                            listCam.Add(res);
                        }
                        SetItemsSource(listCam);
                    }
                }
            });
        }

        /// <summary>
        /// add
        /// </summary>
        /// <param name="source"></param>
        private void SetItemsSource(List<CameraManagement> source)
        {
            if (source != null && source.Count > 0)
            {
                var result = new List<ChildStackSource>();
                var countSqrt = Math.Sqrt(source.Count);
                var rowNum = Convert.ToInt32(countSqrt); // so dong
                if (rowNum < countSqrt)
                {
                    rowNum += 1;
                }
                var columnNum = rowNum; // so cot

                while (columnNum > 1)
                {
                    var maxCamInlayout = columnNum * countSqrt;
                    if (source.Count >= maxCamInlayout)
                    {
                        break;
                    }
                    columnNum--;
                }

                for (int i = 0; i < rowNum; i++)
                {
                    var temp = new ChildStackSource();
                    temp.ChildSource = source.Skip(i * columnNum).Take(columnNum).ToList();
                    result.Add(temp);
                }

                ItemsSource = result;
            }
        }

        /// <summary>
        /// Nhận dữ liệu khi popup gia hạn thời gian đóng
        /// </summary>
        /// <param name="minutes">phút</param>
        private void RequestMoreTimeStream(int minutes)
        {
            if (selectedItem != null)
            {
                selectedItem.TotalTime += minutes * 60;
                TotalTime = selectedItem.TotalTime;
            }
        }

        /// <summary>
        /// Load lại giao diện khi thay đổi xe hoặc giao diện trước đó dispose do máy chạy ngầm
        /// </summary>
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

        /// <summary>
        /// Clear giao diện khi load lại xe hoặc máy chạy ngầm
        /// </summary>
        private void ClearAllMediaPlayer()
        {
            foreach (var child in itemsSource)
            {
                foreach (var item in child.ChildSource)
                {
                    item.Dispose();
                }
            }
            ItemsSource = new List<ChildStackSource>();
        }

        /// <summary>
        /// Gửi request ping  cho việc gia hạn
        /// </summary>
        /// <param name="timeSecond">giây</param>
        /// <param name="chanel">kênh</param>
        private void SendRequestTime(int timeSecond, int chanel)
        {
            RunOnBackground(async () =>
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
                    SendRequestTime(timeSecond, chanel);
                }
            });
        }

        /// <summary>
        /// timer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!string.IsNullOrEmpty(vehicleSelectedPlate))
            {
                if (selectedItem != null && TotalTime != selectedItem.TotalTime)
                {
                    TotalTime = selectedItem.TotalTime;                  
                }
                if (TotalTime > 0)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        TotalTime -= 1;
                    });
                }

                counterRequestPing--;
                if (counterRequestPing == 0)
                {
                    counterRequestPing = 15;
                    UpdateTimeAndLocation();
                    foreach (var child in itemsSource)
                    {
                        foreach (var cam in child.ChildSource)
                        {
                            if (cam.AutoRequestPing || cam.TotalTime > maxTimeCameraRemain)
                            {
                                if (cam != null && !cam.IsError && cam.IsLoaded)
                                {
                                    SendRequestTime(maxTimeCameraRemain, cam.Data.Channel);
                                }
                                if (cam.AutoRequestPing && cam.TotalTime < maxTimeCameraRemain)
                                {
                                    cam.TotalTime = maxTimeCameraRemain;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Update lại thời gian và vị trí ở khung chi tiết
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

        #endregion Private method
    }
}