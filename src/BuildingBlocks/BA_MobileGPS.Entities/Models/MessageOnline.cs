using BA_MobileGPS.Entities;

using Newtonsoft.Json;

using System;

namespace BA_MobileGPS.Models
{
    public class MessageOnline : BaseModel
    {
        [JsonIgnore]
        public long Id { get; set; }

        private string receiver = string.Empty;
        public string Receiver { get => receiver; set => SetProperty(ref receiver, value); }

        private string content = string.Empty;
        public string Content { get => content; set => SetProperty(ref content, value, relatedProperty: nameof(WordsLeft)); }

        private DateTimeOffset createdDate;
        public DateTimeOffset CreatedDate { get => createdDate; set => SetProperty(ref createdDate, value); }

        [JsonIgnore]
        public int WordsLeft => 160 - (Content?.Length ?? 0);

        private bool isSend;

        [JsonIgnore]
        public bool IsSend { get => isSend; set => SetProperty(ref isSend, value, relatedProperty: nameof(Status)); }

        private bool isRead;

        [JsonIgnore]
        public bool IsRead { get => isRead; set => SetProperty(ref isRead, value); }

        [JsonIgnore]
        public string Status => IsSend ? "Đã gửi" : ((CreatedDate.Subtract(DateTime.Now).TotalSeconds > 15) ? "Chưa gửi được" : "Đang gửi");
    }

    public class MessageOnlineByUser : BaseModel
    {
        public string Title { get; set; }

        public MessageOnline LastMessage { get; set; }
    }
}