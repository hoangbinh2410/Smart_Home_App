using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.Infrastructure.Repository;
using BA_MobileGPS.Service.Utilities;

namespace BA_MobileGPS.Service
{
    public class ShowHideColumnService : RealmBaseService<ShowHideColumnReportRealm, ShowHideColumnResponse>, IShowHideColumnService
    {
        private readonly IRequestProvider requestProvider;

        public ShowHideColumnService(IRequestProvider requestProvider, IBaseRepository baseRepository, IMapper mapper) : base(baseRepository, mapper)
        {
            this.requestProvider = requestProvider;
        }
    }
}