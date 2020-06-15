using BA_MobileGPS.Utilities;

using System;

namespace BA_MobileGPS.Entities
{
    public class SOSHistory : BaseModel
    {
        public long Id { get; set; }
        public bool IsOnSOS { get; set; }

        private DateTimeOffset createdDate;
        public DateTimeOffset CreatedDate { get => createdDate; set => SetProperty(ref createdDate, value); }

        public DateTimeOffset updateDate;

        public DateTimeOffset UpdateDate { get => updateDate; set => SetProperty(ref updateDate, value); }

        private string devicePlate;
        public string DevicePlate { get => devicePlate; set => SetProperty(ref devicePlate, value); }

        public string TimeSOS
        {
            get
            {
                if (UpdateDate >= CreatedDate)
                {
                    return CreatedDate.FormatDateTime() + " - " + UpdateDate.FormatDateTime();
                }
                else
                {
                    return CreatedDate.FormatDateTime();
                }
            }
        }

        public string StatusName
        {
            get
            {
                if (IsOnSOS)
                {
                    return "Đang cảnh báo";
                }
                else
                {
                    return "Đã cảnh báo";
                }
            }
        }
    }
}