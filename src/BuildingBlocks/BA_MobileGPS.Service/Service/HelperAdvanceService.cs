using AutoMapper;

using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.Infrastructure.Repository;

namespace BA_MobileGPS.Service
{
    public class HelperAdvanceService : RealmBaseService<HelperAdvanceRealm, HelperAdvance>, IHelperAdvanceService
    {
        public HelperAdvanceService(IBaseRepository baseRepository, IMapper mapper) : base(baseRepository, mapper)
        {
        }
    }
}