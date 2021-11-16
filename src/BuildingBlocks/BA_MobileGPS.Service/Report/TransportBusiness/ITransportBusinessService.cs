using BA_MobileGPS.Entities.RequestEntity.Report.TransportBusiness;
using BA_MobileGPS.Entities.ResponeEntity.Report.TransportBusiness;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service.Report.TransportBusiness
{
    public interface ITransportBusinessService
    {
        Task<IList<TransportBusinessResponse>> GetData(TransportBusinessRequest input);
    }
}
