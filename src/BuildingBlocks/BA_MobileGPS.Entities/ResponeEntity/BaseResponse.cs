namespace BA_MobileGPS.Entities
{
    public class BaseResponse<T>
    {
        public T Data { get; set; }

        public bool Success { get; set; }

        public StatusCode StatusCode { get; set; }

        public string Message { get; set; }
    }
}