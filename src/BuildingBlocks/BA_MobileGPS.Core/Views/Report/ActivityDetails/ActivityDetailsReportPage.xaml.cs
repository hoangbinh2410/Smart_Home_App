using BA_MobileGPS.Entities;
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
    public partial class ActivityDetailsReportPage : ContentPage
    {
        private bool IsShowFilter = false;

        public ActivityDetailsReportPage()
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

        public string TotalTime
        {
            get; set;
        } = string.Empty;

        public double SumTotalKm
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
                this.TotalTime = string.Format("{0:hh\\:mm}", ret);
                this.SumTotalKm = Math.Round(enumerableItems.Sum(x => x.TotalKm), 2);
                this.TotalConstantNorms = enumerableItems.FirstOrDefault().ConstantNorms;
            };
        }
    }
}