using System.Text.RegularExpressions;

namespace BA_MobileGPS.Core
{
    public class ExpressionRule<T> : IValidationRule<T>
    {
        public string Expression { get; set; }

        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            if (value == null)
            {
                return true;
            }

            var str = value as string;
            Regex regex = new Regex(Expression);
            Match match = regex.Match(str);

            return match.Success;
        }
    }

    public class ExpressionDangerousCharsRule<T> : IValidationRule<T>
    {
        public string Expression { get; set; }
        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            if (value == null)
            {
                return true;
            }

            var str = value as string;

            Regex regex = new Regex(string.Format("[{0}]", Expression));
            Match match = regex.Match(str);

            return !match.Success;
        }
    }
}