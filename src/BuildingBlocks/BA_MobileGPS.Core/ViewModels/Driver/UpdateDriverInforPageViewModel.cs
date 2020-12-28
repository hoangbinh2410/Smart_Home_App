using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Globalization;
using BA_MobileGPS.Utilities;


namespace BA_MobileGPS.Core.ViewModels
{
    public class UpdateDriverInforPageViewModel : ViewModelBase
    {
        private readonly IDriverInforService driverInforService;
        private readonly string NotEmptyMessenge = "Đây là trường bắt buộc";
        public ICommand SaveDriverInforCommand { get; }
        private bool IsInsertpage = false;
        public UpdateDriverInforPageViewModel(INavigationService navigationService, IDriverInforService driverInforService) : base(navigationService)
        {
            this.driverInforService = driverInforService;
            SaveDriverInforCommand = new DelegateCommand(SaveDriverInfor);
            SetLicenseTypeSource();
            InitValidations();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.TryGetValue(ParameterKey.DriverInformation, out DriverInfor driver))
            {
                // Form Edit
                Driver = driver;
                SelectedLicenseType = driver.LicenseType;
                BirthDay = driver.Birthday == null ? DateTime.Now.Date : driver.Birthday;
                IssueDate = driver.IssueLicenseDate == null ? DateTime.Now.Date : driver.IssueLicenseDate;
                ExpiredDate = driver.ExpireLicenseDate == null ? DateTime.Now.Date : driver.ExpireLicenseDate;
                IsInsertpage = true;
            }
            else
            {
                // Form thêm mới
                Driver = new DriverInfor();
                Driver.FK_CompanyID = UserInfo.CompanyId;
                SelectedLicenseType = 0;
                Driver.CreatedDate = DateTime.Now;
                Driver.CreatedByUser = UserInfo.UserId.ToString();
                BirthDay = DateTime.Now.Date;
                IssueDate = DateTime.Now.Date;
                ExpiredDate = DateTime.Now.Date;              
            }
        }

        private DriverInfor driver;
        public DriverInfor Driver
        {
            get { return driver; }
            set { SetProperty(ref driver, value); }
        }

        private DateTime birthDay;
        public DateTime BirthDay
        {
            get { return birthDay; }
            set { SetProperty(ref birthDay, value); }
        }
        private DateTime issueDate;
        public DateTime IssueDate
        {
            get { return issueDate; }
            set { SetProperty(ref issueDate, value);
                ExpireLicenseDateHasError = false;
            }
        }
        private DateTime expiredDate;
        public DateTime ExpiredDate
        {
            get { return expiredDate; }
            set { SetProperty(ref expiredDate, value);
                ExpireLicenseDateHasError = false;
            }
        }

        private ValidatableObject<string> displayName;
        public ValidatableObject<string> DisplayName
        {
            get { return displayName; }
            set { SetProperty(ref displayName, value); }
        }
        private ValidatableObject<string> address;
        public ValidatableObject<string> Address
        {
            get { return address; }
            set { SetProperty(ref address, value); }
        }
        private ValidatableObject<string> mobile;
        public ValidatableObject<string> Mobile
        {
            get { return mobile; }
            set { SetProperty(ref mobile, value); }
        }
        private ValidatableObject<string> identityNumber;
        public ValidatableObject<string> IdentityNumber
        {
            get { return identityNumber; }
            set { SetProperty(ref identityNumber, value); }
        }
        private ValidatableObject<string> driverLicense;
        public ValidatableObject<string> DriverLicense
        {
            get { return driverLicense; }
            set { SetProperty(ref driverLicense, value); }
        }

        private int selectedLicenseType;
        public int SelectedLicenseType
        {
            get => selectedLicenseType;
            set { SetProperty(ref selectedLicenseType, value); }
        }

        private bool expireLicenseDateHasError;
        public bool ExpireLicenseDateHasError
        {
            get { return expireLicenseDateHasError; }
            set { SetProperty(ref expireLicenseDateHasError, value); }
        }

        private string expireLicenseDateErrorMessage;
        public string ExpireLicenseDateErrorMessage
        {
            get { return expireLicenseDateErrorMessage; }
            set { SetProperty(ref expireLicenseDateErrorMessage, value); }
        }


        public string DateFormat => CultureInfo.CurrentCulture.TwoLetterISOLanguageName.Equals("vi") ? "dd/MM/yyyy" : "MM/dd/yyyy";
        public List<string> ListDriverLicenseType { get; set; }



        private void SaveDriverInfor()
        {
            if (Validate())
            {
                RunOnBackground(async () =>
                {
                    UpdateData();
                    SetDateData();
                    var res = await driverInforService.AddDriverInfor(Driver);
                    return res;
                }, res =>
                {
                    //success khi id trả về  > 0
                    if (IsInsertpage && res > 0)
                    {
                        SafeExecute(async () =>
                        {
                            var a = await NavigationService.NavigateAsync("NavigationPage/SuccessUpdateDriverPage", null, true, true);
                        });

                    }
                });

            }
        }


        private bool Validate()
        {
            
            if (IssueDate.Date >= ExpiredDate.Date)
            {
                ExpireLicenseDateHasError = true;
                ExpireLicenseDateErrorMessage = "Ngày hết hạn phải lớn hơn ngày cấp";
                return false;
            }

            return DisplayName.Validate() && Address.Validate()
                && Mobile.Validate() && IdentityNumber.Validate()
                && DriverLicense.Validate();
        }


        private void SetLicenseTypeSource()
        {
            var list = new List<string>
            {
                "Chọn loại bằng lái"
            };
            var temp = Enum.GetNames(typeof(DriverLicenseEnum)).ToList();
            list.AddRange(temp);
            ListDriverLicenseType = list;
        }

        private void UpdateData()
        {
            Driver.Name = StringHelper.ConvertToVn(Driver.DisplayName);
            Driver.EmployeeCode = string.Empty;
            Driver.PK_EmployeeID = Driver.ID;
            Driver.PhoneNumber1 = string.Empty;
            Driver.PhoneNumber2 = string.Empty;
            Driver.Sex = 0;
            Driver.UpdatedByUser = UserInfo.UserId.ToString();
            Driver.UpdatedDate = DateTime.Now;
        }

        private void SetDateData()
        {
            Driver.ExpireLicenseDate = expiredDate;
            Driver.Birthday = birthDay;
            Driver.IssueLicenseDate = issueDate;
        }

        private void InitValidations()
        {
            DisplayName = new ValidatableObject<string>();
            Address = new ValidatableObject<string>();
            Mobile = new ValidatableObject<string>();
            IdentityNumber = new ValidatableObject<string>();
            DriverLicense = new ValidatableObject<string>();

            DisplayName.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = NotEmptyMessenge });
            Address.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = NotEmptyMessenge });

            Mobile.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = NotEmptyMessenge });
            Mobile.Validations.Add(new PhoneNumberRule<string> { ValidationMessage = "Số không chính xác" });

            IdentityNumber.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = NotEmptyMessenge });
            IdentityNumber.Validations.Add(new MinLenghtRule<string> { ValidationMessage = "Tối thiểu 12", MinLenght = 12 });

            DriverLicense.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = NotEmptyMessenge });
        }
    }
}
