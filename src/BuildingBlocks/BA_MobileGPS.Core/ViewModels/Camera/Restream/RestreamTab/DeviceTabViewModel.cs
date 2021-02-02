using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Extensions;
using BA_MobileGPS.Core.Interfaces;
using BA_MobileGPS.Core.Models;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service.IService;
using BA_MobileGPS.Utilities;
using LibVLCSharp.Shared;
using Prism.Commands;
using Prism.Navigation;
using Syncfusion.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using ItemTappedEventArgs = Syncfusion.ListView.XForms.ItemTappedEventArgs;

namespace BA_MobileGPS.Core.ViewModels
{
    public class DeviceTabViewModel : RestreamChildVMBase
    {
        public ICommand UploadToCloudTappedCommand { get; }
        public ICommand ReLoadCommand { get; }
        public ICommand LoadMoreItemsCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand SelectVehicleCameraCommand { get; }

        CancellationTokenSource cts = new CancellationTokenSource();
        public DeviceTabViewModel(INavigationService navigationService,
            IStreamCameraService cameraService,
            IScreenOrientServices screenOrientServices) : base(navigationService, cameraService, screenOrientServices)
        {
            UploadToCloudTappedCommand = new DelegateCommand(UploadToCloudTapped);
            ReLoadCommand = new DelegateCommand(ReloadVideo);
            LoadMoreItemsCommand = new DelegateCommand<object>(LoadMoreItems, CanLoadMoreItems);
            SearchCommand = new DelegateCommand(SearchData);
            VideoItemTapCommand = new DelegateCommand<ItemTappedEventArgs>(VideoSelectedChange);
            SelectVehicleCameraCommand = new DelegateCommand(SelectVehicleCamera);
            mediaPlayerVisible = false;
            videoItemsSource = new ObservableCollection<RestreamVideoModel>();
            InitDateTimeInSearch();
            vehicle = new CameraLookUpVehicleModel();
            listChannel = new List<ChannelModel> { new ChannelModel() { Name = "Tất cả kênh", Value = 0 } };
            selectedChannel = listChannel[0];
        }

        #region Lifecycle

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            LibVLCSharp.Shared.Core.Initialize();
            LibVLC = new LibVLC("--no-rtsp-tcp");
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey(ParameterKey.SelectDate)
                 && (parameters.ContainsKey(ParameterKey.VehiclePlate)))
            {
                var selectDate = parameters.GetValue<DateTime>(ParameterKey.SelectDate);
                var vehicleDetail = parameters.GetValue<CameraLookUpVehicleModel>(ParameterKey.VehiclePlate);
                Vehicle = vehicleDetail;
                DateStart = selectDate.Date;
                DateEnd = selectDate.Date.AddDays(1).AddMinutes(-1);
                SetChannelSource(vehicleDetail.CameraChannels);
                SearchData();
            }
            else if (parameters.ContainsKey(ParameterKey.Vehicle) && parameters.GetValue<CameraLookUpVehicleModel>(ParameterKey.Vehicle) is CameraLookUpVehicleModel vehicle)
            {
                Vehicle = vehicle;
                SetChannelSource(vehicle.CameraChannels);
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            DisposeVLC();
        }

        #endregion Lifecycle

        #region Property
        public string BusyIndicatorText { get; set; }

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

        // Loi abort 10s
        private bool isAbort { get; set; }

        /// <summary>
        /// Thời gian trừ trước và sau thời gian của ảnh => gửi request video
        /// 1.5 theo yêu cầu từ QA
        /// </summary>
        private readonly double configMinute = 1.5;

        private List<RestreamVideoModel> VideoItemsSourceOrigin = new List<RestreamVideoModel>();
        private bool IsLoadingCamera = false;

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

        #endregion Property

