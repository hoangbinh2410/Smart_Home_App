using BA_MobileGPS.Utilities.Enums;

using System;
using System.Collections.Generic;

namespace BA_MobileGPS.Entities.ResponeEntity.Camera
{
    /// <summary>
    /// thông tin phí của xe
    /// </summary>
    /// <Modified>
    /// Name     Date         Comments
    /// tunglm  17/04/2019   created
    /// </Modified>
    public class CameraImageResponse
    {
        public bool State { get; set; }
        public ResponseEnum ErrorCode { get; set; }

        public List<ImageCamera> ListCameraImage { get; set; }

        public int TotalCount { set; get; }
        public string Message { get; set; }
    }

    public class ImageCamera
    {
        public string Address { set; get; }

        public DateTime CreatedDate { set; get; }

        public string Channel { set; get; }

        public string ImageLink { set; get; }

        public float Latitude { set; get; }

        public float Longitude { set; get; }
    }
}