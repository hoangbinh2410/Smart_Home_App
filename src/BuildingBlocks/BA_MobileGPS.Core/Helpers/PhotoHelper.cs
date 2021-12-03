using System;
using System.Threading.Tasks;

namespace BA_MobileGPS.Core.Helpers
{
    public class PhotoHelper
    {
        public static async Task CanTakePhoto(Action action)
        {
            if (await PermissionHelper.CheckPhotoPermissions())
            {
                action?.Invoke();
            }
        }

        public static async Task CanPickPhoto(Action action = null)
        {
            if (await PermissionHelper.CheckStoragePermissions())
            {
                action?.Invoke();
            }
        }
    }
}