using BA_MobileGPS.Entities;

namespace BA_MobileGPS.Core.Models
{
    public class ChannelModel
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }

    public class ChannelCamera : BaseModel
    {
        public int Channel { get; set; }

        public string Name { get; set; }

        private ChannelCameraStatus status;
        public ChannelCameraStatus Status { get => status; set => SetProperty(ref status, value); }

        private bool isShow;
        public bool IsShow { get => isShow; set => SetProperty(ref isShow, value); }
    }

    public enum ChannelCameraStatus
    {
        Selected = 0,
        UnSelected = 1,
        Error
    }
}