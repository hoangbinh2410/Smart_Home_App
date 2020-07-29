using BA_MobileGPS.Core.Models;
using BA_MobileGPS.Core.Resource;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms;

namespace VMS_MobileGPS.Views
{
    public partial class DetailVehiclePopup : PopupPage
    {
        private Action<string> callback;
        public DetailVehiclePopup(string vehicleCode,Action<string> callback)
        {
           
            InitializeComponent();
            this.callback = callback;
            lblName.Text = "TÀU" + " " + vehicleCode;
        }

        private void Mornitoring_Tapped(object sender, EventArgs e)
        {
            Close();
            callback?.Invoke(MobileResource.Online_Label_TitlePage);
        }

        private void Route_Tapped(object sender, EventArgs e)
        {
            Close();
            callback?.Invoke(MobileResource.Route_Label_TitleVMS);
        }

        private void Distance_Tapped(object sender, EventArgs e)
        {
            Close();
            callback?.Invoke(MobileResource.Route_Label_DistanceTitle);
        }

        private void Information_Tapped(object sender, EventArgs e)
        {
            Close();
            callback?.Invoke(MobileResource.DetailVehicle_Label_TilePage);
        }
        private void Close_Tapped(object sender, EventArgs e)
        {
            Close();
        }

        private void Close()
        {
            PopupNavigation.Instance.PopAsync();
        }

      
    }
}
