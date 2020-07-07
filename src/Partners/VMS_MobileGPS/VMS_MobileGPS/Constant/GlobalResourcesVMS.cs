using BA_MobileGPS.Core;

using VMS_MobileGPS.Model;

namespace VMS_MobileGPS.Constant
{
    public sealed class GlobalResourcesVMS : ExtendedBindableObject
    {
        private static readonly GlobalResourcesVMS _current = new GlobalResourcesVMS();
        public static GlobalResourcesVMS Current => _current;

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static GlobalResourcesVMS()
        {
        }

        private GlobalResourcesVMS()
        {
        }

        private int totalMessage;

        public int TotalMessage
        {
            get { return totalMessage; }
            set
            {
                totalMessage = value;

                RaisePropertyChanged(() => TotalMessage);
            }
        }

        private int totalByteSms;

        public int TotalByteSms
        {
            get { return totalByteSms; }
            set
            {
                totalByteSms = value;

                RaisePropertyChanged(() => TotalByteSms);
            }
        }

        private DeviceManager deviceManager = new DeviceManager();

        public DeviceManager DeviceManager
        {
            get { return deviceManager; }
            set
            {
                deviceManager = value;

                RaisePropertyChanged(() => DeviceManager);
            }
        }
        private PermissionManager permissionManager = new PermissionManager();
        public PermissionManager PermissionManager
        {
            get { return permissionManager; }
            set
            {
                permissionManager = value;

                RaisePropertyChanged(() => PermissionManager);
            }
        }
        private int offMapZoomLevel = 8;
        public int OffMapZoomLevel
        {
            get { return offMapZoomLevel; }
            set
            {
                offMapZoomLevel = value;

                RaisePropertyChanged(() => OffMapZoomLevel);
            }
        }
        private int maxOffMapZoom = 9;
        public int MaxOffMapZoom
    {
            get { return maxOffMapZoom; }
            set {
                maxOffMapZoom = value;

                RaisePropertyChanged(() => MaxOffMapZoom);
            }
        }
        private int minOffMapZoom = 5;
        public int MinOffMapZoom
        {
            get { return minOffMapZoom; }
            set
            {
                minOffMapZoom = value;

                RaisePropertyChanged(() => MinOffMapZoom);
            }
        }
    }
}