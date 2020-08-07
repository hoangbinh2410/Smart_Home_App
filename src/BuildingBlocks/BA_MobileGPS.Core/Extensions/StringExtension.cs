using BA_MobileGPS.Core.Resource;

using System;
using System.Diagnostics;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace Xamarin.Forms.Extensions
{
    public static class StringExtension
    {
        public static bool IsDateTime(this string date)
        {
            try
            {
                DateTime dt = DateTime.Parse(date);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string ToUnSign(this string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, string.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }

        public static bool UnSignContains(this string a, string b)
        {
            return a.ToUnSign().ToUpper().Contains(b.ToUnSign().ToUpper());
        }

        public static string ToBase64(this string text)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
        }

        public static string Base64Decode(this string base64EncodedData)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(base64EncodedData));
        }

        public static string NumericFormat(this string input)
        {
            string result = input;
            if (string.IsNullOrWhiteSpace(result))
                result = "0";
            else if (result.StartsWith("0") && result.Length > 1 && !result.Contains(",") && !result.Contains("."))
                result = result.Remove(result.IndexOf('0'), 1);
            else if (result.StartsWith(","))
                result = result.Remove(result.IndexOf(','), 1);
            else if (result.StartsWith("."))
                result = result.Remove(result.IndexOf('.'), 1);
            else if (Convert.ToDecimal(result) >= 1000 && !result.EndsWith("."))
                result = Convert.ToDecimal(result).ToString("###,###,###,###,##0.##");
            else if (Convert.ToDecimal(result) < 1000 && result.Contains(",") && !result.EndsWith("."))
                result = result.Remove(result.IndexOf(','), 1);
            return result;
        }

        public static bool EmailValidate(this string input)
        {
            try
            {
                Regex regex = new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
                return regex.IsMatch(input.Trim());
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public static bool IsEmail(this string input)
        {
            try
            {
                var email = new MailAddress(input);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public static bool IsGuidEmpty(this Guid id)
        {
            return id.Equals(Guid.Empty);
        }

        public static string SecondsToString(this int secs)
        {
            TimeSpan time = TimeSpan.FromSeconds(secs);

            string answer = string.Empty;

            if (time.Days > 0)
            {
                if (time.Days > 1)
                {
                    answer = string.Format("{0} {1} ", time.Days, MobileResource.Common_Label_Days);
                }
                else
                {
                    answer = string.Format("{0} {1} ", time.Days, MobileResource.Common_Label_Day);
                }
            }

            if (time.Hours > 0)
            {
                if (time.Hours > 1)
                {
                    answer += string.Format("{0} {1} ", time.Hours, MobileResource.Common_Label_Hours);
                }
                else
                {
                    answer += string.Format("{0} {1} ", time.Hours, MobileResource.Common_Label_Hour);
                }
            }
            if (time.Minutes > 0 || time.Minutes == 0)
            {
                if (time.Minutes > 1)
                {
                    answer += string.Format("{0} {1}", time.Minutes, MobileResource.Common_Label_Minutes);
                }
                else
                {
                    answer += string.Format("{0} {1}", time.Minutes, MobileResource.Common_Label_Minute);
                }
            }
            else
            {
                answer += string.Format("{0} {1}", 0, MobileResource.Common_Label_Minute);
            }

            return answer;
        }

        public static string SecondsToStringShort(this int secs)
        {
            TimeSpan time = TimeSpan.FromSeconds(secs);

            string answer;

            if (time.Days > 0)
            {
                answer = string.Format("{0}{1} {2}{3} {4}{5}",
                               time.Days, MobileResource.Common_Label_Day2,
                               time.Hours, MobileResource.Common_Label_Hour2,
                               time.Minutes, MobileResource.Common_Label_Minute2);
            }
            else if (time.Hours > 0)
            {
                answer = string.Format("{0}{1} {2}{3}",
                               time.Hours, MobileResource.Common_Label_Hour2,
                               time.Minutes, MobileResource.Common_Label_Minute2);
            }
            else
            {
                answer = string.Format("{0}{1}",
                               time.Minutes, MobileResource.Common_Label_Minute2);
            }

            return answer;
        }

        public static string MinutesToStringShort(this int minutes)
        {
            TimeSpan time = TimeSpan.FromMinutes(minutes);

            string answer;

            if (time.Days > 0)
            {
                answer = string.Format("{0} {1} {2} {3} {4} {5}",
                              time.Days, MobileResource.Common_Label_Day,
                              time.Hours, MobileResource.Common_Label_Hour,
                              time.Minutes, MobileResource.Common_Label_Minute);
            }
            else if (time.Hours > 0)
            {
                answer = string.Format("{0} {1} {2} {3}",
                               time.Hours, MobileResource.Common_Label_Hour,
                               time.Minutes, MobileResource.Common_Label_Minute);
            }
            else
            {
                answer = string.Format("{0} {1}",
                               time.Minutes, MobileResource.Common_Label_Minute);
            }

            return answer;
        }
    }
}