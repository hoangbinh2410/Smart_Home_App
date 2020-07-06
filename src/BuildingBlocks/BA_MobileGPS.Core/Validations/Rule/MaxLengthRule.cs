namespace BA_MobileGPS.Core
{
    public class MaxLengthRule<T> : IValidationRule<T>
    {
        public string ValidationMessage
        {
            get; set;
        }

        public int MaxLenght { get; set; }

        public bool Check(T value)
        {
            if (value == null)
            {
                return false;
            }

            var str = value as string;

            ValidationMessage = string.Format(ValidationMessage, MaxLenght);

            return str.Trim().Length <= MaxLenght;
        }
    }
}