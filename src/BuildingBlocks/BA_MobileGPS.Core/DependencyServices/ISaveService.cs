using System.IO;

namespace BA_MobileGPS.Core
{
    public interface ISaveService
    {
        string Save(Stream stream);
    }
}