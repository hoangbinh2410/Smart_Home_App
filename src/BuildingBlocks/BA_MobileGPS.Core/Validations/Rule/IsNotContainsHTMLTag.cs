using BA_MobileGPS.Utilities;

namespace BA_MobileGPS.Core
{
    public class IsNotContainsHTMLTag<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            if (value == null)
            {
                return false;
            }

            var str = value as string;

            if (StringHelper.IsContainsHTMLTag(str))
            {
                return false;
            }
            return true;
        }
    }
}