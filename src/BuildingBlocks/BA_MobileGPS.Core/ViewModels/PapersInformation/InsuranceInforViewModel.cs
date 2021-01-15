using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Windows.Input;

namespace BA_MobileGPS.Core.ViewModels
{
    public class InsuranceInforViewModel : ViewModelBase
    {
        private string NotEmptyMessenge = MobileResource.ListDriver_Messenger_NotNull;
        public ICommand SaveInsuranceInforCommand { get; }
        public ICommand SelectRegisterDateCommand { get; }
        public ICommand SelectExpireDateCommand { get; }
        public InsuranceInforViewModel(INavigationService navigationService) : base(navigationService)
        {
            SaveInsuranceInforCommand = new DelegateCommand(SaveInsuranceInfor);
            SelectRegisterDateCommand = new DelegateCommand(SelectRegisterDate);
            SelectExpireDateCommand = new DelegateCommand(SelectExpireDate);        
            InitValidations();
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey(ParameterKey.DateResponse)
                && parameters.GetValue<PickerDateTimeResponse>(ParameterKey.DateResponse) is PickerDateTimeResponse date)
            {
                if (date.PickerType == (int)ComboboxType.First)
                {
                    //register
                    RegistrationDate.Value = date.Value;
                }
                else if (date.PickerType == (int)ComboboxType.Second)
                {
                    //expire
                   ExpireDate.Value = date.Value;
                }
            }
        }
        private ValidatableObject<string> insuranceNumber;

        public ValidatableObject<string> InsuranceNumber
        {
            get { return insuranceNumber; }
            set { SetProperty(ref insuranceNumber, value); }
        }

        private ValidatableObject<DateTime> registrationDate;

        public ValidatableObject<DateTime> RegistrationDate
        {
            get { return registrationDate; }
            set { SetProperty(ref registrationDate, value); }
        }

        private ValidatableObject<DateTime> expireDate;

        public ValidatableObject<DateTime> ExpireDate
        {
            get { return expireDate; }
            set { SetProperty(ref expireDate, value); }
        }

        private ValidatableObject<int> daysNumberForAlertAppear;

        public ValidatableObject<int> DaysNumberForAlertAppear
        {
            get { return daysNumberForAlertAppear; }
            set { SetProperty(ref daysNumberForAlertAppear, value); }
        }

        private ValidatableObject<int> selectedInsuranceType;

        public ValidatableObject<int> SelectedInsuranceType
        {
            get { return selectedInsuranceType; }
            set { SetProperty(ref selectedInsuranceType, value); }
        }

        private ValidatableObject<string> insuranceFee;

        public ValidatableObject<string> InsuranceFee
        {
            get { return insuranceFee; }
            set { SetProperty(ref insuranceFee, value); }
        }

        private ValidatableObject<string> unitName;

        public ValidatableObject<string> UnitName
        {
            get { return unitName; }
            set { SetProperty(ref unitName, value); }
        }

        private ValidatableObject<string> contact;

        public ValidatableObject<string> Contact
        {
            get { return contact; }
            set { SetProperty(ref contact, value); }
        }

        private ValidatableObject<string> notes;

        public ValidatableObject<string> Notes
        {
            get { return notes; }
            set { SetProperty(ref notes, value); }
        }

        /// <summary>
        ///  Init validation rule cho các ô nhập dữ liệu
        /// </summary>
        private void InitValidations()
        {
            InsuranceNumber = new ValidatableObject<string>();
            InsuranceNumber.OnChanged += ValidationStringValue_OnChanged;
            RegistrationDate = new ValidatableObject<DateTime>();
            RegistrationDate.OnChanged += ValidationDateTimeValue_OnChanged;
            ExpireDate = new ValidatableObject<DateTime>();
            ExpireDate.OnChanged += ValidationDateTimeValue_OnChanged;
            DaysNumberForAlertAppear = new ValidatableObject<int>();
            DaysNumberForAlertAppear.OnChanged += ValidationIntValue_OnChanged;
            SelectedInsuranceType = new ValidatableObject<int>();
            SelectedInsuranceType.OnChanged += ValidationIntValue_OnChanged;
            InsuranceFee = new ValidatableObject<string>();
            InsuranceFee.OnChanged += ValidationStringValue_OnChanged; ;
            Contact = new ValidatableObject<string>();
            Contact.OnChanged += ValidationStringValue_OnChanged; ;
            Notes = new ValidatableObject<string>();
            Notes.OnChanged += ValidationStringValue_OnChanged;

            SetValidationRule();
        }

