using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Entities.ResponeEntity.Report.Station
{
    [Serializable]
    public class StationDetailsResponse : ReportBaseResponse
    {
        //STT Dòng
        public int RowNumber { get; set; }
        public int FK_LandmarkID { get; set; }
        public DateTime TimeInStation { get; set; }
        public DateTime TimeOutStation { get; set; }
        public int PeriodInOutStation { get; set; }
        public string Name { get; set; }
        public int FK_LandmarkCatalogueID { get; set; }
        public string PrivateCode { get; set; }
        public string GroupName { get; set; }
    }
}
