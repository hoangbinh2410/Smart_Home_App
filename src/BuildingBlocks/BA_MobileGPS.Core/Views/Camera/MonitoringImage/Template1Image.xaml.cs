using Prism.Events;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;

namespace BA_MobileGPS.Core.Views.Camera.MonitoringImage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Template1Image : ContentView
    {
        public Template1Image()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }
}