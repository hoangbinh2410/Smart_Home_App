using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.RequestEntity;
using BA_MobileGPS.Entities.RequestEntity.Support;
using BA_MobileGPS.Entities.ResponeEntity;
using BA_MobileGPS.Service.IService;
using BA_MobileGPS.Utilities.Constant;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service.Service
{
    public class VerifysmsService : IVerifysmsService
    {
        private readonly IRequestProvider _iRequestProvider;
        public VerifysmsService(IRequestProvider iRequestProvider)
        {
            _iRequestProvider = iRequestProvider;
        }
        public async Task<VehiclePhoneRespone> CheckHasOtpsms(VehiclePhoneRequest request)
        {
            VehiclePhoneRespone result = new VehiclePhoneRespone();
            try
            {
                var respone = await _iRequestProvider.PostAsync<VehiclePhoneRequest, BaseResponse<VehiclePhoneRespone>>(ApiUri.GET_Has_OTP_SMS, request);
                if (respone != null && respone.Data != null)
                {
                    result = respone.Data;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(MethodBase.GetCurrentMethod().Name, e);
            }
            return result;
        }
    }
}
