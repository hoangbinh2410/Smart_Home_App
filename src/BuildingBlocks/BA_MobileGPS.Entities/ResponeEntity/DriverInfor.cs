using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Entities
{
    public class DriverInfor
    {
        public readonly Guid Id = new Guid();
        public string AvatarUrl { get; set; } = "avatar_default";
        public string FullName { get; set; } = " A B C D";
        public DateTime BirthDay { get; set; } = DateTime.Today.AddDays(-1800);
        public string IdentityNumber { get; set; } = "123123123";
        public string PhoneNum { get; set; } = "12354353534";
        public string Address { get; set; } = " Tỉnh A Phố B";
        public DriverLicenseEnum DriverLicenseType { get; set; } = DriverLicenseEnum.A3;
        public string DriverLicense { get; set; } = "123123123544";
        public DateTime LicenseEffectiveDate { get; set; } = DateTime.Today.AddDays(-800);
        public DateTime LicenseExpriedDate { get; set; } = DateTime.Today.AddDays(-300);
        public string DriverLicenseTypeName
        {
            get
            {
                return string.Format("Bằng lái {0}", DriverLicenseType.ToString());
            }
        }
    }

    public enum DriverLicenseEnum
    {
        A1,
        A2,
        A3,
        A4,
        B1, B2, C, D, E, F
    }
}
