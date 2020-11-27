using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using Syncfusion.Data;
using Syncfusion.SfDataGrid.XForms;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BA_MobileGPS.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActivitySummariesReportPage : ContentPage
    {
        private bool IsShowFilter = false;

        public ActivitySummariesReportPage()
        {
            InitializeComponent();

            dataGrid.CellRenderers.Remove("TableSummary");
            dataGrid.CellRenderers.Add("TableSummary", new GridTableActivitySummariesCellRenderer());

            showhideColumn.TranslateTo(0, (int)Application.Current.MainPage.Height, 250);
            IsShowFilter = !IsShowFilter;

            if (Device.RuntimePlatform == Device.Android)
            {
                dataGrid.RowHeight = 55;
            }
        }

        private void HideableToolbarItem_Clicked(object sender, EventArgs e)
        {
            if (IsShowFilter)
            {
                showhideColumn.TranslateTo(0, 0, 250);
            }
            else
            {
                showhideColumn.TranslateTo(0, showhideColumn.Height, 250);
            }
            IsShowFilter = !IsShowFilter;
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            HideableToolbarItem_Clicked(sender, e);
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            HideableToolbarItem_Clicked(sender, e);
        }
    }

    public class GridTableActivitySummariesCellRenderer : GridTableSummaryCellRenderer
    {
        public GridTableActivitySummariesCellRenderer()
        {
        }

        public override void OnInitializeDisplayView(DataColumnBase dataColumn, SfLabel view)
        {
            base.OnInitializeDisplayView(dataColumn, view);
            if (view == null) return;
            view.FontSize = 10;
        }
    }

    public class CustomActivitySummariesAggregate : ISummaryAggregate
    {
        public CustomActivitySummariesAggregate()
        {
        }

        public string TotalTime
        {
            get; set;
        } = string.Empty;

        public double SumTotalKmGps
        {
            get; set;
        } = 0;

        public int TotalVmax
        {
            get; set;
        } = 0;

        public int TotalVarg
        {
            get; set;
        } = 0;

        public Action<System.Collections.IEnumerable, string, System.ComponentModel.PropertyDescriptor> CalculateAggregateFunc()
        {
            return (items, property, pd) =>
            {
                var enumerableItems = items as IEnumerable<ActivitySummariesModel>;
                if (enumerableItems.ToList().Count == 1)
                {
                    var item = enumerableItems.FirstOrDefault();
                    this.TotalTime = item.JoinActivityTimes;
                    this.SumTotalKmGps = Math.Round(item.TotalKmGps, 2);
                    this.TotalVmax = item.Vmax;
                    this.TotalVarg = (int)Math.Round((double)item.Varg);
                }
                else if (enumerableItems.ToList().Count > 1)
                {
                    this.TotalTime = DateTimeHelper.FormatTimeSpan24h(new TimeSpan(enumerableItems.Sum(x => x.ActivityTimes.Ticks)));
                    this.SumTotalKmGps = Math.Round(enumerableItems.Sum(x => x.TotalKmGps), 2);
                    this.TotalVmax = enumerableItems.Select(x => x.Vmax).Max();
                    this.TotalVarg = (int)Math.Round(enumerableItems.Where(item => item.TotalKmGps > 0).Average(x => x.Varg));
                }
            };
        }
    }
}