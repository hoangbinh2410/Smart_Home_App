using Rg.Plugins.Popup.Pages;
using Syncfusion.ListView.XForms.Control.Helpers;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.Views
{
    public partial class LoginPreviewFeaturesPage : PopupPage
    {
        public LoginPreviewFeaturesPage()
        {
            InitializeComponent();
        }
        private void SfListView_Loaded(object sender, Syncfusion.ListView.XForms.ListViewLoadedEventArgs e)
        {
            var visualContainer = listView.GetVisualContainer();
            var totalextent = (double)visualContainer.GetType().GetRuntimeProperties().FirstOrDefault(container => container.Name == "TotalExtent").GetValue(visualContainer);

            listView.HeightRequest = totalextent + 10;
        }
    }
}
