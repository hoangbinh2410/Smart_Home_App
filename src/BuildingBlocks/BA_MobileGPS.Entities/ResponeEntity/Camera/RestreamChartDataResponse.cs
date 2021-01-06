using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Entities
{
    public class RestreamChartDataResponse : BaseResponse<List<RestreamChartData>>
    {

    }

    public class RestreamChartData
    {
        public string VehiclePlate { get; set; }
        public List<AppVideoTimeInfor> DeviceTimes { get; set; } = new List<AppVideoTimeInfor>();
        public List<AppVideoTimeInfor> CloudTimes { get; set; } = new List<AppVideoTimeInfor>();
    }

    public class AppVideoTimeInfor : VideoTimeInfo
    {
        [JsonProperty("c")]
        public byte Channel { get; set; }

        public string ChannelName
        {
            get
            {
                return string.Format("CH {0}", Channel);
            }
        }
    }
}
