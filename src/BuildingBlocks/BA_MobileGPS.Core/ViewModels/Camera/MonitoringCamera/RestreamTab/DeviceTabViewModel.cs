using BA_MobileGPS.Service.IService;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BA_MobileGPS.Core.ViewModels
{
    public class DeviceTabViewModel : ViewModelBase
    {
        private readonly IStreamCameraService streamCameraService;
        public DeviceTabViewModel(INavigationService navigationService, IStreamCameraService cameraService) : base(navigationService)
        {
            streamCameraService = cameraService;
        }

        private List<RestreamVideoModel> videoItemsSource;
        public List<RestreamVideoModel> VideoItemsSource
        {
            get { return videoItemsSource; }
            set { SetProperty(ref videoItemsSource, value);
                RaisePropertyChanged();
            }
        }


    }

    public class RestreamVideoModel
    {
        public string VideoName { get; set; }
        public DateTime VideoStartTime { get; set; }
        public string VideoImageSource { get; set; }
        public string VideoTime { get; set; }
        public string VideoUrl { get; set; }
        
    }
}
