using BA_MobileGPS.Service.IService;
using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Service.Service
{
   public class GetStatusService : IGetStatusService
    {
        private readonly IRequestProvider _requestProvider;
        public GetStatusService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }
    }
}
