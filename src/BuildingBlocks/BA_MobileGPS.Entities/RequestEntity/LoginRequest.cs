using System;
using System.Collections.Generic;

namespace BA_MobileGPS.Entities
{
    public class LoginRequest
    {
        public string UserName { set; get; }

        public string Password { set; get; }

        public AppType AppType { set; get; }

        /// <summary>
        /// Chuyển đổi bản tin login sang mảng byte
        /// </summary>
        /// <returns></returns>
        public List<byte> ConvertToByteArray()
        {
            try
            {
                var Result = new List<byte>();

                Result.AddRange(SerializeLibrary.ConvertStringToArray(UserName));
                Result.AddRange(SerializeLibrary.ConvertStringToArray(Password));
                Result.AddRange(SerializeLibrary.ConvertByteToArray((byte)AppType));

                return Result;
            }
            catch
            { }

            return new List<byte>();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool FromByteArray(byte[] data)
        {
            try
            {
                int Index = 0;
                UserName = SerializeLibrary.GetStringFromArray(data, Index, ref Index);
                Password = SerializeLibrary.GetStringFromArray(data, Index, ref Index);
                AppType = (AppType)data[Index]; Index += 1;
                return true;
            }
            catch (Exception)
            {
                //BA.Utilities.LogExceptions.WriteLog("DecodeLogin", 0, $"{UserName} - {Password} - {Converter.ConvertByteArrayToString(data)}", ex);
            }

            return false;
        }
    }
}