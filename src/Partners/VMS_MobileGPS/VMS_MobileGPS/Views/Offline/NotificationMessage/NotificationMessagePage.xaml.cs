using BA_MobileGPS.Models;

using System;

using VMS_MobileGPS.ViewModels;

using Xamarin.Forms;

namespace VMS_MobileGPS.Views
{
    public partial class NotificationMessagePage : ContentPage
    {
        public NotificationMessagePage()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async void Delete_Tapped(object sender, EventArgs e)
        {
            if (await DisplayAlert("Tin nhắn", "Bạn có chắc muốn xoá hay không?", "Có", "Không"))
            {
                if (((VisualElement)sender).BindingContext is MessageSOS item && BindingContext is NotificationMessageViewModel viewModel)
                {
                    viewModel.DeleteMessage(item);
                }
            }
            else
            {
                ListView.ResetSwipe();
            }
        }
    }
}