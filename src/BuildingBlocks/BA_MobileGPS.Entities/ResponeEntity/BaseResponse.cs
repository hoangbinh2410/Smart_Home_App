using BA_MobileGPS.Entities.Enums;
using System.Collections.Generic;

namespace BA_MobileGPS.Entities
{  
    public class ResponseStreamBase<T>
    {
        public T Data { get; set; }

        public string InternalMessage { get; set; }

        public int StatusCode { get; set; }

        public string UserMessage { get; set; }
    }

    public class ResponseBase<T>
    {
        public T Data { get; set; }

        public int statusCode { set; get; }

        public ResponseCodeEnums responseCode { set; get; }

        public string usermessage { set; get; }

        public string internalmessage { get; set; }
    } 
}