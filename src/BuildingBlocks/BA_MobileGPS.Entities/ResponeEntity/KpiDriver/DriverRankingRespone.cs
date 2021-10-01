using BA_MobileGPS.Utilities.Constant;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace BA_MobileGPS.Entities
{
    public class DriverRankingRespone : BaseModel
    {
        [JsonIgnore]
        public int STT { get; set; }

        public int? DriverId { get; set; }
        public string DriverName { get; set; }
        private string driverAvatar;
        public string DriverAvatar { get => driverAvatar; set => SetProperty(ref driverAvatar, value, nameof(AvatarDisplay)); }

        [JsonIgnore]
        public string AvatarDisplay => string.IsNullOrEmpty(DriverAvatar) ? "ic_driveravatar.png" : $"{ServerConfig.ApiEndpoint}{DriverAvatar}";

        public string AverageRank { get; set; }
        public float AverageScore { get; set; }

        [JsonIgnore]
        public Color BacgroundYourDriver { get; set; }

        public List<DriverRankByDay> DriverRankByDay { get; set; }
    }

    public class DriverRankByDay : BaseModel
    {
        public DateTime Date { get; set; }
        public string Rank { get; set; }
        public float Score { get; set; }
        private Color bacgroundColor=Color.FromHex("#E4E4E4");
        public Color BacgroundColor { get => bacgroundColor; set => SetProperty(ref bacgroundColor, value); }
    }
}