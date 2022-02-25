using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.RequestEntity.Report.TransportBusiness;
using BA_MobileGPS.Entities.ResponeEntity.Report.TransportBusiness;
using BA_MobileGPS.Service.Report.TransportBusiness;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service.Service.Report.TransportBusiness
{
    public class TransportBusinessService : ServiceBase<TransportBusinessRequest, TransportBusinessResponse>, ITransportBusinessService
    {
        public TransportBusinessService()
        {
        }
        public async override Task<IList<TransportBusinessResponse>> GetData(TransportBusinessRequest input)
        {
            var respone = new List<TransportBusinessResponse>();
            try
            {
                var temp = await RequestProvider.PostAsync<TransportBusinessRequest, ResponseBase<List<TransportBusinessResponse>>>(ApiUri.GET_GetTransportBusiness, input);
                if (temp != null && temp.Data.Count > 0)
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
