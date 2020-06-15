using Realms;

using System;

namespace BA_MobileGPS.Entities
{
    public class HelperAdvanceRealm : RealmObject, IRealmEntity
    {
        [Indexed]
        [PrimaryKey]
        public long Id { get; set; }

        public int HelperAdvanceID { set; get; }

        public bool IsViewHelp { set; get; }

        public DateTimeOffset? LastSynchronizationDate { get; set; }
        public DateTimeOffset? SysLastChangeDate { get; set; }
        public DateTimeOffset? SysDeletedDate { get; set; }
        public RealmInteger<int> Counter { get; set; }
    }
}