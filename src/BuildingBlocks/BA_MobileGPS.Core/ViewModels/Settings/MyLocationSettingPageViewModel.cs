using BA_MobileGPS.Core.GoogleMap.Behaviors;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;

using Prism.Commands;
using Prism.Navigation;

using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class MyLocationSettingPageViewModel : ViewModelBase
    {
        private readonly IUserService userService;
        public DelegateCommand<CameraIdledEventArgs> CameraIdledCommand { get; private set; }
        public ICommand ChangeMapTypeCommand { get; private set; }

        public ICommand SaveSettingsCommand { get; private set; }
        public ICommand MyLocationCommand { get; private set; }

        public MyLocationSettingPageViewModel(INavigationService navigationService, IUserService userService) : base(navigationService)
        {
            this.userService = userService;

            if (MobileUserSettingHelper.MapType == 4 || MobileUserSettingHelper.MapType == 5)
            {
                mapType = MapType.Hybrid;
            }
            else
            {
                mapType = MapType.Street;
            }
            latitude = (float)MobileUserSettingHelper.LatCurrent;
            latitude = (float)MobileUserSettingHelper.LngCurrent;
            zoomLevel = MobileUserSettingHelper.Mapzoom;

            SaveSettingsCommand = new Command(SaveSettings);
            CameraIdledCommand = new DelegateCommand<CameraIdledEventArgs>(UpdateMapInfo);
            ChangeMapTypeCommand = new DelegateCommand(ChangeMapType);
            MyLocationCommand = new DelegateCommand(GetMylocation);
        }

        #region Property

        public AnimateCameraRequest AnimateCameraRequest { get; } = new AnimateCameraRequest();

        private MapType mapType;
        public MapType MapType { get => mapType; set => SetProperty(ref mapType, value); }

        private double zoomLevel;

        public double ZoomLevel { get => zoomLevel; set => SetProperty(ref zoomLevel, value); }

        private float latitude;

        public float Latitude { get => latitude; set => SetProperty(ref latitude, value); }

        private float longitude;

        public float Longitude { get => longitude; set => SetProperty(ref longitude, value); }

        #endregion Property

        private void SaveSettings()
        {
            SafeExecute(async () =>
            {
                using (new HUDService())
                {
                    byte maptype = 1;

                    if (MapType == MapType.Hybrid)
                    {
                        maptype = 4;
                    }
                    var result = await userService.SetAdminUserSettings(new AdminUserConfiguration()
                    {
                        FK_UserID = UserInfo.UserId,
                        Latitude = Latitude,
                        Longitude = Longitude,
                        MapType = maptype,
                        MapZoom = (byte)ZoomLevel
                    });
                    if (result)
                    {
                        DisplayMessage.ShowMessageInfo("Đã lưu thành công");
                    }
                    else
                    {
                        DisplayMessage.ShowMessageInfo("Lưu không thành công bạn vui lòng kiểm tra lại");
                    }
                    MobileUserSettingHelper.Set(MobileUserConfigurationNames.MBMapType, maptype);
                    MobileUserSettingHelper.Set(MobileUserConfigurationNames.MBLatitude, Math.Round(Latitude, 2));
                    MobileUserSettingHelper.Set(MobileUserConfigurationNames.MBLongitude, Math.Round(Longitude, 2));
                    MobileUserSettingHelper.Set(MobileUserConfigurationNames.MBMapZoom, Math.Round(ZoomLevel));

                    await NavigationService.GoBackAsync();
                }
            });
        }

        private void UpdateMapInfo(CameraIdledEventArgs args)
        {
            if (args != null && args.Position != null)
            {
                Latitude = (float)args.Position.Target.Latitude;
                Longitude = (float)args.Position.Target.Longitude;
                ZoomLevel = args.Position.Zoom;
            }
        }

        private void ChangeMapType()
        {
            if (MapType == MapType.Street)
            {
                MapType = MapType.Hybrid;
            }
            else
            {
                MapType = MapType.Street;
            }
        }

        public async void GetMylocation()
        {
            var mylocation = await LocationHelper.GetGpsLocation();

            if (mylocation != null)
            {
                await AnimateCameraRequest.AnimateCamera(CameraUpdateFactory.NewPosition(new Position(mylocation.Latitude, mylocation.Longitude)), TimeSpan.FromMilliseconds(300));
            }
        }
    }
}