using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.RequestEntity;
using BA_MobileGPS.Entities.ResponeEntity.OTP;
using BA_MobileGPS.Utilities.Enums;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface IAuthenticationService
    {
        Task<LoginResponse> Login(LoginRequest request);

        Task<bool> ChangePassword(ChangePassRequest request);

        Task<bool> CheckUserExists(ForgotPasswordRequest input);

        Task<SendCodeSMSResponse> SendCodeSMS(ForgotPasswordRequest input);

        Task<StateVerifyCode> CheckVerifyCode(VerifyCodeRequest input);

        Task<bool> ChangePassWordForget(ChangePasswordForgotRequest input);

        Task<OtpResultResponse> GetOTP(string targetNumber, string customerID);

        Task<ResultVerifyOtp> CheckVehicleOtpsms(VerifyOtpRequest request);

        Task<bool> VerifyPhoneNumberOtp(VerifyPhoneRequest request);
    }
}