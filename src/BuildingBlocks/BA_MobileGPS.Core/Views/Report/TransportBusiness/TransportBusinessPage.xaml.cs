using BA_MobileGPS.Entities.ResponeEntity.Report.TransportBusiness;
using Syncfusion.Data;
using Syncfusion.SfDataGrid.XForms;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.Views.Report.TransportBusiness
{
    public partial class TransportBusinessPage : ContentPage
    {
        private bool IsShowFilter = false;
        public TransportBusinessPage()
        {
            InitializeComponent();
            dataGrid.CellRenderers.Remove("TableSummary");
            dataGrid.CellRenderers.Add("TableSummary", new GridTableActivityDetailsCellRenderer());

            showhideColumn.TranslateTo(0, (int)Application.Current.MainPage.Height, 250);
            IsShowFilter = !IsShowFilter;

            if (Device.RuntimePlatform == Device.Android)
            {
                dataGrid.RowHeight = 55;
            }

        }

        private void HideableToolbarItem_Clicked(object sender, System.EventArgs e)
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

        private void TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            HideableToolbarItem_Clicked(sender, e);
        }

        private void Button_Clicked(object sender, System.EventArgs e)
        {
            HideableToolbarItem_Clicked(sender, e);
        }
    }

    public class GridTableActivityDetailsCellRenderer : GridTableSummaryCellRenderer
    {
        public GridTableActivityDetailsCellRenderer()
        {
        }

        public override void OnInitializeDisplayView(DataColumnBase dataColumn, SfLabel view)
        {
            base.OnInitializeDisplayView(dataColumn, view);
            if (view == null) return;
            view.FontSize = 10;
        }
    }

    public class CustomTransportBusinessAggregate : ISummaryAggregate
    {
        public CustomTransportBusinessAggregate()
        {
        }

        public string TotalTimes {get; set;}
        public string SumTotalKm {get; set;} 
        public string TotalKmMechanical {get; set;}
        public string TotalUseFuel {get; set;}
        public string TotalNorms {get; set;}

        public Action<System.Collections.IEnumerable, string, System.ComponentModel.PropertyDescriptor> CalculateAggregateFunc()
        {
            return (items, property, pd) =>
            {
                double totalTimes = 0;
                double sumTotalKm = 0;
                double totalKmMechanical = 0;
                double totalUseFuel = 0;
                double totalNorms = 0;
                var enumerableItems = items as IEnumerable<TransportBusinessResponse>;
                if(enumerableItems != null)
                {
                    foreach (var item in enumerableItems)
                    {
                        totalTimes = totalTimes + item.TotalTime;
                        sumTotalKm = sumTotalKm + item.TotalKmGps;
                        totalKmMechanical = totalKmMechanical + item.KmOfPulseMechanical;
                        totalUseFuel = totalUseFuel + item.UseFuel;
                        totalNorms = totalNorms + item.Norms;
                    }
                    TotalTimes = String.Format("{0:n}", Math.Round(totalTimes, 2));
                    SumTotalKm = String.Format("{0:n}", Math.Round(sumTotalKm, 2));
                    TotalKmMechanical = String.Format("{0:n}", Math.Round(totalKmMechanical, 2));
                    TotalUseFuel = String.Format("{0:n}", Math.Round(totalUseFuel, 2));
                    TotalNorms = String.Format("{0:n}", Math.Round(totalNorms, 2));
                }    
                
            };
        }
    }
}
