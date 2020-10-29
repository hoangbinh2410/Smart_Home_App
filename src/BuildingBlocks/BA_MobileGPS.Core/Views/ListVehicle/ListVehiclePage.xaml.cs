using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Core.ViewModels;
using BA_MobileGPS.Utilities;
using Prism;
using Prism.Ioc;
using Prism.Events;
using Prism.Navigation;

using System;
using System.Reflection;
using System.Timers;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using BA_MobileGPS.Core.ViewModels.Base;

namespace BA_MobileGPS.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListVehiclePage : ContentView
    {
        private enum States
        {
            ShowStatus,
            HideStatus
        }

        private Timer timer;

        private bool infoStatusIsShown = false;

        private readonly BA_MobileGPS.Core.Animation animations = new BA_MobileGPS.Core.Animation();

        private ListVehiclePageViewModel vm;

        private readonly IEventAggregator eventAggregator;

        public ListVehiclePage()
        {
            try
            {
                InitializeComponent();

                // Initialize the View Model Object
                vm = (ListVehiclePageViewModel)BindingContext;

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
            entrySearchVehicle.Placeholder = MobileResource.Online_Label_SeachVehicle2;
            lblNotFound.Text = MobileResource.ListVehicle_Label_NotFound;
            eventAggregator = PrismApplicationBase.Current.Container.Resolve<IEventAggregator>();
            eventAggregator.GetEvent<DestroyEvent>().Subscribe(Destroy);
        }

        private async void FilterCarType_Tapped(object sender, EventArgs e)
        {
            if (infoStatusIsShown)
            {
                await animations.Go(States.HideStatus, true);
            }
            else
            {
                vm.CacularVehicleStatus();
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

        private void Destroy()
        {
            timer.Stop();
            timer.Dispose();
            eventAggregator.GetEvent<DestroyEvent>().Unsubscribe(Destroy);
        }
    }
}