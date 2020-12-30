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
using BA_MobileGPS.Core.Resources;
using Plugin.Media;
using Xamarin.Forms;
using BA_MobileGPS.Core.Helpers;
using Plugin.Media.Abstractions;
using XamStorage;
using System.IO;
using BA_MobileGPS.Utilities.Constant;
using System.Reflection;

namespace BA_MobileGPS.Core.ViewModels
{
    public class UpdateDriverInforPageViewModel : ViewModelBase
    {
        private readonly IDriverInforService driverInforService;
        private readonly IUserService userService;
        private const string NotEmptyMessenge = "Vui lòng nhập ";
        private const string NotSelectMessenger = "Vui lòng chọn ";
        private const string updateTitle = "Xem thông tin lái xe";
        private const string addTitle = "Nhập thông tin lái xe";
        private const string successTitle = "Thêm lái xe thành công";
        private const string licenseRank = "Bằng lái xe hạng ";

        public ICommand SaveDriverInforCommand { get; }

        public ICommand ContinueInsertCommand { get; }
        public ICommand ChangeDriverAvtarCommand { get; }
        public ICommand PushToFromDatePageCommand { get; }
        private bool IsInsertpage = true;
        public UpdateDriverInforPageViewModel(INavigationService navigationService, IDriverInforService driverInforService,
            IUserService userService) : base(navigationService)
        {
            this.driverInforService = driverInforService;
            this.userService = userService;
            SaveDriverInforCommand = new DelegateCommand(SaveDriverInfor);
            ContinueInsertCommand = new DelegateCommand(ContinueInsert);
            ChangeDriverAvtarCommand = new DelegateCommand(ChangeDriverAvtar);
            PushToFromDatePageCommand = new DelegateCommand<object>(ExecuteToFromDate);
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
                AvartarDisplay = imageLocation;
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
                PageTitle = updateTitle;
            }
            else
            {
                // Form thêm mới
                Driver = new DriverInfor();
                Driver.FK_CompanyID = UserInfo.CompanyId;
                PageTitle = addTitle;
            }
            SetData(Driver);
        }

