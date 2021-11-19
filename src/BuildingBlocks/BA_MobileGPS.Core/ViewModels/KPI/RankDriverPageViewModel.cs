using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Extensions;
using ItemTappedEventArgs = Syncfusion.ListView.XForms.ItemTappedEventArgs;

namespace BA_MobileGPS.Core.ViewModels
{
    public class RankDriverPageViewModel : ViewModelBase
    {
        private readonly IKPIDriverService _kPIDriverService;
        public ICommand PushToFromDatePageCommand { get; private set; }
        public ICommand NextTimeCommand { get; private set; }
        public ICommand PreviosTimeCommand { get; private set; }
        public ICommand SwichDateTypeCommand { get; private set; }
        public ICommand SearchDriverCommand { get; private set; }
        public ICommand TapRankByDayCommand { get; private set; }
        public ICommand NavigateCommand { get; private set; }
        public ICommand NavigateRankUserCommand { get; private set; }

        public RankDriverPageViewModel(INavigationService navigationService,
            IKPIDriverService kPIDriverService) : base(navigationService)
        {
            _kPIDriverService = kPIDriverService;
            Title = MobileResource.Title_Kpi_Page;
            PushToFromDatePageCommand = new DelegateCommand(ExecuteToFromDate);
            NextTimeCommand = new DelegateCommand(NextTime);
            PreviosTimeCommand = new DelegateCommand(PreviosTime);
            SwichDateTypeCommand = new DelegateCommand(SwichDateType);
            SearchDriverCommand = new DelegateCommand<TextChangedEventArgs>(SearchDriverwithText);
            NavigateCommand = new DelegateCommand<DriverRankByDay>(Navigate);
            NavigateRankUserCommand = new DelegateCommand<DriverRankingRespone>(NavigateRankUser);
            TapRankByDayCommand = new DelegateCommand<ItemTappedEventArgs>(TapRankByDay);
        }

        #region Property RankPoint

        private bool isLoading = true;
        public bool IsLoading { get => isLoading; set => SetProperty(ref isLoading, value); }

        private int selectedTabIndex = 0;
        public int SelectedTabIndex { get => selectedTabIndex; set => SetProperty(ref selectedTabIndex, value); }

        private DateTime fromDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1, 0, 0, 0);
        public DateTime FromDate { get => fromDate; set => SetProperty(ref fromDate, value); }

        private ObservableCollection<DriverRankByDay> rankPointSource = new ObservableCollection<DriverRankByDay>();
        public ObservableCollection<DriverRankByDay> RankPointSource { get => rankPointSource; set => SetProperty(ref rankPointSource, value); }

        private DriverRankByDay selectedRankPoint = new DriverRankByDay();
        public DriverRankByDay SelectedRankPoint { get => selectedRankPoint; set => SetProperty(ref selectedRankPoint, value); }

        private DriverRankingRespone averageRankPoint = new DriverRankingRespone();
        public DriverRankingRespone AverageRankPoint { get => averageRankPoint; set => SetProperty(ref averageRankPoint, value); }

        #endregion Property RankPoint

        private CancellationTokenSource cts;

        #region Property RankUserPoint

        private DateTime dateRank = DateTime.Today;
        public DateTime DateRank { get => dateRank; set => SetProperty(ref dateRank, value); }

        public string searchedText;
        public string SearchedText { get => searchedText; set => SetProperty(ref searchedText, value); }

        public bool isShowMonth;
        public bool IsShowMonth { get => isShowMonth; set => SetProperty(ref isShowMonth, value); }

        private List<DriverRankingRespone> ListRankUserOrigin = new List<DriverRankingRespone>();

        private ObservableCollection<DriverRankingRespone> rankUserSource = new ObservableCollection<DriverRankingRespone>();
        public ObservableCollection<DriverRankingRespone> RankUserSource { get => rankUserSource; set => SetProperty(ref rankUserSource, value); }

        private DriverRankingRespone userRank1 = new DriverRankingRespone();
        public DriverRankingRespone UserRank1 { get => userRank1; set => SetProperty(ref userRank1, value); }

        private DriverRankingRespone userRank2 = new DriverRankingRespone();
        public DriverRankingRespone UserRank2 { get => userRank2; set => SetProperty(ref userRank2, value); }

        private DriverRankingRespone userRank3 = new DriverRankingRespone();
        public DriverRankingRespone UserRank3 { get => userRank3; set => SetProperty(ref userRank3, value); }

        #endregion Property RankUserPoint

