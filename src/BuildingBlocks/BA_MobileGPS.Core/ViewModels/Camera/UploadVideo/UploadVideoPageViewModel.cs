using BA_MobileGPS.Entities;
using BA_MobileGPS.Service.IService;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Extensions;

namespace BA_MobileGPS.Core.ViewModels
{
    public class UploadVideoPageViewModel : ViewModelBase
    {
        private readonly IStreamCameraService streamCameraService;

        public ICommand UploadVideoCommand { get; }

        public UploadVideoPageViewModel(INavigationService navigationService, IStreamCameraService streamCameraService) : base(navigationService)
        {
            this.streamCameraService = streamCameraService;
            UploadVideoCommand = new Command(UploadVideo);
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
                FromDate = Convert.ToDateTime("2021-05-04 08:00:11"),
                ToDate = Convert.ToDateTime("2021-05-04 08:05:11"),
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
                    ListVideo = result.Data.ToObservableCollection();
                }
            });
        }

        private void UploadVideo(object obj)
        {
            SafeExecute(async () =>
            {
                Xamarin.Forms.DependencyService.Get<IHUDProvider>().DisplayProgress("");
                var lstvideoSelected = ListVideo.Where(x => x.IsSelected == true).ToList();
                if (lstvideoSelected != null && lstvideoSelected.Count > 0)
                {
                    var isfinshUpload = false;
                    int loopIndex = 1;
                    while (loopIndex <= lstvideoSelected.Count && !isfinshUpload)
                    {
                        var result = await streamCameraService.UploadToCloud(new StartRestreamRequest()
                        {
                            Channel = VideoRestreamInfo.Channel,
                            CustomerID = 1010,
                            StartTime = lstvideoSelected[loopIndex].StartTime,
                            EndTime = lstvideoSelected[loopIndex].EndTime,
                            VehicleName = "CAMTEST2"
                        });
                        if (result != null && result.Data)
                        {
                            loopIndex++;
                            if (loopIndex == lstvideoSelected.Count)
                            {
                                isfinshUpload = true;
                            }
                        }
                    }
                }
                Xamarin.Forms.DependencyService.Get<IHUDProvider>().Dismiss();
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await PageDialog.DisplayAlertAsync("Thông báo", "Video đang được tải về server. Quý khách có thể xem các video đã tải trên tab Yêu cầu", "Đóng");

                    await NavigationService.GoBackAsync();
                });
            });
        }
    }
}