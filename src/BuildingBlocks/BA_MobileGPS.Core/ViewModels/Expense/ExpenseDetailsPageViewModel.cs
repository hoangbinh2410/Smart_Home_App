using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.ResponeEntity.Support;
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

        public string vehiclePlate = string.Empty;

        public string VehiclePlate
        {
            get { return vehiclePlate; }
            set { SetProperty(ref vehiclePlate, value); }
        }
        private DateTime _chooseDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1, 0, 0, 0);
        public virtual DateTime ChooseDate
        {
            get => _chooseDate;
            set => SetProperty(ref _chooseDate, value);
        }
        private int _totalMoney = 600000;
        public int TotalMoney
        {
            get { return _totalMoney; }
            set { SetProperty(ref _totalMoney, value); }
        }
        private List<MenuExpenseDetails> _menuItems = new List<MenuExpenseDetails>();
        public List<MenuExpenseDetails> MenuItems
        {
            get { return _menuItems; }
            set { SetProperty(ref _menuItems, value); }
        }
        #endregion

        #region Contructor
        public ICommand ChooseDateDateTimePageCommand { get; private set; }
        public ICommand NavigateCommand { get; }
        public ExpenseDetailsPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            ChooseDateDateTimePageCommand = new DelegateCommand(ExecuteToChooseDateTime);
            EventAggregator.GetEvent<SelectDateTimeEvent>().Subscribe(UpdateDateTime);
            NavigateCommand = new DelegateCommand<ItemTappedEventArgs>(NavigateClicked);
        }

        #endregion 

        #region Lifecycle

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey("ExpenseDetails") && parameters.GetValue<MenuExpense>("ExpenseDetails") is MenuExpense objSupport)
            {
                GetListMenuExpense();
                TotalMoney = objSupport.Money;
            }
            else
            {
                TotalMoney = 0;
                MenuItems = new List<MenuExpenseDetails>();
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
            SafeExecute(async () =>
            {
                if (IsConnected)
                {
                    using (new HUDService(MobileResource.Common_Message_Processing))
                    {
                        await Task.Delay(500);
                        MenuItems = new List<MenuExpenseDetails>()
                        {
                            new MenuExpenseDetails(){NameExpense = "Đổ dầu",MoneyExpense=100000,LocationExpense="Bến xe Long Thành",ImageExpense=""},
                            new MenuExpenseDetails(){NameExpense = "Cơm",MoneyExpense=100000,LocationExpense="",ImageExpense=""},
                            new MenuExpenseDetails(){NameExpense = "Bồi dưỡng",MoneyExpense=100000,LocationExpense="",ImageExpense=""},
                            new MenuExpenseDetails(){NameExpense = "Sửa chữa, vá vỏ",MoneyExpense=100000,LocationExpense="Ga Hà Nội",ImageExpense=""},
                            new MenuExpenseDetails(){NameExpense = "Bốc xếp",MoneyExpense=100000,LocationExpense="Ga Hà Nội",ImageExpense=""},
                            new MenuExpenseDetails(){NameExpense = "Vé cầu đường",MoneyExpense=100000,LocationExpense="BOT Hà Nam",ImageExpense=""},
                        };
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
            if (item == null || item.ItemData == null)
            {
                SafeExecute(async () =>
                {
                    await NavigationService.NavigateAsync("ImportExpensePage");
                });
                return;
            };
            var parameters = new NavigationParameters
            {
                { "ImportExpense", item.ItemData},
            };
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("ImportExpensePage", parameters);
            });
        }
        #endregion PrivateMethod
    }
    public class MenuExpenseDetails
    {
        public string NameExpense { get; set; }
        public int MoneyExpense { get; set; }
        public string LocationExpense { get; set; }
        public string ImageExpense { get; set; }
    }
}
