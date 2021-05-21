using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BA_MobileGPS.Entities.ResponeEntity.Issues
{
    public class IssuesRespone : BaseModel
    {
        public Guid Id { get; set; }

        public Guid FK_UserID { get; set; }

        public int FK_CompanyID { get; set; }

        public string IssueCode { get; set; }

        public DateTime DateRequest { get; set; }

        public string Status { get; set; }

        public string ContentRequest { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime CreatedDate { get; set; }

        private bool isFavorites;

        [JsonIgnore]
        public bool IsFavorites { get => isFavorites; set => SetProperty(ref isFavorites, value); }
    }

    public class IssuesDetailRespone : BaseModel
    {
        public string IssueCode { get; set; }

        public DateTime DateRequest { get; set; }

        public List<IssueStatusRespone> IssueStatus { get; set; }

        public string ContentRequest { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime CreatedDate { get; set; }
    }

    public class IssueStatusRespone
    {
        public string Status { get; set; }

        public DateTime DateChangeStatus { get; set; }

        [JsonIgnore]
        public bool IsShowLine { get; set; }

        [JsonIgnore]
        public bool IsDueDate { get; set; }
    }
}