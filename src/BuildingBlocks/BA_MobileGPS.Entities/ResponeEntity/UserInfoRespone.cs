using BA_MobileGPS.Utilities.Constant;

using Newtonsoft.Json;

using System;

using Xamarin.Forms;

namespace BA_MobileGPS.Entities
{
    public class UserInfoRespone : BaseModel
    {
        [JsonProperty("0")]
        public string UserName { get; set; }

        private string avatarUrl;

        [JsonProperty("1")]
        public string AvatarUrl { get => avatarUrl; set => SetProperty(ref avatarUrl, value, nameof(AvatarDisplay)); }

        private string avatarPathLocal;
        public string AvatarPathLocal { get => avatarPathLocal; set => SetProperty(ref avatarPathLocal, value, nameof(AvatarDisplay)); }

        public ImageSource AvatarDisplay => string.IsNullOrWhiteSpace(AvatarPathLocal) ? (string.IsNullOrWhiteSpace(AvatarUrl) ? "ic_launcher.png" : $"{ServerConfig.ApiEndpoint}{AvatarUrl}") : ImageSource.FromFile(AvatarPathLocal);

        [JsonProperty("2")]
        public string FullName { get; set; }

        [JsonProperty("3")]
        public string PhoneNumber { get; set; }

        [JsonProperty("4")]
        public string Email { get; set; }

        private DateTime? dateOfBirth;

        [JsonProperty("5")]
        public DateTime? DateOfBirth { get => dateOfBirth; set => SetProperty(ref dateOfBirth, value); }

        [JsonProperty("6")]
        public int GenderId { get; set; }

        [JsonProperty("7")]
        public string Career { get; set; }

        [JsonProperty("8")]
        public string Role { get; set; }

        [JsonProperty("9")]
        public int ReligionId { get; set; }

        [JsonProperty("10")]
        public string Address { get; set; }

        [JsonProperty("11")]
        public string Facebook { get; set; }
    }
}