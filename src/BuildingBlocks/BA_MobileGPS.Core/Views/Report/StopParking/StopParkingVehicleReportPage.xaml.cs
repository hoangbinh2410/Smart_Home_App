using BA_MobileGPS.Entities;
using Syncfusion.Data;
using Syncfusion.SfDataGrid.XForms;
using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BA_MobileGPS.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StopParkingVehicleReportPage : ContentPage
    {
        private bool IsShowFilter = false;

        public StopParkingVehicleReportPage()
        {
            InitializeComponent();

            dataGrid.CellRenderers.Remove("TableSummary");
            dataGrid.CellRenderers.Add("TableSummary", new GridTableStopParkingVehicleCellRenderer());

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

    public class GridTableStopParkingVehicleCellRenderer : GridTableSummaryCellRenderer
    {
        public GridTableStopParkingVehicleCellRenderer()
        {
        }

        public override void OnInitializeDisplayView(DataColumnBase dataColumn, SfLabel view)
        {
            base.OnInitializeDisplayView(dataColumn, view);
            if (view == null) return;
            view.FontSize = 10;
        }
    }

    public class CustomStopParkingVehicleAggregate : ISummaryAggregate
    {
        public CustomStopParkingVehicleAggregate()
        {
        }

        public string TotalStopParkingTime
        {
            get; set;
        } = string.Empty;

        public Action<System.Collections.IEnumerable, string, System.ComponentModel.PropertyDescriptor> CalculateAggregateFunc()
        {
            return (items, property, pd) =>
            {
                var enumerableItems = items as IEnumerable<StopParkingVehicleModel>;
                TimeSpan ret = TimeSpan.Zero;
                foreach (var p in enumerableItems)
                    if (p.StopParkingTime != null)
                        ret = ret + TimeSpan.Parse(p.StopParkingTime);
                this.TotalStopParkingTime = MinusTimeSpanHelper.MinusTimeSpan(ret);
            };
        }
    }
}