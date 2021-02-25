using System.Collections.Generic;

namespace BA_MobileGPS.Entities
{
    public class NoticeDetailRespone
    {
        public int Id { get; set; }

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

        /// <summary>
        /// Link kích vào thì hiển thị sang trang mong muốn
        /// </summary>
        public string Linkview { get; set; }

        public bool IsAlwayShow { get; set; }

        public bool IsFeedback { get; set; }

        /// <summary>
        /// Tên đường link file đính kèm
        /// </summary>
        public List<string> ListFileAttachs { get; set; }
    }
}