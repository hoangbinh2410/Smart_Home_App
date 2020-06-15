using Realms;

namespace BA_MobileGPS.Entities
{
    public interface IRealmEntity : IEntity
    {
        RealmInteger<int> Counter { get; set; }
    }
}