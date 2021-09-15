using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace BA_MobileGPS.Core.ViewModels
{
    public class RankDriverPageViewModel : ViewModelBase
    {
        public ICommand PushToFromDatePageCommand { get; private set; }
        public ICommand NextTimeCommand { get; private set; }
        public ICommand PreviosTimeCommand { get; private set; }

        public RankDriverPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Điểm xếp hạng lái xe";
            PushToFromDatePageCommand = new DelegateCommand(ExecuteToFromDate);
            NextTimeCommand = new DelegateCommand(NextTime);
            PreviosTimeCommand = new DelegateCommand(PreviosTime);
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

        #region Property RankPoint

        private DateTime dateRank = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 23, 59, 59);
        public virtual DateTime DateRank { get => dateRank; set => SetProperty(ref dateRank, value); }

        public string searchedText;
        public string SearchedText { get => searchedText; set => SetProperty(ref searchedText, value); }

        private ObservableCollection<RankPoint> rankUserSource = new ObservableCollection<RankPoint>();
        public ObservableCollection<RankPoint> RankUserSource { get => rankPointSource; set => SetProperty(ref rankPointSource, value); }

        #endregion Property RankPoint

        #region Lifecycle

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            EventAggregator.GetEvent<SelectDateTimeEvent>().Subscribe(UpdateDateTime);
            GetListRankPoint();
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
                    DateRank = DateRank.AddDays(-1);
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
                    if (ToDate.Subtract(DateRank).TotalDays >= 1)
                    {
                        DateRank = DateRank.AddDays(1);
                    }
                }
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
                var parameters = new NavigationParameters
                {
                    { "DataPicker", FromDate },
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
}