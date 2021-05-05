using BA_MobileGPS.Entities;
using BA_MobileGPS.Service.IService;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms.Extensions; 

namespace BA_MobileGPS.Core.ViewModels
{
    public class UploadVideoPageViewModel : ViewModelBase
    {
        private readonly IStreamCameraService streamCameraService;

        public UploadVideoPageViewModel(INavigationService navigationService, IStreamCameraService streamCameraService) : base(navigationService)
        {
            this.streamCameraService = streamCameraService;
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            GetListVideoUploadActive();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        private VideoRestreamInfo videoRestreamInfo = new VideoRestreamInfo();
        public VideoRestreamInfo VideoRestreamInfo { get => videoRestreamInfo; set => SetProperty(ref videoRestreamInfo, value); }
        private ObservableCollection<VideoUploadTimeInfo> listVideo = new ObservableCollection<VideoUploadTimeInfo>();
        public ObservableCollection<VideoUploadTimeInfo> ListVideo { get => listVideo; set => SetProperty(ref listVideo, value); }

        private void GetListVideoUploadActive()
        {
            var req = new CameraUploadRequest()
            {
                CustomerId = 1010,
                FromDate = Convert.ToDateTime("2021-05-04 08:05:11"),
                ToDate = Convert.ToDateTime("2021-05-04 08:15:11"),
                Channel = 1,
                VehiclePlate = "CAMTEST2"
            };
            RunOnBackground(async () =>
            {
                return await streamCameraService.GetListVideoNotUpload(req);
            }, (result) =>
            {
                if (result != null && result.Data != null && result.Data.Count > 0)
                {
                    VideoRestreamInfo = result;
                    foreach (var item in result.Data)
                    {
                        item.FileName = FormatVideoFileName(item);
                    }
                    ListVideo = result.Data.ToObservableCollection();
                }
            });
        }


        private string FormatVideoFileName(VideoUploadTimeInfo info)
        {
            var dt = info.StartTime;
            return (dt.Day < 10 ? "0" + dt.Day.ToString() : dt.Day.ToString()) + (dt.Month < 9 ? "0" + (dt.Month + 1).ToString() : (dt.Month + 1).ToString()) +
             dt.Year + '_' +
            (dt.Hour < 10 ? "0" + dt.Hour.ToString() : dt.Hour.ToString()) +
            (dt.Minute < 10 ? "0" + dt.Minute.ToString() : dt.Minute.ToString()) +
            (dt.Second < 10 ? "0" + dt.Second.ToString() : dt.Second.ToString()) + "_CAM" + VideoRestreamInfo.Channel;
        }
    }
}