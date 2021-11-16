using BA_MobileGPS.Entities.RequestEntity.Report.Station;
using BA_MobileGPS.Entities.ResponeEntity.Report.Station;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service.Report.Station
{
    public interface IStationDetailsService
    {
        Task<IList<StationDetailsResponse>> GetData(StationDetailsRequest input);
    }
}
