using Syncfusion.Data;
using Syncfusion.SfDataGrid.XForms;
using System;
using System.Globalization;

using Xamarin.Forms;

namespace BA_MobileGPS.Core
{
    public class TableSummaryConverter : IValueConverter
    {
        public string ColName { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var data = value != null ? value as SummaryRecordEntry : null;
            if (data != null)
            {
                SfDataGrid dataGrid = (SfDataGrid)parameter;
                var summaryText = Math.Round(decimal.Parse(SummaryCreator.GetSummaryDisplayText(data, ColName, dataGrid.View)),1);
                return summaryText.ToString();
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}