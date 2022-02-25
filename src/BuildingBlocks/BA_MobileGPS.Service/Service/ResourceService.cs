using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.Infrastructure.Repository;
using BA_MobileGPS.Service.Utilities;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public class ResourceService : RealmBaseService<MobileResourceRealm, MobileResourceRespone>, IResourceService
    {
        private readonly IRequestProvider RequestProvider;

        public ResourceService(IRequestProvider requestProvider, IBaseRepository baseRepository, IMapper mapper) : base(baseRepository, mapper)
        {
            this.RequestProvider = requestProvider;
        }

        public async Task<List<MobileResourceRespone>> GetAllResources(int appID, string Culture, long? LastTime)
        {
            List<MobileResourceRespone> result = new List<MobileResourceRespone>();
            try
            {
                var url = $"{ApiUri.GET_MOBILERESOURCE}?appID={appID}&culture={Culture}&LastUpdate={LastTime}";
                var data = await RequestProvider.GetAsync<ResponseBase<List<MobileResourceRespone>>>(url);

                if (data != null&& data.Data.Count>0)
                {
                    result = data.Data;
                }
            }
            catch (Exception e)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, e);
            }
            return result;
        }
    }
}