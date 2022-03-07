using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.ResponeEntity;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public class UserService : IUserService
    {
        private readonly IRequestProvider RequestProvider;

        public UserService(IRequestProvider requestProvider)
        {
            RequestProvider = requestProvider;
        }
     
        public async Task<bool> UpdateUserInfo(UpdateUserInfoRequest request)
        {
            bool result = false;
            try
            {
                var respone = await RequestProvider.PostAsync<UpdateUserInfoRequest, ResponseBase<bool>>(ApiUri.USER_UPDATE_INFO, request);
                if (respone != null)
                {
                    result = respone.Data;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return result;
        }

        public async Task<bool> SetUserSettings(UserSettingsRequest request)
        {
            bool result = false;
            try
            {
                var respone = await RequestProvider.PostAsync<UserSettingsRequest, ResponseBase<bool>>(ApiUri.USER_SET_SETTINGS, request);
                if (respone != null)
                {
                    result = respone.Data;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return result;          
        }

        public async Task<bool> SetAdminUserSettings(AdminUserConfiguration request)
        {
            bool result = false;
            try
            {
                var respone = await RequestProvider.PostAsync<AdminUserConfiguration, ResponseBase<bool>>(ApiUri.ADMIN_USER_SET_SETTINGS, request);
                if (respone != null)
                {
                    result = respone.Data;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return result;           
        }

        public async Task<UserInfoRespone> GetUserInfomation(Guid pk_userID)
        {
            UserInfoRespone result = new UserInfoRespone();
            try
            {
                var respone = await RequestProvider.GetAsync<ResponseBase<UserInfoRespone>>($"{ApiUri.GET_USERINFOMATION}?userId={pk_userID}");
               if(respone != null && respone.Data != null)
                {
                    result = respone.Data;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return result;
            
        }

        public async Task<UserViewModel> GetUserInfomation(string username)
        {
            UserViewModel result = new UserViewModel();
            try
            {
                string url = $"{ApiUri.GET_USERBYUSERNAME}?username={username}";
                var response = await RequestProvider.GetAsync<ResponseBase<UserViewModel>>(url);
                if (response?.Data != null)
                {
                    result = response.Data;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return result;
        }

        public async Task<ImageFileInfoResponse> UploadImageAsync(UploadImageBase64Request request)
        {
            ImageFileInfoResponse result = new ImageFileInfoResponse();
            try
            {
                var respone = await RequestProvider.PostAsync<UploadImageBase64Request,ResponseBase<ImageFileInfoResponse>>(ApiUri.USER_UPDATE_AVATAR, request);
                if (respone != null)
                {
                    result = respone.Data;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return result;
        }
    }
}