using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Core.Resources;
using System;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BA_MobileGPS.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RouteView : ContentView
    {
        private bool infoWindowIsShown = true;
        private double TimeSelectorContainerHeight;
        private bool IsExpanded;

        public RouteView()
        {
            InitializeComponent();
            lblMore.Text = MobileResource.Route_Label_More.Trim().ToUpper();
            entrySearch.Placeholder = MobileResource.Route_Label_SearchFishing;
            lblTitle.Text = MobileResource.Route_Label_Title;
            map.InitialCameraUpdate = CameraUpdateFactory.NewPositionZoom(new Position(MobileUserSettingHelper.LatCurrentScreenMap, MobileUserSettingHelper.LngCurrentScreenMap), MobileUserSettingHelper.Mapzoom);
            map.IsUseCluster = false;
            map.IsTrafficEnabled = false;
            map.UiSettings.ZoomGesturesEnabled = true;
            map.UiSettings.ZoomControlsEnabled = false;
            map.UiSettings.RotateGesturesEnabled = false;
            frVehicleInfo.TranslationX = 0;
            IconInfo.Foreground = (Color)Prism.PrismApplicationBase.Current.Resources["PrimaryColor"];
            TimeSelectorContainerHeight = Device.RuntimePlatform == Device.iOS ? TimeSelectorContainer.HeightRequest + 4 : TimeSelectorContainer.HeightRequest;
            IsExpanded = false;
        }

        public static string ConvertIntToHex(int value)
        {
            return value.ToString("X").PadLeft(6, '0');
        }

        private void IconInfo_Clicked(object sender, EventArgs e)
        {
            if (infoWindowIsShown)
            {
                IconInfo.Foreground = (Color)Prism.PrismApplicationBase.Current.Resources["GrayColor2"];
                Action<double> frcallback = input2 => frVehicleInfo.TranslationX = input2;
                frVehicleInfo.Animate("animehicleInfo", frcallback, 0, -300, 16, 300, Easing.CubicInOut);
            }
            else
            {
                IconInfo.Foreground = (Color)Prism.PrismApplicationBase.Current.Resources["PrimaryColor"];
                Action<double> frcallback = input2 => frVehicleInfo.TranslationX = input2;
                frVehicleInfo.Animate("animehicleInfo", frcallback, -300, 0, 16, 300, Easing.CubicInOut);
            }

            infoWindowIsShown = !infoWindowIsShown;
        }

        public double CalcCurrentValue(double from, double to, double animationRatio)
        {
            return (from + (to - from) * animationRatio);
        }

        private void TimeSelector_Tapped(object sender, EventArgs e)
        {
            SetSelectedButton(sender as ContentView);

            if (IsExpanded)
            {
                TimeSelectorContainer.Animate("invis", new Animation((d) =>
                {
                    TimeSelectorContainer.HeightRequest = CalcCurrentValue(TimeSelectorContainerHeight * 2, TimeSelectorContainerHeight, d);
                }),
              length: 200,
              easing: Easing.Linear);
                IsExpanded = false;
            }
        }

        private void TimeSelectorOther_Tapped(object sender, EventArgs e)
        {
            SetSelectedButton(sender as ContentView);

            if (!IsExpanded)
            {
                TimeSelectorContainer.Animate("invis", new Animation((d) =>
                {
                    TimeSelectorContainer.HeightRequest = CalcCurrentValue(TimeSelectorContainerHeight, TimeSelectorContainerHeight * 2, d);
                }),
               length: 200,
               easing: Easing.Linear);
                IsExpanded = true;
            }
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

        public async void GetMylocation(object sender, EventArgs e)
        {
            try
            {
                var mylocation = await LocationHelper.GetGpsLocation();

                if (mylocation != null)
                {
                    if (!map.MyLocationEnabled)
                    {
                        map.MyLocationEnabled = true;
                    }
                    await map.AnimateCamera(CameraUpdateFactory.NewPosition(new Position(mylocation.Latitude, mylocation.Longitude)), TimeSpan.FromMilliseconds(300));
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
        }
    }
}