using Rg.Plugins.Popup.Pages;

using Syncfusion.SfCalendar.XForms;

using Xamarin.Forms;

namespace BA_MobileGPS.Core.Views
{
    public partial class SelectDateTimeCalendar : PopupPage
    {
        public SelectDateTimeCalendar()
        {
            InitializeComponent();

            calendar.MonthViewSettings.SelectionShape = SelectionShape.Fill;
            calendar.MonthViewSettings.DateSelectionColor = calendar.MonthViewSettings.TodaySelectionBackgroundColor;
            calendar.MonthViewSettings.SelectedDayTextColor = Color.White;
        }
    }
}