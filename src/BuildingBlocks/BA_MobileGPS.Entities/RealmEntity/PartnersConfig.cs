using Realms;
using System;

namespace BA_MobileGPS.Entities.RealmEntity
{
    public class PartnersConfig : RealmObject, IRealmEntity
    {
        [Indexed]
        [PrimaryKey]
        public long Id { get; set; }

        public int FK_CompanyID { get; set; }

        public int Theme { get; set; }

        public string LoginLogo { get; set; }

        public string InAppLogo { get; set; }

        public string Hotline { get; set; }

        public string Website { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public DateTimeOffset? LastSynchronizationDate { get; set; }
        public DateTimeOffset? SysLastChangeDate { get; set; }
        public DateTimeOffset? SysDeletedDate { get; set; }
        public RealmInteger<int> Counter { get; set; }
    }
}