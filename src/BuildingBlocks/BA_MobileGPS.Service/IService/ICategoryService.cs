using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.Constant;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface ICategoryService
    {
        Task<List<CategoryResponse>> GetListCategorybyName(string name,string culture);      
    }
}