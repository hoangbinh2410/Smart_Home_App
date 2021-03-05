using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Utilities;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace BA_MobileGPS.Core.ViewModels
{
    public class ForgotPasswordPageViewModel : ViewModelBaseLogin
    {
        public ICommand SendInfoCommand { get; }
        public ForgotPasswordPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            SendInfoCommand = new DelegateCommand(SendInfo);
        }
        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            InitValidation();
        }

        private void InitValidation()
        {
            accountName = new ValidatableObject<string>();
            accountName.OnChanged += StringValue_OnChanged;
            phoneNumber = new ValidatableObject<string>();
            phoneNumber.OnChanged += StringValue_OnChanged;

            accountName.Validations.Add(new IsNotNullOrEmptyRule<string> { 
                ValidationMessage = MobileResource.ForgotPassword_Message_Validate_IsNull_AccountName });
            accountName.Validations.Add(new MaxLengthRule<string> { 
                ValidationMessage = MobileResource.ForgotPassword_Message_Validate_MaxLength_AccountName, MaxLenght = 50 });
            accountName.Validations.Add(new IsNotContainsHTMLTag<string> { 
                ValidationMessage = MobileResource.ForgotPassword_Message_Validate_ConstantHTML_AccountName });
            accountName.Validations.Add(new ExpressionDangerousCharsRule<string> { 
                Expression = MobileSettingHelper.ConfigDangerousCharTextBox, 
                ValidationMessage = MobileResource.Common_Property_DangerousCharShow(MobileResource.ForgotPassword_Label_AccountName, 
                                                                                      MobileSettingHelper.ConfigDangerousCharTextBox) });

            phoneNumber.Validations.Add(new IsNotNullOrEmptyRule<string> { 
                ValidationMessage = MobileResource.ForgotPassword_Message_Validate_IsNull_Phone });
            phoneNumber.Validations.Add(new IsNotContainsHTMLTag<string> { 
                ValidationMessage = MobileResource.ForgotPassword_Message_Validate_ConstantHTML_AccountName });
            phoneNumber.Validations.Add(new ExpressionDangerousCharsRule<string> { 
                Expression = MobileSettingHelper.ConfigDangerousCharTextBox, 
                ValidationMessage = MobileResource.Common_Property_DangerousCharShow(MobileResource.ForgotPassword_Label_Phone, 
                                                                                     MobileSettingHelper.ConfigDangerousCharTextBox) });
            if (Settings.CurrentLanguage.Equals(CultureCountry.Vietnamese))
            {
                phoneNumber.Validations.Add(new PhoneNumberRule<string> { 
                    ValidationMessage = MobileResource.ForgotPassword_Message_Validate_Rule_Phone, 
                    CountryCode = CountryCodeConstant.VietNam });
            }
        }

        private void StringValue_OnChanged(object sender, string e)
        {
            try
            {
                var control = (ValidatableObject<string>)sender;
                control.IsNotValid = false;
            }
            catch (Exception ex)
            {

                throw;
            }
          
        }

        private bool Validate()
        {
            var accValidate = accountName.Validate();
            var phoneValidate = phoneNumber.Validate();
            return accValidate && phoneValidate; 
        }

        private void SendInfo()
        {
            if (Validate())
            {

            }
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
    }
}
