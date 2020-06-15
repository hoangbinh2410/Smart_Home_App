using Realms;

namespace BA_MobileGPS.Entities.Infrastructure.Repository
{
    public interface IRealmConnection
    {
        Realm Connection { get; }

        void Destroy();
    }
}