using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Enums;

using Prism.Commands;
using Prism.Navigation;

using System;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class VerifyCodeSMSPageViewModel : ViewModelBaseLogin
    {
    
        private readonly IAuthenticationService _iAuthenticationService;

        public VerifyCodeSMSPageViewModel(INavigationService navigationService,
             IAuthenticationService iAuthenticationService) : base(navigationService)
        {
            try
            {
                Title = MobileResource.VerifyCodeMS_Label_TilePage.ToUpper();
                _code = new ValidatableObject<string>();
                this._iAuthenticationService = iAuthenticationService;
 
                //SecondCountDown = 200;

                VerifyCodeCommand = new DelegateCommand(ExcuteVerifyCode);
                ResendCodeCommand = new DelegateCommand(ExcuteResendCode);
                IsVisibleButtonResend = false;
                IsVisibleButtonVerifyCode = true;
                AddValidations();
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            //get a collection of typed parameters
            if (parameters?.GetValue<string>("Phonenumber") is string phoneNumber && parameters?.GetValue<string>("AccountName") is string account && parameters?.GetValue<int>("SecondCountDown") is int coundown)
            {
                PhoneNumber = phoneNumber;
                AccountName = account;
                SecondCountDown = coundown;
                SetValueTimeCountDown();
            }
        }

        #region property
        private string title;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }
        private ValidatableObject<string> _code;

        public ValidatableObject<string> Code
        {
            get { return _code; }
            set { _code = value; }
        }

        private string _timeNotice;

        public string TimeNotice
        {
            get { return _timeNotice; }
            set { SetProperty(ref _timeNotice, value); }
        }

        public string PhoneNumber { get; set; }

        public string AccountName { get; set; }
        public int SecondCountDown { get; set; }

        private bool _isVisibleButtonResend;

        public bool IsVisibleButtonResend
        {
            get { return _isVisibleButtonResend; }
            set { SetProperty(ref _isVisibleButtonResend, value); }
        }

        private bool _isVisibleButtonVerifyCode;

        public bool IsVisibleButtonVerifyCode
        {
            get { return _isVisibleButtonVerifyCode; }
            set { SetProperty(ref _isVisibleButtonVerifyCode, value); }
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

        #endregion property

        #region command

        public ICommand VerifyCodeCommand { get;}

        public ICommand ResendCodeCommand { get;}

        #endregion command

        #region excutecommand

        private void AddValidations()
        {
            _code.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = MobileResource.VerifyCodeMS_Message_Validate_IsNull_Code });
            _code.Validations.Add(new MaxLengthRule<string> { ValidationMessage = MobileResource.VerifyCodeMS_Message_Validate_MaxLength_Code, MaxLenght = MobileSettingHelper.NumberOfDigitVerifyCode });
            _code.Validations.Add(new ExpressionDangerousCharsRule<string> { Expression = MobileSettingHelper.ConfigDangerousCharTextBox, ValidationMessage = MobileResource.Common_Property_DangerousCharShow(MobileResource.VerifyCodeMS_Label_Code, MobileSettingHelper.ConfigDangerousCharTextBox) });
        }

        private bool Validate()
        {
            return _code.Validate();
        }

        private async void ExcuteVerifyCode()
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
                        var inputVerifyCode = new VerifyCodeRequest
                        {
                            PhoneNumber = PhoneNumber,
                            VerifyCode = Code.Value,
                            AppID = (int)App.AppType
                        };
                        var responseSendCodeSMS = await _iAuthenticationService.CheckVerifyCode(inputVerifyCode);
                        if (responseSendCodeSMS == StateVerifyCode.Success)
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                var p = new NavigationParameters
                                {
                                    { "AccountName", AccountName }
                                };
                                await NavigationService.NavigateAsync("ChangePasswordForgotPage", p, useModalNavigation: false,true);
                            });
                        }
                        else
                        {
                            switch (responseSendCodeSMS)
                            {
                                case StateVerifyCode.OverWrongPerCode:
                                    DisplayMessage.ShowMessageInfo(MobileResource.VerifyCodeMS_Message_ErrorOverWrongPerCode, 5000);
                                    break;

                                case StateVerifyCode.OverWrongPerDay:
                                    DisplayMessage.ShowMessageInfo(MobileResource.VerifyCodeMS_Message_ErrorOverWrongPerDay, 5000);
                                    break;

                                case StateVerifyCode.TimeOut:
                                    DisplayMessage.ShowMessageInfo(MobileResource.VerifyCodeMS_Message_ErrorTimeOut, 5000);
                                    break;

                                case StateVerifyCode.WrongVerifyCode:
                                    DisplayMessage.ShowMessageInfo(MobileResource.VerifyCodeMS_Message_ErrorWrongVerifyCode, 5000);
                                    break;

                                default:
                                    DisplayMessage.ShowMessageInfo(MobileResource.VerifyCodeMS_Message_ErrorVerifyCode, 5000);
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    DisplayMessage.ShowMessageInfo(MobileResource.Common_ConnectInternet_Error, 5000);
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void StartCountdownAsync()
        {
            Device.StartTimer(TimeSpan.FromSeconds(1), SetTimeNotice);
        }

        private bool SetTimeNotice()
        {
            if (SecondCountDown < 0)
            {
                IsVisibleButtonResend = true;
                IsVisibleButtonVerifyCode = false;
                return false;
            }
            else
            {
                IsVisibleButtonResend = false;
                IsVisibleButtonVerifyCode = true;
                TimeNotice = string.Format(MobileResource.VerifyCodeMS_Label_TimeCountDown, SecondCountDown);
                SecondCountDown -= 1;
                return true;
            }
        }

        private void SetValueTimeCountDown()
        {
            if (SecondCountDown <= 0)
            {
                SecondCountDown = MobileSettingHelper.AuthenticationSecond;
            }
            TimeNotice = string.Format(MobileResource.VerifyCodeMS_Label_TimeCountDown, SecondCountDown);
            IsVisibleButtonResend = false;
            IsVisibleButtonVerifyCode = true;
            StartCountdownAsync();
        }

        private void CallHotline()
        {
            if (!string.IsNullOrEmpty(MobileSettingHelper.HotlineGps))
            {
                PhoneDialer.Open(MobileSettingHelper.HotlineGps);
            }
        }

        private async void ExcuteResendCode()
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
                    if (SecondCountDown <= 0)
                    {
                        var inputSendCodeSMS = new ForgotPasswordRequest
                        {
                            PhoneNumber = PhoneNumber,
                            UserName = AccountName,
                            AppID = (int)App.AppType
                        };
                        var responseSendCodeSMS = await _iAuthenticationService.SendCodeSMS(inputSendCodeSMS);
                        if ((int)responseSendCodeSMS.StateRegister == (int)StatusRegisterSMS.Success)
                        {
                            DisplayMessage.ShowMessageInfo(MobileResource.ForgotPassword_Message_SuccessSendSMS, 5000);
                            SecondCountDown = responseSendCodeSMS.SecondCountDown;
                            SetValueTimeCountDown();
                        }
                        else
                        {
                            switch ((int)responseSendCodeSMS.StateRegister)
                            {
                                case (int)StatusRegisterSMS.ErrorLogSMS:
                                    DisplayMessage.ShowMessageInfo(MobileResource.ForgotPassword_Message_ErrorErrorLogSMS, 5000);
                                    break;

                                case (int)StatusRegisterSMS.ErrorSendSMS:
                                    DisplayMessage.ShowMessageInfo(MobileResource.ForgotPassword_Message_ErrorErrorSendSMS, 5000);
                                    break;

                                case (int)StatusRegisterSMS.OverCountOneDay:
                                    DisplayMessage.ShowMessageInfo(MobileResource.ForgotPassword_Message_ErrorOverCountOneDay, 5000);
                                    break;

                                case (int)StatusRegisterSMS.WasRegisterSuccess:
                                    DisplayMessage.ShowMessageInfo(MobileResource.ForgotPassword_Message_ErrorWasRegisterSuccess, 5000);
                                    break;

                                default:
                                    DisplayMessage.ShowMessageInfo(MobileResource.ForgotPassword_Message_ErrorSendSMS, 5000);
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    DisplayMessage.ShowMessageInfo(MobileResource.Common_ConnectInternet_Error, 5000);
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                IsBusy = false;
            }
        }

        #endregion excutecommand
    }
}
