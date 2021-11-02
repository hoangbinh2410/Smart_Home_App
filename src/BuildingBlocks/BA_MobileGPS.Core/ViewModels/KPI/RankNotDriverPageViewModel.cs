using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Entities;
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

namespace BA_MobileGPS.Core.ViewModels
{
    public class RankNotDriverPageViewModel : ViewModelBase
    {
        private readonly IKPIDriverService _kPIDriverService;
        public ICommand PushToFromDatePageCommand { get; private set; }
        public ICommand NextTimeCommand { get; private set; }
        public ICommand PreviosTimeCommand { get; private set; }
        public ICommand SwichDateTypeCommand { get; private set; }
        public ICommand SearchDriverCommand { get; private set; }
        public ICommand TapRankByDayCommand { get; private set; }
        public ICommand NavigateCommand { get; private set; }

        public RankNotDriverPageViewModel(INavigationService navigationService,
            IKPIDriverService kPIDriverService) : base(navigationService)
        {
            _kPIDriverService = kPIDriverService;
            Title = "Điểm xếp hạng lái xe";
            PushToFromDatePageCommand = new DelegateCommand(ExecuteToFromDate);
            NextTimeCommand = new DelegateCommand(NextTime);
            PreviosTimeCommand = new DelegateCommand(PreviosTime);
            SwichDateTypeCommand = new DelegateCommand(SwichDateType);
            SearchDriverCommand = new DelegateCommand<TextChangedEventArgs>(SearchDriverwithText);
            NavigateCommand = new DelegateCommand<DriverRankByDay>(Navigate);
        }

        #region Property RankPoint

        private bool isLoading = true;
        public bool IsLoading { get => isLoading; set => SetProperty(ref isLoading, value); }

        private DateTime fromDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1, 0, 0, 0);
        public DateTime FromDate { get => fromDate; set => SetProperty(ref fromDate, value); }

        private DriverRankingRespone averageRankPoint = new DriverRankingRespone();
        public DriverRankingRespone AverageRankPoint { get => averageRankPoint; set => SetProperty(ref averageRankPoint, value); }

        #endregion Property RankPoint

        private CancellationTokenSource cts;

        #region Property RankUserPoint

        private DateTime dateRank = DateTime.Now.Date;
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
            GetListUserRank();
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
                if (IsShowMonth)
                {
                    DateRank = DateRank.AddMonths(-1);
                }
                else
                {
                    DateRank = DateRank.AddDays(-1);
                }
                GetListUserRank();
            });
        }

        private void NextTime()
        {
            SafeExecute(() =>
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

        private void GetListUserRank()
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
            }, showLoading: true);
        }

        public void UpdateDateTime(PickerDateResponse param)
        {
            if (param != null)
            {
                if (param.PickerType == (short)ComboboxType.First)
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

        public void ExecuteToFromDate()
        {
            SafeExecute(async () =>
            {
                DateTime date = DateRank;
                var parameters = new NavigationParameters
                {
                    { "DataPicker", date },
                    { "PickerType", ComboboxType.First }
                };
                if (!IsShowMonth)
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
            try
            {
                SafeExecute(async () =>
                {
                    var parameters = new NavigationParameters
                    {
                        { ParameterKey.KPIRankDriverID,AverageRankPoint.DriverId },
                         { ParameterKey.KPIRankPage,args },
                    };
                    await NavigationService.NavigateAsync("KpiDriverChartPage", parameters);
                });
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        #endregion PrivateMethod
    }
}