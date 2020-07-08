using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VMS_MobileGPS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SOSPage : ContentPage
    {
        public SOSPage()
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