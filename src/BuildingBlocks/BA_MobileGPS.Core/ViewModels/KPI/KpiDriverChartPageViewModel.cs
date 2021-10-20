using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;
using Prism.Commands;
using Prism.Navigation;
using Syncfusion.SfChart.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace BA_MobileGPS.Core.ViewModels
{
    public class KpiDriverChartPageViewModel : ViewModelBase
    {
        private readonly IKPIDriverService _KPIDriverService;
        public ICommand PushToFromDatePageCommand { get; private set; }
        public ICommand NextTimeCommand { get; private set; }
        public ICommand PreviosTimeCommand { get; private set; }

        public KpiDriverChartPageViewModel(INavigationService navigationService, IKPIDriverService KPIDriverService) : base(navigationService)
        {
            Title = "Điểm xếp hạng lái xe";
            _KPIDriverService = KPIDriverService;
            PushToFromDatePageCommand = new DelegateCommand(ExecuteToFromDate);
            NextTimeCommand = new DelegateCommand(NextTime);
            PreviosTimeCommand = new DelegateCommand(PreviosTime);
        }

        #region Lifecycle

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            EventAggregator.GetEvent<SelectDateTimeEvent>().Subscribe(UpdateDateTime);
            if (parameters != null)
            {
                if (parameters.GetValue<int>(ParameterKey.KPIRankDriverID) is int driverID
                    && parameters.TryGetValue(ParameterKey.KPIRankPage, out DriverRankByDay rankByDay))
                {
                    DriverID = driverID;
                    DateSearch = rankByDay.Date;
                    GetDriverKpiChart(driverID);
                }
            }

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

        #region Property

        private bool isLoading = true;
        public bool IsLoading { get => isLoading; set => SetProperty(ref isLoading, value); }

        private int selectedTabIndex = 0;
        public int SelectedTabIndex { get => selectedTabIndex; set => SetProperty(ref selectedTabIndex, value); }

        private int driverID = 0;
        public int DriverID { get => driverID; set => SetProperty(ref driverID, value); }

        private DateTime dateSearch = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1, 0, 0, 0);
        public virtual DateTime DateSearch { get => dateSearch; set => SetProperty(ref dateSearch, value); }

        private ChartSeriesCollection chartSeries = new ChartSeriesCollection();
        public ChartSeriesCollection ChartSeries { get => chartSeries; set => SetProperty(ref chartSeries, value); }

        private ChartSeriesCollection chartSeries2 = new ChartSeriesCollection();
        public ChartSeriesCollection ChartSeries2 { get => chartSeries2; set => SetProperty(ref chartSeries2, value); }

        public ObservableCollection<ChartDataModel> ChartDataSource { get; set; }

        public ObservableCollection<ChartDataModel> ChartDataSourceActual { get; set; }

        public ObservableCollection<ChartDataModel> ChartDataSource2 { get; set; }

        public ObservableCollection<ChartDataModel> ChartDataSourceActual2 { get; set; }

        private EvaluationsByType evaluationType1 = new EvaluationsByType();
        public EvaluationsByType EvaluationType1 { get => evaluationType1; set => SetProperty(ref evaluationType1, value); }

        private EvaluationsByType evaluationType2 = new EvaluationsByType();
        public EvaluationsByType EvaluationType2 { get => evaluationType2; set => SetProperty(ref evaluationType2, value); }

        #endregion Property

        private void GetDriverKpiChart(int driverID)
        {
            IsLoading = true;
            RunOnBackground(async () =>
            {
                return await _KPIDriverService.GetDriverKpiChart(new Entities.DriverKpiChartRequest()
                {
                    DriverID = driverID,
                    FromDate = DateSearch.ToString("dd/MM/yyyy"),
                    ToDate = DateSearch.ToString("dd/MM/yyyy")
                });
            }, (result) =>
            {
                if (result != null && result.EvaluationsByType != null && result.EvaluationsByType.Count > 0)
                {
                    GenChartEvaluationType1(result.EvaluationsByType);
                    GenChartEvaluationType2(result.EvaluationsByType);
                }
                IsLoading = false;
            });
        }

        private void GetDriverKpiChartSearch()
        {
            RunOnBackground(async () =>
            {
                return await _KPIDriverService.GetDriverKpiChart(new Entities.DriverKpiChartRequest()
                {
                    DriverID = DriverID,
                    FromDate = DateSearch.ToString("dd/MM/yyyy"),
                    ToDate = DateSearch.ToString("dd/MM/yyyy")
                });
            }, (result) =>
            {
                if (result != null && result.EvaluationsByType != null && result.EvaluationsByType.Count > 0)
                {
                    GenChartEvaluationType1(result.EvaluationsByType);
                    GenChartEvaluationType2(result.EvaluationsByType);
                }
            }, showLoading: true);
        }

        private void GenChartEvaluationType1(List<EvaluationsByType> list)
        {
            if (list != null && list.Count > 0)
            {
                ChartDataSource = new ObservableCollection<ChartDataModel>();
                ChartDataSourceActual = new ObservableCollection<ChartDataModel>();
                var ItemEvaluation = list.FirstOrDefault(x => x.TypeId == 1);
                if (ItemEvaluation != null)
                {
                    EvaluationType1 = ItemEvaluation;
                    foreach (var item in ItemEvaluation.Evaluations)
                    {
                        var rank = GetTilteRank(item.OrderDisPlay);
                        ChartDataSource.Add(new ChartDataModel(item.Name, rank.Value));
                        if (item.OrderDisPlayActual != null)
                        {
                            var rankactual = GetTilteRank(item.OrderDisPlayActual.GetValueOrDefault());
                            ChartDataSourceActual.Add(new ChartDataModel(item.Name, rankactual.Value));
                        }
                    }
                    DrawChartSeries();
                }
            }
        }

        private void GenChartEvaluationType2(List<EvaluationsByType> list)
        {
            if (list != null && list.Count > 0)
            {
                ChartDataSource2 = new ObservableCollection<ChartDataModel>();
                ChartDataSourceActual2 = new ObservableCollection<ChartDataModel>();
                var ItemEvaluation = list.FirstOrDefault(x => x.TypeId == 2);
                if (ItemEvaluation != null)
                {
                    EvaluationType2 = ItemEvaluation;
                    foreach (var item in ItemEvaluation.Evaluations)
                    {
                        var rank = GetTilteRank(item.OrderDisPlay);
                        ChartDataSource2.Add(new ChartDataModel(item.Name, rank.Value));
                        if (item.OrderDisPlayActual != null)
                        {
                            var rankactual = GetTilteRank(item.OrderDisPlayActual.GetValueOrDefault());
                            ChartDataSourceActual2.Add(new ChartDataModel(item.Name, rankactual.Value));
                        }
                    }
                    DrawChartSeries2();
                }
            }
        }

        private void DrawChartSeries()
        {
            ChartSeries.Clear();
            var chart = new ChartSeriesCollection();
            var series = new RadarSeries()
            {
                Label = "Điểm tiêu chuẩn",
                Opacity = .6,
                StrokeWidth = 2,
                DrawType = PolarRadarSeriesDrawType.Line,
                LegendIcon = ChartLegendIcon.Rectangle,
                XBindingPath = nameof(ChartDataModel.Name),
                YBindingPath = nameof(ChartDataModel.Value),
                ItemsSource = ChartDataSource,
            };
            var series2 = new RadarSeries()
            {
                Label = "Điểm thực tế",
                Opacity = .6,
                StrokeWidth = 2,
                DrawType = PolarRadarSeriesDrawType.Line,
                LegendIcon = ChartLegendIcon.Rectangle,
                XBindingPath = nameof(ChartDataModel.Name),
                YBindingPath = nameof(ChartDataModel.Value),
                ItemsSource = ChartDataSourceActual,
            };
            chart.Add(series);
            chart.Add(series2);

            ChartSeries = chart;
        }

        private void DrawChartSeries2()
        {
            ChartSeries2.Clear();
            var chart = new ChartSeriesCollection();
            var series = new RadarSeries()
            {
                Label = "Điểm tiêu chuẩn",
                Opacity = .6,
                StrokeWidth = 2,
                DrawType = PolarRadarSeriesDrawType.Line,
                LegendIcon = ChartLegendIcon.Rectangle,
                XBindingPath = nameof(ChartDataModel.Name),
                YBindingPath = nameof(ChartDataModel.Value),
                ItemsSource = ChartDataSource2,
            };
            var series2 = new RadarSeries()
            {
                Label = "Điểm thực tế",
                Opacity = .6,
                StrokeWidth = 2,
                DrawType = PolarRadarSeriesDrawType.Line,
                LegendIcon = ChartLegendIcon.Rectangle,
                XBindingPath = nameof(ChartDataModel.Name),
                YBindingPath = nameof(ChartDataModel.Value),
                ItemsSource = ChartDataSourceActual2,
            };
            chart.Add(series);
            chart.Add(series2);

            ChartSeries2 = chart;
        }

        private KPIRank GetTilteRank(int orderDisPlay)
        {
            var list = new List<KPIRank>()
            {
                new KPIRank()
                {
                    Name="A",
                    Value=5,
                    OrderDisPlay=1
                },
                 new KPIRank()
                {
                    Name="B",
                    Value=4,
                    OrderDisPlay=2
                },
                  new KPIRank()
                {
                    Name="C",
                    Value=3,
                    OrderDisPlay=3
                },
                   new KPIRank()
                {
                    Name="D",
                    Value=2,
                    OrderDisPlay=4
                },
                    new KPIRank()
                {
                    Name="E",
                    Value=1,
                    OrderDisPlay=5
                }
            };

            return list.FirstOrDefault(x => x.OrderDisPlay == orderDisPlay);
        }

        public void UpdateDateTime(PickerDateTimeResponse param)
        {
            if (param != null)
            {
                if (param.PickerType == (short)ComboboxType.First)
                {
                    DateSearch = param.Value;
                    GetDriverKpiChartSearch();
                }
            }
        }

        public void ExecuteToFromDate()
        {
            SafeExecute(async () =>
            {
                DateTime date = DateSearch;
                var parameters = new NavigationParameters
                {
                    { "DataPicker", date },
                    { "PickerType", ComboboxType.First }
                };
                await NavigationService.NavigateAsync("SelectDateTimeCalendar", parameters);
            });
        }

        private void PreviosTime()
        {
            SafeExecute(() =>
            {
                DateSearch = DateSearch.AddDays(-1);
                GetDriverKpiChartSearch();
            });
        }

        private void NextTime()
        {
            SafeExecute(() =>
            {
                if (DateSearch.Date <= DateTime.Today)
                {
                    DateSearch = DateSearch.AddDays(1);
                    GetDriverKpiChartSearch();
                }
            });
        }
    }

    public class KPIRank
    {
        public string Name { get; set; }

        public int Value { get; set; }

        public int OrderDisPlay { get; set; }
    }

    public class ChartDataModel
    {
        public string Name { get; set; }

        public int Value { get; set; }

        public ChartDataModel(string name, int value)
        {
            Name = name;
            Value = value;
        }
    }
}