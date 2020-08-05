using BA_MobileGPS.Core.Resource;
using BA_MobileGPS.Core.ViewModels;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;

using Prism.Navigation;

using System;
using System.Linq;
using System.Reflection;
using System.Timers;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BA_MobileGPS.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListVehiclePage : ContentView, IDestructible
    {
        private enum States
        {
            ShowFilter,
            HideFilter,
            ShowStatus,
            HideStatus
        }

        private Timer timer;
        private readonly IHelperAdvanceService _helperAdvanceService;

        private bool infoStatusIsShown = false;

        private readonly Animation animations = new Animation();

        private ListVehiclePageViewModel vm;

        public ListVehiclePage(IHelperAdvanceService helperAdvanceService)
        {
            try
            {
                InitializeComponent();

                // Initialize the View Model Object
                vm = (ListVehiclePageViewModel)BindingContext;

                this._helperAdvanceService = helperAdvanceService;

                int pageWidth = (int)Application.Current.MainPage.Width;

                AbsoluteLayout.SetLayoutBounds(boxStatusVehicle, new Rectangle(1, 0, pageWidth, 1));

                animations.Add(States.ShowStatus, new[]
                {
                    new ViewTransition(boxStatusVehicle, AnimationType.TranslationX, 0, (uint)pageWidth, delay: 200), // Active and visible
                    new ViewTransition(boxStatusVehicle, AnimationType.Opacity, 1, 0), // Active and visible
                });

                animations.Add(States.HideStatus, new[]
                {
                    new ViewTransition(boxStatusVehicle, AnimationType.TranslationX, pageWidth),
                    new ViewTransition(boxStatusVehicle, AnimationType.Opacity, 0),
                });

                Device.BeginInvokeOnMainThread(async () =>
                {
                    await animations.Go(States.HideStatus, false);
                });

                StartTimmerCaculatorStatus();
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        private async void FilterCarType_Tapped(object sender, EventArgs e)
        {
            if (infoStatusIsShown)
            {
                await animations.Go(States.HideStatus, true);
            }
            else
            {
                await animations.Go(States.ShowStatus, true);
            }

            infoStatusIsShown = !infoStatusIsShown;
        }

        private async void HideBoxStatus()
        {
            await animations.Go(States.HideStatus, true);

            infoStatusIsShown = false;
        }

        private async void ShowBoxStatus()
        {
            await animations.Go(States.ShowStatus, true);

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
                vm.CacularVehicleStatus();
            }
        }

        public void Destroy()
        {
            timer.Stop();
            timer.Dispose();
        }
    }
}