using BA_MobileGPS.Core.Models;

using Xamarin.Forms;

namespace HMU_mLearning.Views
{
    public class GeneralDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate FirstTemplate { get; set; }

        public DataTemplate SecondTemplate { get; set; }

        public DataTemplate ThirdTemplate { get; set; }

        public DataTemplate FourthTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is BaseSelector data)
            {
                switch (data.TemplateType)
                {
                    case TemplateType.First:
                        return FirstTemplate;

                    case TemplateType.Second:
                        return SecondTemplate;

                    case TemplateType.Third:
                        return ThirdTemplate;

                    case TemplateType.Fourth:
                        return FourthTemplate;
                }
            }

            return default;
        }
    }
}