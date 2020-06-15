using BA_MobileGPS.Entities;

using Realms;

namespace BA_MobileGPS.Service
{
    public interface IBoundaryServices<TEntity, TViewModel>
        where TEntity : RealmObject, IRealmEntity
        where TViewModel : class
    {
        TViewModel Update(TViewModel viewModel);
    }
}