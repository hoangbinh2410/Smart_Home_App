using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.ResponeEntity.OTP;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Windows.Input;
using Xamarin.Essentials;

namespace BA_MobileGPS.Core.ViewModels
{
    public class NumberPhoneLoginPageViewModel : ViewModelBaseLogin
    {
        #region Property

        public ValidatableObject<string> NumberPhone { get; set; }

        private bool isOTPZalo = false;
        public bool IsOTPZalo { get { return isOTPZalo; } set { SetProperty(ref isOTPZalo, value); } }

        private LoginResponse _user = new LoginResponse();
        private bool _rememberme = false;
        private string _userName = string.Empty;
        private string _password = string.Empty;

        private LoginResponse userInfo;
        public LoginResponse UserInfo
        { get { if (StaticSettings.User != null) { UserInfo = StaticSettings.User; } return userInfo; } set => SetProperty(ref userInfo, value); }

        #endregion Property

        #region Contructor

        public ICommand PushOTPSMSPageCommand { get; private set; }
        public ICommand PushOTPPageCommand { get; private set; }
        public ICommand PushZaloPageCommand { get; private set; }
        private readonly IAuthenticationService _iAuthenticationService;
        public NumberPhoneLoginPageViewModel(INavigationService navigationService, IAuthenticationService iAuthenticationService) : base(navigationService)
        {
            PushOTPSMSPageCommand = new DelegateCommand(PushOTPSMSPage);
            PushOTPPageCommand = new DelegateCommand(PushOTPPage);
            PushZaloPageCommand = new DelegateCommand(PushZaloPage);
            _iAuthenticationService = iAuthenticationService;
            InitValidations();
        }

        #endregion Contructor

        #region Lifecycle

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
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

        #endregion Lifecycle
        
        #region PrivateMethod
        private void InitValidations()
        {
            NumberPhone = new ValidatableObject<string>();
            NumberPhone.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Số điện thoại không được để trống" });
        }
        private bool ValidateNumberPhone()
        {
            if (!NumberPhone.Validate())
            {
                return false;
            }
            if (!StringHelper.ValidPhoneNumer(NumberPhone.Value, MobileSettingHelper.LengthAndPrefixNumberPhone))
            {
                DisplayMessage.ShowMessageInfo("Vui lòng kiểm tra lại thông tin số điện thoại đã nhập!", 5000);
                return false;
            }
            if (NumberPhone.Value.Trim() != _user.PhoneNumber.Trim())
            {
                DisplayMessage.ShowMessageInfo("Vui lòng nhập số điện thoại đã đăng ký tài khoản", 5000);
                return false;
            }
            return true;
        }
        //Gửi lấy mã OTP cho sms
        private void PushOTPSMSPage()
        {
            var objResponse = new SendCodeSMSResponse();
            //Kiểm tra số điện thoại nhập vào
            bool isValid = ValidateNumberPhone();
            if (!isValid)
            {
                return;
            }
            SafeExecute(async () =>
            {
                if (IsConnected)
                {
                    var inputSendCodeSMS = new ForgotPasswordRequest
                    {
                        phoneNumber = NumberPhone.Value,
                        userName = _user.UserName,
                        AppID = (int)App.AppType
                    };
                    using (new HUDService(MobileResource.Common_Message_Processing))
                    {
                       objResponse = await _iAuthenticationService.SendCodeSMS(inputSendCodeSMS);
                    }
                }
                else
                {
                    DisplayMessage.ShowMessageInfo(MobileResource.Common_ConnectInternet_Error, 5000);
                    return;
                }
                // Sau khi gọi API 
                if (objResponse != null && !string.IsNullOrEmpty(objResponse.SecurityCodeSMS))
                {
                    var parameters = new NavigationParameters
                    {
                        { "OTPSms", objResponse },
                        { "User", _user },
                        { "Rememberme", _rememberme },
                        { "UserName", _userName },
                        { "Password", _password },
                    };
                    await NavigationService.NavigateAsync("/VerifyOTPSmsPage", parameters);
                }
                else
                {
                    DisplayMessage.ShowMessageInfo("Vui lòng kiểm tra lại số điện thoại", 5000);
                }
            });
        }
        /// <summary>Get mã OTP</summary>
        /// <Modified>
        /// Name     Date         Comments
        /// ducpv  1/12/2021   created
        /// </Modified>
        private void PushOTPPage()
        {
            var objResponse = new OtpResultResponse();
            string customerID = String.Empty;
            //Kiểm tra số điện thoại nhập vào
            bool isValid = ValidateNumberPhone();
            if (!isValid)
            {
                return;
            }
            SafeExecute(async () =>
            {
                if (IsConnected)
                {
                    using (new HUDService(MobileResource.Common_Message_Processing))
                    {
                        objResponse = await _iAuthenticationService.GetOTP(NumberPhone.Value, customerID);
                    }
                }
                else
                {
                    DisplayMessage.ShowMessageInfo(MobileResource.Common_ConnectInternet_Error, 5000);
                    return;
                }
                // Sau khi gọi API
                if (objResponse != null && !string.IsNullOrEmpty(objResponse.OTP))
                {
                    var parameters = new NavigationParameters
                    {
                        { "OTPZalo", objResponse },
                        { "User", _user },
                        { "Rememberme", _rememberme },
                        { "UserName", _userName },
                        { "Password", _password },
                    };
                    await NavigationService.NavigateAsync("VerifyOTPCodePage", parameters);
                }
                else
                {
                    DisplayMessage.ShowMessageInfo("Quý khách Vui lòng kiểm tra like Zalo Bình Anh ?", 5000);
                }
            });

        }
        private void PushZaloPage()
        {
            SafeExecute(async () => await Launcher.OpenAsync(new Uri(MobileSettingHelper.LinkZaloBA)));
        }
        #endregion PrivateMethod
    }
}