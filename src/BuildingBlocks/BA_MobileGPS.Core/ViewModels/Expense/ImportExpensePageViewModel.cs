using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.ResponeEntity.Expense;
using BA_MobileGPS.Entities.ResponeEntity.Report.Station;
using BA_MobileGPS.Service;
using BA_MobileGPS.Service.Report.Station;
using BA_MobileGPS.Utilities;
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
using System.Reflection;
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
        public ICommand PushComboboxPlacePage { get; private set; }
        public ICommand PushComboboxExpensePage { get; private set; }
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
            PushComboboxPlacePage = new DelegateCommand(PushComboboxPlace);
            PushComboboxExpensePage = new DelegateCommand(PushComboboxExpensene);
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
        private ComboboxResponse statusSignalLossSelected;
        public ComboboxResponse StatusSignalLossSelected { get => statusSignalLossSelected; set => SetProperty(ref statusSignalLossSelected, value); }
        public ExpenseDetailsRespone expenseDetail = new ExpenseDetailsRespone();
        public ExpenseDetailsRespone ExpenseDetail { get { return expenseDetail; } set { SetProperty(ref expenseDetail, value); } }
        public bool IInsertExpense;

        private ListExpenseCategoryByCompanyRespone selected = new ListExpenseCategoryByCompanyRespone();
        public ListExpenseCategoryByCompanyRespone SelectedExpense { get { return selected; } set { SetProperty(ref selected, value); } }

        private List<LocationStationResponse> listPlace = new List<LocationStationResponse>();
        public List<LocationStationResponse> ListPlace { get { return listPlace; } set { SetProperty(ref listPlace, value); } }
        private List<ListExpenseCategoryByCompanyRespone> listExpenseCategory = new List<ListExpenseCategoryByCompanyRespone>();
        public List<ListExpenseCategoryByCompanyRespone> ListExpenseCategory { get { return listExpenseCategory; } set { SetProperty(ref listExpenseCategory, value); } }

        public enum DataItem
        {
            [Description("Địa điểm khác")]
            Place = 1,

            [Description("bg_Account.png")]
            Image = 2
        }       

        private LocationStationResponse _selectedLocation = new LocationStationResponse();

        public LocationStationResponse SelectedLocation { get { return _selectedLocation; }set { SetProperty(ref _selectedLocation, value); }}

        private bool iHasOtherPlace = false;

        public bool IHasOtherPlace { get { return iHasOtherPlace; } set { SetProperty(ref iHasOtherPlace, value); } }

        private bool iButtonView = true;

        public bool IButtonView { get { return iButtonView; } set { SetProperty(ref iButtonView, value); } }

        //private int? landmarkID = null;

        //public int? LandmarkID { get { return landmarkID; } set { SetProperty(ref landmarkID, value); } }

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
                ExpenseDetail.OtherAddress = string.Empty;
                SelectedLocation = new LocationStationResponse();
            }
        }
        // Chọn địa điểm
        private void SelectPlace()
        {
            //LandmarkID = SelectedLocation.PK_LandmarkID;
            if (SelectedLocation.Address == DataItem.Place.ToDescription())
            {
                IHasOtherPlace = true;
            }
            else
            {
                IHasOtherPlace = false;
                ExpenseDetail.OtherAddress = string.Empty;
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
                TakeNewImage();
            }
            else if (result.Equals(MobileResource.Common_Message_ChooseAvailablePhotos))
            {
                PickImage();
            }
        }

        // Hiển thị khi có para
        private void GetViewPage(ExpenseDetailsRespone obj, List<ListExpenseCategoryByCompanyRespone> ListExpense)
        {
            SelectedExpense = ListExpense.Where(x => x.Name == obj.Name).FirstOrDefault();
            //PriceExpense = obj.ExpenseCost.ToString("N");
            if (!string.IsNullOrEmpty(obj.OtherAddress)) 
            {

                IHasOtherPlace = true;
            }
            SelectedLocation = ListPlace.Where(x => x.Name == obj.LandmarkName).ToList().FirstOrDefault();

            //LandmarkID = SelectedLocation.PK_LandmarkID;
        }

        // danh sách phí, danh sách địa điểm trong combobox
        private void GetListExpenseCategory()
        {
            SafeExecute(async () =>
            {
                var _listPlace = await _StationLocation.GetListLocationStation(UserInfo.CompanyId);
                if (_listPlace == null && listPlace.Count < 0)
                {
                    _displayMessage.ShowMessageInfo(MobileResource.Common_ConnectInternet_Error);
                }
                else
                {
                    _listPlace.Add(new LocationStationResponse { Address = DataItem.Place.ToDescription() });
                    ListPlace = _listPlace;
                }               
            });
        }
        // reset ảnh
        private async void ResetImage()
        {
           bool result = await _PageDialog.DisplayAlertAsync("Cảnh báo", "Xóa ảnh", MobileResource.Common_Button_Yes, MobileResource.Common_Button_No);
       
            if (!result)
            {
                return;
            }
            else
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
                        OtherAddress = ExpenseDetail.OtherAddress,
                        ExpenseCost = ExpenseDetail.ExpenseCost,
                        FK_CompanyID = UserInfo.CompanyId,
                        ExpenseDate = Expensedate,
                        Note = ExpenseDetail.Note,
                        FK_ExpenseCategoryID = SelectedExpense.ID,
                        FK_LandmarkID = SelectedLocation.PK_LandmarkID,
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
                        OtherAddress = ExpenseDetail.OtherAddress,
                        ExpenseCost = ExpenseDetail.ExpenseCost,
                        //ExpenseCost = Convert.ToDecimal(PriceExpense)
                        FK_CompanyID = UserInfo.CompanyId,
                        ExpenseDate = Expensedate,
                        Note = ExpenseDetail.Note,
                        FK_ExpenseCategoryID = SelectedExpense.ID,
                        FK_LandmarkID = SelectedLocation.PK_LandmarkID,
                        FK_VehicleID = VehicleID,
                    };
                    IInsertExpense = await _ExpenseService.GetExpense(RequestExpense);
                    if (IInsertExpense)
                    {
                        await Application.Current.MainPage.DisplayAlert("Thành công", "Lưu thành công", MobileResource.Common_Button_OK);
                        SelectedLocation = null;
                        SelectedExpense = null;
                        ExpenseDetail = new ExpenseDetailsRespone();
                        //IHasOtherPlace = false;                        
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

        private void TakeNewImage()
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

        private void PickImage()
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
        public async void PushComboboxPlace()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;
            try
            {
                var p = new NavigationParameters
                {
                    { "dataCombobox", ListExpenseCategory },
                    { "ComboboxType", ComboboxType.Second },
                    { "Title", MobileResource.ReportSignalLoss_TitleStatus }
                };
                await NavigationService.NavigateAsync("BaseNavigationPage/ComboboxPage", p, useModalNavigation: true, true);
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        private List<ComboboxRequest> LoadAllExpense()
        {
            return new List<ComboboxRequest>() {
                    new ComboboxRequest(){Key = 0 , Value = MobileResource.ReportSignalLoss_TitleStatus_All},
                    new ComboboxRequest(){Key = 1 , Value = MobileResource.ReportSignalLoss_TitleStatus_GPS},
                    new ComboboxRequest(){Key = 2 , Value = MobileResource.ReportSignalLoss_TitleStatus_GMS},
                };
        }
        public async void PushComboboxExpensene()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;
            try
            {
                var p = new NavigationParameters
                {
                    { "dataCombobox", LoadAllExpense() },
                    { "ComboboxType", ComboboxType.First },
                    { "Title", MobileResource.ReportSignalLoss_TitleStatus }
                };
                await NavigationService.NavigateAsync("BaseNavigationPage/ComboboxPage", p, useModalNavigation: true, true);
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        public override void UpdateCombobox(ComboboxResponse param)
        {
            base.UpdateCombobox(param);
            if (param != null)
            {
                var dataResponse = param;
                if (dataResponse.ComboboxType == (Int16)ComboboxType.First)
                {
                    StatusSignalLossSelected = dataResponse;
                }
                if (dataResponse.ComboboxType == (Int16)ComboboxType.Second)
                {
                    StatusSignalLossSelected = dataResponse;
                }
            }
        }

        #endregion PrivateMethod
    }
}