using System;
using System.Collections.Generic;

namespace BA_MobileGPS.Entities
{
    public class AlertOnlineViewModel
    {
        public List<AlertOnlineDetailModel> Alerts { get; set; }

        public int TotalRow { get; set; }
    }

    public class AlertOnlineDetailModel : BaseModel
    {
        public int RowNumber { set; get; }

        public long PK_AlertDetailID { get; set; }

        public string IconMobile { get; set; }

        public long FK_VehicleID { get; set; }

        public string VehiclePlate { set; get; }

        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public short FK_AlertTypeID { get; set; }

        public string AlertName { set; get; }

        public int? FK_CompanyID { get; set; }

        public float? StartLongitude { get; set; }

        public float? StartLatitude { get; set; }

        public float? EndLatitude { get; set; }

        public float? EndLongitude { get; set; }

        public string Content { get; set; }

        public int Flags { get; set; }

        public bool IsRead { get; set; }

        public bool IsProcessed { get; set; }

        public string AlertContent { get; set; }

        public string ProccessContent { get; set; }

        public int ColorAlert { set; get; }
    }
}