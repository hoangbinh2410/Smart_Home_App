using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
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
        #region Property

        private string timeRequest = "300";
        public string TimeRequest
        { 
            get { return timeRequest; } 
            set { SetProperty(ref timeRequest, value); } 
        }

        private LoginResponse _user = new LoginResponse();
        private bool _rememberme = false;
        private string _userName = string.Empty;
        private string _password = string.Empty;

        private static Timer _timerCountDown;
        public int index = 300;
        private OtpResultResponse _objOtp;

        public ValidatableObject<string> OtpSms { get; set; }

        #endregion Property

        #region Contructor

        public ICommand GetOTPAgainCommand { get; private set; }
        public ICommand PushMainPageCommand { get; private set; }
        private readonly IAuthenticationService _iAuthenticationService;

        public VerifyOTPSmsPageViewModel(INavigationService navigationService, IAuthenticationService iAuthenticationService) : base(navigationService)
        {
            GetOTPAgainCommand = new DelegateCommand(GetOPTAgain);
            PushMainPageCommand = new DelegateCommand(PushMainPage);
            InitValidations();
            _iAuthenticationService = iAuthenticationService;
        }

        #endregion Contructor

        #region Lifecycle

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            if (parameters.ContainsKey("OTPZalo") && parameters.GetValue<OtpResultResponse>("OTPZalo") is OtpResultResponse objOtp)
            {
                _objOtp = objOtp;
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
            SetTimerCountDown();
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


        #region PrivateMethod

        private void InitValidations()
        {
            OtpSms = new ValidatableObject<string>();
            OtpSms.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "mã OTP không được để trống" });
        }

        private void SetTimerCountDown()
        {
            _timerCountDown = new Timer();
            _timerCountDown.AutoReset = true;
            _timerCountDown.Enabled = true;
            _timerCountDown.Elapsed += OnTimedEventCountDown;
            _timerCountDown.Interval = 1000;
            _timerCountDown.Start();
        }

        private void OnTimedEventCountDown(Object source, System.Timers.ElapsedEventArgs e)
        {
            TimeRequest = index.ToString();

            if (index == 0)
            {
                TimeRequest = index.ToString();
                _timerCountDown.Close();
            }

            index--;
        }

        // lấy lại mã OTP
        private void GetOPTAgain()
        {  // Kiểm tra lấy lại mã otp
            SafeExecute(async () =>
            {
                if(index == 0)
                {
                    string customerID = String.Empty;
                    if (IsConnected)
                    {
                        if (_objOtp != null)
                        {
                            using (new HUDService(MobileResource.Common_Message_Processing))
                            {
                                _objOtp = await _iAuthenticationService.GetOTP(_user.PhoneNumber, customerID);
                            }
                        }
                        // Nếu không lấy lại mã sms
                        else
                        {
                            var inputSendCodeSMS = new ForgotPasswordRequest
                            {
                                phoneNumber = _user.PhoneNumber,
                                userName = _user.UserName,
                                AppID = (int)App.AppType
                            };
                            using (new HUDService(MobileResource.Common_Message_Processing))
                            {
                                var objsbsResponse = await _iAuthenticationService.SendCodeSMS(inputSendCodeSMS);
                            }
                        }
                    }
                    index = 300;
                    _timerCountDown.Close();
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
                SafeExecute(async () =>
                {
                    if (IsConnected)
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
                            var inputVerifyCode = new VerifyCodeRequest
                            {
                                phoneNumber = _user.PhoneNumber,
                                verifyCode = OtpSms.Value,
                                AppID = (int)App.AppType
                            };
                            // kiểm tra mã otp
                            CheckVerifyCodeResponse responseSendCodeSMS = new CheckVerifyCodeResponse();
                            responseSendCodeSMS = await _iAuthenticationService.CheckVerifyCode(inputVerifyCode);
                            if ((int)responseSendCodeSMS.StateVerifyCode == (int)StateVerifyCode.Success)
                            {
                                RememberSettings();
                                await NavigationService.NavigateAsync("/MainPage");
                            }
                            else
                            {
                                switch ((int)responseSendCodeSMS.StateVerifyCode)
                                {
                                    case (int)StateVerifyCode.OverWrongPerCode:
                                        DisplayMessage.ShowMessageInfo(MobileResource.VerifyCodeMS_Message_ErrorOverWrongPerCode, 5000);
                                        break;

                                    case (int)StateVerifyCode.OverWrongPerDay:
                                        DisplayMessage.ShowMessageInfo(MobileResource.VerifyCodeMS_Message_ErrorOverWrongPerDay, 5000);
                                        break;

                                    case (int)StateVerifyCode.TimeOut:
                                        DisplayMessage.ShowMessageInfo(MobileResource.VerifyCodeMS_Message_ErrorTimeOut, 5000);
                                        break;

                                    case (int)StateVerifyCode.WrongVerifyCode:
                                        DisplayMessage.ShowMessageInfo(MobileResource.VerifyCodeMS_Message_ErrorWrongVerifyCode, 5000);
                                        break;

                                    default:
                                        DisplayMessage.ShowMessageInfo(MobileResource.VerifyCodeMS_Message_ErrorVerifyCode, 5000);
                                        break;
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

        #endregion PrivateMethod
    }
}