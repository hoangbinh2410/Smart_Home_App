using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms;
using XamStorage;

namespace BA_MobileGPS.Core.ViewModels
{
    public class InsertOrUpdateDriverPageViewModel : ViewModelBase
    {
        private readonly IDriverInforService driverInforService;
        private readonly IUserService userService;
        private string NotEmptyMessenge = MobileResource.ListDriver_Messenger_NotNull;

        public ICommand SaveDriverInforCommand { get; }
        public ICommand ContinueInsertCommand { get; }
        public ICommand ChangeDriverAvtarCommand { get; }
        public ICommand PushToFromDatePageCommand { get; }
        public ICommand GoBackCommand { get; }
        private bool IsInsertpage = true;

        public InsertOrUpdateDriverPageViewModel(INavigationService navigationService, IDriverInforService driverInforService,
            IUserService userService) : base(navigationService)
        {
            this.driverInforService = driverInforService;
            this.userService = userService;
            SaveDriverInforCommand = new DelegateCommand(SaveDriverInfor);
            ContinueInsertCommand = new DelegateCommand(ContinueInsert);
            ChangeDriverAvtarCommand = new DelegateCommand(ChangeDriverAvtar);
            PushToFromDatePageCommand = new DelegateCommand<object>(ExecuteToFromDate);
            GoBackCommand = new DelegateCommand(GoBack);
            SetLicenseTypeSource();
            SetGenderSource();
            InitValidations();
            showSuccessScreen = false;
            EventAggregator.GetEvent<SelectDateEvent>().Subscribe(UpdateDateTime);
        }

        public override void OnDestroy()
        {
            EventAggregator.GetEvent<SelectDateEvent>().Unsubscribe(UpdateDateTime);
            base.OnDestroy();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters?.GetValue<string>(ParameterKey.ImageLocation) is string imageLocation)
            {
                newAvatarPath = imageLocation;
            }
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            if (parameters.TryGetValue(ParameterKey.DriverInformation, out DriverInfor driver))
            {
                // Form Edit
                Driver = driver;
                IsInsertpage = false;
                PageTitle = MobileResource.ListDriver_Title_Update;
            }
            else
            {
                // Form thêm mới
                Driver = new DriverInfor();
                Driver.FK_CompanyID = UserInfo.CompanyId;
                PageTitle = MobileResource.ListDriver_Title_Insert;
            }
            SetData(Driver);
        }

        private string newAvatarPath { get; set; } = string.Empty;
        #region Binding Property
        private string avartarDisplay;

        public string AvartarDisplay
        {
            get { return avartarDisplay; }
            set { SetProperty(ref avartarDisplay, value); }
        }

        private DriverInfor driver;

        public DriverInfor Driver
        {
            get { return driver; }
            set { SetProperty(ref driver, value); }
        }

        private ValidatableObject<DateTime> birthDay;

        public ValidatableObject<DateTime> BirthDay
        {
            get { return birthDay; }
            set { SetProperty(ref birthDay, value); }
        }

        private ValidatableObject<DateTime> issueDate;

        public ValidatableObject<DateTime> IssueDate
        {
            get { return issueDate; }
            set
            {
                SetProperty(ref issueDate, value);
            }
        }

        private ValidatableObject<DateTime> expiredDate;

