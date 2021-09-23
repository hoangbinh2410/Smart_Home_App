using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace BA_MobileGPS.Core.ViewModels
{
    public class KpiDriverChartPageViewModel : BindableBase
    {
        public ObservableCollection<ChartDataModel> PolarData1 { get; set; }

        public ObservableCollection<ChartDataModel> PolarData2 { get; set; }

        public KpiDriverChartPageViewModel()
        {
            PolarData1 = new ObservableCollection<ChartDataModel>
            {
                new ChartDataModel("Vận tốc cực đại", 100),
                new ChartDataModel("Giảm tốc đột ngột",  90),
                new ChartDataModel("Tăng tốc đột ngột", 85),
                new ChartDataModel("Khởi động đột ngột",  70),
                new ChartDataModel("Thời gian quá tốc độ", 90),
                new ChartDataModel("Số lần quá tốc độ",  60),
            };

            PolarData2 = new ObservableCollection<ChartDataModel>
            {
                new ChartDataModel("Vận tốc cực đại",  100),
                new ChartDataModel("Giảm tốc đột ngột",  100),
                new ChartDataModel("Tăng tốc đột ngột", 100),
                new ChartDataModel("Khởi động đột ngột",  100),
                new ChartDataModel("Thời gian quá tốc độ",  100),
                new ChartDataModel("Số lần quá tốc độ",  100),
            };
        }
    }

    public class ChartDataModel
    {
        public string Name { get; set; }

        public int Value { get; set; }

        public ChartDataModel(string name, int value)
        {
            Name = name;
            Value = value;
        }
    }
}