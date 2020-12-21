using System;
using BA_MobileGPS.Core.Resources;

using System.Timers;

using VMS_MobileGPS.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VMS_MobileGPS.Views.ListVehicle
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
            lblNotFound.Text = MobileResource.ListVehicle_Label_NotFound;
            entrySearchVehicle.Placeholder = MobileResource.Online_Label_SeachVehicle2;
            int pageWidth = (int)Application.Current.MainPage.Width;
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
                default:
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
                default:
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