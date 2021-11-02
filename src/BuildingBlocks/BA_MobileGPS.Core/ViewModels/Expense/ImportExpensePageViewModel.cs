using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
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
        public ImportExpensePageViewModel(INavigationService navigationService, IPageDialogService PageDialog, IExpenseService ExpenseService) : base(navigationService)
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
        }
        #endregion Contructor
        #region Lifecycle

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);          
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
        private List<ListExpenseCategoryByCompanyRespone> listExpense;
        public List<ListExpenseCategoryByCompanyRespone> ListExpense
        { get { return listExpense; } set { SetProperty(ref listExpense, value); } }
        //private string expense = "Chọn loại phí";
        //public string Expense
        //{ get { return expense; } set { SetProperty(ref expense, value); } }

        private int priceExpense = 0;
        public int PriceExpense
        { get { return priceExpense; } set { SetProperty(ref priceExpense, value); } }

        private string note = String.Empty;
        public string Note
        { get { return note; } set { SetProperty(ref note, value); } }

        private string place ="Chọn địa điểm";
        public string Place
        { get { return place; } set { SetProperty(ref place, value); } }

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
        private void GetListExpense()
        {

        }
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

        }
        private void SaveAndCountinute()
        {

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
