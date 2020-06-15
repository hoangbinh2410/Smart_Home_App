using BA_MobileGPS.Entities;

using System;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface IAuthenticationService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);

        Task<LoginResponse> LoginStreamAsync(LoginRequest request);

        Task<UserToken> GetTokenAsync(string userName, string password);

        Task<bool> ChangePassword(ChangePassRequest request);

        Task<bool> Logout(Guid UserID);

        Task<bool> CheckUserExists(ForgotPasswordRequest input);

        Task<SendCodeSMSResponse> SendCodeSMS(ForgotPasswordRequest input);

        Task<CheckVerifyCodeResponse> CheckVerifyCode(VerifyCodeRequest input);

        Task<bool> ChangePassWordForget(ChangePasswordForgotRequest input);

        Task<OtpVerifyDataRespone> SentOTP(string username, string phonenumber);

        Task<VerifyOtpResponse> VerifyOTP(string SecurityCode, long UserSecuritySMSLogID);
    }
}