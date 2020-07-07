using BA_MobileGPS.Core;
using BA_MobileGPS.Core.Events;
using BA_MobileGPS.Core.ViewModels.Base;
using BA_MobileGPS.Core.Views;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.RealmEntity;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;
using Microsoft.AppCenter;
using Prism.Commands;
using Prism.Common;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using Realms.Sync;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using VMS_MobileGPS.Views;
using Xamarin.Forms;

namespace VMS_MobileGPS.ViewModels
{
   public class OnlinePageViewModel : ViewModelBase
    {
        #region Contractor
        public IEventAggregator _eventAggregator;

        private readonly IRealmBaseService<BoundaryRealm, LandmarkResponse> boundaryRepository;

        private readonly IUserService userService;

        public ICommand HotlineTapCommand { get; }

        public ICommand ChangeMapTypeCommand { get; private set; }

        public DelegateCommand<CameraIdledEventArgs> CameraIdledCommand { get; private set; }

        public DelegateCommand ShowBorderCommand { get; private set; }

        public OnlinePageViewModel(INavigationService navigationService, IEventAggregator eventAggregator, 
            IUserService userService, IRealmBaseService<BoundaryRealm, LandmarkResponse> boundaryRepository) : base(navigationService)
        {
            this.boundaryRepository = boundaryRepository;
            this.userService = userService;
            _eventAggregator = eventAggregator;
            HotlineTapCommand = new DelegateCommand(HotlineTap);
            _bottomGroupMargin = new Thickness(15, 30);
            _eventAggregator.GetEvent<DetailVehiclePopupCloseEvent>().Subscribe(DetailVehiclePopupClose);
            ChangeMapTypeCommand = new DelegateCommand(ChangeMapType);
            CameraIdledCommand = new DelegateCommand<CameraIdledEventArgs>(UpdateMapInfo);
            ShowBorderCommand = new DelegateCommand(ShowBorder);
        }

        #endregion

        #region Property

        private MapType mapType;
        public MapType MapType { get => mapType; set => SetProperty(ref mapType, value); }

        private MapSpan visibleRegion;
        public MapSpan VisibleRegion { get => visibleRegion; set => SetProperty(ref visibleRegion, value); }

        private Color colorMapType;
        public Color ColorMapType { get => colorMapType; set => SetProperty(ref colorMapType, value); }

        private Thickness _bottomGroupMargin;
        public Thickness BottomGroupMargin
        {
            get { return _bottomGroupMargin; }
            set
            {
                SetProperty(ref _bottomGroupMargin, value);
                RaisePropertyChanged();
            }
        }

        private VehicleOnline carActive;

        public VehicleOnline CarActive
        {
            get { return carActive; }
            set
            {
                SetProperty(ref carActive, value);
                RaisePropertyChanged();
            }
        }

        private double zoomLevel;

        public double ZoomLevel { get => zoomLevel; set => SetProperty(ref zoomLevel, value); }

        public ObservableCollection<Circle> Circles { get; set; } = new ObservableCollection<Circle>();

        public ObservableCollection<Polygon> Boundaries { get; set; } = new ObservableCollection<Polygon>();

        public ObservableCollection<Polyline> Borders { get; set; } = new ObservableCollection<Polyline>();

        #endregion

        #region Private Method

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            Boundaries.Clear();
            Borders.Clear();

            var listBoudary = boundaryRepository.Find(b => b.IsShowBoudary);

            foreach (var item in listBoudary)
            {
                AddBoundary(item);
            }

            var listName = boundaryRepository.Find(b => b.IsShowName);

            if (GetControl<Map>("googleMap") is Map map)
            {
                TryExecute(() =>
                {
                    foreach (var pin in map.Pins.Where(p => p.Tag.ToString().Contains("Boundary")).ToList())
                    {
                        map.Pins.Remove(pin);
                    }
                });
            }

            foreach (var item in listName)
            {
                AddName(item);
            }
        }

        private void ChangeMapType()
        {
            SafeExecute(async () =>
            {
                if (MapType == MapType.Street)
                {
                    MapType = MapType.Hybrid;
                    ColorMapType = (Color)App.Current.Resources["Color_Navigation"];
                }
                else
                {
                    ColorMapType = (Color)App.Current.Resources["Color_Placeholder"];
                    MapType = MapType.Street;
                }
                byte maptype = 1;
                if (MapType == MapType.Hybrid)
                {
                    maptype = 4;
                }
                var result = await userService.SetAdminUserSettings(new AdminUserConfiguration()
                {
                    FK_UserID = UserInfo.UserId,
                    Latitude = (float)MobileUserSettingHelper.LatCurrent,
                    Longitude = (float)MobileUserSettingHelper.LngCurrent,
                    MapType = maptype,
                    MapZoom = (byte)MobileUserSettingHelper.Mapzoom
                });
                MobileUserSettingHelper.Set(MobileUserConfigurationNames.MBMapType, maptype);
            });
        }

