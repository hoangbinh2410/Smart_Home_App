using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Entities
{
    public class MotoConfigRespone
    {
        public MotoConfigResult SetConfigResult { set; get; }
    }

    public enum MotoConfigResult : int
    {
        RUNNING = 0,
        FAIL = 1,
        OK = 2,
        NOTONLINE = 3,
        NOTSUPPORT = 4,
    }
}
