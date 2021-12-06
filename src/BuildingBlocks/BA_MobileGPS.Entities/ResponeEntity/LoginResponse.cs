using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace BA_MobileGPS.Entities
{
    public class LoginResponse : BaseModel
    {
        [JsonProperty("0")]
        public LoginStatus Status { set; get; }

        [JsonProperty("1")]
        public int CompanyId { set; get; }

        [JsonProperty("2")]
        public CompanyType CompanyType { set; get; }

        [JsonProperty("3")]
        public List<int> Permissions { set; get; }

        [JsonProperty("4")]
        public Guid UserId { set; get; }

        [JsonProperty("5")]
        public bool IsNeedChangePassword { set; get; }

        private string avatarUrl;

        [JsonProperty("6")]
        public string AvatarUrl { get => avatarUrl; set => SetProperty(ref avatarUrl, value); }

        [JsonProperty("7")]
        public List<CompanyConfiguration> CompanyConfigurations { set; get; }

        [JsonProperty("8")]
        public List<MobileUserSetting> MobileUserSetting { set; get; }

        [JsonProperty("9")]
        public List<HostConfiguration> HostConfigurations { get; set; }

        [JsonProperty("10")]
        public DateTime TimeServer { set; get; }

        [JsonProperty("11")]
        public string AccessToken { get; set; }

        [JsonProperty("12")]
        public CustomerType SubCustomer { get; set; }

        [JsonProperty("13")]
        public bool Has2FactorAuthentication { get; set; }

        [JsonProperty("14")]
        public bool IsNeededOtp { get; set; }

        [JsonProperty("15")]
        public int XNCode { get; set; }

        [JsonProperty("16")]
        public string UserName { get; set; }

        [JsonProperty("17")]
        public string FullName { get; set; }

        [JsonProperty("18")]
        public string PhoneNumber { get; set; }

        [JsonProperty("19")]
        public UserType UserType { get; set; }
    }
}