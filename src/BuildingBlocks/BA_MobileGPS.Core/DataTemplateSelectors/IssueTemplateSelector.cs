using BA_MobileGPS.Entities.ResponeEntity.Issues;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.DataTemplateSelectors
{
    public class IssueTemplateSelector : DataTemplateSelector
    {
        public DataTemplate TemplateDueDate { get; set; }

        public DataTemplate TemplateIssueStatus { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is IssueStatusRespone data)
            {
                if (data.IsDueDate)
                {
                    return TemplateDueDate;
                }
                else
                {
                    return TemplateIssueStatus;
                }
            }

            return default;
        }
    }
}