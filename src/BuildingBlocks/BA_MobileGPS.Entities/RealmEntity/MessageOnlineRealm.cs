using Realms;

using System;

namespace BA_MobileGPS.Entities
{
    public class MessageOnlineRealm : RealmObject, IRealmEntity
    {
        [Indexed]
        [PrimaryKey]
        public long Id { get; set; }

        public RealmInteger<int> Counter { get; set; }

        public string Receiver { get; set; }

        public string Content { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public bool IsSend { get; set; }

        public bool IsRead { get; set; }

        public DateTimeOffset? LastSynchronizationDate { get; set; }

        public DateTimeOffset? SysLastChangeDate { get; set; }

        public DateTimeOffset? SysDeletedDate { get; set; }
    }
}