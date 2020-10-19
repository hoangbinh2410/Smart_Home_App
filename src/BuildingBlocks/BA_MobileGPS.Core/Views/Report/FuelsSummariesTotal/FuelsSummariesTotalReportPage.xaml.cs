using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BA_MobileGPS.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FuelsSummariesTotalReportPage : ContentPage
    {
        private bool IsShowFilter = false;

        public FuelsSummariesTotalReportPage()
        {
            InitializeComponent();

            showhideColumn.TranslateTo(0, (int)Application.Current.MainPage.Height, 250);
            IsShowFilter = !IsShowFilter;
            if (Device.RuntimePlatform == Device.Android)
            {
                dataGrid.RowHeight = 55;
            }

            FixColumTablet();
        }

        private void FixColumTablet()
        {
            if (TargetIdiom.Tablet == Device.Idiom)
            {
                dataGrid.GridColumnSizer.DataGrid.Columns["OrderNumber"].Width = 60;
                dataGrid.GridColumnSizer.DataGrid.Columns["StartTime"].Width = 200;
                dataGrid.GridColumnSizer.DataGrid.Columns["SumFirstLits"].Width = 160;
                dataGrid.GridColumnSizer.DataGrid.Columns["SumSuckTotal"].Width = 160;
                dataGrid.GridColumnSizer.DataGrid.Columns["LastLits"].Width = 160;
                dataGrid.GridColumnSizer.DataGrid.Columns["SumLiterConsumable"].Width = 160;
                dataGrid.GridColumnSizer.DataGrid.Columns["Detail"].Width = 160;
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