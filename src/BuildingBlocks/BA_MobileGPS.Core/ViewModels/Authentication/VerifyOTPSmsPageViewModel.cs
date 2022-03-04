using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.RequestEntity;
using BA_MobileGPS.Entities.ResponeEntity.OTP;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities.Enums;
using Com.OneSignal;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Timer = System.Timers.Timer;

namespace BA_MobileGPS.Core.ViewModels
{
    public class VerifyOTPSmsPageViewModel : ViewModelBaseLogin
    {
        private string timeRequest = string.Empty;

        public string TimeRequest
        {
            get { return timeRequest; }
            set { SetProperty(ref timeRequest, value); }
        }

        private LoginResponse _user = new LoginResponse();
        private bool _rememberme = false;
        private string _userName = string.Empty;
        private string _password = string.Empty;
        public string Numberphone = string.Empty;
        private Timer _timerCountDown;
        public int index { get; set; }
        private OtpResultResponse _objOtp;

        public SendCodeSMSResponse objsbsResponse { get; set; }
        public ValidatableObject<string> OtpSms { get; set; }

        public ICommand GetOTPAgainCommand { get; private set; }
        public ICommand PushMainPageCommand { get; private set; }
        private readonly IAuthenticationService _iAuthenticationService;

        public VerifyOTPSmsPageViewModel(INavigationService navigationService, IAuthenticationService iAuthenticationService) : base(navigationService)
        {
            GetOTPAgainCommand = new DelegateCommand(GetOPTAgain);
            PushMainPageCommand = new DelegateCommand(PushMainPage);
            InitValidations();
            _iAuthenticationService = iAuthenticationService;
            SetTimerCountDown();
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            if (parameters.ContainsKey("OTPZalo") && parameters.GetValue<OtpResultResponse>("OTPZalo") is OtpResultResponse objOtp)
            {
                _objOtp = objOtp;
                TimeRequest ="300";
                index = 300;
            }
            if (parameters.ContainsKey("OTPsms") && parameters.GetValue<SendCodeSMSResponse>("OTPsms") is SendCodeSMSResponse objOtpsms)
            {
                objsbsResponse = objOtpsms;
                TimeRequest = objOtpsms.SecondCountDown.ToString();
                index = objOtpsms.SecondCountDown;
            }
            if (parameters.ContainsKey("Numberphone") && parameters.GetValue<string>("Numberphone") is string numberphone)
            {
                Numberphone = numberphone;
            }
            if (parameters.ContainsKey("User") && parameters.GetValue<LoginResponse>("User") is LoginResponse user)
            {
                _user = user;
            }
            if (parameters.ContainsKey("Rememberme") && parameters.GetValue<bool>("Rememberme") is bool rememberme)
            {
                _rememberme = rememberme;
            }
            if (parameters.ContainsKey("UserName") && parameters.GetValue<string>("UserName") is string userName)
            {
                _userName = userName;
            }
            if (parameters.ContainsKey("Password") && parameters.GetValue<string>("Password") is string password)
            {
                _password = password;
            }
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

        private void InitValidations()
        {
            OtpSms = new ValidatableObject<string>();
            OtpSms.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "mã OTP không được để trống" });
        }

        private void SetTimerCountDown()
        {
            _timerCountDown = new Timer(1000);
            _timerCountDown.Elapsed += OnTimedEventCountDown;
            _timerCountDown.AutoReset = true;
            _timerCountDown.Enabled = true;
        }

        private void OnTimedEventCountDown(Object source, System.Timers.ElapsedEventArgs e)
        {
            TimeRequest = index.ToString();

            if (index == 0)
            {
                TimeRequest = index.ToString();
                _timerCountDown.Close();
            }
            if (index > 0)
            {
                index--;
            }
        }

