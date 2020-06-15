using BA_MobileGPS.Entities;

using Realms;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BA_MobileGPS.Service
{
    public interface IRealmBaseService<TEntity, TViewModel>
        where TEntity : RealmObject, IRealmEntity
        where TViewModel : class
    {
        TViewModel Add(TViewModel viewModel);

        void AddRange(IEnumerable<TViewModel> viewModels);

        void AddRangeAsync(IEnumerable<TViewModel> viewModels);

        TViewModel Update(TViewModel viewModel);

        void Delete(long id);

        void Delete(Expression<Func<TEntity, bool>> predicate);

        bool Any();

        bool Any(Expression<Func<TEntity, bool>> predicate);

        TViewModel Get(long id);

        TViewModel Get(Expression<Func<TEntity, bool>> predicate);

        long GetLastId();

        IEnumerable<TViewModel> All();

        IEnumerable<TViewModel> Find(Expression<Func<TEntity, bool>> predicate);
    }
}