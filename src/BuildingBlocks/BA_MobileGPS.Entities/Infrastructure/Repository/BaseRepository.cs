using Realms;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace BA_MobileGPS.Entities.Infrastructure.Repository
{
    public class BaseRepository : IBaseRepository
    {
        private readonly IRealmConnection _realmConnection;
        private readonly object _lock = new object();

        private Realm _connection;

        private Realm Connection
        {
            get
            {
                if (_connection?.IsClosed ?? true)
                {
                    _connection = _realmConnection.Connection;
                }
                return _connection;
            }
        }

        public BaseRepository(IRealmConnection realmConnection)
        {
            _realmConnection = realmConnection;
            _connection = _realmConnection.Connection;
        }

        public TEntity Add<TEntity>(TEntity entity) where TEntity : RealmObject, IRealmEntity
        {
            lock (_lock)
            {
                using (var transaction = Connection.BeginWrite())
                {
                    entity.Id = GetLastId<TEntity>() + 1;
                    var result = Connection.Add(entity);
                    transaction.Commit();
                    return result;
                }
            }
        }

        public void AddRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : RealmObject, IRealmEntity
        {
            lock (_lock)
            {
                using (var transaction = Connection.BeginWrite())
                {
                    foreach (var entity in entities)
                    {
                        entity.Id = GetLastId<TEntity>() + 1;
                        Connection.Add(entity);
                    }
                    transaction.Commit();
                }
            }
        }

        public void AddRangeAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : RealmObject, IRealmEntity
        {
            new Thread(() =>
            {
                using (var transaction = Connection.BeginWrite())
                {
                    foreach (var entity in entities)
                    {
                        entity.Id = GetLastId<TEntity>() + 1;
                        Connection.Add(entity);
                    }
                    transaction.Commit();
                }
            }).Start();
        }

        public TEntity Update<TEntity>(TEntity entity) where TEntity : RealmObject, IRealmEntity
        {
            lock (_lock)
            {
                using (var transation = Connection.BeginWrite())
                {
                    var result = Connection.Add(entity, true);
                    transation.Commit();
                    return result;
                }
            }
        }

        public void Delete<TEntity>(long id) where TEntity : RealmObject, IRealmEntity
        {
            lock (_lock)
            {
                using (var transation = Connection.BeginWrite())
                {
                    var entity = Get<TEntity>(id);
                    if (entity != null)
                        Connection.Remove(entity);
                    transation.Commit();
                }
            }
        }

        public void Delete<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : RealmObject, IRealmEntity
        {
            lock (_lock)
            {
                using (var transation = Connection.BeginWrite())
                {
                    var entity = Get(predicate);
                    if (entity != null)
                        Connection.Remove(entity);
                    transation.Commit();
                }
            }
        }

        public bool Any<TEntity>() where TEntity : RealmObject, IRealmEntity
        {
            lock (_lock)
                return Connection.All<TEntity>()?.Any() ?? false;
        }

        public bool Any<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : RealmObject, IRealmEntity
        {
            lock (_lock)
                return Connection.All<TEntity>()?.Any(predicate) ?? false;
        }

        public TEntity Get<TEntity>(long id) where TEntity : RealmObject, IRealmEntity
        {
            lock (_lock)
                return Connection.Find<TEntity>(id);
        }

        public TEntity Get<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : RealmObject, IRealmEntity
        {
            lock (_lock)
                return Connection.All<TEntity>()?.FirstOrDefault(predicate);
        }

        public long GetLastId<TEntity>() where TEntity : RealmObject, IRealmEntity
        {
            lock (_lock)
            {
                var data = Connection.All<TEntity>().ToList();
                if (data != null && data.Count > 0)
                {
                    return data.OrderByDescending(x => x.Id).First().Id;
                }
                else
                {
                    return 0;
                }

            }

        }

        public IEnumerable<TEntity> All<TEntity>() where TEntity : RealmObject, IRealmEntity
        {
            lock (_lock)
                return Connection.All<TEntity>();
        }

        public IEnumerable<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : RealmObject, IRealmEntity
        {
            lock (_lock)
                return Connection.All<TEntity>().Where(predicate);
        }
    }
}