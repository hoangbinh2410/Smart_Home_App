using BA_MobileGPS.Entities;
using System;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface IPingServerService
    {
        Task<ResponseBase<bool>> PingServerStatus();

        Task<ResponseBase<DateTime>> GetTimeServer();
    }
}