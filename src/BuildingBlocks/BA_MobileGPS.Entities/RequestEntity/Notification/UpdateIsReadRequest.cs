using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Entities
{
    public class UpdateIsReadRequest
    {
        public Guid userId { get; set; }

        public int fk_NoticeContentID { get; set; }
    }
}
