using Prism.Events;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Prism.Ioc;
using System;
using Prism.Navigation;
using Syncfusion.ListView.XForms;
using Syncfusion.ListView.XForms.Control.Helpers;
using System.Reflection;
using System.Linq;
using BA_MobileGPS.Core.ViewModels;

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
            

            // Initialize the View Model Object
        }

    }
}