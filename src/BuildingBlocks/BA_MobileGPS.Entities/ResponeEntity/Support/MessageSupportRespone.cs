using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Entities.ResponeEntity.Support
{
    public class MessageSupportRespone
    {
        [JsonProperty("Id")]
        public Guid ID { get; set; }
        [JsonProperty("FK_SupportCategoryID")]
        public Guid FK_SupportCategoryID { get; set; }
        [JsonProperty("Questions")]
        public string Questions { get; set; }
        [JsonProperty("Guides")]
        public string Guides { get; set; }
        [JsonProperty("OrderNo")]
        public int OrderNo { get; set; }
    }
}
