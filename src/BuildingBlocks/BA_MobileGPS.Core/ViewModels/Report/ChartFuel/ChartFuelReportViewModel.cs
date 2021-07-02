using BA_MobileGPS.Core.Extensions;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.RequestEntity;
using BA_MobileGPS.Service;

using FFImageLoading.Forms;

using Prism;
using Prism.Navigation;

using Syncfusion.SfChart.XForms;
using Syncfusion.XForms.Buttons;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;

using SelectionChangedEventArgs = Syncfusion.XForms.ComboBox.SelectionChangedEventArgs;
using SelectionChangingEventArgs = Syncfusion.XForms.ComboBox.SelectionChangingEventArgs;

namespace BA_MobileGPS.Core.ViewModels
{
    public class ChartFuelReportViewModel : ReportBaseViewModel<IFuelChartService, FuelChartRequest, List<FuelChartReport>>
    {
        public bool ShowByTime { get; set; }

        public override int MaxRangeDate => 1;

        protected override bool ShowLoading => true;

        private string selectedTime;
        public string SelectedTime { get => selectedTime; set => SetProperty(ref selectedTime, value); }

        public DateTime? MinimumDate => ShowByTime ? (DateTime?)FromDate : null;

        public DateTime? MaximumDate => ShowByTime ? (DateTime?)ToDate : null;

        private ChartSeriesCollection chartSeries = new ChartSeriesCollection();
        public ChartSeriesCollection ChartSeries { get => chartSeries; set => SetProperty(ref chartSeries, value); }

        private List<FuelChartDisplay> listFuelSumary;
        public List<FuelChartDisplay> ListFuelSumary { get => listFuelSumary; set => SetProperty(ref listFuelSumary, value); }

        private FuelChartDisplay currentChartDisplay;
        public FuelChartDisplay CurrentChartDisplay { get => currentChartDisplay; set => SetProperty(ref currentChartDisplay, value); }

        private bool isShowInfo;

        public bool IsShowInfo { get => isShowInfo; set => SetProperty(ref isShowInfo, value); }

        public ICommand DateSelectedCommand { get; }
        public ICommand TimeSelectingCommand { get; }
        public ICommand TimeSelectedCommand { get; }
        public ICommand StateChangedCommand { get; }
        public ICommand TrackballCreatedCommand { get; }

        public ChartFuelReportViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = MobileResource.PourFuelReport_Label_TileChartDetailPage;

            ToDate = DateTime.Now;

            DateSelectedCommand = new Command(DateSelected);
            TimeSelectingCommand = new Command<SelectionChangingEventArgs>(TimeSelecting);
            TimeSelectedCommand = new Command<SelectionChangedEventArgs>(TimeSelected);
            StateChangedCommand = new Command<StateChangedEventArgs>(StateChanged);
            TrackballCreatedCommand = new Command<ChartTrackballCreatedEventArgs>(TrackballCreated);
            IsShowInfo = false;
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            EventAggregator.GetEvent<UserBehaviorEvent>().Publish(new UserBehaviorModel()
            {
                Page = Entities.Enums.MenuKeyEnums.MobileFuelChart,
                Type = UserBehaviorType.Start
            });
        }

        public override void OnDestroy()
        {
            base.Dispose();
            EventAggregator.GetEvent<UserBehaviorEvent>().Publish(new UserBehaviorModel()
            {
                Page = Entities.Enums.MenuKeyEnums.MobileFuelChart,
                Type = UserBehaviorType.End
            });
        }

        private void DateSelected()
        {
            RaisePropertyChanged(nameof(MinimumDate));
            RaisePropertyChanged(nameof(MaximumDate));

            if (Vehicle != null)
            {
                GetDataChart();
                IsShowInfo = false;
            }
        }

        private void TimeSelecting(SelectionChangingEventArgs args)
        {
            SafeExecute(() =>
            {
                if (args.Value is string value && int.TryParse(value.Remove(value.Length - 1, 1), out int hours))
                {
                    FromDate = ToDate.Subtract(TimeSpan.FromHours(hours).Subtract(TimeSpan.FromSeconds(1)));

                    if (Vehicle != null)
                    {
                        GetDataChart();
                        IsShowInfo = false;
                    }
                }
            });
        }

        private void TimeSelected(SelectionChangedEventArgs args)
        {
            RaisePropertyChanged(nameof(MinimumDate));
            RaisePropertyChanged(nameof(MaximumDate));

            SelectedTime = null;
        }

        public override void OnVehicleSelected()
        {
            if (Vehicle != null)
            {
                GetDataChart();
                IsShowInfo = false;
            }
        }

        private void StateChanged(StateChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(MinimumDate));
            RaisePropertyChanged(nameof(MaximumDate));

