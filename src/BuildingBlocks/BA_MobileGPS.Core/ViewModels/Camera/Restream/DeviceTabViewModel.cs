using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Extensions;
using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Core.Interfaces;
using BA_MobileGPS.Core.Models;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.Enums;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Enums;
using LibVLCSharp.Shared;
using Prism.Commands;
using Prism.Navigation;
using Prism.Navigation.TabbedPages;
using Syncfusion.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using ItemTappedEventArgs = Syncfusion.ListView.XForms.ItemTappedEventArgs;

namespace BA_MobileGPS.Core.ViewModels
{
    public class DeviceTabViewModel : RestreamChildVMBase
    {
        public ICommand UploadToCloudTappedCommand { get; }

        public ICommand UploadToCloudInListTappedCommand { get; }

        public ICommand ScreenShotTappedCommand { get; }

        public ICommand ReLoadCommand { get; }
        public ICommand LoadMoreItemsCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand SelectVehicleCameraCommand { get; }

        private CancellationTokenSource cts = new CancellationTokenSource();

        public DeviceTabViewModel(INavigationService navigationService,
            IStreamCameraService cameraService,
            IScreenOrientServices screenOrientServices) : base(navigationService, cameraService, screenOrientServices)
        {
            UploadToCloudTappedCommand = new DelegateCommand(UploadToCloudTapped);
            UploadToCloudInListTappedCommand = new DelegateCommand<RestreamVideoModel>(UploadToCloudInListTapped);
            ReLoadCommand = new DelegateCommand(ReloadVideo);
            SearchCommand = new DelegateCommand(SearchData);
            VideoItemTapCommand = new DelegateCommand<ItemTappedEventArgs>(VideoSelectedChange);
            SelectVehicleCameraCommand = new DelegateCommand(SelectVehicleCamera);
            ScreenShotTappedCommand = new DelegateCommand(TakeSnapShot);
            EventAggregator.GetEvent<UserMessageCameraEvent>().Unsubscribe(UserMessageCamera);
            EventAggregator.GetEvent<UserMessageCameraEvent>().Subscribe(UserMessageCamera);
            mediaPlayerVisible = false;
            videoItemsSource = new ObservableCollection<RestreamVideoModel>();
            InitDateTimeInSearch();
            vehicle = new CameraLookUpVehicleModel();
            listChannel = new List<ChannelModel> { new ChannelModel() { Name = MobileResource.Camera_Lable_AllChannel, Value = 0 } };
            selectedChannel = listChannel[0];
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            LibVLCSharp.Shared.Core.Initialize();
            LibVLC = new LibVLC("--no-rtsp-tcp");
            SetChannelSource();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey(ParameterKey.SelectDate)
                  && (parameters.ContainsKey(ParameterKey.VehiclePlate)))
            {
                var selectDate = parameters.GetValue<DateTime>(ParameterKey.SelectDate);
                var vehicleDetail = parameters.GetValue<CameraLookUpVehicleModel>(ParameterKey.VehiclePlate);
                ValidateVehicleCamera(vehicleDetail);
                DateStart = selectDate.Date;
                DateEnd = selectDate.Date.AddDays(1).AddMinutes(-1);
                SearchData();
            }
            else if (parameters.ContainsKey(ParameterKey.Vehicle) && parameters.GetValue<CameraLookUpVehicleModel>(ParameterKey.Vehicle) is CameraLookUpVehicleModel vehicle)
            {
                Vehicle = vehicle;
            }
            else if (parameters.ContainsKey(ParameterKey.GotoMyVideoPage) && parameters.GetValue<bool>(ParameterKey.GotoMyVideoPage) is bool obj)
            {
                GotoMyVideoPage();
            }
            if (parameters.ContainsKey("ReportDate")
                  && (parameters.ContainsKey(ParameterKey.VehiclePlate)))
            {
                var selectDate = parameters.GetValue<Tuple<DateTime, DateTime>>("ReportDate");
                var vehicleDetail = parameters.GetValue<CameraLookUpVehicleModel>(ParameterKey.VehiclePlate);
                ValidateVehicleCamera(vehicleDetail);
                DateStart = selectDate.Item1;
                DateEnd = selectDate.Item2;
                SearchData();
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            EventAggregator.GetEvent<UserMessageCameraEvent>().Unsubscribe(UserMessageCamera);
            DisposeVLC();
        }

