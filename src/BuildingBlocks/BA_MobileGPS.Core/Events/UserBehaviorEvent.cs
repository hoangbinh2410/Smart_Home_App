using BA_MobileGPS.Entities.Enums;
using BA_MobileGPS.Entities.RequestEntity;
using Prism.Events;

namespace BA_MobileGPS.Core
{
    public class UserBehaviorEvent : PubSubEvent<UserBehaviorModel>
    {
    }

    public class UserBehaviorModel
    {
        public MenuKeyEnums Page { get; set; }

        public UserBehaviorType Type { get; set; }
    }
}