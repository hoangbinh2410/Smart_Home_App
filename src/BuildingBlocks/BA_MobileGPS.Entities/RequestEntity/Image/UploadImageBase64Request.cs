using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Entities
{
    public class UploadImageBase64Request : UploadImageRequest
    {
        public string Base64String { get; set; }
    }
}
