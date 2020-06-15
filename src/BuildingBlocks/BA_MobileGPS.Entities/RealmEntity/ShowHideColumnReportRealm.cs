using Realms;

using System;

namespace BA_MobileGPS.Entities
{
    public class ShowHideColumnReportRealm : RealmObject, IRealmEntity
    {
        [Indexed]
        [PrimaryKey]
        public long Id { get; set; }

        public int IDTable { get; set; }

        public int IDColumn { get; set; }

        public bool Value { get; set; }  // true: show || false : Hide

        /// <summary>
        /// Giá trị
        /// </summary>

        public DateTimeOffset? LastSynchronizationDate { get; set; }
        public DateTimeOffset? SysLastChangeDate { get; set; }
        public DateTimeOffset? SysDeletedDate { get; set; }
        public RealmInteger<int> Counter { get; set; }
    }
}