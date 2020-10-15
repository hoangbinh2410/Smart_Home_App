using BA_MobileGPS.Entities;
using Realms.Exceptions;
using Syncfusion.SfDataGrid.XForms;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BA_MobileGPS.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PourFuelReportPage : ContentPage
    {
        private bool IsShowFilter = false;

        public PourFuelReportPage()
        {
            InitializeComponent();

            showhideColumn.TranslateTo(0, (int)Application.Current.MainPage.Height, 250);
            IsShowFilter = !IsShowFilter;

            FixColumTablet();
        }

        private void FixColumTablet()
        {
            if (TargetIdiom.Tablet == Device.Idiom)
            {
                dataGrid.GridColumnSizer.DataGrid.Columns["OrderNumber"].Width = 60;
                dataGrid.GridColumnSizer.DataGrid.Columns["StartTime"].Width = 200;
                dataGrid.GridColumnSizer.DataGrid.Columns["ChangingFuel"].Width = 200;
                dataGrid.GridColumnSizer.DataGrid.Columns["FuelStatus"].Width = 150;
                dataGrid.GridColumnSizer.DataGrid.Columns["CurrentAddress"].Width = 250;
            }
        }

        private void HideableToolbarItem_Clicked(object sender, EventArgs e)
        {
            if (IsShowFilter)
            {
                showhideColumn.TranslateTo(0, 0, 250);
            }
            else
            {
                showhideColumn.TranslateTo(0, showhideColumn.Height, 250);
            }
            IsShowFilter = !IsShowFilter;
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            HideableToolbarItem_Clicked(sender, e);
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            HideableToolbarItem_Clicked(sender, e);
        }
    }
}