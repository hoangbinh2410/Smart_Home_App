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
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class DeviceTabViewModel : TabbedPageChildVMBase
    {
        private readonly IStreamCameraService streamCameraService;
        private readonly IScreenOrientServices screenOrientServices;
        private int pageIndex { get; set; } = 0;
        private int pageCount { get; } = 20;

        private List<AppVideoTimeInfor> basePNCSource { get; set; } = new List<AppVideoTimeInfor>();

        public ICommand VideoItemTapCommand { get; }
        public ICommand CloseVideoCommand { get; }
        public ICommand PreviousChapterCommand { get; }
        public ICommand NextChapterChapterCommand { get; }
        public ICommand DownloadTappedCommand { get; }
        public ICommand ShareTappedCommand { get; }
        public ICommand FullScreenTappedCommand { get; }
        public ICommand ReLoadCommand { get; }
        public ICommand LoadMoreItemsCommand { get; set; }

        private readonly int maxLoadingTime = 60;
        private int customerId = 2020;
        private string bks = "TESTTBI";
        private DateTime date = Convert.ToDateTime("2020-11-17");

        public DeviceTabViewModel(INavigationService navigationService, IStreamCameraService cameraService, IScreenOrientServices screenOrientServices) : base(navigationService)
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
            mediaPlayerVisible = true;
            videoItemsSource = new ObservableCollection<RestreamVideoModel>();
        }

        public override void OnPageAppearingFirstTime()
        {
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
                //  IsError = true;
            });
        }

        private void Media_EndReached(object sender, EventArgs e)
        {
            //Device.BeginInvokeOnMainThread(() =>
            //{
            //    IsError = true;
            //});
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

        private List<string> listChannel;

        public List<string> ListChannel
        {
            get { return listChannel; }
            set { SetProperty(ref listChannel, value); }
        }

        private string selectedChannel;

        public string SelectedChannel
        {
            get { return selectedChannel; }
            set { SetProperty(ref selectedChannel, value, SelectedChannelChanged); }
        }

        public void SelectedChannelChanged()
        {
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
            IsError = false;
            BusyIndicatorActive = true;
            // var endTimeFix = videoSlected.VideoEndTime.AddMinutes(1);
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
                ReloadPage(vehiclePlate);
            }
        }

        private void ReloadPage(Vehicle vehiclePlate = null)
        {
            if (vehiclePlate == null)
            {
                var req = new CameraRestreamRequest()
                {
                    CustomerId = customerId,
                    Date = date,
                    VehicleNames = bks
                };
                GetListDataFromPNC(req);
            }
        }

        private List<RestreamVideoModel> ConvertSourceFromPNC(IEnumerable<AppVideoTimeInfor> source)
        {
            var result = new List<RestreamVideoModel>();
            foreach (var item in source)
            {
                var temp = new RestreamVideoModel()
                {
                    VideoName = string.Format("Video_CAM{0}_{1}", item.Channel, item.StartTime.ToString("yyyyMMdd_hhmmss")),
                    VideoStartTime = item.StartTime,
                    VideoImageSource = "bg_HomeTop.png",
                    VideoTime = item.EndTime - item.StartTime,
                    Data = new StreamStart() { Channel = item.Channel },
                    VideoEndTime = item.EndTime
                };
                result.Add(temp);
            }
            return result;
        }

        private void CloseVideo()
        {
            MediaPlayerVisible = false;
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
                return await streamCameraService.StartRestream(req);
            }, (result) =>
            {
                if (result?.Data != null)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        media.Media = new Media(libVLC, new Uri(result.Data.Link));
                        await Task.Delay(1000);
                        Media.Play();
                    });
                }
            });
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
            ConvertSourceFromPNC(source).
                ForEach((model) =>
                {
                    VideoItemsSource.Add(model);
                });
        }

        private void GetListDataFromPNC(CameraRestreamRequest req)
        {
            RunOnBackground(async () =>
            {
                return await streamCameraService.GetListVideoByDate(req);
            }, (result) =>
             {
                 if (result != null && result.Count > 0)
                 {
                     foreach (var camera in result)
                     {
                         foreach (var videoInfor in camera.Data)
                         {
                             videoInfor.Channel = camera.Channel;
                             videoInfor.EndTime = videoInfor.EndTime.AddMinutes(9);
                         }
                         basePNCSource.AddRange(camera.Data);
                     }
                     if (basePNCSource.Count > 0)
                     {
                         var source = basePNCSource.Take(pageCount);
                         pageIndex++;
                         var sourceConvert = ConvertSourceFromPNC(source);
                         VideoItemsSource = new ObservableCollection<RestreamVideoModel>(sourceConvert);
                     }
                 }
             });
        }

        private void SetChannelSource()
        {
            var source = new List<string>() { "Kênh 1", "Kênh 2" };
            ListChannel = source;
        }
    }
}