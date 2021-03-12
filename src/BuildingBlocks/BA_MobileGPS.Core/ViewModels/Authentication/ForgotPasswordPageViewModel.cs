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
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class ForgotPasswordPageViewModel : ViewModelBaseLogin
    {
        private readonly IAuthenticationService _authenticationService;
        public ICommand SendInfoCommand { get; }
        public ForgotPasswordPageViewModel(INavigationService navigationService, IAuthenticationService iAuthenticationService) : base(navigationService)
        {
            SendInfoCommand = new DelegateCommand(SendInfo);
            _authenticationService = iAuthenticationService;
        }
        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            InitValidation();
        }
        private ValidatableObject<string> accountName;
        public ValidatableObject<string> AccountName
        {
            get { return accountName; }
            set { SetProperty(ref accountName, value); }
        }

        private ValidatableObject<string> phoneNumber;
        public ValidatableObject<string> PhoneNumber
        {
            get { return phoneNumber; }
            set { SetProperty(ref phoneNumber, value); }
        }
        private void InitValidation()
        {
            accountName = new ValidatableObject<string>();
            accountName.OnChanged += StringValue_OnChanged;
            phoneNumber = new ValidatableObject<string>();
            phoneNumber.OnChanged += StringValue_OnChanged;

            accountName.Validations.Add(new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = MobileResource.ForgotPassword_Message_Validate_IsNull_AccountName
            });
            accountName.Validations.Add(new MaxLengthRule<string>
            {
                ValidationMessage = MobileResource.ForgotPassword_Message_Validate_MaxLength_AccountName,
                MaxLenght = 50
            });
            accountName.Validations.Add(new IsNotContainsHTMLTag<string>
            {
                ValidationMessage = MobileResource.ForgotPassword_Message_Validate_ConstantHTML_AccountName
            });
            accountName.Validations.Add(new ExpressionDangerousCharsRule<string>
            {
                Expression = MobileSettingHelper.ConfigDangerousCharTextBox,
                ValidationMessage = MobileResource.Common_Property_DangerousCharShow(MobileResource.ForgotPassword_Label_AccountName,
                                                                                      MobileSettingHelper.ConfigDangerousCharTextBox)
            });

            phoneNumber.Validations.Add(new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = MobileResource.ForgotPassword_Message_Validate_IsNull_Phone
            });
            phoneNumber.Validations.Add(new IsNotContainsHTMLTag<string>
            {
                ValidationMessage = MobileResource.ForgotPassword_Message_Validate_ConstantHTML_AccountName
            });
            phoneNumber.Validations.Add(new ExpressionDangerousCharsRule<string>
            {
                Expression = MobileSettingHelper.ConfigDangerousCharTextBox,
                ValidationMessage = MobileResource.Common_Property_DangerousCharShow(MobileResource.ForgotPassword_Label_Phone,
                                                                                     MobileSettingHelper.ConfigDangerousCharTextBox)
            });
            if (Settings.CurrentLanguage.Equals(CultureCountry.Vietnamese))
            {
                phoneNumber.Validations.Add(new PhoneNumberRule<string>
                {
                    ValidationMessage = MobileResource.ForgotPassword_Message_Validate_Rule_Phone,
                    CountryCode = CountryCodeConstant.VietNam
                });
            }
        }

        private void StringValue_OnChanged(object sender, string e)
        {

            var control = (ValidatableObject<string>)sender;
            control.IsNotValid = false;
        }

        private bool Validate()
        {
            var accValidate = accountName.Validate();
            var phoneValidate = phoneNumber.Validate();
            return accValidate && phoneValidate;
        }

        private async void SendInfo()
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
                        PhoneNumber.Value = StringHelper.TrimSpace(PhoneNumber.Value);
                        AccountName.Value = StringHelper.TrimSpace(AccountName.Value);
                        // gọi service check
                        var inputCheckInfor = new ForgotPasswordRequest
                        {
                            phoneNumber = PhoneNumber.Value,
                            userName = AccountName.Value
                        };

                        var responseCheckInfor = await _authenticationService.CheckUserExists(inputCheckInfor);
                        if (responseCheckInfor)
                        {
                            SendSMSCheckUser();
                        }
                        else
                        {
                            DisplayMessage.ShowMessageInfo(MobileResource.ForgotPassword_Message_ErrorCheckInforUser, 5000);
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



        private async void SendSMSCheckUser()
        {
            try
            {
                var inputSendCodeSMS = new ForgotPasswordRequest
                {
                    phoneNumber = PhoneNumber.Value,
                    userName = AccountName.Value,
                    AppID = (int)App.AppType
                };
                var responseSendCodeSMS = await _authenticationService.SendCodeSMS(inputSendCodeSMS);
                if ((int)responseSendCodeSMS.StateRegister == (int)StatusRegisterSMS.Success)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        var p = new NavigationParameters
                        {
                                { "Phonenumber", PhoneNumber.Value },
                                { "AccountName", AccountName.Value },
                                { "SecondCountDown", responseSendCodeSMS.SecondCountDown }
                        };
                        await NavigationService.NavigateAsync("VerifyCodeSMSPage", p, useModalNavigation: false, true);
                    });
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
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
        }
    }
}
