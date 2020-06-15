using BA_MobileGPS.Entities;

using System.Collections.Generic;

namespace BA_MobileGPS.Service
{
    public interface IFuelChartService : IReportBaseService<FuelChartRequest, List<FuelChartReport>>
    {
    }
}