using System;
using System.Collections.Generic;

namespace BA_MobileGPS.Entities
{
    public class NotificationRespone : BaseModel
    {
        public int PK_NoticeContentID { get; set; }

        /// <summary>
        /// Tên loại thông báo
        /// </summary>
        public string NoticeTypeName { get; set; }

        /// <summary>
        /// Link Icon cho App
        /// </summary>
        public string LinkIconApp { get; set; }

        /// <summary>
        /// Tiêu đề thông báo
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Nội dung tóm tắt thông báo
        /// </summary>
        public string SubBody { get; set; }

        /// <summary>
        /// Nội dung thông báo
        /// </summary>
        public string Body { get; set; }

        private bool isRead;
        public bool IsRead { get => isRead; set => SetProperty(ref isRead, value); }

        /// <summary>
        /// Link kích vào thì hiển thị sang trang mong muốn
        /// </summary>
        public string Linkview { get; set; }

        /// <summary>
        /// Tên đường link file đính kèm
        /// </summary>
        public List<string> ListFileAttachs { get; set; }

        /// <summary>
        /// Ngày tạo thông báo
        /// </summary>
        public DateTime CreatedDate { get; set; }
    }

    public class NotificationBody
    {
        public string Body { get; set; }
    }

    public class NotificationWhenLoginRespone
    {
        public int PK_NoticeContentID { get; set; }

        public string Title { get; set; }

        public bool IsAlwayShow { get; set; }
    }

    public class NotificationAfterLoginRespone
    {
        public int PK_NoticeContentID { get; set; }

        public string Title { get; set; }

        public bool IsAlwayShow { get; set; }

        public bool IsFeedback { get; set; }
    }
}