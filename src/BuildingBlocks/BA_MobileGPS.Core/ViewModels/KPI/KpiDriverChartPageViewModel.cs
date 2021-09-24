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
            GetDriverKpiChart();
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

        private int selectedTabIndex = 0;
        public int SelectedTabIndex { get => selectedTabIndex; set => SetProperty(ref selectedTabIndex, value); }

        private DateTime dateTab1 = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1, 0, 0, 0);
        public virtual DateTime DateTab1 { get => dateTab1; set => SetProperty(ref dateTab1, value); }

        private DateTime dateTab2 = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1, 0, 0, 0);
        public virtual DateTime DateTab2 { get => dateTab2; set => SetProperty(ref dateTab2, value); }

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

        private void GetDriverKpiChart()
        {
            RunOnBackground(async () =>
            {
                return await _KPIDriverService.GetDriverKpiChart(new Entities.DriverKpiChartRequest()
                {
                    DriverID = 270487,
                    FromDate = "06/08/2020",
                    ToDate = "06/08/2020"
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
                    if (SelectedTabIndex == 0)
                    {
                        DateTab1 = param.Value;
                    }
                    else
                    {
                        DateTab2 = param.Value;
                    }
                }
            }
        }

        public void ExecuteToFromDate()
        {
            SafeExecute(async () =>
            {
                DateTime date = DateTab1;
                if (SelectedTabIndex == 0)
                {
                    date = DateTab1;
                }
                else
                {
                    date = DateTab2;
                }
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
                if (SelectedTabIndex == 0)
                {
                    DateTab1 = DateTab1.AddDays(-1);
                }
                else
                {
                    DateTab2 = DateTab2.AddDays(-1);
                }
            });
        }

        private void NextTime()
        {
            SafeExecute(() =>
            {
                if (SelectedTabIndex == 0)
                {
                    DateTab1 = DateTab1.AddDays(1);
                }
                else
                {
                    DateTab2 = DateTab2.AddDays(1);
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