using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BA_MobileGPS.Core
{
    /// <summary>
    /// Class làm việc với quyền
    /// </summary>
    /// <Modified>
    /// Name     Date         Comments
    /// Namth  11/27/2017   created
    /// </Modified>
    public static class PermissionHelper
    {
        private const string POSITIVE = "Cài đặt";
        private const string NEGATIVE = "Để sau";

        /// <summary>
        /// Kiểm tra xem có quyền truy cập Camera không thì mới tiếp tục cho phép hoạt động.
        /// </summary>
        /// <param name="permission">The permission.</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// Truongpv  09/08/2018   created
        /// </Modified>
        public static async Task<bool> CheckCameraPermissions()
        {
            var title = "Quyền truy cập camera";
            var question = "Chức năng yêu cầu quyền truy cập camera của bạn.";

            return await CheckPermission<Plugin.Permissions.CameraPermission>(Permission.Camera, title, question);
        }

        /// <summary>
        /// Kiểm tra xem có quyền truy cập thư mục ảnh không thì mới tiếp tục cho phép hoạt động.
        /// </summary>
        /// <param name="permission">The permission.</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// Truongpv  09/08/2018   created
        /// </Modified>
        public static async Task<bool> CheckPhotoPermissions()
        {
            var title = "Quyền truy cập thư mục ảnh";
            var question = "Chức năng yêu cầu quyền truy cập thư mục ảnh của bạn.";

            return await CheckPermission<PhotosPermission>(Permission.Photos, title, question);
        }

        /// <summary>
        /// Kiểm tra xem có quyền truy cập thư mục ảnh không thì mới tiếp tục cho phép hoạt động.
        /// </summary>
        /// <param name="permission">The permission.</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// Truongpv  09/08/2018   created
        /// </Modified>
        public static async Task<bool> CheckStoragePermissions()
        {
            var title = "Quyền truy cập bộ nhớ";
            var question = "Chức năng yêu cầu quyền truy cập bộ nhớ của bạn.";

            return await CheckPermission<StoragePermission>(Permission.Storage, title, question);
        }

        /// <summary>
        /// Kiểm tra xem có quyền không thì mới tiếp tục cho phép hoạt động.
        /// </summary>
        /// <param name="permission">The permission.</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// Truongpv  09/08/2018   created
        /// </Modified>
        public static async Task<bool> CheckPermission<TPermission>(Permission permission, string title, string question) where TPermission : BasePermission, new()
        {
            return await CheckPermission<TPermission>(permission, title, question, POSITIVE, NEGATIVE);
        }

        /// <summary>
        /// Kiểm tra xem có quyền không thì mới tiếp tục cho phép hoạt động.
        /// </summary>
        /// <param name="permission">The permission.</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// Truongpv  09/08/2018   created
        /// </Modified>
        public static async Task<bool> CheckPermission<TPermission>(Permission permission, string title, string question, string positive, string negative) where TPermission : BasePermission, new()
        {
            bool request = false;

            var permissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync<TPermission>();

            if (permissionStatus == PermissionStatus.Denied)
            {
                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(permission))
                {
                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        var task = Application.Current?.MainPage?.DisplayAlert(title, question, positive, negative);
                        if (task == null)
                            return false;

                        var result = await task;
                        if (result)
                        {
                            CrossPermissions.Current.OpenAppSettings();
                        }

                        return false;
                    }
                }

                request = true;
            }

            if (request || permissionStatus != PermissionStatus.Granted)
            {
                permissionStatus = await CrossPermissions.Current.RequestPermissionAsync<TPermission>();

                if (permissionStatus != PermissionStatus.Granted)
                {
                    try
                    {
                        var task = Application.Current?.MainPage?.DisplayAlert(title, question, positive, negative);

                        if (task == null)
                            return false;

                        var result = await task;
                        if (result)
                        {
                            CrossPermissions.Current.OpenAppSettings();
                        }

                        return false;
                    }
                    catch (Exception ex)
                    {
                        var a = ex.Message;
                        Debugger.Log(2, "aaaaa", a);
                        return false;
                    }
                }
            }

            return true;

            //return true;
        }

        public static async Task<bool> CheckLocationPermissions()
        {
            bool result = false;
            var permissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync<Plugin.Permissions.LocationPermission>();
            //Nếu từ chối quyền
            if (permissionStatus == PermissionStatus.Denied)
            {
                if (Device.RuntimePlatform == Device.iOS)
                {
                    OpenAppSettings();

                    return false;
                }

                result = true;
            }
            if (result || permissionStatus != PermissionStatus.Granted)
            {
                var newStatus = await CrossPermissions.Current.RequestPermissionAsync<Plugin.Permissions.LocationPermission>();
                if (newStatus != PermissionStatus.Granted)
                {
                    OpenAppSettings();

                    return false;
                }
                else
                {
                    result = true;
                }
            }
            else
            {
                result = true;
            }
            return result;
        }

        private static async void OpenLocationSettings()
        {
            var title = "Quyền truy cập vị trí";
            var question = "Chức năng yêu cầu quyền truy cập vị trí của bạn.";

            if (Application.Current?.MainPage != null)
            {
                var task = Application.Current?.MainPage?.DisplayAlert(title, question, POSITIVE, NEGATIVE);
                if (task == null)
                    return;

                var result = await task;
                if (result)
                {
                    DependencyService.Get<ISettingsService>().OpenLocationSettings();
                }
            }
        }

        private static async void OpenAppSettings()
        {
            var title = "Quyền truy cập vị trí";
            var question = "Chức năng yêu cầu quyền truy cập vị trí của bạn.";

            if (Application.Current?.MainPage != null)
            {
                var task = Application.Current?.MainPage?.DisplayAlert(title, question, POSITIVE, NEGATIVE);
                if (task == null)
                    return;

                var result = await task;
                if (result)
                {
                    CrossPermissions.Current.OpenAppSettings();
                }
            }
        }

        public static async Task RequestPermission<TPermission>(PopupPage page, Action action = null) where TPermission : BasePermission, new()
        {
            var permissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync<TPermission>();
            if (permissionStatus != PermissionStatus.Granted)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await PopupNavigation.Instance.PushAsync(page);
                });
            }
            else
            {
                action?.Invoke();
            }
        }

        public static async Task CheckLocationAsync(Action action)
        {
            await RequestPermission<LocationPermission>(new Views.Permissions.LocationPermission(action), action);
        }
    }
}