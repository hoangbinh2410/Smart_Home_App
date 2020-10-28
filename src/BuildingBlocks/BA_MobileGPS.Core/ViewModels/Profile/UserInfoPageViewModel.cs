using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;

using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Forms.Extensions;
using XamStorage;

namespace BA_MobileGPS.Core.ViewModels
{
    public class UserInfoPageViewModel : ViewModelBase
    {
        private readonly ICategoryService categoryService;
        private readonly IUserService userService;

        public string DateFormat => CultureInfo.CurrentCulture.TwoLetterISOLanguageName.Equals("vi") ? "dd/MM/yyyy" : "MM/dd/yyyy";

        private UserInfoRespone currentUser = new UserInfoRespone();
        public UserInfoRespone CurrentUser { get => currentUser; set => SetProperty(ref currentUser, value); }

        private ObservableCollection<Gender> listGender;
        public ObservableCollection<Gender> ListGender { get => listGender; set => SetProperty(ref listGender, value); }

        private Gender selectedGender;
        public Gender SelectedGender { get => selectedGender; set => SetProperty(ref selectedGender, value, onChanged: OnGenderChanged); }

        private ObservableCollection<Religion> listReligion;
        public ObservableCollection<Religion> ListReligion { get => listReligion; set => SetProperty(ref listReligion, value); }

        private Religion selectedReligion;
        public Religion SelectedReligion { get => selectedReligion; set => SetProperty(ref selectedReligion, value, onChanged: OnReligionChanged); }
        private bool isShowPhoneNumber;
        public bool IsShowPhoneNumber { get => isShowPhoneNumber; set => SetProperty(ref isShowPhoneNumber, value); }
        public ICommand AvatarTappedCommand { get; private set; }
        public ICommand UpdateUserInfoCommand { get; private set; }

        public UserInfoPageViewModel(INavigationService navigationService, ICategoryService categoryService, IUserService userService) : base(navigationService)
        {
            this.categoryService = categoryService;
            this.userService = userService;
            AvatarTappedCommand = new DelegateCommand(AvatarTapped);
            UpdateUserInfoCommand = new DelegateCommand(UpdateUserInfo);
            isShowPhoneNumber = MobileUserSettingHelper.IsShowPhoneNumber;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters?.GetValue<string>(ParameterKey.ImageLocation) is string imageLocation)
            {
                CurrentUser.AvatarPathLocal = imageLocation;
            }
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            GetUserInfo();
        }

        private void GetListGender()
        {
            Task.Run(async () =>
            {
                return await categoryService.GetListGender(CultureInfo.CurrentCulture.Name);
            }).ContinueWith(task => Device.BeginInvokeOnMainThread(() =>
            {
                if (task.Status == TaskStatus.RanToCompletion)
                {
                    ListGender = task.Result.ToObservableCollection();
                    ListGender.Insert(0, new Gender()
                    {
                        ConfigID = 0,
                        ValueByLanguage = MobileResource.Common_Value_SelectGender
                    });
                    SelectedGender = ListGender.ToList().Find(g => g.ConfigID == CurrentUser.GenderId);
                }
                else if (task.IsFaulted)
                {
                    Logger.WriteError(task.Exception);
                }
            }));
        }

        private void GetListReligion()
        {
            Task.Run(async () =>
            {
                return await categoryService.GetListReligion(CultureInfo.CurrentCulture.Name);
            }).ContinueWith(task => Device.BeginInvokeOnMainThread(() =>
            {
                if (task.Status == TaskStatus.RanToCompletion)
                {
                    ListReligion = task.Result.ToObservableCollection();
                    ListReligion.Insert(0, new Religion()
                    {
                        ConfigID = 0,
                        ValueByLanguage = MobileResource.Common_Value_SelectReligion
                    });
                    SelectedReligion = ListReligion.ToList().Find(re => re.ConfigID == CurrentUser.ReligionId);
                }
                else if (task.IsFaulted)
                {
                    Logger.WriteError(task.Exception);
                }
            }));
        }

