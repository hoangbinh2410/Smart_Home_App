using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public class ServiceBase<TInputModel, TEntity>
        where TInputModel : class, new()
        where TEntity : class, new()
    {
        private IRequestProvider requestProvider = null;

        protected IRequestProvider RequestProvider
        {
            get
            {
                if (requestProvider == null)
                {
                    requestProvider = new RequestProvider();
                }
                return requestProvider;
            }
        }

        public ServiceBase()
        {
        }

        public virtual Task<IList<TEntity>> GetData(TInputModel input)
        {
            return null;
        }

        public async Task<IList<string>> GetAddressReport(string input)
        {
            var respone = new List<string>();
            try
            {
                string url = $"{ApiUri.GET_REPORTADDRESS}?Coordinates={input}";
                var temp = await RequestProvider.GetAsync<AddressReportResponse>(url);
                if (temp != null)
                {
                    if (temp.State)
                    {
                        respone = temp.ListAddress;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return respone;
        }

        public async Task<ValidatedReportRespone> ValidateDateTimeReport(Guid UserId,
          DateTime FromDate, DateTime ToDate)
        {
            ValidatedReportRespone respone = new ValidatedReportRespone();
            respone.State = StateValidateReport.None;

            try
            {
                string url = $"{ApiUri.GET_VALIDATEDATETIME}?UserId={UserId}&FromDate={FromDate.ToString("yyyy-MM-dd HH:mm:ss")}&ToDate={ToDate.ToString("yyyy-MM-dd HH:mm:ss")}";

                var result = await RequestProvider.GetAsync<ResponseBaseV2<ValidatedReportRespone>>(url);
                if (result != null && result.Data != null)
                {
                    respone = result.Data;
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