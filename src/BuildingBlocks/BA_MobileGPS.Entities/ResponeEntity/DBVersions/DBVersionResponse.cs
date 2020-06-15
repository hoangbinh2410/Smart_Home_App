using System;

namespace BA_MobileGPS.Entities
{
    /// <summary>
    /// Model version db
    /// </summary>
    /// <Modified>
    /// Name     Date         Comments
    /// Hoangdt  11/03/2019   created
    /// </Modified>
    public class DatabaseVersionsResponse : CommonResponseRealBase
    {
        public string CreatedDate { get; set; }

        public string PK_VersionMobileID { get; set; }

        public string TableName { get; set; }

        public DateTime UpdatedDate { get; set; }

        public int VersionDB { get; set; }
    }
}