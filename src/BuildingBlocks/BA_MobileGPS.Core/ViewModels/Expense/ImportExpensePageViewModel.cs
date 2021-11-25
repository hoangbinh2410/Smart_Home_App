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
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class ImportExpensePageViewModel : ViewModelBase
    {
        #region Contructor

        public ICommand ChoseImageCommand { get; private set; }
        public ICommand ChangeExpenseCommand { get; private set; }
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
            ChoseImageCommand = new DelegateCommand(ChoseImage);
            ResetImageCommand = new DelegateCommand(ResetImage);
            SaveExpenseCommand = new DelegateCommand(SaveExpense);
            ChangeExpenseCommand = new DelegateCommand(ChangeExpense);
            SaveAndCountinuteCommand = new DelegateCommand(SaveAndCountinute);
            PushComboboxPlacePage = new DelegateCommand(PushComboboxPlace);
            PushComboboxExpensePage = new DelegateCommand(PushComboboxExpensene);
            EventAggregator.GetEvent<SelectComboboxEvent>().Subscribe(UpdateDataCombobox);
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
            if (parameters.ContainsKey("MenuExpense") && parameters.GetValue<List<ListExpenseCategoryByCompanyRespone>>("MenuExpense") is List<ListExpenseCategoryByCompanyRespone> ListExpense)
            {
                ListExpenseCategory = ListExpense;
                if (parameters.ContainsKey("ImportExpense") && parameters.GetValue<ExpenseDetailsRespone>("ImportExpense") is ExpenseDetailsRespone obj)
                {
                    ExpenseDetail = obj;
                    //if (!string.IsNullOrEmpty(obj.Photo))
                    //{
                    //    IsHasImg = false;
                    //}
                    GetListExpenseCategory(obj, ListExpense);
                }
                else
                {
                    _displayMessage.ShowMessageInfo(MobileResource.Common_ConnectInternet_Error);
                }
            }
            else
            {
                _displayMessage.ShowMessageInfo(MobileResource.Common_ConnectInternet_Error);
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters?.GetValue<string>(ParameterKey.ImageLocation) is string imageLocation)
            {
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
            EventAggregator.GetEvent<SelectComboboxEvent>().Unsubscribe(UpdateDataCombobox);
        }

        #endregion Lifecycle

        #region Property

        private ComboboxResponse statusSignalLossSelected;
        public ComboboxResponse StatusSignalLossSelected { get => statusSignalLossSelected; set => SetProperty(ref statusSignalLossSelected, value); }
        public ExpenseDetailsRespone expenseDetail = new ExpenseDetailsRespone();

        public ExpenseDetailsRespone ExpenseDetail
        { get { return expenseDetail; } set { SetProperty(ref expenseDetail, value); } }

        public bool IInsertExpense;

        private ListExpenseCategoryByCompanyRespone selected = new ListExpenseCategoryByCompanyRespone();

        public ListExpenseCategoryByCompanyRespone SelectedExpense
        { get { return selected; } set { SetProperty(ref selected, value); } }

        private List<LocationStationResponse> listPlace = new List<LocationStationResponse>();

        public List<LocationStationResponse> ListPlace
        { get { return listPlace; } set { SetProperty(ref listPlace, value); } }

        private List<ListExpenseCategoryByCompanyRespone> listExpenseCategory = new List<ListExpenseCategoryByCompanyRespone>();

        public List<ListExpenseCategoryByCompanyRespone> ListExpenseCategory
        { get { return listExpenseCategory; } set { SetProperty(ref listExpenseCategory, value); } }

        private ComboboxResponse _selectedExpenseName = new ComboboxResponse();

        public ComboboxResponse SelectedExpenseName { get => _selectedExpenseName; set => SetProperty(ref _selectedExpenseName, value); }

        private List<ComboboxRequest> _listExpenseName = new List<ComboboxRequest>();
        public List<ComboboxRequest> ListExpenseName
        { get { return _listExpenseName; } set { SetProperty(ref _listExpenseName, value); } }

        private List<ComboboxRequest> _listPlaceName = new List<ComboboxRequest>();
        public List<ComboboxRequest> ListPlaceName
        { get { return _listPlaceName; } set { SetProperty(ref _listPlaceName, value); } }
        private ComboboxResponse _selectedPlaceName = new ComboboxResponse();
        public ComboboxResponse SelectedPlaceName { get => _selectedPlaceName; set => SetProperty(ref _selectedPlaceName, value); }

        private LocationStationResponse _selectedLocation = new LocationStationResponse();

        public LocationStationResponse SelectedLocation
        { get { return _selectedLocation; } set { SetProperty(ref _selectedLocation, value); } }

        private bool iHasOtherPlace = false;

        public bool IHasOtherPlace
        { get { return iHasOtherPlace; } set { SetProperty(ref iHasOtherPlace, value); } }

        //private bool isHasImg = false;

        //public bool IsHasImg
        //{ get { return isHasImg; } set { SetProperty(ref isHasImg, value); } }

        private string imagePathLocal = String.Empty;
        public string ImagePathLocal { get => imagePathLocal; set => SetProperty(ref imagePathLocal, value /*,nameof(AvatarDisplay)*/); }

        #endregion Property

        #region PrivateMethod

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
            //IsHasImg = true;
        }

        // danh sách phí, danh sách địa điểm trong combobox
        private void GetListExpenseCategory(ExpenseDetailsRespone obj, List<ListExpenseCategoryByCompanyRespone> ListExpense)
        {
            SafeExecute(async () =>
            {           
                ListExpenseCategory = ListExpense.ToList();
                foreach (var item in ListExpense.ToList())
                {
                    ListExpenseName.Add(new ComboboxRequest()
                    {
                        Value = item.Name
                    });
                }
                var _listPlace = await _StationLocation.GetListLocationStation(UserInfo.CompanyId);
                if (_listPlace == null && listPlace.Count < 0)
                {
                    _displayMessage.ShowMessageInfo(MobileResource.Common_ConnectInternet_Error);
                }
                else
                {
                    ListPlace = _listPlace;                 
                    ListPlaceName.Add(new ComboboxRequest()
                    {
                        Key = -1,
                        Value = DataItem.Place.ToDescription()
                    });
                    foreach (var item in _listPlace.ToList())
                    {
                        ListPlaceName.Add(new ComboboxRequest()
                        {
                            Value = item.Name
                        });
                    }
                }
                SelectedExpense = ListExpense.Where(x => x.Name == obj.Name).FirstOrDefault();
                if (SelectedExpense == null)
                {
                    SelectedExpense = new ListExpenseCategoryByCompanyRespone();
                }
                SelectedLocation = ListPlace.Where(x => x.Name == obj.LandmarkName).ToList().FirstOrDefault();
                if (SelectedLocation == null)
                {
                    SelectedLocation = new LocationStationResponse();
                }
                if (!string.IsNullOrEmpty(obj.OtherAddress))
                {
                    IHasOtherPlace = true;
                }
                if (!string.IsNullOrEmpty(obj.Photo))
                {
                    ImagePathLocal = obj.Photo;
                }
            });
        }
        // reset ảnh
        private async void ResetImage()
        {
            //if (!string.IsNullOrEmpty(ExpenseDetail.Photo))
            //{
            //    _displayMessage.ShowMessageWarning("Không được xóa ảnh!");
            //    return;
            //}
            bool result = await _PageDialog.DisplayAlertAsync("Cảnh báo", "Bạn có chắc muốn xóa ảnh không?", MobileResource.Common_Button_Yes, MobileResource.Common_Button_No);

            if (!result)
            {
                return;
            }
            else
            {
                ImagePathLocal = String.Empty;
                //IsHasImg = false;
            }
        }
        //Đổi phí
        private void ChangeExpense()
        {
            SafeExecute(async () =>
            {
                if (ExpenseDetail.ExpenseCost < 0)
                {
                    _displayMessage.ShowMessageWarning("Số tiền không được nhỏ hơn 0");
                    return;
                }
                if (!StringHelper.HasDangerousCharsCanNull(ExpenseDetail.Note) && !string.IsNullOrEmpty(ExpenseDetail.Note))
                {
                    _displayMessage.ShowMessageWarning("Không được nhập ký tự đặc biệt");
                    return;
                }
                if (SelectedLocation.Name == DataItem.Place.ToDescription())
                {
                    if (string.IsNullOrEmpty(ExpenseDetail.OtherAddress))
                    {
                        _displayMessage.ShowMessageWarning("Vui lòng nhập địa chỉ khác");
                        return;
                    }
                    if (!StringHelper.ValidateAddress(ExpenseDetail.OtherAddress))
                    {
                        _displayMessage.ShowMessageWarning("Không được nhập ký tự đặc biệt");
                        return;
                    }
                }
                if (ImagePathLocal == ExpenseDetail.Photo || string.IsNullOrEmpty(ImagePathLocal))
                {
                    var RequestExpense = new ImportExpenseRequest()
                    {
                        Id = ExpenseDetail.ID,
                        Photo = ImagePathLocal,
                        OtherAddress = ExpenseDetail.OtherAddress,
                        ExpenseCost = ExpenseDetail.ExpenseCost,
                        FK_CompanyID = UserInfo.CompanyId,
                        ExpenseDate = ExpenseDetail.ExpenseDate,
                        Note = ExpenseDetail.Note,
                        FK_ExpenseCategoryID = SelectedExpense.ID,
                        FK_LandmarkID = SelectedLocation.PK_LandmarkID,
                        FK_VehicleID = ExpenseDetail.FK_VehicleID,
                        User = UserInfo.UserId,
                        IsChangePhoto = false
                    };
                    IInsertExpense = await _ExpenseService.GetExpense(RequestExpense);
                } //else if (string.IsNullOrEmpty(ImagePathLocal))
                //{
                //    var RequestExpense = new ImportExpenseRequest()
                //    {
                //        Id = ExpenseDetail.ID,
                //        Photo = ImagePathLocal,
                //        OtherAddress = ExpenseDetail.OtherAddress,
                //        ExpenseCost = ExpenseDetail.ExpenseCost,
                //        FK_CompanyID = UserInfo.CompanyId,
                //        ExpenseDate = ExpenseDetail.ExpenseDate,
                //        Note = ExpenseDetail.Note,
                //        FK_ExpenseCategoryID = SelectedExpense.ID,
                //        FK_LandmarkID = SelectedLocation.PK_LandmarkID,
                //        FK_VehicleID = ExpenseDetail.FK_VehicleID,
                //        User = UserInfo.UserId,
                //        IsChangePhoto = true
                //    };
                //    IInsertExpense = await _ExpenseService.GetExpense(RequestExpense);
                //}
                else
                {
                    var RequestExpense = new ImportExpenseRequest()
                    {
                        Id = ExpenseDetail.ID,
                        Photo = GetBase64StringForImage(ImagePathLocal),
                        OtherAddress = ExpenseDetail.OtherAddress,
                        ExpenseCost = ExpenseDetail.ExpenseCost,
                        FK_CompanyID = UserInfo.CompanyId,
                        ExpenseDate = ExpenseDetail.ExpenseDate,
                        Note = ExpenseDetail.Note,
                        FK_ExpenseCategoryID = SelectedExpense.ID,
                        FK_LandmarkID = SelectedLocation.PK_LandmarkID,
                        FK_VehicleID = ExpenseDetail.FK_VehicleID,
                        User = UserInfo.UserId,
                        IsChangePhoto = true
                    };
                    IInsertExpense = await _ExpenseService.GetExpense(RequestExpense);
                }
                if (IInsertExpense)
                {
                    await Application.Current.MainPage.DisplayAlert("Thông báo", "Lưu thành công", MobileResource.Common_Button_OK);
                    await NavigationService.GoBackAsync();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Thông báo", "Lưu thất bại", MobileResource.Common_Button_OK);
                }

            });
        }
        // lưu phí
        private void SaveExpense()
        {
            SafeExecute(async () =>
            {
                if (!string.IsNullOrEmpty(SelectedExpense.Name))
                {
                    if (ExpenseDetail.ExpenseCost < 0)
                    {
                        _displayMessage.ShowMessageWarning("Số tiền không được nhỏ hơn 0");
                        return;
                    }
                    if (!StringHelper.HasDangerousCharsCanNull(ExpenseDetail.Note) && !string.IsNullOrEmpty(ExpenseDetail.Note))
                    {
                        _displayMessage.ShowMessageWarning("Không được nhập ký tự đặc biệt");
                        return;
                    }
                    if (SelectedLocation.Name == DataItem.Place.ToDescription())
                    {
                        if (string.IsNullOrEmpty(ExpenseDetail.OtherAddress))
                        {
                            _displayMessage.ShowMessageWarning("Vui lòng nhập địa chỉ khác");
                            return;
                        }
                        if (!StringHelper.ValidateAddress(ExpenseDetail.OtherAddress))
                        {
                            _displayMessage.ShowMessageWarning("Không được nhập ký tự đặc biệt");
                            return;
                        }
                    }                 
                    var RequestExpense = new ImportExpenseRequest()
                    {
                        Id = ExpenseDetail.ID,
                        Photo = GetBase64StringForImage(ImagePathLocal),
                        OtherAddress = ExpenseDetail.OtherAddress,
                        ExpenseCost = ExpenseDetail.ExpenseCost,
                        FK_CompanyID = UserInfo.CompanyId,
                        ExpenseDate = ExpenseDetail.ExpenseDate,
                        Note = ExpenseDetail.Note,
                        FK_ExpenseCategoryID = SelectedExpense.ID,
                        FK_LandmarkID = SelectedLocation.PK_LandmarkID,
                        FK_VehicleID = ExpenseDetail.FK_VehicleID,
                        User = UserInfo.UserId,
                        IsChangePhoto = false
                    };
                    IInsertExpense = await _ExpenseService.GetExpense(RequestExpense);
                    if (IInsertExpense)
                    {
                        await Application.Current.MainPage.DisplayAlert("Thông báo", "Lưu thành công", MobileResource.Common_Button_OK);
                        await NavigationService.GoBackAsync();
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Thông báo", "Lưu thất bại", MobileResource.Common_Button_OK);
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
                    if (ExpenseDetail.ExpenseCost < 0)
                    {
                        _displayMessage.ShowMessageWarning("Số tiền không được nhỏ hơn 0");
                        return;
                    }
                    if (!StringHelper.HasDangerousCharsCanNull(ExpenseDetail.Note) && !string.IsNullOrEmpty(ExpenseDetail.Note))
                    {
                        _displayMessage.ShowMessageWarning("Không được nhập ký tự đặc biệt");
                        return;
                    }
                    if (SelectedLocation.Name == DataItem.Place.ToDescription())
                    {
                        if (string.IsNullOrEmpty(ExpenseDetail.OtherAddress))
                        {
                            _displayMessage.ShowMessageWarning("Vui lòng nhập địa chỉ khác");
                            return;
                        }
                        if (!StringHelper.ValidateAddress(ExpenseDetail.OtherAddress))
                        {
                            _displayMessage.ShowMessageWarning("Không được nhập ký tự đặc biệt");
                            return;
                        }
                    }              
                    var RequestExpense = new ImportExpenseRequest()
                    {
                        Id = ExpenseDetail.ID,
                        Photo = GetBase64StringForImage(ImagePathLocal),
                        OtherAddress = ExpenseDetail.OtherAddress,
                        ExpenseCost = ExpenseDetail.ExpenseCost,
                        FK_CompanyID = UserInfo.CompanyId,
                        ExpenseDate = ExpenseDetail.ExpenseDate,
                        Note = ExpenseDetail.Note,
                        FK_ExpenseCategoryID = SelectedExpense.ID,
                        FK_LandmarkID = SelectedLocation.PK_LandmarkID,
                        FK_VehicleID = ExpenseDetail.FK_VehicleID,
                        User = UserInfo.UserId,
                        IsChangePhoto = false
                    };
                    IInsertExpense = await _ExpenseService.GetExpense(RequestExpense);                
                    if (IInsertExpense)
                    {
                        await Application.Current.MainPage.DisplayAlert("Thông báo", "Lưu thành công", MobileResource.Common_Button_OK);
                        SelectedExpense = new ListExpenseCategoryByCompanyRespone();
                        SelectedLocation = new LocationStationResponse();
                        ExpenseDetail.Note = String.Empty;
                        ExpenseDetail.ExpenseCost = 0;
                        ImagePathLocal = String.Empty;
                        //IsHasImg = false;
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Thông báo", "Lưu thất bại", MobileResource.Common_Button_OK);
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

                    fileAvatar.Dispose();
                });
            });
        }

        //danh sách Địa điẻm
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
                    { "dataCombobox", ListPlaceName },
                    { "ComboboxType", ComboboxType.Third },
                    { "Title", "Chọn địa điểm" }
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

        //Danh sách loại chi phí
        public async void PushComboboxExpensene()
        {
            if (!string.IsNullOrEmpty(ExpenseDetail.Name))
            {
                _displayMessage.ShowMessageWarning("Vui lòng không đổi loại phí!");
                return;
            }
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;
            try
            {
                var p = new NavigationParameters
                {
                    { "dataCombobox", ListExpenseName},
                    { "ComboboxType", ComboboxType.Second },
                    { "Title", "Chọn loại phí" }
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

        // Trả về giá trị loại phí được chọn
        private void FilterExpense(ComboboxResponse param)
        {
            if (param != null)
            {
                SelectedExpense = ListExpenseCategory.Where(x => x.Name == param.Value).FirstOrDefault();
                if (!SelectedExpense.HasLandmark)
                {
                    ExpenseDetail.OtherAddress = string.Empty;
                    SelectedLocation = new LocationStationResponse();
                }
                if (SelectedExpense.HasPhoto && ExpenseDetail.Photo != null)
                {
                    ImagePathLocal = ExpenseDetail.Photo;
                }
                else
                {
                    ImagePathLocal = string.Empty;
                }
                //IsHasImg = false;
            }
            else
            {
                SelectedExpense = new ListExpenseCategoryByCompanyRespone();
            }
        }

        // chọn địa điểm từ combobox
        public void UpdateDataCombobox(ComboboxResponse param)
        {
            base.UpdateCombobox(param);
            if (param != null)
            {
                var dataResponse = param;
                if (dataResponse.ComboboxType == (Int16)ComboboxType.Third)
                {
                    SelectedPlaceName = dataResponse;
                    FilterPlace(dataResponse);
                }
                if (dataResponse.ComboboxType == (Int16)ComboboxType.Second)
                {
                    SelectedExpenseName = dataResponse;
                    FilterExpense(dataResponse);
                }
            }
        }

        // trả về giá trị của địa điểm được chọn
        private void FilterPlace(ComboboxResponse param)
        {
            if (param != null)
            {
                if (param != null && param.Value != DataItem.Place.ToDescription())
                {
                    SelectedLocation = ListPlace.Where(x => x.Name == param.Value).FirstOrDefault();
                    IHasOtherPlace = false;
                    ExpenseDetail.OtherAddress = string.Empty;
                }
                else
                {
                    SelectedLocation = new LocationStationResponse
                    {
                        Name = DataItem.Place.ToDescription()
                    };
                    IHasOtherPlace = true;
                };
            }
            else
            {
                return;
            }
        }
        // Convert url image to base64
        //private string GetBase64StringForUrlImage(string imgPath)
        //{
        //    var webClient = new WebClient();
        //    //byte[] imageBytes = webClient.DownloadData("http://www.google.com/images/logos/ps_logo2.png");
        //    byte[] imageBytes = webClient.DownloadData(imgPath);
        //    string base64String = Convert.ToBase64String(imageBytes);
        //    return base64String;
        //}
        //// Chuyển từ image to base64
        private string GetBase64StringForImage(string imgPath)
        {
            if (string.IsNullOrEmpty(imgPath))
            {
                return string.Empty;
            }
            byte[] imageBytes = System.IO.File.ReadAllBytes(imgPath);
            string base64String = Convert.ToBase64String(imageBytes);
            return base64String;
        }
        #endregion PrivateMethod
    }
}