using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VMS_MobileGPS.Views
{
    public partial class OnlinePage : ContentPage
    {
        public OnlinePage()
        {
            InitializeComponent();
            googleMap.UiSettings.ZoomControlsEnabled = false;
        }
    }
}