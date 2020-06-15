using BA_MobileGPS.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface IActionOnOffMachineLogService
    {
        Task<IList<ActionOnOffMachineLogViewModel>> GetData(ActionOnOffMachineLogRequest input);
    }
}