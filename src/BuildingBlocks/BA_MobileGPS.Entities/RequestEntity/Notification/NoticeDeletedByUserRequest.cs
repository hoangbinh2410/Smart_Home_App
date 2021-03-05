using System;
using System.Collections.Generic;

namespace BA_MobileGPS.Entities
{
    public class NoticeDeletedByUserRequest
    {
        public int NoticeId { get; set; }

        public Guid UserID { get; set; }
    }

    public class NoticeDeletedRangeByUserRequest
    {
        public List<int> NoticeIds { get; set; }

        public Guid UserID { get; set; }
    }
}