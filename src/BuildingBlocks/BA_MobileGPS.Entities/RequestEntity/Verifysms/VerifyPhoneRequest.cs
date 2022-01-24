using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Entities.RequestEntity
{
   public class VerifyPhoneRequest
    {
        public Guid UserID { get; set; }
        public int CompanyID { get; set; }
        public string PhoneNumber { get; set; }
    }
}
