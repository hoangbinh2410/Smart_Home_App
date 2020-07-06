//using BA_MobileGPS.Core.Resource;
//using BA_MobileGPS.Core.Views.Permissions;
//using Plugin.Media;
//using Plugin.Permissions;
//using Rg.Plugins.Popup.Services;
//using System;
//using System.Threading.Tasks;

//using Xamarin.Forms;

//namespace BA_MobileGPS.Core.Helpers
//{
//    public class PhotoHelper
//    {
//        public static async Task CanTakePhoto(Action action)
//        {
//            await PermissionHelper.
//                 RequestPermission<Plugin.Permissions.CameraPermission>(
//                            new Views.Permissions.CameraPermission(async () => await RequireStoragePermission(action)),
//                            async () => await RequireStoragePermission(action));

//        }

//        public static async Task CanPickPhoto(Action action = null)
//        {
//            await PermissionHelper.
//                  RequestPermission<PhotosPermission>(
//                             new PhotoPermission(async () => await RequireStoragePermission(action)),
//                             async () => await RequireStoragePermission(action));
//        }

//        private static async Task RequireStoragePermission(Action action = null)
//        {
//            await PermissionHelper.
//                  RequestPermission<Plugin.Permissions.StoragePermission>(new Views.Permissions.StoragePermission(action), action);
//        }

//    }
//}