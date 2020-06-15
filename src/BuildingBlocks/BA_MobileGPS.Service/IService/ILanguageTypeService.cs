using BA_MobileGPS.Entities;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface ILanguageService : IRealmBaseService<LanguageRealm, LanguageRespone>
    {
        Task<List<LanguageRespone>> GetAllLanguageType();

        Task<bool> UpdateLanguageByUser(RequestUpdateLanguage model);
    }
}