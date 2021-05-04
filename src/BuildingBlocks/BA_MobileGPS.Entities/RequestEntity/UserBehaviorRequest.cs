using BA_MobileGPS.Entities.Enums;
using Newtonsoft.Json;
using System;

namespace BA_MobileGPS.Entities.RequestEntity
{
    public class UserBehaviorRequest
    {
        /// <summary>
        /// Id hành vi
        /// </summary>
        [JsonProperty(PropertyName = "0")]
        public Guid Id { set; get; }

        /// <summary>
        /// Mã hệ thống
        /// </summary>
        [JsonProperty(PropertyName = "1")]
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
        /// Tên đầy đủ người dùng
        /// </summary>
        [JsonProperty("4")]
        public MenuKeyEnums MenuKey { set; get; }

        /// <summary>
        /// Mã xí nghiệp
        /// </summary>
        [JsonProperty("5")]
        public int XNCode { set; get; }

        /// <summary>
        /// Thời điểm
        /// </summary>
        [JsonProperty("6")]
        public long Time { set; get; }

        /// <summary>
        /// Loại start end
        /// </summary>
        [JsonProperty("7")]
        public UserBehaviorType TimeType { set; get; }

        /// <summary>
        /// Mã công ty
        /// </summary>
        [JsonProperty("8")]
        public int CompanyId { set; get; }
    }

    public enum UserBehaviorType
    {
        Start = 0,
        End = 1
    }
}