using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Entities.RequestEntity
{
    public class VerifyOtpRequest
    {
        public string SecurityCode { get; set; }
        public long UserSecuritySMSLogID { get; set; }
        public string UserName { get; set; }
        public int XNcode { get; set; }
        public string PhoneNumber { get; set; }
        public List<string> VehiclePlate{get; set;}
    }   
}