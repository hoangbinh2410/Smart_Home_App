using BA_MobileGPS.Entities;

namespace MOTO_MobileGPS.Constant
{
    public sealed class GlobalResourcesMoto : BaseModel
    {
        private static readonly GlobalResourcesMoto _current = new GlobalResourcesMoto();
        public static GlobalResourcesMoto Current => _current;

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static GlobalResourcesMoto()
        {
        }

        private GlobalResourcesMoto()
        {
        }

        private MotoDetailViewModel motoDetail = new MotoDetailViewModel();

        public MotoDetailViewModel MotoDetail
        {
            get => motoDetail; set => SetProperty(ref motoDetail, value);
        }
    }
}
