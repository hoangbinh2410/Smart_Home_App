using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.Infrastructure.Repository;
using BA_MobileGPS.Service.Utilities;

namespace BA_MobileGPS.Service
{
    public class SOSHistoryService : RealmBaseService<SOSHistoryRealm, SOSHistory>, ISOSHistoryService
    {
        private readonly IRequestProvider requestProvider;

        public SOSHistoryService(IRequestProvider requestProvider, IBaseRepository baseRepository, IMapper mapper) : base(baseRepository, mapper)
        {
            this.requestProvider = requestProvider;
        }
    }
}