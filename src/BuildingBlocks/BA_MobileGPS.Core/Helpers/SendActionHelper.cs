using BA_MobileGPS.Core.Helpers;
using System;
using System.Reflection;
using Xamarin.Essentials;

namespace BA_MobileGPS.Core
{
    public static class SendActionHelper
    {
        public async static void SendSMS(string phonenumberStr, string messaging)
        {
            try
            {
                var message = new SmsMessage(messaging, new[] { phonenumberStr });
                await Sms.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException ex)
            {
                // Sms is not supported on this device.
                LoggerHelper.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            catch (Exception ex)
            {
                // Other error has occurred.
                LoggerHelper.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        public static void SendMakePhoneCall(string phonenumber)
        {
            try
            {
                PhoneDialer.Open(phonenumber);
            }
            catch (ArgumentNullException anEx)
            {
                // Number was null or white space
                LoggerHelper.WriteError(MethodBase.GetCurrentMethod().Name, anEx);
            }
            catch (FeatureNotSupportedException ex)
            {
                // Phone Dialer is not supported on this device.
                LoggerHelper.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            catch (Exception ex)
            {
                // Other error has occurred.
                LoggerHelper.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }
    }
}