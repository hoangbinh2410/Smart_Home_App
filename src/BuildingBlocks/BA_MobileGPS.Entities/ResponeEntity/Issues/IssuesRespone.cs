using BA_MobileGPS.Entities.Enums;
using Newtonsoft.Json;
using System;

namespace BA_MobileGPS.Entities.ResponeEntity.Issues
{
    public class IssuesRespone : BaseModel
    {
        public Guid Id { get; set; }

        public Guid FK_UserID { get; set; }

        public int FK_CompanyID { get; set; }

        public string IssueCode { get; set; }

        public IssuesStatusEnums Status { get; set; }

        public string Content { get; set; }

        public string Description { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        private bool isFavorites;

        [JsonIgnore]
        public bool IsFavorites { get => isFavorites; set => SetProperty(ref isFavorites, value); }
    }
}