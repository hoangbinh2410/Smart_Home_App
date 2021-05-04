using Realms;

using System;

namespace BA_MobileGPS.Entities.RealmEntity
{
    public class BoundaryRealm : RealmObject, IRealmEntity
    {
        [Indexed]
        [PrimaryKey]
        public long Id { get; set; }

        public RealmInteger<int> Counter { get; set; }

        public int PK_LandmarkID { get; set; }

        public string Name { get; set; }

        public string Polygon { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public int Color { get; set; }

        public bool IsClosed { get; set; }

        public bool IsShowBoudary { get; set; }

        public bool IsShowName { get; set; }

        public bool IsEnableBoudary { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public DateTimeOffset? LastSynchronizationDate { get; set; }

        public DateTimeOffset? SysLastChangeDate { get; set; }

        public DateTimeOffset? SysDeletedDate { get; set; }
    }
}