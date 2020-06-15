namespace BA_MobileGPS.Entities
{
    public class FeedbackTypeViewModel : BaseModel
    {
        public int FK_FeedbackTypeID { get; set; }

        public string ValueByLanguage { get; set; }

        private int mark;

        public int Mark
        { get => mark; set => SetProperty(ref mark, value, nameof(Mark)); }
    }
}