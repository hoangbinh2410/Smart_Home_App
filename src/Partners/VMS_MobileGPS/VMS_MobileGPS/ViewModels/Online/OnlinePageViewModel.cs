using BA_MobileGPS.Core;
using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Events;
using BA_MobileGPS.Core.Extensions;
using BA_MobileGPS.Core.GoogleMap.Behaviors;
using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Core.Resource;
using BA_MobileGPS.Core.ViewModels;
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
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using VMS_MobileGPS.Views;
using Xamarin.Forms;

namespace VMS_MobileGPS.ViewModels
{
   public class OnlinePageViewModel : ViewModelBase
    {
        private readonly IRealmBaseService<BoundaryRealm, LandmarkResponse> boundaryRepository;
        private readonly IUserService userService;

        public ObservableCollection<Circle> Circles { get; set; } = new ObservableCollection<Circle>();
        public ObservableCollection<Polygon> Boundaries { get; set; } = new ObservableCollection<Polygon>();
        public ObservableCollection<Polyline> Borders { get; set; } = new ObservableCollection<Polyline>();

        public ICommand NavigateToSettingsCommand { get; private set; }
        public ICommand ChangeMapTypeCommand { get; private set; }
        public ICommand PushToRouterPageCommand { get; private set; }
        public ICommand PushToFABPageCommand { get; private set; }
        public DelegateCommand<CameraIdledEventArgs> CameraIdledCommand { get; private set; }
        public DelegateCommand PushToDetailPageCommand { get; private set; }
        public DelegateCommand ShowBorderCommand { get; private set; }
        public DelegateCommand HideBorderCommand { get; private set; }
        public ICommand MyLocationCommand { get; private set; }
        public ICommand PushtoListVehicleOnlineCommand { get; private set; }
        public DelegateCommand GoDistancePageCommand { get; private set; }

        public OnlinePageViewModel(INavigationService navigationService, IRealmBaseService<BoundaryRealm, LandmarkResponse> boundaryRepository,
            IUserService userService)
            : base(navigationService)
        {
            this.boundaryRepository = boundaryRepository;
            this.userService = userService;

            if (Settings.CurrentCompany != null && Settings.CurrentCompany.FK_CompanyID > 0)
            {
                Title = Settings.CurrentCompany.CompanyName;
            }
            else
            {
                Title = MobileResource.Online_Label_TitlePage;
            }

            carActive = new VehicleOnline();
            selectedVehicleGroup = new List<int>();

            //if (MobileUserSettingHelper.MapType == 4 || MobileUserSettingHelper.MapType == 5)
            //{
            //    mapType = MapType.Hybrid;
            //    ColorMapType = (Color)App.Current.Resources["Color_Navigation"];
            //}
            //else
            //{
            //    mapType = MapType.Street;
            //    ColorMapType = (Color)App.Current.Resources["Color_Placeholder"];
            //}

            zoomLevel = MobileUserSettingHelper.Mapzoom;

            NavigateToSettingsCommand = new DelegateCommand(NavigateToSettings);
            ChangeMapTypeCommand = new DelegateCommand(ChangeMapType);
            PushToRouterPageCommand = new DelegateCommand(PushtoRouterPage);
            PushToFABPageCommand = new DelegateCommand<object>(PushtoFABPage);
            PushToDetailPageCommand = new DelegateCommand(PushtoDetailPage);
            CameraIdledCommand = new DelegateCommand<CameraIdledEventArgs>(UpdateMapInfo);
            ShowBorderCommand = new DelegateCommand(ShowBorder);
            HideBorderCommand = new DelegateCommand(HideBorder);
            MyLocationCommand = new DelegateCommand(GetMylocation);
            PushtoListVehicleOnlineCommand = new DelegateCommand(PushtoListVehicleOnlinePage);
            GoDistancePageCommand = new DelegateCommand(GoDistancePage);
        }

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

        public override void OnPageAppearingFirstTime()
        {
            base.OnPageAppearingFirstTime();

            //if (PermissionHelper.CheckLocationPermissions(false).Result)
            //{
            //    if (GetControl<Map>("googleMap") is Map map)
            //    {
            //        TryExecute(() =>
            //        {
            //            map.MyLocationEnabled = true;
            //        });
            //    }

            //}
        }

