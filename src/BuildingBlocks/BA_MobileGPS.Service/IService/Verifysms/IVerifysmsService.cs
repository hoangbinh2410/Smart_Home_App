using BA_MobileGPS.Entities.RequestEntity;
using BA_MobileGPS.Entities.ResponeEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service.IService
{
    public interface IVerifysmsService
    {
        Task<VehiclePhoneRespone> CheckHasOtpsms(VehiclePhoneRequest request);
    }
}
