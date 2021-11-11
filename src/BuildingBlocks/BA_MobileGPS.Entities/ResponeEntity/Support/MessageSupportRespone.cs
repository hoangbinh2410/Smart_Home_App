using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

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

        private string _textBtnYes = string.Empty;
        public string TextBtnYes
        { 
            get => _textBtnYes; 
            set => SetProperty(ref _textBtnYes, value); 
        }
        private string _textBtnNo = string.Empty;
        public string TextBtnNo
        {
            get => _textBtnNo;
            set => SetProperty(ref _textBtnNo, value);
        }
        private bool _isVisibleYesNo ;
        public bool IsVisibleYesNo
        {
            get => _isVisibleYesNo;
            set => SetProperty(ref _isVisibleYesNo, value);
        }
        private bool _isShowIconBtnYes;
        public bool ISShowIconBtnYes
        {
            get => _isShowIconBtnYes;
            set => SetProperty(ref _isShowIconBtnYes, value);
        }
        private int _borderWidthBtnYes;
        public int BorderWidthBtnYes
        {
            get => _borderWidthBtnYes;
            set => SetProperty(ref _borderWidthBtnYes, value);
        }
        private Color _backgroundColorBtnYes;
        public Color BackgroundColorBtnYes
        {
            get => _backgroundColorBtnYes;
            set => SetProperty(ref _backgroundColorBtnYes, value);
        }
        private Color _textColorBtnYes;
        public Color TextColorBtnYes
        {
            get => _textColorBtnYes;
            set => SetProperty(ref _textColorBtnYes, value);
        }
    }
}
