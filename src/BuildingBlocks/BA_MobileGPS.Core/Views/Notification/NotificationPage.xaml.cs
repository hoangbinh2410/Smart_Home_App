using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Core.ViewModels;
using BA_MobileGPS.Entities;
using System;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.Views
{
    public partial class NotificationPage : ContentPage
    {
        public NotificationPage()
        {
            InitializeComponent();
        }

        private async void Delete_Tapped(object sender, EventArgs e)
        {
            if (await DisplayAlert(MobileResource.Notification_Label_TilePage, MobileResource.Notification_Label_DeleteNotice, MobileResource.Notification_Label_DeleteAllNoticeAction, MobileResource.Common_Message_Skip))
            {
                if (((VisualElement)sender).BindingContext is NotificationRespone item && BindingContext is NotificationPageViewModel viewModel)
                {
                    viewModel.DeleteNotification(item);
                }
            }
            else
            {
                listView.ResetSwipe();
            }
        }

        private void ReadNoitice_Tapped(object sender, EventArgs e)
        {
            if (((VisualElement)sender).BindingContext is NotificationRespone item && BindingContext is NotificationPageViewModel viewModel)
            {
                viewModel.UpdateIsReadNotification(item, false);
                listView.ResetSwipe();
            }
        }
    }
}