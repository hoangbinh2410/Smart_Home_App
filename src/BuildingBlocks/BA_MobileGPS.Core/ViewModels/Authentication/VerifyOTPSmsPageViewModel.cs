using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.ResponeEntity.OTP;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities.Enums;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Windows.Input;
using Timer = System.Timers.Timer;

namespace BA_MobileGPS.Core.ViewModels
{
    public class VerifyOTPSmsPageViewModel : ViewModelBaseLogin
    {
        #region Contructor

        public ICommand GetOTPAgainCommand { get; private set; }
        public ICommand PushMainPageCommand { get; private set; }
        private readonly IDisplayMessage _displayMessage;
        private readonly IAuthenticationService _iAuthenticationService;

        public VerifyOTPSmsPageViewModel(INavigationService navigationService, IDisplayMessage displayMessage, IAuthenticationService iAuthenticationService) : base(navigationService)
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
            if (parameters.ContainsKey("Numberphone") && parameters.GetValue<string>("Numberphone") is string Numberphone)
            {
                NumberPhone = Numberphone;
                if (parameters.ContainsKey("OTPZalo") && parameters.GetValue<OtpResultResponse>("OTPZalo") is OtpResultResponse objOtp)
                {
                    _objOtp = objOtp;
                }
                SetTimerCountDown();
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

        #endregion Lifecycle

        #region Property

        private static Timer _timerCountDown;
        private string timeRequest = String.Empty;
        public string NumberPhone { get; set; }
        public int index = 300;
        private OtpResultResponse _objOtp;
        private LoginResponse userInfo;
        public LoginResponse UserInfo
        { get { if (StaticSettings.User != null) { UserInfo = StaticSettings.User; } return userInfo; } set => SetProperty(ref userInfo, value); }

        public string TimeRequest
        { get { return timeRequest; } set { SetProperty(ref timeRequest, value); } }

        //private ValidatableObject<string> _code;
        public ValidatableObject<string> Code { get; set; }     

        #endregion Property

        #region PrivateMethod

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
            index--;
            if (index == 0)
            {
                TimeRequest = index.ToString();
                _timerCountDown.Close();
                index = 300;
                return;
            }
            TimeRequest = index.ToString();
        }

        // lấy lại mã OTP
        private void GetOPTAgain()
        {  // Kiểm tra lấy lại mã otp
            SafeExecute(async () =>
            {
                string customerID = String.Empty;
                if (IsConnected)
                {
                    if (_objOtp != null)
                    {
                        using (new HUDService(MobileResource.Common_Message_Processing))
                        {
                          var  objResponse = await _iAuthenticationService.GetOTP(NumberPhone, customerID);
                        }
                    }
                    // Nếu không lấy lại mã sms
                    else
                    {
                        var inputSendCodeSMS = new ForgotPasswordRequest
                        {
                            phoneNumber = NumberPhone,
                            userName = UserInfo.UserName,
                            AppID = (int)App.AppType
                        };
                        using (new HUDService(MobileResource.Common_Message_Processing))
                        {
                            var objsbsResponse = await _iAuthenticationService.SendCodeSMS(inputSendCodeSMS);
                        }
                    }                
                }        
            });
            SetTimerCountDown();
        }
        // check OTP
        private bool CheckVerifyOtp()
        {           
            if (!Code.Validate())
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
            if (_objOtp.OTP != Code.Value)
            {
                DisplayMessage.ShowMessageInfo("Mã OTP không chính xác!", 5000);
                return false;
            }
            return true;
        }
        //private bool Validate()
        //{
        //    return _code.Validate();
        //}

        // KIểm tra OTP
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
                        if (!CheckVerifyOtp())
                        {
                            return;
                        }
                        await NavigationService.NavigateAsync("/MainPage");
                    }
                    // Kiểm tra nã Otp sms
                    else /*if (Validate())*/
                    {
                        var inputVerifyCode = new VerifyCodeRequest
                        {
                            phoneNumber = "0971545577",
                            verifyCode = Code.Value,
                            AppID = (int)App.AppType
                        };
                        // kiểm tra mã otp
                        var responseSendCodeSMS = await _iAuthenticationService.CheckVerifyCode(inputVerifyCode);
                        if ((int)responseSendCodeSMS.StateVerifyCode == (int)StateVerifyCode.Success)
                        {
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

        #endregion PrivateMethod
    }
}