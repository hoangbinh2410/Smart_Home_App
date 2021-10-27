using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Entities.RequestEntity.Support
{
    public class SupportBapRequest
    {
        public int Xncode { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public string Mobilestr { get; set; }
        public List<Vehiclelist> Vehiclelist { get; set; }
        public string Description { get; set; }
    }
    public class Errorlist
    {
        public string Code { get; set; }
    }

    public class Vehiclelist
    {
        public string Platestr { get; set; }
        public List<Errorlist> Errorlist { get; set; }
        public string Description { get; set; }
    }
}
