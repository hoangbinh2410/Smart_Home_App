using BA_MobileGPS.Core.Interfaces;
using BA_MobileGPS.Core.ViewModels.Base;
using BA_MobileGPS.Service.IService;
using Prism.Commands;
using Prism.Navigation;
using System.Windows.Input;

namespace BA_MobileGPS.Core.ViewModels
{
    public class RestreamChildVMBase : TabbedPageChildVMBase
    {
        protected readonly IStreamCameraService streamCameraService;
        protected readonly IScreenOrientServices screenOrientServices;

        public ICommand FullScreenTappedCommand { get; }
        public ICommand VideoItemTapCommand { get; set; }

        public RestreamChildVMBase(INavigationService navigationService, 
            IStreamCameraService cameraService,
            IScreenOrientServices screenOrientServices) : base(navigationService)
        {
            streamCameraService = cameraService;
            this.screenOrientServices = screenOrientServices;
            FullScreenTappedCommand = new DelegateCommand(FullScreenTapped);
            IsFullScreenOff = true;
            IsError = false;
        }

        #region Property
        protected int pageIndex { get; set; } = 0;
        protected int pageCount { get; } = 10; // so luong anh cho 1 lan infinite scroll 

        private bool isFullScreenOff;

        public bool IsFullScreenOff
        {
            get => isFullScreenOff; set => SetProperty(ref isFullScreenOff, value);
        }

        private string errorMessenger;

        public string ErrorMessenger
        {
            get => errorMessenger;
            set => SetProperty(ref errorMessenger, value);
        }

        private bool isError;

        public bool IsError
        {
            get => isError;
            set => SetProperty(ref isError, value);
        }

        private bool busyIndicatorActive;

        public bool BusyIndicatorActive
        {
            get => busyIndicatorActive;
            set => SetProperty(ref busyIndicatorActive, value);
        }

        #endregion



        #region Function

        private void FullScreenTapped()
        {
            if (isFullScreenOff)
            {
                screenOrientServices.ForceLandscape();
            }
            else screenOrientServices.ForcePortrait();
            IsFullScreenOff = !isFullScreenOff;
        }
        #endregion
    }
}
