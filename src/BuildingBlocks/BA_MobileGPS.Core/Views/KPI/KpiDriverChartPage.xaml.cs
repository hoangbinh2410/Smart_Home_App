using BA_MobileGPS.Core.Resources;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.Views
{
    public partial class KpiDriverChartPage : ContentPage
    {
        public KpiDriverChartPage()
        {
            InitializeComponent();
            if (Device.RuntimePlatform == Device.iOS)
            {
                Chart.PrimaryAxis.LabelStyle.FontSize = 5;
                chart2.PrimaryAxis.LabelStyle.FontSize = 5;
            }        
            text_assess.Text = MobileResource.Text_Assess;
            title_tabview.Title = MobileResource.Title_Tabview;
            before_day_lb.Text = MobileResource.Before_Day_lb;
            next_day_lb.Text = MobileResource.Next_Day_lb;
            sum_lb.Text = MobileResource.Sum_Point_lb;
            rank_lb.Text = MobileResource.Rank_lb;
            table_lb.Text = MobileResource.Table_lb;
            criteria_lb.Text = MobileResource.Criteria_lb;
            point_lb.Text = MobileResource.Point_lb;
            rank_lb2.Text = MobileResource.Rank_lb2;
            safe_criteria_lb.Text = MobileResource.Safe_Criteria_lb;
            economical_tabview.Title = MobileResource.Economical_Tabview;
            before_day_lb2.Text = MobileResource.Before_Day_lb;
            next_day_lb_2.Text = MobileResource.Next_Day_lb;
            point_sum_lb.Text = MobileResource.Sum_Point_lb;
            rank_lb3.Text = MobileResource.Rank_lb2;
            table_criteria_lb.Text = MobileResource.Table_Criteria_lb;
            rank_lb4.Text = MobileResource.Rank_lb2;
            point_lb2.Text = MobileResource.Point_lb;
            criteria_lb2.Text = MobileResource.Criteria_lb;
            economical_criteria_lb.Text = MobileResource.Economical_Criteria_lb;
        }

        private void NumericalAxis_LabelCreated(object sender, Syncfusion.SfChart.XForms.ChartAxisLabelEventArgs e)
        {
            int data = int.Parse(e.LabelContent);
            if (data == 5)
            {
                e.LabelContent = "A";
            }
            if (data == 4)
            {
                e.LabelContent = "B";
            }
            if (data == 3)
            {
                e.LabelContent = "C";
            }
            if (data == 2)
            {
                e.LabelContent = "D";
            }
            if (data == 1)
            {
                e.LabelContent = "E";
            }
        }

        private void primary_LabelCreated(object sender, Syncfusion.SfChart.XForms.ChartAxisLabelEventArgs e)
        {
            if (e.LabelStyle != null)
            {
                if (Device.RuntimePlatform == Device.iOS)
                {
                    e.LabelStyle.FontSize = 5;
                }
                else
                {
                    e.LabelStyle.FontSize = 10;
                }
            }
        }
    }
}