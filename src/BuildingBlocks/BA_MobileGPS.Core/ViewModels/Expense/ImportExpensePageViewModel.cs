using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.ResponeEntity.Report.Station;
using BA_MobileGPS.Service;
using BA_MobileGPS.Service.Report.Station;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class ImportExpensePageViewModel : ViewModelBase
    {
        #region Contructor
        public ICommand BackPageCommand { get; private set; }
        public ICommand SelectExpenseCommand { get; private set; }
        public ICommand SelectPlaceCommand { get; private set; }
        public ICommand ChoseImageCommand { get; private set; }
        public ICommand ResetImageCommand { get; private set; }
        public ICommand SaveExpenseCommand { get; private set; }
        public ICommand SaveAndCountinuteCommand { get; private set; }
        protected IPageDialogService _PageDialog { get; private set; }
        private IExpenseService _ExpenseService { get;  set; }
        private IStationLocationService _StationLocation { get;  set; }
        private readonly IDisplayMessage _displayMessage;
        public ImportExpensePageViewModel(INavigationService navigationService, IPageDialogService PageDialog, IExpenseService ExpenseService, IDisplayMessage displayMessage, IStationLocationService StationLocation) : base(navigationService)
        {
            BackPageCommand = new DelegateCommand(ClosePage);
            SelectExpenseCommand = new DelegateCommand(SelectExpense);
            SelectPlaceCommand = new DelegateCommand(SelectPlace);
            ChoseImageCommand = new DelegateCommand(ChoseImage);
            ResetImageCommand = new DelegateCommand(ResetImage);
            SaveExpenseCommand = new DelegateCommand(SaveExpense);
            SaveAndCountinuteCommand = new DelegateCommand(SaveAndCountinute);
            _ExpenseService = ExpenseService;
            _PageDialog = PageDialog;
            _displayMessage = displayMessage;
            _StationLocation = StationLocation;
        }
        #endregion Contructor
        #region Lifecycle

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            GetListExpenseCategory();
            //GetListPlace();

        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters?.GetValue<string>(ParameterKey.ImageLocation) is string imageLocation)
            {
                ImagePathLocal = imageLocation;
            }          
        }

        public override void OnPageAppearingFirstTime()
        {
            base.OnPageAppearingFirstTime();
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
        }

        public override void OnDestroy()
        {
        }
        #endregion Lifecycle
        #region Property
        public ImportExpenseRespone ResponeExpense;
        //public ImportExpenseRequest RequestExpense = new ImportExpenseRequest();
        private List<LocationStationResponse> listPlace;
        private ListExpenseCategoryByCompanyRespone selected;
        public ListExpenseCategoryByCompanyRespone Selected
        { get { return selected; } set { SetProperty(ref selected, value); } }
        public List<LocationStationResponse> ListPlace { get { return listPlace; } set { SetProperty(ref listPlace, value); } }
        private List<ListExpenseCategoryByCompanyRespone> listExpenseCategory;
        public List<ListExpenseCategoryByCompanyRespone> ListExpenseCategory
        { get { return listExpenseCategory; } set { SetProperty(ref listExpenseCategory, value); } }

        private ListExpenseCategoryByCompanyRespone expenseCategory;
        public ListExpenseCategoryByCompanyRespone ExpenseCategory
        { get { return expenseCategory; } set { SetProperty(ref expenseCategory, value); } }
        private LoginResponse userInfo;
        public LoginResponse UserInfo { get { if (StaticSettings.User != null) { userInfo = StaticSettings.User; } return userInfo; } set => SetProperty(ref userInfo, value); }
        private decimal priceExpense = 0;
        public decimal PriceExpense
        { get { return priceExpense; } set { SetProperty(ref priceExpense, value); } }

        private string note = String.Empty;
        public string Note
        { get { return note; } set { SetProperty(ref note, value); } }

         private string textExpense = String.Empty;
        public string TextExpense
        { get { return textExpense; } set { SetProperty(ref textExpense, value); } }
        private string textplace = String.Empty;
        public string TextPlace
        { get { return textplace; } set { SetProperty(ref textplace, value); } }

        private LocationStationResponse _selectedLocation = new LocationStationResponse();
        public LocationStationResponse SelectedLocation
        {
            get { return _selectedLocation; }
            set { SetProperty(ref _selectedLocation, value); }
        }
        public bool iButtonView = false;
        public bool IButtonView

        { get { return iButtonView; } set { SetProperty(ref iButtonView, value); } }

        private string imagePathLocal = "bg_Account.png";
        public string ImagePathLocal { get => imagePathLocal; set => SetProperty(ref imagePathLocal, value /*,nameof(AvatarDisplay)*/); }

        public ImageSource AvatarDisplay => string.IsNullOrWhiteSpace(ImagePathLocal)  ? "bg_Account.png" : /*$"{ServerConfig.ApiEndpoint}{AvatarUrl}") :*/ ImageSource.FromFile(ImagePathLocal);
        #endregion Property

        #region PrivateMethod
        private void ClosePage()
        {
            SafeExecute(async () =>
            {             
                await NavigationService.GoBackToRootAsync(null);
            });
        }
        private void SelectExpense()
        {

        }
        private void SelectPlace()
        {

        }
        private async void ChoseImage()
        {
            string result = await _PageDialog.DisplayActionSheetAsync("", MobileResource.Common_Button_Cancel, null, MobileResource.Common_Message_TakeNewPhoto, MobileResource.Common_Message_ChooseAvailablePhotos);

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
        private void GetListExpenseCategory()
        {
            SafeExecute(async () =>
            {
                //ListExpenseCategory = await _ExpenseService.GetExpenseCategory(UserInfo.CompanyId);     
                ListExpenseCategory = await _ExpenseService.GetExpenseCategory(303);
                ListPlace = await _StationLocation.GetListLocationStation(303);
            });
        }
        //private void GetListPlace()
        //{
        //    SafeExecute(async () =>
        //    {
        //        //ListExpenseCategory = await _stationLocation.GetListLocationStation(UserInfo.CompanyId);     
               
        //    });
        //}
        private async void ResetImage()
        {
            string result = await _PageDialog.DisplayActionSheetAsync("", MobileResource.Common_Button_Cancel, null, MobileResource.Common_Message_TakeNewPhoto, MobileResource.Common_Message_ChooseAvailablePhotos);

            if (result == null)
                return;

            if (result.Equals(MobileResource.Common_Message_TakeNewPhoto))
            {
                return;
            }
            else if (result.Equals(MobileResource.Common_Message_ChooseAvailablePhotos))
            {
                ImagePathLocal = "bg_Account.png";
            }         
        }
        private void SaveExpense()
        {
            ImportExpenseRequest RequestExpense = new ImportExpenseRequest()
            {
                Photo = ImagePathLocal,
                OtherAddress = TextPlace,
                ExpenseCost = PriceExpense,
                FK_CompanyID = 303,
                ExpenseDate = DateTime.Now,
                Note = Note,
                FK_ExpenseCategoryID = Selected.ID,
                FK_LandmarkID = SelectedLocation.PK_LandmarkID,
                FK_VehicleID = 1,              
            };
            SafeExecute(async () =>
            {              
                if (TextExpense != "" && TextPlace != "" && Note!= "")
                {
                    ResponeExpense = await _ExpenseService.GetExpense(RequestExpense);
                    if (ResponeExpense.Data == true && ResponeExpense != null)
                    {
                        await Application.Current.MainPage.DisplayAlert(MobileResource.Common_Message_PickPhotoUnsupported, MobileResource.Common_Message_PickPhotoPermissionNotGranted, MobileResource.Common_Button_OK);
                        await NavigationService.GoBackToRootAsync(null);
                    }
                    else
                    {
                        _displayMessage.ShowMessageInfo(MobileResource.Common_ConnectInternet_Error);
                    }
                }
                else
                {
                    _displayMessage.ShowMessageWarning("Nhập đầy đủ thông tin");
                }
            });
        }
        private void SaveAndCountinute()
        {
            TextExpense = "";
            TextPlace = "";
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
                        Name = "bg_Account.png",
                        PhotoSize = PhotoSize.MaxWidthHeight,
                        MaxWidthHeight = 1000,
                        CompressionQuality = 85,
                        RotateImage = true,
                        SaveToAlbum = false
                    });

                    if (fileAvatar == null)
                        return;

                    ImagePathLocal = fileAvatar.Path;

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

                    ImagePathLocal = fileAvatar.Path;

                    ProcessImage();

                    fileAvatar.Dispose();
                });
            });
        }
        private async void ProcessImage()
        {
            var @params = new NavigationParameters
            {
                { "ImagePath", ImagePathLocal }
            };

            await NavigationService.NavigateAsync("BaseNavigationPage/ImageEditorPage", @params, true, true);
        }
        #endregion PrivateMethod

    }
}
