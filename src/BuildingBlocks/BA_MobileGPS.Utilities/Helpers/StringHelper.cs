using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace BA_MobileGPS.Utilities
{
    public class StringHelper
    {
        //private static readonly Regex Pattern = new Regex("[&<>\"'/]");//|[\n]{2}

        private static readonly Regex Pattern = new Regex("[<>\"']");//|[\n]{2}

        /// <summary>
        /// Bo cac tag html, tra ve thuan text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="isRemove"></param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// sonnl  25/10/2018   created
        /// </Modified>
        public static string RemoveDangerousChars(string text, bool isRemove = true)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(text)) return string.Empty;
                //Xóa khoảng trắng đầu cuối
                text = text.Trim();
                text = WebUtility.HtmlDecode(text);
                //Xóa nhiều hơn 1 khoảng trắng ở giữa các ký tự
                var r = new Regex(@"\s+");
                text = r.Replace(text, @" ");
                //Remove Html Tag
                text = Regex.Replace(text, @"<(.|\n)*?>", string.Empty);

                //Nếu remove những ký tự nguy hiểm , nếu không nghĩa là chấp nhận cho nhập HTML nghĩa là chấp nhận ký tự nguy hiểm
                if (isRemove)
                {
                    ////Xóa tất cả ký tự nguy hiểm
                    //text = Pattern.Replace(text, string.Empty);

                    //Mới thêm sonnl 01/11/2018
                    text = Pattern.Replace(text, string.Empty);
                    text = text.Replace("/", "|").Replace("\\", "|").Replace("[&]", "&").Replace("&", "[&]");
                }
            }
            catch
            {
                return string.Empty;
            }
            return text;
        }

        //public static string StripHtml(string srt)
        //{
        //    return srt;
        //}

        /// <summary>
        /// Encodes the HTML tag.
        /// </summary>
        /// <param name="text">Doan text can encode.</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments sonnl se xoa trong tuong lai
        /// Namth  2/9/2012   created
        /// sonkt   25/07/2018
        /// </Modified>
        public static string EncodeHtmlTag(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            if (text.IndexOf('<') >= 0 || text.IndexOf('>') >= 0)
            {
                //return HttpUtility.HtmlEncode(text);
                text = text.Replace("<", "&lt;").Replace(">", "&gt;");
                return text;
            }
            return text;
        }

        /// <summary>
        /// Bo cac tag html, tra ve thuan text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments sonnl se xoa trong tuong lai
        /// Namth  2/9/2012   created
        /// </Modified>
        public static string StripHtml(string text)
        {
            string ret = string.Empty;
            try
            {
                string s = RemoveMultipleWhitespace(text.Trim());
                string strip = Regex.Replace(s, @"<(.|\n)*?>", string.Empty);
                //ret = !string.IsNullOrEmpty(strip) ? strip : EncodeHtmlTag(text);
                ret = strip;
            }
            catch
            {
                // ignored
            }

            return ret;
        }

        /// <summary>
        /// Bo nhieu hon 1 khoang trang , decode lai tranh nhap ma decode , xoa html tag , encode lai
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// sonnl  29/10/2018   created
        /// </Modified>
        public static string StripHtmlEncode(string text)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(text)) return string.Empty;
                text = RemoveMultipleWhitespace(text.Trim());
                text = WebUtility.HtmlDecode(text);
                text = StripHtml(text);
                text = WebUtility.HtmlEncode(text);
                return text;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string HtmlDecodeNew(string text)
        {
            return text;
        }

        //public static string RemoveHtmlJavascriptTag(string srt)
        //{
        //    return srt;
        //}

        //public static bool IsContainsHtmlTag(string srt)
        //{
        //    return false;
        //}

        /// <summary>
        /// Bo cac tag html, tra ve thuan text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="isEncode">Mã hóa ký tự nguy hiểm</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// Namth  2/9/2012   created
        /// </Modified>
        public static string EncodeDangerousChars(string text, bool isEncode = true)
        {
            try
            {
                //Xóa khoảng trắng đầu cuối
                text = text.Trim();
                text = WebUtility.HtmlDecode(text);
                //Xóa nhiều hơn 1 khoảng trắng ở giữa các ký tự
                var r = new Regex(@"\s+");
                text = r.Replace(text, @" ");
                //Remove Html Tag
                text = Regex.Replace(text, @"<(.|\n)*?>", string.Empty);

                //Nếu không encode những ký tự nguy hiểm , nếu không nghĩa là chấp nhận cho nhập HTML nghĩa là chấp nhận ký tự nguy hiểm
                if (isEncode)
                {
                    //text = WebUtility.HtmlEncode(text);

                    //Mới thêm sonnl 01/11/2018
                    text = Pattern.Replace(text, string.Empty);
                    text = text.Replace("/", "|").Replace("\\", "|").Replace("[&]", "&").Replace("&", "[&]");
                }
            }
            catch
            {
                return string.Empty;
            }
            return text;
        }

        ///// <summary>
        ///// Removes the HTML va javascript tag.
        ///// </summary>
        ///// <param name="text">Doan text can remove</param>
        ///// <returns></returns>
        ///// <Modified>
        ///// Name     Date         Comments
        ///// Namth  2/9/2012   created
        ///// </Modified>
        //public static string RemoveHtmlJavascriptTag(string text)
        //{
        //    string ret = "";
        //    try
        //    {
        //        text = HttpContext.Current.Server.HtmlDecode(text);
        //        Regex re = new Regex(@"<script\s[^>]*>.*?</script>|<\s*(?!/?(?:br?|i|p|u)\b[^>]*>)[^>]*>", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        //        if (text != null) ret = re.Replace(text, string.Empty);
        //    }
        //    catch
        //    {
        //        // ignored
        //    }

        //    return ret;
        //}

        ///// <summary>
        ///// Kiem tra xem doan text co chua cac the html ko?
        ///// </summary>
        ///// <param name="text">The text.</param>
        ///// <returns>
        /////   <c>true</c> doan ma co chua the html ; otherwise <c>false</c>.
        ///// </returns>
        ///// <Modified>
        ///// Name     Date         Comments
        ///// Namth  2/9/2012   created
        ///// </Modified>
        //public static bool IsContainsHtmlTag(string text)
        //{
        //    Regex regex = new Regex(@"<(.|\n)*?>", RegexOptions.IgnoreCase);

        //    return regex.IsMatch(text);
        //}

        /// <summary>
        /// Replaces the single quotes.
        /// fix quotes for SQL insertion...
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// Namth  15/8/2012   created
        /// </Modified>
        public static string ReplaceSingleQuotes(string text)
        {
            string result = string.Empty;
            if (string.IsNullOrEmpty(text))
            {
                return result;
            }
            result = text.Replace("'", "''").Trim();

            return result;
        }

        /// <summary>
        /// Removes multiple whitespace characters from a string.
        /// </summary>
        /// <param name="text">
        /// </param>
        /// <returns>
        /// The remove multiple whitespace.
        /// </returns>
        public static string RemoveMultipleWhitespace(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;
            var r = new Regex(@"\s+");
            return r.Replace(text, @" ");
        }

        /// <summary>
        /// Bo tat ca khoang trang khoang trang trong chuoi truyen vao
        /// 29R2 7314 -> 29R27314
        /// </summary>
        /// <param name="text">
        /// </param>
        /// <returns>
        /// The remove multiple whitespace.
        /// </returns>
        public static string RemoveWhitespace(string text)
        {
            string result = String.Empty;
            if (String.IsNullOrEmpty(text))
            {
                return result;
            }

            var r = new Regex(@"\s+");
            return r.Replace(text, string.Empty);
        }

        /// <summary>
        /// Xóa bỏ khoảng trắng, xuống dòng
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string CleanWhiteSpace(string text)
        {
            //result = Regex.Replace(text, @"\s*\n\s*", "\n", RegexOptions.Multiline);
            var result = Regex.Replace(text, @"\s{2,}", " ", RegexOptions.Multiline);
            return result;
        }

        /// <summary>
        /// Truncates a string with the specified limits and adds (...) to the end if truncated
        /// </summary>
        /// <param name="input">
        /// input string
        /// </param>
        /// <param name="limit">
        /// max size of string
        /// </param>
        /// <returns>
        /// truncated string
        /// </returns>
        public static string Truncate(string input, int limit)
        {
            string output = input;

            if (String.IsNullOrEmpty(input))
            {
                return null;
            }

            // Check if the string is longer than the allowed amount
            // otherwise do nothing
            if (output.Length > limit && limit > 0)
            {
                // cut the string down to the maximum number of characters
                output = output.Substring(0, limit);

                // Check if the space right after the truncate point
                // was a space. if not, we are in the middle of a word and
                // need to cut out the rest of it
                if (input.Substring(output.Length, 1) != " ")
                {
                    int lastSpace = output.LastIndexOf(" ", StringComparison.Ordinal);

                    // if we found a space then, cut back to that space
                    if (lastSpace != -1)
                    {
                        output = output.Substring(0, lastSpace);
                    }
                }

                // Finally, add the "..."
                output += "...";
            }

            return output;
        }

        /// <summary>
        /// Truncates a string with the specified limits by adding (...) to the middle
        /// </summary>
        /// <param name="input">
        /// input string
        /// </param>
        /// <param name="limit">
        /// max size of string
        /// </param>
        /// <returns>
        /// truncated string
        /// </returns>
        public static string TruncateMiddle(string input, int limit)
        {
            if (String.IsNullOrEmpty(input))
            {
                return null;
            }

            string output = input;
            const string middle = "...";

            // Check if the string is longer than the allowed amount
            // otherwise do nothing
            if (output.Length > limit && limit > 0)
            {
                // figure out how much to make it fit...
                int left = (limit / 2) - (middle.Length / 2);
                int right = limit - left - (middle.Length / 2);

                if ((left + right + middle.Length) < limit)
                {
                    right++;
                }
                else if ((left + right + middle.Length) > limit)
                {
                    right--;
                }

                // cut the left side
                output = input.Substring(0, left);

                // add the middle
                output += middle;

                // add the right side...
                output += input.Substring(input.Length - right, right);
            }

            return output;
        }

        /// <summary>
        /// Bo xung tag <br/> vao chuoi html
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="limit">The limit.</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// Namth  16/1/2013   created
        /// </Modified>
        public static string AppendBreak(string input, int limit)
        {
            string output = input;
            if (!string.IsNullOrEmpty(input))
            {
                string[] values = EnumerateByLength(input, limit).ToArray();
                if (values.Length > 1)
                {
                    return string.Join("<br/>", values);
                }
            }
            return output;
        }

        /// <summary>
        /// Tach chuoi theo do dai
        /// </summary>
        /// <param name="text"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        protected static IEnumerable<string> EnumerateByLength(string text, int length)
        {
            int index = 0;
            while (index < text.Length)
            {
                int charCount = Math.Min(length, text.Length - index);
                yield return text.Substring(index, charCount);
                index += length;
            }
        }

        /// <summary>
        /// Cắt khoảng trắng
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string TrimSpace(string str)
        {
            if (str == null) return string.Empty;
            string sT = str.TrimEnd().TrimStart();

            while (sT.Contains("  "))
            {
                sT = sT.Remove(sT.IndexOf("  ", StringComparison.Ordinal), 1);
            }
            return sT;
        }

        public static string ToVietnameseWithoutAccent(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"\p{IsCombiningDiacriticalMarks}+");
                string strFormD = text.Normalize(System.Text.NormalizationForm.FormD);
                return regex.Replace(strFormD, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
            }
            return text;
        }

        /// <summary>
        /// encrypt password
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string EncryptPassword(string password)
        {
            UnicodeEncoding encoding = new UnicodeEncoding();
            byte[] hashBytes = encoding.GetBytes(password);

            //Compute the SHA-1 hash
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();

            byte[] cryptPassword = sha1.ComputeHash(hashBytes);

            return BitConverter.ToString(cryptPassword);
        }

        /// <summary>
        /// Lay ra ten va gia tri cua tung thuoc tinh cua doi tuong
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// Namth  13/9/2012   created
        /// </Modified>
        public static string GetValueProperties<T>(T obj) where T : class
        {
            StringBuilder sb = new StringBuilder();

            PropertyInfo[] pros = obj.GetType().GetProperties();

            sb.AppendLine($"BEGIN:{obj.GetType().Name.ToUpper()}");
            foreach (PropertyInfo p in pros)
            {
                sb.AppendFormat("[{0}:{1}]", p.Name, p.GetValue(obj, null));
                sb.AppendLine();
            }
            sb.AppendLine($"END:{obj.GetType().Name.ToUpper()}");
            return sb.ToString();
        }

        /// <summary>
        /// Lay ra ten va gia tri cua cac thuoc tinh trong object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// Namth  13/9/2012   created
        /// </Modified>
        public static string GetValueProperties<T>(List<T> list) where T : class
        {
            StringBuilder sb = new StringBuilder();
            if (list != null && list.Count > 0)
            {
                foreach (T item in list)
                {
                    sb.Append(GetValueProperties(item));
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Tạo link google map API javascript
        /// </summary>
        /// <param name="key"></param>
        /// <param name="lstParam"></param>
        /// <returns></returns>
        public static string BuildLinkGoogleMapAPI(string key, Dictionary<string, string> lstParam)
        {
            string linkFormat;

            if (string.IsNullOrEmpty(key))
            {
                if (lstParam.Count == 0)
                {
                    return "<script type = \"text/javascript\" src=\"https://maps.google.com/maps/api/js\" /></script>";
                }
                linkFormat = "<script type = \"text/javascript\" src=\"https://maps.google.com/maps/api/js?{0}\" /></script>";
            }
            else
            {
                if (lstParam.Count == 0)
                {
                    return "<script  type = \"text/javascript\" src=\"https://maps.googleapis.com/maps/api/js?key=" + key + "\"></script>";
                }
                linkFormat = "<script  type = \"text/javascript\" src=\"https://maps.googleapis.com/maps/api/js?key=" + key + "&{0}\"></script>";
            }

            var linkParams = lstParam.Aggregate(string.Empty, (current, item) => current + $"&{item.Key}={item.Value}");

            return string.Format(linkFormat, linkParams);
        }

        #region Generating XNCode from ListXNofPartner

        /// <summary>
        /// Generate xn code from input string
        /// </summary>
        /// <param name="xnCode"></param>
        /// <returns></returns>
        public static List<int> GenerateXNCode(string xnCode)
        {
            if (string.IsNullOrEmpty(xnCode)) return new List<int>();

            if (xnCode.Length == 3)
            {
                if (int.TryParse(xnCode, out var xnc) && xnc > 0)
                {
                    return new List<int> { xnc };
                }
                return new List<int>();
            }

            string[] aryXNCode = xnCode.Split(';');
            var lstReturn = new List<int>();
            foreach (var xnc in aryXNCode)
            {
                if (xnc.Contains(".."))
                {
                    lstReturn.AddRange(ConvertRangeXN(xnc));
                }
                else
                {
                    lstReturn.Add(int.Parse(xnc));
                }
            }
            return lstReturn.Distinct().OrderBy(x => x).ToList();
        }

        /// <summary>
        /// Convert range xn with swap.
        /// </summary>
        /// <param name="rangeXN"></param>
        /// <returns></returns>
        private static List<int> ConvertRangeXN(string rangeXN)
        {
            var lstReturn = new List<int>();
            rangeXN = rangeXN.Replace("..", ".");

            string[] aryXNCode = rangeXN.Split('.');
            if (aryXNCode.Length <= 1) return new List<int>();

            for (int i = 0; i < aryXNCode.Length - 1; i++)
            {
                int start = Convert.ToInt32(aryXNCode[i]);
                int end = Convert.ToInt32(aryXNCode[i + 1]);
                if (start == 0 && end == 0) continue;
                if (start > end)
                {
                    int tmp = start;
                    start = end;
                    end = tmp;
                }
                for (int j = start; j <= end; j++)
                {
                    lstReturn.Add(j);
                }
            }

            return lstReturn;
        }

        /// <summary>
        /// Kiem tra xem doan text co chua cac the html ko?
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>
        ///   <c>true</c> doan ma co chua the html ; otherwise <c>false</c>.
        /// </returns>
        /// <Modified>
        /// Name     Date         Comments
        /// Namth  2/9/2012   created
        /// </Modified>
        public static bool IsContainsHTMLTag(string text)
        {
            Regex regex = new Regex(@"<(.|\n)*?>", RegexOptions.IgnoreCase);

            return regex.IsMatch(text);
        }

        ///// <summary>
        ///// Mã hóa chuỗi
        ///// </summary>
        ///// <param name="input">Chuỗi cần mã hóa</param>
        ///// <param name="key">Key để giải mã và mã hóa</param>
        ///// <returns></returns>
        //public static string Encrypt(string input, string key = "binh-anh1-234567")
        //{
        //    if (string.IsNullOrEmpty(input))
        //    {
        //        return "";
        //    }
        //    byte[] inputArray = Encoding.UTF8.GetBytes(input);
        //    TripleDESCryptoServiceProvider tripleDes =
        //        new TripleDESCryptoServiceProvider
        //        {
        //            Key = Encoding.UTF8.GetBytes(key),
        //            Mode = CipherMode.ECB,
        //            Padding = PaddingMode.PKCS7
        //        };

        //    ICryptoTransform cTransform = tripleDes.CreateEncryptor();
        //    byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
        //    tripleDes.Clear();
        //    return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        //}

        ///// <summary>
        ///// Giải mã chuỗi
        ///// </summary>
        ///// <param name="input">Đoạn mã cần giải mã</param>
        ///// <param name="key">Key đã dùng để mã hóa</param>
        ///// <returns></returns>
        //public static string Decrypt(string input, string key = "binh-anh1-234567")
        //{
        //    if (string.IsNullOrEmpty(input))
        //    {
        //        return "";
        //    }
        //    byte[] inputArray = Convert.FromBase64String(input);
        //    TripleDESCryptoServiceProvider tripleDes =
        //        new TripleDESCryptoServiceProvider
        //        {
        //            Key = Encoding.UTF8.GetBytes(key),
        //            Mode = CipherMode.ECB,
        //            Padding = PaddingMode.PKCS7
        //        };
        //    ICryptoTransform cTransform = tripleDes.CreateDecryptor();
        //    byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
        //    tripleDes.Clear();
        //    return Encoding.UTF8.GetString(resultArray);
        //}

        #endregion Generating XNCode from ListXNofPartner

        public static bool ValidateEmail(string email)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);

            return match.Success;
        }

        public static bool HasDangerousChars(string input)
        {
            if (input == null)
                return false;

            Regex regex = new Regex("['\"<>/&]");
            Match match = regex.Match(input);

            return match.Success;
        }

        public static bool ValidateAddress(string input)
        {
            if (input == null)
                return false;

            Regex regex = new Regex("['\"<>&]");
            Match match = regex.Match(input);

            return !match.Success;
        }

        #region So dien thoai

        public static bool ValidPhoneNumer(string phoneNumber, string lengthAndPrefixNumber, out string newPhoneNumer)
        {
            Regex rx = new Regex("^[0-9]+$");

            // Nhập chưa ký tự
            if (!rx.IsMatch(phoneNumber))
            {
                newPhoneNumer = string.Empty;
                return false;
            }

            //Chỉ lấy lại kí tự số
            phoneNumber = new string(phoneNumber.Where(char.IsDigit).ToArray());

            // Nhập linh tinh là biến
            if (string.IsNullOrEmpty(phoneNumber))
            {
                newPhoneNumer = string.Empty;
                return false;
            }

            //Danh sach do dai sdt va dau so tuong ung
            var lstlengthAndPrefixNumberStr = lengthAndPrefixNumber.Split(';');

            var lstValid = lstlengthAndPrefixNumberStr.Select(item => item.Split('-')).
                ToDictionary(lf => int.Parse(lf[0]), lf => lf[1].Split(',').ToList());

            //So dien thoai co do dai tuong ung va dau so tuong ung voi do dai
            if (lstValid.Where(valid => phoneNumber.Length == valid.Key).
                Any(valid => valid.Value.Any(pre => phoneNumber.IndexOf(pre, StringComparison.Ordinal) == 0)))
            {
                newPhoneNumer = phoneNumber;
                return true;
            }
            newPhoneNumer = string.Empty;
            return false;
        }

        public static bool ValidPhoneNumer(string phoneNumber, string lengthAndPrefixPhoneNumber)
        {
            Regex rx = new Regex("^[0-9]+$");

            // Nhập chưa ký tự
            if (!rx.IsMatch(phoneNumber))
            {
                return false;
            }

            //Chỉ lấy lại kí tự số
            phoneNumber = new string(phoneNumber.Where(char.IsDigit).ToArray());

            // Nhập linh tinh là biến
            if (string.IsNullOrEmpty(phoneNumber))
            {
                return false;
            }

            //Danh sach do dai sdt va dau so tuong ung
            var lstlengthAndPrefixNumberStr = lengthAndPrefixPhoneNumber.Split(';');

            var lstValid = lstlengthAndPrefixNumberStr.Select(item => item.Split('-')).
                ToDictionary(lf => int.Parse(lf[0]), lf => lf[1].Split(',').ToList());

            //So dien thoai co do dai tuong ung va dau so tuong ung voi do dai
            if (lstValid.Where(valid => phoneNumber.Length == valid.Key).
                Any(valid => valid.Value.Any(pre => phoneNumber.IndexOf(pre, StringComparison.Ordinal) == 0)))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Chuyen 1 ki tu co dau -> ko dau.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// Namth  23/6/2012   created
        /// </Modified>
        private static char ConvertedVnChar(char x)
        {
            if ((x >= 'a' && x <= 'z') || (x >= '0' && x <= '9') || (x >= 'A' && x <= 'Z'))
            {
                return x;
            }
            String s = x.ToString();

            if ("àáạảãâầấậẩẫăắằặẳẵ".Contains(s)) return 'a';
            if ("èéẹẻẽêềếệểễ".Contains(s)) return 'e';
            if ("ìíịỉĩ".Contains(s)) return 'i';
            if ("đ".Contains(s)) return 'd';
            if ("òóọỏõôồốộổỗơờớợởỡ".Contains(s)) return 'o';
            if ("ùúụủũưừứựửữ".Contains(s)) return 'u';
            if ("ỳýỵỷỹ".Contains(s)) return 'y';
            if ("ÀÁẠẢÃÂẦẤẬẨẪĂẮẰẶẲẴ".Contains(s)) return 'A';
            if ("ÈÉẸẺẼÊỀẾỆỂỄ".Contains(s)) return 'E';
            if ("ÌÍỊỈĨ".Contains(s)) return 'I';
            if ("Đ".Contains(s)) return 'D';
            if ("ÒÓỌỎÕÔỒỐỘỔỖƠỜỚỢỞỠ".Contains(s)) return 'O';
            if ("ÙÚỤỦŨƯỪỨỰỬỮ".Contains(s)) return 'U';
            if ("ỲÝỴỶỸ".Contains(s)) return 'Y';
            if (x == '\t' || x == ' ') return '-';
            if (@"_&*?(){}[]\|/+:'"";,.-".Contains(s)) return '-';

            return '@';
        }

        /// <summary>
        /// Chuyen 1 chuoi tieng viet co dau thanh khong dau.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// Namth  7/9/2012   created
        /// </Modified>
        public static string ConvertToVn(string input)
        {
            StringBuilder sb = new StringBuilder();
            char[] ca = input.Trim().ToCharArray();
            foreach (char t in ca)
            {
                char x = ConvertedVnChar(t);
                if (x != '@')
                    sb.Append(x);
            }

            return Regex.Replace(sb.ToString(), @"-+", " ").Trim(' ').ToLower();
        }

        public static string[] VietNamChar = new string[]
        {
            "áàạảãâấầậẩẫăắằặẳẵ",
            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
            "éèẹẻẽêếềệểễ",
            "ÉÈẸẺẼÊẾỀỆỂỄ",
            "óòọỏõôốồộổỗơớờợởỡ",
            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
            "úùụủũưứừựửữ",
            "ÚÙỤỦŨƯỨỪỰỬỮ",
            "íìịỉĩ",
            "ÍÌỊỈĨ",
            "đ",
            "Đ",
            "ýỳỵỷỹ",
            "ÝỲỴỶỸ"
        };

        public static readonly Regex IsVietNamChar = new Regex("^[" + string.Join("", VietNamChar) + "0-9]+", RegexOptions.Compiled);

        #endregion So dien thoai

        public static string ConvertIntToHex(int value)
        {
            return value.ToString("X").PadLeft(6, '0');
        }

        /// <summary>
        /// Chuyen 1 chuoi tieng viet co dau thanh khong dau va co gach ngang.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// trungtq  23/6/2012   created
        /// </Modified>
        public static string ConvertToDashesVn(string input)
        {
            StringBuilder sb = new StringBuilder();
            char[] ca = input.Trim().ToCharArray();
            foreach (char t in ca)
            {
                char x = ConvertedVnChar(t);
                if (x != '@')
                    sb.Append(x);
            }

            return Regex.Replace(sb.ToString(), @"-+", "-").Trim('-').ToLower();

        }

        /// <summary>
        /// Chuyen 1 chuoi tieng viet co dau thanh khong dau va co gach chan.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// trungtq  23/6/2012   created
        /// </Modified>
        public static string ConvertToUnderlinedVn(string input)
        {
            StringBuilder sb = new StringBuilder();
            char[] ca = input.Trim().ToCharArray();
            foreach (char t in ca)
            {
                char x = ConvertedVnChar(t);
                if (x != '@')
                    sb.Append(x);
            }

            return Regex.Replace(sb.ToString(), @"-+", "_").Trim('_').ToLower();

        }
    }
}