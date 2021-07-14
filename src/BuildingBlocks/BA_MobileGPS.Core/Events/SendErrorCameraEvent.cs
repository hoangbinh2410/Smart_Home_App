using BA_MobileGPS.Entities;
using Prism.Events;
using System;

namespace BA_MobileGPS.Core
{
    public class SendErrorCameraEvent : PubSubEvent<int>
    {
    }

    public class SendErrorDoubleStremingCameraEvent : PubSubEvent<Tuple<PlaybackUserRequest, int>>
    {
    }
}