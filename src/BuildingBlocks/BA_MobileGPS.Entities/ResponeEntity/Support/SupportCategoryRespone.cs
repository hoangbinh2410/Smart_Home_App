﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Entities.ResponeEntity.Support
{
    public class SupportCategoryRespone
    {
        [JsonProperty("id")]
        public Guid ID { get; set; }

        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("flags")]
        public int Flags { get; set; }

        [JsonProperty("iconApp")]
        public string IconApp { get; set; }

        [JsonProperty("orderNo")]
        public string OrderNo { get; set; }
    }
}
