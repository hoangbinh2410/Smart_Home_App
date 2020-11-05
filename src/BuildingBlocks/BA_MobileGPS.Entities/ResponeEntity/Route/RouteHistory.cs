using BA_MobileGPS.Utilities;

using System;
using System.Collections.Generic;
using System.Reflection;

namespace BA_MobileGPS.Entities
{
    public class RouteHistoryResponse
    {
        public float DateKm { set; get; }

        public string DirectionDetail { set; get; }

        public List<StatePoint> StatePoints { set; get; }

        public List<GSMPoint> GSMPoints { set; get; }

        public List<byte> VelocityPoints { set; get; }

        public TimePoint TimePoints { set; get; }

        public RouteHistoryResponse()
        {
            StatePoints = new List<StatePoint>();
            GSMPoints = new List<GSMPoint>();
            VelocityPoints = new List<byte>();
            TimePoints = new TimePoint();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public List<byte> ConvertToByteArray()
        {
            try
            {
                var Result = new List<byte>();

                Result.AddRange(SerializeLibrary.ConvertFloatToArray(DateKm));
                Result.AddRange(SerializeLibrary.ConvertStringToArray(DirectionDetail));

                List<byte> tmp = new List<byte>();
                foreach (var item in StatePoints)
                    tmp.AddRange(Converter.ConvertToByteArray(item));
                Result.AddRange(SerializeLibrary.ConvertListByteToArray(StatePoints.Count, tmp));

                tmp = new List<byte>();
                foreach (var item in GSMPoints)
                    tmp.AddRange(Converter.ConvertToByteArray(item));
                Result.AddRange(SerializeLibrary.ConvertListByteToArray(GSMPoints.Count, tmp));

                Result.AddRange(SerializeLibrary.ConvertListByteToArray(VelocityPoints.Count, VelocityPoints));
                Result.AddRange(TimePoints.ConvertToByteArray());

                return Result;
            }
            catch
            { }

            return new List<byte>();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public bool FromByteArray(byte[] data)
        {
            try
            {
                int Index = 0;

                DateKm = SerializeLibrary.GetFloatFromArray(data, Index); Index += 4;
                DirectionDetail = SerializeLibrary.GetStringFromArray(data, Index, ref Index);

                short lengthStopPoint = SerializeLibrary.GetInt16FromArray(data, Index); Index += 2;
                StatePoints = new List<StatePoint>();
                for (int i = 0; i < lengthStopPoint; i++)
                {
                    var state = new StatePoint();
                    state.StartIndex = SerializeLibrary.GetInt32FromArray(data, Index); Index += 4;
                    state.EndIndex = SerializeLibrary.GetInt32FromArray(data, Index); Index += 4;
                    state.State = (StateType)data[Index]; Index += 1;
                    state.StartTime = SerializeLibrary.GetDateTimeFromArray(data, Index); Index += 8;
                    state.Duration = SerializeLibrary.GetInt32FromArray(data, Index); Index += 4;
                    StatePoints.Add(state);
                }

                short lengthGSMPoint = SerializeLibrary.GetInt16FromArray(data, Index); Index += 2;
                GSMPoints = new List<GSMPoint>();
                for (int i = 0; i < lengthGSMPoint; i++)
                {
                    var gsm = new GSMPoint();
                    gsm.StartPoint = SerializeLibrary.GetInt32FromArray(data, Index); Index += 4;
                    gsm.EndPoint = SerializeLibrary.GetInt32FromArray(data, Index); Index += 4;
                    gsm.StartTime = SerializeLibrary.GetDateTimeFromArray(data, Index); Index += 8;
                    gsm.Duration = SerializeLibrary.GetInt32FromArray(data, Index); Index += 4;
                    GSMPoints.Add(gsm);
                }

                short lengthVelocityPoints = SerializeLibrary.GetInt16FromArray(data, Index); Index += 2;
                VelocityPoints = new List<byte>();
                for (int i = 0; i < lengthVelocityPoints; i++)
                {
                    byte velocity = (byte)SerializeLibrary.GetByteFromArray(data, Index); Index += 1;
                    VelocityPoints.Add(velocity);
                }

                TimePoints = new TimePoint
                {
                    StartTime = SerializeLibrary.GetDateTimeFromArray(data, Index)
                };
                Index += 8;

                short lengthAddedTimes = SerializeLibrary.GetInt16FromArray(data, Index); Index += 2;
                TimePoints.AddedTimes = new List<int>();
                for (int i = 0; i < lengthVelocityPoints; i++)
                {
                    TimePoints.AddedTimes.Add(SerializeLibrary.GetInt32FromArray(data, Index));
                    Index += 4;
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public bool FromByteArray32(byte[] data)
        {
            try
            {
                int Index = 0;

                DateKm = SerializeLibrary.GetFloatFromArray(data, Index); Index += 4;
                DirectionDetail = SerializeLibrary.GetStringFromArray32(data, Index, ref Index);

                int lengthStopPoint = SerializeLibrary.GetInt32FromArray(data, Index); Index += 4;
                StatePoints = new List<StatePoint>();
                for (int i = 0; i < lengthStopPoint; i++)
                {
                    var state = new StatePoint();
                    state.StartIndex = SerializeLibrary.GetInt32FromArray(data, Index); Index += 4;
                    state.EndIndex = SerializeLibrary.GetInt32FromArray(data, Index); Index += 4;
                    state.State = (StateType)data[Index]; Index += 1;
                    state.StartTime = SerializeLibrary.GetDateTimeFromArray(data, Index); Index += 8;
                    state.Duration = SerializeLibrary.GetInt32FromArray(data, Index); Index += 4;
                    StatePoints.Add(state);
                }

                int lengthGSMPoint = SerializeLibrary.GetInt32FromArray(data, Index); Index += 4;
                GSMPoints = new List<GSMPoint>();
                for (int i = 0; i < lengthGSMPoint; i++)
                {
                    var gsm = new GSMPoint();
                    gsm.StartPoint = SerializeLibrary.GetInt32FromArray(data, Index); Index += 4;
                    gsm.EndPoint = SerializeLibrary.GetInt32FromArray(data, Index); Index += 4;
                    gsm.StartTime = SerializeLibrary.GetDateTimeFromArray(data, Index); Index += 8;
                    gsm.Duration = SerializeLibrary.GetInt32FromArray(data, Index); Index += 4;
                    GSMPoints.Add(gsm);
                }

                int lengthVelocityPoints = SerializeLibrary.GetInt32FromArray(data, Index); Index += 4;
                VelocityPoints = new List<byte>();
                for (int i = 0; i < lengthVelocityPoints; i++)
                {
                    byte velocity = (byte)SerializeLibrary.GetByteFromArray(data, Index); Index += 1;
                    VelocityPoints.Add(velocity);
                }

                TimePoints = new TimePoint
                {
                    StartTime = SerializeLibrary.GetDateTimeFromArray(data, Index)
                };
                Index += 8;

                int lengthAddedTimes = SerializeLibrary.GetInt32FromArray(data, Index); Index += 4;
                TimePoints.AddedTimes = new List<int>();
                for (int i = 0; i < lengthVelocityPoints; i++)
                {
                    TimePoints.AddedTimes.Add(SerializeLibrary.GetInt32FromArray(data, Index));
                    Index += 4;
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }

            return false;
        }
    }

    public class RouteHistoryRequest
    {
        public Guid UserId { set; get; }

        public int CompanyId { set; get; }

        public string VehiclePlate { set; get; }

        public DateTime FromDate { set; get; }

        public DateTime ToDate { set; get; }

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

                UserId = Guid.Parse(SerializeLibrary.GetStringFromArray(data, Index, ref Index));
                VehiclePlate = SerializeLibrary.GetStringFromArray(data, Index, ref Index);
                FromDate = SerializeLibrary.GetDateTimeFromArray(data, Index); Index += 8;
                ToDate = SerializeLibrary.GetDateTimeFromArray(data, Index); Index += 8;
                CompanyId = SerializeLibrary.GetInt32FromArray(data, Index); Index += 4;

                return true;
            }
            catch
            {
            }

            return false;
        }
    }
}