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
            AccountName = new ValidatableObject<string>();
            AccountName.OnChanged += StringValue_OnChanged;
            PhoneNumber = new ValidatableObject<string>();
            PhoneNumber.OnChanged += StringValue_OnChanged;

            AccountName.Validations.Add(new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = MobileResource.ForgotPassword_Message_Validate_IsNull_AccountName
            });
            AccountName.Validations.Add(new MaxLengthRule<string>
            {
                ValidationMessage = MobileResource.ForgotPassword_Message_Validate_MaxLength_AccountName,
                MaxLenght = 50
            });
            AccountName.Validations.Add(new IsNotContainsHTMLTag<string>
            {
                ValidationMessage = MobileResource.ForgotPassword_Message_Validate_ConstantHTML_AccountName
            });
            AccountName.Validations.Add(new ExpressionDangerousCharsRule<string>
            {
                Expression = MobileSettingHelper.ConfigDangerousCharTextBox,
                ValidationMessage = MobileResource.Common_Property_DangerousCharShow(MobileResource.ForgotPassword_Label_AccountName,
                                                                                      MobileSettingHelper.ConfigDangerousCharTextBox)
            });

            PhoneNumber.Validations.Add(new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = MobileResource.ForgotPassword_Message_Validate_IsNull_Phone
            });
            PhoneNumber.Validations.Add(new IsNotContainsHTMLTag<string>
            {
                ValidationMessage = MobileResource.ForgotPassword_Message_Validate_ConstantHTML_AccountName
            });
            PhoneNumber.Validations.Add(new ExpressionDangerousCharsRule<string>
            {
                Expression = MobileSettingHelper.ConfigDangerousCharTextBox,
                ValidationMessage = MobileResource.Common_Property_DangerousCharShow(MobileResource.ForgotPassword_Label_Phone,
                                                                                     MobileSettingHelper.ConfigDangerousCharTextBox)
            });
            if (Settings.CurrentLanguage.Equals(CultureCountry.Vietnamese))
            {
                PhoneNumber.Validations.Add(new PhoneNumberRule<string>
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
            var accValidate = AccountName.Validate();
            var phoneValidate = PhoneNumber.Validate();
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
                            PhoneNumber = PhoneNumber.Value,
                            UserName = AccountName.Value
                        };

                        var responseCheckInfor = await _authenticationService.CheckUserExists(inputCheckInfor);
                        if (responseCheckInfor)
                        {
                            SendSMSCheckUser();
                        }
                        else
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                var actioncall = await PageDialog.DisplayAlertAsync("Thông báo",
                                       string.Format("Tài khoản và số điện thoại phải nhập đúng với tài khoản và số điện thoại khi tạo. Vui lòng kiểm tra lại hoặc gọi điện lên số {0} để hỗ trợ", MobileSettingHelper.HotlineGps),
                                       "Liên hệ", "Bỏ qua");
                                if (actioncall)
                                {
                                    if (!string.IsNullOrEmpty(MobileSettingHelper.HotlineGps))
                                    {
                                        PhoneDialer.Open(MobileSettingHelper.HotlineGps);
                                    }
                                }
                            });
                            //DisplayMessage.ShowMessageInfo(MobileResource.ForgotPassword_Message_ErrorCheckInforUser, 5000);
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
                    PhoneNumber = PhoneNumber.Value,
                    UserName = AccountName.Value,
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