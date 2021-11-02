using Syncfusion.XForms.PopupLayout;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.Views.Expense
{
    public partial class ExpenseDetailsPage : ContentPage
    {
        public ExpenseDetailsPage()
        {
            InitializeComponent();
        }

        private void ShowSfPopup_Clicked(object sender, System.EventArgs e)
        {
            popupLayout.Show();
        }
    }
}
