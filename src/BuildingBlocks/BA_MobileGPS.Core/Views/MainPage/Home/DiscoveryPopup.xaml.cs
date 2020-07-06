using BA_MobileGPS.Entities;
using Rg.Plugins.Popup.Pages;
using Syncfusion.ListView.XForms;
using Syncfusion.ListView.XForms.Control.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BA_MobileGPS.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DiscoveryPopup : PopupPage
    {
        public DiscoveryPopup(List<HomeMenuItem> items)
        {          
            InitializeComponent();
            SetListViewSource(items);
        }

        private void SetListViewSource(List<HomeMenuItem> items)
        {
            listView.ItemsSource = items;
            var rows = items.Count / 4;         
            listView.LayoutManager = new GridLayout() { SpanCount = rows };         
        }

        private void listView_Loaded(object sender, Syncfusion.ListView.XForms.ListViewLoadedEventArgs e)
        {
            var visualContainer = listView.GetVisualContainer();
            var totalextent = (double)visualContainer.GetType().GetRuntimeProperties().FirstOrDefault(container => container.Name == "TotalExtent").GetValue(visualContainer);

            listView.WidthRequest = totalextent + 10;
        }
    }
}