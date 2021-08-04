using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;

using System;
using System.Collections.Generic;

namespace BA_MobileGPS.Core.Extensions
{
    public static class StateVehicleExtension
    {
        /*
	 * Trạng thái bật máy
	 *
	 * @return isEngineOn
	 */

        public static bool IsEngineOn(int state)
        {
            return (state & 8) == 0;
        }

        /* Trả về trạng thái động cơ */

        public static string EngineStateConverter(int state)
        {
            if (!IsEngineOn(state))
            {
                return MobileResource.Common_Label_TurnOff;
            }
            else
            {
                return MobileResource.Common_Label_TurnOn;
            }
        }

        /*
	 * Trạng thái tắt máy
	 *
	 * @return isEngineOff
	 */

        public static bool IsEngineOff(int state)
        {
            return (state & 8) > 0;
        }

        /*
	 * Trạng thái đóng cửa/hạ ben
	 *
	 * @return isDoorClose
	 */

        public static bool IsDoorClose(int state)
        {
            return (state & 16) == 0;
        }

        /*
	 * Trạng thái mở cửa
	 *
	 * @return isDoorOpen
	 */

        public static bool IsDoorOpen(int state)
        {
            return (state & 16) > 0;
        }

        /* Trả về trạng thái đóng/mở cửa */

        public static string DoorOpen(int state)
        {
            if (!IsDoorOpen(state))
            {
                return MobileResource.Common_Label_DoorClose;
            }
            else
            {
                return MobileResource.Common_Label_DoorOpen;
            }
        }

        /*
	 * Trạng thái bật điều hòa
	 *
	 * @return isAirConditionerOn
	 */

        public static bool IsAirConditionerOn(int state)
        {
            return (state & 32) == 0;
        }

        /* Trả về trạng thái điều hòa */

        public static string AirConditionerOn(int state)
        {
            if (!IsAirConditionerOn(state))
            {
                return MobileResource.Common_Label_TurnOff;
            }
            else
            {
                return MobileResource.Common_Label_TurnOn;
            }
        }

        /*
         * Trạng thái tắt điều hòa
         *
         * @return isAirConditionerOff
         */

        public static bool IsAirConditionerOff(int state)
        {
            return (state & 32) > 0;
        }

        /*
         * Trạng thái trộn bê tông
         *
         * @return isMixingConcrete
         */

        public static bool IsMixingConcrete(int state)
        {
            return (state & 49152) == 16384;
        }

        /*
         * Trạng thái xả bê tông
         *
         * @return isDisposingConcrete
         */

        public static bool IsDisposingConcrete(int state)
        {
            return (state & 49152) == 32768;
        }

        /*
         * Trạng thái dừng bê tông
         *
         * @return isStoppedConcrete
         */

        public static bool IsStoppedConcrete(int state)
        {
            return (state & 49152) != 32768 || (state & 49152) != 16384;
        }

        /* Trả về trạng thái bê tông */

        public static string ConcreteState(int state)
        {
            if (IsMixingConcrete(state))
            {
                return MobileResource.DetailVehicle_Label_ValueConcrete_Mixer;
            }
            else if (IsDisposingConcrete(state))
            {
                return MobileResource.DetailVehicle_Label_ValueConcrete_Concreting;
            }
            else if (IsStoppedConcrete(state))
            {
                return MobileResource.DetailVehicle_Label_ValueConcrete_Normal;
            }
            else
            {
                return MobileResource.DetailVehicle_Label_ValueConcrete_Normal;
            }
        }

        /*
         * Bắt đầu chuyến đưa rước
         *
         * @return isOnTrip
         */

        public static bool IsOnTrip(int state)
        {
            return (state & 1024) > 0;
        }

        /*
         * Kết thúc chuyến đưa rước
         *
         * @return isTripOff
         */

        public static bool IsTripOff(int state)
        {
            return (state & 1024) == 0;
        }

        /*
       * Di chuyển
       *
       * @return isOnTrip
       */

        public static bool IsMoving(int velocity)
        {
            return (velocity > CompanyConfigurationHelper.Vmin) ? true : false;
        }

