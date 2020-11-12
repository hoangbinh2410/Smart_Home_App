using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Models;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service.IService;
using LibVLCSharp.Shared;
using Prism.Commands;
using Prism.Navigation;
using Syncfusion.ListView.XForms;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace BA_MobileGPS.Core.ViewModels
{
    public class DeviceTabViewModel : ViewModelBase
    {
        private LibVLC libVLC;
        private readonly IStreamCameraService streamCameraService;
        private readonly int maxLoadingTime = 20;
        public ICommand VideoItemTapCommand { get; }
        public ICommand CloseVideoCommand { get; }

        private int customerId = 1010;
        private string bks = "CAM.PNC1";
        private DateTime date = Convert.ToDateTime("2020-11-05");

        public DeviceTabViewModel(INavigationService navigationService, IStreamCameraService cameraService) : base(navigationService)
        {
            streamCameraService = cameraService;
            VideoItemTapCommand = new DelegateCommand<object>(VideoItemTap);
            CloseVideoCommand = new DelegateCommand(CloseVideo);
            mediaPlayerVisible = false;
            LibVLCSharp.Shared.Core.Initialize();
            libVLC = new LibVLC("--no-osd", "--rtsp-tcp");
            Media = new CameraManagement(maxLoadingTime, libVLC);
        }

        private void CloseVideo()
        {
            MediaPlayerVisible = false;
            Media.Dispose();
        }

        private void VideoItemTap(object obj)
        {
            if (obj != null && obj is ItemTappedEventArgs itemTapped)
            {
                var item = (RestreamVideoModel)itemTapped.ItemData;
                MediaPlayerVisible = true;

                media.SetMedia(item.VideoUrl);
                





                //var req = new StartRestreamRequest()
                //{
                //    Channel = item.Channel,
                //    CustomerID = customerId,
                //    StartTime = item.VideoStartTime,
                //    EndTime = item.VideoEndTime,
                //    VehicleName = bks
                //};
                //SendRequestRestream(req);
            }
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
        //}

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
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

        private CameraManagement media;

        public CameraManagement Media
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

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey(ParameterKey.Vehicle) && parameters.GetValue<Vehicle>(ParameterKey.Vehicle) is Vehicle vehiclePlate)
            {
                CloseVideo();
                Reload();
            }
            //var req = new CameraRestreamRequest()
            //{
            //    customerId = customerId,
            //    Date = date,
            //    VehicleNames = bks
            //};
            //GetData(req);
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
}