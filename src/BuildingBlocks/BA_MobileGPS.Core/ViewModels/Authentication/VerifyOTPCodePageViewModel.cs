using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.ResponeEntity.OTP;
using BA_MobileGPS.Utilities;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class VerifyOTPCodePageViewModel : ViewModelBaseLogin
    {
        #region Property

        public ValidatableObject<string> OtpValue { get; set; }
        private OtpResultResponse _objOtp;

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
                if (parameters.ContainsKey("OTP") && parameters.GetValue<OtpResultResponse>("OTP") is OtpResultResponse objOtp)
                {
                    _objOtp = objOtp;
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
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("/MainPage");
            });
        }
        // Lấy lại mã OTP
        private void PushOTPAgain()
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("NavigationPage/QRCodeLogin");
            });
        }
        #endregion PrivateMethod
    }
}