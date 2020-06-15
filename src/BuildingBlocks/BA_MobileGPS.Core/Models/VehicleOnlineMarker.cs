﻿using BA_MobileGPS.Entities;

using System;

namespace BA_MobileGPS
{
    public class VehicleOnlineMarker : BaseModel
    {
        public long VehicleId { set; get; }

        public string VehiclePlate { set; get; }

        public float Lat { set; get; }

        public float Lng { set; get; }

        public int State { set; get; }

        public int Velocity { set; get; }

        public DateTime GPSTime { get; set; }

        public DateTime VehicleTime { get; set; }

        public IconCode IconCode { get; set; }

        public string PrivateCode { set; get; }

        public string IconImage { set; get; }

        public DoubleMarker DoubleMarker { get; set; }
    }
}