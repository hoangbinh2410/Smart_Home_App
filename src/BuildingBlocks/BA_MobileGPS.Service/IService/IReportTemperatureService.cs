using BA_MobileGPS.Entities;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface IReportTemperatureService
    {
        Task<IList<TemperatureVehicleResponse>> GetData(TemperartureVehicleRequest input);

        Task<IList<string>> GetAddressReport(string input);
    }
}