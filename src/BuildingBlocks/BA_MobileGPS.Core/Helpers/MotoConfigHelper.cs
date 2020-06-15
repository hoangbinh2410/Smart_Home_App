namespace BA_MobileGPS.Core
{
    public class MotoConfigHelper
    {
        public static string JoinPhoneNumber(string item)
        {
            if (!string.IsNullOrEmpty(item))
            {
                item = item + ",";
            }
            return item;
        }

        public static string JoinPhoneNumberEx(string item)
        {
            if (item == "0" || item == "000" || item == "0900000000" || item == string.Empty || item == null)
            {
                item = string.Empty;
            }
            else
            {
                item = item + ",";
            }
            return item;
        }

        public static string FixPhoneNumber(string item)
        {
            return string.IsNullOrEmpty(item) ? "0900000000" : item;
        }

        public static string ReFixPhoneNumber(string item)
        {
            return item == "0" || item == "000" || item == "0900000000" ? string.Empty : item;
        }

        public static string ConvertPhoneNumber(string item)
        {
            return item.Replace("0900000000", "000");
        }

        public static string ConvertPhoneNumberEx(string item)
        {
            string result = string.Empty;
            string[] str = item.Split(',');
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] != "000")
                {
                    result = result + str[i].Trim() + ",";
                }
            }
            return result.Length > 0 ? result.Substring(0, result.Length - 1) : result;
        }
    }
}