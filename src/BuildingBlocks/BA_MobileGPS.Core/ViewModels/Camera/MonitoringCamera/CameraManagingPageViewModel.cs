using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Core.Interfaces;
using BA_MobileGPS.Core.Models;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Core.Views;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.RequestEntity;
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
using System.Timers;
using System.Windows.Input;

using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    /// <summary>
    /// Thay đổi ngày 25/02/2021: Hiển thị tất cả các camera của xe (k check lỗi)
    /// => lỗi sẽ dc trả về sua khi call start livestream
    /// </summary>
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
        private readonly int maxLoadingTime = 60; //second
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
            SelectVehicleCameraCommand = new DelegateCommand(SelectVehicleCamera);
            SelectedChannelCommand = new DelegateCommand<object>(SelectedChannel);
            EventAggregator.GetEvent<SendErrorCameraEvent>().Subscribe(SetErrorChannelCamera);
        }

        #region Life Cycle

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            //Check parameter key
            if (parameters.ContainsKey(ParameterKey.Vehicle) && parameters.GetValue<CameraLookUpVehicleModel>(ParameterKey.Vehicle) is CameraLookUpVehicleModel vehicle)
            {
                Vehicle = vehicle;
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
            GetChannelCamera();

            EventAggregator.GetEvent<UserBehaviorEvent>().Publish(new UserBehaviorModel()
            {
                Page = Entities.Enums.MenuKeyEnums.TrackingVideo,
                Type = UserBehaviorType.Start
            });
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
            EventAggregator.GetEvent<SendErrorCameraEvent>().Unsubscribe(SetErrorChannelCamera);
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

            EventAggregator.GetEvent<UserBehaviorEvent>().Publish(new UserBehaviorModel()
            {
                Page = Entities.Enums.MenuKeyEnums.TrackingVideo,
                Type = UserBehaviorType.End
            });
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

        private List<CameraChannel> mCameraVehicle;

        private ObservableCollection<ChannelCamera> channelCamera;

        public ObservableCollection<ChannelCamera> ChannelCamera
        {
            get { return channelCamera; }
            set
            {
                SetProperty(ref channelCamera, value);
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

        private CameraLookUpVehicleModel vehicle = new CameraLookUpVehicleModel();
        public CameraLookUpVehicleModel Vehicle { get => vehicle; set => SetProperty(ref vehicle, value); }

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

        public ICommand SelectVehicleCameraCommand { get; }
        public ICommand SelectedChannelCommand { get; }

        private void SelectVehicleCamera()
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("BaseNavigationPage/VehicleCameraLookup", null, useModalNavigation: true, animated: true);
            });
        }

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
                    if (item.Data != null && !string.IsNullOrEmpty(item.Data.Link))
                    {
                        item.Clear();
                        item.StartWorkUnit(Vehicle.VehiclePlate);
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

        private void GetChannelCamera()
        {
            ChannelCamera = new ObservableCollection<ChannelCamera>()
            {
                new ChannelCamera(){Channel=1, Name="CH1",Status=ChannelCameraStatus.Selected,IsShow=true},
                new ChannelCamera(){Channel=2, Name="CH2",Status=ChannelCameraStatus.Selected,IsShow=true},
                new ChannelCamera(){Channel=3, Name="CH3",Status=ChannelCameraStatus.Selected,IsShow=false},
                new ChannelCamera(){Channel=4, Name="CH4",Status=ChannelCameraStatus.Selected,IsShow=false}
            };
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
                        CurrentAddress = await _geocodeService.GetAddressByLatLng(CurrentComanyID, deviceResponseData.Latitude.ToString(), deviceResponseData.Longitude.ToString());
                        CurrentTime = deviceResponseData.DeviceTime;

                        if (deviceResponseData.CameraChannels != null && deviceResponseData.CameraChannels.Count > 0)
                        {
                            var listCam = new List<CameraManagement>();
                            foreach (var item in deviceResponseData.CameraChannels)
                            {
                                var request = new StreamStartRequest()
                                {
                                    Channel = item.Channel,
                                    IMEI = currentIMEI,
                                    VehiclePlate = Vehicle.VehiclePlate,
                                    xnCode = currentXnCode
                                };
                                // var res = await RequestStartCam(item.Channel);
                                var cam = new CameraManagement(maxLoadingTime,
                                    libVLC,
                                    _streamCameraService,
                                    request, EventAggregator);
                                listCam.Add(cam);
                            }
                            mCameraVehicle = deviceResponseData.CameraChannels;
                            var query = (from p in ChannelCamera
                                         join pts in deviceResponseData.CameraChannels on p.Channel equals pts.Channel
                                         select p).ToList();
                            ChannelCamera = query.ToObservableCollection();
                            SetItemsSource(listCam);
                        }
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
                GetCameraInfor(Vehicle.VehiclePlate);
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteError("ReloadCamera", ex);
            }
        }

        /// <summary>
        /// Clear giao diện khi load lại xe hoặc máy chạy ngầm
        /// </summary>
        private void ClearAllMediaPlayer(bool isloadchannel = true)
        {
            foreach (var child in itemsSource)
            {
                foreach (var item in child.ChildSource)
                {
                    item.Dispose();
                }
            }
            if (isloadchannel)
            {
                foreach (var item in ChannelCamera)
                {
                    item.IsShow = true;
                    item.Status = ChannelCameraStatus.Selected;
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
                    VehiclePlate = Vehicle.VehiclePlate,
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
            if (Vehicle != null && !string.IsNullOrEmpty(Vehicle.VehiclePlate))
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
                    if (itemsSource != null && itemsSource.Count > 0)
                    {
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
                    if (Vehicle != null && !string.IsNullOrEmpty(Vehicle.VehiclePlate))
                    {
                        var deviceResponse = await _streamCameraService.GetDevicesStatus(ConditionType.BKS, Vehicle.VehiclePlate);
                        var deviceResponseData = deviceResponse?.Data?.FirstOrDefault();
                        if (deviceResponseData != null)
                        {
                            CurrentAddress = await _geocodeService.GetAddressByLatLng(CurrentComanyID, deviceResponseData.Latitude.ToString(), deviceResponseData.Longitude.ToString());
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

        private void SelectedChannel(object obj)
        {
            try
            {
                if (obj != null && obj is ChannelCamera item)
                {
                    //	Kênh không active  không thay đổi
                    if (item.Status == ChannelCameraStatus.Error)
                    {
                        DisplayMessage.ShowMessageInfo($"Kênh {item.Channel} không hoạt động");
                        return;
                    }
                    if (mCameraVehicle != null && mCameraVehicle.Count > 0)
                    {
                        var channel = ChannelCamera.FirstOrDefault(x => x.Channel == item.Channel);
                        if (channel != null)
                        {
                            if (channel != null)
                            {
                                channel.IsShow = !item.IsShow;
                                var listCam = new List<CameraManagement>();
                                foreach (var itemcam in ChannelCamera.Where(x => x.Status != ChannelCameraStatus.Error).ToList())
                                {
                                    if (itemcam.IsShow)
                                    {
                                        itemcam.Status = ChannelCameraStatus.Selected;
                                        var request = new StreamStartRequest()
                                        {
                                            Channel = itemcam.Channel,
                                            IMEI = currentIMEI,
                                            VehiclePlate = Vehicle.VehiclePlate,
                                            xnCode = currentXnCode
                                        };
                                        var cam = new CameraManagement(maxLoadingTime, libVLC, _streamCameraService, request, EventAggregator);
                                        listCam.Add(cam);
                                    }
                                    else
                                    {
                                        itemcam.Status = ChannelCameraStatus.UnSelected;
                                    }
                                }
                                if (listCam.Count > 0)
                                {
                                    SetItemsSource(listCam);
                                }
                                else
                                {
                                    ClearAllMediaPlayer(false);
                                }
                            }
                        }
                        else
                        {
                            DisplayMessage.ShowMessageWarning($"Kênh {item.Channel} không hoạt động");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        public void SetErrorChannelCamera(int channel)
        {
            var channelcame = ChannelCamera.FirstOrDefault(x => x.Channel == channel);
            if (channelcame != null)
            {
                channelcame.Status = ChannelCameraStatus.Error;
            }
        }

        #endregion Private method
    }
}