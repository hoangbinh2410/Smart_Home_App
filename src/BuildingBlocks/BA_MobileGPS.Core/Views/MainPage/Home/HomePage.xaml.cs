using BA_MobileGPS.Entities;
using Syncfusion.ListView.XForms.Control.Helpers;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.Views
{
    public partial class HomePage : ScrollView
    {
        public HomePage()
        {
            InitializeComponent();

         
        }

        private void SfListView_Loaded(object sender, Syncfusion.ListView.XForms.ListViewLoadedEventArgs e)
        {
            ListViewDynamicSizeConfig();
        }

        private void ListViewDynamicSizeConfig()
        {
            var visualContainer = favouriteListView.GetVisualContainer();
            var totalextent = (double)visualContainer.GetType().GetRuntimeProperties().FirstOrDefault(container => container.Name == "TotalExtent").GetValue(visualContainer);

            favouriteListView.WidthRequest = totalextent + 10;
        }

  
    }
}
