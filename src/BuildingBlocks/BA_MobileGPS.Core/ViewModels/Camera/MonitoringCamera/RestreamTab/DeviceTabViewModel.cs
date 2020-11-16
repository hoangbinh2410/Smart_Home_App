using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Interfaces;
using BA_MobileGPS.Core.Models;
using BA_MobileGPS.Core.ViewModels.Base;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service.IService;
using LibVLCSharp.Shared;
using Prism.Commands;
using Prism.Navigation;
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

        private readonly int maxLoadingTime = 20;
        private int customerId = 2020;
        private string bks = "TESTTBI";
        private DateTime date = Convert.ToDateTime("2020-11-16");

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
            var endTimeFix = videoSlected.VideoEndTime.AddMinutes(1);
            var req = new StartRestreamRequest()
            {
                Channel = videoSlected.Data.Channel,
                CustomerID = customerId,
                StartTime = videoSlected.VideoStartTime,
                EndTime = endTimeFix,
                VehicleName = bks
            };
            SendRequestRestream(req);
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
            
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey(ParameterKey.Vehicle) && parameters.GetValue<Vehicle>(ParameterKey.Vehicle) is Vehicle vehiclePlate)
            {
                CloseVideo();
                Reload(vehiclePlate);
            }
        }

        private void Reload(Vehicle vehiclePlate = null)
        {
            if (vehiclePlate == null)
            {
                var req = new CameraRestreamRequest()
                {
                    CustomerId = customerId,
                    Date = date,
                    VehicleNames = bks
                };
                GetListData(req);
            }         
        }

        private void CloseVideo()
        {
            MediaPlayerVisible = false;
            Media.ClearMedia();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            Media.Dispose();
        }

        private async void SendRequestRestream(StartRestreamRequest req)
        {
            // Đóng video đang chạy:
            Media.ClearMedia();
            // Request
            var res = await streamCameraService.StartRestream(req);
            if (res?.Data != null)
            {          
                media.Data = res.Data;

                // Set lại video
                media.SetMedia(res.Data.Link);
            }          
        }

        private async void GetListData(CameraRestreamRequest req)
        {
            var source = new List<RestreamVideoModel>();
            try
            {
                var res = await streamCameraService.GetListVideoByDate(req);

                foreach (var camera in res)
                {
                    foreach (var videoInfor in camera.Data)
                    {
                        var temp = new RestreamVideoModel()
                        {
                            VideoName = string.Format("Video_CAM{0}_{1}", camera.Channel, videoInfor.StartTime.ToString("yyyyMMdd_hhmmss")),
                            VideoStartTime = videoInfor.StartTime,
                            VideoImageSource = "bg_HomeTop.png",
                            VideoTime = videoInfor.EndTime - videoInfor.StartTime,
                            Data = new StreamStart() { Channel = camera.Channel },
                            VideoEndTime = videoInfor.EndTime
                        };
                        source.Add(temp);
                    }
                }
                VideoItemsSource = source;
            }
            catch (Exception ex)
            {

               
            }
           
        }
    }
  
}