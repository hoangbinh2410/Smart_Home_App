namespace BA_MobileGPS.Entities.ModelViews
{
    public class VehicleStatusViewModel : BaseModel
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Icon { get; set; }

        public bool IsEnable { get; set; }

        private int countCar;
        public int CountCar { get => countCar; set => SetProperty(ref countCar, value); }
    }
}