using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
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
        public ICommand PushToFromDatePageCommand { get; private set; }
        public ICommand NextTimeCommand { get; private set; }
        public ICommand PreviosTimeCommand { get; private set; }
        public ICommand SwichDateTypeCommand { get; private set; }
        public ICommand SearchDriverCommand { get; private set; }

        public RankDriverPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Điểm xếp hạng lái xe";
            PushToFromDatePageCommand = new DelegateCommand(ExecuteToFromDate);
            NextTimeCommand = new DelegateCommand(NextTime);
            PreviosTimeCommand = new DelegateCommand(PreviosTime);
            SwichDateTypeCommand = new DelegateCommand(SwichDateType);
            SearchDriverCommand = new DelegateCommand<TextChangedEventArgs>(SearchDriverwithText);
        }

        #region Property RankPoint

        private int selectedTabIndex = 0;
        public int SelectedTabIndex { get => selectedTabIndex; set => SetProperty(ref selectedTabIndex, value); }

        private DateTime fromDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1, 0, 0, 0);
        public virtual DateTime FromDate { get => fromDate; set => SetProperty(ref fromDate, value); }

        private DateTime toDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 23, 59, 59);
        public virtual DateTime ToDate { get => toDate; set => SetProperty(ref toDate, value); }

        private ObservableCollection<RankPoint> rankPointSource = new ObservableCollection<RankPoint>();
        public ObservableCollection<RankPoint> RankPointSource { get => rankPointSource; set => SetProperty(ref rankPointSource, value); }

        private RankPoint myRankPoint = new RankPoint();
        public RankPoint MyRankPoint { get => myRankPoint; set => SetProperty(ref myRankPoint, value); }
        private RankPoint averageRankPoint = new RankPoint();
        public RankPoint AverageRankPoint { get => averageRankPoint; set => SetProperty(ref averageRankPoint, value); }

        #endregion Property RankPoint
        private CancellationTokenSource cts;

        #region Property RankPoint

        private DateTime dateRank = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 23, 59, 59);
        public virtual DateTime DateRank { get => dateRank; set => SetProperty(ref dateRank, value); }

        public string searchedText;
        public string SearchedText { get => searchedText; set => SetProperty(ref searchedText, value); }

        public bool isShowMonth;
        public bool IsShowMonth { get => isShowMonth; set => SetProperty(ref isShowMonth, value); }

        private List<UserRank> ListRankUserOrigin = new List<UserRank>();

        private ObservableCollection<UserRank> rankUserSource = new ObservableCollection<UserRank>();
        public ObservableCollection<UserRank> RankUserSource { get => rankUserSource; set => SetProperty(ref rankUserSource, value); }

        private UserRank userRank1 = new UserRank();
        public UserRank UserRank1 { get => userRank1; set => SetProperty(ref userRank1, value); }

        private UserRank userRank2 = new UserRank();
        public UserRank UserRank2 { get => userRank2; set => SetProperty(ref userRank2, value); }

        private UserRank userRank3 = new UserRank();
        public UserRank UserRank3 { get => userRank3; set => SetProperty(ref userRank3, value); }

        #endregion Property RankPoint

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
                    if (ToDate.Subtract(FromDate).TotalDays >= 1)
                    {
                        FromDate = FromDate.AddDays(1);
                    }
                }
                else
                {
                    if (IsShowMonth)
                    {
                        if (ToDate.Month > DateRank.Month)
                        {
                            DateRank = DateRank.AddMonths(1);
                        }
                    }
                    else
                    {
                        if (ToDate.Subtract(DateRank).TotalDays >= 1)
                        {
                            DateRank = DateRank.AddDays(1);
                        }
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
                    return ListRankUserOrigin.Where(x => x.DriverName.ToUpper().Contains(keySearch) || string.IsNullOrEmpty(keySearch)).OrderByDescending(x => x.Point);
                }, cts.Token).ContinueWith(task => Device.BeginInvokeOnMainThread(() =>
                {
                    if (task.Status == TaskStatus.RanToCompletion && !cts.IsCancellationRequested)
                    {
                        RankUserSource = new ObservableCollection<UserRank>();

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
            if (SelectedTabIndex == 0)
            {
                if (FromDate > ToDate)
                {
                    DisplayMessage.ShowMessageInfo(MobileResource.Route_Label_StartDateMustSmallerThanEndDate);
                }
                else if (ToDate.Subtract(FromDate).TotalDays > 60)
                {
                    DisplayMessage.ShowMessageInfo(string.Format(MobileResource.Route_Label_TotalTimeLimit, 60));
                }
            }
            else
            {
                if (DateRank > ToDate)
                {
                    DisplayMessage.ShowMessageInfo(MobileResource.Route_Label_StartDateMustSmallerThanEndDate);
                }
                else if (ToDate.Subtract(DateRank).TotalDays > 60)
                {
                    DisplayMessage.ShowMessageInfo(string.Format(MobileResource.Route_Label_TotalTimeLimit, 60));
                }
            }
        }

        private void GetListRankPoint()
        {
            RankPointSource = new ObservableCollection<RankPoint>()
            {
                new RankPoint()
                {
                    Point=90,
                    Rank="A",
                    Time=DateTime.Now
                },
                 new RankPoint()
                {
                    Point=80,
                    Rank="B",
                    Time=DateTime.Now
                },
                  new RankPoint()
                {
                    Point=70,
                    Rank="A",
                    Time=DateTime.Now
                },
                   new RankPoint()
                {
                    Point=60,
                    Rank="C",
                    Time=DateTime.Now
                },
                    new RankPoint()
                {
                    Point=50,
                    Rank="D",
                    Time=DateTime.Now
                },
                     new RankPoint()
                {
                    Point=40,
                    Rank="E",
                    Time=DateTime.Now
                }
            };
            MyRankPoint = RankPointSource[0];
            var average = new RankPoint();
            average.Point = (int)RankPointSource.Average(x => x.Point);
            average.Rank = "A";
            AverageRankPoint = average;
        }

        private void GetListUserRank()
        {
            RankUserSource = new ObservableCollection<UserRank>()
            {
                new UserRank()
                {
                    Point=90,
                    Rank="A",
                    DriverName="Trần Hoàng Nam",
                    DriverAvatar="https://photo-cms-nghenhinvietnam.zadn.vn/w700/Uploaded/2021/unvjapu/2019_03_28/quang-hai/7_thgo.jpg"
                },
                new UserRank()
                {
                    Point=80,
                    Rank="B",
                    DriverName="Hoàng Thanh Bình",
                    DriverAvatar="https://photo-cms-nghenhinvietnam.zadn.vn/w700/Uploaded/2021/unvjapu/2019_03_28/quang-hai/7_thgo.jpg"
                },
                new UserRank()
                {
                    Point=70,
                    Rank="C",
                    DriverName="Trần Quang Trung",
                    DriverAvatar="https://photo-cms-nghenhinvietnam.zadn.vn/w700/Uploaded/2021/unvjapu/2019_03_28/quang-hai/7_thgo.jpg"
                },
                new UserRank()
                {
                    Point=60,
                    Rank="D",
                    DriverName="Nguyễn Thị Huyền Trang",
                    DriverAvatar="https://photo-cms-nghenhinvietnam.zadn.vn/w700/Uploaded/2021/unvjapu/2019_03_28/quang-hai/7_thgo.jpg"
                },
                 new UserRank()
                {
                    Point=60,
                    Rank="D",
                    DriverName="Nguyễn Thị Huyền Trang",
                    DriverAvatar="https://photo-cms-nghenhinvietnam.zadn.vn/w700/Uploaded/2021/unvjapu/2019_03_28/quang-hai/7_thgo.jpg"
                },
                  new UserRank()
                {
                    Point=60,
                    Rank="D",
                    DriverName="Nguyễn Thị Huyền Trang",
                    DriverAvatar="https://photo-cms-nghenhinvietnam.zadn.vn/w700/Uploaded/2021/unvjapu/2019_03_28/quang-hai/7_thgo.jpg"
                },
            };
            ListRankUserOrigin = RankUserSource.ToList();
            var lstUserShowRank = RankUserSource.OrderByDescending(x => x.Point).Take(3).ToList();
            if (lstUserShowRank != null && lstUserShowRank.Count == 3)
            {
                UserRank1 = lstUserShowRank[0];
                UserRank2 = lstUserShowRank[1];
                UserRank3 = lstUserShowRank[2];
            }
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

    public class RankPoint
    {
        public DateTime Time { get; set; }
        public int Point { get; set; }
        public string Rank { get; set; }
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