        /*
       * Dừng đỗ
       *
       * @return isOnTrip
       */

        public static bool IsStoping(int velocity)
        {
            return (velocity < CompanyConfigurationHelper.Vmin) ? true : false;
        }

        /*
      * Bắt đầu chuyến đưa rước
      *
      * @return isOnTrip
      */

        public static bool IsOverVelocity(int velocity)
        {
            return (velocity > CompanyConfigurationHelper.DefaultMaxVelocityBlue) ? true : false;
        }

        /* Xe nợ phí */

        public static bool IsVehicleDebtMoney(int messageID, int DataExt)
        {
            return (messageID == 2 || messageID == 3 || (messageID == 128 && (DataExt & 5) > 0)) ? true : false;
        }

        public static bool IsVehicleDebtMoneyViview(int messageID, int DataExt)
        {
            return (messageID == 1 || messageID == 2 || messageID == 3 || (messageID == 128 && (DataExt & 5) > 0)) ? true : false;
        }

        /* Xe dừng dịch vụ */

        public static bool IsVehicleStopService(int messageID)
        {
            return (messageID == 128 || messageID == 3) ? true : false;
        }

        /* Trạng thái gms */

        public static bool IsLostGSM(DateTime vehicleTime)
        {
            if (StaticSettings.TimeServer.Subtract(vehicleTime).TotalMinutes > CompanyConfigurationHelper.DefaultTimeLossConnect)//Nếu xe mất GMS
            {
                return true;
            }
            return false;
        }

        /* Trạng thái gps */

        public static bool IsLostGPS(DateTime gpstime, DateTime vehicleTime)
        {
            if (vehicleTime.Subtract(gpstime).TotalMinutes >= CompanyConfigurationHelper.DefaultMinTimeLossGPS)//Nếu xe mất GPS
            {
                return true;
            }
            return false;
        }

        public static bool IsVehicleUpdate(DateTime gpstime, DateTime vehicleTime)
        {
            //nếu thời gian hiện tại - thời gian của xe mà lớn hơn 2 thì update xe đó
            var time = StaticSettings.TimeServer.Subtract(vehicleTime).TotalMinutes;
            if (time >= CompanyConfigurationHelper.TimeVehicleSync)//Nếu xe mất GPS
            {
                return true;
            }
            return false;
        }

        public static List<long> GetVehicleSyncData(List<VehicleOnline> mVehicleList)
        {
            var result = new List<long>();
            if (mVehicleList != null && mVehicleList.Count > 0)
            {
                mVehicleList.ForEach(x =>
                {
                    if (IsVehicleUpdate(x.GPSTime, x.VehicleTime))
                    {
                        result.Add(x.VehicleId);
                    }
                });
            }
            return result;
        }

        public static List<VehicleOnline> GetVehicleLostGPSAndLostGSM()
        {
            var result = new List<VehicleOnline>();
            if (StaticSettings.ListVehilceOnline != null && StaticSettings.ListVehilceOnline.Count > 0)
            {
                StaticSettings.ListVehilceOnline.ForEach(x =>
                {
                    if (IsLostGPS(x.GPSTime, x.VehicleTime) || IsLostGSM(x.VehicleTime))
                    {
                        result.Add(x);
                    }
                });
            }
            return result;
        }

        /* Trạng thái gps */

