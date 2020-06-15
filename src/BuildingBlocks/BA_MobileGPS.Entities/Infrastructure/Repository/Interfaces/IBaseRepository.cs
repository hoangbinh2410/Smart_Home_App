using Realms;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BA_MobileGPS.Entities.Infrastructure.Repository
{
    public interface IBaseRepository
    {
        TEntity Add<TEntity>(TEntity entity) where TEntity : RealmObject, IRealmEntity;

        void AddRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : RealmObject, IRealmEntity;

        void AddRangeAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : RealmObject, IRealmEntity;

        TEntity Update<TEntity>(TEntity entity) where TEntity : RealmObject, IRealmEntity;

        void Delete<TEntity>(long id) where TEntity : RealmObject, IRealmEntity;

        void Delete<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : RealmObject, IRealmEntity;

        bool Any<TEntity>() where TEntity : RealmObject, IRealmEntity;

        bool Any<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : RealmObject, IRealmEntity;

        IEnumerable<TEntity> All<TEntity>() where TEntity : RealmObject, IRealmEntity;

        TEntity Get<TEntity>(long id) where TEntity : RealmObject, IRealmEntity;

        TEntity Get<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : RealmObject, IRealmEntity;

        long GetLastId<TEntity>() where TEntity : RealmObject, IRealmEntity;

        IEnumerable<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : RealmObject, IRealmEntity;
    }
}