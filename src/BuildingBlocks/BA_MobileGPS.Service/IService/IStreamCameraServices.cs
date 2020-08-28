﻿using BA_MobileGPS.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service.IService
{
    public interface IStreamCameraService
    {
        Task<StreamStartResponse> StartStream(StreamStartRequest request);
        Task<StreamStopResponse> StopStream(StreamStopRequest request);
        Task<StreamPingResponse> RequestMoreStreamTime(StreamPingRequest request);
        /// <summary>
        /// </summary>
        /// <param name="conditionType">Kiểu tìm kiếm 
        /// 1: Tìm theo MXN 
        /// 2: Tìm theo BKS 
        /// 3: Tìm theo IMEI</param>
        /// <param name="conditionValue">Thông tin tìm kiếm</param>
        /// <returns></returns>
        Task<StreamDevicesResponse> GetDevicesStatus(int conditionType, string conditionValue);

       
    }
}
