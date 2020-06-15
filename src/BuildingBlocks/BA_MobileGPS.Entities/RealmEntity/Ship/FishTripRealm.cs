using Realms;
using System;

namespace BA_MobileGPS.Entities
{
    public class FishTripRealm : RealmObject, IRealmEntity
    {
        [Indexed]
        [PrimaryKey]
        public long Id { get; set; }

        public int PK_FishTrip { get; set; }

        public string ShipPlate { get; set; }

        public string Imei { get; set; }

        public DateTimeOffset StartTime { get; set; }

        public double StartLatitude { get; set; }

        public double StartLongitude { get; set; }

        public DateTimeOffset EndTime { get; set; }

        public double EndLatitude { get; set; }

        public double EndLongitude { get; set; }

        public bool IsDeleted { get; set; }

        public RealmInteger<int> Counter { get; set; }

        public DateTimeOffset? LastSynchronizationDate { get; set; }

        public DateTimeOffset? SysLastChangeDate { get; set; }

        public DateTimeOffset? SysDeletedDate { get; set; }
    }
}