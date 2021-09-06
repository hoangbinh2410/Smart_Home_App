using BA_MobileGPS.Core.Resources;
using System;
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

            // Namth: thêm đoạn check quyền location thì mới cho phép tiếp tục hoạt động.
            if (await PermissionHelper.CheckLocationPermissions())
            {
                try
                {
                    var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                    var location = await Geolocation.GetLocationAsync(request);

                    if (location != null)
                    {
                        position = location;
                        Settings.Latitude = (float)position.Latitude;
                        Settings.Longitude = (float)position.Longitude;
                    }
                }
                catch (FeatureNotSupportedException)
                {
                    await Application.Current?.MainPage?.DisplayAlert(MobileResource.Common_Label_Notification, MobileResource.Common_Message_GpsNotfound, MobileResource.Common_Button_OK);
                    // Handle not supported on device exception
                }
                catch (FeatureNotEnabledException)
                {
                    await Application.Current?.MainPage?.DisplayAlert(MobileResource.Common_Label_Notification, MobileResource.Common_Message_GpsNotfound, MobileResource.Common_Button_OK);
                }
                catch (PermissionException)
                {
                    await Application.Current?.MainPage?.DisplayAlert(MobileResource.Common_Label_Notification, MobileResource.Common_Message_GpsNotfound, MobileResource.Common_Button_OK);
                }
                catch (Exception)
                {
                    await Application.Current?.MainPage?.DisplayAlert(MobileResource.Common_Label_Notification, MobileResource.Common_Message_GpsNotfound, MobileResource.Common_Button_OK);
                }
            }

            return position;
        }
    }
}