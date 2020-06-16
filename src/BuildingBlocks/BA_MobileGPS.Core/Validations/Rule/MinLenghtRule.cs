namespace BA_MobileGPS.Core
{
    public class MinLenghtRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }

        public int MinLenght { get; set; }

        public bool Check(T value)
        {
            if (value == null)
            {
                return false;
            }

            var str = value as string;

            ValidationMessage = string.Format(ValidationMessage, MinLenght);

            return str.Trim().Length >= MinLenght;
        }
    }
}