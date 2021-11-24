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

        public double TotalTimes
        {
            get; set;
        } = 0;

        public double SumTotalKm
        {
            get; set;
        } = 0;
        
        public double TotalKmMechanical
        {
            get; set;
        } = 0;

        public double TotalUseFuel
        {
            get; set;
        } = 0;

        public double TotalNorms
        {
            get; set;
        } = 0;

        public Action<System.Collections.IEnumerable, string, System.ComponentModel.PropertyDescriptor> CalculateAggregateFunc()
        {
            return (items, property, pd) =>
            {
                TotalTimes = 0;
                SumTotalKm = 0;
                TotalKmMechanical = 0;
                TotalUseFuel = 0;
                TotalNorms = 0;
                var enumerableItems = items as IEnumerable<TransportBusinessResponse>;
                if(enumerableItems != null)
                {
                    foreach (var item in enumerableItems)
                    {
                        TotalTimes = TotalTimes + item.TotalTime;
                        SumTotalKm = SumTotalKm + item.TotalKmGps;
                        TotalKmMechanical = TotalKmMechanical + item.KmOfPulseMechanical;
                        TotalUseFuel = TotalUseFuel + item.UseFuel;
                        TotalNorms = TotalNorms + item.Norms;
                    }
                    TotalTimes = Math.Round(TotalTimes, 2);
                    SumTotalKm = Math.Round(SumTotalKm, 2);
                    TotalKmMechanical = Math.Round(TotalKmMechanical, 2);
                    TotalUseFuel = Math.Round(TotalUseFuel, 2);
                    TotalNorms = Math.Round(TotalNorms, 2);
                }    
                
            };
        }
    }
}
