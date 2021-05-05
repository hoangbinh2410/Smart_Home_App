namespace BA_MobileGPS.Entities
{
    public enum StatusCode
    {
        OK = 200,
        BadRequest = 400,
        NotFound = 404,
        ServerError = 500
    }

    public class StatusCodeCamera
    {
        public const int STATUS_OK = 0;

        public const int WARNING_NODATA = 1;

        public const int ERROR_PRIVATECODE = 100;
        public const int ERROR_INVALIDINPUT = 101;
        public const int ERROR_DATANOTFOUND = 102;
        public const int ERROR_DEVICENOTREADY = 103;
        public const int ERROR_OPERATION = 104;

        /// <summary>
        /// Thiết bị đang được xem lại. Vui lòng hủy phiên xem lại hoặc thử lại sau.
        /// </summary>
        public const int ERROR_STREAMING_BY_PLAYBACK = 105;

        /// <summary>
        /// Thiết bị đang được xem lại. Vui lòng hủy phiên hiện tại hoặc thử lại sau.
        /// </summary>
        public const int ERROR_PLAYBACK_IS_BUSY = 106;

        /// <summary>
        /// Thiết bị đang phát trực tiếp. Vui lòng hủy phát trực tiếp hoặc thử lại sau.
        /// </summary>
        public const int ERROR_PLAYBACK_BY_STREAMING = 107;

        public const int ERROR_EXCEPTION = -1;

        public const int WEB_ERROR_OFFSET = 1000;
        public const int BAP_ERROR_OFFSET = 2000;
    }
}