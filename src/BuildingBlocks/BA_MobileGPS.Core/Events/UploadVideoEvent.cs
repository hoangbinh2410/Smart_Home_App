using BA_MobileGPS.Entities;
using Prism.Events;

namespace BA_MobileGPS.Core
{
    public class UploadVideoEvent : PubSubEvent<VideoRestreamInfo>
    {
    }
}