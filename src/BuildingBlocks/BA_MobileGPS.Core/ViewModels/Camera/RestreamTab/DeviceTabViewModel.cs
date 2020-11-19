using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Interfaces;
using BA_MobileGPS.Core.Models;
using BA_MobileGPS.Core.ViewModels.Base;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service.IService;
using BA_MobileGPS.Utilities;
using LibVLCSharp.Shared;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class DeviceTabViewModel : TabbedPageChildVMBase
    {
        private readonly IStreamCameraService streamCameraService;
        private readonly IScreenOrientServices screenOrientServices;
        private readonly int configMinute = 3;
        // private readonly int 
        private int pageIndex { get; set; } = 0;
        private int pageCount { get; } = 20;
        private readonly DateTime fromTimeDefault = Convert.ToDateTime("2020-11-16 12:00:00 AM");
        private readonly DateTime toTimeDefault = Convert.ToDateTime("2020-11-16 11:00:00 PM");
        private List<RestreamVideoModel> basePNCSource { get; set; } = new List<RestreamVideoModel>();

        public ICommand VideoItemTapCommand { get; }
        public ICommand CloseVideoCommand { get; }
        public ICommand PreviousChapterCommand { get; }
        public ICommand NextChapterChapterCommand { get; }
        public ICommand DownloadTappedCommand { get; }
        public ICommand ShareTappedCommand { get; }
        public ICommand FullScreenTappedCommand { get; }
        public ICommand ReLoadCommand { get; }
        public ICommand LoadMoreItemsCommand { get;  }
        public ICommand RestoreSearchCommand { get;  }

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
            VideoItemTapCommand = new DelegateCommand<object>(VideoItemTap);
            CloseVideoCommand = new DelegateCommand(CloseVideo);
            PreviousChapterCommand = new DelegateCommand(PreviousChapter);
            NextChapterChapterCommand = new DelegateCommand(NextChapterChapter);
            DownloadTappedCommand = new DelegateCommand(DownloadTapped);
            ShareTappedCommand = new DelegateCommand(ShareTapped);
            FullScreenTappedCommand = new DelegateCommand(FullScreenTapped);
            ReLoadCommand = new DelegateCommand(ReloadVideo);
            LoadMoreItemsCommand = new DelegateCommand<object>(LoadMoreItems, CanLoadMoreItems);
            RestoreSearchCommand = new DelegateCommand(RestoreSearch);
            mediaPlayerVisible = false;
            videoItemsSource = new ObservableCollection<RestreamVideoModel>();
        }

        public override void OnPageAppearingFirstTime()
        {
            dateStart = fromTimeDefault;
            dateEnd = toTimeDefault;
            LibVLCSharp.Shared.Core.Initialize();
            LibVLC = new LibVLC("--no-rtsp-tcp");
            Media = new MediaPlayer(libVLC);
            Media.TimeChanged += Media_TimeChanged;
            Media.EndReached += Media_EndReached;
            Media.EncounteredError += Media_EncounteredError;
            IsFullScreenOff = true;
            base.OnPageAppearingFirstTime();
            SetChannelSource();
            IsError = false;
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
            set => SetProperty(ref dateStart, value, DateStartChange);
        }
        private void DateStartChange()
        {
            GetListImageDataFromPNC(dateStart, dateEnd);
        }

        private DateTime dateEnd;
        public DateTime DateEnd
        {
            get => dateEnd;
            set => SetProperty(ref dateEnd, value, DateEndChange);
        }

        private void DateEndChange()
        {
            GetListImageDataFromPNC(dateStart, dateEnd);
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

        private void Media_EncounteredError(object sender, EventArgs e)
        {          
            Device.BeginInvokeOnMainThread(() =>
            {
                IsError = true;
                ErrorMessenger = "Có lỗi khi gán url..";
            });
        }

        private void Media_EndReached(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                IsError = true;
                ErrorMessenger = "Hết video";
            });
        }

        private void Media_TimeChanged(object sender, MediaPlayerTimeChangedEventArgs e)
        {
            if (media.Time > 1 && busyIndicatorActive)
            {
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
            GetListImageDataFromPNC(dateStart, dateEnd, channel: selectedChannel.Value);
        }

        /// <summary>
        /// Reload khi video bi loi ket noi
        /// </summary>
        private void ReloadVideo()
        {
            VideoSelectedChange();
        }

        private bool isFullScreenOff;

        public bool IsFullScreenOff
        {
            get { return isFullScreenOff; }
            set
            {
                SetProperty(ref isFullScreenOff, value, ScreenOrientChange);
                RaisePropertyChanged();
            }
        }

        private RestreamVideoModel videoSlected;

        public RestreamVideoModel VideoSlected
        {
            get { return videoSlected; }
            set
            {
                SetProperty(ref videoSlected, value, VideoSelectedChange);
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<RestreamVideoModel> videoItemsSource;

        public ObservableCollection<RestreamVideoModel> VideoItemsSource
        {
            get { return videoItemsSource; }
            set
            {
                SetProperty(ref videoItemsSource, value);
                RaisePropertyChanged();
            }
        }

        private bool mediaPlayerVisible;

        public bool MediaPlayerVisible
        {
            get { return mediaPlayerVisible; }
            set
            {
                SetProperty(ref mediaPlayerVisible, value);
                RaisePropertyChanged();
            }
        }

        private LibVLC libVLC;

        public LibVLC LibVLC
        {
            get { return libVLC; }
            set { SetProperty(ref libVLC, value); }
        }

        private MediaPlayer media;

        public MediaPlayer Media
        {
            get { return media; }
            set
            {
                SetProperty(ref media, value);
                RaisePropertyChanged();
            }
        }

        private void NextChapterChapter()
        {
            //var selectIndex = videoItemsSource.IndexOf(videoSlected);
            //if (selectIndex == videoItemsSource.Count - 1)
            //{
            //    VideoSlected = videoItemsSource.First();
            //}
            //else VideoSlected = videoItemsSource[selectIndex + 1];
        }

        private void PreviousChapter()
        {
            //var selectIndex = videoItemsSource.IndexOf(videoSlected);
            //if (selectIndex == 0)
            //{
            //    VideoSlected = videoItemsSource.Last();
            //}
            //else VideoSlected = videoItemsSource[selectIndex - 1];
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

        private void ShareTapped()
        {
        }

        private void DownloadTapped()
        {
        }

        private void VideoItemTap(object obj)
        {
            if (obj != null)
            {
                var item = (RestreamVideoModel)obj;

                MediaPlayerVisible = true;
                VideoSlected = item;
            }
        }

        private void VideoSelectedChange()
        {
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsError = false;
                    BusyIndicatorActive = true;
                    try
                    {
                        ThreadPool.QueueUserWorkItem((r) => { Media.Stop(); });
                        Media.Media.Dispose();
                        Media.Media = null;                     
                    }
                    catch (Exception ex)
                    {

                        
                    }                  
                });
           
          
           
            var req = new StopRestreamRequest()
            {
                Channel = videoSlected.Data.Channel,
                CustomerID = customerId,
                VehicleName = bks
            };
            StopRestream(req);
        }

        public override void OnIsActiveChanged(object sender, EventArgs e)
        {
            base.OnIsActiveChanged(sender, e);
            if (!IsActive)
            {
                CloseVideo();
            }
        }

        private void ScreenOrientChange()
        {
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey(ParameterKey.Vehicle) && parameters.GetValue<Vehicle>(ParameterKey.Vehicle) is Vehicle vehiclePlate)
            {
                CloseVideo();
                //ReloadPage(vehiclePlate);
                DisplayMessage.ShowMessageError("Chức năng chưa có nhé...");
            }
        }

        private void ReloadPage(Vehicle vehiclePlate = null)
        {
            GetListImageDataFromPNC(dateStart, dateEnd);
        }


        private void CloseVideo()
        {
            MediaPlayerVisible = false;
            if (Media.Media != null)
            {
                Media.Media?.Dispose();
                Media.Media = null;
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (Media.Media != null)
            {
                Media.Media?.Dispose();
                Media.Media = null;
            }
            var media = Media;
            Media = null;
            media?.Dispose();
        }

        private void StopRestream(StopRestreamRequest req)
        {
            RunOnBackground(async () =>
            {
                await streamCameraService.StopRestream(req);
            }, async () =>
             {
                 await Task.Delay(6000);
                 var start = new StartRestreamRequest()
                 {
                     Channel = videoSlected.Data.Channel,
                     CustomerID = customerId,
                     StartTime = videoSlected.VideoStartTime,
                     EndTime = videoSlected.VideoEndTime,
                     VehicleName = bks
                 };

                 StartRestream(start);
             });
        }


        private void StartRestream(StartRestreamRequest req)
        {
            RunOnBackground(async () =>
            {
                Debug.WriteLine(" GUI LENH START......................");
                return await streamCameraService.StartRestream(req);
            }, (result) =>
            {
                if (result?.Data != null)
                {
                    SetMediaUrl(result.Data.Link);
                }
                //Debug.WriteLine(" CHO 5S......................");
                //await Task.Delay(5000);
                //Debug.WriteLine(" KIEM TRA THIET BI......................");
                //var status = await CheckDeviceStatus();
            });
        }

        private void SetMediaUrl(string url)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                media.Media = new Media(libVLC, new Uri(url));
                await Task.Delay(1000);
                Media.Play();
            });
        }

        private async Task<bool> CheckDeviceStatus()
        {
            var deviceStatus = await streamCameraService.GetDevicesStatus(ConditionType.BKS, bks);

            var device = deviceStatus?.Data?.FirstOrDefault();
            var streamDevice = device.CameraChannels.FirstOrDefault(x => x.Channel == videoSlected.Data.Channel);
            if (streamDevice?.CameraStatus != 2 && streamDevice?.CameraStatus != 3)
            {
                return false;
            }
            else return true;
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
            if (basePNCSource.Count < pageIndex * pageCount)
                return false;
            return true;
        }

        private void LoadMore()
        {
            var source = basePNCSource.Skip(pageIndex * pageCount).Take(pageCount);
            pageIndex++;
            foreach (var item in source)
            {
                VideoItemsSource.Add(item);
            }
        }

        private void GetListImageDataFromPNC(DateTime fromTime, DateTime toTime, int? limit = null, int? channel = null)
        {
            VideoItemsSource.Clear();
            var xncode = customerId;
            var vehiclePlate = bks;
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
                         videoModel.VideoName = string.Format("Video_CAM{0}_{1}", image.Channel, 
                             videoModel.VideoStartTime.ToString("yyyyMMdd_hhmmss"));
                         basePNCSource.Add(videoModel);
                     }
                     LoadMore();
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
            DateStart = fromTimeDefault;
            DateEnd = toTimeDefault;
            SelectedChannel = null;
        }
    }

    public class ChannelModel
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }
}