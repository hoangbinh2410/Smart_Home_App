using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.Enums;
using BA_MobileGPS.Entities.ResponeEntity.Issues;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Extensions;
using ItemTappedEventArgs = Syncfusion.ListView.XForms.ItemTappedEventArgs;

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
        public ICommand NavigateCommand { get; private set; }
        public ICommand SetFavoriteIssueCommand { get; private set; }
        public ICommand LoadMoreItemsCommand { get; }
        private readonly IIssueService _issueService;

        public ListIssuePageViewModel(INavigationService navigationService, IIssueService issueService) : base(navigationService)
        {
            Title = "Danh sách yêu cầu hỗ trợ";
            _issueService = issueService;
            PushToFromDateTimePageCommand = new DelegateCommand(ExecuteToFromDateTime);
            PushToEndDateTimePageCommand = new DelegateCommand(ExecuteToEndDateTime);
            PushStatusIssueCommand = new DelegateCommand(ExecuteStatusStatusIssueCombobox);
            SearchIssueCommand = new DelegateCommand<TextChangedEventArgs>(SearchIssuewithText);
            SortCommand = new DelegateCommand(SortIssue);
            SelectFavoriteIssueCommand = new DelegateCommand(SelectFavoriteIssue);
            LoadMoreItemsCommand = new DelegateCommand(LoadMoreItems, CanLoadMoreItems);
            SetFavoriteIssueCommand = new DelegateCommand<object>(SetFavoriteIssue);
            NavigateCommand = new DelegateCommand<ItemTappedEventArgs>(Navigate);
            StatusIssueSelected = new ComboboxResponse()
            {
                Key = 0,
                Value = "Tất cả"
            };
            isSelectedFavorites = false;
            searchedText = "";
        }

        #region Property

        private int pageIndex { get; set; } = 0;
        private int pageCount { get; } = 10;

        private DateTime fromDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 0, 0, 0);
        public virtual DateTime FromDate { get => fromDate; set => SetProperty(ref fromDate, value); }

        private DateTime toDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 23, 59, 59);
        public virtual DateTime ToDate { get => toDate; set => SetProperty(ref toDate, value); }

        public string searchedText;
        public string SearchedText { get => searchedText; set => SetProperty(ref searchedText, value); }

        private ComboboxResponse statusIssueSelected;
        public ComboboxResponse StatusIssueSelected { get => statusIssueSelected; set => SetProperty(ref statusIssueSelected, value); }

        private ObservableCollection<IssuesRespone> listIssue = new ObservableCollection<IssuesRespone>();
        public ObservableCollection<IssuesRespone> ListIssue { get => listIssue; set => SetProperty(ref listIssue, value); }

        private List<IssuesRespone> ListIssueByOrigin = new List<IssuesRespone>();

        public IssueSortOrderType SortTypeSelected = IssueSortOrderType.CreatedDateDES;

        private bool isSelectedFavorites;
        public bool IsSelectedFavorites { get => isSelectedFavorites; set => SetProperty(ref isSelectedFavorites, value); }

        #endregion Property

        #region Lifecycle

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            EventAggregator.GetEvent<SelectDateTimeEvent>().Subscribe(UpdateDateTime);
            EventAggregator.GetEvent<SelectComboboxEvent>().Subscribe(UpdateCombobox);
            GetListIssue();
        }

        public override void OnDestroy()
        {
            EventAggregator.GetEvent<SelectDateTimeEvent>().Unsubscribe(UpdateDateTime);
            EventAggregator.GetEvent<SelectComboboxEvent>().Unsubscribe(UpdateCombobox);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
        }

        public override void OnPageAppearingFirstTime()
        {
            base.OnPageAppearingFirstTime();
        }

        #endregion Lifecycle

        #region PrivateMethod

        private void GetListIssue()
        {
            RunOnBackground(async () =>
            {
                return await _issueService.GetIssueByCompanyID(CurrentComanyID);
            }, (result) =>
            {
                ListIssueByOrigin = result;
                ListIssue = result.ToObservableCollection();
            });
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
                    FilterIssue();
                }
            }
        }

        private void SortIssue()
        {
            switch (SortTypeSelected)
            {
                case IssueSortOrderType.CreatedDateASC:
                    SortTypeSelected = IssueSortOrderType.CreatedDateDES;
                    ListIssue = ListIssue.OrderBy(x => x.CreatedDate).ToObservableCollection();
                    break;

                case IssueSortOrderType.CreatedDateDES:
                    SortTypeSelected = IssueSortOrderType.CreatedDateASC;
                    ListIssue = ListIssue.OrderByDescending(x => x.CreatedDate).ToObservableCollection();
                    break;
            }
        }

        private void SearchIssuewithText(TextChangedEventArgs args)
        {
            SafeExecute(() =>
            {
                var keySearch = string.Empty;
                if (args != null)
                {
                    keySearch = args.NewTextValue.ToUpper().Trim();
                }
                if (cts != null)
                    cts.Cancel(true);

                cts = new CancellationTokenSource();

                Task.Run(async () =>
                {
                    await Task.Delay(500, cts.Token);
                    if (cts.IsCancellationRequested)
                        return null;
                    return ListIssueByOrigin.Where(x => (x.Content.ToUpper().Contains(keySearch) || string.IsNullOrEmpty(keySearch))
                    && ((x.Status == (IssuesStatusEnums)StatusIssueSelected.Key)
                    || StatusIssueSelected.Key == 0
                    || StatusIssueSelected == null) && (x.IsFavorites == IsSelectedFavorites || !IsSelectedFavorites));
                }, cts.Token).ContinueWith(task => Device.BeginInvokeOnMainThread(() =>
                {
                    if (task.Status == TaskStatus.RanToCompletion && !cts.IsCancellationRequested)
                    {
                        ListIssue = new ObservableCollection<IssuesRespone>();

                        if (task.Result != null && task.Result.Count() > 0)
                        {
                            ListIssue = task.Result.ToObservableCollection();
                            SetSortOrder();
                        }
                    }
                    else if (task.IsFaulted)
                    {
                    }
                }));
            });
        }

        private void SetSortOrder()
        {
            switch (SortTypeSelected)
            {
                case IssueSortOrderType.CreatedDateASC:
                    ListIssue = ListIssue.OrderBy(x => x.CreatedDate).ToObservableCollection();
                    break;

                case IssueSortOrderType.CreatedDateDES:
                    ListIssue = ListIssue.OrderByDescending(x => x.CreatedDate).ToObservableCollection();
                    break;
            }
        }

        private void SelectFavoriteIssue()
        {
            IsSelectedFavorites = !IsSelectedFavorites;
            FilterIssue();
        }

        private bool CanLoadMoreItems()
        {
            return false;
            //if (ListIssue.Count <= pageIndex * pageCount)
            //    return false;
            //return true;
        }

        private void LoadMoreItems()
        {
            SafeExecute(() =>
            {
                LoadMore();
            });
        }

        private void LoadMore()
        {
            try
            {
                var source = listIssue.Skip(pageIndex * pageCount).Take(pageCount).ToList();
                pageIndex++;
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (source != null && source.Count() > 0)
                    {
                        for (int i = 0; i < source.Count; i++)
                        {
                            ListIssue.Add(source[i]);
                        }
                        SetSortOrder();
                    }
                });
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        private void SetFavoriteIssue(object obj)
        {
            if (obj != null && obj is IssuesRespone agrs)
            {
                var item = (IssuesRespone)agrs;
                var model = ListIssue.FirstOrDefault(x => x.Id == item.Id);
                if (model != null)
                {
                    model.IsFavorites = !item.IsFavorites;
                    FavoritesVehicleHelper.UpdateFavoritesIssue(item.Id);
                }
            }
        }

        private void FilterIssue()
        {
            var lst = ListIssueByOrigin.Where(x => (x.Content.ToUpper().Contains(SearchedText) || string.IsNullOrEmpty(SearchedText))
                                            && (x.Status == (IssuesStatusEnums)StatusIssueSelected.Key
                                            || StatusIssueSelected.Key == 0
                                            || StatusIssueSelected == null)
                                            && (x.IsFavorites == IsSelectedFavorites || !IsSelectedFavorites)).ToList();
            ListIssue = lst.ToObservableCollection();
            SetSortOrder();
        }

        public void Navigate(ItemTappedEventArgs args)
        {
            if (!(args.ItemData is IssuesRespone item))
            {
                return;
            }
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("IssuesDetailPage", parameters: new NavigationParameters
                             {
                                 { ParameterKey.IssuesKey, item }
                            });
            });
        }

        #endregion PrivateMethod
    }

    public enum IssueSortOrderType
    {
        CreatedDateASC,
        CreatedDateDES,
    }
}