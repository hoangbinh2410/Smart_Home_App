﻿using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.ResponeEntity.OTP;
using Com.OneSignal;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Timer = System.Timers.Timer;

namespace BA_MobileGPS.Core.ViewModels
{
    public class VerifyOTPCodePageViewModel : ViewModelBaseLogin
    {
        #region Property

        private string _textGetOTPAgain = string.Empty;
        public string TextGetOTPAgain
        {
            get => _textGetOTPAgain;
            set => SetProperty(ref _textGetOTPAgain, value);
        }

        private LoginResponse _user = new LoginResponse();
        private bool _rememberme = false;
        private string _userName = string.Empty;
        private string _password = string.Empty;

        public ValidatableObject<string> OtpValue { get; set; }
        private bool _isGetOTPAgain;
        private OtpResultResponse _objOtp;
        private static Timer _timerGetOTPAgain;
        private static Timer _timerCountDown;
        private int index = 60;

        #endregion Property

        #region Constructor

        public ICommand GetOTPAgain { get; private set; }
        public ICommand PushMainPageCommand { get; private set; }
        public VerifyOTPCodePageViewModel(INavigationService navigationService) : base(navigationService)
        {
            PushMainPageCommand = new DelegateCommand(PushMainPage);
            GetOTPAgain = new DelegateCommand(PushOTPAgain);
            InitValidations();
        }

        #endregion Constructor

        #region Lifecycle

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            if (parameters != null)
            {
                if (parameters.ContainsKey("OTPZalo") && parameters.GetValue<OtpResultResponse>("OTPZalo") is OtpResultResponse objOtp)
                {
                    _isGetOTPAgain = false;
                    SetTimerGetOTPAgain();
                    SetTimerCountDown();
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



        #region PrivateMethod 

        private void InitValidations()
        {
            OtpValue = new ValidatableObject<string>();
            OtpValue.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "mã OTP không được để trống" });
        }

        //Check xác thực OTP
        private bool CheckVerifyOtp()
        {
            if (!OtpValue.Validate())
            {
                return false;
            }
            if (_objOtp == null || string.IsNullOrEmpty(_objOtp.OTP))
            {
                DisplayMessage.ShowMessageInfo("Vui lòng kiểm tra lại mã xác thực OTP", 5000);
                return false;
            }
            if (DateTime.Now.Subtract(_objOtp.StartTimeOtp).TotalMinutes > 30)
            {
                DisplayMessage.ShowMessageInfo("Mã OTP đã hết hạn!", 5000);
                return false;
            }
            if (_objOtp.OTP != OtpValue.Value)
            {
                DisplayMessage.ShowMessageInfo("Mã OTP không chính xác!", 5000);
                return false;
            }
            return true;
        }

        // Xác thực OTP
        private void PushMainPage()
        {

            if(!CheckVerifyOtp())
            {
                return;
            }
            //nếu nhớ mật khẩu thì lưu lại thông tin username và password
            if (_rememberme)
            {
                Settings.Rememberme = true;
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

            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("/MainPage");
            });
        }
        private void SetTimerGetOTPAgain()
        {
            _timerGetOTPAgain = new Timer(60000);
            _timerGetOTPAgain.Elapsed += OnTimedEventGetOTPAgain;
            _timerGetOTPAgain.AutoReset = true;
            _timerGetOTPAgain.Enabled = true;
        }
        private void OnTimedEventGetOTPAgain(Object source, System.Timers.ElapsedEventArgs e)
        {
            _isGetOTPAgain = true;
            TextGetOTPAgain = "Lấy mã khác?";
            _timerGetOTPAgain.Close();
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
            if(!_isGetOTPAgain)
            {
                index = index - 1;
                TextGetOTPAgain = string.Format("Chờ {0}s", index);
            }  
            else
            {
                _timerCountDown.Close();
            }    
            
        }
        // Lấy lại mã OTP
        private void PushOTPAgain()
        {
            if(!_isGetOTPAgain)
            {
                return;
            }
            var parameters = new NavigationParameters
                {
                    { "User", _user },
                    { "Rememberme", _rememberme },
                    { "UserName", _userName },
                    { "Password", _password },
                };
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("NavigationPage/QRCodeLogin", parameters);
            });
        }
        #endregion PrivateMethod
    }
}