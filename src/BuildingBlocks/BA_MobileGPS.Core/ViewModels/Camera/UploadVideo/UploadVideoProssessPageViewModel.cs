using BA_MobileGPS.Entities;
using Prism.Navigation;
using System;

namespace BA_MobileGPS.Core.ViewModels
{
    public class UploadVideoProssessPageViewModel : ViewModelBase
    {
        public UploadVideoProssessPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            EventAggregator.GetEvent<UploadFinishVideoEvent>().Subscribe(UploadFinishVideo);
        }

        private void UploadFinishVideo(bool obj)
        {
            ClosePageCommand.Execute(null);
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            TotalVideoUploaded = GlobalResources.Current.TotalVideoUpload;
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
            EventAggregator.GetEvent<UploadFinishVideoEvent>().Unsubscribe(UploadFinishVideo);
        }

        private int totalVideoUploaded;

        public int TotalVideoUploaded
        {
            get => totalVideoUploaded; set => SetProperty(ref totalVideoUploaded, value);
        }
    }
}