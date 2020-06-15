using Realms;

using System;

namespace BA_MobileGPS.Entities
{
    public class LanguageRealm : RealmObject, IRealmEntity
    {
        [Indexed]
        [PrimaryKey]
        public long Id { get; set; }

        public int PK_LanguageID { set; get; }

        public string CodeName { set; get; }

        public string Icon { set; get; }

        public string Description { set; get; }

        public DateTimeOffset? LastSynchronizationDate { get; set; }
        public DateTimeOffset? SysLastChangeDate { get; set; }
        public DateTimeOffset? SysDeletedDate { get; set; }
        public RealmInteger<int> Counter { get; set; }
    }
}