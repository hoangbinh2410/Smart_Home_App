using BA_MobileGPS.Core.Extensions;
using System;

namespace BA_MobileGPS.Entities
{
    public class VehicleOnlineViewModel : BaseModel
    {
        public long VehicleId { set; get; }

        public string VehiclePlate { set; get; }

        public string PrivateCode { set; get; }

        private float lat;
        public float Lat { get => lat; set => SetProperty(ref lat, value); }

        private float lng;
        public float Lng { get => lng; set => SetProperty(ref lng, value); }

        private int state;
        public int State { get => state; set => SetProperty(ref state, value); }

        private string iconImage;
        public string IconImage { get => iconImage; set => SetProperty(ref iconImage, value); }

        private IconCode iconCode;
        public IconCode IconCode { get => iconCode; set => SetProperty(ref iconCode, value); }

        private int velocity;
        public int Velocity { get => velocity; set => SetProperty(ref velocity, value); }

        private DateTime _GPSTime;
        public DateTime GPSTime { get => _GPSTime; set => SetProperty(ref _GPSTime, value); }

        private DateTime vehicleTime;
        public DateTime VehicleTime { get => vehicleTime; set => SetProperty(ref vehicleTime, value, relatedProperty: nameof(LostGSMTime)); }

        public string GroupIDs { set; get; }

        // Trạng thái máy
        private string statusEngineer;

        public string StatusEngineer { get => statusEngineer; set => SetProperty(ref statusEngineer, value); }

        private double totalKm;
        public double TotalKm { get => totalKm; set => SetProperty(ref totalKm, value); }

        private byte messageId;
        public byte MessageId { get => messageId; set => SetProperty(ref messageId, value, nameof(IsShowDetail)); }

        public byte KindID { set; get; }

        public string Imei { set; get; }

        private DateTime daturityDate;
        public DateTime MaturityDate { get => daturityDate; set => SetProperty(ref daturityDate, value); }

        private string messageBAP;
        public string MessageBAP { get => messageBAP; set => SetProperty(ref messageBAP, value); }

        private string messageDetailBAP;
        public string MessageDetailBAP { get => messageDetailBAP; set => SetProperty(ref messageDetailBAP, value); }

        public int SortOrder { set; get; }

        public bool IsEnableAcc { set; get; }

        public bool IsQcvn31 { set; get; }

        public bool HasImage { set; get; }

        public bool HasVideo { set; get; }

        private string currentAddress;
        public string CurrentAddress { get => currentAddress; set => SetProperty(ref currentAddress, value); }

        public int DataExt { set; get; }

        public int LostGSMTime => (int)StaticSettings.TimeServer.Subtract(VehicleTime).TotalSeconds;

        public int stopTime;
        public int StopTime { get => stopTime; set => SetProperty(ref stopTime, value, nameof(StopTimeView)); }

        public string temperature;
        public string Temperature { get => temperature; set => SetProperty(ref temperature, value); }

        private bool isFavorite;
        public bool IsFavorite { get => isFavorite; set => SetProperty(ref isFavorite, value); }

        public int StopTimeView
        {
            get
            {
                if (StateVehicleExtension.IsStoping(Velocity) && StopTime > 0)
                {
                    return StopTime;
                }
                else
                {
                    return 0;
                }
            }
        }

        public bool IsShowDetail
        {
            get
            {
                return (MessageId == 2 || MessageId == 3 || MessageId == 128) ? false : true;
            }
        }
    }
}