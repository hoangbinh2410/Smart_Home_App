using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;

using System;
using System.Reflection;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public class MotoDetailService : IMotoDetailService
    {
        private readonly IRequestProvider _IRequestProvider;

        public MotoDetailService(IRequestProvider IRequestProvider)
        {
            this._IRequestProvider = IRequestProvider;
        }
      
    }
}