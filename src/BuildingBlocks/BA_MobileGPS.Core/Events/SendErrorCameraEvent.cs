using BA_MobileGPS.Entities;
using Prism.Events;
using System;
using System.Collections.Generic;

namespace BA_MobileGPS.Core
{
    public class SendErrorCameraEvent : PubSubEvent<int>
    {
    }

    public class SendErrorDoubleStremingCameraEvent : PubSubEvent<List<CameraStartRespone>>
    {
    }
}