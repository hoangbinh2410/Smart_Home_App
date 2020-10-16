using Plugin.Messaging;

namespace BA_MobileGPS.Core
{
    public static class SendActionHelper
    {
        public static void SendSMS(string phonenumberStr, string messaging)
        {
            var smsMessenger = CrossMessaging.Current.SmsMessenger;
            if (smsMessenger.CanSendSms)
            {
                smsMessenger.SendSms(phonenumberStr, messaging);
            }
        }

        public static void SendMakePhoneCall(string phonenumber)
        {
            var phoneDialer = CrossMessaging.Current.PhoneDialer;
            if (phoneDialer.CanMakePhoneCall)
            {
                phoneDialer.MakePhoneCall(phonenumber);
            }
        }

        public static void SendEmail(string emailAddress, string subject, string body)
        {
            var emailMessenger = CrossMessaging.Current.EmailMessenger;
            if (emailMessenger.CanSendEmail)
            {
                emailMessenger.SendEmail(emailAddress, subject, body);

                var email = new EmailMessageBuilder()
                  .To(emailAddress)
                  .Subject(subject)
                  .Body(body)
                  .Build();

                emailMessenger.SendEmail(email);
            }
        }
    }
}
