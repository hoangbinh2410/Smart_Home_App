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
        public ICommand DownloadTappedCommand { get; }
        public ICommand FullScreenTappedCommand { get; }
        public ICommand ReLoadCommand { get; }
        public ICommand LoadMoreItemsCommand { get; }
        public ICommand RestoreSearchCommand { get; }
        public ICommand VideoItemTapCommand { get; set; }

        //private readonly int maxLoadingTime = 60;
        private int customerId = 1010;

        private string bks = "QATEST1";
        // private DateTime date = Convert.ToDateTime("2020-11-17");

        public DeviceTabViewModel(INavigationService navigationService,
            IStreamCameraService cameraService,
            IScreenOrientServices screenOrientServices) : base(navigationService)
        {
            streamCameraService = cameraService;
            this.screenOrientServices = screenOrientServices;
            DownloadTappedCommand = new DelegateCommand(DownloadTapped);
            FullScreenTappedCommand = new DelegateCommand(FullScreenTapped);
            ReLoadCommand = new DelegateCommand(ReloadVideo);
            LoadMoreItemsCommand = new DelegateCommand<object>(LoadMoreItems, CanLoadMoreItems);
            RestoreSearchCommand = new DelegateCommand(RestoreSearch);
            VideoItemTapCommand = new DelegateCommand<ItemTappedEventArgs>(VideoSelectedChange);
            mediaPlayerVisible = false;
            videoItemsSource = new ObservableCollection<RestreamVideoModel>();
        }

        public override void OnPageAppearingFirstTime()
        {
            VMBusy = true;
            DateStart = fromTimeDefault;
            DateEnd = toTimeDefault;
            LibVLCSharp.Shared.Core.Initialize();
            LibVLC = new LibVLC("--no-rtsp-tcp");
            MediaPlayer = new MediaPlayer(libVLC);
            MediaPlayer.TimeChanged += Media_TimeChanged;
            MediaPlayer.EndReached += Media_EndReached;
            MediaPlayer.EncounteredError += Media_EncounteredError;
            IsFullScreenOff = true;
            base.OnPageAppearingFirstTime();
            SetChannelSource();
            IsError = false;
            VMBusy = false;
            ReloadPage();
        }

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
                else GetListImageDataFrom(dateStart, dateEnd);
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
        private readonly DateTime fromTimeDefault = Convert.ToDateTime("2020-11-16 12:00:00 AM");
        private readonly DateTime toTimeDefault = Convert.ToDateTime("2020-11-16 11:00:00 PM");
        private List<RestreamVideoModel> VideoItemsSourceOrigin = new List<RestreamVideoModel>();
        private bool VMBusy { get; set; }
        private bool IsLoadingCamera = false;

        // dem so lan request lai khi connect fail, gioi han la 3
        private int resetDeviceCounter = 0;

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
                ErrorMessenger = "Aborting 10s";
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

        public void SelectedChannelChanged()
        {
            if (!VMBusy)
            {
                GetListImageDataFrom(dateStart, dateEnd, channel: selectedChannel.Value);
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

        private bool isFullScreenOff;

        public bool IsFullScreenOff
        {
            get => isFullScreenOff; set => SetProperty(ref isFullScreenOff, value);
        }

        private RestreamVideoModel videoSlected;

        public RestreamVideoModel VideoSlected
        {
            get => videoSlected; set => SetProperty(ref videoSlected, value);
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

        private void FullScreenTapped()
        {
            if (isFullScreenOff)
            {
                screenOrientServices.ForceLandscape();
            }
            else screenOrientServices.ForcePortrait();
            IsFullScreenOff = !isFullScreenOff;
        }

        private void DownloadTapped()
        {
            DisplayMessage.ShowMessageError("Chức năng chưa hoàn thiện...");
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
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey(ParameterKey.Vehicle) && parameters.GetValue<Vehicle>(ParameterKey.Vehicle) is Vehicle vehiclePlate)
            {
                CloseVideo();
                //ReloadPage(vehiclePlate);
                DisplayMessage.ShowMessageError("Chức năng chưa hoàn thiện...");
            }
        }

        private void ReloadPage(Vehicle vehiclePlate = null)
        {
            GetListImageDataFrom(dateStart, dateEnd);
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

        private void StopAndStartRestream()
        {
            var req = new StopRestreamRequest()
            {
                Channel = VideoSlected.Data.Channel,
                CustomerID = customerId,
                VehicleName = bks
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
                     CustomerID = customerId,
                     StartTime = VideoSlected.VideoStartTime,
                     EndTime = VideoSlected.VideoEndTime,
                     VehicleName = bks
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
                    });
                }
                else
                {
                    // Video dang duoc xem tu thiet bi khác
                }
            });
        }

        private async Task<bool> CheckDeviceStatus()
        {
            IsLoadingCamera = true;
            var result = false;
            var loopIndex = 0;
            while (IsLoadingCamera && loopIndex <= 5)
            {
                var deviceStatus = await streamCameraService.GetDevicesStatus(ConditionType.BKS, bks);
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
                if (IsLoadingCamera && loopIndex <= 5)
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

        private void GetListImageDataFrom(DateTime fromTime, DateTime toTime, int? limit = null, int? channel = null)
        {
            VideoItemsSourceOrigin.Clear();

            VideoItemsSource = new ObservableCollection<RestreamVideoModel>();

            var xncode = customerId;
            var vehiclePlate = bks;
            pageIndex = 0;
            RunOnBackground(async () =>
            {
                return await streamCameraService.RestreamCaptureImageInfo(xncode, vehiclePlate, fromTime, toTime, limit, channel);
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
            RunOnBackground(async () =>
            {
                return await streamCameraService.GetDevicesStatus(ConditionType.BKS, bks);
            }, (result) =>
            {
                var device = result?.Data?.FirstOrDefault();
                if (device?.CameraChannels != null && device.CameraChannels.Count > 0)
                {
                    var source = new List<ChannelModel>();
                    foreach (var channel in device.CameraChannels)
                    {
                        var temp = new ChannelModel()
                        {
                            Value = channel.Channel,
                            Name = string.Format("Kênh {0}", channel.Channel)
                        };
                        source.Add(temp);
                    }
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        ListChannel = source;
                    });
                }
            });
        }

        private void RestoreSearch()
        {
            VMBusy = true;
            DateStart = fromTimeDefault;
            DateEnd = toTimeDefault;
            SelectedChannel = null;
            VMBusy = false;
            ReloadPage();
        }
    }
}