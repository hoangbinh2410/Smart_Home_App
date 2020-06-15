using BA_MobileGPS.Entities;

using Prism.Events;

namespace BA_MobileGPS.Core
{
    public class SelectDateTimeEvent : PubSubEvent<PickerDateTimeResponse>
    {
    }

    public class SelectTimeEvent : PubSubEvent<PickerDateTimeResponse>
    {
    }
}