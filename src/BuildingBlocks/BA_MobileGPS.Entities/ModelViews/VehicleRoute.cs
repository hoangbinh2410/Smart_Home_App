using System;

namespace BA_MobileGPS.Entities
{
    public class VehicleRoute : BaseModel
    {
        public int Index { get; set; }

        public DateTime Time { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        private string address;
        public string Address { get => address; set => SetProperty(ref address, value); }

        private StatePoint statetype;
        public StatePoint StateType { get => statetype; set => SetProperty(ref statetype, value); }

        public int Velocity { get; set; }

        public int State { get; set; }

        public float Km { get; set; }
    }
}