using BA_MobileGPS.Entities;

using Prism.Events;

namespace BA_MobileGPS.Core
{
    public class SelectDateTimeEvent : PubSubEvent<PickerDateTimeResponse>
    {
    }

    public class SelectDateEvent : PubSubEvent<PickerDateResponse>
    {
    }

    public class SelectMonthEvent : PubSubEvent<PickerDateResponse>
    {
    }

    public class SelectTimeEvent : PubSubEvent<PickerDateTimeResponse>
    {
    }
}