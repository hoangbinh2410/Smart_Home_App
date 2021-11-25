using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.RequestEntity.Expense;
using BA_MobileGPS.Entities.ResponeEntity.Expense;
using BA_MobileGPS.Entities.ResponeEntity.Support;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Extensions;
using Prism.Commands;
using Prism.Navigation;
using Syncfusion.ListView.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
//using Xamarin.Forms;
using Xamarin.Forms.Extensions;
using SelectionChangedEventArgs = Syncfusion.ListView.XForms.ItemTappedEventArgs;

namespace BA_MobileGPS.Core.ViewModels.Expense
{
    public class ExpenseDetailsPageViewModel : ViewModelBase
    {
        #region Property

        private Vehicle _vehicle;
        public Vehicle Vehicle
        {
            get => _vehicle;
            set => SetProperty(ref _vehicle, value);
        }

        private DateTime _chooseDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 0, 0, 0);
        public virtual DateTime ChooseDate
        {
            get => _chooseDate;
            set => SetProperty(ref _chooseDate, value);
        }

        private decimal _totalMoney = 0;
        public decimal TotalMoney
        {
            get { return _totalMoney; }
            set { SetProperty(ref _totalMoney, value); }
        }

        private string _sourceImage = string.Empty;
        public string SourceImage
        {
            get { return _sourceImage; }
            set { SetProperty(ref _sourceImage, value); }
        }
        private string _sourceLocal = string.Empty;
        public string SourceLocal
        {
            get { return _sourceLocal; }
            set { SetProperty(ref _sourceLocal, value); }
        }

        private ObservableCollection<ExpenseDetailsRespone> _menuItems = new ObservableCollection<ExpenseDetailsRespone>();
        public ObservableCollection<ExpenseDetailsRespone> MenuItems
        {
            get { return _menuItems; }
            set { SetProperty(ref _menuItems, value); }
        }

        private ComboboxResponse _selectedExpenseName = new ComboboxResponse();
        public ComboboxResponse SelectedExpenseName
        {
            get => _selectedExpenseName;
            set => SetProperty(ref _selectedExpenseName, value);
        }

        private List<ExpenseDetailsRespone> _menuItemsRemember { get; set; }
        private List<ComboboxRequest> ListExpenseName = new List<ComboboxRequest>();
        private List<ListExpenseCategoryByCompanyRespone> ListMenuExpense = new List<ListExpenseCategoryByCompanyRespone>();
        private bool _isCall = false;
        private bool _isShowImage = false;
        public bool IsShowImage
        {
            get { return _isShowImage; }
            set { SetProperty(ref _isShowImage, value); }
        }



        #endregion

        #region Contructor

        public ICommand ChooseDateDateTimePageCommand { get; private set; }
        public ICommand TabImageCommand { get; private set; }
        public ICommand NavigateCommand { get; }
        public ICommand ShowPicturnCommand { get; }
        private IExpenseService _ExpenseService { get; set; }
        public ICommand DeleteItemCommand { get; private set; }
        public ICommand PushExpenseNameCommand { get; private set; }
        public ExpenseDetailsPageViewModel(INavigationService navigationService, IExpenseService ExpenseService)
            : base(navigationService)
        {
            ChooseDateDateTimePageCommand = new DelegateCommand(ExecuteToChooseDateTime);
            NavigateCommand = new DelegateCommand<SelectionChangedEventArgs>(NavigateClicked);
            ShowPicturnCommand = new DelegateCommand<ExpenseDetailsRespone>(ShowPicturnClicked);
            DeleteItemCommand = new DelegateCommand<ExpenseDetailsRespone>(DeleteItemClicked);
            PushExpenseNameCommand = new DelegateCommand(ExecuteExpenseNameCombobox);
            EventAggregator.GetEvent<SelectDateEvent>().Subscribe(UpdateDateTime);
            TabImageCommand = new DelegateCommand(TabImage);
            EventAggregator.GetEvent<SelectComboboxEvent>().Subscribe(UpdateValueCombobox);
            _ExpenseService = ExpenseService;
        }

        #endregion 