        public ValidatableObject<DateTime> ExpiredDate
        {
            get { return expiredDate; }
            set
            {
                SetProperty(ref expiredDate, value);
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
            set
            {
                SetProperty(ref selectedLicenseType, value);
                SelectLicenseTypeHasError = false;
            }
        }

        private byte selectedGender;

        public byte SelectedGender
        {
            get { return selectedGender; }
            set
            {
                SetProperty(ref selectedGender, value);
                SelectGenderHasError = false;
            }
        }

        private bool showSuccessScreen;

        public bool ShowSuccessScreen
        {
            get { return showSuccessScreen; }
            set { SetProperty(ref showSuccessScreen, value); }
        }

        private string pageTitle;

        public string PageTitle
        {
            get { return pageTitle; }
            set { SetProperty(ref pageTitle, value); }
        }

        private bool selectLicenseTypeHasError;

        public bool SelectLicenseTypeHasError
        {
            get { return selectLicenseTypeHasError; }
            set
            {
                SetProperty(ref selectLicenseTypeHasError, value);
            }
        }

        private bool selectGenderHasError;

        public bool SelectGenderHasError
        {
            get { return selectGenderHasError; }
            set { SetProperty(ref selectGenderHasError, value); }
        }

        public string SelectLicenseTypeErrorMessage { get; set; } = string.Format("{0}{1}",
            MobileResource.ListDriver_Messenger_NotSelect, MobileResource.ListDriver_Messenger_LicenseType);

        public string SelectGenderErrorMessage { get; set; } = string.Format("{0}{1}",
            MobileResource.ListDriver_Messenger_NotSelect, MobileResource.ListDriver_Messenger_Gender);

        public string DateFormat => CultureInfo.CurrentCulture.TwoLetterISOLanguageName.Equals("vi") ? "dd/MM/yyyy" : "MM/dd/yyyy";
        public List<string> ListDriverLicenseType { get; set; }
        public List<string> ListGender { get; set; }
        #endregion

        #region function Save
        private void SaveNewAvartar()
        {
            IFile file = null;

            RunOnBackground(async () =>
            {
                file = await FileSystem.Current.GetFileFromPathAsync(newAvatarPath);
                using (Stream stream = await file.OpenAsync(XamStorage.FileAccess.Read))
                {
                    return await userService.UpdateUserAvatar("DriverAvatar", stream, file.Name);
                }
            }, res =>
            {
                if (res != null)
                {
                    Driver.DriverImage = res;
                }
                SaveDriver();
                file.DeleteAsync();
            });
        }

        private void SaveDriverInfor()
        {
            if (Validate())
            {
                if (!string.IsNullOrEmpty(newAvatarPath))
                {
                    SaveNewAvartar();
                }
                else SaveDriver();
            }
            else PageDialog.DisplayAlertAsync(MobileResource.Common_Label_Notification,
                MobileResource.Common_Message_ErrorTryAgain,
                MobileResource.Common_Button_OK);
        }

        private void SaveDriver()
        {
            SafeExecute(() =>
            {
                RunOnBackground(async () =>
                {
                    GetData();
                    var res = await driverInforService.AddDriverInfor(Driver);
                    return res;
                }, res =>
                {
                    var param = new NavigationParameters();
                    param.Add("RefreshData", true);
                    //success khi id trả về  > 0
                    if (IsInsertpage && res > 0)
                    {
                        // add success
                        ShowSuccessScreen = true;
                        PageTitle = MobileResource.ListDriver_Title_Success;
                        // clear data
                        Driver = new DriverInfor();
                        Driver.FK_CompanyID = UserInfo.CompanyId;
                        SetData(Driver);                      
                    }
                    else if (!IsInsertpage && res == Driver.PK_EmployeeID)
                    {                      
                        NavigationService.GoBackAsync(param, true, true);
                    }
                    else PageDialog.DisplayAlertAsync(MobileResource.Common_Label_Notification,
                        MobileResource.ListDriver_Messenger_DuplicateData,
                       MobileResource.Common_Button_OK);
                });
            });
        }

        private void ContinueInsert()
        {
            ShowSuccessScreen = false;
            PageTitle = MobileResource.ListDriver_Title_Insert;
            Driver = new DriverInfor();
            Driver.FK_CompanyID = UserInfo.CompanyId;
            SetData(Driver);
        }
        
        private void GetData()
        {
            try
            {
                Driver.DisplayName = displayName.Value;
                Driver.Address = address.Value;
                Driver.Mobile = mobile.Value;
                Driver.IdentityNumber = identityNumber.Value;
                Driver.DriverLicense = driverLicense.Value;
                Driver.LicenseType = selectedLicenseType;
                Driver.ExpireLicenseDate = expiredDate.Value;
                Driver.Birthday = birthDay.Value;
                Driver.IssueLicenseDate = issueDate.Value;
                Driver.Sex = (byte)(SelectedGender - 1);
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }
        #endregion
        
        #region Validation
        private bool Validate()
        {
            var name = DisplayName.Validate();
            var address = Address.Validate();
            var mobile = Mobile.Validate();
            var cmt = IdentityNumber.Validate();
            var driverLicense = DriverLicense.Validate();
            var birthday = BirthDay.Validate();
            var issuedate = IssueDate.Validate();
            var expriDate = ExpiredDate.Validate();

            var overdate = true;
            if (IssueDate.Value >= ExpiredDate.Value)
            {
                ExpiredDate.IsNotValid = true;
                ExpiredDate.ErrorFirst = MobileResource.ListDriver_Notify_DateGreater;
                overdate = false;
            }
            var licenseType = true;
            if (SelectedLicenseType < 1)
            {
                SelectLicenseTypeHasError = true;
                licenseType = false;
            }
            var gender = true;
            if (SelectedGender == 0)
            {
                SelectGenderHasError = true;
                gender = false;
            }

            return name && address && mobile && cmt && overdate && licenseType && gender
                && driverLicense && birthday && issuedate && expriDate;
        }
        private void InitValidations()
        {
            DisplayName = new ValidatableObject<string>();
            DisplayName.OnChanged += DisplayName_OnChanged;
            Address = new ValidatableObject<string>();
            Address.OnChanged += Address_OnChanged;
            Mobile = new ValidatableObject<string>();
            Mobile.OnChanged += Mobile_OnChanged;
            IdentityNumber = new ValidatableObject<string>();
            IdentityNumber.OnChanged += IdentityNumber_OnChanged;
            DriverLicense = new ValidatableObject<string>();
            DriverLicense.OnChanged += DriverLicense_OnChanged;
            BirthDay = new ValidatableObject<DateTime>();
            BirthDay.OnChanged += BirthDay_OnChanged; ;
            IssueDate = new ValidatableObject<DateTime>();
            IssueDate.OnChanged += IssueDate_OnChanged; ;
            ExpiredDate = new ValidatableObject<DateTime>();
            ExpiredDate.OnChanged += ExpiredDate_OnChanged;

            DisplayName.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = NotEmptyMessenge + "họ tên" });
            DisplayName.Validations.Add(new ExpressionDangerousCharsUpdateRule<string>
            {
                DangerousChar = "['\"<>/&]",
                ValidationMessage = "Vui lòng nhập họ tên hợp lệ"
            });

            Address.Validations.Add(new ExpressionDangerousCharsUpdateRule<string>
            {
                DangerousChar = "['\"<>/&]",
                ValidationMessage = "Vui lòng nhập đia chỉ hợp lệ"
            });

            Mobile.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = NotEmptyMessenge + "số điện thoại" });
            Mobile.Validations.Add(new PhoneNumberRule<string>
            {
                ValidationMessage = "Số điện thoại không hợp lệ",
                CountryCode = CountryCodeConstant.VietNam
            });

            IdentityNumber.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = NotEmptyMessenge + "CMND" });
            IdentityNumber.Validations.Add(new ExpressionDangerousCharsUpdateRule<string>
            {
                DangerousChar = "['\"<>/&]",
                ValidationMessage = "Vui lòng nhập CMND hợp lệ"
            });

            DriverLicense.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = NotEmptyMessenge + "bằng lái" });
            DriverLicense.Validations.Add(new MinLenghtRule<string> { ValidationMessage = "Vui lòng nhập số bằng lái xe hợp lệ", MinLenght = 12 });
            DriverLicense.Validations.Add(new ExpressionDangerousCharsUpdateRule<string>
            {
                DangerousChar = "['\"<>/&]",
                ValidationMessage = "Vui lòng nhập bằng lái hợp lệ"
            });

            BirthDay.Validations.Add(new EmptyDateTimeRule<DateTime> { ValidationMessage = NotEmptyMessenge + "ngày sinh" }); ;
            IssueDate.Validations.Add(new EmptyDateTimeRule<DateTime> { ValidationMessage = NotEmptyMessenge + "ngày cấp bằng" });
            ExpiredDate.Validations.Add(new EmptyDateTimeRule<DateTime> { ValidationMessage = NotEmptyMessenge + "ngày hết hạn bằng" });
        }

        private void ExpiredDate_OnChanged(object sender, DateTime e)
        {
            if (ExpiredDate.IsNotValid)
            {
                ExpiredDate.IsNotValid = false;
            }
        }

        private void IssueDate_OnChanged(object sender, DateTime e)
        {
            if (IssueDate.IsNotValid)
            {
                IssueDate.IsNotValid = false;
            }
        }

        private void BirthDay_OnChanged(object sender, DateTime e)
        {
            if (BirthDay.IsNotValid)
            {
                BirthDay.IsNotValid = false;
            }
        }

        private void DriverLicense_OnChanged(object sender, string e)
        {
            if (DriverLicense.IsNotValid)
            {
                DriverLicense.IsNotValid = false;
            }
        }

        private void IdentityNumber_OnChanged(object sender, string e)
        {
            if (IdentityNumber.IsNotValid)
            {
                IdentityNumber.IsNotValid = false;
            }
        }

        private void Mobile_OnChanged(object sender, string e)
        {
            if (Mobile.IsNotValid)
            {
                Mobile.IsNotValid = false;
            }
        }

        private void Address_OnChanged(object sender, string e)
        {
            if (Address.IsNotValid)
            {
                Address.IsNotValid = false;
            }
        }

        private void DisplayName_OnChanged(object sender, string e)
        {
            if (DisplayName.IsNotValid)
            {
                DisplayName.IsNotValid = false;
            }
        }
        #endregion

        #region Avatar
        private async void ChangeDriverAvtar()
        {
            string result = await PageDialog.DisplayActionSheetAsync("", MobileResource.Common_Button_Cancel, null, MobileResource.Common_Message_TakeNewPhoto, MobileResource.Common_Message_ChooseAvailablePhotos);

            if (result == null)
                return;

            if (result.Equals(MobileResource.Common_Message_TakeNewPhoto))
            {
                TakeNewAvatar();
            }
            else if (result.Equals(MobileResource.Common_Message_ChooseAvailablePhotos))
            {
                PickAvatar();
            }
        }

        private void TakeNewAvatar()
        {
            TryExecute(async () =>
            {
                await CrossMedia.Current.Initialize();
                if (!CrossMedia.Current.IsCameraAvailable)
                {
                    await Application.Current.MainPage.DisplayAlert(MobileResource.Common_Label_CameraTitle, MobileResource.Common_Message_CameraUnavailable, MobileResource.Common_Button_OK);
                    return;
                }

                if (!CrossMedia.Current.IsTakePhotoSupported)
                {
                    await Application.Current.MainPage.DisplayAlert(MobileResource.Common_Label_CameraTitle, MobileResource.Common_Message_CameraUnsupported, MobileResource.Common_Button_OK);
                    return;
                }
                await PhotoHelper.CanTakePhoto(async () =>
                {
                    var fileAvatar = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                    {
                        DefaultCamera = CameraDevice.Rear,
                        Directory = "BAGPS",
                        Name = string.Format("{0}.jpg", DateTime.Now.ToString("yyyyMMddHHmmss")),
                        PhotoSize = PhotoSize.MaxWidthHeight,
                        MaxWidthHeight = 1000,
                        CompressionQuality = 85,
                        RotateImage = true,
                        SaveToAlbum = false
                    });

                    if (fileAvatar == null)
                        return;
                    ProcessImage(fileAvatar.Path);

                    fileAvatar.Dispose();
                });
            });
        }

        private void PickAvatar()
        {
            TryExecute(async () =>
            {
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await Application.Current.MainPage.DisplayAlert(MobileResource.Common_Message_PickPhotoUnsupported, MobileResource.Common_Message_PickPhotoPermissionNotGranted, MobileResource.Common_Button_OK);
                    return;
                }
                await PhotoHelper.CanPickPhoto(async () =>
                {
                    var fileAvatar = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                    {
                        PhotoSize = PhotoSize.MaxWidthHeight,
                        MaxWidthHeight = 1000,
                        CompressionQuality = 85,
                        RotateImage = true
                    });

                    if (fileAvatar == null)
                        return;

                    ProcessImage(fileAvatar.Path);

                    fileAvatar.Dispose();
                });
            });
        }
       
        private async void ProcessImage(string imagePath)
        {
            AvartarDisplay = imagePath;
            var @params = new NavigationParameters
            {
                { "ImagePath", imagePath }
            };

            await NavigationService.NavigateAsync("BaseNavigationPage/ImageEditorPage", @params, true, true);
        }
        #endregion
        private void SetLicenseTypeSource()
        {
            var list = new List<string>
            {
                MobileResource.ListDriver_Item_SelectLicenseType
            };
            var temp = Enum.GetNames(typeof(DriverLicenseEnum)).Select(b => string.Format("{0}{1}", MobileResource.ListDriver_Messenger_LicenseRank, b)).ToList();
            list.AddRange(temp);
            ListDriverLicenseType = list;
        }
        private void SetGenderSource()
        {
            ListGender = new List<string>();
            ListGender.Add(MobileResource.ListDriver_Item_SelectGender);
            ListGender.Add(MobileResource.ListDriver_Item_Male);
            ListGender.Add(MobileResource.ListDriver_Item_Female);
            ListGender.Add(MobileResource.ListDriver_Item_Other);
            SelectedGender = 0;
        }
        private void SetData(DriverInfor driver)
        {
            //reset data
            DisplayName.Value = driver.DisplayName;
            Address.Value = driver.Address;
            Mobile.Value = driver.Mobile;
            IdentityNumber.Value = driver.IdentityNumber;
            DriverLicense.Value = driver.DriverLicense;
            SelectedLicenseType = driver.LicenseType;
            if (driver.Birthday != null)
            {
                BirthDay.Value = (DateTime)driver.Birthday;
            }
            if (driver.IssueLicenseDate != null)
            {
                IssueDate.Value = (DateTime)driver.IssueLicenseDate;
            }
            if (driver.ExpireLicenseDate != null)
            {
                ExpiredDate.Value = (DateTime)driver.ExpireLicenseDate;
            }

            AvartarDisplay = string.IsNullOrEmpty(driver.DriverImage) ? "avatar_default.png" : $"{ServerConfig.ApiEndpoint}{driver.DriverImage}";
            newAvatarPath = string.Empty;
            if (driver.Sex == null)
            {
                SelectedGender = 0;
            }
            else SelectedGender = (byte)(driver.Sex + 1);
        }
        /// <summary>
        /// Mở popup chọn ngày
        /// </summary>
        public virtual void ExecuteToFromDate(object obj)
        {
            if (obj is string param)
            {
                var parameters = new NavigationParameters();
                switch (param)
                {
                    case "BirthDay":
                        var day = BirthDay.Value == new DateTime() ? DateTime.Now : BirthDay.Value;
                        parameters.Add("PickerType", ComboboxType.First);
                        parameters.Add("DataPicker", day);
                        break;

                    case "IssueDate":
                        var issueDay = IssueDate.Value == new DateTime() ? DateTime.Now : IssueDate.Value;
                        parameters.Add("PickerType", ComboboxType.Second);
                        parameters.Add("DataPicker", issueDay);
                        break;

                    case "ExpDate":
                        var expDay = ExpiredDate.Value == new DateTime() ? DateTime.Now : ExpiredDate.Value;
                        parameters.Add("PickerType", ComboboxType.Third);
                        parameters.Add("DataPicker", expDay);
                        break;
                }

                TryExecute(async () =>
                {
                    await NavigationService.NavigateAsync("SelectDateCalendar", parameters);
                });
            }
        }

        private void UpdateDateTime(PickerDateResponse obj)
        {
            try
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    switch (obj.PickerType)
                    {
                        case (short)ComboboxType.First:
                            BirthDay.Value = obj.Value;
                            break;

                        case (short)ComboboxType.Second:
                            IssueDate.Value = obj.Value;
                            break;

                        case (short)ComboboxType.Third:
                            ExpiredDate.Value = obj.Value;
                            break;
                    }
                });
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        private void GoBack()
        {
            var param = new NavigationParameters();
            param.Add("RefreshData", true);
            NavigationService.GoBackAsync(param, true, true);
        }
    }
}