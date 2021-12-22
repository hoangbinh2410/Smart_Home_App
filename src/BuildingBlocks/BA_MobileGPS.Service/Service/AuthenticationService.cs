using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.RequestEntity;
using BA_MobileGPS.Entities.ResponeEntity;
using BA_MobileGPS.Entities.ResponeEntity.OTP;
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

        public async Task<OtpResultResponse> GetOTP(string targetNumber, string customerID)
        {
            var respone = new OtpResultResponse();
            try
            {
                var temp = await _IRequestProvider.GetAsync<BaseResponse<OtpResultResponse>>($"{ApiUri.GETOTP}?targetNumber={targetNumber}&customerID={customerID}");
                if (temp != null)
                {
                    if (temp.Success)
                    {
                        respone = temp.Data;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return respone;
        }

        public async Task<OtpResultResponse> GetsmsOTp(string targetNumber, string Namecustomer)
        {
            var respone = new OtpResultResponse();
            try
            {
                var temp = await _IRequestProvider.GetAsync<BaseResponse<OtpResultResponse>>($"{ApiUri.GETSMSOTP}?targetNumber={targetNumber}&customerID={Namecustomer}");
                if (temp != null)
                {
                    if (temp.Success)
                    {
                        respone = temp.Data;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return respone;
        }

        public async Task<VehiclePhoneRespone> CheckVehicleOtpsms(VehiclePhoneRequest request)
        {
            VehiclePhoneRespone result = new VehiclePhoneRespone();
            try
            {
                var respone = await _IRequestProvider.PostAsync<VehiclePhoneRequest, ResponseBaseV2<VehiclePhoneRespone>>(ApiUri.GET_Vehicle_OTP_SMS, request);
                if (respone != null && respone.Data != null)
                {
                    result = respone.Data;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(MethodBase.GetCurrentMethod().Name, e);
            }
            return result;
        }
    }
}