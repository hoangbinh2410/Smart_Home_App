using BA_MobileGPS.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface IFuelsSummariesService
    {
        Task<IList<FuelsSummariesModel>> GetData(FuelsSummariesRequest input);
    }
}