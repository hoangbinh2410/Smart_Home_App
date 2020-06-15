using BA_MobileGPS.Utilities;

using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BA_MobileGPS.Entities
{
    public class Message
    {
        private static readonly object LockObj = new object();

        /// <summary>
        /// Chuyển đổi dữ liệu nhận về thành string
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ConvertStreamToArray(Stream data)
        {
            lock (LockObj)
            {
                long FileLength = 0;
                byte[] readBuffer = new byte[4096];
                int bytesRead;
                List<byte> ListData = new List<byte>();

                //Chuyển toàn bộ ảnh vào buffer
                while ((bytesRead = data.Read(readBuffer, 0, 4096)) > 0)
                {
                    FileLength += bytesRead;
                    if (FileLength >= Constants.MAXDATAFROMCLIENT)
                        break;

                    for (int i = 0; i < bytesRead; i++)
                        ListData.Add(readBuffer[i]);
                }

                //Chuyển đổi dữ liệu về string
                return Encoding.UTF8.GetString(ListData.ToArray(), 0, ListData.Count);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="RequestContent"></param>
        /// <returns></returns>
        public static byte[] DecodeMessage(string RequestContent)
        {
            lock (LockObj)
            {
                var Result = StaxiCrypt.HTTPCoding.DeCode(RequestContent);
                return Result;
            }
        }

        /// <summary>
        /// Chuyển đổi mảng byte thành
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string EncodeMessage(byte[] data)
        {
            lock (LockObj)
            {
                return StaxiCrypt.HTTPCoding.EnCode(data);
            }
        }
    }
}