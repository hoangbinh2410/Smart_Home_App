using System.Linq;

namespace BA_MobileGPS.Core
{
    public class IsNotContains<T> : IValidationRule<T>
    {
        public string[] Keywords { get; set; }
        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            if (value == null)
            {
                return true;
            }

            var str = value as string;

            if (Keywords.Count() > 0 && Keywords.Any(v => str.ToLower().Contains(v.ToLower())))
            {
                return false;
            }

            return true;
        }
    }
}