        private string newAvatarPath { get; set; } = string.Empty;

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
            set
            {
                SetProperty(ref issueDate, value);

            }
        }
        private DateTime expiredDate;
        public DateTime ExpiredDate
        {
            get { return expiredDate; }
            set
            {
                SetProperty(ref expiredDate, value);
                ExpiredDateHasErr = false;
            }
        }

        private bool expiredDateHasErr;
        public bool ExpiredDateHasErr
        {
            get { return expiredDateHasErr; }
            set { SetProperty(ref expiredDateHasErr, value); }
        }

        public string OverDateErrMessenger { get; set; } = "Ngày hết hạn phải lớn hơn ngày cấp";

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
        public string SelectLicenseTypeErrorMessage { get; set; } = string.Format("{0}{1}",NotSelectMessenger,"loại bằng lái");
        public string SelectGenderErrorMessage { get; set; } = string.Format("{0}{1}", NotSelectMessenger, "giới tính");

        public string DateFormat => CultureInfo.CurrentCulture.TwoLetterISOLanguageName.Equals("vi") ? "dd/MM/yyyy" : "MM/dd/yyyy";
        public List<string> ListDriverLicenseType { get; set; }
        public List<string> ListGender { get; set; }

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
        }

        private void SaveDriver()
        {
            RunOnBackground(async () =>
            {
                GetData();
                var res = await driverInforService.AddDriverInfor(Driver);
                return res;
            }, res =>
            {
                //success khi id trả về  > 0
                if (IsInsertpage && res > 0)
                {
                    // add success
                    ShowSuccessScreen = true;
                    PageTitle = successTitle;
                    // clear data
                    Driver = new DriverInfor();
                    Driver.FK_CompanyID = UserInfo.CompanyId;
                    SetData(Driver);

                }
                else if (!IsInsertpage && res == Driver.PK_EmployeeID)
                {
                    NavigationService.GoBackAsync(null, true, true);
                }
                else PageDialog.DisplayActionSheetAsync("Thông báo", string.Format("Thất bại {0}", res), "OK");
            });
        }

        private void ContinueInsert()
        {
            ShowSuccessScreen = false;
            PageTitle = addTitle;
            Driver = new DriverInfor();
            Driver.FK_CompanyID = UserInfo.CompanyId;
            SetData(Driver);
        }


        private bool Validate()
        {
            var res = DisplayName.Validate() && Address.Validate()
              && Mobile.Validate() && IdentityNumber.Validate()
              && DriverLicense.Validate();
            if (!res)
            {
                return false;
            }

            if (IssueDate >= ExpiredDate)
            {
                ExpiredDateHasErr = true;
                return false;
            }

            if (SelectedLicenseType < 1)
            {
                SelectLicenseTypeHasError = true;
                return false;
            }

            if (SelectedGender == 0)
            {
                SelectGenderHasError = true;
                return false;
            }

            return true;

        }


        private void SetLicenseTypeSource()
        {
            var list = new List<string>
            {
                "Chọn loại bằng lái"
            };
            var temp = Enum.GetNames(typeof(DriverLicenseEnum)).Select(b=> string.Format("{0}{1}",licenseRank,b)).ToList();
            list.AddRange(temp);
            ListDriverLicenseType = list;
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
                Driver.ExpireLicenseDate = expiredDate;
                Driver.Birthday = birthDay;
                Driver.IssueLicenseDate = issueDate;
                Driver.Sex = (byte)(SelectedGender - 1);
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }

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
            BirthDay = (DateTime)driver.Birthday;
            IssueDate = (DateTime)driver.IssueLicenseDate;
            ExpiredDate = (DateTime)driver.ExpireLicenseDate;
            AvartarDisplay = string.IsNullOrEmpty(driver.DriverImage) ? "avatar_default.png" : $"{ServerConfig.ApiEndpoint}{driver.DriverImage}";
            newAvatarPath = string.Empty;
            if (driver.Sex == null)
            {
                SelectedGender = 0;
            }
            else SelectedGender = (byte)(driver.Sex + 1);
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

            DisplayName.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = NotEmptyMessenge + "họ tên" });
            DisplayName.Validations.Add(new ExpressionDangerousCharsRule<string> { Expression = "['\"<>/&]", 
                ValidationMessage = MobileResource.Common_Property_DangerousChars("Họ tên") });

            Address.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = NotEmptyMessenge+ "địa chỉ" });
            Address.Validations.Add(new ExpressionDangerousCharsRule<string>
            {
                Expression = "['\"<>/&]",
                ValidationMessage = MobileResource.Common_Property_DangerousChars("Địa chỉ")
            });
           // Address.Validations.Add(new MaxLengthRule<string> { ValidationMessage = "Không nhập quá 150 kí tự", MaxLenght = 150 });

            Mobile.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = NotEmptyMessenge +"số điện thoại" });
            Mobile.Validations.Add(new PhoneNumberRule<string> { ValidationMessage = "Số điện thoại không hợp lệ", 
                CountryCode = CountryCodeConstant.VietNam });

            IdentityNumber.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = NotEmptyMessenge +"CMND" });
            IdentityNumber.Validations.Add(new ExpressionDangerousCharsRule<string>
            {
                Expression = "['\"<>/&]",
                ValidationMessage = MobileResource.Common_Property_DangerousChars("CMND")
            });
           // IdentityNumber.Validations.Add(new MaxLengthRule<string> { ValidationMessage = "Không nhập quá 15 kí tự", MaxLenght = 15 });

            DriverLicense.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = NotEmptyMessenge +"bằng lái"});
            DriverLicense.Validations.Add(new MinLenghtRule<string> { ValidationMessage = "Đúng 12 kí tự", MinLenght = 12 });
            DriverLicense.Validations.Add(new MaxLengthRule<string> { ValidationMessage = "Đúng 12 kí tự", MaxLenght = 12 });
            DriverLicense.Validations.Add(new ExpressionDangerousCharsRule<string>
            {
                Expression = "['\"<>/&]",
                ValidationMessage = MobileResource.Common_Property_DangerousChars("Bằng lái")
            });
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
            var @params = new NavigationParameters
            {
                { "ImagePath", imagePath }
            };

            await NavigationService.NavigateAsync("BaseNavigationPage/ImageEditorPage", @params, true, true);
        }

        private void SetGenderSource()
        {
            ListGender = new List<string>();
            ListGender.Add("Chọn giới tính");
            ListGender.Add("Nam");
            ListGender.Add("Nữ");
            ListGender.Add("Khác");
            SelectedGender = 0;
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
                        parameters.Add("PickerType", ComboboxType.First);
                        parameters.Add("DataPicker", BirthDay);
                        break;
                    case "IssueDate":
                        parameters.Add("PickerType", ComboboxType.Second);
                        parameters.Add("DataPicker", IssueDate);
                        break;
                    case "ExpDate":
                        parameters.Add("PickerType", ComboboxType.Third);
                        parameters.Add("DataPicker", ExpiredDate);
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
                            BirthDay = obj.Value;
                            break;
                        case (short)ComboboxType.Second:
                            IssueDate = obj.Value;
                            break;
                        case (short)ComboboxType.Third:
                            ExpiredDate = obj.Value;
                            break;
                    }
                });
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }

        }


    }
}
