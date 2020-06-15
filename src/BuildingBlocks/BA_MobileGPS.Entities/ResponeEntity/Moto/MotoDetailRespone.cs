using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Entities
{
    public class MotoDetailRespone
    {
        public MotoDetailViewModel MotoDetailResult { set; get; }
    }

    public class MotoDetailViewModel : BaseModel
    {
        public long IMEI { set; get; }

        public string XNCode { set; get; }

        public string VehiclePlate { set; get; }

        private bool isOnline;
        public bool IsOnline { get => isOnline; set => SetProperty(ref isOnline, value); }

        public float Latitude { set; get; }

        public float Longitude { set; get; }

        public int Speed { set; get; }

        public int Status { set; get; }

        public string BTS { set; get; }

        public int AccuVoltage { set; get; }

        public int BatteryVoltage { set; get; }

        public int PowerMode { set; get; }

        public DateTime ConnectTime { set; get; }

        public DateTime UpdatePostionTime { set; get; }

        public DateTime DisconnectTime { set; get; }

        private string address;
        public string Address { get => address; set => SetProperty(ref address, value); }

        private string devicePhone;
        public string DevicePhone { get => devicePhone; set => SetProperty(ref devicePhone, value); }

    }

}
