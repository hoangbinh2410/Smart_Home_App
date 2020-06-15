using Realms;

using System;

namespace BA_MobileGPS.Entities.RealmEntity
{
    public class DBLocalVersionRealm : RealmObject, IRealmEntity
    {
        [Indexed]
        [PrimaryKey]
        public long Id { get; set; }

        public string PK_VersionMobileID { get; set; }

        public string TableName { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        public int VersionDB { get; set; }

        // thời gian đồng bộ gần nhất
        public DateTimeOffset? LastSynchronizationDate { get; set; }

        // thời gian thay đổi gần nhất
        public DateTimeOffset? SysLastChangeDate { get; set; }

        public DateTimeOffset? SysDeletedDate { get; set; }
        public RealmInteger<int> Counter { get; set; }
    }
}