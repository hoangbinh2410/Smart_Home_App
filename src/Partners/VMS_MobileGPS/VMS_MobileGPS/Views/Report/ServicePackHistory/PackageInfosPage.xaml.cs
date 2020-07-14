using Syncfusion.SfDataGrid.XForms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VMS_MobileGPS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PackageInfosPage : ContentPage
    {
        public PackageInfosPage()
        {
            InitializeComponent();
        }

        private void SfDataGrid_ItemsSourceChanged(object sender, GridItemsSourceChangedEventArgs e)
        {
            if (sender is SfDataGrid dataGrid)
            {
                if (dataGrid.View != null)
                {
                    var height = (dataGrid.View.Records.Count * dataGrid.RowHeight) + dataGrid.HeaderRowHeight;
                    ((VisualElement)dataGrid.Parent).HeightRequest = height;
                }

            }
        }
    }
}