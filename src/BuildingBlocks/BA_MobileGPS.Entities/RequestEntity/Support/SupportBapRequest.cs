using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Entities.RequestEntity.Support
{
    public class SupportBapRequest
    {
        public int xncode { get; set; }
        public ContactInfo contactinfo { get; set; }
        public List<Vehiclelist> vehiclelist { get; set; }
        public string description { get; set; }
    }

    public class Errorlist
    {
        public string code { get; set; }
    }

    public class ContactInfo
    {
        public string username { get; set; }
        public string fullname { get; set; }
        public string mobilestr { get; set; }
    }

    public class Vehiclelist
    {
        public string platestr { get; set; }
        public List<Errorlist> errorlist { get; set; }
        public string description { get; set; }
    }

}