        private void HotlineTap()
        {
            if (PopupNavigation.Instance.PopupStack.Count > 0)
            {
                PopupNavigation.Instance.PopAllAsync();
            }
            //PopupNavigation.Instance.PushAsync(new DetailVehiclePopup());
            BottomGroupMargin = new Thickness(15, 30, 15, 130);

        }

        private void DetailVehiclePopupClose()
        {
            BottomGroupMargin = new Thickness(15, 30);
        }

        public static string ConvertIntToHex(int value)
        {
            return value.ToString("X").PadLeft(6, '0');
        }

        private void AddBoundary(LandmarkResponse boundary)
        {
            TryExecute(() =>
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
                        Tag = "POLYGON"
                    };

                    for (int i = 0; i < result.Length; i += 2)
                    {
                        polygon.Positions.Add(new Position(FormatHelper.ConvertToDouble(result[i + 1], 6), FormatHelper.ConvertToDouble(result[i], 6)));
                    }

                    polygon.Clicked += Polygon_Clicked;
                    Boundaries.Add(polygon);
                }
                else
                {
                    var polyline = new Polyline
                    {
                        IsClickable = false,
                        StrokeColor = color,
                        StrokeWidth = 2f,
                        Tag = "POLYGON"
                    };

                    for (int i = 0; i < result.Length; i += 2)
                    {
                        polyline.Positions.Add(new Position(FormatHelper.ConvertToDouble(result[i + 1], 6), FormatHelper.ConvertToDouble(result[i], 6)));
                    }

                    Borders.Add(polyline);
                }
            });
        }

        private void AddName(LandmarkResponse name)
        {
            TryExecute(() =>
            {
                if (GetControl<Map>("googleMap") is Map map)
                {
                    TryExecute(() =>
                    {
                        map.Pins.Add(new Pin
                        {
                            Label = name.Name,
                            Position = new Position(name.Latitude, name.Longitude),
                            Icon = BitmapDescriptorFactory.FromView(new BoundaryNameInfoWindow(name.Name) { WidthRequest = name.Name.Length < 20 ? 6 * name.Name.Length : 110, HeightRequest = 18 * ((name.Name.Length / 20) + 1) }),
                            Tag = "Boundary" + name.Name
                        });
                    });
                }
            });
        }

        private void Polygon_Clicked(object sender, EventArgs e)
        {
            //if (PageUtilities.GetCurrentPage(Application.Current.MainPage) is Views.OnlinePage onlinePage)
            //{
            //    onlinePage.HideBoxInfo();
            //}

            //if (PageUtilities.GetCurrentPage(Application.Current.MainPage) is Views.OnlinePageNoCluster onlinePageNoCluster)
            //{
            //    onlinePageNoCluster.HideBoxInfo();
            //}
        }

        public void ShowBorder()
        {
            Circles.Clear();

            Circles.Add(new Circle
            {
                StrokeWidth = 2,
                StrokeColor = (Color)App.Current.Resources["Color_Navigation"],
                FillColor = Color.Transparent,
                Radius = Distance.FromKilometers(10 * 1.852),
                Center = new Position(CarActive.Lat, CarActive.Lng)
            });

            Circles.Add(new Circle
            {
                StrokeWidth = 2,
                StrokeColor = (Color)App.Current.Resources["Color_Navigation"],
                FillColor = Color.Transparent,
                Radius = Distance.FromKilometers(20 * 1.852),
                Center = new Position(CarActive.Lat, CarActive.Lng)
            });

            Circles.Add(new Circle
            {
                StrokeWidth = 2,
                StrokeColor = (Color)App.Current.Resources["Color_Navigation"],
                FillColor = Color.Transparent,
                Radius = Distance.FromKilometers(30 * 1.852),
                Center = new Position(CarActive.Lat, CarActive.Lng)
            });

            RaisePropertyChanged(nameof(Circles));
        }

        private void UpdateMapInfo(CameraIdledEventArgs args)
        {
            if (args != null && args.Position != null)
            {
                ZoomLevel = args.Position.Zoom;
            }
        }

        #endregion 
    }
}