        private void GetUserInfo()
        {
            Task.Run(async () =>
            {
                return await userService.GetUserInfomation(UserInfo.UserId);
            }).ContinueWith(task => Device.BeginInvokeOnMainThread(() =>
            {
                if (task.Status == TaskStatus.RanToCompletion)
                {
                    if (task.Result != null)
                    {
                        CurrentUser = task.Result;

                        StaticSettings.User.AvatarUrl = CurrentUser.AvatarUrl;

                        GetListGender();
                        GetListReligion();
                    }
                    else
                    {
                        DisplayMessage.ShowMessageInfo("Có lỗi sảy ra xin vui lòng thử lại");
                    }
                }
                else if (task.IsFaulted)
                {
                    Logger.WriteError(task.Exception);
                }
            }));
        }

        private async void AvatarTapped()
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
                        DefaultCamera = CameraDevice.Front,
                        Directory = "BAGPS",
                        Name = "avatar.jpg",
                        PhotoSize = PhotoSize.MaxWidthHeight,
                        MaxWidthHeight = 1000,
                        CompressionQuality = 85,
                        RotateImage = true,
                        SaveToAlbum = false
                    });

                    if (fileAvatar == null)
                        return;

                    CurrentUser.AvatarPathLocal = fileAvatar.Path;

                    ProcessImage();

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

                     CurrentUser.AvatarPathLocal = fileAvatar.Path;

                     ProcessImage();

                     fileAvatar.Dispose();
                 });
            });
        }

        private async void ProcessImage()
        {
            var @params = new NavigationParameters
            {
                { "ImagePath", CurrentUser.AvatarPathLocal }
            };

            await NavigationService.NavigateAsync("BaseNavigationPage/ImageEditorPage", @params, true);
        }

        private void OnGenderChanged()
        {
            if (SelectedGender != null)
            {
                CurrentUser.GenderId = SelectedGender.ConfigID;
            }
        }

        private void OnReligionChanged()
        {
            if (SelectedReligion != null)
            {
                CurrentUser.ReligionId = SelectedReligion.ConfigID;
            }
        }

        #region Validate

        private bool ValidateInputs()
        {
            return ValidateDateOfBirth() && ValidateFullName() && ValidatePhoneNumber() && ValidateEmail() && ValidateCareer() && ValidateRole() && ValidateAddress() && ValidateFacebook();
        }

        private string dateOfBirthErrorMessage;
        public string DateOfBirthErrorMessage { get => dateOfBirthErrorMessage; set => SetProperty(ref dateOfBirthErrorMessage, value); }

        private bool dateOfBirthHasError;
        public bool DateOfBirthHasError { get => dateOfBirthHasError; set => SetProperty(ref dateOfBirthHasError, value); }

        private bool ValidateDateOfBirth()
        {
            DateOfBirthErrorMessage = null;
            DateOfBirthHasError = true;

            if (CurrentUser.DateOfBirth != null)
            {
                if (DateTime.Compare(DateTime.Parse(CurrentUser.DateOfBirth.ToString()), DateTime.MinValue) < 0 || DateTime.Compare(DateTime.Parse(CurrentUser.DateOfBirth.ToString()), DateTime.Now.Date) > 0)
                {
                    DateOfBirthErrorMessage = MobileResource.Common_Property_Invalid(MobileResource.UserInfo_Label_DayOfBirth);
                    return false;
                }
            }

            DateOfBirthHasError = false;

            return true;
        }

        private string fullNameErrorMessage;
        public string FullNameErrorMessage { get => fullNameErrorMessage; set => SetProperty(ref fullNameErrorMessage, value); }

        private bool fullNameHasError;
        public bool FullNameHasError { get => fullNameHasError; set => SetProperty(ref fullNameHasError, value); }

        private bool ValidateFullName()
        {
            FullNameErrorMessage = null;
            FullNameHasError = true;

            if (string.IsNullOrWhiteSpace(CurrentUser.FullName))
            {
                FullNameErrorMessage = MobileResource.Common_Property_NullOrEmpty(MobileResource.UserInfo_Label_FullName);
                return false;
            }
            if (CurrentUser.FullName?.Length < 3)
            {
                FullNameErrorMessage = MobileResource.Common_Property_MinLength(MobileResource.UserInfo_Label_FullName);
                return false;
            }
            if (StringHelper.HasDangerousChars(CurrentUser.FullName))
            {
                FullNameErrorMessage = MobileResource.Common_Property_DangerousChars(MobileResource.UserInfo_Label_FullName);
                return false;
            }

            FullNameHasError = false;

            return true;
        }

        private string phoneNumberErrorMessage;
        public string PhoneNumberErrorMessage { get => phoneNumberErrorMessage; set => SetProperty(ref phoneNumberErrorMessage, value); }

        private bool phoneNumberHasError;
        public bool PhoneNumberHasError { get => phoneNumberHasError; set => SetProperty(ref phoneNumberHasError, value); }

        private bool ValidatePhoneNumber()
        {
            PhoneNumberErrorMessage = null;
            PhoneNumberHasError = true;

            if (string.IsNullOrWhiteSpace(CurrentUser.PhoneNumber))
            {
                PhoneNumberErrorMessage = MobileResource.Common_Property_NullOrEmpty(MobileResource.UserInfo_Label_PhoneNumber);
                return false;
            }
            if (!StringHelper.ValidPhoneNumer(CurrentUser.PhoneNumber, MobileSettingHelper.LengthAndPrefixNumberPhone))
            {
                PhoneNumberErrorMessage = MobileResource.Common_Property_Invalid(MobileResource.UserInfo_Label_PhoneNumber);

                return false;
            }

            PhoneNumberHasError = false;

            return true;
        }

        private string emailErrorMessage;
        public string EmailErrorMessage { get => emailErrorMessage; set => SetProperty(ref emailErrorMessage, value); }

        private bool emailHasError;
        public bool EmailHasError { get => emailHasError; set => SetProperty(ref emailHasError, value); }

        private bool ValidateEmail()
        {
            EmailErrorMessage = null;
            EmailHasError = true;

            if (string.IsNullOrWhiteSpace(CurrentUser.Email))
            {
                return true;
            }
            if (StringHelper.HasDangerousChars(CurrentUser.Email))
            {
                EmailErrorMessage = MobileResource.Common_Property_DangerousChars(MobileResource.UserInfo_Label_Email);
                return false;
            }
            if (!StringHelper.ValidateEmail(CurrentUser.Email))
            {
                EmailErrorMessage = MobileResource.Common_Property_Invalid(MobileResource.UserInfo_Label_Email);
                return false;
            }

            EmailHasError = false;

            return true;
        }

        private string careerErrorMessage;
        public string CareerErrorMessage { get => careerErrorMessage; set => SetProperty(ref careerErrorMessage, value); }

        private bool careerHasError;
        public bool CareerHasError { get => careerHasError; set => SetProperty(ref careerHasError, value); }

        private bool ValidateCareer()
        {
            CareerErrorMessage = null;
            CareerHasError = true;

            if (StringHelper.HasDangerousChars(CurrentUser.Career))
            {
                CareerErrorMessage = MobileResource.Common_Property_DangerousChars(MobileResource.UserInfo_Label_Career);
                return false;
            }

            CareerHasError = false;

            return true;
        }

        private string roleErrorMessage;
        public string RoleErrorMessage { get => roleErrorMessage; set => SetProperty(ref roleErrorMessage, value); }

        private bool roleHasError;
        public bool RoleHasError { get => roleHasError; set => SetProperty(ref roleHasError, value); }

        private bool ValidateRole()
        {
            RoleErrorMessage = null;
            RoleHasError = true;

            if (StringHelper.HasDangerousChars(CurrentUser.Role))
            {
                RoleErrorMessage = MobileResource.Common_Property_DangerousChars(MobileResource.UserInfo_Label_Role);
                return false;
            }

            RoleHasError = false;

            return true;
        }

        private string addressErrorMessage;
        public string AddressErrorMessage { get => addressErrorMessage; set => SetProperty(ref addressErrorMessage, value); }

        private bool addressHasError;
        public bool AddressHasError { get => addressHasError; set => SetProperty(ref addressHasError, value); }

        private bool ValidateAddress()
        {
            AddressErrorMessage = null;
            AddressHasError = true;

            if (!StringHelper.ValidateAddress(CurrentUser.Address))
            {
                AddressErrorMessage = MobileResource.Common_Property_NotContainChars(MobileResource.UserInfo_Label_Address, "',\",<,>,&");
                return false;
            }

            AddressHasError = false;

            return true;
        }

        private string facebookErrorMessage;
        public string FacebookErrorMessage { get => facebookErrorMessage; set => SetProperty(ref facebookErrorMessage, value); }

        private bool facebookHasError;
        public bool FacebookHasError { get => facebookHasError; set => SetProperty(ref facebookHasError, value); }

        private bool ValidateFacebook()
        {
            FacebookErrorMessage = null;
            FacebookHasError = true;

            if (StringHelper.HasDangerousChars(CurrentUser.Facebook))
            {
                FacebookErrorMessage = MobileResource.Common_Property_DangerousChars(MobileResource.UserInfo_Label_Facebook);
                return false;
            }

            FacebookHasError = false;

            return true;
        }

        #endregion Validate

        private void UpdateUserInfo()
        {
            if (!ValidateInputs() || !IsConnected || IsBusy)
                return;

            IsBusy = true;
            DependencyService.Get<IHUDProvider>().DisplayProgress("");

            Task.Run(async () =>
            {
                IFile file = null;
                string avatarUrl = null;

                if (!string.IsNullOrWhiteSpace(CurrentUser.AvatarPathLocal))
                {
                    file = await FileSystem.Current.GetFileFromPathAsync(CurrentUser.AvatarPathLocal);

                    using (Stream stream = await file.OpenAsync(XamStorage.FileAccess.Read))
                    {
                        avatarUrl = await userService.UpdateUserAvatar(CurrentUser.UserName, stream, file.Name);
                    }
                }

                var request = new UpdateUserInfoRequest
                {
                    UserID = UserInfo.UserId,
                    AvatarUrl = avatarUrl ?? CurrentUser.AvatarUrl,
                    FullName = CurrentUser.FullName?.Trim(),
                    PhoneNumber = CurrentUser.PhoneNumber?.Trim(),
                    Email = CurrentUser.Email?.Trim(),
                    DateOfBirth = CurrentUser.DateOfBirth,
                    GenderId = CurrentUser.GenderId,
                    Career = CurrentUser.Career?.Trim(),
                    Role = CurrentUser.Role?.Trim(),
                    ReligionId = CurrentUser.ReligionId,
                    Address = CurrentUser.Address?.Trim(),
                    Facebook = CurrentUser.Facebook?.Trim()
                };

                var result = await userService.UpdateUserInfo(request);

                if (result && avatarUrl != null)
                {
                    StaticSettings.User.AvatarUrl = avatarUrl;

                    file?.DeleteAsync();
                }

                return result;
            }).ContinueWith(task => Device.BeginInvokeOnMainThread(async () =>
            {
                IsBusy = false;
                DependencyService.Get<IHUDProvider>().Dismiss();

                if (task.Status == TaskStatus.RanToCompletion)
                {
                    if (task.Result)
                    {
                        if (StaticSettings.User != null)
                        {
                            StaticSettings.User.UserName = CurrentUser.UserName?.Trim();
                            StaticSettings.User.FullName = CurrentUser.FullName?.Trim();
                            StaticSettings.User.PhoneNumber = CurrentUser.PhoneNumber?.Trim();
                            await NavigationService.GoBackAsync(useModalNavigation: true);
                        }
                    }
                }
                else if (task.IsFaulted)
                {
                    DisplayMessage.ShowMessageInfo(MobileResource.Common_Message_ErrorTryAgain);
                    Logger.WriteError(task.Exception);
                }
            }));
        }
    }
}