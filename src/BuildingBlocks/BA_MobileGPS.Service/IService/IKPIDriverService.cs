using BA_MobileGPS.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface IKPIDriverService
    {
        Task<List<DriverRankingRespone>> GetDriverRankingByDay(DriverRankingRequest request);

        Task<List<DriverRankingRespone>> GetDriverRankingByMonth(DriverRankingRequest request);

        Task<DriverKpiChartRespone> GetDriverKpiChart(DriverKpiChartRequest request);
    }
}