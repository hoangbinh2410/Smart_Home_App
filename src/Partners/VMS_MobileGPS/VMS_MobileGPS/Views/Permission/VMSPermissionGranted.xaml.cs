using Rg.Plugins.Popup.Pages;
using VMS_MobileGPS.ViewModels;

namespace VMS_MobileGPS.Views.Popup
{
    public partial class VMSPermissionGranted : PopupPage
    {
        public VMSPermissionGranted()
        {
            this.BindingContext = new VMSPermissionGrantedViewModel();
            InitializeComponent();
        }
    }
}