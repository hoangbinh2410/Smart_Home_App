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
    public class QRCodeLoginViewModel :ViewModelBaseLogin

    {
        #region Constructor
        public ICommand PushtoLanguageCommand { get; private set; }
        public ICommand PushOTPPageCommand { get; private set; }
        public ICommand PushZaloPageCommand { get; private set; }
        public QRCodeLoginViewModel(INavigationService navigationService) : base(navigationService)
    {
            PushOTPPageCommand = new DelegateCommand(PushOTPPage);
            PushZaloPageCommand = new DelegateCommand(PushZaloPage);
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
        #region Property
        #endregion Property

        #region PrivateMethod    
        private void PushOTPPage()
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("VerifyOTPCodePage");
            });
        }
        private void PushZaloPage()
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("VerifyOTPCodePage");
            });
        }

        # endregion PrivateMethod
    }
}