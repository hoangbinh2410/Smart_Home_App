using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.ResponeEntity.Expense;
using BA_MobileGPS.Entities.ResponeEntity.Report.Station;
using BA_MobileGPS.Service;
using BA_MobileGPS.Service.Report.Station;
using BA_MobileGPS.Utilities.Extensions;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private IExpenseService _ExpenseService { get; set; }
        private IStationLocationService _StationLocation { get; set; }
        private readonly IDisplayMessage _displayMessage;

        public ImportExpensePageViewModel(INavigationService navigationService, IPageDialogService PageDialog, IExpenseService ExpenseService, IDisplayMessage displayMessage, IStationLocationService StationLocation) : base(navigationService)
        {
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
            Title = "Nhập chi phí";
        }

        #endregion Contructor

        #region Lifecycle

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            GetListExpenseCategory();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey("MenuExpense") && parameters.GetValue<List<ListExpenseCategoryByCompanyRespone>>("MenuExpense") is List<ListExpenseCategoryByCompanyRespone> ListExpense)
            {
                ListExpenseCategory = ListExpense;
                if (parameters.ContainsKey(ParameterKey.Vehicle) && parameters.GetValue<Vehicle>(ParameterKey.Vehicle) is Vehicle para && parameters.ContainsKey("DayChoose") && parameters.GetValue<DateTime>("DayChoose") is DateTime date)
                {
                    VehicleID = para.VehicleId;
                    Expensedate = date;
                    if (parameters.ContainsKey("ImportExpense") && parameters.GetValue<ExpenseDetailsRespone>("ImportExpense") is ExpenseDetailsRespone obj)
                    {
                        ExpenseDetail = obj;
                        GetViewPage(obj, ListExpense);
                    }
                    else
                    {
                        IButtonView = false;
                    }
                }
                else
                {
                    _displayMessage.ShowMessageInfo(MobileResource.Common_ConnectInternet_Error);
                }
                if (parameters?.GetValue<string>(ParameterKey.ImageLocation) is string imageLocation)
                {
                }
            }
            else
            {
                return;
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

        public ExpenseDetailsRespone expenseDetail = new ExpenseDetailsRespone();

        public ExpenseDetailsRespone ExpenseDetail { get { return expenseDetail; } set { SetProperty(ref expenseDetail, value); } }
        public bool IInsertExpense;
        private ListExpenseCategoryByCompanyRespone selected = new ListExpenseCategoryByCompanyRespone();

        public ListExpenseCategoryByCompanyRespone SelectedExpense
        { get { return selected; } set { SetProperty(ref selected, value); } }

        private List<LocationStationResponse> listPlace = new List<LocationStationResponse>();
        public List<LocationStationResponse> ListPlace { get { return listPlace; } set { SetProperty(ref listPlace, value); } }
        private List<ListExpenseCategoryByCompanyRespone> listExpenseCategory = new List<ListExpenseCategoryByCompanyRespone>();

        public List<ListExpenseCategoryByCompanyRespone> ListExpenseCategory
        { get { return listExpenseCategory; } set { SetProperty(ref listExpenseCategory, value); } }

        public enum DataItem
        {
            [Description("Địa điểm khác")]
            Place = 1,

            [Description("bg_Account.png")]
            Image = 2
        }

        private LoginResponse userInfo;
        public LoginResponse UserInfo { get { if (StaticSettings.User != null) { userInfo = StaticSettings.User; } return userInfo; } set => SetProperty(ref userInfo, value); }
        private decimal priceExpense = 0;

        public decimal PriceExpense
        { get { return priceExpense; } set { SetProperty(ref priceExpense, value); } }

        private string note = String.Empty;

        public string Note
        { get { return note; } set { SetProperty(ref note, value); } }

        private string otheraddress = String.Empty;

        public string Otheraddress
        { get { return otheraddress; } set { SetProperty(ref otheraddress, value); } }

        private LocationStationResponse _selectedLocation = new LocationStationResponse();

        public LocationStationResponse SelectedLocation
        {
            get { return _selectedLocation; }
            set { SetProperty(ref _selectedLocation, value); }
        }

        private bool iHasOtherPlace = false;

        public bool IHasOtherPlace

        { get { return iHasOtherPlace; } set { SetProperty(ref iHasOtherPlace, value); } }

        private bool iButtonView = true;

        public bool IButtonView

        { get { return iButtonView; } set { SetProperty(ref iButtonView, value); } }

        private int? landmarkID = null;

        public int? LandmarkID

        { get { return landmarkID; } set { SetProperty(ref landmarkID, value); } }

        private long vehicleID;

        public long VehicleID

        { get { return vehicleID; } set { SetProperty(ref vehicleID, value); } }

        private DateTime expensedate;

        public DateTime Expensedate

        { get { return expensedate; } set { SetProperty(ref expensedate, value); } }

        private string imagePathLocal = DataItem.Image.ToDescription();
        public string ImagePathLocal { get => imagePathLocal; set => SetProperty(ref imagePathLocal, value /*,nameof(AvatarDisplay)*/); }

        public ImageSource AvatarDisplay => string.IsNullOrWhiteSpace(ImagePathLocal) ? "bg_Account.png" : /*$"{ServerConfig.ApiEndpoint}{AvatarUrl}") :*/ ImageSource.FromFile(ImagePathLocal);

        #endregion Property

        #region PrivateMethod

        // Chọn phí
        private void SelectExpense()
        {
            if (!SelectedExpense.HasLandmark)
            {
                Otheraddress = string.Empty;
                LandmarkID = null;
            }
        }

        // Chọn địa điểm
        private void SelectPlace()
        {
            LandmarkID = SelectedLocation.PK_LandmarkID;
            if (SelectedLocation.Address == DataItem.Place.ToDescription())
            {
                IHasOtherPlace = true;
                Otheraddress = ExpenseDetail.OtherAddress;
            }
            else
            {
                IHasOtherPlace = false;
                Otheraddress = string.Empty;
            }
        }

        //chọn ảnh
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

        // Hiển thị khi có para
        private void GetViewPage(ExpenseDetailsRespone obj, List<ListExpenseCategoryByCompanyRespone> ListExpense)
        {
            SelectedExpense = ListExpense.Where(x => x.Name == obj.Name).FirstOrDefault();
            PriceExpense = obj.ExpenseCost;
            if (!string.IsNullOrEmpty(obj.OtherAddress))
            {
                Otheraddress = obj.OtherAddress;
                IHasOtherPlace = true;
            }
            SelectedLocation = ListPlace.Where(x => x.Name == obj.LandmarkName).ToList().FirstOrDefault();

            LandmarkID = SelectedLocation.PK_LandmarkID;
        }

        // danh sách phí, danh sách địa điểm trong combobox
        private void GetListExpenseCategory()
        {
            SafeExecute(async () =>
            {
                var _listPlace = await _StationLocation.GetListLocationStation(UserInfo.CompanyId);
                _listPlace.Add(new LocationStationResponse { Address = DataItem.Place.ToDescription() });
                ListPlace = _listPlace;
            });
        }

        // reset ảnh
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
                ImagePathLocal = DataItem.Image.ToDescription();
            }
        }

        // lưu phí
        private void SaveExpense()
        {
            SafeExecute(async () =>
            {
                if (!string.IsNullOrEmpty(SelectedExpense.Name))
                {
                    var RequestExpense = new ImportExpenseRequest()
                    {
                        Id = ExpenseDetail.ID,
                        Photo = ImagePathLocal,
                        OtherAddress = Otheraddress,
                        ExpenseCost = PriceExpense,
                        FK_CompanyID = UserInfo.CompanyId,
                        ExpenseDate = Expensedate,
                        Note = Note,
                        FK_ExpenseCategoryID = SelectedExpense.ID,
                        FK_LandmarkID = LandmarkID,
                        FK_VehicleID = VehicleID,
                    };
                    IInsertExpense = await _ExpenseService.GetExpense(RequestExpense);
                    if (IInsertExpense)
                    {
                        await Application.Current.MainPage.DisplayAlert("Thành công", "Lưu thành công", MobileResource.Common_Button_OK);
                        await NavigationService.GoBackAsync();
                    }
                    else
                    {
                        _displayMessage.ShowMessageInfo(MobileResource.Common_ConnectInternet_Error);
                    }
                }
                else
                {
                    _displayMessage.ShowMessageWarning("Vui lòng chọn loại chi phí");
                }
            });
        }
        // Tiếp tục lưu phí
        private void SaveAndCountinute()
        {
            SafeExecute(async () =>
            {
                if (!string.IsNullOrEmpty(SelectedExpense.Name))
                {
                    var RequestExpense = new ImportExpenseRequest()
                    {
                        Id = ExpenseDetail.ID,
                        Photo = ImagePathLocal,
                        OtherAddress = Otheraddress,
                        ExpenseCost = PriceExpense,
                        FK_CompanyID = UserInfo.CompanyId,
                        ExpenseDate = Expensedate,
                        Note = Note,
                        FK_ExpenseCategoryID = SelectedExpense.ID,
                        FK_LandmarkID = LandmarkID,
                        FK_VehicleID = VehicleID,
                    };
                    IInsertExpense = await _ExpenseService.GetExpense(RequestExpense);
                    if (IInsertExpense)
                    {
                        await Application.Current.MainPage.DisplayAlert("Thành công", "Lưu thành công", MobileResource.Common_Button_OK);                       
                        Note = string.Empty;                       
                        PriceExpense = 0;
                        SelectedExpense = null;
                        Otheraddress = string.Empty;
                        IHasOtherPlace = false;
                        SelectedLocation = null;
                        ImagePathLocal = DataItem.Image.ToDescription();
                    }
                    else
                    {
                        _displayMessage.ShowMessageInfo(MobileResource.Common_ConnectInternet_Error);
                    }
                }
                else
                {
                    _displayMessage.ShowMessageWarning("Vui lòng chọn loại chi phí");
                }
            });
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
                        Name = DataItem.Image.ToDescription(),
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