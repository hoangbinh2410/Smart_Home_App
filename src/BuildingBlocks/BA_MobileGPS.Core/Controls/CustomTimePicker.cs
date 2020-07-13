using BA_MobileGPS.Core.Resource;

using Syncfusion.SfPicker.XForms;

using System.Collections.ObjectModel;

namespace BA_MobileGPS.Controls
{
    public class CustomTimePicker : SfPicker
    {
        // Time api is used to modify the Hour collection as per change in Time
        /// <summary>
        /// Time is the acutal DataSource for SfPicker control which will holds the collection of Hour ,Minute and Format
        /// </summary>
        public ObservableCollection<object> Time { get; set; }

        //Minute is the collection of minute numbers
        public ObservableCollection<object> Minute;

        //Hour is the collection of hour numbers
        public ObservableCollection<object> Hour;

        public CustomTimePicker()
        {
            Time = new ObservableCollection<object>();
            Hour = new ObservableCollection<object>();
            Minute = new ObservableCollection<object>();

            //Enable Column Header of SfPicker
            ShowColumnHeader = true;
            //Enable Footer of SfPicker
            ShowFooter = false;
            //Enable Header of SfPicker
            ShowHeader = false;

            EnableLooping = true;

            //SfPicker header text
            HeaderText = MobileResource.Common_Label_TimePicker;

            // Column header text collection
            ColumnHeaderText = new ObservableCollection<string>
            {
                MobileResource.Common_Label_Hour,
                MobileResource.Common_Label_Minute
            };

            PopulateTimeCollection();

            ItemsSource = Time;
        }

        private void PopulateTimeCollection()
        {
            //Populate Hour
            for (int i = 0; i < 24; i++)
            {
                if (i < 10)
                    Hour.Add("0" + i);
                else
                    Hour.Add(i.ToString());
            }

            //Populate Minute
            for (int j = 0; j < 60; j++)
            {
                if (j < 10)
                    Minute.Add("0" + j);
                else
                    Minute.Add(j.ToString());
            }

            Time.Add(Hour);
            Time.Add(Minute);
        }
    }
}