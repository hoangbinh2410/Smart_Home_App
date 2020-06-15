using BA_MobileGPS.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface IUserLandmarkGroupService
    {
        Task<List<UserLandmarkGroupRespone>> GetDataGroupByUserId(Guid userId);

        Task<List<UserLandmarkGroupRespone>> GetDataCategoryByUserId(Guid userId);

        Task<BaseResponse<bool>> SendUpdate(UserLandmarkGroupRequest request);

        Task<List<UserLandmarkRespone>> GetDataLandmarkByGroupId(string request);

        Task<List<UserLandmarkRespone>> GetDataLandmarkByCategory(string request);
    }
}