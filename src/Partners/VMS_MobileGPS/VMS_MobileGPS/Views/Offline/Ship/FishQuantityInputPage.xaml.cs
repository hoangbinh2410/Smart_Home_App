using BA_MobileGPS.Entities;

using System;

using VMS_MobileGPS.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VMS_MobileGPS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FishQuantityInputPage : ContentPage
    {
        public FishQuantityInputPage()
        {
            InitializeComponent();
        }

        private async void Delete_Tapped(object sender, EventArgs e)
        {
            if (await DisplayAlert("Thông báo", "Bạn có chắc muốn xoá hay không?", "Có", "Không"))
            {
                if (((VisualElement)sender).BindingContext is FishTrip item && BindingContext is FishQuantityInputViewModel viewModel)
                {
                    viewModel.DeleteFishTrip(item);
                }
            }
            else
            {
                ListView.ResetSwipe();
            }
        }
    }
}