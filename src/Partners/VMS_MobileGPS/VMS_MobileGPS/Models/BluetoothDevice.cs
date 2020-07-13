using System;

namespace VMS_MobileGPS.Models
{
    public class BluetoothDevice
    {
        public int Index { get; set; }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Rssi { get; set; }
    }
}