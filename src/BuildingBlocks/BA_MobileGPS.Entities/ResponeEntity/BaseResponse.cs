using BA_MobileGPS.Entities.Enums;

namespace BA_MobileGPS.Entities
{
    public class BaseResponse<T>
    {
        public T Data { get; set; }

        public bool Success { get; set; }

        public StatusCode StatusCode { get; set; }

        public string Message { get; set; }
    }

    public class ResponseStreamBase<T>
    {
        public T Data { get; set; }

        public string InternalMessage { get; set; }

        public int StatusCode { get; set; }

        public string UserMessage { get; set; }
    }

    public class ResponseBaseV2<T>
    {
        public T Data { get; set; }

        public int statusCode { set; get; }

        public ResponseCodeEnums responseCode { set; get; }

        public string usermessage { set; get; }

        public string internalmessage { get; set; }
    }
}