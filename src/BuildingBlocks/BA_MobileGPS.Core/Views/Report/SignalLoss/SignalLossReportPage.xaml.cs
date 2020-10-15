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
    public partial class SignalLossReportPage : ContentPage
    {
        private bool IsShowFilter = false;

        public SignalLossReportPage()
        {
            InitializeComponent();
            dataGrid.CellRenderers.Remove("TableSummary");
            dataGrid.CellRenderers.Add("TableSummary", new GridTableSignalLossCellRenderer());

            showhideColumn.TranslateTo(0, (int)Application.Current.MainPage.Height, 250);
            IsShowFilter = !IsShowFilter;

            if (Device.RuntimePlatform == Device.Android)
            {
                dataGrid.RowHeight = 55;
            }

            FixColumTablet();
        }

        private void FixColumTablet()
        {
            if (TargetIdiom.Tablet == Device.Idiom)
            {
                dataGrid.GridColumnSizer.DataGrid.Columns["OrderNumber"].Width = 60;
                dataGrid.GridColumnSizer.DataGrid.Columns["StartTime"].Width = 160;
                dataGrid.GridColumnSizer.DataGrid.Columns["EndTime"].Width = 160;
                dataGrid.GridColumnSizer.DataGrid.Columns["TotalTimes"].Width = 200;
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

    public class GridTableSignalLossCellRenderer : GridTableSummaryCellRenderer
    {
        public GridTableSignalLossCellRenderer()
        {
        }

        public override void OnInitializeDisplayView(DataColumnBase dataColumn, SfLabel view)
        {
            base.OnInitializeDisplayView(dataColumn, view);
            if (view == null) return;
            view.FontSize = 10;
        }
    }

    public class CustomSignalLossAggregate : ISummaryAggregate
    {
        public CustomSignalLossAggregate()
        {
        }

        public string TotalTimes
        {
            get; set;
        } = string.Empty;

        public Action<System.Collections.IEnumerable, string, System.ComponentModel.PropertyDescriptor> CalculateAggregateFunc()
        {
            return (items, property, pd) =>
            {
                var enumerableItems = items as IEnumerable<SignalLossResponse>;
                if (enumerableItems.ToList().Count > 0)
                    this.TotalTimes = DateTimeHelper.FormatTimeSpan24h(new TimeSpan(enumerableItems.Sum(x => x.TotalTimes.Ticks)));
            };
        }
    }
}