namespace BA_MobileGPS.Core.Models
{
    public class BaseSelector
    {
        public TemplateType TemplateType { get; set; }
    }

    public enum TemplateType
    {
        First,
        Second,
        Third,
        Fourth
    }
}