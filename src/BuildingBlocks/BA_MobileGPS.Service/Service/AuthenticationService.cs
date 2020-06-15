using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;

using Newtonsoft.Json;

using System;
using System.Diagnostics;
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

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            LoginResponse user = null;
            try
            {
                var result = await _IRequestProvider.PostAsync<LoginRequest, LoginResponse>(ApiUri.POST_LOGIN, request);
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

        public async Task<bool> Logout(Guid UserID)
        {
            bool respone = false;
            try
            {
                var URL = $"{ApiUri.POST_LOGOUT}?UserID={UserID}";

                var result = await _IRequestProvider.GetAsync<bool>(URL);
                if (result)
                {
                    respone = result;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
            return respone;
        }

        #region quên mật khẩu

        public async Task<bool> CheckUserExists(ForgotPasswordRequest input)
        {
            bool respone = false;
            try
            {
                var result = await _IRequestProvider.GetAsync<bool>($"{ApiUri.VALIDATEPHONEBYUSER}?userName={input.userName}&phoneNumber={input.phoneNumber}");
                if (result)
                {
                    respone = result;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
            return respone;
        }

        public async Task<SendCodeSMSResponse> SendCodeSMS(ForgotPasswordRequest input)
        {
            var result = new SendCodeSMSResponse();
            try
            {
                var response = await _IRequestProvider.GetAsync<SendCodeSMSResponse>($"{ApiUri.SENDVERIFYCODE}?username={input.userName}&phone={input.phoneNumber}&appID={input.AppID}");
                if (response != null)
                {
                    result = response;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
            return result;
        }

        public async Task<CheckVerifyCodeResponse> CheckVerifyCode(VerifyCodeRequest input)
        {
            var result = new CheckVerifyCodeResponse();
            try
            {
                var response = await _IRequestProvider.GetAsync<CheckVerifyCodeResponse>($"{ApiUri.CHECKVERIFYCODE}?verifyCode={input.verifyCode}&phoneNumber={input.phoneNumber}&AppID={input.AppID}");
                if (response != null)
                {
                    result = response;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
            return result;
        }

        public async Task<bool> ChangePassWordForget(ChangePasswordForgotRequest input)
        {
            bool result = false;
            try
            {
                MsgRequest msg = new MsgRequest
                {
                    Param = Message.EncodeMessage(input.ConvertToByteArray().ToArray())
                };
                string data = JsonConvert.SerializeObject(msg);
                var response = await _IRequestProvider.PostStreamAsync<bool>(ApiUri.CHANGEPASSWORDFORGET, new MemoryStream(Encoding.UTF8.GetBytes(data)));
                if (response)
                {
                    result = response;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
            return result;
        }

        public async Task<UserToken> GetTokenAsync(string userName, string password)
        {
            UserToken token = null;
            try
            {
                string uri = "/oauth/token";
                string data = string.Format("grant_type=password&username={0}&password={1}", userName, password);
                var result = await _IRequestProvider.PostAsync<UserToken>(uri, data, Config.ClientId, Config.ClientSecret);
                if (result != null)
                {
                    token = result;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"GetTokenAsync-error:{e.Message}"); ;
            }
            return token;
        }

        #endregion quên mật khẩu

        #region OTP

        public async Task<OtpVerifyDataRespone> SentOTP(string username, string phonenumber)
        {
            var result = new OtpVerifyDataRespone();
            try
            {
                var response = await _IRequestProvider.GetAsync<OtpVerifyDataRespone>($"{ApiUri.GET_SENTOTP}?userName={username}&phoneNumber={phonenumber}");
                if (response != null)
                {
                    result = response;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
            return result;
        }

        public async Task<VerifyOtpResponse> VerifyOTP(string SecurityCode, long UserSecuritySMSLogID)
        {
            var result = new VerifyOtpResponse();
            try
            {
                var response = await _IRequestProvider.GetAsync<VerifyOtpResponse>($"{ApiUri.GET_VERIFYOTP}?SecurityCode={SecurityCode}&UserSecuritySMSLogID={UserSecuritySMSLogID}");
                if (response != null)
                {
                    if (response.Success)
                    {
                        result = response;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
            return result;
        }

        #endregion OTP
    }
}