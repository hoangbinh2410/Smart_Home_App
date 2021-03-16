﻿using BA_MobileGPS.Entities;
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
    }
}