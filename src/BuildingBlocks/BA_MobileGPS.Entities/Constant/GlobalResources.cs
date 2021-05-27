namespace BA_MobileGPS.Entities
{
    public sealed class GlobalResources : BaseModel
    {
        private static readonly GlobalResources _current = new GlobalResources();
        public static GlobalResources Current => _current;

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static GlobalResources()
        {
        }

        private GlobalResources()
        {
        }

        private int totalAlert;
        public int TotalAlert { get => totalAlert; set => SetProperty(ref totalAlert, value, relatedProperty: nameof(TotalAlertDisplay)); }

        public string TotalAlertDisplay => TotalAlert > 100 ? "99+" : TotalAlert.ToString();

        private int totalVideoUpload;

        public int TotalVideoUpload
        {
            get => totalVideoUpload; set => SetProperty(ref totalVideoUpload, value);
        }

        private int totalVideoUploaded;

        public int TotalVideoUploaded
        {
            get => totalVideoUploaded; set => SetProperty(ref totalVideoUploaded, value);
        }
    }
}