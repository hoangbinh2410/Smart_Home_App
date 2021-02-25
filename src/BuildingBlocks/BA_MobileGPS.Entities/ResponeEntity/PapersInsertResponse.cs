using BA_MobileGPS.Utilities.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace BA_MobileGPS.Entities.ResponeEntity
{
    public class PapersInforInsertResponse : ResponseBaseV2<PapersIdResponse>
    {

    }

    public class PapersIdResponse
    {
        public Guid PK_PaperInfoID { get; set; }
        public string ErrorMessenger { get; set; }
    }

    public class BasePaperInforResponse : ResponseBaseV2<BasePaperInfor>
    {

    }

    public class BasePaperInfor
    {
        public string Id { get; set; }
        public int FK_CompanyID { get; set; }
        public long FK_VehicleID { get; set; }
        public string FK_PaperCategoryID { get; set; }
        public string PaperNumber { get; set; }
        public DateTime DateOfIssue { get; set; }
        public DateTime ExpireDate { get; set; }
        public int DayOfAlertBefore { get; set; }
        public string Description { get; set; }
        public PaperInfoExtend paperInfoExtend { get; set; }
    }

    public class PaperInfoExtend
    {
        public string Id { get; set; }
        public string WarrantyCompany { get; set; }
        public decimal Cost { get; set; }
        public string Contact { get; set; }
        public int FK_InsuranceCategoryID { get; set; }
    }

    /// <summary>
    /// Thong tin item trong list
    /// </summary>
    public class PaperItemInfor
    {
        public Guid Id { get; set; }
        public int FK_CompanyID { get; set; }
        public long FK_VehicleID { get; set; }
        public Guid FK_PaperCategoryID { get; set; }
        public string PaperCategoryName { get; set; }
        public DateTime DateOfIssue { get; set; }
        public DateTime ExpireDate { get; set;}
        public DateTime CreatedDate { get; set; }


        // Dùng ở itemTemplate theo loại giấy tờ => hiển thị biển số
        [JsonIgnore]
        public string VehiclePlate
        {
            get
            {
                var vehicle = StaticSettings.ListVehilceOnline.FirstOrDefault(x => x.VehicleId == FK_VehicleID);
                if (vehicle != null)
                {
                    return vehicle.VehiclePlate.ToUpper();
                }
                else return string.Empty;

            }
        }

        // Dùng ở itemTemplate theo loại giấy tờ => HIỂN THỊ ngày hết hạn
        [JsonIgnore]
        public string ExpireDateDisplay
        {
            get
            {
                return string.Format("Ngày hết hạn: {0}", ExpireDate.ToString("dd/MM/yyyy"));
            }
        }

        // Dùng ở itemTemplate theo loại giấy tờ => màu text còn số ngày hết hạn
        [JsonIgnore]
        public Color RemainDateColor { get; private set; }    

        // Dùng ở itemTemplate theo loại giấy tờ => số ngày hết hạn
        [JsonIgnore]
        public string RemainDate { get; private set; }
       
        // Không lấy dc configcomapny ở entities nên phải update
        public void UpdateData(int dayAllowRegister)
        {
            var days = (ExpireDate.Date - DateTime.Now).Days;
            if (days < 0)
            {              
                RemainDateColor = Color.FromHex("#F80A0A");
                RemainDate = "Còn 0 ngày";
            }
            else
            {
                var temp = (ExpireDate - new TimeSpan(dayAllowRegister, 0, 0, 0)).Date;
                RemainDate = string.Format("Còn {0} ngày", days);
                RemainDateColor = DateTime.Now.Date < temp ? Color.FromHex("#1CB6E8") : Color.FromHex("#FF6C3E");
            }
        }


    }

    /// <summary>
    /// danh sach giay to tra ve
    /// </summary>
    public class ListPaperResponse : ResponseBaseV2<List<PaperItemInfor>>
    {

    }
}
