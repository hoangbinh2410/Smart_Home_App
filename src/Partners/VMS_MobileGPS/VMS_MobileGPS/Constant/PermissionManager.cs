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
            Task.Run(async () =>
            {
                IsCameraGranted = await CrossPermissions.Current.CheckPermissionStatusAsync<CameraPermission>() == PermissionStatus.Granted;
                IsLocationGranted = await CrossPermissions.Current.CheckPermissionStatusAsync<LocationPermission>() == PermissionStatus.Granted;
                IsPhotoGranted = await CrossPermissions.Current.CheckPermissionStatusAsync<PhotosPermission>() == PermissionStatus.Granted;
                IsStorageGranted = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>() == PermissionStatus.Granted;
            });            
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
