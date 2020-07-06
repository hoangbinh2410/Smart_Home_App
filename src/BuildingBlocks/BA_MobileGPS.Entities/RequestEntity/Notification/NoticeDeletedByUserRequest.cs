using System;
using System.Collections.Generic;

namespace BA_MobileGPS.Entities
{
    public class NoticeDeletedByUserRequest
    {
        public int FK_NoticeContentID { get; set; }

        public Guid FK_UserID { get; set; }
    }

    public class NoticeDeletedRangeByUserRequest
    {
        public List<int> FK_NoticeContentID { get; set; }

        public Guid FK_UserID { get; set; }
    }
}