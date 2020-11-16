﻿using BA_MobileGPS.Entities;
using System;
using System.Collections.Generic;
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
        Task<StreamDevicesResponse> GetDevicesStatus(ConditionType type, string value);

        Task<List<CaptureImageData>> GetCaptureImageLimit(int xncode, string vehiclePlate, int limit);

        Task<List<CaptureImageData>> GetCaptureImageTime(int xncode, string vehiclePlate, DateTime fromTime, DateTime toTime);

        Task<List<CaptureImageData>> GetListCaptureImage(StreamImageRequest request);

        Task<List<CameraRestreamInfo>> GetListVideoByDate(CameraRestreamRequest request);

        Task<List<CameraRestreamUpload>> GetListVideoOnServer(CameraRestreamRequest request);

        Task<RestreamStartResponese> StartRestream(StartRestreamRequest request);

        Task<StreamStopResponse> StopRestream(StopRestreamRequest request);

    }

    public enum ConditionType
    {
        MXN = 1,
        BKS = 2,
        IMEI = 3
    }
}