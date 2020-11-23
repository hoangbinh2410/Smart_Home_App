using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Extensions;
using BA_MobileGPS.Core.Interfaces;
using BA_MobileGPS.Core.Models;
using BA_MobileGPS.Core.ViewModels.Base;
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
    public class DeviceTabViewModel : TabbedPageChildVMBase
    {
        private readonly IStreamCameraService streamCameraService;
        private readonly IScreenOrientServices screenOrientServices;
        public ICommand UploadToCloudTappedCommand { get; }
        public ICommand FullScreenTappedCommand { get; }
        public ICommand ReLoadCommand { get; }
        public ICommand LoadMoreItemsCommand { get; }
        public ICommand RefreshDataCommand { get; }
        public ICommand VideoItemTapCommand { get; set; }

        public DeviceTabViewModel(INavigationService navigationService,
            IStreamCameraService cameraService,
            IScreenOrientServices screenOrientServices) : base(navigationService)
        {
            streamCameraService = cameraService;
            this.screenOrientServices = screenOrientServices;
            UploadToCloudTappedCommand = new DelegateCommand(UploadToCloudTapped);
            FullScreenTappedCommand = new DelegateCommand(FullScreenTapped);
            ReLoadCommand = new DelegateCommand(ReloadVideo);
            LoadMoreItemsCommand = new DelegateCommand<object>(LoadMoreItems, CanLoadMoreItems);
            RefreshDataCommand = new DelegateCommand(RefreshData);
            VideoItemTapCommand = new DelegateCommand<ItemTappedEventArgs>(VideoSelectedChange);
            mediaPlayerVisible = false;
            videoItemsSource = new ObservableCollection<RestreamVideoModel>();
            VMBusy = true;
            DateStart = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 0, 0, 0);
            DateEnd = DateTime.Now;
            IsFullScreenOff = true;
            IsError = false;
            VMBusy = false;
            Vehicle = new Vehicle()
            {
                VehiclePlate = "QATEST1",
            };
        }

        #region Lifecycle

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            LibVLCSharp.Shared.Core.Initialize();
            LibVLC = new LibVLC("--no-rtsp-tcp");
            MediaPlayer = new MediaPlayer(libVLC);
            MediaPlayer.TimeChanged += Media_TimeChanged;
            MediaPlayer.EndReached += Media_EndReached;
            MediaPlayer.EncounteredError += Media_EncounteredError;
        }

        public override void OnPageAppearingFirstTime()
        {
            base.OnPageAppearingFirstTime();

            SetChannelSource();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey(ParameterKey.Vehicle) && parameters.GetValue<Vehicle>(ParameterKey.Vehicle) is Vehicle vehicle)
            {
                Vehicle = new Vehicle()
                {
                    VehiclePlate = "QATEST1",
                };
                GetListImageDataFrom();
                CloseVideo();
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (MediaPlayer.Media != null)
            {
                MediaPlayer.Media?.Dispose();
                MediaPlayer.Media = null;
            }
            var media = MediaPlayer;
            MediaPlayer = null;
            media?.Dispose();
        }

        #endregion Lifecycle

        #region Property

        private Vehicle vehicle = new Vehicle();
        public Vehicle Vehicle { get => vehicle; set => SetProperty(ref vehicle, value); }

        private bool busyIndicatorActive;

        public bool BusyIndicatorActive
        {
            get { return busyIndicatorActive; }
            set
            {
                SetProperty(ref busyIndicatorActive, value);
                RaisePropertyChanged();
            }
        }

        private string errorMessenger;

        public string ErrorMessenger
        {
            get { return errorMessenger; }
            set
            {
                SetProperty(ref errorMessenger, value);
                RaisePropertyChanged();
            }
        }

        private DateTime dateStart;

        public DateTime DateStart
        {
            get => dateStart;
            set => SetProperty(ref dateStart, value, DateChange);
        }

        private void DateChange()
        {
            if (!VMBusy)
            {
                if (dateStart.Date != dateEnd.Date)
                {
                    DisplayMessage.ShowMessageError("Ngày bắt đầu không trùng ngày kết thúc, vui lòng kiểm tra lại");
                }
                else GetListImageDataFrom();
            }
        }

        private DateTime dateEnd;

        public DateTime DateEnd
        {
            get => dateEnd;
            set => SetProperty(ref dateEnd, value, DateChange);
        }

        private bool isError;

        public bool IsError
        {
            get { return isError; }
            set
            {
                SetProperty(ref isError, value);
                RaisePropertyChanged();
            }
        }

        // Loi abort 10s
        private bool isAbort { get; set; }

        private readonly int configMinute = 3;
        private int pageIndex { get; set; } = 0;
        private int pageCount { get; } = 20;
        private List<RestreamVideoModel> VideoItemsSourceOrigin = new List<RestreamVideoModel>();
        private bool VMBusy { get; set; }
        private bool IsLoadingCamera = false;

        // dem so lan request lai khi connect fail, gioi han la 3
        private int resetDeviceCounter = 0;
        
        private List<ChannelModel> listChannel;

        public List<ChannelModel> ListChannel
        {
            get { return listChannel; }
            set { SetProperty(ref listChannel, value); }
        }

        private ChannelModel selectedChannel;

        public ChannelModel SelectedChannel
        {
            get { return selectedChannel; }
            set { SetProperty(ref selectedChannel, value, SelectedChannelChanged); }
        }

        private bool isFullScreenOff;

        public bool IsFullScreenOff
        {
            get => isFullScreenOff; set => SetProperty(ref isFullScreenOff, value);
        }

        private RestreamVideoModel videoSlected;

        public RestreamVideoModel VideoSlected
        {
            get => videoSlected;
            set
            {
                SetProperty(ref videoSlected, value);
            }
        }

        private ObservableCollection<RestreamVideoModel> videoItemsSource;

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
        /// Err : Can't connect to server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///
        private void Media_EncounteredError(object sender, EventArgs e)
        {
            resetDeviceCounter++;
            if (resetDeviceCounter < 4)
            {
                StopAndStartRestream();
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
        /// Err : Abortting after 10s
        /// hoặc hết video
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Media_EndReached(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                IsError = true;
                isAbort = true;
                ErrorMessenger = "Kết nối không ổn định. Hoặc đã hết thời lượng video";
            });
            // hết video??/
        }

        private void Media_TimeChanged(object sender, MediaPlayerTimeChangedEventArgs e)
        {
            if (MediaPlayer.Time > 1 && busyIndicatorActive)
            {
                resetDeviceCounter = 0;
                Device.BeginInvokeOnMainThread(() =>
                {
                    BusyIndicatorActive = false;
                });
            }
        }

        public void SelectedChannelChanged()
        {
            if (!VMBusy)
            {
                GetListImageDataFrom();
            }
        }

        /// <summary>
        /// Reload khi video bi loi ket noi
        /// </summary>
        private void ReloadVideo()
        {
            if (isAbort)
            {
                IsError = false;
                isAbort = false;
                BusyIndicatorActive = true;
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (videoSlected?.Data != null)
                    {
                        MediaPlayer.Media = new Media(libVLC, new Uri(videoSlected?.Data.Link));
                        await Task.Delay(1000);
                        MediaPlayer.Play();
                    }
                });
            }
            else
                StopAndStartRestream();
        }

        private void FullScreenTapped()
        {
            if (isFullScreenOff)
            {
                screenOrientServices.ForceLandscape();
            }
            else screenOrientServices.ForcePortrait();
            IsFullScreenOff = !isFullScreenOff;
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

        private void VideoSelectedChange(ItemTappedEventArgs args)
        {
            if (!(args.ItemData is RestreamVideoModel item))
            {
                return;
            }
            SafeExecute(() =>
            {
                VideoSlected = item;
                MediaPlayerVisible = true;
                IsLoadingCamera = false;
                resetDeviceCounter = 0;
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsError = false;
                    BusyIndicatorActive = true;
                    if (MediaPlayer.Media != null)
                    {
                        ThreadPool.QueueUserWorkItem((r) => { MediaPlayer.Stop(); });
                        MediaPlayer.Media.Dispose();
                        MediaPlayer.Media = null;
                    }
                });
                StopAndStartRestream();
            });
        }

        public override void OnIsActiveChanged(object sender, EventArgs e)
        {
            base.OnIsActiveChanged(sender, e);
            if (!IsActive)
            {
                CloseVideo();
                Device.BeginInvokeOnMainThread(() =>
                {
                    VideoSlected = null;
                });
            }
        }

        private void CloseVideo()
        {
            MediaPlayerVisible = false;
            if (MediaPlayer.Media != null)
            {
                MediaPlayer.Media?.Dispose();
                MediaPlayer.Media = null;
            }
        }

        private void StopAndStartRestream()
        {
            var req = new StopRestreamRequest()
            {
                Channel = VideoSlected.Data.Channel,
                CustomerID = UserInfo.XNCode,
                VehicleName = Vehicle.VehiclePlate
            };
            RunOnBackground(async () =>
            {
                await streamCameraService.StopRestream(req);
            }, async () =>
            {
                await Task.Delay(6000);
                var start = new StartRestreamRequest()
                {
                    Channel = VideoSlected.Data.Channel,
                    CustomerID = UserInfo.XNCode,
                    StartTime = VideoSlected.VideoStartTime,
                    EndTime = VideoSlected.VideoEndTime,
                    VehicleName = Vehicle.VehiclePlate
                };

                StartRestream(start);
            });
        }

        private void StartRestream(StartRestreamRequest req)
        {
            RunOnBackground(async () =>
            {
                return await streamCameraService.StartRestream(req);
            }, (result) =>
            {
                if (result?.Data != null)
                {
                    MediaPlayer.Media = new Media(libVLC, new Uri(result.Data.Link));
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        var isSteaming = await CheckDeviceStatus();
                        if (isSteaming)
                        {
                            IsLoadingCamera = false;
                            VideoSlected.Data = result.Data;
                            MediaPlayer.Play();
                        }
                        else
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                IsError = true;
                                ErrorMessenger = "Có lỗi khi kết nối server";
                            });
                        }
                    });
                }
                else
                {
                    // Video dang duoc xem tu thiet bi khác
                    StopAndStartRestream();
                }
            });
        }

        private async Task<bool> CheckDeviceStatus()
        {
            IsLoadingCamera = true;
            var result = false;
            var loopIndex = 0;
            while (IsLoadingCamera && loopIndex <= 7)
            {
                var deviceStatus = await streamCameraService.GetDevicesStatus(ConditionType.BKS, Vehicle.VehiclePlate);
                var device = deviceStatus?.Data?.FirstOrDefault();
                var streamDevice = device.CameraChannels.FirstOrDefault(x => x.Channel == videoSlected.Data.Channel);
                if (streamDevice?.CameraStatus != null)
                {
                    var isStreaming = CameraStatusExtension.IsRestreaming(streamDevice.CameraStatus);
                    if (isStreaming)
                    {
                        IsLoadingCamera = false;
                        result = true;
                    }
                }
                loopIndex++;
                if (IsLoadingCamera && loopIndex <= 7)
                {
                    await Task.Delay(1000);
                }
            }
            return result;
        }

        private void LoadMoreItems(object obj)
        {
            var listview = obj as Syncfusion.ListView.XForms.SfListView;
            listview.IsBusy = true;
            try
            {
                pageIndex++;
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

        private bool CanLoadMoreItems(object obj)
        {
            if (VideoItemsSourceOrigin.Count < pageIndex * pageCount)
                return false;
            return true;
        }

        private void LoadMore()
        {
            var source = VideoItemsSourceOrigin.Skip(pageIndex * pageCount).Take(pageCount);
            pageIndex++;
            foreach (var item in source)
            {
                VideoItemsSource.Add(item);
            }
        }

        private void GetListImageDataFrom()
        {
            VideoItemsSourceOrigin.Clear();
            VideoItemsSource = new ObservableCollection<RestreamVideoModel>();
            pageIndex = 0;
            RunOnBackground(async () =>
            {
                return await streamCameraService.RestreamCaptureImageInfo(UserInfo.XNCode,
                    Vehicle.VehiclePlate,
                    DateStart,
                    DateEnd,
                    null,
                    SelectedChannel.Value); ;
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
                            EventType = image.Type
                        };

                        videoModel.VideoName = string.Format("Camera{0}_{1}", image.Channel,
                            videoModel.VideoStartTime.ToString("yyyyMMdd_hhmmss"));

                        VideoItemsSourceOrigin.Add(videoModel);
                    }
                    VideoItemsSource = VideoItemsSourceOrigin.Skip(pageIndex * pageCount).Take(pageCount).ToObservableCollection();
                }
            });
        }

        private void SetChannelSource()
        {
            var source = new List<ChannelModel>();
            for (int i = 1; i < 5; i++)
            {
                var temp = new ChannelModel()
                {
                    Value = i,
                    Name = string.Format("Kênh {0}", i)
                };
                source.Add(temp);
                SelectedChannel = source[0];
            }           
        }

        private void RefreshData()
        {
            GetListImageDataFrom();
        }

        #endregion PrivateMethod
    }
}