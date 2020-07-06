namespace BA_MobileGPS.Entities
{
    public class UserLandmarkGroupRespone : BaseModel
    {
        public int PK_LandmarksGroupID { get; set; }

        public string Name { get; set; }

        public bool IsSystem { get; set; }

        private bool isVisible;
        public bool IsVisible { get => isVisible; set => SetProperty(ref isVisible, value); }

        private bool isDisplayBound;
        public bool IsDisplayBound { get => isDisplayBound; set => SetProperty(ref isDisplayBound, value); }

        private bool isDisplayName;
        public bool IsDisplayName { get => isDisplayName; set => SetProperty(ref isDisplayName, value); }

        public bool IsChanged { get; set; }
    }

    public class UserLandmarkRespone
    {
        public int PK_LandmarkID { get; set; }

        public int FK_LandmarksGroupID { get; set; }

        public string Name { get; set; }

        public float Latitude { get; set; }

        public float Longitude { get; set; }

        public int Color { get; set; }

        public string Polygon { get; set; }

        public string IconApp { get; set; }

        public bool IsClosed { get; set; }
    }
}