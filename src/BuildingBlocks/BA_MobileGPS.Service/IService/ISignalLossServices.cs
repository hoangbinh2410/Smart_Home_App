using BA_MobileGPS.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface ISignalLossServices
    {
        Task<IList<SignalLossResponse>> GetData(SignalLossRequest input);
    }
}