using BA_MobileGPS.Core.Models;
using Prism.Events;
using System;

namespace BA_MobileGPS.Core.Events
{
    public class TabItemSwitchEvent : PubSubEvent<Tuple<ItemTabPageEnums, object>>
    {
    }

    public class TabSelectedChangedEvent : PubSubEvent<int>
    {
    }
}