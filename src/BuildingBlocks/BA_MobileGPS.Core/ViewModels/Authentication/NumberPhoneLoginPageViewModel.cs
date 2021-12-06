using Prism.Commands;
using Prism.Navigation;
using System.Windows.Input;

namespace BA_MobileGPS.Core.ViewModels
{
    public class NumberPhoneLoginPageViewModel : ViewModelBaseLogin
    {
        #region Contructor
         public ICommand PushOTPSMSPageCommand { get; private set; }
        public ICommand PushMainPageCommand { get; private set; }
        public NumberPhoneLoginPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            PushOTPSMSPageCommand = new DelegateCommand(PushOTPSMSPage);          
        }

        #endregion Contructor

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
        private void PushOTPSMSPage()
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("VerifyOTPSmsPage");
            });
        }
        #endregion PrivateMethod
    }
}