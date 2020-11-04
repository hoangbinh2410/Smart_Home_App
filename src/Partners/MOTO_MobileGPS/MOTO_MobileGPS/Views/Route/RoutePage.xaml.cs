using BA_MobileGPS.Core;
using BA_MobileGPS.Core.Events;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Core.ViewModels;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.RealmEntity;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;
using Prism;
using Prism.Events;
using Prism.Ioc;
using Prism.Navigation;
using System;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MOTO_MobileGPS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RoutePage : ContentView, INavigationAware, IDestructible
    {
        private readonly IRealmBaseService<BoundaryRealm, LandmarkResponse> boundaryRepository;
        private readonly IEventAggregator eventAggregator;

        private bool infoWindowIsShown;
        private bool viewHasAppeared;
        private RouteViewModel vm;

        public RoutePage()
        {
            InitializeComponent();

            boundaryRepository = PrismApplicationBase.Current.Container.Resolve<IRealmBaseService<BoundaryRealm, LandmarkResponse>>();

            // Initialize the View Model Object
            vm = (RouteViewModel)BindingContext;

            map.InitialCameraUpdate = CameraUpdateFactory.NewPositionZoom(new Position(MobileUserSettingHelper.LatCurrentScreenMap, MobileUserSettingHelper.LngCurrentScreenMap), MobileUserSettingHelper.Mapzoom);

            map.UiSettings.ZoomControlsEnabled = false;
            map.UiSettings.RotateGesturesEnabled = false;

            map.PinClicked += Map_PinClicked;

            eventAggregator = PrismApplicationBase.Current.Container.Resolve<IEventAggregator>();
            eventAggregator.GetEvent<ThemeChangedEvent>().Subscribe(ThemeChanged);
            lblMore.Text = MobileResource.Route_Label_More.Trim().ToUpper();
            entrySearch.Placeholder = MobileResource.Route_Label_SearchFishing;
            lblTitle.Text = MobileResource.Route_Label_Title;
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            OnPageAppearingFirstTime();
            GoogleMapAddBoundary();
            GoogleMapAddName();
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

        private void GoogleMapAddBoundary()
        {
            vm.Boundaries.Clear();
            //vm.Polylines.Clear();

            foreach (var line in vm.Polylines.ToList().FindAll(l => "Boundary".Equals(l.Tag)))
            {
                vm.Polylines.Remove(line);
            }

            var listBoudary = boundaryRepository.Find(b => b.IsShowBoudary);

            foreach (var item in listBoudary)
            {
                AddBoundary(item);
            }
        }

        private void AddBoundary(LandmarkResponse boundary)
        {
            try
            {
                var result = boundary.Polygon.Split(',');

                var color = Color.FromHex(ConvertIntToHex(boundary.Color));

                if (boundary.IsClosed)
                {
                    var polygon = new Polygon
                    {
                        IsClickable = true,
                        StrokeWidth = 1f,
                        StrokeColor = color.MultiplyAlpha(.5),
                        FillColor = color.MultiplyAlpha(.3),
                        Tag = "Boundary"
                    };

                    for (int i = 0; i < result.Length; i += 2)
                    {
                        polygon.Positions.Add(new Position(FormatHelper.ConvertToDouble(result[i + 1], 6), FormatHelper.ConvertToDouble(result[i], 6)));
                    }

                    vm.Boundaries.Add(polygon);
                }
                else
                {
                    var polyline = new Polyline
                    {
                        IsClickable = false,
                        StrokeColor = color,
                        StrokeWidth = 2f,
                        Tag = "Boundary"
                    };

                    for (int i = 0; i < result.Length; i += 2)
                    {
                        polyline.Positions.Add(new Position(FormatHelper.ConvertToDouble(result[i + 1], 6), FormatHelper.ConvertToDouble(result[i], 6)));
                    }

                    vm.Polylines.Add(polyline);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
        }

        public static string ConvertIntToHex(int value)
        {
            return value.ToString("X").PadLeft(6, '0');
        }

        private void GoogleMapAddName()
        {
            try
            {
                var listName = boundaryRepository.Find(b => b.IsShowName);

                foreach (var pin in map.Pins.Where(p => p.Tag.ToString().Contains("Boundary")).ToList())
                {
                    map.Pins.Remove(pin);
                }

                foreach (var item in listName)
                {
                    AddName(item);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
        }

        private void AddName(LandmarkResponse name)
        {
            try
            {
                map.Pins.Add(new Pin
                {
                    Label = name.Name,
                    Position = new Position(name.Latitude, name.Longitude),
                    Icon = BitmapDescriptorFactory.FromView(new BoundaryNameInfoWindow(name.Name) { WidthRequest = name.Name.Length < 20 ? 6 * name.Name.Length : 110, HeightRequest = 18 * ((name.Name.Length / 20) + 1) }),
                    Tag = "Boundary" + name.Name
                });
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
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

        private void ThemeChanged()
        {
            foreach (var btn in TimeSelector.Children.Cast<ContentView>())
            {
                btn.BackgroundColor = (Color)Prism.PrismApplicationBase.Current.Resources["PrimaryColor"];
                if (btn.Content is Label lbl2)
                {
                    lbl2.TextColor = Color.White;
                }
            }
        }

        public void Destroy()
        {
            eventAggregator.GetEvent<ThemeChangedEvent>().Unsubscribe(ThemeChanged);
        }
    }
}