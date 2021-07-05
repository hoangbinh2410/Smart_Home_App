using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Core.Views.Camera.MonitoringImage;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.Views
{
    public partial class ImageManagingPage : ContentPage
    {
        private bool isAddTemp4 = true;

        public ImageManagingPage()
        {
            InitializeComponent();
            entrySearch.Placeholder = MobileResource.Route_Label_SearchFishing;
            title.Text = MobileResource.Image_Lable_ImageMonitoring;
            ImagePanel.Children.Add(new Template4Image());
        }

        private void SelectView_Tapped(object sender, System.EventArgs e)
        {
            ImagePanel.Children.Clear();
            if (isAddTemp4)
            {
                ImagePanel.Children.Add(new Template1Image());
            }
            else
            {
                ImagePanel.Children.Add(new Template4Image());
            }
            isAddTemp4 = !isAddTemp4;
        }
    }
}