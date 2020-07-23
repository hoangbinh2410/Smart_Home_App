using BA_MobileGPS.Core;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VMS_MobileGPS.Constant
{
    public class PermissionManager : ExtendedBindableObject
    {
        public PermissionManager()
        {

                IsCameraGranted =  CrossPermissions.Current.CheckPermissionStatusAsync<CameraPermission>().Result == PermissionStatus.Granted;
                IsLocationGranted =  CrossPermissions.Current.CheckPermissionStatusAsync<LocationPermission>().Result == PermissionStatus.Granted;
                IsPhotoGranted =  CrossPermissions.Current.CheckPermissionStatusAsync<PhotosPermission>().Result == PermissionStatus.Granted;
                IsStorageGranted =  CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>().Result == PermissionStatus.Granted;
                
        }
        private bool isCameraGranted;

        public bool IsCameraGranted
        {
            get { return isCameraGranted; }
            set
            {
                isCameraGranted = value;

                RaisePropertyChanged(() => IsCameraGranted);
            }
        }

        private bool isLocationGranted;

        public bool IsLocationGranted
        {
            get { return isLocationGranted; }
            set
            {
                isLocationGranted = value;

                RaisePropertyChanged(() => IsLocationGranted);
            }
        }

        private bool isPhotoGranted;

        public bool IsPhotoGranted
        {
            get { return isPhotoGranted; }
            set
            {
                isPhotoGranted = value;

                RaisePropertyChanged(() => IsPhotoGranted);
            }
        }

        private bool isStorageGranted;

        public bool IsStorageGranted
        {
            get { return isStorageGranted; }
            set
            {
                isStorageGranted = value;

                RaisePropertyChanged(() => IsStorageGranted);
            }
        }
    }
}
