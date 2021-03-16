using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;

using Newtonsoft.Json;

using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IRequestProvider _IRequestProvider;

        public AuthenticationService(IRequestProvider IRequestProvider)
        {
            this._IRequestProvider = IRequestProvider;
        }

        public async Task<LoginResponse> LoginStreamAsync(LoginRequest request)
        {
            LoginResponse user = null;
            try
            {
                MsgRequest msg = new MsgRequest();

                msg.Param = Message.EncodeMessage(request.ConvertToByteArray().ToArray());

                string data = JsonConvert.SerializeObject(msg);

                byte[] ImageBuffer = Encoding.UTF8.GetBytes(data);

                Stream stream = new MemoryStream(ImageBuffer);

                var result = await _IRequestProvider.PostStreamAsync<LoginResponse>(ApiUri.POST_LOGIN, stream);
                if (result != null)
                {
                    user = result;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
            return user;
        }

        public async Task<bool> ChangePassword(ChangePassRequest request)
        {
            MsgRequest msg = new MsgRequest
            {
                Param = Message.EncodeMessage(request.ConvertToByteArray().ToArray())
            };
            string data = JsonConvert.SerializeObject(msg);

            return await _IRequestProvider.PostStreamAsync<bool>(ApiUri.POST_CHANGE_PASS, new MemoryStream(Encoding.UTF8.GetBytes(data)));
        }
    }
}