using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BA_MobileGPS.Entities
{
    public static class SerializeLibrary
    {
        //Chuyển đổi kiểu Int16 ---------------------------------------------------------------------------------------------------
        public static byte[] ConvertInt16ToArray(short value)
        {
            return new byte[] { (byte)(value & 0xff), (byte)((value & 0xff00) >> 8) };
        }

        public static short GetInt16FromArray(byte firstByte, byte secondByte)
        {
            return (short)(((secondByte << 8) & 0xff00) | (firstByte & 0xff));
        }

        public static short GetInt16FromArray(byte[] inputData, int index)
        {
            return (short)(((inputData[index + 1] << 8) & 0xff00) | (inputData[index] & 0xff));
        }

        //Chuyển đổi kiểu Int32 ----------------------------------------------------------------------------------------------------
        public static byte[] ConvertInt32ToArray(int value)
        {
            return BitConverter.GetBytes(value);
        }

        public static int GetInt32FromArray(byte[] inputData, int index)
        {
            return inputData[index] + inputData[index + 1] * 256 + inputData[index + 2] * 65536 + inputData[index + 3] * 16777216;
        }

        // Chuyển đổi kiểu string ---------------------------------------------------------------------------------------------------
        public static byte[] ConvertStringToArray(string value)
        {
            if (value == null) value = string.Empty;

            var bytes = System.Text.Encoding.UTF8.GetBytes(value);

            var length = (short)bytes.Length;
            if (length == 0) return new byte[] { 0, 0 };
            else
            {
                var lengthBytes = ConvertInt16ToArray(length);
                return lengthBytes.Concat(bytes).ToArray();
            }
        }

        public static string GetStringFromArray(byte[] inputData, int index, ref int len)
        {
            try
            {
                short Length = GetInt16FromArray(inputData[index], inputData[index + 1]);
                List<byte> tmp = new List<byte>();

                for (int i = index + 2; i < index + 2 + Length; i++)
                    tmp.Add(inputData[i]);

                len = index + Length + 2;
                return Encoding.UTF8.GetString(tmp.ToArray());
            }
            catch
            {
            }

            len = index + 2;
            return string.Empty;
        }

        // Chuyển đổi kiểu mảng int -------------------------------------------------------------------------------------------------------
        public static byte[] ConvertListIntToArray(List<int> value)
        {
            //Mảng 4 byte
            var bytes = value.Count;

            var length = (short)bytes;
            if (length == 0) return new byte[] { 0, 0 };
            else
            {
                var lengthBytes = ConvertInt16ToArray(length);

                var Result = new List<byte>();
                Result.AddRange(lengthBytes);

                foreach (var item in value)
                    Result.AddRange(ConvertInt32ToArray(item));

                return Result.ToArray();
            }
        }

        // Chuyển đổi số long ---------------------------------------------------------------------------------------------------------------
        public static byte[] ConvertLongToArray(long value)
        {
            return BitConverter.GetBytes(value);
        }

        public static long GetLongFromArray(byte[] inputData, int index)
        {
            return BitConverter.ToInt64(inputData, index);
        }

        // Chuyển đổi số float --------------------------------------------------------------------------------------------------------------
        public static byte[] ConvertFloatToArray(float value)
        {
            return BitConverter.GetBytes(value);
        }

        public static float GetFloatFromArray(byte[] inputData, int index)
        {
            float result = 0;

            byte[] dataArr = new byte[4];

            dataArr[0] = inputData[index];
            dataArr[1] = inputData[index + 1];
            dataArr[2] = inputData[index + 2];
            dataArr[3] = inputData[index + 3];
            // Chuyển sang kiểu float
            result = BitConverter.ToSingle(dataArr, 0);

            return result;
        }

        // Chuyển đổi thời gian ------------------------------------------------------------------------------------------------------------
        private static readonly long MinTimeTick = DateTime.Parse("01/01/1970 00:00:00").Ticks;

        public static byte[] ConvertDateTimeToArray(DateTime time)
        {
            long dt = 0;

            if (time == DateTime.MinValue)
            {
                dt = 0;
                return BitConverter.GetBytes(dt);
            }

            long ticks = time.Ticks - MinTimeTick;
            ticks /= 10000000; //Convert windows ticks to seconds
            dt = long.Parse(ticks.ToString());
            return BitConverter.GetBytes(dt);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static DateTime GetDateTimeFromLong(long timeValue)
        {
            if (timeValue == 0) return DateTime.MinValue;
            long ticks = (timeValue * 10000000) + MinTimeTick;
            return new DateTime(ticks);
        }

        public static DateTime GetDateTimeFromArray(byte[] msg, int index)
        {
            return GetDateTimeFromLong(GetLongFromArray(msg, index));
        }

        // Copy một mảng vào trong dữ liệu ------------------------------------------------------------------------------------------------
        public static byte[] ConvertListByteToArray(int count, List<byte> data)
        {
            if (data.Count == 0) return new byte[] { 0, 0 };
            var lengthBytes = ConvertInt16ToArray((short)count);

            var Result = new List<byte>();
            Result.AddRange(lengthBytes);
            Result.AddRange(data);

            return Result.ToArray();
        }

        //Chuyển số byte ------------------------------------------------------------------------------------------------------------------
        public static byte[] ConvertByteToArray(byte value)
        {
            return new byte[] { value };
        }

        public static int GetByteFromArray(byte[] inputData, int index)
        {
            return inputData[index];
        }

        // Chuyển đổi số bool -------------------------------------------------------------------------------------------------------------
        public static byte[] ConvertBoolToArray(bool value)
        {
            return new byte[] { (byte)(value ? 1 : 0) };
        }

        // Chuyển đổi số double -----------------------------------------------------------------------------------------------------------
        public static byte[] ConvertDoubleToArray(double value)
        {
            return BitConverter.GetBytes(value);
        }

        public static double GetDoubleFromArray(byte[] inputData, int index)
        {
            return BitConverter.ToDouble(inputData, index);
        }
    }
}