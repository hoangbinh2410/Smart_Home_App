using BA_MobileGPS.Core.Resources;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.Views
{
    public partial class CameraTimeChart : ContentPage
    {
        public CameraTimeChart()
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, string.Empty);
            title.Text = MobileResource.Camera_Lable_Chart;
            timehasvideo.Text = MobileResource.Camera_Lable_TimeHasVideo;
            timesavevideo.Text = MobileResource.Camera_Lable_TimeSaveVideo;
            txtnote.Text = MobileResource.Camera_Lable_NoteChart;
        }
    }
}
