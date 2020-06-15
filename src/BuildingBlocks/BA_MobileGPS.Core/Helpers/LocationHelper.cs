using BA_MobileGPS.Utilities;

using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;

using System;
using System.Reflection;
using System.Threading.Tasks;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace BA_MobileGPS.Core
{
    /// <summary>
    /// Tiện ích dùng cho dịch vụ vị trí
    /// Move từ BaseService của NamTH sang cho tập trung.
    /// </summary>
    /// <Modified>
    /// Name     Date         Comments
    /// Namth  27/11/2017   created
    /// </Modified>
    public class LocationHelper
    {
        /// <summary>
        /// Lấy vị trí người dùng khi mở apps
        /// </summary>
        /// <returns>namth/14/09/2017</returns>
        public static async Task<Xamarin.Essentials.Location> GetGpsLocation()
        {
            // Namth:  Nếu không có quyền thì lấy mặc định là vị trí công ty Bình Anh
            Xamarin.Essentials.Location position = new Xamarin.Essentials.Location(MobileUserSettingHelper.LatCurrentScreenMap, MobileUserSettingHelper.LngCurrentScreenMap);

            try
            {
                // Namth: thêm đoạn check quyền location thì mới cho phép tiếp tục hoạt động.
                if (await PermissionHelper.CheckLocationPermissions())
                {
                    var locator = CrossGeolocator.Current;

                    // Thêm lệnh này để chạy location trên Android
                    locator.DesiredAccuracy = 100;

                    if (locator.IsGeolocationEnabled)
                    {
                        var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                        position = await Geolocation.GetLocationAsync();

                        Settings.Latitude = (float)position.Latitude;
                        Settings.Longitude = (float)position.Longitude;
                    }
                    // Chưa bật định vị
                    else
                    {
                        await Application.Current?.MainPage?.DisplayAlert("Thông báo", "GPS chưa được bật trên thiết bị của bạn. Vui lòng kiểm tra cài đặt trên điện thoại.", "Đồng ý");
                    }
                }
            }
            catch (GeolocationException geoEx)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, geoEx);
                return position;
            }
            catch (TaskCanceledException ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
                return position;
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);

                return position;
            }

            return position;
        }
    }
}