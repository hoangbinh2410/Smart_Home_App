﻿using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.ResponeEntity.OTP;
using BA_MobileGPS.Service;
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
    public class QRCodeLoginViewModel :ViewModelBaseLogin

    {
        #region Property

        public ValidatableObject<string> NumberPhone { get; set; }

        #endregion Property

        #region Constructor

        public ICommand PushtoLanguageCommand { get; private set; }
        public ICommand PushOTPPageCommand { get; private set; }
        public ICommand PushZaloPageCommand { get; private set; }

        private readonly IAuthenticationService _iAuthenticationService;
        public QRCodeLoginViewModel(INavigationService navigationService, IAuthenticationService iAuthenticationService) : base(navigationService)
        {
            _iAuthenticationService = iAuthenticationService;
            PushOTPPageCommand = new DelegateCommand(PushOTPPage);
            PushZaloPageCommand = new DelegateCommand(PushZaloPage);
            InitValidations();
        }
        #endregion Constructor

        #region Lifecycle

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);          
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
            NumberPhone.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Số điện thoại không được để trống"});
        }
        private bool ValidateNumberPhone()
        {
            if(!NumberPhone.Validate())
            {
                return false;
            }    
            if(!StringHelper.ValidPhoneNumer(NumberPhone.Value, MobileSettingHelper.LengthAndPrefixNumberPhone))
            {
                DisplayMessage.ShowMessageInfo("Vui lòng kiểm tra lại thông tin số điện thoại đã nhập!", 5000);
                return false;
            }    
            return true;
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
                        { "OTP", objResponse },
                    };
                    await NavigationService.NavigateAsync("VerifyOTPCodePage", parameters);
                }
                else
                {
                    DisplayMessage.ShowMessageInfo("Vui lòng kiểm tra lại mã xác thực OTP", 5000);
                }
            });
            
        }
        private void PushZaloPage()
        {
            
        }

        # endregion PrivateMethod
    }
}