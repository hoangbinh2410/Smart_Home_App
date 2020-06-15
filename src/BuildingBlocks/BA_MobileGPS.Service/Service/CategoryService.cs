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

        public Task<List<Gender>> GetListGender(string culture)
        {
            return RequestProvider.GetAsync<List<Gender>>($"{ApiUri.CATEGORY_LIST_GENDER}?culture={culture}");
        }

        public Task<List<Religion>> GetListReligion(string culture)
        {
            return RequestProvider.GetAsync<List<Religion>>($"{ApiUri.CATEGORY_LIST_RELIGION}?culture={culture}");
        }
    }
}