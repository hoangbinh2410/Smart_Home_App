using System.Collections.Generic;

namespace BA_MobileGPS.Entities
{
    public class ChangePassRequest
    {
        public string UserName { set; get; }

        public string OldPassword { set; get; }

        public string NewPassword { set; get; }

        public List<byte> ConvertToByteArray()
        {
            try
            {
                var Result = new List<byte>();

                Result.AddRange(SerializeLibrary.ConvertStringToArray(UserName));
                Result.AddRange(SerializeLibrary.ConvertStringToArray(OldPassword));
                Result.AddRange(SerializeLibrary.ConvertStringToArray(NewPassword));

                return Result;
            }
            catch
            { }

            return new List<byte>();
        }
    }
}