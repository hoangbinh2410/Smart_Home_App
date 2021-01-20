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
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class CabSignInforViewModel : ViewModelBase
    {
        private bool IsUpdateForm { get; set; } = false;
        private string NotEmptyMessenge = MobileResource.ListDriver_Messenger_NotNull;
        private long currentVehicleId { get; set; } = 0;
        private readonly IPapersInforService paperinforService;
        public ICommand SaveCabSignInforCommand { get; }
        public ICommand SelectRegisterDateCommand { get; }
        public ICommand SelectExpireDateCommand { get; }
        public ICommand ChangeToInsertFormCommand { get; }
        public CabSignInforViewModel(INavigationService navigationService, IPapersInforService paperinforService) : base(navigationService)
        {
            this.paperinforService = paperinforService;
            SaveCabSignInforCommand = new DelegateCommand(SaveSignInfor);
            SelectRegisterDateCommand = new DelegateCommand(SelectRegisterDate);
            SelectExpireDateCommand = new DelegateCommand(SelectExpireDate);
            ChangeToInsertFormCommand = new DelegateCommand(ChangeToInsertForm);
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

        private bool createButtonVisible;
        public bool CreateButtonVisible
        {
            get { return createButtonVisible; }
            set
            {
                SetProperty(ref createButtonVisible, value);
                RaisePropertyChanged();
            }
        }

        private ValidatableObject<string> signNumber;

        public ValidatableObject<string> SignNumber
        {
            get { return signNumber; }
            set { SetProperty(ref signNumber, value); }
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
            set { SetProperty(ref alertMessenger, value); }
        }

        /// <summary>
        ///  Init validation rule cho các ô nhập dữ liệu
        /// </summary>
        private void InitValidations()
        {
            SignNumber = new ValidatableObject<string>();
            SignNumber.OnChanged += ValidationStringValue_OnChanged;
            RegistrationDate = new ValidatableObject<DateTime>();
            RegistrationDate.OnChanged += ValidationDateTimeValue_OnChanged;
            ExpireDate = new ValidatableObject<DateTime>();
            ExpireDate.OnChanged += ValidationDateTimeValue_OnChanged;
            DaysNumberForAlertAppear = new ValidatableObject<int>();
            DaysNumberForAlertAppear.OnChanged += ValidationIntValue_OnChanged;
            Notes = new ValidatableObject<string>();
            Notes.OnChanged += ValidationStringValue_OnChanged;

            SetValidationRule();
        }

        private void ValidationIntValue_OnChanged(object sender, int e)
        {
            // Check chon xe chua?
            if (currentVehicleId == 0 && ViewHasAppeared)
            {
                DisplayMessage.ShowMessageWarning("Vui lòng chọn xe trước khi nhập dữ liệu");
            }
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

            // Check chon xe chua?
            if (currentVehicleId == 0 && ViewHasAppeared)
            {
                DisplayMessage.ShowMessageWarning("Vui lòng chọn xe trước khi nhập dữ liệu");
            }
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
            // Check chon xe chua?
            if (currentVehicleId == 0 && ViewHasAppeared)
            {
                DisplayMessage.ShowMessageWarning("Vui lòng chọn xe trước khi nhập dữ liệu");
            }
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
            SignNumber.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = NotEmptyMessenge + "số phù hiệu" });
            SignNumber.Validations.Add(new ExpressionDangerousCharsUpdateRule<string>
            {
                DangerousChar = "['\"<>/&]",
                ValidationMessage = "Vui lòng nhập số bảo hiểm hợp lệ"
            });

            RegistrationDate.Validations.Add(new EmptyDateTimeRule<DateTime> { ValidationMessage = NotEmptyMessenge + "ngày đăng kí" });

            ExpireDate.Validations.Add(new EmptyDateTimeRule<DateTime> { ValidationMessage = NotEmptyMessenge + "ngày hết hạn" });

            DaysNumberForAlertAppear.Validations.Add(new IntMinValueRule<int>() { ValidationMessage = NotEmptyMessenge + "số ngày cảnh báo trước", MinValue = 1 });

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
            var insuranceNum = SignNumber.Validate();
            var dateRegis = RegistrationDate.Validate();
            var dateExp = ExpireDate.Validate();
            var dayPrepareAlert = DaysNumberForAlertAppear.Validate();
            var note = Notes.Validate();

            return (insuranceNum && dateRegis && dateExp && dayPrepareAlert
                 && note);
        }

        private void SaveSignInfor()
        {
            if (Validate())
            {
                var data = GetFormData();
                data.FK_CompanyID = UserInfo.CompanyId;
                data.FK_VehicleID = currentVehicleId;
                SafeExecute(async () =>
                {
                    if (IsUpdateForm)
                    {
                        data.UpdatedByUser = UserInfo.UserId;
                        data.Id = oldInfor.Id;
                        var res = await paperinforService.UpdateSignPaper(data);
                        if (res?.PK_PaperInfoID != new Guid())
                        {
                            DisplayMessage.ShowMessageSuccess("Cập nhật thông tin thành công");
                        }
                        else DisplayMessage.ShowMessageError("Cập nhật thông tin thất bại");
                    }
                    else
                    {

                        data.CreatedByUser = UserInfo.UserId;
                        var res = await paperinforService.InsertSignPaper(data);
                        if (res?.PK_PaperInfoID != new Guid())
                        {
                            DisplayMessage.ShowMessageSuccess("Thêm mới thông tin thành công");
                        }
                        else DisplayMessage.ShowMessageError("Thêm mới thông tin thất bại");

                    }
                });

            }
        }
        private PaperCabSignInforRequest oldInfor { get; set; }
        private void UpdateFormData(int companyId, long vehicleId)
        {
            CreateButtonVisible = false;
            SafeExecute(async () =>
            {
                var paper = await paperinforService.GetLastPaperSignByVehicleId(companyId, vehicleId);
                if (paper != null)
                {
                    oldInfor = paper;
                    IsUpdateForm = true;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        SignNumber.Value = paper.PaperNumber;
                        RegistrationDate.Value = paper.DateOfIssue;
                        ExpireDate.Value = paper.ExpireDate;
                        DaysNumberForAlertAppear.Value = paper.DayOfAlertBefore;
                        Notes.Value = paper.Description;
                    });
                    if (DateTime.Now > paper.ExpireDate)
                    {
                        AlertMessenger = string.Format("<font color=#E65353>{0}</font>", MobileResource.PaperInfor_Msg_Expired);
                        CreateButtonVisible = true;
                    }
                    else if (paper.ExpireDate.AddDays(-CompanyConfigurationHelper.DayAllowRegister) <= DateTime.Now)
                    {
                        AlertMessenger = string.Format("<font color=#FFF766>{0}</font>", MobileResource.PaperInfor_Msg_NearExpire);
                        CreateButtonVisible = true;
                    }
                }
                else ClearData();
            });
        }

        private void ClearData()
        {
            SignNumber.Value = string.Empty;
            RegistrationDate.Value = DateTime.MinValue;
            ExpireDate.Value = DateTime.MinValue;
            DaysNumberForAlertAppear.Value = 3;
            Notes.Value = string.Empty;

            CreateButtonVisible = false;
        }

        private void ChangeToInsertForm()
        {
            SignNumber.Value = string.Empty;
            ExpireDate.Value = DateTime.MinValue;
            DaysNumberForAlertAppear.Value = 3;
            Notes.Value = string.Empty;

            RegistrationDate.Value = oldInfor.ExpireDate.AddDays(1);
            CreateButtonVisible = false;
        }

        private PaperCabSignInforRequest GetFormData()
        {
            return new PaperCabSignInforRequest()
            {
                PaperNumber = SignNumber.Value,
                DateOfIssue = RegistrationDate.Value,
                ExpireDate = ExpireDate.Value,
                DayOfAlertBefore = DaysNumberForAlertAppear.Value,
                Description = Notes.Value
            };
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

    }
}
