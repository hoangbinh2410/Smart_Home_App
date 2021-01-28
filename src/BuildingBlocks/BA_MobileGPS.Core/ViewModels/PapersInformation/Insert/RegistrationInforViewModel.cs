using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.RequestEntity;
using BA_MobileGPS.Service.IService;
using BA_MobileGPS.Utilities;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class RegistrationInforViewModel : ViewModelBase
    {
        private bool IsUpdateForm { get; set; } = false;
       
        private long currentVehicleId { get; set; } = 0;
        private string NotEmptyMessenge = MobileResource.ListDriver_Messenger_NotNull;
        private readonly IPapersInforService paperinforService;
        public ICommand SaveRegistrationInforCommand { get; }
        public ICommand SelectRegisterDateCommand { get; }
        public ICommand SelectExpireDateCommand { get; }
        public ICommand ChangeToInsertFormCommand { get; }
        public RegistrationInforViewModel(INavigationService navigationService, IPapersInforService paperinforService) : base(navigationService)
        {
            this.paperinforService = paperinforService;
            SaveRegistrationInforCommand = new DelegateCommand(SaveRegistrationInfor).ObservesCanExecute(() => SaveEnable);
            SelectRegisterDateCommand = new DelegateCommand(SelectRegisterDate);
            SelectExpireDateCommand = new DelegateCommand(SelectExpireDate);
            ChangeToInsertFormCommand = new DelegateCommand(ChangeToInsertForm);
            InitValidations();
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
            set
            {
                SetProperty(ref saveEnable, value);
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

        private ValidatableObject<string> identityCode;

        public ValidatableObject<string> IdentityCode
        {
            get { return identityCode; }
            set { SetProperty(ref identityCode, value); }
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

        private ValidatableObject<string> unitName;

        public ValidatableObject<string> UnitName
        {
            get { return unitName; }
            set { SetProperty(ref unitName, value); }
        }
        /// <summary>
        /// Type decimal, dùng string thay thế do có lỗi với ValidatableObject
        /// </summary>
        private ValidatableObject<string> registrationFee;

        public ValidatableObject<string> RegistrationFee
        {
            get { return registrationFee; }
            set { SetProperty(ref registrationFee, value); }
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
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///  Init validation rule cho các ô nhập dữ liệu
        /// </summary>
        private void InitValidations()
        {
            IdentityCode = new ValidatableObject<string>();
            IdentityCode.OnChanged += ValidationStringValue_OnChanged;
            RegistrationDate = new ValidatableObject<DateTime>();
            RegistrationDate.OnChanged += ValidationDateTimeValue_OnChanged;
            ExpireDate = new ValidatableObject<DateTime>();
            ExpireDate.OnChanged += ValidationDateTimeValue_OnChanged;
            DaysNumberForAlertAppear = new ValidatableObject<string>();
            DaysNumberForAlertAppear.OnChanged += ValidationStringValue_OnChanged;
            RegistrationFee = new ValidatableObject<string>();
            RegistrationFee.OnChanged += ValidationStringValue_OnChanged;
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

            if (obj.IsNotValid)
            {
                obj.IsNotValid = false;
                obj.Errors.Clear();
            }
        }

        private void SetValidationRule()
        {
            IdentityCode.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = NotEmptyMessenge + "mã số" });
            IdentityCode.Validations.Add(new ExpressionDangerousCharsUpdateRule<string>
            {
                DangerousChar = "['\"<>/&]",
                ValidationMessage = "Vui lòng không nhập [,',\",<,>,/, &,]"
            });

            RegistrationDate.Validations.Add(new EmptyDateTimeRule<DateTime> { ValidationMessage = NotEmptyMessenge + "ngày đăng kí" });

            ExpireDate.Validations.Add(new EmptyDateTimeRule<DateTime> { ValidationMessage = NotEmptyMessenge + "ngày hết hạn" });

            DaysNumberForAlertAppear.Validations.Add(new IntMinValueRule<string>() { ValidationMessage = NotEmptyMessenge + "số ngày cảnh báo trước", MinValue = 1 });

            Notes.Validations.Add(new ExpressionDangerousCharsUpdateRule<string>
            {
                DangerousChar = "['\"<>/&]",
                ValidationMessage = "Vui lòng không nhập [,',\",<,>,/, &,]"
            });
            Notes.Validations.Add(new MaxLengthRule<string>() {  MaxLenght=1000, ValidationMessage ="Không nhập quá 1000 kí tự"});
           
        }

        /// <summary>
        /// kiểm tra validation
        /// </summary>
        /// <returns>true : pass</returns>
        private bool Validate()
        {
            var insuranceNum = IdentityCode.Validate();
            var dateRegis = RegistrationDate.Validate();
            var dateExp = ExpireDate.Validate();
            var dayPrepareAlert = DaysNumberForAlertAppear.Validate();
            var money = RegistrationFee.Validate();
            var departmentUnit = UnitName.Validate();
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
                var oldExpireDay = oldInfor.PaperInfo.ExpireDate;
                if (RegistrationDate.Value <= oldExpireDay)
                {
                    RegistrationDate.IsNotValid = true;
                    RegistrationDate.ErrorFirst = "Ngày đăng kí mới > ngày hết hạn giấy tờ gần nhất";
                    oldExpireDayRule = false;
                }
            }

            return (insuranceNum && dateRegis && dateExp && dayPrepareAlert
                && money && departmentUnit && note && newRule && outDateRule
                && oldExpireDayRule);
        }

        private void SaveRegistrationInfor()
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
                        var res = await paperinforService.UpdateRegistrationPaper(data);
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
                        var res = await paperinforService.InsertRegistrationPaper(data);
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

        private void SelectRegisterDate()
        {
            SafeExecute(async () =>
            {
                var parameters = new NavigationParameters();
                var day = RegistrationDate.Value == new DateTime() ? DateTime.Now : RegistrationDate.Value;
                parameters.Add("PickerType", (short)ComboboxType.First);
                parameters.Add("DataPicker", day);
                await NavigationService.NavigateAsync("SelectDatePicker", parameters);
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
                await NavigationService.NavigateAsync("SelectDatePicker", parameters);
            });
        }
        private PaperRegistrationInsertRequest oldInfor { get; set; }
        private void UpdateFormData(int companyId, long vehicleId)
        {
            CreateButtonVisible = false;
            SafeExecute(async () =>
            {
                var paper = await paperinforService.GetLastPaperRegistrationByVehicleId(companyId, vehicleId);
                if (paper != null)
                {

                    oldInfor = paper;
                    IsUpdateForm = true;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        IdentityCode.Value = paper.PaperInfo.PaperNumber;
                        RegistrationDate.Value = paper.PaperInfo.DateOfIssue;
                        ExpireDate.Value = paper.PaperInfo.ExpireDate;
                        DaysNumberForAlertAppear.Value = paper.PaperInfo.DayOfAlertBefore.ToString();
                        Notes.Value = paper.PaperInfo.Description;
                        UnitName.Value = paper.WarrantyCompany;
                        RegistrationFee.Value = paper.Cost?.ToString("G0");

                        SaveEnable = false;
                    });
                    if (DateTime.Now.Date > paper.PaperInfo.ExpireDate.Date)
                    {
                        AlertMessenger = string.Format("<font color=#E65353>{0}</font>", MobileResource.PaperInfor_Msg_Expired);
                        CreateButtonVisible = true;
                    }
                    else if (paper.PaperInfo.ExpireDate.Date.AddDays(-CompanyConfigurationHelper.DayAllowRegister) <= DateTime.Now.Date)
                    {
                        AlertMessenger = string.Format("<font color=#F99B09>{0}</font>", MobileResource.PaperInfor_Msg_NearExpire);
                        CreateButtonVisible = true;
                    }
                }
                else ClearData();
            });
        }

        private void ClearData()
        {
            IdentityCode.Value = string.Empty;
            RegistrationDate.Value = DateTime.Now;
            ExpireDate.Value = DateTime.Now;
            DaysNumberForAlertAppear.Value = "3";
            RegistrationFee.Value = string.Empty;
            Notes.Value = string.Empty;
            UnitName.Value = string.Empty;
            CreateButtonVisible = false;
        }

        private void ChangeToInsertForm()
        {
            IdentityCode.Value = string.Empty;
            ExpireDate.Value = DateTime.Now;
            DaysNumberForAlertAppear.Value = "3";
            RegistrationFee.Value = string.Empty;
            Notes.Value = string.Empty;
            UnitName.Value = string.Empty;
            CreateButtonVisible = false;

            RegistrationDate.Value = oldInfor.PaperInfo.ExpireDate.AddDays(1);
            CreateButtonVisible = false;
            IsUpdateForm = false;
        }

        private PaperRegistrationInsertRequest GetFormData()
        {
            var res = new PaperRegistrationInsertRequest();
            res.PaperInfo = new PaperBasicInfor()
            {
                PaperNumber = IdentityCode.Value,
                DateOfIssue = RegistrationDate.Value,
                ExpireDate = ExpireDate.Value,
                DayOfAlertBefore = Convert.ToInt32(DaysNumberForAlertAppear.Value),
                Description = Notes.Value
            };     
            if (!string.IsNullOrEmpty(RegistrationFee.Value))
            {
                res.Cost = Convert.ToDecimal(RegistrationFee.Value);
            }
            res.WarrantyCompany = UnitName.Value;

            return res;

        }
    }
}
