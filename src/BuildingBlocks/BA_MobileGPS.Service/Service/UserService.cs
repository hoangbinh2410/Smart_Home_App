using BA_MobileGPS.Entities;
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

        public async Task<string> UpdateUserAvatar(string userName, Stream avatar, string fileName)
        {
            return await RequestProvider.UploadImageAsync<string>($"{ApiUri.USER_UPDATE_AVATAR}?username={userName}", avatar, fileName);
        }

        public async Task<bool> UpdateUserInfo(UpdateUserInfoRequest request)
        {
            return await RequestProvider.PostAsync<UpdateUserInfoRequest, bool>(ApiUri.USER_UPDATE_INFO, request);
        }

        public async Task<bool> SetUserSettings(UserSettingsRequest request)
        {
            return await RequestProvider.PostAsync<UserSettingsRequest, bool>(ApiUri.USER_SET_SETTINGS, request);
        }

        public async Task<bool> SetAdminUserSettings(AdminUserConfiguration request)
        {
            return await RequestProvider.PostAsync<AdminUserConfiguration, bool>(ApiUri.ADMIN_USER_SET_SETTINGS, request);
        }

        public Task<UserInfoRespone> GetUserInfomation(Guid pk_userID)
        {
            return RequestProvider.GetAsync<UserInfoRespone>($"{ApiUri.GET_USERINFOMATION}?userId={pk_userID}");
        }

        public async Task<UserViewModel> GetUserInfomation(string username)
        {
            UserViewModel result = new UserViewModel();
            try
            {
                string url = $"{ApiUri.GET_USERBYUSERNAME}?username={username}";
                var response = await RequestProvider.GetAsync<ResponseBaseV2<UserViewModel>>(url);
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
    }
}