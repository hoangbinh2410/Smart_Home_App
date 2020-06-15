namespace BA_MobileGPS.Entities
{
    public class ComboboxResponse : BaseModel
    {
        public int Key { get; set; }

        public string Keys { get; set; }

        private string _value;
        public string Value { get => _value; set => SetProperty(ref _value, value); }

        public short ComboboxType { get; set; }
    }
}