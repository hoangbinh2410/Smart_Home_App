using BA_MobileGPS.Entities;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface ICategoryService
    {
        Task<List<Gender>> GetListGender(string culture);

        Task<List<Religion>> GetListReligion(string culture);
    }
}