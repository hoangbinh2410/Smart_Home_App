using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Entities
{

    public class DriverInfor : BaseModel
    {
        public int ID { get; set; }
        public int PK_EmployeeID { get; set; } // Dung cho update, = ID
        public string EmployeeCode { get; set; } 
        public int FK_CompanyID { get; set; } 
       // public int FK_DepartmentID { get; set; } 
        public string Name { get; set; } 
        public string DisplayName { get; set; } //Họ tên require
        public DateTime Birthday { get; set; } //Ngày sinh require
        public string DriverImage { get; set; }
      //   public bool Sex { get; set; } 
        public string Address { get; set; } //Địa chỉ  require
        public string Mobile { get; set; } //Số điện thoại require
        public string IdentityNumber { get; set; } //Số CMND require
        public int LicenseType { get; set; } //Loại bằng lái require
        public string DriverLicense { get; set; } //Số bằng lái require
        public DateTime IssueLicenseDate { get; set; } //Ngày cấp null 
        public DateTime ExpireLicenseDate { get; set; } //Ngày hết hạn require
        public string CreatedByUser { get; set; } // Nguoi tao require
        public DateTime CreatedDate { get; set; } //Ngày tao require
        public string UpdatedByUser { get; set; } // nguoi sua require
        public DateTime UpdatedDate { get; set; } //Ngày sua require

        [JsonIgnore]
        public DriverLicenseEnum LicenseTypeName
        {
            get 
            { 
                return (DriverLicenseEnum)LicenseType;
            }
            set
            {
                LicenseType = (int)value;
                RaisePropertyChanged();
            }
        }

        [JsonIgnore]
        public string AvartarDisplay
        {
            get
            {
                if (string.IsNullOrEmpty(DriverImage))
                {
                    return "avatar_default.png";
                }
                return DriverImage;
            }
            set
            {
                DriverImage = value;                
            }
        }

    }

    public class DriverDeleteRequest
    {
        public int PK_EmployeeID { get; set; }
        public Guid? UpdatedByUser { get; set; }
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
