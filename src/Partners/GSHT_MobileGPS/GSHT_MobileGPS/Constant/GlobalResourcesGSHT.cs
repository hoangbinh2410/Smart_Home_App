using BA_MobileGPS.Core;
using BA_MobileGPS.Entities;

namespace GSHT_MobileGPS.Constant
{
    public sealed class GlobalResourcesGSHT : ExtendedBindableObject
    {
        private static readonly GlobalResourcesGSHT _current = new GlobalResourcesGSHT();
        public static GlobalResourcesGSHT Current => _current;

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static GlobalResourcesGSHT()
        {
        }

        private GlobalResourcesGSHT()
        {
        }

        private PartnersConfiguration partnerConfig = new PartnersConfiguration()
        {
            InAppLogo = "ic_logo.png",
            LoginLogo= "Logo_GSHT.png"
        };

        public PartnersConfiguration PartnerConfig
        {
            get { return partnerConfig; }
            set
            {
                partnerConfig = value;

                RaisePropertyChanged(() => PartnerConfig);
            }
        }
    }
}