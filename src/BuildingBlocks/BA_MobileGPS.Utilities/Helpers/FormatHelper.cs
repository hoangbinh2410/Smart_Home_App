using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace BA_MobileGPS.Utilities
{
    public class FormatHelper
    {
        protected const string FormatDoubleString = "#,###,###,##0.###";

        /// <summary>
        /// Culture mac dinh (VietNam)
        /// </summary>
        protected static CultureInfo DefaultCulture
        {
            get
            {
                return new CultureInfo("vi-VN");
            }
        }

        /// <summary>
        /// Convert to double, mac dinh lay 2 so sau dau phay
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// Namth  6/8/2012   created
        /// </Modified>
        public static double ConvertToDouble(object obj)
        {
            return ConvertToDouble(obj, 2);
        }

        /// <summary>
        /// Convert to double.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="numAfterComma">So luong ki tu sau dau phay.</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// Namth  6/8/2012   created
        /// </Modified>
        public static double ConvertToDouble(object obj, int numAfterComma)
        {
            return ConvertToDouble(obj, numAfterComma, new CultureInfo("en-US"));
        }

        /// <summary>
        /// Convert to dobule
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="numAfterComma">So luong ki tu sau dau phay</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// Namth  6/8/2012   created
        /// </Modified>
        public static double ConvertToDouble(object obj, int numAfterComma, CultureInfo culture)
        {
            double ret = 0;
            try
            {
                double.TryParse(obj.ToString(), NumberStyles.Float, culture, out ret);

                if (ret > 0)
                {
                    ret = Math.Round(ret, numAfterComma, MidpointRounding.AwayFromZero);
                }
                else
                {
                    double.TryParse(obj.ToString(), NumberStyles.Float, new CultureInfo("vi-VN"), out ret);
                    if (ret > 0)
                    {
                        ret = Math.Round(ret, numAfterComma, MidpointRounding.AwayFromZero);
                    }
                }
            }
            catch
            {
                // ignored
            }
            return ret;
        }

        /// <summary>
        /// Lam tron so
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// Namth  6/8/2012   created
        /// </Modified>
        public static double RoundDouble(double obj)
        {
            return RoundDouble(obj, 3);
        }

        /// <summary>
        /// Lam tron so
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="number">The number.</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// Namth  6/8/2012   created
        /// </Modified>
        public static double RoundDouble(double obj, int number)
        {
            double ret = 0;
            try
            {
                ret = Math.Round(Convert.ToDouble(obj), number);
            }
            catch
            {
                // ignored
            }
            return ret;
        }

        /// <summary>
        /// Format double: Mac dinh lay 2 so sau dau phay.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// Namth  6/8/2012   created
        /// </Modified>
        public static string FormatDouble(object obj)
        {
            return FormatDouble(obj, 2);
        }

        /// <summary>
        /// Format double
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="digits">The digits.</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// TRAN QUANG TRUNG  15/03/2013   created
        /// </Modified>
        public static string FormatDouble(object obj, int digits)
        {
            string ret = string.Empty;
            if (obj == null) return ret;
            try
            {
                ret = Math.Round(Convert.ToDouble(obj), digits).ToString(FormatDoubleString, DefaultCulture);
            }
            catch
            {
                // ignored
            }
            return ret;
        }

        /// <summary>
        /// Format double theo culture
        /// </summary>
        /// <param name="obj">obj</param>
        /// <param name="number">so ki tu can lay sau dau phay</param>
        /// <param name="culture">culture</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// Namth  8/6/2012   created
        /// </Modified>
        public static string FormatDouble(object obj, int number, CultureInfo culture)
        {
            string ret = string.Empty;
            if (obj == null) return ret;
            try
            {
                ret = Math.Round(Convert.ToDouble(obj), number, MidpointRounding.AwayFromZero).ToString(FormatDoubleString, culture);
            }
            catch
            {
                // ignored
            }
            return ret;
        }

        /// <summary>
        /// Dinh dang kieu double
        /// Neu ==0 => tra ve khi tu mac dinh
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="culture">The culture.</param>
        /// <param name="digits"></param>
        /// <param name="defaultChar">The default char.</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// TRAN QUANG TRUNG  15/3/2013   created
        /// </Modified>
        public static string FormatDoubleChar(object obj, CultureInfo culture, int digits, string defaultChar)
        {
            string ret = defaultChar;
            if (obj == null) return ret;
            try
            {
                ret = (!Convert.ToDouble(obj).Equals(0)) ? Math.Round(Convert.ToDouble(obj), digits, MidpointRounding.AwayFromZero).ToString(FormatDoubleString, culture) : defaultChar;
            }
            catch
            {
                ret = defaultChar;
            }
            return ret;
        }

        /// <summary>
        /// Format từ một số phút kiểu int sang định dạng HH:mm
        /// </summary>
        /// <param name="minutes"></param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// Sonnl  14-04-2015   created
        /// </Modified>
        public static string FormatHourMinuteStr(int minutes)
        {
            if (minutes < 0) return string.Empty;

            var hourPart = minutes / 60;
            var minPart = minutes % 60;

            var hourPartStr = hourPart.ToString();
            var minPartStr = minPart.ToString();

            if (hourPart < 10)
            {
                hourPartStr = "0" + hourPartStr;
            }

            if (minPart < 10)
            {
                minPartStr = "0" + minPartStr;
            }

            return hourPartStr + ":" + minPartStr;
        }

        public static string FormatJson(string json)
        {
            const string indentString = "    ";
            var indent = 0;
            var quoted = false;
            var sb = new StringBuilder();
            for (var i = 0; i < json.Length; i++)
            {
                var ch = json[i];
                switch (ch)
                {
                    case '{':
                    case '[':
                        sb.Append(ch);
                        if (!quoted)
                        {
                            sb.AppendLine();
                            Enumerable.Range(0, ++indent).ToList().ForEach(item => sb.Append(indentString));
                        }

                        break;

                    case '}':
                    case ']':
                        if (!quoted)
                        {
                            sb.AppendLine();
                            Enumerable.Range(0, --indent).ToList().ForEach(item => sb.Append(indentString));
                        }

                        sb.Append(ch);
                        break;

                    case '"':
                        sb.Append(ch);
                        var escaped = false;
                        var index = i;
                        while (index > 0 && json[--index] == '\\')
                            escaped = !escaped;
                        if (!escaped)
                            quoted = !quoted;
                        break;

                    case ',':
                        sb.Append(ch);
                        if (!quoted)
                        {
                            sb.AppendLine();
                            Enumerable.Range(0, indent).ToList().ForEach(item => sb.Append(indentString));
                        }

                        break;

                    case ':':
                        sb.Append(ch);
                        if (!quoted)
                            sb.Append(" ");
                        break;

                    default:
                        sb.Append(ch);
                        break;
                }
            }

            return sb.ToString();
        }
    }
}