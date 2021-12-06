using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace BA_MobileGPS.Core.ViewModels
{
    public class VerifyOTPSmsPageViewModel : ViewModelBaseLogin
    {
        #region Contructor
        public ICommand GetOTPAgainCommand { get; private set; }
        public ICommand PushMainPageCommand { get; private set; }
        private readonly IDisplayMessage _displayMessage;
        public VerifyOTPSmsPageViewModel(INavigationService navigationService, IDisplayMessage displayMessage) : base(navigationService)
        {
            GetOTPAgainCommand = new DelegateCommand(GetOPTAgain);
            PushMainPageCommand = new DelegateCommand(PushMainPage);
            _displayMessage = displayMessage;
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
            TimeOut();
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
        private int timeRequest = 300;
        public int TimeRequest { get { return timeRequest; } set { SetProperty(ref timeRequest, value); } }
        #endregion Property
        #region PrivateMethod
        private void TimeOut()
        {
            for (int i = 300; i > 0 ; i --)
            {
                TimeRequest = i;
            }
            if(TimeRequest == 0)
            {
                _displayMessage.ShowMessageInfo("Không được nhập ký tự đặc biệt");
            }
        }
        private void GetOPTAgain()
        {

        }
        private void PushMainPage()
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("NumberPhoneLoginPage");
            });
        }
        #endregion PrivateMethod
    }
}
