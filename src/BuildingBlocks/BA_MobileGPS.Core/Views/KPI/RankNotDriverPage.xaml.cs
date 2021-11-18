using BA_MobileGPS.Core.Resources;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.Views
{
    public partial class RankNotDriverPage : ContentPage
    {
        public RankNotDriverPage()
        {
            InitializeComponent();
            date_lb.Text = MobileResource.Date_lb;
            month_lb.Text = MobileResource.Month_lb;
            date_lb2.Text = MobileResource.Date_lb;
            month_lb2.Text = MobileResource.Month_lb;
            entrySearchVehicle.Placeholder = MobileResource.SearchVehicle;
            STT_lb.Text = MobileResource.Ordinal_Number_lb;
            driver_lb.Text = MobileResource.Driver_lb;
            point_lb.Text = MobileResource.Point_lb;
            rank_lb.Text = MobileResource.Rank_lb;
        }
    }
}