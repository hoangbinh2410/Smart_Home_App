using BA_MobileGPS.Entities;

using Prism.Events;

namespace BA_MobileGPS.Core
{
    public class SelectLanguageTypeEvent : PubSubEvent<LanguageRespone>
    {
    }

    public class SelectCountryCodeEvent : PubSubEvent<CountryCode>
    {
    }
}