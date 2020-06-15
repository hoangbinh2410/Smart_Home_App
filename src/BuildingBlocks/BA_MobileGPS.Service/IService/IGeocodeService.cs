﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface IGeocodeService
    {
        Task<string> GetAddressByLatLng(string lat, string lng);

        Task<List<string>> GetAddressesByLatLng(string lats, string lngs);
    }
}