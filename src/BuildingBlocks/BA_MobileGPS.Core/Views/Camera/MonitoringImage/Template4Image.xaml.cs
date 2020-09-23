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

namespace BA_MobileGPS.Core.Views.Camera.MonitoringImage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Template4Image : ContentView
    {

        public Template4Image()
        {
            InitializeComponent();
        }


        private void listGroup_Loaded(object sender, Syncfusion.ListView.XForms.ListViewLoadedEventArgs e)
        {
            var favouriteListView = ((SfListView)sender);

            var visualContainer = favouriteListView.GetVisualContainer();
            var totalextent = (double)visualContainer.GetType().GetRuntimeProperties().FirstOrDefault(container => container.Name == "TotalExtent").GetValue(visualContainer);

            favouriteListView.HeightRequest = totalextent + 10;
        }
    }
}