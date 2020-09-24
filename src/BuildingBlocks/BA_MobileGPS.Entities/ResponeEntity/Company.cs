using Newtonsoft.Json;

using System;

namespace BA_MobileGPS.Entities
{
    [Serializable]
    public class Company : BaseModel
    {
        public int FK_CompanyID { get; set; }

        public string CompanyName { get; set; }

        public int ParentCompanyID { get; set; }

        public Guid UserId { set; get; }

        public int XNCode { get; set; }

        public UserType UserType { get; set; }

        public CompanyType CompanyType { get; set; }

        private bool isSelected;

        [JsonIgnore]
        public bool IsSelected { get => isSelected; set => SetProperty(ref isSelected, value); }
    }
}