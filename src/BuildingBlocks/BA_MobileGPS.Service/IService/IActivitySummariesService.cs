using BA_MobileGPS.Entities;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface IActivitySummariesService
    {
        Task<IList<ActivitySummariesModel>> GetData(ActivitySummariesRequest input);
    }
}