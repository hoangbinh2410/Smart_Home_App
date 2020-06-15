using System.Globalization;

namespace BA_MobileGPS.Core
{
    /// <summary>
    /// Helper chứa các hàm tiện ích dùng chung
    /// </summary>
    /// <Modified>
    /// Name     Date         Comments
    /// Namth  21/07/2014   created
    /// </Modified>
    public class GeneralHelper
    {
        /// <summary>
        /// Convert String -> Double.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// Namth  6/6/2012   created
        /// </Modified>
        public static double ConvertToDouble(string input, CultureInfo culture)
        {
            double ret = 0;
            double.TryParse(input, NumberStyles.Float, culture, out ret);
            return ret;
        }

        /// <summary>
        /// Convert String -> Double.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// Namth  6/6/2012   created
        /// </Modified>
        public static double ConvertToDouble(string input)
        {
            return ConvertToDouble(input, new CultureInfo("en-US"));
        }

        /// <summary>
        /// Kiểm tra tính hợp lệ của kinh độ, vĩ độ thuộc vùng biên giới Lào, Campuchia, Việt nam
        /// </summary>
        /// <param name="Longitude">Kinh do</param>
        /// <param name="Latitude">Vi do</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// tuyenvt  18/06/2012   created
        /// </Modified>
        public static bool IsValidVietNamCoordinate(double longitude, double latitude, bool enableValidate)
        {
            // Nếu bật cần check tọa độ trong phạm vi Việt Nam
            if (enableValidate)
            {
                if (!(latitude == 0 || longitude == 0))
                {
                    if (longitude < 101.801062 || longitude > 109.636230) return false;
                    if (latitude < 8.285289 || latitude > 23.545763) return false;
                }
            }
            return true;
        }
    }
}