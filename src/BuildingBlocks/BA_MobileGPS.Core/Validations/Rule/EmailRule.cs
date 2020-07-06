using System.Text.RegularExpressions;

namespace BA_MobileGPS.Core
{
    public class EmailRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; } = "This field is required";

        public bool Check(T value)
        {
            if (value == null)
            {
                return false;
            }

            var str = value as string;
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(str);

            return match.Success;
        }
    }
}