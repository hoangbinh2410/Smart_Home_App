namespace BA_MobileGPS.Entities
{
    public class AlertTypeModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public short MinutesToRepeat { get; set; }

        public string AllowRangeValue { get; set; }

        public short DayOfProcess { get; set; }

        public string IconMobile { get; set; }
    }
}