using System;

namespace BA_MobileGPS.Entities
{
    public class UpdateIsReadRequest
    {
        public Guid UserID { get; set; }

        public int NoticeId { get; set; }
    }
}