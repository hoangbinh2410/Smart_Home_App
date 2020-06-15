using System;
using System.Collections.Generic;

namespace BA_MobileGPS.Entities
{
    public class TimePoint
    {
        public DateTime StartTime { set; get; }

        public List<int> AddedTimes { set; get; }

        public TimePoint()
        {
            AddedTimes = new List<int>();
        }

        public List<byte> ConvertToByteArray()
        {
            try
            {
                var Result = new List<byte>();

                Result.AddRange(SerializeLibrary.ConvertDateTimeToArray(StartTime));
                Result.AddRange(SerializeLibrary.ConvertListIntToArray(AddedTimes));

                return Result;
            }
            catch
            { }

            return new List<byte>();
        }
    }
}