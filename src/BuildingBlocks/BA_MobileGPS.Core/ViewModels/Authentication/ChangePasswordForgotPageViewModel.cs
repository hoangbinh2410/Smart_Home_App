using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class ChangePasswordForgotPageViewModel : ViewModelBaseLogin
    {
        private readonly IAuthenticationService _iAuthenticationService;

        public ChangePasswordForgotPageViewModel(INavigationService navigationService,
              IAuthenticationService iAuthenticationService) : base(navigationService)
        {
            Title = MobileResource.ChangePassForgot_Label_TilePage.ToUpper();
            this._iAuthenticationService = iAuthenticationService;
            ChangePasswordCommand = new DelegateCommand(ExcuteChangePassword);
            CheckEnableButtonCommmand = new DelegateCommand(() => CanCheckEnableButtonCommmand());
            _passwordNew = new ValidatableObject<string>();
            _passwordReply = new ValidatableObject<string>();
            AddValidations_passwordNew();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            //get a collection of typed parameters
            AccountName = parameters["AccountName"] as string;
        }

        #region property
        private string title;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }
        public string AccountName { get; set; }
        public int SecondCountDown { get; set; }

        private ValidatableObject<string> _passwordNew;

        public ValidatableObject<string> PasswordNew
        {
            get { return _passwordNew; }
            set
            {
                SetProperty(ref _passwordNew, value);
            }
        }

        private ValidatableObject<string> _passwordReply;

        public ValidatableObject<string> PasswordReply
        {
            get { return _passwordReply; }
            set { SetProperty(ref _passwordReply, value); }
        }

        private string _hotline = MobileSettingHelper.HotlineGps;

        public string Hotline
        {
            get
            {
                return _hotline;
            }
            set
            {
                _hotline = value;
                RaisePropertyChanged(() => Hotline);
            }
        }

        private bool _isEnableChangePass = true;

        public bool IsEnableChangePass
        {
            get { return _isEnableChangePass; }
            set { SetProperty(ref _isEnableChangePass, value); }
        }

        #endregion property

        #region Command

        public DelegateCommand ChangePasswordCommand { get; private set; }
        public DelegateCommand CheckEnableButtonCommmand { get; private set; }

        #endregion Command

        #region ExcuteCommand

        private async void ExcuteChangePassword()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;
            try
            {
                if (IsConnected)
                {
                    if (Validate())
                    {
                        var inputChangePass = new ChangePasswordForgotRequest
                        {
                            UserName = AccountName,
                            NewPassword = PasswordNew.Value
                        };
                        var responseChangePass = await _iAuthenticationService.ChangePassWordForget(inputChangePass);
                        if (responseChangePass)
                        {
                            StartCountdownAsync();
                            await ShowMessageSuccess(MobileResource.ChangePassForgot_Message_SuccessChangePassword, MobileResource.Common_Label_BAGPS, MobileResource.ChangePassForgot_Button_ClosePopup,
                            async () =>
                            {
                                //await NavigationService.NavigateAsync("/LoginPage", useModalNavigation: true);
                                await NavigationService.GoBackAsync(null,useModalNavigation: true,true);
                            });
                        }
                        else
                        {
                            DisplayMessage.ShowMessageInfo(MobileResource.ChangePassForgot_Message_ErrorChangePassword, 5000);
                        }
                    }
                }
                else
                {
                    DisplayMessage.ShowMessageInfo(MobileResource.Common_ConnectInternet_Error, 5000);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void StartCountdownAsync()
        {
            SecondCountDown = 3;
            Device.StartTimer(TimeSpan.FromSeconds(1), CheckTimeGoBack);
        }

        private bool CheckTimeGoBack()
        {
            if (SecondCountDown < 0)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await NavigationService.NavigateAsync("/LoginPage",null, useModalNavigation: true,true);
                });
                return false;
            }
            SecondCountDown -= 1;
            return true;
        }

        private void AddValidations_passwordNew()
        {
            _passwordNew.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = MobileResource.ChangePassForgot_Message_Validate_IsNull_NewPassword });
            _passwordNew.Validations.Add(new MinLenghtRule<string> { ValidationMessage = MobileResource.ChangePassForgot_Message_Validate_MinLength_NewPassword, MinLenght = 7 });
            _passwordNew.Validations.Add(new MaxLengthRule<string> { ValidationMessage = MobileResource.ChangePassForgot_Message_Validate_MaxLength_NewPassword, MaxLenght = 250 });
            _passwordNew.Validations.Add(new ExpressionDangerousCharsRule<string> { Expression = MobileSettingHelper.ConfigDangerousCharTextBox, ValidationMessage = MobileResource.Common_Property_DangerousCharShow(MobileResource.ChangePassword_Label_NewPassword, MobileSettingHelper.ConfigDangerousCharTextBox) });
        }

        private void AddValidations_passwordReply()
        {
            _passwordReply.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = MobileResource.ChangePassForgot_Message_Validate_IsNull_ReplyPassword });
            _passwordReply.Validations.Add(new RepeatPasswordRule<string> { ValidationMessage = MobileResource.ChangePassForgot_Message_Validate_Equal_ReplyPassword, Password = PasswordNew.Value });
            _passwordReply.Validations.Add(new ExpressionDangerousCharsRule<string> { Expression = MobileSettingHelper.ConfigDangerousCharTextBox, ValidationMessage = MobileResource.Common_Property_DangerousCharShow(MobileResource.ChangePassForgot_Label_ReplyPassword, MobileSettingHelper.ConfigDangerousCharTextBox) });
        }

        private bool Validate()
        {
            if (_passwordNew.Validate())
            {
                _passwordReply.Validations.Clear();
                AddValidations_passwordReply();
                return _passwordReply.Validate();
            }
            return false;
        }

        public async Task ShowMessageSuccess(string message,
          string title,
          string buttonText,
          Action afterHideCallback)
        {
            await PageDialog.DisplayAlertAsync(title, message, buttonText);
            afterHideCallback?.Invoke();
        }

        private void CanCheckEnableButtonCommmand()
        {
            IsEnableChangePass = !string.IsNullOrWhiteSpace(PasswordNew.Value) && !string.IsNullOrWhiteSpace(PasswordReply.Value);
        }

        #endregion ExcuteCommand
    }
}
