using System.Collections.Generic;

namespace BA_MobileGPS.Entities
{
    public class Converter
    {
        /// <summary>
        /// Chuyển đổi
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public static string ConvertByteArrayToString(byte[] Data)
        {
            string Result = string.Empty;

            foreach (var item in Data)
                Result += item.ToString() + " ";

            return Result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static List<byte> ConvertToByteArray(GSMPoint data)
        {
            try
            {
                var Result = new List<byte>();

                Result.AddRange(SerializeLibrary.ConvertInt32ToArray(data.StartPoint));
                Result.AddRange(SerializeLibrary.ConvertInt32ToArray(data.EndPoint));
                Result.AddRange(SerializeLibrary.ConvertDateTimeToArray(data.StartTime));
                Result.AddRange(SerializeLibrary.ConvertInt32ToArray(data.Duration));

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

        public static List<byte> ConvertToByteArray(StatePoint data)
        {
            try
            {
                var Result = new List<byte>();

                Result.AddRange(SerializeLibrary.ConvertInt32ToArray(data.StartIndex));
                Result.AddRange(SerializeLibrary.ConvertInt32ToArray(data.EndIndex));
                Result.AddRange(SerializeLibrary.ConvertByteToArray((byte)data.State));
                Result.AddRange(SerializeLibrary.ConvertDateTimeToArray(data.StartTime));
                Result.AddRange(SerializeLibrary.ConvertInt32ToArray(data.Duration));

                return Result;
            }
            catch
            { }

            return new List<byte>();
        }

        public static RouteHistoryResponse ConvertToRouteHistory()
        {
            //if (data != null)
            //{
            //    ExtendedByteBuffer byteBuffer = ExtendedByteBuffer.wrap(data);
            //    reponse = new GetRouteHistoryReponseModel();

            //    reponse.dateKm = byteBuffer.getFloat();
            //    reponse.directionDetail = byteBuffer.getShortString();

            //    // Danh sách điểm dừng
            //    int lengthStopPoint = byteBuffer.getShort();
            //    reponse.stopPointList = new ArrayList<StopPoint>();
            //    for (int i = 0; i < lengthStopPoint; i++)
            //    {
            //        reponse.stopPointList.add(StopPoint.parseStopPoint(byteBuffer));
            //    }

            //    // Danh sách điểm GSM
            //    int lengthGSMPoint = byteBuffer.getShort();
            //    reponse.gsmPointList = new ArrayList<GSMPoint>();
            //    for (int j = 0; j < lengthGSMPoint; j++)
            //    {
            //        reponse.gsmPointList.add(GSMPoint.parseGSMPoint(byteBuffer));
            //    }

            //    // Danh sách điểm Velocity
            //    int lengthVelocityPoint = byteBuffer.getShort();
            //    reponse.velocityPoints = new ArrayList<VelocityPoint>();
            //    for (int j = 0; j < lengthVelocityPoint; j++)
            //    {
            //        reponse.velocityPoints.add(VelocityPoint.parseVelocityPoint(byteBuffer));
            //    }

            //    reponse.timePoint = TimePoint.parseTimePoint(byteBuffer);
            //}

            return null;
        }
    }
}