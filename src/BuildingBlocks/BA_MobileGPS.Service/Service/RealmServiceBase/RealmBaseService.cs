using AutoMapper;

using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.Infrastructure.Repository;

using Realms;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BA_MobileGPS.Service
{
    public class RealmBaseService<TEntity, TViewModel> : IRealmBaseService<TEntity, TViewModel>
        where TEntity : RealmObject, IRealmEntity
        where TViewModel : class
    {
        private readonly IBaseRepository _baseRepository;
        private readonly IMapper _mapper;

        public RealmBaseService(IBaseRepository baseRepository, IMapper mapper)
        {
            _baseRepository = baseRepository;
            this._mapper = mapper;
        }

        public TViewModel Add(TViewModel viewModel)
        {
            try
            {
                var entity = _mapper.Map<TEntity>(viewModel);
                var result = _baseRepository.Add(entity);
                return _mapper.Map<TViewModel>(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddRange(IEnumerable<TViewModel> viewModels)
        {
            try
            {
                var entities = _mapper.Map<IEnumerable<TViewModel>, IEnumerable<TEntity>>(viewModels);
                _baseRepository.AddRange(entities);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddRangeAsync(IEnumerable<TViewModel> viewModels)
        {
            try
            {
                var entities = _mapper.Map<IEnumerable<TEntity>>(viewModels);
                _baseRepository.AddRangeAsync(entities);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public TViewModel Update(TViewModel viewModel)
        {
            try
            {
                var entity = _mapper.Map<TEntity>(viewModel);
                entity = _baseRepository.Update(entity);
                return _mapper.Map<TViewModel>(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete(long id)
        {
            try
            {
                _baseRepository.Delete<TEntity>(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                _baseRepository.Delete(predicate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Any()
        {
            try
            {
                return _baseRepository.Any<TEntity>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return _baseRepository.Any(predicate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public TViewModel Get(long id)
        {
            try
            {
                var entity = _baseRepository.Get<TEntity>(id);
                return _mapper.Map<TViewModel>(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public TViewModel Get(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                var entity = _baseRepository.Get(predicate);
                return _mapper.Map<TViewModel>(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public long GetLastId()
        {
            try
            {
                return _baseRepository.GetLastId<TEntity>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<TViewModel> All()
        {
            try
            {
                var entities = _baseRepository.All<TEntity>();
                return _mapper.Map<IEnumerable<TViewModel>>(entities);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<TViewModel> Find(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                var entities = _baseRepository.Find(predicate);
                return _mapper.Map<IEnumerable<TViewModel>>(entities);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}