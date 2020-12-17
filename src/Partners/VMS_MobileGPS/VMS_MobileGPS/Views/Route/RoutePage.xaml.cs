using BA_MobileGPS.Core;
using BA_MobileGPS.Core.Events;
using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Core.Resources;
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
using VMS_MobileGPS.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VMS_MobileGPS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RoutePage : ContentPage, INavigationAware
    {
        private readonly IRealmBaseService<BoundaryRealm, LandmarkResponse> boundaryRepository;
        private bool infoWindowIsShown = true;
        private double TimeSelectorContainerHeight;
        private bool IsExpanded;
        private RoutePageViewModel vm;
        public RoutePage()
        {
            InitializeComponent();
            lblMore.Text = MobileResource.Route_Label_More.Trim().ToUpper();
            entrySearch.Placeholder = MobileResource.Route_Label_SearchFishing;
            lblTitle.Text = MobileResource.Route_Label_TitleVMS;
            map.InitialCameraUpdate = CameraUpdateFactory.NewPositionZoom(new Position(MobileUserSettingHelper.LatCurrentScreenMap, MobileUserSettingHelper.LngCurrentScreenMap), MobileUserSettingHelper.Mapzoom);
            map.IsUseCluster = false;
            map.IsTrafficEnabled = false;
            map.UiSettings.ZoomGesturesEnabled = true;
            map.UiSettings.ZoomControlsEnabled = false;
            map.UiSettings.RotateGesturesEnabled = false;
            frVehicleInfo.TranslationX = 0;
            IconInfo.Foreground = (Color)PrismApplicationBase.Current.Resources["PrimaryColor"];
            TimeSelectorContainerHeight = Device.RuntimePlatform == Device.iOS ? TimeSelectorContainer.HeightRequest + 4 : TimeSelectorContainer.HeightRequest;
            IsExpanded = false;
            boundaryRepository = PrismApplicationBase.Current.Container.Resolve<IRealmBaseService<BoundaryRealm, LandmarkResponse>>();
            // Initialize the View Model Object
            vm = (RoutePageViewModel)BindingContext;
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
                LoggerHelper.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
           
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            GoogleMapAddBoundary();
            GoogleMapAddName();
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
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
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
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
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
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }
    }
}