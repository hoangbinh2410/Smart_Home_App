using BA_MobileGPS.Core.Resources;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.Views
{
    public partial class RankDriverPage : ContentPage
    {
        public RankDriverPage()
        {
            InitializeComponent();
            my_point_tabView.Title = MobileResource.My_Point_TabView_Title;
            point_lb.Text = MobileResource.Point_lb;
            rank_lb.Text = MobileResource.Rank_lb;
            comment_span.Text = MobileResource.Comment_Span;
            date_lb.Text = MobileResource.Date_lb;
            point_lb1.Text = MobileResource.Point_lb;
            ranklb2.Text = MobileResource.Rank_lb2;
            detail_lb.Text = MobileResource.Detail_lb;
            //text_lable_look.Text = MobileResource.Look_lb;
            month_point_medium_lb.Text = MobileResource.Month_Point_Medium;
            rank_table_tabview.Title = MobileResource.Rank_Table_Tabview;
            date_lb2.Text = MobileResource.Date_lb;
            month_lb.Text = MobileResource.Month_lb;
            date_lb3.Text = MobileResource.Date_lb;
            month_lb2.Text = MobileResource.Month_lb;
            entrySearchVehicle.Placeholder = MobileResource.SearchVehicle;
            ordinal_number_lb.Text = MobileResource.Ordinal_Number_lb;
            driver_lb.Text = MobileResource.Driver_lb;
            rank_lb3.Text = MobileResource.Rank_lb;
        }
    }
}