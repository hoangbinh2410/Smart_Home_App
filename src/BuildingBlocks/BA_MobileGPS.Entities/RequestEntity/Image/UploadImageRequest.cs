using BA_MobileGPS.Utilities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Entities
{
    public class UploadImageRequest
    {
        public FileType FileType { get; set; }

        public AppType SystemType { get; set; }

        public ModuleType ModuleType { get; set; }
    }
}
