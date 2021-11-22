using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.RequestEntity.Expense;
using BA_MobileGPS.Entities.ResponeEntity.Expense;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;
using Prism.Commands;
using Prism.Navigation;
using Syncfusion.ListView.XForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace BA_MobileGPS.Core.ViewModels.Expense
{
    public class ExpenceManagementPageViewModel : ViewModelBase
    {
        #region Property

        private Vehicle _vehicle;
        public Vehicle Vehicle
        {
            get => _vehicle;
            set => SetProperty(ref _vehicle, value);
        }

        private DateTime fromDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1, 0, 0, 0);
        public virtual DateTime FromDate
        {
            get => fromDate;
            set => SetProperty(ref fromDate, value);
        }

        private DateTime toDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 23, 59, 59);
        public virtual DateTime ToDate 
        { 
            get => toDate; 
            set => SetProperty(ref toDate, value); 
        }

        private List<ExpenseRespone> _menuItems = new List<ExpenseRespone>();
        public List<ExpenseRespone> MenuItems
        {
            get { return _menuItems; }
            set { SetProperty(ref _menuItems, value); }
        }

        private decimal _totalMoney = 0;
        public decimal TotalMoney
        {
            get { return _totalMoney; }
            set { SetProperty(ref _totalMoney, value); }
        }

        private bool _isCall = false;

        #endregion Property

        #region Contructor

        public ICommand PushToFromDateTimePageCommand { get; private set; }
        public ICommand PushToEndDateTimePageCommand { get; private set; }
        public ICommand SearchDataCommand { get; private set; }
        public ICommand AddDataCommand { get; private set; }
        public ICommand DeleteItemCommand { get; private set; }
        public ICommand NavigateCommand { get; }
        public ICommand NewNavigateCommand { get; }
        private IExpenseService _ExpenseService { get; set; }

        public ExpenceManagementPageViewModel(INavigationService navigationService, IExpenseService expenseService)
            : base(navigationService)
        {
            PushToFromDateTimePageCommand = new DelegateCommand(ExecuteToFromDateTime);
            PushToEndDateTimePageCommand = new DelegateCommand(ExecuteToEndDateTime);
            EventAggregator.GetEvent<SelectDateTimeEvent>().Subscribe(UpdateDateTime);
            SearchDataCommand = new DelegateCommand(SearchDataClicked);
            AddDataCommand = new DelegateCommand(AddDataClicked);
            DeleteItemCommand = new DelegateCommand<ExpenseRespone>(DeleteItemClicked);
            NavigateCommand = new DelegateCommand<ItemTappedEventArgs>(NavigateClicked);
            NewNavigateCommand = new DelegateCommand<ItemTappedEventArgs>(NewNavigateClicked);
            _ExpenseService = expenseService;
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
            if (parameters != null)
            {
                if (parameters.ContainsKey(ParameterKey.Vehicle) && parameters.GetValue<Vehicle>(ParameterKey.Vehicle) is Vehicle vehicle)
                {
                    _isCall = false;
                    Vehicle = vehicle;
                }
            }
            if (_isCall)
            {
               // GetListExpense();
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
            EventAggregator.GetEvent<SelectDateTimeEvent>().Unsubscribe(UpdateDateTime);
        }

        #endregion Lifecycle

        #region PrivateMethod

        private void ExecuteToFromDateTime()
        {
            _isCall = false;
            SafeExecute(async () =>
            {
                var parameters = new NavigationParameters
                {
                    { "DataPicker", FromDate },
                    { "PickerType", ComboboxType.First }
                };
                await NavigationService.NavigateAsync("SelectDateTimeCalendar", parameters);
            });
        }

        private void ExecuteToEndDateTime()
        {
            _isCall = false;
            SafeExecute(async () =>
            {
                var parameters = new NavigationParameters
                {
                    { "DataPicker", ToDate },
                    { "PickerType", ComboboxType.Second }
                };
                await NavigationService.NavigateAsync("SelectDateTimeCalendar", parameters);
            });
        }

        private void UpdateDateTime(PickerDateTimeResponse param)
        {
            if (param != null)
            {
                if (param.PickerType == (short)ComboboxType.First)
                {
                    FromDate = param.Value;
                }
                else if (param.PickerType == (short)ComboboxType.Second)
                {
                    ToDate = param.Value;
                }
                else if (param.PickerType == (short)ComboboxType.Third)
                {
                }
            }
        }

        private void GetListExpense()
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
                ToDate = ToDate,
                FromDate = FromDate
            };
            SafeExecute(async () =>
            {
                if (IsConnected)
                {
                    using (new HUDService(MobileResource.Common_Message_Processing))
                    {
                        MenuItems = await _ExpenseService.GetListExpense(request);
                        //if(MenuItems == null || MenuItems.Count ==0)
                        //{
                        //    DisplayMessage.ShowMessageInfo(MobileResource.Common_Lable_NotFound, 1500);
                        //}    
                        SumMoney();
                    }
                }
                else
                {
                    DisplayMessage.ShowMessageInfo(MobileResource.Common_ConnectInternet_Error, 5000);
                }
            });
        }

        private void SumMoney()
        {
            TotalMoney = 0;
            if (MenuItems != null && MenuItems.Count > 0)
            {
                TotalMoney = MenuItems.Sum(x => x.Total);
            }
        }

        private void SearchDataClicked()
        {
            if (ValidateDateTime())
            {
                GetListExpense();
            }
        }

        private bool ValidateDateTime()
        {
            var result = true;
            if (FromDate > ToDate)
            {
                DisplayMessage.ShowMessageInfo("Thời gian bắt đầu phải nhỏ hơn thời gian kết thúc");
                result = false;
            }
            else if (ToDate.Subtract(FromDate).TotalDays > 90)
            {
                DisplayMessage.ShowMessageInfo("Vui lòng tìm kiếm trong phạm vi 90 ngày!");
                result = false;
            }
            return result;
        }

        private void AddDataClicked()
        {
        }

        private async void DeleteItemClicked(ExpenseRespone obj)
        {
            if (obj == null || obj.Expenses == null)
            {
                DisplayMessage.ShowMessageError("Có lỗi khi xóa, kiểm tra lại", 5000);
                return;
            }    
            var action = await PageDialog.DisplayAlertAsync("Cảnh báo",string.Format("Bạn chắc chắn muốn xóa chi phí ngày {0}?", obj.ExpenseDate.ToString("dd/MM/yyyy")) , "Có", "Không");
            if (!action)
            {
                return;
            }
            List<Guid> listguid = new List<Guid>();
            foreach(var guid in obj.Expenses)
            {
                listguid.Add(guid.ID);
            }
            DeleteExpenseRequest request = new DeleteExpenseRequest()
            {
                ListID = new List<Guid>(listguid)
            };
            TryExecute(async () =>
            {
                if (IsConnected)
                {
                    using (new HUDService(MobileResource.Common_Message_Processing))
                    {
                        bool isdeleted = await _ExpenseService.Deletemultiple(request);
                        if(isdeleted)
                        {
                            GetListExpense();
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

        public void NavigateClicked(ItemTappedEventArgs item)
        {
            //if (Vehicle == null || Vehicle.PrivateCode == null)
            //{
            //    DisplayMessage.ShowMessageInfo(MobileResource.Common_Message_NoSelectVehiclePlate, 5000);
            //    return;
            //}

            var parameters = new NavigationParameters
            {
                { ParameterKey.Vehicle, Vehicle }
            };

            if (item != null && item.ItemData != null)
            {

                parameters.Add("ExpenseDetails", item.ItemData);
            }

            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("ExpenseDetailsPage", parameters);
            });

        }
        public void NewNavigateClicked(ItemTappedEventArgs item)
        {
            //if(Vehicle == null || Vehicle.PrivateCode == null)
            //{
            //    DisplayMessage.ShowMessageInfo(MobileResource.Common_Message_NoSelectVehiclePlate, 5000);
            //    return;
            //}    

            var parameters = new NavigationParameters();
            //{
            //    { ParameterKey.Vehicle, Vehicle }
            //};

            if (item != null && item.ItemData != null)
            {

                parameters.Add("ExpenseDetails", item.ItemData);
            }

            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("ExpenseDetailsPage", parameters);
            });

        }      
        #endregion PrivateMethod
    }
}