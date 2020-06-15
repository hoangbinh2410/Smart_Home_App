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

        public double Heading1 { get; set; }

        public double Heading2 { get; set; }

        public double DeltaAngle { get; set; }

        public float? Direction { get; set; }

        private StatePoint state;
        public StatePoint State { get => state; set => SetProperty(ref state, value); }

        public int Velocity { get; set; }
    }
}