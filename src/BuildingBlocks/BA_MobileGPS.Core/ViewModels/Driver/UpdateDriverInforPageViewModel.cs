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

namespace BA_MobileGPS.Core.ViewModels
{
    public class UpdateDriverInforPageViewModel : ViewModelBase
    {
        private readonly IDriverInforService driverInforService;
        private readonly IUserService userService;
        private const string NotEmptyMessenge = "Đây là trường bắt buộc";
        private const string updateTitle = "Xem thông tin lái xe";
        private const string addTitle = "Nhập thông tin lái xe";
        private const string successTitle = "Thêm lái xe thành công";
        //private readonly string MinDateMessenger = "Ngày quá bé";
        public ICommand SaveDriverInforCommand { get; }
        public ICommand ContinueInsertCommand { get; }
        public ICommand ChangeDriverAvtarCommand { get; }
        private bool IsInsertpage = true;
        public UpdateDriverInforPageViewModel(INavigationService navigationService, IDriverInforService driverInforService,
            IUserService userService) : base(navigationService)
        {
            this.driverInforService = driverInforService;
            this.userService = userService;
            SaveDriverInforCommand = new DelegateCommand(SaveDriverInfor);
            ContinueInsertCommand = new DelegateCommand(ContinueInsert);
            ChangeDriverAvtarCommand = new DelegateCommand(ChangeDriverAvtar);
            SetLicenseTypeSource();
            SetGenderSource();
            InitValidations();
            showSuccessScreen = false;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters?.GetValue<string>(ParameterKey.ImageLocation) is string imageLocation)
            {
                newAvatarPath = imageLocation;
                AvartarDisplay = imageLocation;
            }
            else
            {
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
            set { SetProperty(ref selectedGender, value);
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
        public string SelectLicenseTypeErrorMessage { get; set; } = NotEmptyMessenge;
        public string SelectGenderErrorMessage { get; set; } = NotEmptyMessenge;

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

            }, res => {
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
            });
        }

        private void ContinueInsert()
        {
            ShowSuccessScreen = false;
            PageTitle = addTitle;
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

            if (IssueDate.Value >= ExpiredDate.Value)
            {
                ExpiredDate.IsNotValid = true;
                ExpiredDate.ErrorFirst = "Ngày hết hạn phải lớn hơn ngày cấp";
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
            var temp = Enum.GetNames(typeof(DriverLicenseEnum)).ToList();
            list.AddRange(temp);
            ListDriverLicenseType = list;
        }

        private void GetData()
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
            Driver.Sex = SelectedGender;
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
            BirthDay.Value = driver.Birthday == null ? DateTime.Now.Date : (DateTime)driver.Birthday;
            IssueDate.Value = driver.IssueLicenseDate == null ? DateTime.Now.Date.AddDays(-1) : (DateTime)driver.IssueLicenseDate;
            ExpiredDate.Value = driver.ExpireLicenseDate == null ? DateTime.Now.Date : (DateTime)driver.ExpireLicenseDate;
            AvartarDisplay = string.IsNullOrEmpty(driver.DriverImage) ? "avatar_default.png" : $"{ServerConfig.ApiEndpoint}{driver.DriverImage}" ;
            newAvatarPath = string.Empty;
        }

        private void InitValidations()
        {
            DisplayName = new ValidatableObject<string>();
            Address = new ValidatableObject<string>();
            Mobile = new ValidatableObject<string>();
            IdentityNumber = new ValidatableObject<string>();
            DriverLicense = new ValidatableObject<string>();
            BirthDay = new ValidatableObject<DateTime>();
            IssueDate = new ValidatableObject<DateTime>();
            ExpiredDate = new ValidatableObject<DateTime>();

            DisplayName.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = NotEmptyMessenge });

            Address.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = NotEmptyMessenge });

            Mobile.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = NotEmptyMessenge });
            Mobile.Validations.Add(new PhoneNumberRule<string> { ValidationMessage = "Số không chính xác" });

            IdentityNumber.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = NotEmptyMessenge });

            DriverLicense.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = NotEmptyMessenge });
            DriverLicense.Validations.Add(new MinLenghtRule<string> { ValidationMessage = "Đúng 12 kí tự", MinLenght = 12 });
            DriverLicense.Validations.Add(new MaxLengthRule<string> { ValidationMessage = "Đúng 12 kí tự", MaxLenght = 12 });

            BirthDay.Validations.Add(new IsNotNullOrEmptyRule<DateTime> { ValidationMessage = NotEmptyMessenge });
            IssueDate.Validations.Add(new IsNotNullOrEmptyRule<DateTime> { ValidationMessage = NotEmptyMessenge });
            ExpiredDate.Validations.Add(new IsNotNullOrEmptyRule<DateTime> { ValidationMessage = NotEmptyMessenge });
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
            SelectedGender = 0;
        }
    }
}
