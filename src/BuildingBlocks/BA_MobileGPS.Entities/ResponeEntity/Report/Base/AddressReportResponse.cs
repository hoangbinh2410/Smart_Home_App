using BA_MobileGPS.Utilities.Enums;

using System.Collections.Generic;

namespace BA_MobileGPS.Entities
{
    public class AddressReportResponse
    {
        public bool State { get; set; }
        public ResponseEnum ErrorCode { get; set; }

        public List<string> ListAddress = new List<string>();
    }
}