using BA_MobileGPS.Core;
using BA_MobileGPS.Service;
using Prism;
using Prism.Ioc;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VMS_MobileGPS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RoutePage : ContentView, INavigationAware
    {
        private readonly IHelperAdvanceService helperAdvanceService;

        private bool infoWindowIsShown;
        private bool viewHasAppeared;

        public RoutePage()
        {
            InitializeComponent();

            map.InitialCameraUpdate = CameraUpdateFactory.NewPositionZoom(new Position(MobileUserSettingHelper.LatCurrentScreenMap, MobileUserSettingHelper.LngCurrentScreenMap), MobileUserSettingHelper.Mapzoom);

            map.UiSettings.ZoomControlsEnabled = false;
            map.UiSettings.RotateGesturesEnabled = false;

            map.PinClicked += Map_PinClicked;

            helperAdvanceService = PrismApplicationBase.Current.Container.Resolve<IHelperAdvanceService>();
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            OnPageAppearingFirstTime();
        }

        private double TimeSelectorContainerHeight;

        protected void OnPageAppearingFirstTime()
        {
            if (!viewHasAppeared)
            {
                TimeSelectorContainerHeight = Device.RuntimePlatform == Device.iOS ? TimeSelectorContainer.HeightRequest + 4 : TimeSelectorContainer.HeightRequest;

                AnimateHeight(TimeSelectorContainer, Callback, TimeSelectorContainerHeight, TimeSelectorContainerHeight * 2, length: 150);
                IsExpanded = true;

                IconInfo_Clicked(this, EventArgs.Empty);

                viewHasAppeared = true;
            }
        }

        private void Map_PinClicked(object sender, PinClickedEventArgs e)
        {
            if (!"state_stop".Equals(e.Pin.Tag) && !"direction".Equals(e.Pin.Tag))
                e.Handled = true;
        }

        private void IconInfo_Clicked(object sender, EventArgs e)
        {
            if (infoWindowIsShown)
            {
                IconInfo.Foreground = (Color)Prism.PrismApplicationBase.Current.Resources["GrayColor2"];
                frVehicleInfo.FadeTo(0, 200);
                frVehicleInfo.TranslateTo(-frVehicleInfo.Width - 5, 0, 250);
            }
            else
            {
                IconInfo.Foreground = (Color)Prism.PrismApplicationBase.Current.Resources["PrimaryColor"];
                frVehicleInfo.TranslateTo(0, 0, 250);
                frVehicleInfo.FadeTo(1, 200);
            }

            infoWindowIsShown = !infoWindowIsShown;
        }

        private bool IsExpanded;

        private void TimeSelector_Tapped(object sender, EventArgs e)
        {
            SetSelectedButton(sender as ContentView);

            if (IsExpanded)
            {
                AnimateHeight(TimeSelectorContainer, Callback, TimeSelectorContainerHeight * 2, TimeSelectorContainerHeight, length: 150);
                IsExpanded = false;
            }
        }

        private void TimeSelectorOther_Tapped(object sender, EventArgs e)
        {
            SetSelectedButton(sender as ContentView);

            if (!IsExpanded)
            {
                AnimateHeight(TimeSelectorContainer, Callback, TimeSelectorContainerHeight, TimeSelectorContainerHeight * 2, length: 150);
                IsExpanded = true;
            }
        }

        private void Callback(double input)
        {
            TimeSelectorContainer.HeightRequest = input; // update the height of the layout with this callback
        }

        private void AnimateHeight(View view, Action<double> callback, double start, double end, uint rate = 16, uint length = 250, Easing easing = null)
        {
            view.Animate("invis", callback, start, end, rate, length, easing ?? Easing.Linear);
        }

        private void SetSelectedButton(ContentView button)
        {
            button.BackgroundColor = Color.White;
            if (button.Content is Label lbl)
            {
                lbl.TextColor = (Color)Prism.PrismApplicationBase.Current.Resources["PrimaryColor"];
            }

            foreach (var btn in TimeSelector.Children.Where(b => b != button).Cast<ContentView>())
            {
                btn.BackgroundColor = (Color)Prism.PrismApplicationBase.Current.Resources["PrimaryColor"];
                if (btn.Content is Label lbl2)
                {
                    lbl2.TextColor = Color.White;
                }
            }
        }
    }
}