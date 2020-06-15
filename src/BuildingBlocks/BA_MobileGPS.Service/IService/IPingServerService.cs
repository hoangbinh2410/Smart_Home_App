using BA_MobileGPS.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface IPingServerService
    {
        Task<BaseResponse<bool>> PingServerStatus();
    }
}