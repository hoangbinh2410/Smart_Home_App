using Prism.Events;
using Shiny;
using System;
using System.Collections.Generic;
using System.Text;

namespace VMS_MobileGPS.Events
{
    public class RecieveLocationEvent : PubSubEvent<RecieveLocation>
    {
    }

    public class RecieveLocation
    {
        public DateTime GPSTime { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }

        public int Velocity { get; set; }
    }
}
