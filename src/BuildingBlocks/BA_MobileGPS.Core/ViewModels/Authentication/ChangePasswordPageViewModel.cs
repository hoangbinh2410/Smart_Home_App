using BA_MobileGPS.Core.Resource;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;

using Prism.Commands;
using Prism.Navigation;

using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace BA_MobileGPS.Core.ViewModels
{
    public class ChangePasswordPageViewModel : ViewModelBase
    {
        private readonly IAuthenticationService authenticationService;

        private ValidatableObject<string> oldPassword = new ValidatableObject<string>();
        public ValidatableObject<string> OldPassword { get => oldPassword; set => SetProperty(ref oldPassword, value); }

        private ValidatableObject<string> newPassword = new ValidatableObject<string>();
        public ValidatableObject<string> NewPassword { get => newPassword; set => SetProperty(ref newPassword, value); }

        private ValidatableObject<string> confirmNewPassword = new ValidatableObject<string>();
        public ValidatableObject<string> ConfirmNewPassword { get => confirmNewPassword; set => SetProperty(ref confirmNewPassword, value); }

        public ICommand ChangePasswordCommand { get; private set; }

        public ChangePasswordPageViewModel(INavigationService navigationService,
            IAuthenticationService authenticationService) : base(navigationService)
        {
            this.authenticationService = authenticationService;

            ChangePasswordCommand = new DelegateCommand(ChangePassword);

            OldPassword.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = MobileResource.ChangePassword_Validate_OldPasswordEmpty });
            NewPassword.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = MobileResource.ChangePassword_Validate_NewPasswordEmpty });
            NewPassword.Validations.Add(new ExpressionDangerousCharsRule<string> { Expression = "['\"<>/&]", ValidationMessage = MobileResource.Common_Property_DangerousChars(MobileResource.ChangePassword_Label_NewPassword) });
            NewPassword.Validations.Add(new MinLenghtRule<string> { MinLenght = 7, ValidationMessage = MobileResource.ChangePassword_Validate_NewPasswordMinLenght });
        }

        public override void OnPushed()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                DependencyService.Get<IApplication>().SetWindowSoftInputMode(WindowSoftInputModeAdjust.Pan);
            }
        }

        public override void OnDestroy()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                DependencyService.Get<IApplication>().SetWindowSoftInputMode(WindowSoftInputModeAdjust.Resize);
            }
        }

        private async Task<bool> Validate()
        {
            return OldPassword.Validate() && NewPassword.Validate() && ValidateOldPassword() && await ValidateNewPassword();
        }

        private bool ValidateOldPassword()
        {
            OldPassword.ErrorFirst = string.Empty;
            OldPassword.IsNotValid = false;

            if (OldPassword.Value != Settings.Password)
            {
                OldPassword.ErrorFirst = MobileResource.ChangePassword_Validate_OldPasswordInvalid;
                OldPassword.IsNotValid = true;
                return false;
            }

            return true;
        }

        private async Task<bool> ValidateNewPassword()
        {
            NewPassword.ErrorFirst = string.Empty;
            NewPassword.IsNotValid = false;

            ConfirmNewPassword.ErrorFirst = string.Empty;
            ConfirmNewPassword.IsNotValid = false;

            if (NewPassword.Value == Settings.Password)
            {
                NewPassword.ErrorFirst = MobileResource.ChangePassword_Validate_NewPasswordIsSameOldPassword;
                NewPassword.IsNotValid = true;
                return false;
            }

            if (NewPassword.Value == Settings.UserName)
            {
                NewPassword.ErrorFirst = MobileResource.ChangePassword_Validate_NewPasswordIsSameUserName;
                NewPassword.IsNotValid = true;
                return false;
            }

            if (ConfirmNewPassword.Value != NewPassword.Value)
            {
                ConfirmNewPassword.ErrorFirst = MobileResource.ChangePassword_Validate_NewPasswordConfirmInvalid;
                ConfirmNewPassword.IsNotValid = true;
                return false;
            }

            if (NewPassword.Value.Contains(" "))
            {
                if (!await PageDialog.DisplayAlertAsync("", MobileResource.ChangePassword_Message_NewPasswordHasSpace, MobileResource.Common_Button_Yes, MobileResource.Common_Button_No))
                {
                    return false;
                }
            }

            return true;
        }

        private async void ChangePassword()
        {
            if (!await Validate() || !IsConnected || IsBusy)
                return;

            IsBusy = true;
            DependencyService.Get<IHUDProvider>().DisplayProgress("");

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Task.Run(async () =>
            {
                var request = new ChangePassRequest
                {
                    UserName = Settings.UserName,
                    OldPassword = OldPassword.Value?.Trim(),
                    NewPassword = NewPassword.Value?.Trim()
                };

                return await authenticationService.ChangePassword(request);
            }).ContinueWith(task => Device.BeginInvokeOnMainThread(async () =>
            {
                IsBusy = false;
                DependencyService.Get<IHUDProvider>().Dismiss();

                if (task.Status == TaskStatus.RanToCompletion)
                {
                    if (task.Result)
                    {
                        await PageDialog.DisplayAlertAsync(MobileResource.Common_Label_BAGPS,
                          MobileResource.ChangePassword_Message_UpdateSuccess,
                          MobileResource.Common_Button_Close);

                        Settings.Password = string.Empty;
                        Settings.UserName = string.Empty;

                        Logout();
                    }
                }
                else if (task.IsFaulted)
                {
                    DisplayMessage.ShowMessageInfo(MobileResource.Common_Message_ErrorTryAgain);
                    Logger.WriteError(MethodBase.GetCurrentMethod().Name, task.Exception.Message);
                }
            }));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }
    }
}