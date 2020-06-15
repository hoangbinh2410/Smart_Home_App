namespace BA_MobileGPS.Entities
{
    public class LandmarkResponse : BaseModel
    {
        public long Id { get; set; }

        public int PK_LandmarkID { get; set; }

        public string Name { get; set; }

        public string Polygon { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public int Color { get; set; }

        public bool IsClosed { get; set; }

        private bool isShowBoudary;
        public bool IsShowBoudary { get => isShowBoudary; set => SetProperty(ref isShowBoudary, value); }

        private bool isShowName;
        public bool IsShowName { get => isShowName; set => SetProperty(ref isShowName, value); }
    }
}