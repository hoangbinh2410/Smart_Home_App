using System;

namespace BA_MobileGPS.Entities
{
    public class InsertFeedbackNoticeRequest
    {
        public int fk_CompanyID { get; set; }

        public int fk_NoticeID { get; set; }

        public string culture { get; set; }

        public string body { get; set; }

        public Guid userId { get; set; }
    }
}