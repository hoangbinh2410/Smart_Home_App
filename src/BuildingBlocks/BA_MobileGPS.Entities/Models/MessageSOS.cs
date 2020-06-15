using BA_MobileGPS.Entities;

using Newtonsoft.Json;

using System;

namespace BA_MobileGPS.Models
{
    public class MessageSOS : BaseModel
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
        public int maxWords = 0;

        public int MaxWords { get => maxWords; set => SetProperty(ref maxWords, value, relatedProperty: nameof(WordsLeft)); }

        [JsonIgnore]
        public int WordsLeft => MaxWords - (Content?.Length ?? 0);

        private bool isOnline;

        [JsonIgnore]
        public bool IsOnline { get => isOnline; set => SetProperty(ref isOnline, value); }

        private bool isSend;

        [JsonIgnore]
        public bool IsSend { get => isSend; set => SetProperty(ref isSend, value, relatedProperty: nameof(Status)); }

        private bool isRead;

        [JsonIgnore]
        public bool IsRead { get => isRead; set => SetProperty(ref isRead, value); }

        private bool isSending;

        [JsonIgnore]
        public bool IsSending { get => isSending; set => SetProperty(ref isSending, value, relatedProperty: nameof(Status)); }

        [JsonIgnore]
        public string Status => IsSend ? "Đã gửi" : ((DateTime.Now.Subtract(CreatedDate.LocalDateTime).TotalSeconds > 15) ? "Chưa gửi được" : "Đang gửi");
    }

    public class MessageByUser : BaseModel
    {
        public string Title { get; set; }

        public MessageSOS LastMessage { get; set; }
    }
}