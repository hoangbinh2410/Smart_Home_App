using System;

namespace BA_MobileGPS.Entities
{
    public class PickerDateTimeResponse
    {
        public DateTime Value { get; set; }

        public short PickerType { get; set; }

        public string Time { get; set; }
    }

    public class PickerRangeDateTimeResponse
    {
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

    }

    public class PickerDateResponse
    {
        public DateTime Value { get; set; }

        public short PickerType { get; set; }
    }
}