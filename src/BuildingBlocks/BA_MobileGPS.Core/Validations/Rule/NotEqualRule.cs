namespace BA_MobileGPS.Core
{
    public class NotEqualRule<T> : IValidationRule<T>
    {
        public string Password { get; set; }

        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            if (value == null)
            {
                return false;
            }

            var str = value as string;

            if (str.Equals(Password))
            {
                return false;
            }

            return true;
        }
    }
}