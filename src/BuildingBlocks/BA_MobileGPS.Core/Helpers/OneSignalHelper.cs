using BA_MobileGPS.Entities;
using Com.OneSignal;
using Com.OneSignal.Abstractions;
using Prism.Events;
using Prism.Ioc;

namespace BA_MobileGPS.Core.Helpers
{
    public static class OneSignalHelper
    {
        public static void RegisterOneSignal(string oneSignalKey)
        {
            //Đăng kí sử dụng OneSignal
            OneSignal.Current.StartInit(oneSignalKey)
                 .HandleNotificationReceived(HandleNotificationReceived)
                 .HandleNotificationOpened(HandleNotificationOpened)
                 .InFocusDisplaying(OSInFocusDisplayOption.Notification)
                 .EndInit();

            OneSignal.Current.IdsAvailable((playerID, pushToken) =>
            {
                Settings.CurrentFirebaseToken = pushToken;
            });
        }

        public static void HandleNotificationReceived(OSNotification notification)
        {
            OSNotificationPayload payload = notification.payload;
            if (payload.additionalData.TryGetValue("Types", out var types))
            {
                var t = types.ToString();

                //NẾU Firebase là tung điều thì mở lên cuốc được tung điều đó
                if (t == (((int)FirebaseNotificationTypeEnum.UpdateApp).ToString()))
                {
                    Settings.IsUpdateApp = true;
                }
                else
                {
                    //trả về parameter thích làm gì thì làm
                    if (payload.additionalData.TryGetValue("Value", out var values))
                    {
                        Settings.ReceivedNotificationType = types.ToString();
                        Settings.ReceivedNotificationValue = values.ToString();
                        Settings.ReceivedNotificationTitle = payload.title;
                    };
                }
            };
        }

        public static void HandleNotificationOpened(OSNotificationOpenedResult result)
        {
            if (result.notification.payload.additionalData.TryGetValue("Types", out var types))
            {
                var t = types.ToString();
                //trả về parameter thích làm gì thì làm
                if (result.notification.payload.additionalData.TryGetValue("Value", out var values))
                {
                    Settings.ReceivedNotificationType = types.ToString();
                    Settings.ReceivedNotificationValue = values.ToString();
                    Settings.ReceivedNotificationTitle = result.notification.payload.title;

                    var _eventAggregator = Prism.PrismApplicationBase.Current.Container.Resolve<IEventAggregator>();

                    _eventAggregator.GetEvent<OneSignalOpendEvent>().Publish(true);
                };
            };
        }
    }
}