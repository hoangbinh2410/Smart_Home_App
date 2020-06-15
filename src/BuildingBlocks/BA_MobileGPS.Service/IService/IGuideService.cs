using System.Collections.Generic;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface IGuideService
    {
        Task<List<string>> GetGuide(int type);
    }
}