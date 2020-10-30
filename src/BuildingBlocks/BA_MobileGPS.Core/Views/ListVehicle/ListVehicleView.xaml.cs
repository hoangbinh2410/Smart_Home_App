using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Core.ViewModels;
using System;
using System.Timers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BA_MobileGPS.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListVehicleView : ContentView
    {
        private Timer timer;
        private ListVehiclePageViewModel vm;
        private bool infoStatusIsShown = false;
        private int pageWidth = 0;
        public ListVehicleView()
        {
            InitializeComponent();
            // Initialize the View Model Object
            
            entrySearchVehicle.Placeholder = MobileResource.Online_Label_SeachVehicle2;
            lblNotFound.Text = MobileResource.ListVehicle_Label_NotFound;
            pageWidth = (int)Application.Current.MainPage.Width;
            boxStatusVehicle.TranslationX = pageWidth;
            StartTimmerCaculatorStatus();
        }

        private void FilterCarType_Tapped(object sender, EventArgs e)
        {
            if (infoStatusIsShown)
            {
                HideBoxStatus();
            }
            else
            {
                vm = (ListVehiclePageViewModel)BindingContext;
                vm.CacularVehicleStatus();
                ShowBoxStatus();
            }
        }

        private void HideBoxStatus()
        {
            Action<double> callback = input => boxStatusVehicle.TranslationX = input;
            boxStatusVehicle.Animate("animboxStatusVehicle", callback, 0, pageWidth, 16, 300, Easing.CubicInOut);
            infoStatusIsShown = false;
        }

        private void ShowBoxStatus()
        {
            Action<double> callback = input => boxStatusVehicle.TranslationX = input;
            boxStatusVehicle.Animate("animboxStatusVehicle", callback, pageWidth, 0, 16, 300, Easing.CubicInOut);
            infoStatusIsShown = true;
        }

        private void TapStatusVehicle(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            HideBoxStatus();
        }

        private void TappedHidenBoxStatus(object sender, EventArgs e)
        {
            HideBoxStatus();
        }

        private void SwipeGestureBoxStatus(object sender, SwipedEventArgs e)
        {
            switch (e.Direction)
            {
                case SwipeDirection.Right:
                    HideBoxStatus();
                    break;

                case SwipeDirection.Left:
                    break;

                case SwipeDirection.Up:
                    break;

                case SwipeDirection.Down:
                    break;
            }
        }

        private void SwipeShowBoxStatus(object sender, SwipedEventArgs e)
        {
            switch (e.Direction)
            {
                case SwipeDirection.Right:
                    ShowBoxStatus();
                    break;

                case SwipeDirection.Left:
                    break;

                case SwipeDirection.Up:
                    break;

                case SwipeDirection.Down:
                    break;
            }
        }

        private void StartTimmerCaculatorStatus()
        {
            timer = new Timer
            {
                Interval = 15000
            };
            timer.Elapsed += T_Elapsed;

            timer.Start();
        }

        private void T_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (infoStatusIsShown)
            {
                vm = (ListVehiclePageViewModel)BindingContext;
                vm.CacularVehicleStatus();
            }
        }
    }
}