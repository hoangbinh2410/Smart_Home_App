namespace BA_MobileGPS.Entities
{
    public class ServicePackHistory
    {
        public ServicePackHistory()
        {
            Dvtninfo = new PackageInfos();
            Dvvtinfo = new PackageInfos();
        }

        public int Monthindex { get; set; }

        public int Month { get; set; }

        public PackageInfos Dvtninfo { get; set; }

        public PackageInfos Dvvtinfo { get; set; }
    }

    public class PackageInfos
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int BlockSMS { get; set; }
    }
}