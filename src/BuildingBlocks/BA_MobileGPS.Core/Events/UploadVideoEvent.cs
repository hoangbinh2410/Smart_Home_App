using BA_MobileGPS.Entities;
using Prism.Events;

namespace BA_MobileGPS.Core
{
    public class UploadVideoEvent : PubSubEvent<bool>
    {
    }

    public class UploadFinishVideoEvent : PubSubEvent<bool>
    {
    }
}