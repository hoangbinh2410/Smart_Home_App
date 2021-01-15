using Syncfusion.SfPicker.XForms;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

using Xamarin.Forms;

using SelectionChangedEventArgs = Syncfusion.SfPicker.XForms.SelectionChangedEventArgs;

namespace BA_MobileGPS.Controls
{
    public class CustomDatePicker : SfPicker
    {
        #region Bindable Properties

        public static readonly BindableProperty DateTimeProperty = BindableProperty.Create(nameof(DateTime), typeof(DateTime), typeof(DateTimePicker), DateTime.Now, BindingMode.TwoWay, propertyChanged: OnDateTimeChanged);

        public DateTime DateTime
        {
            get { return (DateTime)GetValue(DateTimeProperty); }
            set { SetValue(DateTimeProperty, value); }
        }

        private static void OnDateTimeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is DateTimePicker control))
                return;

            if (newValue == oldValue || newValue == null)
            {
                return;
            }

            if (!DateTime.TryParse(newValue.ToString(), out DateTime dateTime))
                return;

            control.SelectedItem = new ObservableCollection<object>
            {
                //Select today dates
                dateTime.Year.ToString(),
                dateTime.Date.Month < 10 ? "0" + dateTime.Date.Month :dateTime.Date.Month.ToString(),
                dateTime.Date.Day < 10 ? "0" + dateTime.Date.Day : dateTime.Date.Day.ToString(),
            };
        }

        #endregion Bindable Properties

        #region Public Properties

        // Months api is used to modify the Day collection as per change in Month
        internal Dictionary<string, string> Months { get; set; }

        /// <summary>
        /// Date is the acutal DataSource for SfPicker control which will holds the collection of Day ,Month and Year
        /// </summary>
        /// <value>The date.</value>
        public ObservableCollection<object> Date { get; set; }

        // Day is the collection of day numbers
        internal ObservableCollection<object> Day { get; set; }

        // Month is the collection of Month Names
        internal ObservableCollection<object> Month { get; set; }

        // Year is the collection of Years from 1990 to 2042
        internal ObservableCollection<object> Year { get; set; }

        /// <summary>
        /// Headers api is holds the column name for every column in date picker
        /// </summary>
        /// <value>The Headers.</value>
        public ObservableCollection<string> Headers { get; set; }

        #endregion Public Properties

        public CustomDatePicker()
        {
            Months = new Dictionary<string, string>();

            Date = new ObservableCollection<object>();
            Day = new ObservableCollection<object>();
            Month = new ObservableCollection<object>();
            Year = new ObservableCollection<object>();
           
            Headers = new ObservableCollection<string>
            {
                Core.Resources.MobileResource.Common_Label_Month,
                Core.Resources.MobileResource.Common_Label_DayUpper,
                Core.Resources.MobileResource.Common_Label_Year
            };
            HeaderText = Core.Resources.MobileResource.Common_Label_DatePicker;

            PopulateDateCollection();

            ItemsSource = Date;
            ColumnHeaderText = Headers;
            SelectionChanged += CustomDatePicker_SelectionChanged;

            ShowFooter = true;
            ShowHeader = true;
            ShowColumnHeader = true;

            OkButtonClicked += DateTimePicker_OkButtonClicked;
        }

        private void DateTimePicker_OkButtonClicked(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedItem != null && SelectedItem is ObservableCollection<object> SelectedItems
                && int.TryParse(SelectedItems[0] as string, out int year) && int.TryParse(SelectedItems[1] as string, out int month)
                && int.TryParse(SelectedItems[2] as string, out int day))
            {
                DateTime = new DateTime(year, month, day);
            }
        }

        private void CustomDatePicker_SelectionChanged(object sender, Syncfusion.SfPicker.XForms.SelectionChangedEventArgs e)
        {
            UpdateDays(Date, e);
        }

        //Updatedays method is used to alter the Date collection as per selection change in Month column(if feb is Selected day collection has value from 1 to 28)
        public void UpdateDays(ObservableCollection<object> Date, Syncfusion.SfPicker.XForms.SelectionChangedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (Date.Count == 3)
                {
                    bool isupdate = false;
                    if (e.OldValue != null && e.NewValue != null && (e.OldValue as ObservableCollection<object>).Count == (e.NewValue as ObservableCollection<object>).Count)
                    {
                        if (!Equals((e.OldValue as IList)[0], (e.NewValue as IList)[0]))
                        {
                            isupdate = true;
                        }
                        if (!Equals((e.OldValue as IList)[2], (e.NewValue as IList)[2]))
                        {
                            isupdate = true;
                        }
                    }

                    if (isupdate)
                    {
                        ObservableCollection<object> days = new ObservableCollection<object>();
                        int month = DateTime.ParseExact(Months[(e.NewValue as IList)[0].ToString()], "MMMM", CultureInfo.DefaultThreadCurrentCulture).Month;
                        int year = int.Parse((e.NewValue as IList)[2].ToString());
                        for (int j = 1; j <= DateTime.DaysInMonth(year, month); j++)
                        {
                            if (j < 10)
                            {
                                days.Add("0" + j);
                            }
                            else
                                days.Add(j.ToString());
                        }
                        ObservableCollection<object> oldvalue = new ObservableCollection<object>();

                        foreach (var item in e.NewValue as IList)
                        {
                            oldvalue.Add(item);
                        }
                        if (days.Count > 0)
                        {
                            Date.RemoveAt(1);
                            Date.Insert(1, days);
                        }

                        if ((Date[1] as IList).Contains(oldvalue[1]))
                        {
                            SelectedItem = oldvalue;
                        }
                        else
                        {
                            oldvalue[1] = (Date[1] as IList)[(Date[1] as IList).Count - 1];
                            SelectedItem = oldvalue;
                        }
                    }
                }
            });
        }

        private void PopulateDateCollection()
        {
            //populate months
            for (int i = 1; i < 13; i++)
            {
                if (!Months.ContainsKey(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i)))
                    Months.Add(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i), CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i));
                Month.Add(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i));
            }

            //populate year
            for (int i = 1950; i < 2050; i++)
            {
                Year.Add(i.ToString());
            }

            //populate Days
            for (int i = 1; i <= DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month); i++)
            {
                if (i < 10)
                {
                    Day.Add("0" + i);
                }
                else
                    Day.Add(i.ToString());
            }

            Date.Add(Month);
            Date.Add(Day);              
            Date.Add(Year);
        }
    }
}