        // lấy lại mã OTP
        private void GetOPTAgain()
        {  // Kiểm tra lấy lại mã otp
            SafeExecute(async () =>
            {
                if (index == 0)
                {
                    string customerID = String.Empty;
                    if (IsConnected)
                    {
                        if (_objOtp != null)
                        {
                            using (new HUDService(MobileResource.Common_Message_Processing))
                            {
                                _objOtp = await _iAuthenticationService.GetOTP(_user.PhoneNumber, customerID);
                                if (_objOtp != null && !string.IsNullOrEmpty(_objOtp.OTP))
                                {
                                    TimeRequest ="300";
                                    index = 300;
                                    DisplayMessage.ShowMessageSuccess("Đã gửi lại mã thành công!", 3000);
                                }
                                else
                                {
                                    DisplayMessage.ShowMessageSuccess("Thất bại! Kiểm tra lại đường truyền", 3000);
                                }
                            }
                        }
                        // Nếu không lấy lại mã sms
                        else
                        {
                            var inputSendCodeSMS = new ForgotPasswordRequest
                            {
                                phoneNumber = Numberphone,
                                userName = _user.UserName,
                                AppID = (int)App.AppType
                            };
                            using (new HUDService(MobileResource.Common_Message_Processing))
                            {
                                objsbsResponse = await _iAuthenticationService.SendCodeSMS(inputSendCodeSMS);
                                if (objsbsResponse!=null && (int)objsbsResponse.StateRegister == (int)StatusRegisterSMS.Success)
                                {
                                    DisplayMessage.ShowMessageSuccess("Đã gửi lại mã thành công!", 3000);
                                }
                                else
                                {
                                    switch ((int)objsbsResponse.StateRegister)
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
                    }
                    index = objsbsResponse.SecondCountDown;
                    SetTimerCountDown();
                }
                else
                {
                    DisplayMessage.ShowMessageInfo("Vui lòng kiểm tra lại, mã OTP đã được gửi!", 5000);
                }
            });
        }

        // check OTP
        private bool CheckVerifyOtpZalo()
        {
            if (!OtpSms.Validate())
            {
                return false;
            }
            if (_objOtp == null || string.IsNullOrEmpty(_objOtp.OTP))
            {
                DisplayMessage.ShowMessageInfo("Vui lòng kiểm tra lại mã xác thực OTP", 5000);
                return false;
            }
            if (DateTime.Now.Subtract(_objOtp.StartTimeOtp).TotalMinutes > 5)
            {
                DisplayMessage.ShowMessageInfo("Mã OTP đã hết hạn!", 5000);
                return false;
            }
            if (_objOtp.OTP != OtpSms.Value)
            {
                DisplayMessage.ShowMessageInfo("Mã OTP không chính xác!", 5000);
                return false;
            }
            return true;
        }

        private void PushMainPage()
        {
            if (IsConnected)
            {
                if (string.IsNullOrEmpty(OtpSms.Value))
                {
                    DisplayMessage.ShowMessageInfo("Mã OTP không được để trống", 5000);
                    return;
                }
                if (index == 0)
                {
                    DisplayMessage.ShowMessageInfo("Quá thời gian nhập mã xác thực", 5000);
                    return;
                }
                SafeExecute(async () =>
                {
                    if (IsConnected)
                    {
                        using (new HUDService(MobileResource.Common_Message_Processing))
                        {
                            // nếu mã OTP zalo
                            if (_objOtp != null)
                            {
                                if (!CheckVerifyOtpZalo())
                                {
                                    return;
                                }
                                RememberSettings();
                                await NavigationService.NavigateAsync("/MainPage");
                            }
                            // nếu mã Otp sms
                            else
                            {
                                if (!OtpSms.Validate())
                                {
                                    return;
                                }
                                VerifyOtpRequest inputVerifyCode = new VerifyOtpRequest()
                                {
                                    UserName = _user.UserName,
                                    XNcode = _user.XNCode,
                                    PhoneNumber = Numberphone,
                                    VehiclePlate = _user.VehiclePlateOTP,
                                    SecurityCode = OtpSms.Value,
                                    UserSecuritySMSLogID = objsbsResponse.SecurityCodeSMSLogID
                                };
                                // kiểm tra mã otp
                                var result = await _iAuthenticationService.CheckVehicleOtpsms(inputVerifyCode);
                                if (result==ResultVerifyOtp.Success)
                                {
                                    RememberSettings();
                                    await NavigationService.NavigateAsync("/MainPage");
                                }
                                else
                                {
                                    switch (result)
                                    {
                                        case ResultVerifyOtp.WrongPerSecurityCode:
                                            DisplayMessage.ShowMessageInfo(MobileResource.VerifyCodeMS_Message_ErrorOverWrongPerCode, 5000);
                                            break;

                                        case ResultVerifyOtp.WrongForUserInDay:
                                            DisplayMessage.ShowMessageInfo(MobileResource.VerifyCodeMS_Message_ErrorOverWrongPerDay, 5000);
                                            break;

                                        case ResultVerifyOtp.TimeOut:
                                            DisplayMessage.ShowMessageInfo(MobileResource.VerifyCodeMS_Message_ErrorTimeOut, 5000);
                                            break;

                                        case ResultVerifyOtp.InCorrect:
                                            DisplayMessage.ShowMessageInfo(MobileResource.VerifyCodeMS_Message_ErrorWrongVerifyCode, 5000);
                                            break;

                                        default:
                                            DisplayMessage.ShowMessageInfo(MobileResource.VerifyCodeMS_Message_ErrorVerifyCode, 5000);
                                            break;
                                    }
                                }
                            }
                        }
                    }
                });
            }
            else
            {
                DisplayMessage.ShowMessageInfo(MobileResource.Common_ConnectInternet_Error, 5000);
            }
        }

        private void RememberSettings()
        {
            //nếu nhớ mật khẩu thì lưu lại thông tin username và password
            if (_rememberme)
            {
                Settings.Rememberme = true;
            }
            else
            {
                Settings.Rememberme = false;
            }
            //Nếu đăng nhập tài khoản khác thì xóa CurrentCompany đi
            if (!string.IsNullOrEmpty(Settings.UserName) && Settings.UserName != _userName && Settings.CurrentCompany != null)
            {
                Settings.CurrentCompany = null;
            }

            Settings.UserName = _userName;
            Settings.Password = _password;

            StaticSettings.Token = _user.AccessToken;
            StaticSettings.User = _user;
            StaticSettings.SessionID = DeviceInfo.Model + "_" + DeviceInfo.Platform + "_" + Guid.NewGuid().ToString();
            OneSignal.Current.SendTag("UserID", _user.UserId.ToString().ToUpper());
            OneSignal.Current.SendTag("UserName", _user.UserName.ToString().ToUpper());
        }
    }
}