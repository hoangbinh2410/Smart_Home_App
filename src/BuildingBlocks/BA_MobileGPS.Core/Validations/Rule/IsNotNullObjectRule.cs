namespace BA_MobileGPS.Core
{
    public class IsNotNullObjectRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; } = "This field is required";

        public bool Check(T value)
        {
            if (value == null)
            {
                return false;
            }

            return true;
        }
    }
}