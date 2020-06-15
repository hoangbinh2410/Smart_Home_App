using Realms;

using System;

namespace BA_MobileGPS.Entities
{
    public class FishTripQuantityRealm : RealmObject, IRealmEntity
    {
        [Indexed]
        [PrimaryKey]
        public long Id { get; set; }

        public long Parent { get; set; }

        public int FK_FishTrip { get; set; }

        public int FK_FishID { get; set; }

        public string FishName { get; set; }

        public double Weight { get; set; }

        public bool IsDeleted { get; set; }

        public RealmInteger<int> Counter { get; set; }

        public DateTimeOffset? LastSynchronizationDate { get; set; }

        public DateTimeOffset? SysLastChangeDate { get; set; }

        public DateTimeOffset? SysDeletedDate { get; set; }
    }
}