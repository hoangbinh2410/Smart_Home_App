using BA_MobileGPS.Entities;
using Syncfusion.Data;
using Syncfusion.SfDataGrid.XForms;
using System;
using System.Collections.Generic;
using System.Linq;
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

    public class CustomActivityDetailsAggregate : ISummaryAggregate
    {
        public CustomActivityDetailsAggregate()
        {
        }

        public double TotalTime
        {
            get; set;
        } = 0;

        public double SumTotalKm
        {
            get; set;
        } = 0;
        
        public double SumTotalKmMechanical
        {
            get; set;
        } = 0;

        public double TotalNorms
        {
            get; set;
        } = 0;

        public double TotalConstantNorms
        {
            get; set;
        } = 0;

        public Action<System.Collections.IEnumerable, string, System.ComponentModel.PropertyDescriptor> CalculateAggregateFunc()
        {
            return (items, property, pd) =>
            {
                var enumerableItems = items as IEnumerable<ActivityDetailsModel>;
                TimeSpan ret = TimeSpan.Zero;
                foreach (var p in enumerableItems)
                    if (p.TotalTimes != null)
                        ret = ret + p.TotalTimes;
                //this.TotalTime = string.Format("{0:hh\\:mm}", ret);
                //this.SumTotalKm = Math.Round(enumerableItems.Sum(x => x.TotalKm), 2);
                //this.TotalConstantNorms = enumerableItems.FirstOrDefault().ConstantNorms;
            };
        }
    }
}
