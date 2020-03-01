using Contracts;
using System;
using System.Linq;
using Entities.Data;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class RepositoryBase<T> : IRepositroyBase<T> where T : class
    {
        protected RepositryContext _repositryContext;
        public RepositoryBase(RepositryContext repositryContext)
        {
            _repositryContext = repositryContext;
        }
        public void Create(T entity) => _repositryContext.Set<T>().Add(entity);
        public void Delete(T entity) => _repositryContext.Set<T>().Remove(entity);
        public void Update(T entity) => _repositryContext.Set<T>().Update(entity);

        public IQueryable<T> FindAll(bool trackChanges) =>
            !trackChanges ? _repositryContext.Set<T>().AsNoTracking() : _repositryContext.Set<T>();

        public IQueryable<T> FindByCondtion(System.Linq.Expressions.Expression<Func<T, bool>> expression, bool trackChanges) =>
            !trackChanges ? _repositryContext.Set<T>().Where(expression).AsNoTracking() : _repositryContext.Set<T>().Where(expression);
    }
}
