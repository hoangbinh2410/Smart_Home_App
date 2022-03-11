using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.Constant;
using BA_MobileGPS.Utilities.Constant;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly IRequestProvider RequestProvider;

        public CategoryService(IRequestProvider requestProvider)
        {
            RequestProvider = requestProvider;
        }

        public async Task<List<CategoryResponse>> GetListCategorybyName(string name, string culture)
        {
            List<CategoryResponse> result = new List<CategoryResponse>();

            var respone = await RequestProvider.GetAsync<ResponseBase<List<CategoryResponse>>>($"{ApiUri.CATEGORY_LIST_GENDER}?name={name}&culture={culture}");
            if (respone != null && respone.Data.Count > 0)
            {
                result = respone.Data;
            }
            return result;
        }     
    }
}