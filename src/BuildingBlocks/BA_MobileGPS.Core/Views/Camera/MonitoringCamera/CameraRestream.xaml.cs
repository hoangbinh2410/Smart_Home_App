using Xamarin.Forms;

namespace BA_MobileGPS.Core.Views
{
    public partial class CameraRestream : ContentPage
    {
        private View currentSelectLayout { get; set; }
        Color unSelected = (Color)Application.Current.Resources["MainTabUnSelectedTextColor"];
        Color selected = (Color)Application.Current.Resources["MainTabSelectedColor"];
        public CameraRestream()
        {
            InitializeComponent();
            currentSelectLayout = deviceGrid;
        }

        // Đổi màu select
        private void TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            var parentLayout = (View)sender;

            parentLayout.BackgroundColor = selected;
            if (currentSelectLayout != null)
            {
                currentSelectLayout.BackgroundColor = unSelected;
            }
            currentSelectLayout = parentLayout;
        }
    }
}
