namespace BA_MobileGPS.Entities
{
    public class RegisterConsultRequest
    {
        public string Fullname { set; get; }

        public string PhoneNumber { set; get; }

        public int FK_ProvinceID { set; get; }

        public int FK_TransportTypeID { set; get; }

        public string ContentVdvisory { set; get; }

        public byte SourceRegistry { set; get; }
    }
}