        private void ValidationIntValue_OnChanged(object sender, int e)
        {
            // Clear validation
            var obj = (ValidatableObject<int>)sender;
            if (obj.IsNotValid)
            {
                obj.IsNotValid = false;
                obj.Errors.Clear();
            }
        }

        private void ValidationStringValue_OnChanged(object sender, string e)
        {
            // Clear validation
            var obj = (ValidatableObject<string>)sender;
            if (obj.IsNotValid)
            {
                obj.IsNotValid = false;
                obj.Errors.Clear();
            }
        }

        private void ValidationDateTimeValue_OnChanged(object sender, DateTime e)
        {
            // Clear validation
            var obj = (ValidatableObject<DateTime>)sender;
            if (obj.IsNotValid)
            {
                obj.IsNotValid = false;
                obj.Errors.Clear();
            }
        }

        private void SetValidationRule()
        {
            InsuranceNumber.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = NotEmptyMessenge + "họ tên" });
            InsuranceNumber.Validations.Add(new ExpressionDangerousCharsUpdateRule<string>
            {
                DangerousChar = "['\"<>/&]",
                ValidationMessage = "Vui lòng nhập số bảo hiểm hợp lệ"
            });

            RegistrationDate.Validations.Add(new EmptyDateTimeRule<DateTime> { ValidationMessage = NotEmptyMessenge + "ngày đăng kí" });

            ExpireDate.Validations.Add(new EmptyDateTimeRule<DateTime> { ValidationMessage = NotEmptyMessenge + "ngày hết hạn" });

            DaysNumberForAlertAppear.Validations.Add(new IntMinValueRule<int>() { ValidationMessage = NotEmptyMessenge + "số ngày cảnh báo trước", MinValue = 1 });

            InsuranceFee.Validations.Add(new ExpressionDangerousCharsUpdateRule<string>
            {
                DangerousChar = "['\"<>/&]",
                ValidationMessage = "Vui lòng nhập số tiền hợp lệ"
            });

            Contact.Validations.Add(new ExpressionDangerousCharsUpdateRule<string>
            {
                DangerousChar = "['\"<>/&]",
                ValidationMessage = "Vui lòng nhập nơi liên hệ hợp lệ"
            });

            Notes.Validations.Add(new ExpressionDangerousCharsUpdateRule<string>
            {
                DangerousChar = "['\"<>/&]",
                ValidationMessage = "Vui lòng nhập ghi chú hợp lệ"
            });
        }

        /// <summary>
        /// kiểm tra validation
        /// </summary>
        /// <returns>true : pass</returns>
        private bool Validate()
        {           
            var insuranceNum = InsuranceNumber.Validate();
            var dateRegis = RegistrationDate.Validate();
            var dateExp = ExpireDate.Validate();
            var dayPrepareAlert = DaysNumberForAlertAppear.Validate();
            var insuranceType = SelectedInsuranceType.Validate();
            var fee = InsuranceFee.Validate();
            var departmentUnit = UnitName.Validate(); 
            var contactInfor = Contact.Validate();
            var note = Notes.Validate();

            return (insuranceNum && dateRegis && dateExp && dayPrepareAlert
                && insuranceType && fee && departmentUnit && contactInfor && note);
        }

        private void SaveInsuranceInfor()
        {
            if (Validate())
            {

            }
        }

        private void SelectRegisterDate()
        {
            SafeExecute(async () =>
            {
                var parameters = new NavigationParameters();
                var day = RegistrationDate.Value == new DateTime() ? DateTime.Now : RegistrationDate.Value;
                parameters.Add("PickerType", ComboboxType.First);
                parameters.Add("DataPicker", day);
                var a = await NavigationService.NavigateAsync("SelectDatePicker",parameters);
            });

        }
        private void SelectExpireDate()
        {
            SafeExecute(async () =>
            {
                var parameters = new NavigationParameters();
                var day = ExpireDate.Value == new DateTime() ? DateTime.Now : ExpireDate.Value;
                parameters.Add("PickerType", ComboboxType.Second);
                parameters.Add("DataPicker", day);
                var a = await NavigationService.NavigateAsync("SelectDatePicker", parameters);
            });
        }

    }
}