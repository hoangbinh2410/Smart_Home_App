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
    public partial class MachineVehicleReport : ContentPage
    {
        private bool IsShowFilter = false;

        public MachineVehicleReport()
        {
            InitializeComponent();

            dataGrid.CellRenderers.Remove("TableSummary");
            dataGrid.CellRenderers.Add("TableSummary", new GridTableMachineVehicleCellRenderer());

            showhideColumn.TranslateTo(0, (int)Application.Current.MainPage.Height, 250);
            IsShowFilter = !IsShowFilter;
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

    public class GridTableMachineVehicleCellRenderer : GridTableSummaryCellRenderer
    {
        public GridTableMachineVehicleCellRenderer()
        {
        }

        public override void OnInitializeDisplayView(DataColumnBase dataColumn, SfLabel view)
        {
            base.OnInitializeDisplayView(dataColumn, view);
            if (view == null) return;
            view.FontSize = 10;
        }
    }

    public class CustomMachineVehicleAggregate : ISummaryAggregate
    {
        public CustomMachineVehicleAggregate()
        {
        }

        public string TotalNumberMinutesTurn
        {
            get; set;
        } = string.Empty;

        public Action<System.Collections.IEnumerable, string, System.ComponentModel.PropertyDescriptor> CalculateAggregateFunc()
        {
            return (items, property, pd) =>
            {
                var enumerableItems = items as IEnumerable<MachineVehicleResponse>;

                this.TotalNumberMinutesTurn = DateTimeHelper.FormatTimeSpan24h(new TimeSpan(enumerableItems.Sum(x => TimeSpan.Parse(x.NumberMinutesTurn).Ticks)));
            };
        }
    }
}