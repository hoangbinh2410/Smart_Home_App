namespace BA_MobileGPS.Entities
{
    public class AlertCompanyConfigRespone : BaseModel
    {
        public int FK_AlertTypeID { get; set; }

        public int MinutesToRepeat { get; set; }

        public string Value { get; set; }

        public string Name { get; set; }

        private bool isVisible;
        public bool IsVisible { get => isVisible; set => SetProperty(ref isVisible, value); }
    }

    public class AlertVehicleConfigRespone : BaseModel
    {
        public long VehicleId { set; get; }

        public string VehiclePlate { set; get; }

        public string PrivateCode { set; get; }

        public string GroupIDs { set; get; }

        public string Imei { set; get; }

        private bool isVisible;
        public bool IsVisible { get => isVisible; set => SetProperty(ref isVisible, value); }
    }

    public class AlertTimeConfigRespone : BaseModel
    {
        public int TimeId { get; set; }

        public string TimeName { get; set; }

        public string TimeNameBox { get; set; }

        private bool isVisible;
        public bool IsVisible { get => isVisible; set => SetProperty(ref isVisible, value); }
    }
}