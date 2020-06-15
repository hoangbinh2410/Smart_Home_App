using BA_MobileGPS.Entities;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface ISpeedOversService
    {
        Task<IList<SpeedOversModel>> GetData(SpeedOversRequest input);
    }
}