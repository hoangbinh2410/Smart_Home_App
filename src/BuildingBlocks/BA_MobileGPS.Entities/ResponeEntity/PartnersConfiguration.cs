namespace BA_MobileGPS.Entities
{
    public class PartnersConfiguration
    {
        public int Id { get; set; }

        public int FK_CompanyID { get; set; }

        public int Theme { get; set; }

        public string LoginLogo { get; set; }

        public string InAppLogo { get; set; }

        public string Hotline { get; set; }

        public string Website { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }
    }
}