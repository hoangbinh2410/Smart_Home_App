using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.RequestEntity;
using BA_MobileGPS.Entities.ResponeEntity;
using BA_MobileGPS.Entities.ResponeEntity.OTP;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface IAuthenticationService
    {
        Task<LoginResponse> LoginStreamAsync(LoginRequest request);

        Task<bool> ChangePassword(ChangePassRequest request);

        Task<bool> CheckUserExists(ForgotPasswordRequest input);

        Task<SendCodeSMSResponse> SendCodeSMS(ForgotPasswordRequest input);

        Task<CheckVerifyCodeResponse> CheckVerifyCode(VerifyCodeRequest input);

        Task<bool> ChangePassWordForget(ChangePasswordForgotRequest input);

        Task<OtpResultResponse> GetOTP(string targetNumber, string customerID);
        Task<CheckVerifyCodeResponse> CheckVehicleOtpsms(VerifyOtpRequest request);
        Task<bool> VerifyPhoneNumberOtp(VerifyPhoneRequest request);

    }
}