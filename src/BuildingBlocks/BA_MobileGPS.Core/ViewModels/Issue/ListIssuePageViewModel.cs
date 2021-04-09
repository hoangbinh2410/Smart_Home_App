using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Windows.Input;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class ListIssuePageViewModel : ViewModelBase
    {
        private CancellationTokenSource cts;
        public ICommand PushToFromDateTimePageCommand { get; private set; }
        public ICommand PushToEndDateTimePageCommand { get; private set; }
        public ICommand SearchIssueCommand { get; private set; }
        public ICommand PushStatusIssueCommand { get; private set; }
        public ICommand SortCommand { get; private set; }
        public ICommand SelectFavoriteIssueCommand { get; private set; }

        public ListIssuePageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Danh sách yêu cầu hỗ trợ";
            PushToFromDateTimePageCommand = new DelegateCommand(ExecuteToFromDateTime);
            PushToEndDateTimePageCommand = new DelegateCommand(ExecuteToEndDateTime);
            PushStatusIssueCommand = new DelegateCommand(ExecuteStatusStatusIssueCombobox);
            SearchIssueCommand = new DelegateCommand<TextChangedEventArgs>(SearchIssuewithText);
            SortCommand = new DelegateCommand(SortIssue);
            SelectFavoriteIssueCommand = new DelegateCommand(SelectFavoriteIssue);
        }

        private DateTime fromDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 0, 0, 0);
        public virtual DateTime FromDate { get => fromDate; set => SetProperty(ref fromDate, value); }

        private DateTime toDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 23, 59, 59);
        public virtual DateTime ToDate { get => toDate; set => SetProperty(ref toDate, value); }

        public string searchedText;
        public string SearchedText { get => searchedText; set => SetProperty(ref searchedText, value); }

        private ComboboxResponse statusIssueSelected;
        public ComboboxResponse StatusIssueSelected { get => statusIssueSelected; set => SetProperty(ref statusIssueSelected, value); }

        public override void Initialize(INavigationParameters parameters)
        {
            EventAggregator.GetEvent<SelectDateTimeEvent>().Subscribe(UpdateDateTime);
            EventAggregator.GetEvent<SelectComboboxEvent>().Subscribe(UpdateCombobox);
        }

        public override void OnDestroy()
        {
            EventAggregator.GetEvent<SelectDateTimeEvent>().Unsubscribe(UpdateDateTime);
            EventAggregator.GetEvent<SelectComboboxEvent>().Unsubscribe(UpdateCombobox);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
        }

        public void UpdateDateTime(PickerDateTimeResponse param)
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

        public void ExecuteToFromDateTime()
        {
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

        public void ExecuteToEndDateTime()
        {
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

        private List<ComboboxRequest> LoadAllStatusIssue()
        {
            return new List<ComboboxRequest>() {
                    new ComboboxRequest(){Key = 0 , Value = "Tất cả"},
                    new ComboboxRequest(){Key = 1 , Value = "Đã gửi yêu cầu"},
                    new ComboboxRequest(){Key = 2 , Value = "CSKH đã tiếp nhận"},
                    new ComboboxRequest(){Key = 3 , Value = "Kỹ thuật đang xử lý"},
                    new ComboboxRequest(){Key = 4 , Value = "Hoàn thành"},
                };
        }

        public async void ExecuteStatusStatusIssueCombobox()
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
                    { "dataCombobox", LoadAllStatusIssue() },
                    { "ComboboxType", ComboboxType.First },
                    { "Title", "Trạng thái" }
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
                    StatusIssueSelected = dataResponse;
                }
            }
        }

        private void SortIssue()
        {
            throw new NotImplementedException();
        }

        private void SearchIssuewithText(TextChangedEventArgs args)
        {
            //if (ListVehicleByGroup == null || ListVehicleByStatus == null || args.NewTextValue == null)
            //    return;

            //if (cts != null)
            //    cts.Cancel(true);

            //cts = new CancellationTokenSource();

            //Task.Run(async () =>
            //{
            //    await Task.Delay(500, cts.Token);

            //    if (string.IsNullOrWhiteSpace(args.NewTextValue))
            //        return ListVehicleByStatus;
            //    return ListVehicleByStatus.FindAll(v => v.VehiclePlate.ToUpper().Contains(args.NewTextValue.ToUpper()) || v.PrivateCode.ToUpper().Contains(args.NewTextValue.ToUpper()));
            //}, cts.Token).ContinueWith(task => Device.BeginInvokeOnMainThread(() =>
            //{
            //    if (task.Status == TaskStatus.RanToCompletion)
            //    {
            //        ListVehicle = new ObservableCollection<VehicleOnlineViewModel>(task.Result);

            //        SetSortOrder(false);
            //    }
            //}));
        }

        private void SelectFavoriteIssue()
        {
            throw new NotImplementedException();
        }
    }
}