        public override void OnSleep()
        {
            base.OnSleep();
            if (MediaPlayerVisible)
            {
                VideoSlected = null;
                cts.Cancel();
                cts.Dispose();
                CloseVideo();
            }
        }

        private CameraLookUpVehicleModel vehicle = new CameraLookUpVehicleModel();
        public CameraLookUpVehicleModel Vehicle { get => vehicle; set => SetProperty(ref vehicle, value); }

        private DateTime dateStart;

        public DateTime DateStart
        {
            get => dateStart;
            set => SetProperty(ref dateStart, value);
        }

        private DateTime dateEnd;

        public DateTime DateEnd
        {
            get => dateEnd;
            set => SetProperty(ref dateEnd, value);
        }

        private List<RestreamVideoModel> VideoItemsSourceOrigin = new List<RestreamVideoModel>();
        private bool isLoadingCamera = false;

        // dem so lan request lai khi connect fail, gioi han la 3
        private int resetDeviceCounter = 0;

        private List<ChannelModel> listChannel;

        /// <summary>
        /// Danh sách kênh
        /// </summary>
        public List<ChannelModel> ListChannel
        {
            get { return listChannel; }
            set { SetProperty(ref listChannel, value); }
        }

        private ChannelModel selectedChannel;

        /// <summary>
        /// Kênh được chọn
        /// </summary>
        public ChannelModel SelectedChannel
        {
            get { return selectedChannel; }
            set { SetProperty(ref selectedChannel, value); }
        }

        private RestreamVideoModel videoSlected;

        /// <summary>
        /// Ảnh được focus
        /// </summary>
        public RestreamVideoModel VideoSlected
        {
            get => videoSlected;
            set
            {
                SetProperty(ref videoSlected, value);
            }
        }

        private ObservableCollection<RestreamVideoModel> videoItemsSource;

        /// <summary>
        /// Source ảnh để chọn video
        /// </summary>
        public ObservableCollection<RestreamVideoModel> VideoItemsSource { get => videoItemsSource; set => SetProperty(ref videoItemsSource, value); }

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

        private string titleLoading = MobileResource.Camera_Message_CameraLoading;

        public string TitleLoading
        {
            get { return titleLoading; }
            set
            {
                SetProperty(ref titleLoading, value);
                RaisePropertyChanged();
            }
        }

