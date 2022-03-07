using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.ResponeEntity;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface IUserService
    {
        Task<UserInfoRespone> GetUserInfomation(Guid pk_userID);    

        Task<bool> UpdateUserInfo(UpdateUserInfoRequest request);

        Task<bool> SetUserSettings(UserSettingsRequest request);

        Task<bool> SetAdminUserSettings(AdminUserConfiguration request);

        Task<UserViewModel> GetUserInfomation(string username);

        Task<ImageFileInfoResponse> UploadImageAsync(UploadImageBase64Request request);

    }
}