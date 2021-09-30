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

        public RankDriverPageViewModel(INavigationService navigationService, IKPIDriverService kPIDriverService) : base(navigationService)
        {
            _kPIDriverService = kPIDriverService;
            Title = "Điểm xếp hạng lái xe";
            PushToFromDatePageCommand = new DelegateCommand(ExecuteToFromDate);
            NextTimeCommand = new DelegateCommand(NextTime);
            PreviosTimeCommand = new DelegateCommand(PreviosTime);
            SwichDateTypeCommand = new DelegateCommand(SwichDateType);
            SearchDriverCommand = new DelegateCommand<TextChangedEventArgs>(SearchDriverwithText);
        }

        #region Property RankPoint

        private bool isLoading = true;
        public bool IsLoading { get => isLoading; set => SetProperty(ref isLoading, value); }

        private int selectedTabIndex = 0;
        public int SelectedTabIndex { get => selectedTabIndex; set => SetProperty(ref selectedTabIndex, value); }

        private DateTime fromDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1, 0, 0, 0);
        public virtual DateTime FromDate { get => fromDate; set => SetProperty(ref fromDate, value); }

        private ObservableCollection<DriverRankByDay> rankPointSource = new ObservableCollection<DriverRankByDay>();
        public ObservableCollection<DriverRankByDay> RankPointSource { get => rankPointSource; set => SetProperty(ref rankPointSource, value); }

        private DriverRankByDay selectedRankPoint = new DriverRankByDay();
        public DriverRankByDay SelectedRankPoint { get => selectedRankPoint; set => SetProperty(ref selectedRankPoint, value); }

        private DriverRankingRespone averageRankPoint = new DriverRankingRespone();
        public DriverRankingRespone AverageRankPoint { get => averageRankPoint; set => SetProperty(ref averageRankPoint, value); }

        #endregion Property RankPoint

        private CancellationTokenSource cts;

        #region Property RankUserPoint

        private DateTime dateRank = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 23, 59, 59);
        public virtual DateTime DateRank { get => dateRank; set => SetProperty(ref dateRank, value); }

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
            EventAggregator.GetEvent<SelectDateTimeEvent>().Subscribe(UpdateDateTime);
            GetListRankPoint();
            GetListUserRank();
        }

        public override void OnDestroy()
        {
            EventAggregator.GetEvent<SelectDateTimeEvent>().Unsubscribe(UpdateDateTime);
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
                    FromDate = FromDate.AddDays(-1);
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
                }
            });
        }

        private void NextTime()
        {
            SafeExecute(() =>
            {
                if (SelectedTabIndex == 0)
                {
                    FromDate = FromDate.AddDays(1);
                }
                else
                {
                    if (IsShowMonth)
                    {
                        DateRank = DateRank.AddMonths(1);
                    }
                    else
                    {
                        DateRank = DateRank.AddDays(1);
                    }
                }
            });
        }

        private void SwichDateType()
        {
            SafeExecute(() =>
            {
                IsShowMonth = !IsShowMonth;
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

        private void GetListRankPoint()
        {
            var request = new Entities.DriverRankingRequest()
            {
                CompanyID = CurrentComanyID,
                FromDate = FromDate,
                ToDate = FromDate,
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
                        RankPointSource = info.DriverRankByDay.ToObservableCollection();
                        SelectedRankPoint = info.DriverRankByDay[0];
                        AverageRankPoint = info;
                    }
                }
            });
        }

        private void GetListUserRank()
        {
            var toDate = DateRank;
            if (IsShowMonth)
            {
                toDate = DateRank.AddMonths(1).AddDays(-1);
            }
            var request = new Entities.DriverRankingRequest()
            {
                CompanyID = CurrentComanyID,
                FromDate = DateRank,
                ToDate = toDate
            };
            RunOnBackground(async () =>
            {
                return await _kPIDriverService.GetDriverRanking(request);
            }, (result) =>
            {
                if (result != null && result.Count > 0)
                {
                    RankUserSource = result.ToObservableCollection();
                    var lstUserShowRank = result.OrderByDescending(x => x.AverageScore).Take(3).ToList();
                    if (lstUserShowRank != null && lstUserShowRank.Count == 3)
                    {
                        UserRank1 = lstUserShowRank[0];
                        UserRank2 = lstUserShowRank[1];
                        UserRank3 = lstUserShowRank[2];
                    }
                }
            });
        }

        public void UpdateDateTime(PickerDateTimeResponse param)
        {
            if (param != null)
            {
                if (param.PickerType == (short)ComboboxType.First)
                {
                    if (SelectedTabIndex == 0)
                    {
                        FromDate = param.Value;
                    }
                    else
                    {
                        DateRank = param.Value;
                    }
                }
                ValidateDateTime();
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
                await NavigationService.NavigateAsync("SelectDateTimeCalendar", parameters);
            });
        }

        #endregion PrivateMethod
    }

    public class UserRank
    {
        public int STT { get; set; }
        public string DriverName { get; set; }
        public string DriverAvatar { get; set; }
        public int Point { get; set; }
        public string Rank { get; set; }
    }
}