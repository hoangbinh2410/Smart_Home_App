namespace BA_MobileGPS.Entities
{
    public class VehicleGroupModel : BaseModel
    {
        public int PK_VehicleGroupID { get; set; }

        public string Name { get; set; }

        public int NumberOfVehicle { get; set; }

        public int ParentVehicleGroupID { get; set; }

        private bool isSelected = false;
        public bool IsSelected { get => isSelected; set => SetProperty(ref isSelected, value); }
    }
}