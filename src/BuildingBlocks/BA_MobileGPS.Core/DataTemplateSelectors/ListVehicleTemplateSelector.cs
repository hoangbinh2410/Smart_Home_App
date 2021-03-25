using BA_MobileGPS.Core.Extensions;
using BA_MobileGPS.Entities;

using Xamarin.Forms;

namespace BA_MobileGPS.Core.DataTemplateSelectors
{
    public class ListVehicleTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DefaultTemplate { get; set; }

        public DataTemplate LostSignalTemplate { get; set; }

        public DataTemplate ExpiredTemplate { get; set; }

        public DataTemplate UnpaidTemplate { get; set; }

        public DataTemplate StopServiceTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is VehicleOnlineViewModel data)
            {
                if (data.IsShowDetail || !MobileSettingHelper.IsUseVehicleDebtMoney)
                {
                    if (StateVehicleExtension.IsLostGSM(data.VehicleTime))
                    {
                        return LostSignalTemplate;
                    }
                    else
                    {
                        return DefaultTemplate;
                    }
                }
                else
                {
                    switch (data.MessageId)
                    {
                        case 2: return ExpiredTemplate;

                        case 3: return UnpaidTemplate;

                        case 128: return StopServiceTemplate;
                    }
                }
            }

            return default;
        }
    }
}