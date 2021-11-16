using BA_MobileGPS.Entities;
using Syncfusion.Data;
using Syncfusion.SfDataGrid.XForms;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.Views.Report.StationDetails
{
    public partial class StationDetailsPage : ContentPage
    {
        private bool IsShowFilter = false;
        public StationDetailsPage()
        {
            InitializeComponent();

            showhideColumn.TranslateTo(0, (int)Application.Current.MainPage.Height, 250);
            IsShowFilter = !IsShowFilter;

            if (Device.RuntimePlatform == Device.Android)
            {
                dataGrid.RowHeight = 55;
            }
        }

        private void HideableToolbarItem_Clicked(object sender, System.EventArgs e)
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

        private void TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            HideableToolbarItem_Clicked(sender, e);
        }

        private void Button_Clicked(object sender, System.EventArgs e)
        {
            HideableToolbarItem_Clicked(sender, e);
        }
    }
}
