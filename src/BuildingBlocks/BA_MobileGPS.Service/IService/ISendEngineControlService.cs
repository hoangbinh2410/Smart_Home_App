using BA_MobileGPS.Entities;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface ISendEngineControlService
    {
        Task<IList<ActionOnOffMachineLogViewModel>> GetData(ActionOnOffMachineLogRequest input);

        Task<SendEngineRespone> SendEngineControl(SendEngineControlRequest input);
    }
}