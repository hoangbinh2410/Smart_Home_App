using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.RequestEntity;
using BA_MobileGPS.Entities.ResponeEntity;
using BA_MobileGPS.Service.IService;
using BA_MobileGPS.Utilities;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class InsuranceInforViewModel : ViewModelBase
    {
     
        private bool IsUpdateForm { get; set; } = false;
        private long currentVehicleId { get; set; } = 0;
        private string NotEmptyMessenge = MobileResource.ListDriver_Messenger_NotNull;
        private readonly IPapersInforService paperinforService;
        public ICommand SaveInsuranceInforCommand { get; }
        public ICommand SelectRegisterDateCommand { get; }
        public ICommand SelectExpireDateCommand { get; }
        public ICommand ChangeToInsertFormCommand { get; }
        public InsuranceInforViewModel(INavigationService navigationService, IPapersInforService paperinforService) : base(navigationService)
        {
            this.paperinforService = paperinforService;
            SaveInsuranceInforCommand = new DelegateCommand(SaveInsuranceInfor).ObservesCanExecute(()=>SaveEnable);
            SelectRegisterDateCommand = new DelegateCommand(SelectRegisterDate);
            SelectExpireDateCommand = new DelegateCommand(SelectExpireDate);
            ChangeToInsertFormCommand = new DelegateCommand(ChangeToInsertForm);
            InitValidations();
            InitInsurancePickerSource();
            Device.StartTimer(new TimeSpan(0, 0, 1), () =>
            {
                ViewHasAppeared = true;
                return false;
            });
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
            else if (parameters.ContainsKey(ParameterKey.Vehicle) && parameters.GetValue<string>(ParameterKey.Vehicle) is string privateCode)
            {
                var vehicle = StaticSettings.ListVehilceOnline.FirstOrDefault(x => x.PrivateCode == privateCode);
                if (vehicle != null)
                {
                    currentVehicleId = vehicle.VehicleId;
                    UpdateFormData(UserInfo.CompanyId, vehicle.VehicleId);
                }
            }
        }
     
        private bool saveEnable = true;
        public bool SaveEnable
        {
            get { return saveEnable; }
            set { SetProperty(ref saveEnable, value);           
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
        /// <summary>
        /// Type int, dùng string thay thế do có lỗi với ValidatableObject
        /// </summary>
        private ValidatableObject<string> daysNumberForAlertAppear;

        public ValidatableObject<string> DaysNumberForAlertAppear
        {
            get { return daysNumberForAlertAppear; }
            set { SetProperty(ref daysNumberForAlertAppear, value); }
        }

        private ValidatableObject<InsuranceCategory> selectedInsuranceType;

        public ValidatableObject<InsuranceCategory> SelectedInsuranceType
        {
            get { return selectedInsuranceType; }
            set { SetProperty(ref selectedInsuranceType, value); }
        }
        /// <summary>
        /// Type decimal, dùng string thay thế do có lỗi với ValidatableObject
        /// </summary>
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

        private string alertMessenger;
        public string AlertMessenger
        {
            get { return alertMessenger; }
            set
            {
                SetProperty(ref alertMessenger, value);
                RaisePropertyChanged(); // bắt buộc có
            }
        }
        private List<InsuranceCategory> listInsuranceType;
        public List<InsuranceCategory> ListInsuranceType
        {
            get { return listInsuranceType; }
            set { SetProperty(ref listInsuranceType, value); }
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
            DaysNumberForAlertAppear = new ValidatableObject<string>();
            DaysNumberForAlertAppear.OnChanged += ValidationStringValue_OnChanged;
            SelectedInsuranceType = new ValidatableObject<InsuranceCategory>();

            InsuranceFee = new ValidatableObject<string>();
            InsuranceFee.OnChanged += ValidationStringValue_OnChanged;
            Contact = new ValidatableObject<string>();
            Contact.OnChanged += ValidationStringValue_OnChanged; ;
            Notes = new ValidatableObject<string>();
            Notes.OnChanged += ValidationStringValue_OnChanged;
            UnitName = new ValidatableObject<string>();
            UnitName.OnChanged += ValidationStringValue_OnChanged;
            SetValidationRule();
        }


        private void ValidationStringValue_OnChanged(object sender, string e)
        {

            var obj = (ValidatableObject<string>)sender;
            // Check chon xe chua?
            if (currentVehicleId == 0 && ViewHasAppeared)
            {
                obj.IsNotValid = true;
                obj.ErrorFirst = "Vui lòng chọn xe trước khi nhập dữ liệu";
                return;
            }
            //enable button save
            if (!SaveEnable && ViewHasAppeared)
            {
                SaveEnable = true;
            }

            // Clear validation              
            if (obj.IsNotValid)
            {
                obj.IsNotValid = false;
                obj.Errors.Clear();
            }

        }

        private void ValidationDateTimeValue_OnChanged(object sender, DateTime e)
        {

            var obj = (ValidatableObject<DateTime>)sender;
            // Check chon xe chua?
            if (currentVehicleId == 0 && ViewHasAppeared)
            {
                obj.IsNotValid = true;
                obj.ErrorFirst = "Vui lòng chọn xe trước khi nhập dữ liệu";
                return;
            }

            //enable button save
            if (!SaveEnable && ViewHasAppeared)
            {
                SaveEnable = true;
            }

            // Clear validation              
            if (obj.IsNotValid)
            {
                obj.IsNotValid = false;
                obj.Errors.Clear();
            }
        }

        private void SetValidationRule()
        {
            InsuranceNumber.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = NotEmptyMessenge + "số bảo hiểm" });
            InsuranceNumber.Validations.Add(new ExpressionDangerousCharsUpdateRule<string>
            {
                DangerousChar = "['\"<>/&]",
                ValidationMessage = "Vui lòng không nhập [,',\",<,>,/, &,]"
            });

            RegistrationDate.Validations.Add(new EmptyDateTimeRule<DateTime> { ValidationMessage = NotEmptyMessenge + "ngày đăng kí" });

            ExpireDate.Validations.Add(new EmptyDateTimeRule<DateTime> { ValidationMessage = NotEmptyMessenge + "ngày hết hạn" });

            DaysNumberForAlertAppear.Validations.Add(new IntMinValueRule<string>() { ValidationMessage = "Vui lòng nhập số ngày > 0", MinValue = 1 });

            Contact.Validations.Add(new ExpressionDangerousCharsUpdateRule<string>
            {
                DangerousChar = "['\"<>/&]",
                ValidationMessage = "Vui lòng không nhập [,',\",<,>,/, &,]"
            });
            Contact.Validations.Add(new MaxLengthRule<string>() { MaxLenght = 500, ValidationMessage = "Không nhập quá 500 kí tự" });
            Notes.Validations.Add(new ExpressionDangerousCharsUpdateRule<string>
            {
                DangerousChar = "['\"<>/&]",
                ValidationMessage = "Vui lòng không nhập [,',\",<,>,/, &,]"
            });
            Notes.Validations.Add(new MaxLengthRule<string>() { MaxLenght = 1000, ValidationMessage = "Không nhập quá 1000 kí tự" });
            UnitName.Validations.Add(new ExpressionDangerousCharsUpdateRule<string>
            {
                DangerousChar = "['\"<>/&]",
                ValidationMessage = "Vui lòng không nhập [,',\",<,>,/, &,]"
            });
            UnitName.Validations.Add(new MaxLengthRule<string>() { MaxLenght = 500, ValidationMessage = "Không nhập quá 500 kí tự" });
            SelectedInsuranceType.Validations.Add(new IsNotNullObjectRule<InsuranceCategory>() { ValidationMessage = MobileResource.ListDriver_Messenger_NotSelect + "loại bảo hiểm" });
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

            //Check số ngày cảnh báo trước quá lớn
            var newRule = true;
            var temp = (ExpireDate.Value - RegistrationDate.Value).TotalDays;
            if (!string.IsNullOrEmpty(DaysNumberForAlertAppear.Value))
            {
                if (Convert.ToInt32(DaysNumberForAlertAppear.Value) >= temp)
                {
                    DaysNumberForAlertAppear.IsNotValid = true;
                    DaysNumberForAlertAppear.ErrorFirst = "Số ngày cảnh báo trước quá lớn";
                    newRule = false;
                }
            }

            //Check ngày đăng kí > ngày hết hạn
            var outDateRule = true;
            if (RegistrationDate.Value >= ExpireDate.Value)
            {
                ExpireDate.IsNotValid = true;
                ExpireDate.ErrorFirst = "Vui lòng nhập ngày hết hạn > ngày đăng ký";
                outDateRule = false;
            }

            //Check ngày đăng kí mới và ngày hết hạn cũ
            var oldExpireDayRule = true;
            if (!IsUpdateForm && oldInfor != null)
            {
                var oldExpireDay = oldInfor.PaperInfo?.ExpireDate;
                if (RegistrationDate.Value <= oldExpireDay)
                {
                    RegistrationDate.IsNotValid = true;
                    RegistrationDate.ErrorFirst = "Ngày đăng kí mới > ngày hết hạn giấy tờ gần nhất";
                    oldExpireDayRule = false;
                }
            }

            return (insuranceNum && dateRegis && dateExp && dayPrepareAlert
                && insuranceType && fee && departmentUnit && contactInfor && note && newRule 
                && outDateRule && oldExpireDayRule);
        }

        private void SaveInsuranceInfor()
        {
            var insertPer = (int)PermissionKeyNames.PaperAddNew;
            if (!UserInfo.Permissions.Distinct().Contains(insertPer))
            {
                DisplayMessage.ShowMessageError("Tài khoản hiện tại chưa có quyền thay đổi dữ liệu");
                return;
            }

            if (currentVehicleId == 0)
            {
                DisplayMessage.ShowMessageError("Vui lòng chọn biển số phương tiện");
                return;
            }
          
            if (Validate())
            {
                var data = GetFormData();
                data.PaperInfo.FK_CompanyID = UserInfo.CompanyId;
                data.PaperInfo.FK_VehicleID = currentVehicleId;
                SafeExecute(async () =>
                {
                    if (IsUpdateForm)
                    {
                        data.PaperInfo.UpdatedByUser = UserInfo.UserId;
                        data.PaperInfo.Id = oldInfor.PaperInfo.Id;
                        var res = await paperinforService.UpdateInsurancePaper(data);
                        if (res?.PK_PaperInfoID != new Guid())
                        {
                            DisplayMessage.ShowMessageSuccess("Cập nhật thông tin thành công");
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(res?.ErrorMessenger))
                            {
                                DisplayMessage.ShowMessageError(res?.ErrorMessenger);
                            }
                            else DisplayMessage.ShowMessageError("Cập nhật thông tin thất bại");
                        }
                    }
                    else
                    {
                        data.PaperInfo.CreatedByUser = UserInfo.UserId;
                        var res = await paperinforService.InsertInsurancePaper(data);
                        if (res?.PK_PaperInfoID != new Guid())
                        {
                            DisplayMessage.ShowMessageSuccess("Thêm mới thông tin thành công");
                            ClearData();
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(res?.ErrorMessenger))
                            {
                                DisplayMessage.ShowMessageError(res?.ErrorMessenger);
                            }
                            else DisplayMessage.ShowMessageError("Thêm mới thông tin thất bại");
                        }

                    }
                });
            }
        }


        private bool createButtonVisible;
        public bool CreateButtonVisible
        {
            get { return createButtonVisible; }
            set
            {
                SetProperty(ref createButtonVisible, value);
                RaisePropertyChanged();// bắt buộc có
            }
        }
        private PaperInsuranceInsertRequest oldInfor { get; set; }
        private void UpdateFormData(int companyId, long vehicleId)
        {
            CreateButtonVisible = false;
            SafeExecute(async () =>
            {
                var paper = await paperinforService.GetLastPaperInsuranceByVehicleId(companyId, vehicleId);
                if (paper != null)
                {
                    
                    oldInfor = paper;
                    IsUpdateForm = true;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        InsuranceNumber.Value = paper.PaperInfo.PaperNumber;
                        RegistrationDate.Value = paper.PaperInfo.DateOfIssue;
                        ExpireDate.Value = paper.PaperInfo.ExpireDate;
                        DaysNumberForAlertAppear.Value = paper.PaperInfo.DayOfAlertBefore.ToString();
                        SelectedInsuranceType.Value = listInsuranceType.FirstOrDefault(x => x.Id == paper.FK_InsuranceCategoryID);
                        InsuranceFee.Value = paper.Cost?.ToString("G0");
                        Contact.Value = paper.Contact;
                        Notes.Value = paper.PaperInfo.Description;
                        UnitName.Value = paper.WarrantyCompany;

                        SaveEnable = false;
                    });
                    if (DateTime.Now > paper.PaperInfo.ExpireDate)
                    {
                        AlertMessenger = string.Format("<font color=#E65353>{0}</font>", MobileResource.PaperInfor_Msg_Expired);
                        CreateButtonVisible = true;
                    }
                    else if (paper.PaperInfo.ExpireDate.AddDays(-CompanyConfigurationHelper.DayAllowRegister) <= DateTime.Now)
                    {
                        AlertMessenger = string.Format("<font color=#F99B09>{0}</font>", MobileResource.PaperInfor_Msg_NearExpire);
                        CreateButtonVisible = true;
                    }
                }
                else ClearData();
            });

        }

        private PaperInsuranceInsertRequest GetFormData()
        {
            var res = new PaperInsuranceInsertRequest();
            res.PaperInfo = new PaperBasicInfor()
            {
                PaperNumber = InsuranceNumber.Value,
                DateOfIssue = RegistrationDate.Value,
                ExpireDate = ExpireDate.Value,
                DayOfAlertBefore = Convert.ToInt32(DaysNumberForAlertAppear.Value.Trim()),
                Description = Notes.Value
            };
            res.FK_InsuranceCategoryID = SelectedInsuranceType.Value?.Id;
            if (!string.IsNullOrEmpty(InsuranceFee.Value))
            {
                res.Cost = Convert.ToDecimal(InsuranceFee.Value.Trim());
            }

            res.Contact = Contact.Value;
            res.WarrantyCompany = UnitName.Value;

            return res;

        }

        private void SelectRegisterDate()
        {
            SafeExecute(async () =>
            {
                var parameters = new NavigationParameters();
                var day = RegistrationDate.Value == new DateTime() ? DateTime.Now : RegistrationDate.Value;
                parameters.Add("PickerType", (short)ComboboxType.First);
                parameters.Add("DataPicker", day);
                var a = await NavigationService.NavigateAsync("SelectDatePicker", parameters);
            });

        }

        private void SelectExpireDate()
        {
            SafeExecute(async () =>
            {
                var parameters = new NavigationParameters();
                var day = ExpireDate.Value == new DateTime() ? DateTime.Now : ExpireDate.Value;
                parameters.Add("PickerType", (short)ComboboxType.Second);
                parameters.Add("DataPicker", day);
                var a = await NavigationService.NavigateAsync("SelectDatePicker", parameters);
            });
        }

        private void InitInsurancePickerSource()
        {
            RunOnBackground(async () =>
            {
                return await paperinforService.GetInsuranceCategories(UserInfo.CompanyId);

            }, res =>
            {
                if (res != null && res.Count > 0)
                {
                    ListInsuranceType = res;
                }
            });
        }

        private void ChangeToInsertForm()
        {
            InsuranceNumber.Value = string.Empty;
            ExpireDate.Value = DateTime.Now;
            DaysNumberForAlertAppear.Value = "3";
            SelectedInsuranceType.Value = null;
            InsuranceFee.Value = null;
            Contact.Value = string.Empty;
            Notes.Value = string.Empty;
            UnitName.Value = string.Empty;

            RegistrationDate.Value = oldInfor.PaperInfo.ExpireDate.AddDays(1);
            CreateButtonVisible = false;
            IsUpdateForm = false;
            SaveEnable = true;
        }
        private void ClearData()
        {
            RegistrationDate.Value = DateTime.Now;
            InsuranceNumber.Value = string.Empty;
            ExpireDate.Value = DateTime.Now;
            DaysNumberForAlertAppear.Value = "3";
            SelectedInsuranceType.Value = null;
            InsuranceFee.Value = null;
            Contact.Value = string.Empty;
            Notes.Value = string.Empty;
            UnitName.Value = string.Empty;

            CreateButtonVisible = false;
        }

    }
}