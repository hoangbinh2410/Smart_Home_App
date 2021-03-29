using Newtonsoft.Json;
using System;

namespace BA_MobileGPS.Entities.RequestEntity
{
    public class UserBehaviorRequest
    {
        /// <summary>
        /// Id hành vi
        /// </summary>
        [JsonProperty("0")]
        public Guid Id { set; get; }

        /// <summary>
        /// Mã hệ thống
        /// </summary>
        [JsonProperty("1")]
        public int SystemType { set; get; }

        /// <summary>
        /// Tên đăng nhập
        /// </summary>
        [JsonProperty("2")]
        public string Username { set; get; }

        /// <summary>
        /// Tên đầy đủ người dùng
        /// </summary>
        [JsonProperty("3")]
        public string Fullname { set; get; }

        /// <summary>
        /// Mã hành vi
        /// </summary>
        [JsonProperty("4")]
        public int ActionCode { set; get; }

        /// <summary>
        /// Tên hành vi
        /// </summary>
        [JsonProperty("5")]
        public string ActionName { set; get; }

        /// <summary>
        /// Thời điểm thực hiện hành vi
        /// </summary>
        [JsonProperty("6")]
        public long ActionTime { set; get; }

        /// <summary>
        /// Mã xí nghiệp
        /// </summary>
        [JsonProperty("7")]
        public int XNCode { set; get; }

        /// <summary>
        /// Menu Key
        /// </summary>
        [JsonProperty("8")]
        public int CurrentPage { set; get; }

        /// <summary>
        /// Thời điểm bắt đầu
        /// </summary>
        [JsonProperty("9")]
        public long StartTime { set; get; }

        /// <summary>
        /// Thời điểm kết thúc
        /// </summary>
        [JsonProperty("10")]
        public long EndTime { set; get; }

        /// <summary>
        /// Mã công ty
        /// </summary>
        [JsonProperty("11")]
        public int CompanyId { set; get; }
    }
}