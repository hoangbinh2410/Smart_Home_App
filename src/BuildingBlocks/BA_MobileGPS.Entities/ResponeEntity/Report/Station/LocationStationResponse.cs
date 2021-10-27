using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Entities.ResponeEntity.Report.Station
{
    public class LocationStationResponse
    {
        public int PK_LandmarkID { get; set; }
        public string Name { get; set; }
        public string PrivateName { get; set; }
        public string Address { get; set; }
    }
}
