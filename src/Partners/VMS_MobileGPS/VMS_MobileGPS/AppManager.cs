using VMS_MobileGPS.Service;

namespace VMS_MobileGPS
{
    public class AppManager
    {
        public static BluetoothService BluetoothService { get; set; }

        public static void Init()
        {
            if (BluetoothService == null)
            {
                BluetoothService = new BluetoothService();
            }
        }
    }
}