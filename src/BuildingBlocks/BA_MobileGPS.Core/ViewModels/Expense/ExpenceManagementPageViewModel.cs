﻿using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using Prism.Commands;
using Prism.Navigation;
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

        public string vehiclePlate = string.Empty;

        public string VehiclePlate
        {
            get { return vehiclePlate; }
            set { SetProperty(ref vehiclePlate, value); }
        }
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

        private List<MenuExpense> _menuItems = new List<MenuExpense>();
        public List<MenuExpense> MenuItems
        {
            get { return _menuItems; }
            set { SetProperty(ref _menuItems, value); }
        }
        private int _totalMoney = 0;
        public int TotalMoney
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
        

        public ExpenceManagementPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            PushToFromDateTimePageCommand = new DelegateCommand(ExecuteToFromDateTime);
            PushToEndDateTimePageCommand = new DelegateCommand(ExecuteToEndDateTime);
            EventAggregator.GetEvent<SelectDateEvent>().Subscribe(UpdateDate);
            EventAggregator.GetEvent<SelectDateTimeEvent>().Subscribe(UpdateDateTime);
            SearchDataCommand = new DelegateCommand(SearchDataClicked);
            AddDataCommand = new DelegateCommand(AddDataClicked);
            DeleteItemCommand = new DelegateCommand<MenuExpense>(DeleteItemClicked);
            
        }

        #endregion Contructor

        #region Lifecycle

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            GetListMenuExpense();     
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters != null)
            {
                if (parameters.ContainsKey(ParameterKey.VehicleRoute) && parameters.GetValue<Vehicle>(ParameterKey.VehicleRoute) is Vehicle vehicle)
                {
                    Vehicle = vehicle;
                    VehiclePlate = vehicle.VehiclePlate;
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
        private void GetListMenuExpense()
        {
            SafeExecute(async () =>
            {
                if (IsConnected)
                {
                    using (new HUDService(MobileResource.Common_Message_Processing))
                    {
                        await Task.Delay(500);
                        MenuItems = new List<MenuExpense>()
                        {
                            new MenuExpense(){Date = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 23, 59, 59),Money=600000},
                            new MenuExpense(){Date = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 23, 59, 59),Money=800000},
                            new MenuExpense(){Date = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 23, 59, 59),Money=6000000},
                            new MenuExpense(){Date = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 23, 59, 59),Money=800000},
                            new MenuExpense(){Date = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 23, 59, 59),Money=600000},
                            new MenuExpense(){Date = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 23, 59, 59),Money=1200000},
                            new MenuExpense(){Date = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 23, 59, 59),Money=600000},
                            new MenuExpense(){Date = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 23, 59, 59),Money=80000000},
                            new MenuExpense(){Date = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 23, 59, 59),Money=600000},
                            new MenuExpense(){Date = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 23, 59, 59),Money=800000},
                            new MenuExpense(){Date = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 23, 59, 59),Money=600000},
                            new MenuExpense(){Date = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 23, 59, 59),Money=1200000},
                        };
                    }
                }
                else
                {
                    DisplayMessage.ShowMessageInfo(MobileResource.Common_ConnectInternet_Error, 5000);
                }
                SumMoney();
            });
        }
        private void SumMoney()
        {
            if(MenuItems != null && MenuItems.Count>0)
            {
                foreach (var obj in MenuItems)
                {
                    TotalMoney = TotalMoney + obj.Money;
                }
            }  
            else
            {
                TotalMoney = 0;
            }    
            
        }
        private void SearchDataClicked()
        {

        }

        private void AddDataClicked()
        {

        }

        private void DeleteItemClicked(MenuExpense obj)
        {
            List<MenuExpense> menuItems = new List<MenuExpense>();
            foreach (var item in MenuItems)
            {
                if(item != obj)
                {
                    menuItems.Add(item);
                }     
            }
            MenuItems = menuItems;
            TotalMoney = TotalMoney - obj.Money;
        }

        #endregion PrivateMethod
    }
    public class MenuExpense
    {
        public DateTime Date { get; set; }
        public int Money { get; set; }
    }
}
