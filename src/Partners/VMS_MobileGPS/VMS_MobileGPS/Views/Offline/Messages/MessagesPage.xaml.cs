using BA_MobileGPS.Models;

using System;

using VMS_MobileGPS.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VMS_MobileGPS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MessagesPage : ContentPage
    {
        public MessagesPage()
        {
            InitializeComponent();
        }

        private async void Delete_Tapped(object sender, EventArgs e)
        {
            if (await DisplayAlert("Tin nhắn", "Bạn có chắc muốn xoá tin nhắn này hay không?", "Có", "Không"))
            {
                if (((VisualElement)sender).BindingContext is MessageByUser item && BindingContext is MessagesViewModel viewModel)
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