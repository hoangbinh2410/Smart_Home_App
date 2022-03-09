using BA_MobileGPS.Entities;
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

        public async Task<List<Gender>> GetListGender(string culture)
        {
            List<Gender> result = new List<Gender>();
          
            var respone =await RequestProvider.GetAsync<ResponseBase<List<Gender>>>($"{ApiUri.CATEGORY_LIST_GENDER}?culture={culture}");
            if(respone != null && respone.Data.Count > 0)
            {
               result = respone.Data;
            }
            return result;
        }

        public async Task<List<Religion>> GetListReligion(string culture)
        {
            List<Religion> result = new List<Religion>();
            var respone =await RequestProvider.GetAsync<ResponseBase<List<Religion>>>($"{ApiUri.CATEGORY_LIST_RELIGION}?culture={culture}");
            if (respone != null && respone.Data.Count > 0)
            {
                result = respone.Data;
            }
            return result;
        }
    }
}