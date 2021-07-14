using BA_MobileGPS.Entities;
using Prism.Events;

namespace BA_MobileGPS.Core
{
    public class SendErrorCameraEvent : PubSubEvent<int>
    {
    }

    public class SendErrorDoubleStremingCameraEvent : PubSubEvent<PlaybackUserRequest>
    {
    }
}