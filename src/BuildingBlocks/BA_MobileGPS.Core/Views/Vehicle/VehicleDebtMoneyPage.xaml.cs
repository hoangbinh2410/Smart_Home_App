using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;

using Syncfusion.SfDataGrid.XForms;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BA_MobileGPS.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VehicleDebtMoneyPage : ContentPage
    {
        private readonly IHelperAdvanceService _helperAdvanceService;

        public VehicleDebtMoneyPage(IHelperAdvanceService helperAdvanceService)
        {
            InitializeComponent();

            dataGrid.CellRenderers.Remove("CaptionSummary");
            dataGrid.CellRenderers.Add("CaptionSummary", new GridCaptionSummaryCellRendererExt());
            this._helperAdvanceService = helperAdvanceService;

            dataGrid.QueryCellStyle += DataGrid_QueryCellStyle1;
            datagridvehiclefree.QueryCellStyle += Datagridvehiclefree_QueryCellStyle;
        }

        private void Datagridvehiclefree_QueryCellStyle(object sender, QueryCellStyleEventArgs e)
        {
            if (e.Column.MappingName == "VehiclePlate" && e.RowData is VehicleFreeResponse model)
            {
                switch (model.MessageIdBAP)
                {
                    case 128:
                        e.Style.ForegroundColor = Color.White;
                        e.Style.BackgroundColor = Color.BlueViolet;
                        break;

                    case 127:
                        e.Style.ForegroundColor = Color.White;
                        e.Style.BackgroundColor = Color.PaleVioletRed;
                        break;

                    case 3:
                        e.Style.ForegroundColor = Color.White;
                        e.Style.BackgroundColor = Color.PaleVioletRed;
                        break;

                    case 2:
                        e.Style.ForegroundColor = Color.White;
                        e.Style.BackgroundColor = Color.CornflowerBlue;
                        break;
                }
            }
            e.Handled = true;
        }

        private void DataGrid_QueryCellStyle1(object sender, QueryCellStyleEventArgs e)
        {
            if (e.Column.MappingName == "VehiclePlate" && e.RowData is VehicleDebtMoneyResponse model)
            {
                switch (model.MessageIdBAP)
                {
                    case 128:
                        e.Style.ForegroundColor = Color.White;
                        e.Style.BackgroundColor = Color.BlueViolet;
                        break;

                    case 127:
                        e.Style.ForegroundColor = Color.White;
                        e.Style.BackgroundColor = Color.PaleVioletRed;
                        break;

                    case 3:
                        e.Style.ForegroundColor = Color.White;
                        e.Style.BackgroundColor = Color.PaleVioletRed;
                        break;

                    case 2:
                        e.Style.ForegroundColor = Color.White;
                        e.Style.BackgroundColor = Color.CornflowerBlue;
                        break;
                }
            }
            e.Handled = true;
        }
    }

    public class GridCaptionSummaryCellRendererExt : GridCaptionSummaryCellRenderer
    {
        public override void OnInitializeDisplayView(DataColumnBase dataColumn, SfLabel view)
        {
            base.OnInitializeDisplayView(dataColumn, view);
            view.HorizontalTextAlignment = TextAlignment.Start;
            view.FontAttributes = FontAttributes.Bold;
            view.FontSize = 12;
            view.Padding = new Thickness(5, 0);
            view.LineBreakMode = LineBreakMode.WordWrap;
            view.TextColor = Color.Black;
        }
    }
}