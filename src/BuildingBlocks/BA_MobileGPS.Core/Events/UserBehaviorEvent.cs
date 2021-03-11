using Prism.Events;

namespace BA_MobileGPS.Core
{
    public class UserBehaviorEvent : PubSubEvent<UserBehaviorModel>
    {
    }

    public class UserBehaviorModel
    {
        public string Page { get; set; }

        public UserBehaviorType Type { get; set; }
    }

    public enum UserBehaviorType
    {
        Start = 0,
        End = 1
    }
}