        public static bool IsLostGPSIcon(DateTime gpstime, DateTime vehicleTime)
        {
            double time = vehicleTime.Subtract(gpstime).TotalMinutes;
            if (CompanyConfigurationHelper.DefaultMinTimeLossGPS < time
                && time <= CompanyConfigurationHelper.DefaultMaxTimeLossGPS)//Nếu xe mất GPS
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// trạng thái  xe dừng đỗ tắt máy
        /// </summary>
        /// <param name="vehicle"></param>
        /// <returns></returns>
        public static bool IsStopAndEngineOff(VehicleOnline vehicle)
        {
            //xe dừng đỗ là xe không mất GSM ,GPS
            if (!IsLostGPS(vehicle.GPSTime, vehicle.VehicleTime) && !IsLostGSM(vehicle.VehicleTime))
            {
                //Nếu xe không cấu hình acc thì dựa vào trạng thái máy là tắt máy thì là dừng đỗ tắt máy
                if (!vehicle.IsEnableAcc && IsEngineOff(vehicle.State))
                {
                    return true;
                }
                //nếu xe có cấu hình sai acc thì dựa vào vận tốc
                else if (vehicle.IsEnableAcc && IsStoping(vehicle.Velocity))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsStopAndEngineOn(VehicleOnline vehicle)
        {
            //xe dừng đỗ là xe không mất GSM ,GPS
            if (!IsLostGPS(vehicle.GPSTime, vehicle.VehicleTime) && !IsLostGSM(vehicle.VehicleTime))
            {
                //Nếu xe không cấu hình acc thì dựa vào trạng thái máy là bật máy thì là dừng đỗ bật máy
                if (!vehicle.IsEnableAcc && IsEngineOn(vehicle.State) && IsStoping(vehicle.Velocity))
                {
                    return true;
                }
                //nếu xe có cấu hình sai acc thì dựa vào vận tốc
                else if (vehicle.IsEnableAcc && IsStoping(vehicle.Velocity) && IsEngineOn(vehicle.State))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// trạng thái  xe di chuyển bật máy
        /// </summary>
        /// <param name="vehicle"></param>
        /// <returns></returns>
        public static bool IsMovingAndEngineON(VehicleOnline vehicle)
        {
            //xe dừng đỗ là xe không mất GSM ,GPS
            if (!IsLostGPS(vehicle.GPSTime, vehicle.VehicleTime) && !IsLostGSM(vehicle.VehicleTime))
            {
                //Nếu xe không cấu hình acc thì dựa vào trạng thái máy là tắt máy thì là dừng đỗ
                if (!vehicle.IsEnableAcc && IsEngineOn(vehicle.State))
                {
                    return true;
                }
                //nếu xe có cấu hình sai acc thì dựa vào vận tốc
                else if (vehicle.IsEnableAcc && IsMoving(vehicle.Velocity))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// trạng thái  xe di chuyển bật máy
        /// </summary>
        /// <param name="vehicle"></param>
        /// <returns></returns>
        public static bool IsEngineState(VehicleOnline vehicle)
        {
            //Nếu xe không cấu hình acc thì dựa vào trạng thái máy là tắt máy thì là dừng đỗ
            if (!vehicle.IsEnableAcc && IsEngineOn(vehicle.State))
            {
                return true;
            }
            //nếu xe có cấu hình sai acc thì dựa vào vận tốc
            else if (vehicle.IsEnableAcc && IsMoving(vehicle.Velocity))
            {
                return true;
            }
            return false;
        }

        /* Trả về trạng thái động cơ */

        public static string EngineState(VehicleOnline vehicle)
        {
            if (!IsEngineState(vehicle))
            {
                return MobileResource.Common_Label_TurnOff;
            }
            else
            {
                return MobileResource.Common_Label_TurnOn;
            }
        }

        /// <summary>
        /// trạng thái  xe di chuyển
        /// </summary>
        /// <param name="vehicle"></param>
        /// <returns></returns>
        public static bool IsMoving(VehicleOnline vehicle)
        {
            //xe dừng đỗ là xe không mất GSM ,GPS
            if (!IsLostGPS(vehicle.GPSTime, vehicle.VehicleTime) && !IsLostGSM(vehicle.VehicleTime))
            {
                //Nếu xe không cấu hình acc thì dựa vào trạng thái máy là tắt máy thì là dừng đỗ
                if (!vehicle.IsEnableAcc && IsEngineOn(vehicle.State) && IsMoving(vehicle.Velocity))
                {
                    return true;
                }
                //nếu xe có cấu hình sai acc thì dựa vào vận tốc
                else if (vehicle.IsEnableAcc && IsMoving(vehicle.Velocity))
                {
                    return true;
                }
            }
            return false;
        }

        /*
        * Bắt đầu chuyến đưa rước
        *
        * @return isOnTrip
        */

        public static bool IsOverVelocity(VehicleOnline vehicle)
        {
            //xe dừng đỗ là xe không mất GSM ,GPS
            if (!IsLostGPS(vehicle.GPSTime, vehicle.VehicleTime) && !IsLostGSM(vehicle.VehicleTime))
            {
                return (vehicle.Velocity > CompanyConfigurationHelper.DefaultMaxVelocityBlue) ? true : false;
            }
            return false;
        }

        public static bool IsOverVelocityRoute(int velocity)
        {
            return (velocity > CompanyConfigurationHelper.DefaultMaxVelocityBlue) ? true : false;
        }

        /// <summary>
        /// Lọc theo trạng thái.
        /// </summary>
        /// <param name="v">The v.</param>
        /// <param name="status">The status.</param>
        /// <returns>
        ///   <c>true</c> if [is visible vehicle] [the specified v]; otherwise, <c>false</c>.
        /// </returns>
        /// Name     Date         Comments
        /// TruongPV  1/9/2019   created
        /// </Modified>
        public static int GetCountCarByStatus(List<VehicleOnline> mVehicleList, VehicleStatusGroup status)
        {
            var result = 0;
            if (mVehicleList != null && mVehicleList.Count > 0)
            {
                mVehicleList.ForEach(x =>
                {
                    switch (status)
                    {
                        case VehicleStatusGroup.All:
                            result = mVehicleList.Count;
                            break;

                        case VehicleStatusGroup.EngineOn:
                            if (IsMovingAndEngineON(x))
                            {
                                result += 1;
                            }
                            break;

                        case VehicleStatusGroup.EngineOFF:
                            if (IsStopAndEngineOff(x))
                            {
                                result += 1;
                            }
                            break;

                        case VehicleStatusGroup.Moving:
                            if (IsMoving(x))
                            {
                                result += 1;
                            }
                            break;

                        case VehicleStatusGroup.Stoping:
                            if (IsStopAndEngineOff(x))
                            {
                                result += 1;
                            }
                            break;

                        case VehicleStatusGroup.StopingOn:
                            if (IsStopAndEngineOn(x))
                            {
                                result += 1;
                            }
                            break;

                        case VehicleStatusGroup.OverVelocity:
                            if (IsOverVelocity(x))
                            {
                                result += 1;
                            }
                            break;

                        case VehicleStatusGroup.LostGPS:
                            // Hiện tại đôi với trường hợp xe mất GSM ==> Thời gian của GPS nó cũng không trả về nên nó cũng thỏa mãn điều kiện DateNow - GPSTime > x phút.
                            // Đối với mất GPS Nam thêm điều kiện không phải làm mất GSM nhé
                            if (IsLostGPS(x.GPSTime, x.VehicleTime) && !(IsLostGSM(x.VehicleTime) || StaticSettings.TimeServer.Subtract(x.GPSTime).TotalMinutes > CompanyConfigurationHelper.DefaultMaxTimeLossGPS))
                            {
                                result += 1;
                            }
                            break;

                        case VehicleStatusGroup.LostGSM:
                            if (IsLostGSM(x.VehicleTime))
                            {
                                result += 1;
                            }
                            break;

                        case VehicleStatusGroup.VehicleDebtMoney:
                            if (App.AppType == AppType.Viview)
                            {
                                if (IsVehicleDebtMoneyViview(x.MessageId, x.DataExt))
                                {
                                    result += 1;
                                }
                            }
                            else
                            {
                                if (IsVehicleDebtMoney(x.MessageId, x.DataExt))
                                {
                                    result += 1;
                                }
                            }
                            break;

                        case VehicleStatusGroup.SatelliteError:
                            if (App.AppType == AppType.VMS)
                            {
                                if (IsSatelliteError(x))
                                {
                                    result += 1;
                                }
                            }
                            break;
                    }
                });
            }
            return result;
        }

        /// <summary>
        /// Lọc theo trạng thái.
        /// </summary>
        /// <param name="v">The v.</param>
        /// <param name="status">The status.</param>
        /// <returns>
        ///   <c>true</c> if [is visible vehicle] [the specified v]; otherwise, <c>false</c>.
        /// </returns>
        /// Name     Date         Comments
        /// TruongPV  1/9/2019   created
        /// </Modified>
        public static List<VehicleOnline> GetVehicleCarByStatus(List<VehicleOnline> mVehicleList, VehicleStatusGroup status)
        {
            // Tạo danh sách mới lọc các xe theo trạng thái xe
            List<VehicleOnline> result = new List<VehicleOnline>();

            mVehicleList.ForEach(x =>
            {
                switch (status)
                {
                    case VehicleStatusGroup.All:
                        result = mVehicleList;
                        break;

                    case VehicleStatusGroup.EngineOn:
                        if (IsMovingAndEngineON(x))
                        {
                            result.Add(x);
                        }
                        break;

                    case VehicleStatusGroup.EngineOFF:
                        if (IsStopAndEngineOff(x))
                        {
                            result.Add(x);
                        }
                        break;

                    case VehicleStatusGroup.Moving:
                        if (IsMoving(x))
                        {
                            result.Add(x);
                        }
                        break;

                    case VehicleStatusGroup.Stoping:
                        if (IsStopAndEngineOff(x))
                        {
                            result.Add(x);
                        }
                        break;

                    case VehicleStatusGroup.StopingOn:
                        if (IsStopAndEngineOn(x))
                        {
                            result.Add(x);
                        }
                        break;

                    case VehicleStatusGroup.OverVelocity:
                        if (IsOverVelocity(x))
                        {
                            result.Add(x);
                        }
                        break;

                    case VehicleStatusGroup.LostGPS:
                        //Hiện tại đôi với trường hợp xe mất GSM ==> Thời gian của GPS nó cũng không trả về nên nó cũng thỏa mãn điều kiện DateNow - GPSTime > x phút.
                        //                      ===>Đối với mất GPS Nam thêm điều kiện không phải làm mất GSM nhé
                        if (IsLostGPS(x.GPSTime, x.VehicleTime) && !(IsLostGSM(x.VehicleTime) || StaticSettings.TimeServer.Subtract(x.GPSTime).TotalMinutes > CompanyConfigurationHelper.DefaultMaxTimeLossGPS))
                        {
                            result.Add(x);
                        }
                        break;

                    case VehicleStatusGroup.LostGSM:
                        if (IsLostGSM(x.VehicleTime))
                        {
                            result.Add(x);
                        }
                        break;

                    case VehicleStatusGroup.VehicleDebtMoney:
                        if (App.AppType == AppType.Viview)
                        {
                            if (IsVehicleDebtMoneyViview(x.MessageId, x.DataExt))
                            {
                                result.Add(x);
                            }
                        }
                        else
                        {
                            if (IsVehicleDebtMoney(x.MessageId, x.DataExt))
                            {
                                result.Add(x);
                            }
                        }
                        break;

                    case VehicleStatusGroup.SatelliteError:
                        if (App.AppType == AppType.VMS)
                        {
                            if (IsSatelliteError(x))
                            {
                                result.Add(x);
                            }
                        }
                        break;
                }
            });

            return result;
        }

        /*
    * Trạng thái Cẩu
    *
    * @return isCrane
    */

        public static bool IsCrane(int state)
        {
            return (state & 16) == 0;
        }

        /* Trả về trạng thái động cơ */

        public static string CraneState(int state)
        {
            if (!IsCrane(state))
            {
                return MobileResource.Common_Label_Craning;
            }
            else
            {
                return MobileResource.Common_Label_LoweredBen;
            }
        }

        /*
    * Trạng thái Ben
    *
    * @return isBen
    */

        public static bool IsBen(int state)
        {
            return (state & 16) == 0;
        }

        /* Trả về trạng thái động cơ */

        public static string BenState(int state)
        {
            if (!IsBen(state))
            {
                return MobileResource.Common_Label_Lowerben;
            }
            else
            {
                return MobileResource.Common_Label_Normal;
            }
        }

        /// <summary>
        /// trạng thái lỗi module vệ tinh
        /// </summary>
        /// <param name="vehicle"></param>
        /// <returns></returns>
        public static bool IsSatelliteError(VehicleOnline vehicle)
        {
            //KCN-7220
            if ((vehicle.State & 64) > 0)
            {
                return true;
            }
            return false;
        }
    }
}