            if (ListDataReport != null)
            {
                OnGetDataSuccess();
                IsShowInfo = false;
            }
        }

        private CancellationTokenSource cts;

        private void TrackballCreated(ChartTrackballCreatedEventArgs e)
        {
            TryExecute(() =>
            {
                if (e.ChartPointsInfo != null && e.ChartPointsInfo.Count > 0)
                {
                    if (cts != null)
                        cts.Cancel();

                    cts = new CancellationTokenSource();

                    var info = e.ChartPointsInfo[0];

                    RunOnBackground(async () =>
                    {
                        await Task.Delay(100);

                        if (cts.IsCancellationRequested)
                            return default;

                        var sumary = e.ChartPointsInfo.Select(c => c.DataPoint).Cast<FuelChartDisplay>().OrderBy(c => c.Id).ToList();

                        sumary.Add(new FuelChartDisplay()
                        {
                            Title = MobileResource.ChartFuelReport_Total,
                            NumberOfLiters = Math.Round(sumary.Sum(c => c.NumberOfLiters), 2)
                        });

                        return sumary;
                    },
                    (result) =>
                    {
                        CurrentChartDisplay = result[0];
                        ListFuelSumary = result;
                        if (CurrentChartDisplay.NumberOfLiters == 0 && CurrentChartDisplay.VelocityGPS == 0)
                        {
                            IsShowInfo = false;
                        }
                        else
                        {
                            IsShowInfo = true;
                        }
                    },
                    cts: cts);
                }
                else
                {
                    CurrentChartDisplay = default;
                    ListFuelSumary = default;
                }
            });
        }

        public override bool ValidateInput()
        {
            var result = base.ValidateInput();

            if (Vehicle == null)
            {
                result = false;
                DisplayMessage.ShowMessageInfo(MobileResource.Common_Message_PleaseSelectVehicle, 3000);
            }

            return result;
        }

        public override FuelChartRequest SetInputData()
        {
            return new FuelChartRequest
            {
                UserID = UserInfo.UserId,
                CompanyID = CurrentComanyID,
                VehicleID = Vehicle.VehicleId,
                VehiclePlate = Vehicle.VehiclePlate,
                FromDate = FromDate,
                ToDate = ToDate
            };
        }

        private async void GetDataChart()
        {
            var isvalid = await ValidateDateTimeReport();
            if (isvalid)
            {
                GetData();
            }
        }

        private DataTemplate trackballLabelTemplate;

        public DataTemplate TrackballLabelTemplate
        {
            get
            {
                if (trackballLabelTemplate != null)
                    return trackballLabelTemplate;

                trackballLabelTemplate = new DataTemplate(() =>
                {
                    var stk = new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Padding = 2,
                        Spacing = 2,
                        Children =
                            {
                                new CachedImage
                                {
                                    HeightRequest = 15,
                                    WidthRequest = 15,
                                    Source = "ic_nl.png",
                                    VerticalOptions = LayoutOptions.Center
                                },
                                new Label
                                {
                                    FontSize = 12,
                                    TextColor = Color.White,
                                    VerticalOptions = LayoutOptions.Center
                                }
                            }
                    };

                    stk.Children[1].SetBinding(Label.TextProperty, "NumberOfLiters");

                    return stk;
                });

                return trackballLabelTemplate;
            }
        }

        public override void OnGetDataSuccess()
        {
            SafeExecute(() =>
            {
                if (!HasData)
                {
                    DisplayMessage.ShowMessageInfo(MobileResource.Common_Message_NoData, 3000);
                    return;
                }

                ChartSeries.Clear();
                ListFuelSumary = default;
                CurrentChartDisplay = default;

                if (ListDataReport != null && ListDataReport.Count > 0)
                {
                    var result = new ChartSeriesCollection();

                    for (int i = 0; i < ListDataReport.Max(d => d.NumberOfLiters.Count); i++)
                    {
                        var data = ListDataReport
                        .Select(c => new FuelChartDisplay()
                        {
                            Id = i + 1,
                            Title = MobileResource.ChartFuelReport_Tank + " " + (i + 1),
                            Time = c.Time,
                            NumberOfLiters = c.NumberOfLiters[i] <= 0 ? 0 : Math.Round(c.NumberOfLiters[i], 2),
                            VelocityGPS = c.VelocityGPS,
                            Km = c.Km,
                            MachineStatus = StateVehicleExtension.EngineStateConverter(c.MachineStatus),
                            CurrentAddress = c.CurrentAddress
                        })
                        .Where(c => (ShowByTime && c.NumberOfLiters >= 0) || c.NumberOfLiters > 0).ToList();

                        result.Add(new SplineAreaSeries()
                        {
                            Label = MobileResource.ChartFuelReport_Tank + " " + (i + 1),
                            Color = ColorChart[i],
                            Opacity = .6,
                            XBindingPath = nameof(FuelChartDisplay.Time),
                            YBindingPath = nameof(FuelChartDisplay.NumberOfLiters),
                            ItemsSource = data,
                            SplineType = SplineType.Cardinal,
                            TrackballLabelTemplate = TrackballLabelTemplate
                        });
                    }

                    ChartSeries = result;
                }
            });
        }

        private readonly Color[] ColorChart = new Color[]
        {
           (Color)PrismApplicationBase.Current.Resources["DangerousColor"],
            (Color)PrismApplicationBase.Current.Resources["PrimaryColor"]
        };

        public override void OnGetDataFail()
        {
            ChartSeries.Clear();
            ListFuelSumary = default;
            CurrentChartDisplay = default;
        }
    }
}