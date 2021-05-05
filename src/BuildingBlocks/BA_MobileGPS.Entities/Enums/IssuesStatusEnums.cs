using System.ComponentModel;

namespace BA_MobileGPS.Entities.Enums
{
    public enum IssuesStatusEnums
    {
        [Description("Tất cả")]
        All = 0,
        [Description("Đã gửi yêu cầu")]
        SendRequestIssue = 1,
        [Description("CSKH đã tiếp nhận")]
        CSKHInReceived = 2,
        [Description("Kỹ thuật đang xử lý")]
        EngineeringIsInprogress = 3,
        [Description("Hoàn thành")]
        Finish = 4
    }
}