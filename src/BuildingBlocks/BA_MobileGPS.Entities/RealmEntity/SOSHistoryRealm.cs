using Realms;

using System;

namespace BA_MobileGPS.Entities
{
    public class SOSHistoryRealm : RealmObject, IRealmEntity
    {
        [Indexed]
        [PrimaryKey]
        public long Id { get; set; }

        public bool IsOnSOS { get; set; }
        public string DevicePlate { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdateDate { get; set; }
        public DateTimeOffset? LastSynchronizationDate { get; set; }
        public DateTimeOffset? SysLastChangeDate { get; set; }
        public DateTimeOffset? SysDeletedDate { get; set; }
        public RealmInteger<int> Counter { get; set; }
    }
}