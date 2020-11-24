using BA_MobileGPS.Core.Interfaces;
using BA_MobileGPS.Core.ViewModels.Base;
using BA_MobileGPS.Service.IService;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Core.ViewModels
{
    public class RestreamChildVMBase : TabbedPageChildVMBase
    {
        protected readonly IStreamCameraService streamCameraService;
        protected readonly IScreenOrientServices screenOrientServices;

        public ICommand FullScreenTappedCommand { get; }
        public RestreamChildVMBase(INavigationService navigationService, 
            IStreamCameraService cameraService,
            IScreenOrientServices screenOrientServices) : base(navigationService)
        {
            streamCameraService = cameraService;
            this.screenOrientServices = screenOrientServices;
            FullScreenTappedCommand = new DelegateCommand(FullScreenTapped);
        }

        #region Property
        protected int pageIndex { get; set; } = 0;
        protected int pageCount { get; } = 10; // so luong anh cho 1 lan infinite scroll 

        private bool isFullScreenOff;

        public bool IsFullScreenOff
        {
            get => isFullScreenOff; set => SetProperty(ref isFullScreenOff, value);
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
