using BA_MobileGPS.Utilities.Constant;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Entities
{

    public class DriverInfor : BaseModel
    {      
        public int PK_EmployeeID { get; set; }

        public int FK_CompanyID { get; set; }

        public string DisplayName { get; set; }

        public DateTime? Birthday { get; set; }

        public string DriverImage { get; set; }

        public byte? Sex { get; set; }

        public string Address { get; set; }

        public string Mobile { get; set; }

        public string IdentityNumber { get; set; }

        public int LicenseType { get; set; }

        public string DriverLicense { get; set; }

        public DateTime? IssueLicenseDate { get; set; }

        public string IssueLicensePlace { get; set; }

        public DateTime? ExpireLicenseDate { get; set; }

        public Guid? UpdatedByUser { get; set; }

        [JsonIgnore]
        public DriverLicenseEnum? LicenseTypeName
        {
            get
            {
                if (LicenseType == 0)
                {
                    return null;
                }
                return (DriverLicenseEnum)LicenseType;
            }
            set
            {
                LicenseType = (int)value;
                RaisePropertyChanged();
            }
        }

        // Dùng ở danh sách
        [JsonIgnore]
        public string AvartarDisplay
        {
            get
            {
                if (string.IsNullOrEmpty(DriverImage))
                {
                    return "avatar_default.png";
                }
                return $"{ServerConfig.ApiEndpoint}{DriverImage}";
            }
        }

    }

    public class InsertDriverInfor
    {
        public int FK_CompanyID { get; set; }

        public string DisplayName { get; set; }

        public DateTime? Birthday { get; set; }

        public string DriverImage { get; set; }

        public byte? Sex { get; set; }

        public string Address { get; set; }

        public string Mobile { get; set; }

        public string IdentityNumber { get; set; }

        public int LicenseType { get; set; }

        public string DriverLicense { get; set; }

        public DateTime? IssueLicenseDate { get; set; }

        public string IssueLicensePlace { get; set; }

        public DateTime? ExpireLicenseDate { get; set; }
    }

    public class DriverDeleteRequest
    {
        public int PK_EmployeeID { get; set; }
        public Guid? UpdatedByUser { get; set; }
    }

    public enum DriverLicenseEnum
    {

        A1 = 1,
        A2 = 2,
        A3 = 3,
        A4 = 4,
        B1 = 5,
        B2 = 6,
        C = 7,
        D = 8,
        E = 9,
        F = 10
    }


}
