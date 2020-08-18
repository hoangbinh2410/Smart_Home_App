using LibVLCSharp.Forms.Shared;
using System;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.Views
{
    public partial class CameraStream : ContentPage
    {
        public CameraStream()
        {
            InitializeComponent();
            SizeChanged += CameraStream_SizeChanged;

       
        }

        private void CameraStream_SizeChanged(object sender, EventArgs e)
        {
            if (Width > Height)
            {
               
                media.VerticalOptions = LayoutOptions.Fill;
            }
            else
            {
               
                media.VerticalOptions = LayoutOptions.Start;
            }
        }

        private void AspectRatioButton_Clicked(object sender, EventArgs e)
        {
            // media.MediaPlayer.
            uint m = 0;
            uint n = 0;
            var a = media.MediaPlayer.Size(0,ref m, ref n);
           

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
           MessagingCenter.Send(this, "Unspecified");
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Send(this, "Portrait");
        }
       
    }
}
