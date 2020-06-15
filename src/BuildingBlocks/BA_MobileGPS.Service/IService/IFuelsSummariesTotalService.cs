using BA_MobileGPS.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface IFuelsSummariesTotalService
    {
        Task<IList<FuelsSummariesTotalResponse>> GetData(FuelsSummariesTotalRequest input);
    }
}