        #region PrivateMethod

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
                    if (videoSlected?.Data != null)
                    {
                        MediaPlayer.Media = new Media(libVLC, new Uri(videoSlected?.Data.Link));
                        MediaPlayer.Play();
                    }
                }, cts.Token);
            }
            else
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsError = true;
                    ErrorMessenger = "Có lỗi khi kết nối server";
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
                    if (videoSlected?.Data != null)
                    {
                        MediaPlayer.Media = new Media(libVLC, new Uri(videoSlected?.Data.Link));
                        MediaPlayer.Play();
                    }
                }, cts.Token);
            }
            else
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsError = true;
                    isAbort = true;
                    ErrorMessenger = "Vui lòng load lại hoặc chọn xem video khác";
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
                isAbort = false;
                StopAndStartRestream();
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
            // var req = new StartRestreamRequest()
            // {
            //     Channel = videoSlected.Data.Channel,
            //     CustomerID = customerId,
            //     StartTime = videoSlected.VideoStartTime,
            //     EndTime = videoSlected.VideoEndTime,
            //     VehicleName = bks
            // };
            // RunOnBackground(async () =>
            // {
            //     return await streamCameraService.UploadToCloud(req);
            // }, (res) =>
            //{
            //    Device.BeginInvokeOnMainThread(() =>
            //    {
            //        if (res?.Data != null && res.Data)
            //        {
            //            DisplayMessage.ShowMessageError("UpLoad thành công");
            //        }
            //        else
            //        {
            //            DisplayMessage.ShowMessageError("Có sự cố khi upload");
            //        }
            //    });

            //});
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
                    IsLoadingCamera = false; // Trạng thái load url media từ pnc                    
                });

                VideoSlected = item; // Set màu select cho item
                StopAndStartRestream();

                _ = Task.Run(async () =>
                  {
                      await Task.Delay(2000);
                      await Task.Delay(60000, cts.Token);
                      if (BusyIndicatorActive)
                      {
                          Device.BeginInvokeOnMainThread(() =>
                          {
                              IsError = true;
                              isAbort = true;
                              ErrorMessenger = "Vui lòng load lại hoặc chọn xem video khác";
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
            try
            {
                var req = new StopRestreamRequest()
                {
                    Channel = VideoSlected.Data.Channel,
                    CustomerID = UserInfo.XNCode,
                    VehicleName = Vehicle.VehiclePlate
                };

                Task.Run(async () =>
                {
                    var res1 = await streamCameraService.StopRestream(req);
                    if (res1 != null)
                    {
                        await Task.Delay(3000, cts.Token);
                        // check xem đã tắt chưa, nếu chưa sẽ gọi lại tắt
                        var res = await streamCameraService.GetDevicesStatus(ConditionType.BKS, Vehicle.VehiclePlate);
                        var data = res?.Data?.FirstOrDefault()?.CameraChannels?.FirstOrDefault(x => x.Channel == VideoSlected.Data.Channel);
                        if (data != null && !CameraStatusExtension.IsRestreaming(data.CameraStatus))
                        {
                            if (IsActive)
                            {
                                StartRestream();
                            }
                        }
                        else StopAndStartRestream();
                    }

                }, cts.Token);
            }
            catch (OperationCanceledException)
            {
                // gọi cts
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        /// <summary>
        /// Gọi api start playback
        /// </summary>
        /// <param name="req"></param>
        private void StartRestream()
        {
            try
            {
                var start = new StartRestreamRequest()
                {
                    Channel = VideoSlected.Data.Channel,
                    CustomerID = UserInfo.XNCode,
                    StartTime = VideoSlected.VideoStartTime,
                    EndTime = VideoSlected.VideoEndTime,
                    VehicleName = Vehicle.VehiclePlate
                };
                Task.Run(async () =>
                {
                    var result = await streamCameraService.StartRestream(start);
                    if (result?.Data != null)
                    {
                        var isSteaming = await CheckDeviceStatus();
                        if (isSteaming)
                        {
                            // init ở đây :
                            InitVLC();
                            MediaPlayer.Media = new Media(libVLC, new Uri(result.Data.Link));
                            MediaPlayer.Play();
                            VideoSlected.Data = result.Data;
                        }
                        else
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                IsError = true;
                                ErrorMessenger = "Có lỗi khi kết nối server";
                            });
                        }
                    }
                    else
                    {
                        // Video dang duoc xem tu thiet bi khác
                        StopAndStartRestream();
                    }

                }, cts.Token);
            }
            catch (OperationCanceledException)
            {
                // gọi cts
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
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
            IsLoadingCamera = true;
            var result = false;
            var loopIndex = 0;
            var start = new StartRestreamRequest()
            {
                Channel = VideoSlected.Data.Channel,
                CustomerID = UserInfo.XNCode,
                StartTime = VideoSlected.VideoStartTime,
                EndTime = VideoSlected.VideoEndTime,
                VehicleName = Vehicle.VehiclePlate
            };
            try
            {
                while (IsLoadingCamera && loopIndex <= 7 && IsActive && !cts.IsCancellationRequested)
                {
                    var deviceStatus = await streamCameraService.GetDevicesStatus(ConditionType.BKS, Vehicle.VehiclePlate);
                    var device = deviceStatus?.Data?.FirstOrDefault();
                    if (device != null && device.CameraChannels != null)
                    {
                        var streamDevice = device.CameraChannels.FirstOrDefault(x => x.Channel == videoSlected.Data.Channel);
                        if (streamDevice?.CameraStatus != null)
                        {
                            var isStreaming = CameraStatusExtension.IsRestreaming(streamDevice.CameraStatus);
                            if (isStreaming)
                            {
                                IsLoadingCamera = false;
                                result = true;
                            }
                            // Neu trang thai chưa thay đổi => gọi lại start
                            else await streamCameraService.StartRestream(start);
                        }
                        loopIndex++;
                        if (IsLoadingCamera && loopIndex <= 7)
                        {
                            await Task.Delay(1500, cts.Token);
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
                DisplayMessage.ShowMessageInfo("Không tìm kiếm xuyên ngày");
                return false;
            }
            else if (dateStart > dateEnd)
            {
                DisplayMessage.ShowMessageInfo("Thời gian bắt đầu phải nhỏ hơn thời gian kết thúc");
                return false;
            }
            else if (Vehicle == null || Vehicle.VehicleId == 0)
            {
                DisplayMessage.ShowMessageInfo(" Vui lòng chọn phương tiện");
                return false;
            }
            else
            {
                return true;
            }
        }

        private void LoadMoreItems(object obj)
        {
            var listview = obj as Syncfusion.ListView.XForms.SfListView;
            listview.IsBusy = true;
            try
            {
                //pageIndex++;
                LoadMore();
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            finally
            {
                listview.IsBusy = false;
                IsBusy = false;
            }
        }

        /// <summary>
        /// CanExcute của infinite scroll
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool CanLoadMoreItems(object obj)
        {
            if (VideoItemsSourceOrigin.Count < pageIndex * pageCount)
                return false;
            return true;
        }

        /// <summary>
        /// infinite scroll
        /// </summary>
        private void LoadMore()
        {
            var source = VideoItemsSourceOrigin.Skip(pageIndex * pageCount).Take(pageCount);
            pageIndex++;
            foreach (var item in source)
            {
                VideoItemsSource.Add(item);
            }
        }

        /// <summary>
        /// Lấy danh sách ảnh từ server
        /// </summary>
        private void GetListImageDataFrom()
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
                        return await streamCameraService.RestreamCaptureImageInfo(UserInfo.XNCode,
                            Vehicle.VehiclePlate,
                            DateStart,
                            DateEnd,
                            SelectedChannel.Value,
                            null);
                    }, (result) =>
                    {
                        if (result != null && result.Count > 0)
                        {
                            foreach (var image in result)
                            {
                                var videoModel = new RestreamVideoModel()
                                {
                                    VideoImageSource = image.Url,
                                    VideoStartTime = image.Time.AddMinutes(-configMinute),
                                    VideoEndTime = image.Time.AddMinutes(configMinute),
                                    VideoTime = TimeSpan.FromMinutes(2 * configMinute),
                                    Data = new StreamStart() { Channel = image.Channel },
                                    EventType = image.Type,
                                    VideoAddress = string.IsNullOrEmpty(image.CurrentAddress) ? "Địa chỉ không xác định" : image.CurrentAddress,
                                    ImageTime = image.Time
                                };

                                videoModel.VideoName = string.Format("Camera{0}_{1}", image.Channel,
                                    videoModel.VideoStartTime.ToString("yyyyMMdd_hhmmss"));

                                VideoItemsSourceOrigin.Add(videoModel);
                            }
                            //Sort lại theo kênh và thời gian ASC
                            VideoItemsSourceOrigin.Sort((x, y) => DateTime.Compare(x.VideoStartTime, y.VideoStartTime));

                            VideoItemsSource = VideoItemsSourceOrigin.Skip(pageIndex * pageCount).Take(pageCount).ToObservableCollection();
                            pageIndex++;
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
        private void SetChannelSource(List<int> lstchannel)
        {
            try
            {
                var source = new List<ChannelModel>();
                source.Add(new ChannelModel() { Name = "Tất cả kênh", Value = 0 });
                if (lstchannel != null)
                {
                    foreach (var item in lstchannel)
                    {
                        var temp = new ChannelModel()
                        {
                            Value = item,
                            Name = string.Format("Kênh {0}", item)
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
            if (dateEnd.TimeOfDay > new TimeSpan(0, 20, 0))
            {
                dateStart = dateEnd.AddMinutes(-20);
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
            GetListImageDataFrom();
            CloseVideo();
        }

        #endregion PrivateMethod
    }
}