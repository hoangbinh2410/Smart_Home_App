using Realms;

using System;

namespace BA_MobileGPS.Entities
{
    public class MobileResourceRealm : RealmObject, IRealmEntity
    {
        [Indexed]
        [PrimaryKey]
        public long Id { get; set; }

        public string CodeName { get; set; }

        public string Name { get; set; }

        public int FK_LanguageID { get; set; }

        public string Value { get; set; }

        public DateTimeOffset? LastSynchronizationDate { get; set; }
        public DateTimeOffset? SysLastChangeDate { get; set; }
        public DateTimeOffset? SysDeletedDate { get; set; }
        public RealmInteger<int> Counter { get; set; }
    }
}