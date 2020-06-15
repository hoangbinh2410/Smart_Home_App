using BA_MobileGPS.Utilities;

using System;

namespace BA_MobileGPS.Entities
{
    public class RegisterConsultResponse
    {
        public RegisterConsultEnum Response { get; set; }
    }

    public class DataPicker
    {
        public int Key { get; set; }
        public string Value { get; set; }
    }

    public class ProvincesRegisterConsult
    {
        public short FK_TransportTypeID { get; set; }

        // Mã ngôn ngữ
        public byte FK_LanguageID { get; set; }

        // Mô tả
        public string Description { get; set; }

        // Người tạo
        public Guid CreatedByUser { get; set; }

        // Ngày tạo
        public DateTime CreatedDate { get; set; }

        public Guid UpdatedByUser { get; set; }

        public DateTime UpdatedDate { get; set; }

        // Tên của loại hình vận tải theo ngôn ngữ
        public string Value { get; set; }
    }

    public class GisProvince_RegisterConsult
    {
        public int PK_ProvinceID { get; set; }

        public string Name { get; set; }

        public string Polygon { get; set; }

        public string CountryCode { get; set; }

        public int BapProvinceId { get; set; }
    }
}