        #region Lifecycle

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            GetListExpenseCategory();
            if (parameters.ContainsKey("ExpenseDetails") && parameters.GetValue<ExpenseRespone>("ExpenseDetails") is ExpenseRespone objSupport)
            {
                TotalMoney = objSupport.Total;
                MenuItems = objSupport.Expenses.ToObservableCollection();
                _menuItemsRemember = objSupport.Expenses;
                ChooseDate = objSupport.ExpenseDate;
            }
            else
            {
                TotalMoney = 0;
                MenuItems = new ObservableCollection<ExpenseDetailsRespone>();
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters != null)
            {
                if (parameters.ContainsKey(ParameterKey.Vehicle) && parameters.GetValue<Vehicle>(ParameterKey.Vehicle) is Vehicle vehicle)
                {
                    Vehicle = vehicle;
                }
            }
            if (_isCall)
            {
                GetListExpenseAgain();
            }
            _isCall = true;
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
            EventAggregator.GetEvent<SelectComboboxEvent>().Unsubscribe(UpdateValueCombobox);
            EventAggregator.GetEvent<SelectDateEvent>().Unsubscribe(UpdateDateTime);
        }

        #endregion Lifecycle

        #region PrivateMethod

        private void ExecuteToChooseDateTime()
        {
            SafeExecute(async () =>
            {
                var parameters = new NavigationParameters
                {
                    { "DataPicker", ChooseDate },
                    { "PickerType", ComboboxType.Third }
                };
                await NavigationService.NavigateAsync("SelectDateCalendar", parameters);
            });
        }
        private void UpdateDateTime(PickerDateResponse param)
        {
            if (param != null)
            {
                if (param.PickerType == (short)ComboboxType.Third)
                {
                    ChooseDate = param.Value;
                }
            }
        }
        public void NavigateClicked(SelectionChangedEventArgs item)
        {
            if (!ValidateDateTime())
            {
                return;
            }

            if (Vehicle == null || Vehicle.PrivateCode == null)
            {
                DisplayMessage.ShowMessageInfo(MobileResource.Common_Message_NoSelectVehiclePlate, 5000);
                return;
            }

            var objExpense = new ExpenseDetailsRespone();
            objExpense.FK_VehicleID = Vehicle.VehicleId;
            objExpense.ExpenseDate = ChooseDate;

            if (item != null && item.ItemData != null)
            {
                var objItem = (ExpenseDetailsRespone)item.ItemData;
                //obj này giá trị đã bị fomat,gửi sang đúng giá trị từ API
                objExpense = MenuItems.Where(x => x.ID == objItem.ID).FirstOrDefault();
            }
            var parameters = new NavigationParameters
            {
                { "MenuExpense", ListMenuExpense},
                { "ImportExpense", objExpense},
            };           
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("ImportExpensePage", parameters);
            });
        }
        public void ShowPicturnClicked(ExpenseDetailsRespone item)
        {
            if (item != null)
            {
                IsShowImage = true;
                SourceImage = item.Photo;
                if (!StringHelper.ValidateAddress(item.OtherAddress))
                {
                   SourceLocal = item.OtherAddress ;
                }
                else
                {
                    SourceLocal = item.LandmarkName;
                }
            }
        }
        private void GetListExpenseCategory()
        {
            RunOnBackground(async () =>
            {
                var companyID = CurrentComanyID;
                return await _ExpenseService.GetExpenseCategory(companyID);
            },
             (result) =>
             {
                 if (result != null)
                 {
                     ListMenuExpense = result.ToList();
                     ListExpenseName.Add(new ComboboxRequest()
                     {
                         Key = -1,
                         Value = DataItem.AllExpense.ToDescription()
                     });
                     foreach (var item in result.ToList())
                     {
                         ListExpenseName.Add(new ComboboxRequest()
                         {
                             Value = item.Name
                         });
                     }
                 }
             });
        }
        private void FilterData(ComboboxResponse param)
        {
            _isCall = false;
            if (param != null && param.Value != DataItem.AllExpense.ToDescription())
            {
                MenuItems = _menuItemsRemember.Where(x => x.Name == param.Value)?.ToList().ToObservableCollection(); ;
                SumMoney();
            }
            else
            {
                MenuItems = _menuItemsRemember.ToObservableCollection(); ;
                SumMoney();
            }
        }
        private async void DeleteItemClicked(ExpenseDetailsRespone obj)
        {
            if (obj == null)
            {
                DisplayMessage.ShowMessageError("Có lỗi khi xóa, kiểm tra lại", 5000);
                return;
            }
            var action = await PageDialog.DisplayAlertAsync("Cảnh báo", "Bạn chắc chắn muốn xóa chi phí này?", "Có", "Không");
            if (!action)
            {
                return;
            }
            DeleteExpenseRequest request = new DeleteExpenseRequest()
            {
                ListID = new List<Guid>()
                {
                    obj.ID
                }
            };
            SafeExecute(async () =>
            {
                if (IsConnected)
                {
                    using (new HUDService(MobileResource.Common_Message_Processing))
                    {
                        bool isdeleted = await _ExpenseService.Deletemultiple(request);
                        if (isdeleted)
                        {
                            GetListExpenseAgain();
                            DisplayMessage.ShowMessageSuccess("Xóa thành công!", 1500);
                        }
                        else
                        {
                            DisplayMessage.ShowMessageError("Có lỗi khi xóa, kiểm tra lại", 5000);
                        }
                    }
                }
                else
                {
                    DisplayMessage.ShowMessageInfo(MobileResource.Common_ConnectInternet_Error, 5000);
                }
            });
        }
        private void GetListExpenseAgain()
        {
            if (Vehicle == null)
            {
                DisplayMessage.ShowMessageInfo(MobileResource.Common_Message_NoSelectVehiclePlate, 5000);
                return;
            }
            var companyID = CurrentComanyID;
            var vehicleID = Vehicle.VehicleId;
            var request = new ExpenseRequest()
            {
                CompanyID = companyID,
                VehicleID = vehicleID,
                ToDate = ChooseDate,
                FromDate = ChooseDate
            };
            TryExecute(async () =>
            {
                if (IsConnected)
                {
                    using (new HUDService(MobileResource.Common_Message_Processing))
                    {
                        var listItems = await _ExpenseService.GetListExpense(request);
                        if (listItems != null && listItems.Count > 0)
                        {
                            //  TotalMoney = listItems.Where(x => x.ExpenseDate.Day == ChooseDate.Day).FirstOrDefault().Total;
                           
                            _menuItemsRemember = listItems.Where(x => x.ExpenseDate.Day == ChooseDate.Day).FirstOrDefault().Expenses;                        
                            if (!string.IsNullOrEmpty(SelectedExpenseName.Value) && SelectedExpenseName.Value != DataItem.AllExpense.ToDescription())
                            {
                                MenuItems = _menuItemsRemember.Where(y => y.Name == SelectedExpenseName.Value)?.ToList().ToObservableCollection(); ;
                                SumMoney();

                            }
                            else
                            {
                                MenuItems = _menuItemsRemember.ToObservableCollection(); ;
                                SumMoney();
                            }
                        }
                        else
                        {

                            TotalMoney = 0;
                            MenuItems = new ObservableCollection<ExpenseDetailsRespone>();
                            _menuItemsRemember = new List<ExpenseDetailsRespone>();
                        }

                    }
                }
                else
                {
                    DisplayMessage.ShowMessageInfo(MobileResource.Common_ConnectInternet_Error, 5000);
                }
            });
        }
        public async void ExecuteExpenseNameCombobox()
        {
            if (Vehicle == null || Vehicle.PrivateCode == null)
            {
                DisplayMessage.ShowMessageInfo(MobileResource.Common_Message_NoSelectVehiclePlate, 5000);
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
                    { "dataCombobox", ListExpenseName },
                    { "ComboboxType", ComboboxType.First },
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
        public void UpdateValueCombobox(ComboboxResponse param)
        {
            base.UpdateCombobox(param);
            if (param != null)
            {
                var dataResponse = param;
                if (dataResponse.ComboboxType == (Int16)ComboboxType.First)
                {
                    SelectedExpenseName = dataResponse;
                    FilterData(dataResponse);
                }
            }
        }
        private bool ValidateDateTime()
        {
            var result = true;
            if (DateTime.Now.Subtract(ChooseDate).TotalDays < 0)
            {
                DisplayMessage.ShowMessageError("Ngày được chọn chưa diễn ra");
                result = false;
            }
            return result;
        }
        private void SumMoney()
        {
            TotalMoney = 0;
            if (MenuItems != null && MenuItems.Count > 0)
            {
                TotalMoney = MenuItems.Sum(x => x.ExpenseCost);
            }
        }
        private void TabImage()
        {
            try
            {
                if (Device.RuntimePlatform == Device.Android)
                {
                    new PhotoBrowser
                    {
                        Photos = new List<Photo>()
                        {
                            new Photo{ URL = SourceImage, Info = SourceLocal}
                        },
                        ActionButtonPressed = (index) =>
                        {
                            PhotoBrowser.Close();
                        },
                        EnableGrid = true
                    }.Show();
                }
                else
                {
                    new PhotoBrowser
                    {
                        Photos = new List<Photo>()
                        {
                            new Photo{ URL = SourceImage, Info = SourceLocal}
                        },
                        ActionButtonPressed = null,
                        EnableGrid = true
                    }.Show();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        #endregion PrivateMethod
    }
}
