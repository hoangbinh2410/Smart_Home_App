using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BA_MobileGPS.Entities.ResponeEntity.Support
{
    public class MessageSupportRespone : BaseModel
    {
        [JsonProperty("Id")]
        public Guid ID { get; set; }

        [JsonProperty("FK_SupportCategoryID")]
        public Guid FK_SupportCategoryID { get; set; }

        [JsonProperty("Questions")]
        public string Questions { get; set; }

        [JsonProperty("Guides")]
        public string Guides { get; set; }

        [JsonProperty("OrderNo")]
        public int OrderNo { get; set; }

        private bool _isShowGuides;
        public bool IsShowGuides { get => _isShowGuides; set => SetProperty(ref _isShowGuides, value); }
        public List<AnswerSupport> Options { get; set; }
    }

    public class AnswerSupport : BaseModel
    {
        public string Name { get; set; }

        public bool IsAnswer { get; set; }

        private bool _selected;
        public bool Selected { get => _selected; set => SetProperty(ref _selected, value); }
    }
}