using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public class SendEngineControlService : ServiceBase<ActionOnOffMachineLogRequest, ActionOnOffMachineLogViewModel>, ISendEngineControlService
    {
        public SendEngineControlService()
        {
        }

        public override async Task<IList<ActionOnOffMachineLogViewModel>> GetData(ActionOnOffMachineLogRequest input)
        {
            var respone = new List<ActionOnOffMachineLogViewModel>();
            try
            {
                //string url = $"{ApiUri.GET_LIST_ENGINE}?FK_UserID={input.FK_UserID}&VehiclePlate={input.VehiclePlate}&PageIndex={input.PageIndex}&PageSize={input.PageSize}&StartDate={JsonConvert.SerializeObject(input.StartDate).Replace("\"", string.Empty)}&EndDate={JsonConvert.SerializeObject(input.EndDate).Replace("\"", string.Empty)}";
                var temp = await RequestProvider.PostAsync<ActionOnOffMachineLogRequest,ResponseBase<List<ActionOnOffMachineLogViewModel>>>(ApiUri.GET_LIST_ENGINE, input);
                if (temp != null && temp.Data != null)
                {
                    respone = temp.Data;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
            return respone;
        }

        public async Task<SendEngineRespone> SendEngineControl(SendEngineControlRequest input)
        {
            var respone = new SendEngineRespone();
            try
            {
                var temp = await RequestProvider.PostAsync<SendEngineControlRequest, ResponseBase<SendEngineRespone>>(ApiUri.GET_SEND_ENGINE_CONTROL, input);
                if (temp != null && temp.Data != null)
                {
                    respone = temp.Data;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return respone;
        }
    }
}