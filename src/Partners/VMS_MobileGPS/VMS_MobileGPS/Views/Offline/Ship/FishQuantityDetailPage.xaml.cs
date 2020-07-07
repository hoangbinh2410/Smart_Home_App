using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VMS_MobileGPS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FishQuantityDetailPage : ContentPage
    {
        public FishQuantityDetailPage()
        {
            try
            {
                InitializeComponent();
            }
            catch (System.Exception ex)
            {

                throw;
            }
          
        }
    }
}