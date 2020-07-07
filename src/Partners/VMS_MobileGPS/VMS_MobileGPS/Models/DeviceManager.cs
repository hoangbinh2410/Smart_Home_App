using BA_MobileGPS.Core;

using VMS_MobileGPS.Service;

namespace VMS_MobileGPS.Model
{
    public class DeviceManager : ExtendedBindableObject
    {
        private string deviceName;

        public string DeviceName
        {
            get { return deviceName; }
            set
            {
                deviceName = value;

                RaisePropertyChanged(() => DeviceName);
            }
        }

        private string devicePlate;

        public string DevicePlate
        {
            get { return devicePlate; }
            set
            {
                devicePlate = value;

                RaisePropertyChanged(() => DevicePlate);
            }
        }

        public BleConnectionState state = BleConnectionState.NO_CONNECTION;

        public BleConnectionState State
        {
            get { return state; }
            set
            {
                state = value;

                RaisePropertyChanged(() => State);
            }
        }

        private bool isSendSos = false;

        public bool IsSendSOS
        {
            get { return isSendSos; }
            set
            {
                isSendSos = value;

                RaisePropertyChanged(() => IsSendSOS);
            }
        }
    }
}