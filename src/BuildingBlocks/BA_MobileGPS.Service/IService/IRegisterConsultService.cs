using BA_MobileGPS.Entities;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface IRegisterConsultService
    {
        Task<int> RegisterConsultRequest(RegisterConsultRequest input);

        Task<List<ProvincesRegisterConsult>> GetDataTransportType(string currentLanguage);

        Task<List<GisProvince_RegisterConsult>> GetDataProvince(string currentLanguage);
    }
}