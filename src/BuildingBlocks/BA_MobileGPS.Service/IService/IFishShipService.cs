using BA_MobileGPS.Entities;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface IFishShipService
    {
        List<FishTrip> GetListFishTrip();

        List<FishTrip> GetListFishTrip(Expression<Func<FishTripRealm, bool>> predicate);

        List<FishTripQuantity> GetFishTripDetail(long fishTripID);

        bool SaveFishTrip(FishTrip fishTrip, IList<FishTripQuantity> fishTripDetail);

        bool UpdateFishTrip(FishTrip fishTrip, IList<FishTripQuantity> fishTripDetail);

        bool DeleteFishTrip(FishTrip fishTrip);

        Task SyncFishTrip(FishTrip fishTrip, IList<FishTripQuantity> fishTripDetail);

        Task<List<Fish>> GetListFish();

        Task<int> SendFishStrip(SendFishTripRequest request);
    }
}