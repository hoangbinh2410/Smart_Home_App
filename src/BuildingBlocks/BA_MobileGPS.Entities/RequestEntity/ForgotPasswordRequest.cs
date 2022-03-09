using System.Collections.Generic;

namespace BA_MobileGPS.Entities
{
    public class ForgotPasswordRequest
    {
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }

        public int AppID { get; set; }
    }

    public class VerifyCodeRequest
    {
        public string VerifyCode { get; set; }
        public string PhoneNumber { get; set; }

        public int AppID { get; set; }
    }

    public class ChangePasswordForgotRequest
    {
        public string UserName { get; set; }
        public string NewPassword { get; set; }

        public List<byte> ConvertToByteArray()
        {
            try
            {
                var Result = new List<byte>();

                Result.AddRange(SerializeLibrary.ConvertStringToArray(UserName));
                Result.AddRange(SerializeLibrary.ConvertStringToArray(NewPassword));

                return Result;
            }
            catch
            { }

            return new List<byte>();
        }
    }
}