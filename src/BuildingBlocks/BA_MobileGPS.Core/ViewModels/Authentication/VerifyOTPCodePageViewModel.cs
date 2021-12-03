using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class VerifyOTPCodePageViewModel : ViewModelBaseLogin
    {
        #region Property

        #endregion Property

        #region Constructor

        public ICommand GetOTPAgain { get; private set; }
        public ICommand PushMainPageCommand { get; private set; }
        public VerifyOTPCodePageViewModel(INavigationService navigationService) : base(navigationService)
        {
            PushMainPageCommand = new DelegateCommand(PushMainPage);
            GetOTPAgain = new DelegateCommand(PushOTPAgain);
        }

        #endregion Constructor

        #region Lifecycle

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);         
        }

        public override void OnPageAppearingFirstTime()
        {
            base.OnPageAppearingFirstTime();
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
        }

        public override void OnDestroy()
        {
        }

        #endregion Lifecycle

        

        #region PrivateMethod 
        // Xác thực OTP
        private void PushMainPage()
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("NavigationPage/QRCodeLogin");
            });
        }
        private void PushOTPAgain()
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("NavigationPage/QRCodeLogin");
            });
        }
        #endregion PrivateMethod
    }
}