        #region Lifecycle

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            EventAggregator.GetEvent<SelectMonthEvent>().Subscribe(UpdateDateTime);
            EventAggregator.GetEvent<SelectDateEvent>().Subscribe(UpdateDateTime);
            GetListRankPoint(true);
        }

        public override void OnDestroy()
        {
            EventAggregator.GetEvent<SelectMonthEvent>().Unsubscribe(UpdateDateTime);
            EventAggregator.GetEvent<SelectDateEvent>().Unsubscribe(UpdateDateTime);
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

        private void PreviosTime()
        {
            SafeExecute(() =>
            {
                if (SelectedTabIndex == 0)
                {
                    FromDate = FromDate.AddMonths(-1);
                    GetListRankPoint();
                }
                else
                {
                    if (IsShowMonth)
                    {
                        DateRank = DateRank.AddMonths(-1);
                    }
                    else
                    {
                        DateRank = DateRank.AddDays(-1);
                    }
                    GetListUserRank();
                }
            });
        }

        private void NextTime()
        {
            SafeExecute(() =>
            {
                if (SelectedTabIndex == 0)
                {
                    if (FromDate.Month == DateTime.Now.Month && FromDate.Year == DateTime.Now.Year)
                    {
                        return;
                    }
                    else
                    {
                        FromDate = FromDate.AddMonths(1);
                        GetListRankPoint();
                    }
                }
                else
                {
                    if (IsShowMonth)
                    {
                        if (DateRank.Month == DateTime.Now.Month && DateRank.Year == DateTime.Now.Year)
                        {
                            return;
                        }
                        else
                        {
                            DateRank = DateRank.AddMonths(1);
                        }
                    }
                    else
                    {
                        if (DateRank.Date < DateTime.Now.Date)
                        {
                            DateRank = DateRank.AddDays(1);
                        }
                        else
                        {
                            return;
                        }
                    }
                    GetListUserRank();
                }
            });
        }

        private void SwichDateType()
        {
            SafeExecute(() =>
            {
                IsShowMonth = !IsShowMonth;
                GetListUserRank();
            });
        }

        private void SearchDriverwithText(TextChangedEventArgs args)
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
                    return ListRankUserOrigin.Where(x => x.DriverName.ToUpper().Contains(keySearch) || string.IsNullOrEmpty(keySearch)).OrderByDescending(x => x.AverageScore);
                }, cts.Token).ContinueWith(task => Device.BeginInvokeOnMainThread(() =>
                {
                    if (task.Status == TaskStatus.RanToCompletion && !cts.IsCancellationRequested)
                    {
                        RankUserSource = new ObservableCollection<DriverRankingRespone>();

                        if (task.Result != null && task.Result.Count() > 0)
                        {
                            RankUserSource = task.Result.ToObservableCollection();
                        }
                    }
                    else if (task.IsFaulted)
                    {
                    }
                }));
            });
        }

        private void ValidateDateTime()
        {
        }

        private void GetListRankPoint(bool isFistLoad = false)
        {
            var request = new Entities.DriverRankingRequest()
            {
                CompanyID = CurrentComanyID,
                FromDate = FromDate,
                ToDate = FromDate.AddMonths(1).AddDays(-1),
                UserIDs = new string[] { UserInfo.UserId.ToString() }
            };
            RunOnBackground(async () =>
            {
                return await _kPIDriverService.GetDriverRanking(request);
            }, (result) =>
            {
                if (result != null && result.Count > 0)
                {
                    var info = result.First();
                    if (info != null)
                    {
                        RankPointSource = info.DriverRankByDay.OrderByDescending(x => x.Date).ToObservableCollection();
                        RankPointSource[0].BacgroundColor = Color.FromHex("#6FDCFF");
                        SelectedRankPoint = RankPointSource[0];
                        AverageRankPoint = info;
                    }
                }
                else
                {
                    RankPointSource = new ObservableCollection<DriverRankByDay>();
                    SelectedRankPoint = new DriverRankByDay();
                    AverageRankPoint = new DriverRankingRespone();
                }
                if (isFistLoad)
                {
                    GetListUserRank(false);
                }
            }, showLoading: true);
        }

        private void GetListUserRank(bool isloading = true)
        {
            var fromDate = DateRank;
            var toDate = DateRank;
            if (IsShowMonth)
            {
                fromDate = new DateTime(fromDate.Year, fromDate.Month, 1, 0, 0, 0);
                toDate = new DateTime(toDate.Year, toDate.Month, 1, 0, 0, 0).AddMonths(1).AddDays(-1);
            }
            var request = new Entities.DriverRankingRequest()
            {
                CompanyID = CurrentComanyID,
                FromDate = fromDate.Date,
                ToDate = toDate.Date.AddDays(1).AddMinutes(-1),
                UserIDs = new string[] { }
            };
            RunOnBackground(async () =>
            {
                return await _kPIDriverService.GetDriverRanking(request);
            }, (result) =>
            {
                if (result != null && result.Count > 0)
                {
                    for (int i = 0; i < result.Count; i++)
                    {
                        result[i].STT = i + 1;
                        if (result[i].DriverId == AverageRankPoint.DriverId)
                        {
                            result[i].BacgroundYourDriver = Color.FromHex("#6FDCFF");
                        }
                        else
                        {
                            result[i].BacgroundYourDriver = Color.FromHex("#D6F3FF");
                        }
                    }
                    ListRankUserOrigin = result;
                    RankUserSource = result.ToObservableCollection();
                    var lstUserShowRank = result.OrderByDescending(x => x.AverageScore).Take(3).ToList();
                    if (lstUserShowRank != null && lstUserShowRank.Count <= 3)
                    {
                        UserRank1 = new DriverRankingRespone();
                        UserRank2 = new DriverRankingRespone();
                        UserRank3 = new DriverRankingRespone();
                        for (int i = 0; i < lstUserShowRank.Count; i++)
                        {
                            if (i == 0)
                            {
                                UserRank1 = lstUserShowRank[0];
                            }
                            if (i == 1)
                            {
                                UserRank2 = lstUserShowRank[1];
                            }
                            if (i == 2)
                            {
                                UserRank3 = lstUserShowRank[2];
                            }
                        }
                    }
                }
                else
                {
                    RankUserSource = new ObservableCollection<DriverRankingRespone>();
                    UserRank1 = new DriverRankingRespone();
                    UserRank2 = new DriverRankingRespone();
                    UserRank3 = new DriverRankingRespone();
                }
            }, showLoading: isloading);
        }

        public void UpdateDateTime(PickerDateResponse param)
        {
            if (param != null)
            {
                if (param.PickerType == (short)ComboboxType.First)
                {
                    if (SelectedTabIndex == 0)
                    {
                        if (param.Value.Month >= DateTime.Now.Month && param.Value.Year >= DateTime.Now.Year)
                        {
                            DisplayMessage.ShowMessageInfo("Tháng tìm kiếm không được lớn hơn ngày hiện tại");
                            return;
                        }
                        else
                        {
                            FromDate = param.Value;
                            GetListRankPoint();
                        }
                    }
                    else
                    {
                        if (IsShowMonth)
                        {
                            if (param.Value.Month >= DateTime.Now.Month && param.Value.Year >= DateTime.Now.Year)
                            {
                                DisplayMessage.ShowMessageInfo("Tháng tìm kiếm không được lớn hơn ngày hiện tại");
                                return;
                            }
                            else
                            {
                                DateRank = param.Value;
                            }
                        }
                        else
                        {
                            if (param.Value.Date < DateTime.Now.Date)
                            {
                                DateRank = param.Value;
                            }
                            else
                            {
                                DisplayMessage.ShowMessageInfo("Ngày tìm kiếm không được lớn hơn ngày hiện tại");
                                return;
                            }
                        }
                        GetListUserRank();
                    }
                }
            }
        }

        public void ExecuteToFromDate()
        {
            SafeExecute(async () =>
            {
                DateTime date = FromDate;
                if (SelectedTabIndex == 0)
                {
                    date = FromDate;
                }
                else
                {
                    date = DateRank;
                }
                var parameters = new NavigationParameters
                {
                    { "DataPicker", date },
                    { "PickerType", ComboboxType.First }
                };
                if (SelectedTabIndex == 1 && !IsShowMonth)
                {
                    await NavigationService.NavigateAsync("SelectDateCalendar", parameters);
                }
                else
                {
                    await NavigationService.NavigateAsync("SelectMonthCalendar", parameters);
                }
            });
        }

        public void Navigate(DriverRankByDay args)
        {
            SafeExecute(async () =>
            {
                var parameters = new NavigationParameters
                    {
                        { ParameterKey.KPIRankDriverID,AverageRankPoint.DriverId },
                         { ParameterKey.KPIRankPage,args.Date },
                    };
                await NavigationService.NavigateAsync("KpiDriverChartPage", parameters);
            });
        }

        private void NavigateRankUser(DriverRankingRespone obj)
        {
            if (!IsShowMonth)
            {
                SafeExecute(async () =>
                {
                    var parameters = new NavigationParameters
                    {
                        { ParameterKey.KPIRankDriverID,AverageRankPoint.DriverId },
                         { ParameterKey.KPIRankPage,DateRank },
                    };
                    await NavigationService.NavigateAsync("KpiDriverChartPage", parameters);
                });
            }
            else
            {
                DisplayMessage.ShowMessageInfo("Chỉ hỗ trợ xem biểu đồ theo ngày");
            }
        }

        private void TapRankByDay(ItemTappedEventArgs obj)
        {
            if (!(obj.ItemData is DriverRankByDay item))
                return;
            foreach (var itemdata in RankPointSource)
            {
                if (itemdata.Date == item.Date)
                {
                    itemdata.BacgroundColor = Color.FromHex("#6FDCFF");
                }
                else
                {
                    itemdata.BacgroundColor = Color.FromHex("#E4E4E4");
                }
            }
        }

        #endregion PrivateMethod
    }
}