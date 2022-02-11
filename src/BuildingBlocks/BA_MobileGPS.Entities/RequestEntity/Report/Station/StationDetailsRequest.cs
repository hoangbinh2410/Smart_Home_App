using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Entities.RequestEntity.Report.Station
{
    public class StationDetailsRequest : ReportBaseModel
    {        
        public int LandmarkId { get; set; }
        public int NumberOfMinute { get; set; }

    }
}
