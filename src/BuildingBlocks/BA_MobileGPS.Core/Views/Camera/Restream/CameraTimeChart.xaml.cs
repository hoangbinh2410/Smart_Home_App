using Xamarin.Forms;

namespace BA_MobileGPS.Core.Views
{
    public partial class CameraTimeChart : ContentPage
    {
        public CameraTimeChart()
        {
            InitializeComponent();
          
        }

        private void listview_SwipeStarted(object sender, Syncfusion.ListView.XForms.SwipeStartedEventArgs e)
        {
            listview.SelectedItem = e.ItemData;
        }
    }
}
