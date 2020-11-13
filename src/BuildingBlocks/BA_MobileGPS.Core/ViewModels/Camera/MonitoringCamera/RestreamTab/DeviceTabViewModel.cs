using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Interfaces;
using BA_MobileGPS.Core.Models;
using BA_MobileGPS.Core.ViewModels.Base;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service.IService;
using LibVLCSharp.Shared;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Syncfusion.ListView.XForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace BA_MobileGPS.Core.ViewModels
{
    public class DeviceTabViewModel : TabbedPageChildVMBase
    {
        private LibVLC libVLC;
        private readonly IStreamCameraService streamCameraService;
        private readonly IScreenOrientServices screenOrientServices;
        public ICommand VideoItemTapCommand { get; }
        public ICommand CloseVideoCommand { get; }
        public ICommand PreviousChapterCommand { get; }
        public ICommand NextChapterChapterCommand { get; }
        public ICommand DownloadTappedCommand { get; }
        public ICommand ShareTappedCommand { get; }
        public ICommand FullScreenTappedCommand { get; }

        //private readonly int maxLoadingTime = 20;
        //private int customerId = 1010;
        //private string bks = "CAM.PNC1";
        //private DateTime date = Convert.ToDateTime("2020-11-05");

        public DeviceTabViewModel(INavigationService navigationService, IStreamCameraService cameraService,IScreenOrientServices screenOrientServices) : base(navigationService)
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

            mediaPlayerVisible = false;

            LibVLCSharp.Shared.Core.Initialize();
            libVLC = new LibVLC("--no-osd", "--rtsp-http");
            Media = new RestreamVideoManagement(libVLC);
            isFullScreenOff = true;
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

        private List<RestreamVideoModel> videoItemsSource;

        public List<RestreamVideoModel> VideoItemsSource
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

        private RestreamVideoManagement media;

        public RestreamVideoManagement Media
        {
            get { return media; }
            set
            {
                SetProperty(ref media, value);
                RaisePropertyChanged();
            }
        }

        private bool autoSwitch;

        public bool AutoSwitch
        {
            get { return autoSwitch; }
            set
            {
                SetProperty(ref autoSwitch, value);
                RaisePropertyChanged();
            }
        }

        private void NextChapterChapter()
        {
            var selectIndex = videoItemsSource.IndexOf(videoSlected);
            if (selectIndex == videoItemsSource.Count - 1)
            {
                VideoSlected = videoItemsSource.First();
            }
            else VideoSlected = videoItemsSource[selectIndex + 1];
        }

        private void PreviousChapter()
        {
            var selectIndex = videoItemsSource.IndexOf(videoSlected);
            if (selectIndex == 0)
            {
                VideoSlected = videoItemsSource.Last();
            }
            else VideoSlected = videoItemsSource[selectIndex - 1];
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
            media.SetMedia(videoSlected.VideoUrl);
        }


        public override void OnIsActiveChanged(object sender, EventArgs e)
        {
            base.OnIsActiveChanged(sender, e);
            if (IsActive)
            {
                Reload();
            }
            else CloseVideo();
        }

        private void ScreenOrientChange()
        {
            MediaPlayerVisible = !isFullScreenOff;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey(ParameterKey.Vehicle) && parameters.GetValue<Vehicle>(ParameterKey.Vehicle) is Vehicle vehiclePlate)
            {
                CloseVideo();
                Reload();
            }
        }

        private void Reload()
        {
            var source = new List<RestreamVideoModel>()
            {
                new RestreamVideoModel()
                {
                    VideoName = string.Format("Video_{0}_{1}", 1, DateTime.Now.ToString("yyyyMMdd_hhmmss")),
                    VideoImageSource = "https://commondatastorage.googleapis.com/gtv-videos-bucket/sample/images/ElephantsDream.jpg",
                    VideoTime = new TimeSpan(0, 5, 0),
                    VideoUrl ="https://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ElephantsDream.mp4",
                    Channel = 1,
                },
                new RestreamVideoModel()
                {
                    VideoName = string.Format("Video_{0}_{1}", 2, DateTime.Now.ToString("yyyyMMdd_hhmmss")),
                    VideoImageSource = "https://commondatastorage.googleapis.com/gtv-videos-bucket/sample/images/BigBuckBunny.jpg",
                    VideoTime = new TimeSpan(0, 5, 0),
                    VideoUrl ="https://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4",
                    Channel = 1,
                },
                new RestreamVideoModel()
                {
                    VideoName = string.Format("Video_{0}_{1}", 3, DateTime.Now.ToString("yyyyMMdd_hhmmss")),
                    VideoImageSource = "https://commondatastorage.googleapis.com/gtv-videos-bucket/sample/images/ForBiggerBlazes.jpg",
                    VideoTime = new TimeSpan(0, 5, 0),
                    VideoUrl ="https://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ForBiggerBlazes.mp4",
                    Channel = 1,
                },
                new RestreamVideoModel()
                {
                    VideoName = string.Format("Video_{0}_{1}", 4, DateTime.Now.ToString("yyyyMMdd_hhmmss")),
                    VideoImageSource = "https://commondatastorage.googleapis.com/gtv-videos-bucket/sample/images/ForBiggerEscapes.jpg",
                    VideoTime = new TimeSpan(0, 5, 0),
                    VideoUrl ="https://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ForBiggerEscapes.mp4",
                    Channel = 1,
                },
                new RestreamVideoModel()
                {
                    VideoName = string.Format("Video_{0}_{1}", 1, DateTime.Now.ToString("yyyyMMdd_hhmmss")),
                    VideoImageSource = "https://commondatastorage.googleapis.com/gtv-videos-bucket/sample/images/ForBiggerFun.jpg",
                    VideoTime = new TimeSpan(0, 5, 0),
                    VideoUrl ="https://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ForBiggerFun.mp4",
                    Channel = 1,
                }
            };
            VideoItemsSource = source;
        }

        private void CloseVideo()
        {
            MediaPlayerVisible = false;
            Media.Dispose();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

        }

        //private async void SendRequestRestream(StartRestreamRequest req)
        //{
        //    var res  = await streamCameraService.StartRestream(req);
        //    if (res?.Data != null)
        //    {
        //        var data = res.Data.FirstOrDefault();
        //        media.Data = data;
        //        media.SetMedia(data.Link);
        //    }
        //var req = new StartRestreamRequest()
        //{
        //    Channel = item.Channel,
        //    CustomerID = customerId,
        //    StartTime = item.VideoStartTime,
        //    EndTime = item.VideoEndTime,
        //    VehicleName = bks
        //};
        //SendRequestRestream(req);
        //}

        //private async void GetData(CameraRestreamRequest req)
        //{
        //    var source = new List<RestreamVideoModel>();

        //    var res = await streamCameraService.GetListVideoByDate(req);
        //    foreach (var camera in res)
        //    {
        //        foreach (var videoInfor in camera.Data)
        //        {
        //            var temp = new RestreamVideoModel()
        //            {
        //                VideoName = string.Format("Video_{0}_{1}", camera.Channel, videoInfor.StartTime.ToString("yyyyMMdd_hhmmss")),
        //                VideoStartTime = videoInfor.StartTime,
        //                VideoImageSource = "bg_HomeTop.png",
        //                VideoTime = videoInfor.EndTime - videoInfor.StartTime,
        //                Channel = camera.Channel,
        //                VideoEndTime = videoInfor.EndTime
        //            };
        //            source.Add(temp);
        //        }
        //    }
        //    VideoItemsSource = source;
        //}
    }

    public class RestreamVideoModel
    {
        public string VideoName { get; set; }
        public DateTime VideoStartTime { get; set; }
        public string VideoImageSource { get; set; }
        public TimeSpan VideoTime { get; set; }
        public string VideoUrl { get; set; }
        public int Channel { get; set; }
        public DateTime VideoEndTime { get; set; }
    }



    public class RestreamVideoManagement : BindableBase, IDisposable
    {

        public RestreamVideoManagement(LibVLC libVLC)
        {
            this.LibVLC = libVLC;
            InitMediaPlayer();
        }

        private void InitMediaPlayer()
        {
            mediaPlayer = new MediaPlayer(libVLC);
            mediaPlayer.TimeChanged += MediaPlayer_TimeChanged;
            mediaPlayer.EncounteredError += MediaPlayer_EncounteredError;
            mediaPlayer.EndReached += MediaPlayer_EndReached;

            mediaPlayer.AspectRatio = "16:9";
            mediaPlayer.Scale = 0;
        }


        public void SetMedia(string url)
        {
            MediaPlayer.Media = new Media(libVLC, new Uri(url));
            MediaPlayer.Play();
        }



        // Hết video?
        private void MediaPlayer_EndReached(object sender, EventArgs e)
        {

        }

        private void MediaPlayer_EncounteredError(object sender, EventArgs e)
        {

        }

        private void MediaPlayer_TimeChanged(object sender, MediaPlayerTimeChangedEventArgs e)
        {

        }

        private LibVLC libVLC;
        public LibVLC LibVLC
        {
            get { return libVLC; }
            set
            {
                SetProperty(ref libVLC, value);
            }
        }

        private MediaPlayer mediaPlayer;

        public MediaPlayer MediaPlayer
        {
            get { return mediaPlayer; }
            set
            {
                SetProperty(ref mediaPlayer, value);
                RaisePropertyChanged();
            }
        }

        public void Dispose()
        {
            mediaPlayer.TimeChanged -= MediaPlayer_TimeChanged;
            mediaPlayer.EncounteredError -= MediaPlayer_EncounteredError;
            mediaPlayer.EndReached -= MediaPlayer_EndReached;

            //mediaPlayer?.Media?.Dispose();
            //mediaPlayer?.Dispose();
        }
    }
}