        private void ValidateVehicleCamera(CameraLookUpVehicleModel vehicle)
        {
            var listVehicleCamera = StaticSettings.ListVehilceCamera;
            if (listVehicleCamera != null)
            {
                var plate = vehicle.VehiclePlate.Contains("_C") ? vehicle.VehiclePlate : vehicle.VehiclePlate + "_C";
                var model = StaticSettings.ListVehilceCamera.FirstOrDefault(x => x.VehiclePlate == plate);
                if (model != null)
                {
                    Vehicle = new CameraLookUpVehicleModel()
                    {
                        VehiclePlate = model.VehiclePlate,
                        Imei = model.Imei,
                        PrivateCode = model.VehiclePlate,
                        Channel = model.Channel
                    };
                }
                else
                {
                    vehicle.Channel = 4;
                    Vehicle = vehicle;
                }
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
            resetDeviceCounter++;
            if (resetDeviceCounter < 4)
            {
                Task.Run(async () =>
                {
                    await Task.Delay(4000, cts.Token);
                    if (videoSlected != null && !string.IsNullOrEmpty(videoSlected.Link))
                    {
                        MediaPlayer.Media = new Media(libVLC, new Uri(videoSlected.Link));
                        MediaPlayer.Play();
                    }
                }, cts.Token);
            }
            else
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsError = true;
                    ErrorMessenger = MobileResource.Camera_Status_ErrorConnect;
                });
            }
        }

        /// <summary>
        /// Err : BỊ abort sau 10s không nhận tín hiệu từ server
        /// hoặc hết video hoặc video bị gọi đóng từ thiết bị khác
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Media_EndReached(object sender, EventArgs e)
        {
            //case abort 10s hoặc bị đóng từ nơi khác => thời gian play < 10s:
            if (MediaPlayer.Time < 10000 && resetDeviceCounter < 4)
            {
                resetDeviceCounter++;
                Task.Run(async () =>
                {
                    await Task.Delay(4000, cts.Token);
                    if (videoSlected != null && !string.IsNullOrEmpty(videoSlected.Link))
                    {
                        MediaPlayer.Media = new Media(libVLC, new Uri(videoSlected.Link));
                        MediaPlayer.Play();
                    }
                }, cts.Token);
            }
            else
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsError = true;
                    ErrorMessenger = MobileResource.Camera_Message_PleaseLoadVideo;
                });
            // hết video??/
        }

        private void Media_TimeChanged(object sender, MediaPlayerTimeChangedEventArgs e)
        {
            if (MediaPlayer.Time > 1 && BusyIndicatorActive)
            {
                resetDeviceCounter = 0;
                Device.BeginInvokeOnMainThread(() =>
                {
                    BusyIndicatorActive = false;
                });
            }
        }

        /// <summary>
        /// Raise khi btn reload fire
        /// </summary>
        private void ReloadVideo()
        {
            try
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsError = false;
                    BusyIndicatorActive = true;
                });
                DisposeVLC();
                resetDeviceCounter = 0;
                StartRestream();
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        private void SelectVehicleCamera()
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("BaseNavigationPage/VehicleCameraLookup", null, useModalNavigation: true, animated: true);
            });
        }

        private void UploadToCloudTapped()
        {
            SafeExecute(async () =>
            {
                if (CheckPermision((int)PermissionKeyNames.UploadVideoStream))
                {
                    if (VideoSlected != null)
                    {
                        var parameters = new NavigationParameters
                      {
                          { "UploadVideo", new CameraUploadRequest(){
                               CustomerId =  UserInfo.XNCode,
                               FromDate = VideoSlected.VideoStartTime,
                               ToDate = VideoSlected.VideoEndTime,
                               Channel = VideoSlected.Channel,
                               VehiclePlate =Vehicle.VehiclePlate,
                          } }
                     };

                        var a = await NavigationService.NavigateAsync("BaseNavigationPage/UploadVideoPage", parameters, true, true);
                    }
                }
                else
                {
                    DisplayMessage.ShowMessageInfo(MobileResource.Common_Message_NotPermission);
                }
            });
        }

        private void UploadToCloudInListTapped(RestreamVideoModel obj)
        {
            SafeExecute(async () =>
            {
                if (CheckPermision((int)PermissionKeyNames.UploadVideoStream))
                {
                    if (obj != null)
                    {
                        var parameters = new NavigationParameters
                         {
                              { "UploadVideo", new CameraUploadRequest(){
                                   CustomerId =  UserInfo.XNCode,
                                   FromDate = obj.VideoStartTime,
                                   ToDate = obj.VideoEndTime,
                                   Channel = obj.Channel,
                                   VehiclePlate =Vehicle.VehiclePlate
                              }
                         }
                     };

                        var a = await NavigationService.NavigateAsync("BaseNavigationPage/UploadVideoPage", parameters, true, true);
                    }
                }
                else
                {
                    DisplayMessage.ShowMessageInfo(MobileResource.Common_Message_NotPermission);
                }
            });
        }

        /// <summary>
        /// Raise khi ảnh đang được chọn thay đổi
        /// </summary>
        /// <param name="args"></param>
        private void VideoSelectedChange(ItemTappedEventArgs args)
        {
            if (!(args.ItemData is RestreamVideoModel item))
            {
                return;
            }

            SafeExecute(async () =>
            {
                if (MediaPlayerVisible)
                {
                    cts.Cancel();
                    cts.Dispose();
                    CloseVideo();
                    await Task.Delay(200);
                    cts = new CancellationTokenSource();
                }

                resetDeviceCounter = 0;
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsError = false;
                    BusyIndicatorActive = true;
                    MediaPlayerVisible = true; // Bật layout media lên
                    isLoadingCamera = false; // Trạng thái load url media từ pnc
                });

                VideoSlected = item; // Set màu select cho item

                StopAndStartRestream();

                // Thay cho timer sau 62s, nếu vẫn có indicator=> lỗi.
                _ = Task.Run(async () =>
                  {
                      await Task.Delay(2000);
                      await Task.Delay(60000, cts.Token);
                      if (BusyIndicatorActive && !IsError)
                      {
                          Device.BeginInvokeOnMainThread(() =>
                          {
                              IsError = true;
                              ErrorMessenger = MobileResource.Camera_Message_PleaseLoadVideo;
                          });
                      }
                  }, cts.Token);
            });
        }

        /// <summary>
        /// Raise khi tap được chọn thay đổi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void OnIsActiveChanged(object sender, EventArgs e)
        {
            base.OnIsActiveChanged(sender, e);
            if (!IsActive)
            {
                CloseVideo();
                if (VideoSlected != null)
                {
                    VideoSlected = null;
                }
            }
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

        /// <summary>
        /// Bắt đầu vòng init video, đống và gọi restart ở server
        /// </summary>
        private void StopAndStartRestream()
        {
            var req = new PlaybackStopRequest()
            {
                Channel = VideoSlected.Channel,
                CustomerID = UserInfo.XNCode,
                VehicleName = Vehicle.VehiclePlate,
                Source = (int)CameraSourceType.App,
                User = UserInfo.UserName,
                SessionID = StaticSettings.SessionID
            };
            RunOnBackground(async () =>
            {
                return await streamCameraService.StopAllPlayback(req);
            }, (result) =>
            {
                if (result)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await Task.Delay(7000);

                        StartRestream();
                    });
                }
            });
        }

        /// <summary>
        /// Gọi api start playback
        /// </summary>
        /// <param name="req"></param>
        private void StartRestream()
        {
            SafeExecute(() =>
            {
                var start = new PlaybackStartRequest()
                {
                    Channel = VideoSlected.Channel,
                    CustomerID = UserInfo.XNCode,
                    StartTime = VideoSlected.VideoStartTime,
                    EndTime = VideoSlected.VideoEndTime,
                    VehicleName = Vehicle.VehiclePlate,
                    Source = (int)CameraSourceType.App,
                    User = UserInfo.UserName,
                    SessionID = StaticSettings.SessionID
                };
                RunOnBackground(async () =>
                {
                    return await streamCameraService.StartPlayback(start);
                }, async (result) =>
                {
                    if (result != null && result.StatusCode == 0 && result.Data !=null)
                    {
                        var model = result.Data.FirstOrDefault();
                        if (model != null)
                        {
                            if (!string.IsNullOrEmpty(model.Link))
                            {
                                TitleLoading = MobileResource.Camera_Message_CameraLoading;
                                var isSteaming = await CheckDeviceStatus();
                                if (isSteaming)
                                {
                                    // init ở đây :
                                    InitVLC();
                                    MediaPlayer.Media = new Media(libVLC, new Uri(model.Link));
                                    MediaPlayer.Play();
                                    VideoSlected.Channel = model.Channel;
                                    VideoSlected.Link = model.Link;
                                }
                                else
                                {
                                    Device.BeginInvokeOnMainThread(() =>
                                    {
                                        IsError = true;
                                        ErrorMessenger = MobileResource.Camera_Status_ErrorConnect;
                                    });
                                }
                            }
                            else
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    IsError = true;
                                    ErrorMessenger = MobileResource.Camera_Status_ErrorConnect;
                                });
                            }
                        }
                    }
                    else
                    {
                        if (result != null && result.StatusCode == 107)
                        {
                            var lstUser = new List<StreamUserRequest>();
                            foreach (var item in result.Data)
                            {
                                if (item.StreamingRequests != null && item.StreamingRequests.Count > 0)
                                {
                                    foreach (var itemuser in item.StreamingRequests)
                                    {
                                        lstUser.Add(itemuser);
                                    }
                                }
                            }
                            if (result.Data != null && lstUser.Count > 0)
                            {
                                SetErrorErrorDoubleStreamingCamera(result.Data);
                            }
                            else
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    IsError = true;
                                    ErrorMessenger = result.UserMessage;
                                });
                            }
                        }
                        else
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                IsError = true;
                                ErrorMessenger = result.UserMessage;
                            });
                        }
                    }
                });
            });
        }

        private void SetErrorErrorDoubleStreamingCamera(List<PlaybackStartRespone> lst)
        {
            var lstUser = new List<StreamUserRequest>();
            foreach (var item in lst)
            {
                if (item.StreamingRequests != null && item.StreamingRequests.Count > 0)
                {
                    foreach (var itemuser in item.StreamingRequests)
                    {
                        var isexist = lstUser.Exists(x => x.User == itemuser.User && x.Session == itemuser.Session);
                        if (!isexist)
                        {
                            lstUser.Add(itemuser);
                        }
                    }
                }
            }
            if (lstUser.Count > 0)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    IsError = true;
                    ErrorMessenger = MobileResource.Camera_Message_DeviceStreamingError;
                    var parameters = new NavigationParameters
                      {
                          { "StreamUser",new Tuple<Vehicle,List<StreamUserRequest>>(Vehicle, lstUser) }
                     };
                    await NavigationService.NavigateAsync("StreamUserMessagePopup", parameters);
                });
            }
        }

        /// <summary>
        /// Kiểm tra trạng thái thiết bị sau khi gọi start
        /// Return:
        ///  True : Thiết bị bắt đàu phát lại
        ///  False : Thiết bị chưa phát video
        /// </summary>
        /// <returns></returns>
        private async Task<bool> CheckDeviceStatus()
        {
            isLoadingCamera = true;
            var result = false;
            var loopIndex = 0;
            var start = new PlaybackStartRequest()
            {
                Channel = VideoSlected.Channel,
                CustomerID = UserInfo.XNCode,
                StartTime = VideoSlected.VideoStartTime,
                EndTime = VideoSlected.VideoEndTime,
                VehicleName = Vehicle.VehiclePlate,
                Source = (int)CameraSourceType.App,
                User = UserInfo.UserName,
            };
            try
            {
                while (isLoadingCamera && loopIndex <= 20 && IsActive && !cts.IsCancellationRequested)
                {
                    var deviceStatus = await streamCameraService.GetDevicesInfo(new StreamDeviceRequest()
                    {
                        ConditionType = (int)ConditionType.BKS,
                        ConditionValues = new List<string>() { Vehicle.VehiclePlate },
                        Source = (int)CameraSourceType.App,
                        User = UserInfo.UserName,
                        SessionID = StaticSettings.SessionID
                    });
                    if (deviceStatus != null && deviceStatus.Channels != null)
                    {
                        if (deviceStatus.Net == (int)NetworkDataType.NETWORK_TYPE_LTE || deviceStatus.Net == (int)NetworkDataType.NETWORK_TYPE_IWLAN)
                        {
                            TitleLoading=MobileResource.Camera_Message_CameraLoading;
                        }
                        else
                        {
                            TitleLoading=" Video có thể load chậm do phương tiện không sử dụng mạng 4G, quý khách vui lòng đợi";
                        }
                        var streamDevice = deviceStatus.Channels.FirstOrDefault(x => x.Channel == videoSlected.Channel);
                        if (streamDevice?.Status != null)
                        {
                            var isStreaming = CameraStatusExtension.IsRestreaming(streamDevice.Status);
                            if (isStreaming)
                            {
                                isLoadingCamera = false;
                                result = true;
                            }
                        }
                        loopIndex++;
                        if (isLoadingCamera && loopIndex <= 20)
                        {
                            await Task.Delay(3000, cts.Token);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }

            return result;
        }

        private bool ValidateInput()
        {
            if (dateStart.Date != dateEnd.Date)
            {
                DisplayMessage.ShowMessageInfo(MobileResource.Common_Message_RequiredDateTimeOver);
                return false;
            }
            else if (dateStart > dateEnd)
            {
                DisplayMessage.ShowMessageInfo(MobileResource.Route_Label_StartDateMustSmallerThanEndDate);
                return false;
            }
            else if (Vehicle == null || string.IsNullOrEmpty(Vehicle.VehiclePlate))
            {
                DisplayMessage.ShowMessageInfo(MobileResource.Common_Message_RequiredVehicle);
                return false;
            }
            else if (Vehicle != null && SelectedChannel.Value > Vehicle.Channel)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var action = await PageDialog.DisplayAlertAsync("Thông báo",
                          string.Format("Gói cước bạn đang sử dụng chỉ xem được {0} kênh. \nVui lòng liên hệ tới hotline {1} để được hỗ trợ",
                          Vehicle.Channel, MobileSettingHelper.HotlineGps),
                          "Liên hệ", "Bỏ qua");
                    if (action)
                    {
                        PhoneDialer.Open(MobileSettingHelper.HotlineGps);
                    }
                });
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Lấy danh sách video từ server
        /// </summary>
        private void GetListVideoDataFrom()
        {
            try
            {
                VideoItemsSourceOrigin.Clear();
                VideoItemsSource = new ObservableCollection<RestreamVideoModel>();
                pageIndex = 0;
                if (ValidateInput())
                {
                    RunOnBackground(async () =>
                    {
                        return await streamCameraService.GetListVideoPlayback(new CameraPlaybackInfoRequest()
                        {
                            XnCode = UserInfo.XNCode,
                            VehiclePlate = Vehicle.VehiclePlate,
                            Channel = SelectedChannel.Value,
                            CompanyID = CurrentComanyID,
                            FromDate = DateStart,
                            ToDate = DateEnd
                        });
                    }, (result) =>
                    {
                        if (result != null && result.Count > 0)
                        {
                            // Lọc sai số : video có timespan = timeEnd - timeStart tối thiểu 64s
                            // result = result.Where(x => (x.EndTime - x.StartTime) >= TimeSpan.FromSeconds(64)).ToList();
                            foreach (var video in result)
                            {
                                var videoModel = new RestreamVideoModel()
                                {
                                    ImageUrl = video.Image,
                                    VideoStartTime = video.StartTime,
                                    VideoEndTime = video.EndTime,
                                    VideoTime = video.EndTime - video.StartTime,
                                    Channel = video.Channel
                                };

                                videoModel.VideoName = string.Format("Camera{0}_{1}", video.Channel,
                                    videoModel.VideoStartTime.ToString("yyyyMMdd_hhmmss"));

                                VideoItemsSourceOrigin.Add(videoModel);
                            }
                            //Sort lại theo kênh và thời gian ASC
                            VideoItemsSourceOrigin.Sort((x, y) => DateTime.Compare(x.VideoStartTime, y.VideoStartTime));
                            VideoItemsSource = VideoItemsSourceOrigin.ToObservableCollection();
                        }
                    }, showLoading: true);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        /// <summary>
        /// Set dữ liệu cho picker channel
        /// Hard 4 kênh (Đã confirm)
        /// </summary>
        private void SetChannelSource()
        {
            try
            {
                var lstchannel = new List<int>() { 1, 2, 3, 4 };
                var source = new List<ChannelModel>();
                source.Add(new ChannelModel() { Name = MobileResource.Camera_Lable_AllChannel, Value = 0 });
                if (lstchannel != null)
                {
                    foreach (var item in lstchannel)
                    {
                        var temp = new ChannelModel()
                        {
                            Value = item,
                            Name = string.Format("{0} {1}", MobileResource.Camera_Lable_Channel, item)
                        };
                        source.Add(temp);
                    }
                }
                ListChannel = source;
                SelectedChannel = source.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        private void InitDateTimeInSearch()
        {
            dateEnd = DateTime.Now;
            //Nếu lớn hơn 00h20p
            if (dateEnd.TimeOfDay > new TimeSpan(0, 30, 0))
            {
                dateStart = dateEnd.AddMinutes(-30);
            }
            else dateStart = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 0, 0, 0);
        }

        private void InitVLC()
        {
            try
            {
                MediaPlayer = new MediaPlayer(libVLC);
                MediaPlayer.TimeChanged += Media_TimeChanged;
                MediaPlayer.EndReached += Media_EndReached;
                MediaPlayer.EncounteredError += Media_EncounteredError;
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
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
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        private void SearchData()
        {
            GetListVideoDataFrom();
            CloseVideo();
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
                    MediaPlayer.TakeSnapshot(0, filePath, 0, 0);
                    if (File.Exists(filePath))
                    {
                        DependencyService.Get<ICameraSnapShotServices>().SaveSnapShotToGalery(filePath);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        private void GotoMyVideoPage()
        {
            SafeExecute(async () =>
            {
                await NavigationService.SelectTabAsync("MyVideoTab", null);
            });
        }

        private void UserMessageCamera(string message)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await PageDialog.DisplayAlertAsync(MobileResource.Common_Label_Notification, message, MobileResource.Common_Button_Close);
            });
        }
    }
}