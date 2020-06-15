using BA_MobileGPS.Entities;

namespace BA_MobileGPS.Core
{
    public class PositionDergee : BaseModel
    {
        private int dergee;
        public int Dergee { get => dergee; set => SetProperty(ref dergee, value); }

        private int min;
        public int Min { get => min; set => SetProperty(ref min, value); }

        private double sec;
        public double Sec { get => sec; set => SetProperty(ref sec, value); }
    }
}