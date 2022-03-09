using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Entities.ResponeEntity
{
    public class ImageFileInfoResponse
    {
        public string Id { get; set; }

        public string Path { get; set; }

        public string FileName { get; set; }

        public string SaveName { get; set; }
        public string Extension { get; set; }    

        public string Md5 { get; set; }

        public string Url { get; set; }
    }
}
