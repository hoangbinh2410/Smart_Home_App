using System;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.Views
{
    public partial class QCVN31SpeedReport : ContentPage
    {
        public QCVN31SpeedReport()
        {
            InitializeComponent();
            FixColumTablet();
        }

        private void FixColumTablet()
        {
            if (TargetIdiom.Tablet == Device.Idiom)
            {
                dataGrid.GridColumnSizer.DataGrid.Columns["OrderNumber"].Width = 60;
                dataGrid.GridColumnSizer.DataGrid.Columns["DT"].Width = 200;
                dataGrid.GridColumnSizer.DataGrid.Columns["Velocities"].Width = 600;
                dataGrid.GridColumnSizer.DataGrid.Columns["Decription"].Width = 150;
            }
        }
    }
}