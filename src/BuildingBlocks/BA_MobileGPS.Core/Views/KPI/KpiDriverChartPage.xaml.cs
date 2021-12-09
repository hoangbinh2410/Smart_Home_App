﻿using BA_MobileGPS.Core.Resources;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.Views
{
    public partial class KpiDriverChartPage : ContentPage
    {
        public KpiDriverChartPage()
        {
            InitializeComponent();
            if (Device.RuntimePlatform == Device.iOS)
            {
                Chart.PrimaryAxis.LabelStyle.FontSize = 5;
                chart2.PrimaryAxis.LabelStyle.FontSize = 5;
            }                             
        }

        private void NumericalAxis_LabelCreated(object sender, Syncfusion.SfChart.XForms.ChartAxisLabelEventArgs e)
        {
            int data = int.Parse(e.LabelContent);
            if (data == 5)
            {
                e.LabelContent = "A";
            }
            if (data == 4)
            {
                e.LabelContent = "B";
            }
            if (data == 3)
            {
                e.LabelContent = "C";
            }
            if (data == 2)
            {
                e.LabelContent = "D";
            }
            if (data == 1)
            {
                e.LabelContent = "E";
            }
            if (data == 0)
            {
                e.LabelContent = "";
            }
        }

        private void primary_LabelCreated(object sender, Syncfusion.SfChart.XForms.ChartAxisLabelEventArgs e)
        {
            if (e.LabelStyle != null)
            {
                if (Device.RuntimePlatform == Device.iOS)
                {
                    e.LabelStyle.FontSize = 5;
                }
                else
                {
                    e.LabelStyle.FontSize = 10;
                }
            }
        }
    }
}