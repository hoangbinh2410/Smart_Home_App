using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public class UserLandmarkGroupService : IUserLandmarkGroupService
    {
        private readonly IRequestProvider requestProvider;

        public UserLandmarkGroupService(IRequestProvider requestProvider)
        {
            this.requestProvider = requestProvider;
        }

        public async Task<List<UserLandmarkGroupRespone>> GetDataGroupByUserId(Guid userId)
        {
            var respone = new List<UserLandmarkGroupRespone>();
            try
            {
                var URL = string.Format(ApiUri.GET_ALL_LANDMARK_GROUP_BY_USERID + "?userId={0}", userId);
                var temp = await requestProvider.GetAsync<ResponseBase<List<UserLandmarkGroupRespone>>>(URL);
                if (temp != null && temp.Data.Count > 0)
                {
                    respone = temp.Data;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
            return respone;
        }

        public async Task<List<UserLandmarkGroupRespone>> GetDataCategoryByUserId(Guid userId)
        {
            var respone = new List<UserLandmarkGroupRespone>();
            try
            {
                var URL = string.Format(ApiUri.GET_ALL_LANDMARK_CATEGORY_BY_USERID + "?userId={0}", userId);
                var temp = await requestProvider.GetAsync<ResponseBase<List<UserLandmarkGroupRespone>>>(URL);
                if (temp != null && temp.Data.Count > 0)
                {
                    respone = temp.Data;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
            return respone;
        }

        public async Task<ResponseBase<bool>> SendUpdate(UserLandmarkGroupRequest request)
        {
            return await requestProvider.PostAsync<UserLandmarkGroupRequest, ResponseBase<bool>>(ApiUri.INSERT_CONFIG_VISIBLE_GROUP_LANDMARK, request);
        }

        public async Task<List<UserLandmarkRespone>> GetDataLandmarkByGroupId(string request)
        {
            var respone = new List<UserLandmarkRespone>();
            try
            {
                var URL = string.Format(ApiUri.GET_LANDMARK_BY_GROUPID + "?listLandmarksGroupID={0}", request);
                var temp = await requestProvider.GetAsync<ResponseBase<List<UserLandmarkRespone>>>(URL);
                if (temp != null && temp.Data.Count > 0)
                {
                    respone = temp.Data;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
            return respone;
        }

        public async Task<List<UserLandmarkRespone>> GetDataLandmarkByCategory(string request)
        {
            var respone = new List<UserLandmarkRespone>();
            try
            {
                var URL = string.Format(ApiUri.GET_LANDMARK_BY_CATEGORY + "?listLandmarksCategory={0}", request);
                var temp = await requestProvider.GetAsync<ResponseBase<List<UserLandmarkRespone>>>(URL);
                if (temp != null && temp.Data.Count > 0)
                {
                    respone = temp.Data;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
            return respone;
        }
    }
}