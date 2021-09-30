using System;
using System.Collections.Generic;

namespace BA_MobileGPS.Entities
{
    public class DriverRankingRespone
    {
        public int DriverId { get; set; }
        public string DriverName { get; set; }
        public string DriverAvatar { get; set; }
        public string AverageRank { get; set; }
        public int AverageScore { get; set; }

        public List<DriverRankByDay> DriverRankByDay { get; set; }
    }

    public class DriverRankByDay
    {
        public DateTime Date { get; set; }
        public string Rank { get; set; }
        public int Score { get; set; }
    }
}