using BA_MobileGPS.Entities.ResponeEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service.IService
{
   public interface IGetStatusService
    {
        Task<StastusSmartHomeResponse> Getall() ;
    }
}
