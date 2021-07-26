using Prism.Events;

namespace BA_MobileGPS.Core
{
    public class UserMessageEvent : PubSubEvent<UserMessageEventModel>
    {
    }

    public class UserMessageEventModel
    {
        public string UserName { get; set; }

        public string Message { get; set; }
    }
}