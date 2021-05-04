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

        private ObservableCollection<CameraRestreamInfo> listVideo = new ObservableCollection<CameraRestreamInfo>();
        public ObservableCollection<CameraRestreamInfo> ListVideo { get => listVideo; set => SetProperty(ref listVideo, value); }

        private void GetListVideoUploadActive()
        {
            var req = new CameraRestreamRequest()
            {
                CustomerId = 1010,
                Date = Convert.ToDateTime("2021-04-14"),
                VehicleNames = "CAMTEST2"
            };
            RunOnBackground(async () =>
            {
                return await streamCameraService.GetListVideoOnDevice(req);
            }, (result) =>
            {
                if (result != null && result.Count > 0)
                {
                    ListVideo = result.ToObservableCollection();
                }
            });
        }
    }
}