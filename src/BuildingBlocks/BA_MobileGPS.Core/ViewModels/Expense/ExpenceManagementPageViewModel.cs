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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BA_MobileGPS.Core.ViewModels.Expense
{
    public class ExpenceManagementPageViewModel : ViewModelBase
    {
        #region Property

        private Vehicle _vehicle = new Vehicle();
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
        private DateTime minfromDate = DateTime.Today.AddYears(-1);
        public virtual DateTime MinfromDate
        {
            get => minfromDate;
            set => SetProperty(ref minfromDate, value);
        }
        private DateTime toDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 23, 59, 59);
        public virtual DateTime ToDate { get => toDate; set => SetProperty(ref toDate, value); }

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
        #endregion Property

        #region Contructor

        public ICommand PushToFromDateTimePageCommand { get; private set; }
        public ICommand PushToEndDateTimePageCommand { get; private set; }
        public ICommand SearchDataCommand { get; private set; }
        public ICommand AddDataCommand { get; private set; }
        public ICommand DeleteItemCommand { get; private set; }
        public ICommand NavigateCommand { get; }
        private IExpenseService _ExpenseService { get; set; }
        public ExpenceManagementPageViewModel(INavigationService navigationService,IExpenseService expenseService)
            : base(navigationService)
        {
            PushToFromDateTimePageCommand = new DelegateCommand(ExecuteToFromDateTime);
            PushToEndDateTimePageCommand = new DelegateCommand(ExecuteToEndDateTime);
            EventAggregator.GetEvent<SelectDateEvent>().Subscribe(UpdateDate);
            EventAggregator.GetEvent<SelectDateTimeEvent>().Subscribe(UpdateDateTime);
            SearchDataCommand = new DelegateCommand(SearchDataClicked);
            AddDataCommand = new DelegateCommand(AddDataClicked);
            DeleteItemCommand = new DelegateCommand<ExpenseRespone>(DeleteItemClicked);
            NavigateCommand = new DelegateCommand<ItemTappedEventArgs>(NavigateClicked);
            _ExpenseService = expenseService;
        }

        #endregion Contructor

        #region Lifecycle

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            GetListExpense();
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

        #region PrivateMethod

        private void ExecuteToFromDateTime()
        {
            SafeExecute(async () =>
            {
                var parameters = new NavigationParameters
                {
                    { "DataPicker", FromDate },
                    { "PickerType", ComboboxType.First }
                };
                await NavigationService.NavigateAsync("SelectDatePicker", parameters);
            });
        }
        private void ExecuteToEndDateTime()
        {
            SafeExecute(async () =>
            {
                var parameters = new NavigationParameters
                {
                    { "DataPicker", ToDate },
                    { "PickerType", ComboboxType.Second }
                };
                await NavigationService.NavigateAsync("SelectDatePicker", parameters);
            });
        }
        private void UpdateDate(PickerDateResponse param)
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
            var companyID = CurrentComanyID;
            var vehicleID = (int)Vehicle.VehicleId;
            var request = new ExpenseRequest()
            {
                CompanyID = 303,
                VehicleID = 43227,
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
            if (MenuItems != null && MenuItems.Count>0)
            {
                foreach (var obj in MenuItems)
                {
                    TotalMoney = TotalMoney + obj.Total;
                }
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
                DisplayMessage.ShowMessageInfo("Bạn không được phép xem quá 90 ngày");
                result = false;
            }
            return result;
        }
        private void AddDataClicked()
        {

        }

        private async void DeleteItemClicked(ExpenseRespone obj)
        {
            var action = await PageDialog.DisplayAlertAsync("Cảnh báo", "Bạn có chắc chắn muốn xóa?","Có", "Không");
            if(!action)
            {
                return;
            }    
        }

        public void NavigateClicked(ItemTappedEventArgs item)
        {
            if (item == null || item.ItemData == null)
            {
                SafeExecute(async () =>
                {
                    await NavigationService.NavigateAsync("ExpenseDetailsPage");
                });
                return;
            };
            var parameters = new NavigationParameters
            {
                { "ExpenseDetails", item.ItemData},
            };
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("ExpenseDetailsPage", parameters);
            });
        }
        #endregion PrivateMethod
    }
}
