using System;

namespace BA_MobileGPS.Entities
{
    public interface IEntity
    {
        long Id { get; set; }

        DateTimeOffset? LastSynchronizationDate { get; set; }

        DateTimeOffset? SysLastChangeDate { get; set; }

        DateTimeOffset? SysDeletedDate { get; set; }
    }
}