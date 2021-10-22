using Syncfusion.SfPicker.XForms;
using System.Collections.ObjectModel;

namespace BA_MobileGPS.Controls
{
    public class CustomMonthPicker : SfPicker
    {
        // Time api is used to modify the Hour collection as per change in Time
        /// <summary>
        /// Time is the acutal DataSource for SfPicker control which will holds the collection of Hour ,Minute and Format
        /// </summary>
        ///
        public ObservableCollection<object> Time { get; set; }

        public ObservableCollection<object> Month { get; set; }

        //Minute is the collection of minute numbers
        public ObservableCollection<object> Year;

        public CustomMonthPicker()
        {
            Time = new ObservableCollection<object>();
            Month = new ObservableCollection<object>();
            Year = new ObservableCollection<object>();

            //Enable Column Header of SfPicker
            ShowColumnHeader = true;
            //Enable Footer of SfPicker
            ShowFooter = false;
            //Enable Header of SfPicker
            ShowHeader = false;

            EnableLooping = true;

            //SfPicker header text
            HeaderText = "Chọn tháng";

            // Column header text collection
            ColumnHeaderText = new ObservableCollection<string>
            {
                "Tháng",
                "Năm"
            };

            PopulateTimeCollection();

            ItemsSource = Time;
        }

        private void PopulateTimeCollection()
        {
            //Populate Hour
            for (int i = 1; i <= 12; i++)
            {
                if (i < 10)
                    Month.Add(i.ToString());
                else
                    Month.Add(i.ToString());
            }

            //Populate Minute
            for (int j = 2010; j < 2030; j++)
            {
                Year.Add(j.ToString());
            }

            Time.Add(Month);
            Time.Add(Year);
        }
    }
}