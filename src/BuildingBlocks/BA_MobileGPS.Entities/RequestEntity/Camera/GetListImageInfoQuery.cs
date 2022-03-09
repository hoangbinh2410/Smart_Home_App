using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Entities.RequestEntity
{
    public class GetListImageInfoQuery
    {
        public int XNcode { get; set; }

        public string VehiclePlate { get; set; }

        public int? Limit { get; set; }

        public DateTime? FromTime { get; set; }

        public DateTime? ToTime { get; set; }
    }
}
