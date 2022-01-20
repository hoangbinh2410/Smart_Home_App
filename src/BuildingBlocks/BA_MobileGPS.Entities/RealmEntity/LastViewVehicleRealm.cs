using Realms;

using System;

namespace BA_MobileGPS.Entities
{
    public class LastViewVehicleRealm : RealmObject, IRealmEntity
    {
        [Indexed]
        [PrimaryKey]
        public long Id { get; set; }

        public string UserId { get; set; }

        public string VehiclePlate { get; set; }

        public string PrivateCode { get; set; }

        /// <summary>
        /// Giá trị
        /// </summary>
        public DateTimeOffset? LastSynchronizationDate { get; set; }

        public DateTimeOffset? SysLastChangeDate { get; set; }

        public DateTimeOffset? SysDeletedDate { get; set; }

        public RealmInteger<int> Counter { get; set; }
    }
}