        public static int ConvertHexToInt(string value)
        {
            int ret = 0;
            try
            {
                // strip the leading 0x
                if (value.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                {
                    value = value.Substring(2);
                }
                if (value.StartsWith("#", StringComparison.OrdinalIgnoreCase))
                {
                    value = value.Substring(1);
                }

                ret = int.Parse(value, NumberStyles.HexNumber);
            }
            catch
            {
                // ignored
            }
            return ret;
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

        #region Property

        public AnimateCameraRequest AnimateCameraRequest { get; } = new AnimateCameraRequest();

        private MapType mapType;
        public MapType MapType { get => mapType; set => SetProperty(ref mapType, value); }

        private MapSpan visibleRegion;
        public MapSpan VisibleRegion { get => visibleRegion; set => SetProperty(ref visibleRegion, value); }

        private Color colorMapType;
        public Color ColorMapType { get => colorMapType; set => SetProperty(ref colorMapType, value); }

        private List<int> selectedVehicleGroup;
        public List<int> SelectedVehicleGroup { get => selectedVehicleGroup; set => SetProperty(ref selectedVehicleGroup, value); }

        private VehicleOnline carActive;

        public VehicleOnline CarActive
        {
            get { return carActive; }
            set
            {
                carActive = value;
                RaisePropertyChanged();
            }
        }

        private double zoomLevel;

        public double ZoomLevel { get => zoomLevel; set => SetProperty(ref zoomLevel, value); }

        public string currentAddress = string.Empty;
        public string CurrentAddress { get => currentAddress; set => SetProperty(ref currentAddress, value); }

        #endregion Property

        #region Private Method

        private void PushtoRouterPage()
        {
            SafeExecute(async () =>
            {
                var navigationPara = new NavigationParameters
                {
                    { ParameterKey.VehicleOnline, CarActive }
                };

                await NavigationService.NavigateAsync("RoutePage", navigationPara, false);
            });
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

        public void HideBorder()
        {
            Circles.Clear();

            RaisePropertyChanged(nameof(Circles));
        }

        public void NavigateToSettings()
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("BoundaryPage");
            });
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

        private void PushtoFABPage(object index)
        {
            SafeExecute(async () =>
            {
                switch ((int)index)
                {
                    case 1:
                        await NavigationService.NavigateAsync("ListVehiclePage", null, useModalNavigation: false);
                        break;

                    case 2:

                        StaticSettings.ClearStaticSettings();
                        GlobalResources.Current.TotalAlert = 0;

                        await NavigationService.NavigateAsync("/NavigationPage/LandingPage");

                        break;

                    case 3:
                        await NavigationService.NavigateAsync("MenuNavigationPage/HeplerPage", null, useModalNavigation: true);
                        break;
                }
            });
        }

        public void GetMylocation()
        {
            SafeExecute(async () =>
            {
                var mylocation = await LocationHelper.GetGpsLocation();

                if (mylocation != null)
                {
                    if (GetControl<Map>("googleMap") is Map map)
                    {
                        TryExecute(() =>
                        {
                            if (!map.MyLocationEnabled)
                            {
                                map.MyLocationEnabled = true;
                            }
                        });
                    }
                    await AnimateCameraRequest.AnimateCamera(CameraUpdateFactory.NewPosition(new Position(mylocation.Latitude, mylocation.Longitude)), TimeSpan.FromMilliseconds(300));
                }
            });
        }

        private void UpdateMapInfo(CameraIdledEventArgs args)
        {
            if (args != null && args.Position != null)
            {
                ZoomLevel = args.Position.Zoom;
            }
        }

        private void PushtoDetailPage()
        {
            SafeExecute(async () =>
            {
                var parameters = new NavigationParameters
                {
                    { ParameterKey.CarDetail, CarActive }
                };

                await NavigationService.NavigateAsync(PageNames.VehicleDetailPage.ToString(), parameters, false);
            });
        }

        private void PushtoListVehicleOnlinePage()
        {
            SafeExecute(async () =>
            {
                var navigationParameters = new NavigationParameters
                {
                    { ParameterKey.OnlinePage, true }
                };
                await NavigationService.NavigateAsync("ListVehiclePage", navigationParameters, useModalNavigation: false);
            });
        }

        public void GoDistancePage()
        {
            SafeExecute(async () =>
            {
                var parameters = new NavigationParameters
                {
                    { ParameterKey.VehicleOnline, CarActive }
                };

                await NavigationService.NavigateAsync("DistancePage", parameters, false);
            });
        }

        //private void OpenDiscoreryBox()
        //{
        //    PopupNavigation.Instance.PushAsync(new OnlineCarInfo());
        //}
        #endregion Private Method
    }
}
