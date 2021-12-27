using System.Collections.Generic;

namespace BA_MobileGPS.Entities
{
    public class DriverKpiChartRespone
    {
        public string TotalScore { get; set; }
        public string TotalRank { get; set; }
        public List<EvaluationsByType> EvaluationsByType { get; set; }
    }

    public class EvaluationsByType
    {
        public string TotalScore { get; set; }
        public string TotalRank { get; set; }
        public int TypeId { get; set; }
        public List<Evaluation> Evaluations { get; set; }
    }

    public class Evaluation
    {
        public int EvaluationItemID { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public double? Value { get; set; }
        public double? Score { get; set; }
        public string NameRank { get; set; }
        public int OrderDisPlay { get; set; }
        public int? OrderDisPlayActual { get; set; }
        public string Comment { get; set; }
    }
}