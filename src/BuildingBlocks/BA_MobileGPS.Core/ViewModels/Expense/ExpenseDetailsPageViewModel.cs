using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.ResponeEntity.Expense;
using BA_MobileGPS.Entities.ResponeEntity.Support;
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
    public class ExpenseDetailsPageViewModel : ViewModelBase
    {
        #region Property

        private Vehicle _vehicle = new Vehicle();
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
        private decimal _totalMoney = 600000;
        public decimal TotalMoney
        {
            get { return _totalMoney; }
            set { SetProperty(ref _totalMoney, value); }
        }
        private string _successImage = string.Empty;
        public string SuccessImage
        {
            get { return _successImage; }
            set { SetProperty(ref _successImage, value); }
        }
        private List<ExpenseDetailsRespone> _menuItems = new List<ExpenseDetailsRespone>();
        public List<ExpenseDetailsRespone> MenuItems
        {
            get { return _menuItems; }
            set { SetProperty(ref _menuItems, value); }
        }
        private ListExpenseCategoryByCompanyRespone _selectedLocation = new ListExpenseCategoryByCompanyRespone();
        public ListExpenseCategoryByCompanyRespone SelectedLocation
        {
            get { return _selectedLocation; }
            set { SetProperty(ref _selectedLocation, value); }
        }
        private List<ListExpenseCategoryByCompanyRespone> _listMenuExpense = new List<ListExpenseCategoryByCompanyRespone>();
        public List<ListExpenseCategoryByCompanyRespone> ListMenuExpense
        {
            get { return _listMenuExpense; }
            set { SetProperty(ref _listMenuExpense, value); }
        }
        #endregion

        #region Contructor
        public ICommand ChooseDateDateTimePageCommand { get; private set; }
        public ICommand NavigateCommand { get; }
        public ICommand ShowPicturnCommand { get; }
        private IExpenseService _ExpenseService { get; set; }
        public ExpenseDetailsPageViewModel(INavigationService navigationService, IExpenseService ExpenseService)
            : base(navigationService)
        {
            ChooseDateDateTimePageCommand = new DelegateCommand(ExecuteToChooseDateTime);
            EventAggregator.GetEvent<SelectDateTimeEvent>().Subscribe(UpdateDateTime);
            NavigateCommand = new DelegateCommand<ItemTappedEventArgs>(NavigateClicked);
            ShowPicturnCommand = new DelegateCommand<ExpenseDetailsRespone>(ShowPicturnClicked);
            _ExpenseService = ExpenseService;
        }

        #endregion 

        #region Lifecycle

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            GetListExpenseCategory();
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
            if (parameters.ContainsKey("ExpenseDetails") && parameters.GetValue<ExpenseRespone>("ExpenseDetails") is ExpenseRespone objSupport)
            {
                TotalMoney = objSupport.Total;
                MenuItems = objSupport.Expenses;
                ChooseDate = objSupport.ExpenseDate;
            }
            else
            {
                TotalMoney = 0;
                MenuItems = new List<ExpenseDetailsRespone>();
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
        private void ExecuteToChooseDateTime()
        {
            SafeExecute(async () =>
            {
                var parameters = new NavigationParameters
                {
                    { "DataPicker", ChooseDate },
                    { "PickerType", ComboboxType.First }
                };
                await NavigationService.NavigateAsync("SelectDatePicker", parameters);
            });
        }
        private void UpdateDateTime(PickerDateTimeResponse param)
        {
            if (param != null)
            {
                if (param.PickerType == (short)ComboboxType.First)
                {
                    ChooseDate = param.Value;
                }
                else if (param.PickerType == (short)ComboboxType.Third)
                {
                }
            }
        }
        private void GetListMenuExpense()
        {
            
        }
        public void NavigateClicked(ItemTappedEventArgs item)
        {
            if (Vehicle == null || Vehicle.PrivateCode == null)
            {
                DisplayMessage.ShowMessageInfo(MobileResource.Common_Message_NoSelectVehiclePlate, 5000);
                return;
            }
            var parameters = new NavigationParameters
            {
                { ParameterKey.Vehicle, Vehicle }
            };

            if (item == null || item.ItemData == null)
            {
                SafeExecute(async () =>
                {
                    await NavigationService.NavigateAsync("ImportExpensePage", parameters);
                });
                return;
            }
            else
            {
                parameters.Add("ImportExpense", item.ItemData);
                SafeExecute(async () =>
                {
                    await NavigationService.NavigateAsync("ImportExpensePage", parameters);
                });
            }           
        }
        public void ShowPicturnClicked(ExpenseDetailsRespone item)
        {
            if(item != null)
            {
                SuccessImage = item.Photo;
            }     
        }
        private void GetListExpenseCategory()
        {
            SafeExecute(async () =>
            {
                if (IsConnected)
                {
                    var companyID = CurrentComanyID;
                    using (new HUDService(MobileResource.Common_Message_Processing))
                    {
                        ListMenuExpense = await _ExpenseService.GetExpenseCategory(303);
                    }
                }
                else
                {
                    DisplayMessage.ShowMessageInfo(MobileResource.Common_ConnectInternet_Error, 5000);
                }
            });
        }
        #endregion PrivateMethod
    }
}
