using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using Prism.Navigation;
using Syncfusion.SfChart.XForms;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BA_MobileGPS.Core.ViewModels
{
    public class KpiDriverChartPageViewModel : ViewModelBase
    {
        private readonly IKPIDriverService _KPIDriverService;

        public KpiDriverChartPageViewModel(INavigationService navigationService, IKPIDriverService KPIDriverService) : base(navigationService)
        {
            Title = "Điểm xếp hạng lái xe";
            _KPIDriverService = KPIDriverService;
        }

        #region Lifecycle

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            GetDriverKpiChart();
        }

        public override void OnDestroy()
        {
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

        private ChartSeriesCollection chartSeries = new ChartSeriesCollection();
        public ChartSeriesCollection ChartSeries { get => chartSeries; set => SetProperty(ref chartSeries, value); }

        public ObservableCollection<ChartDataModel> ChartDataSource { get; set; }

        public ObservableCollection<ChartDataModel> ChartDataSourceActual { get; set; }

        private EvaluationsByType evaluationType1=new EvaluationsByType();
        public EvaluationsByType EvaluationType1 { get => evaluationType1; set => SetProperty(ref evaluationType1, value); }

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