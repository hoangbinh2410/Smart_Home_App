using BA_MobileGPS.Entities.ResponeEntity.Expense;
using System;
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
            try
            {
                ImageButton imageButton = (ImageButton)sender;
                ExpenseDetailsRespone commandParameter = (ExpenseDetailsRespone)imageButton.CommandParameter;
                popupLayout.PopupView.HeaderTitle = commandParameter.Name;
                popupLayout.Show();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
