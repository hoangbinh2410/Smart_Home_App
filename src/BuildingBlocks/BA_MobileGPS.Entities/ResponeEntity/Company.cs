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

        private bool isSelected;

        [JsonIgnore]
        public bool IsSelected { get => isSelected; set => SetProperty(ref isSelected, value); }
    }
}