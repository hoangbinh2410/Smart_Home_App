using Prism.Commands;
using Prism.Navigation;
using System.Windows.Input;

namespace BA_MobileGPS.Core.ViewModels
{
    public class LoginFailedPopupViewModel : ViewModelBaseLogin
    {
        public ICommand ForgotPasswordCommand { get; }

        public ICommand ForgotUserNameCommand { get; }

        public LoginFailedPopupViewModel(INavigationService navigationService) : base(navigationService)
        {
            ForgotPasswordCommand = new DelegateCommand(ForgotPassword);
            ForgotUserNameCommand = new DelegateCommand(ForgotUserName);
        }

        private void ForgotUserName()
        {
            SafeExecute(async () =>
            {
                await NavigationService.GoBackAsync(useModalNavigation: true, animated: true, parameters: new NavigationParameters
                        {
                            { "LoginFailedPopup",  false}
                        });
            });
        }

        private void ForgotPassword()
        {
            SafeExecute(async () =>
            {
                await NavigationService.GoBackAsync(useModalNavigation: true, animated: true, parameters: new NavigationParameters
                        {
                            { "LoginFailedPopup",  true}
                        });
            });
        }
    }
}