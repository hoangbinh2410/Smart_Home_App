using System;

namespace BA_MobileGPS.Entities
{
    public class UpdateUserInfoRequest
    {
        public Guid UserID { get; set; }

        public string UserName { get; set; }

        public string AvatarUrl { get; set; }

        public string FullName { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public int GenderId { get; set; }

        public string Career { get; set; }

        public string Role { get; set; }

        public int ReligionId { get; set; }

        public string Address { get; set; }

        public string Facebook { get; set; }
    }
}