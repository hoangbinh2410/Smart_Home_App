using BA_MobileGPS.Utilities.Enums;

using System;

namespace BA_MobileGPS.Entities
{
    public class SendCodeSMSResponse
    {
        //số lần xác thực sai trong ngày
        public int CountVerifyFailInDay { get; set; }

        //lưu id của một bản tin xác thực
        public long SecurityCodeSMSLogID { get; set; }

        //mã xác thực trả về
        public string SecurityCodeSMS { get; set; }

        /// <summary>
        /// Mã xác thực gửi đi
        /// </summary>
        public string VerifyCode { get; set; }

        public int SecondCountDown { get; set; }

        public string PhoneNumber { get; set; }

        public Guid UserId { get; set; }

        //trang thai xác nhận-- thành công hay không
        public bool IsActiveSMS { get; set; }

        //thông báo
        public string Message { get; set; }

        // trang thái gửi yêu cầu kích hoạt
        public StatusRegisterSMS StateRegister { set; get; }
    }
}