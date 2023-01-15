using Microsoft.EntityFrameworkCore;
using SimpleQueue.Data;
using SimpleQueue.Domain.Interfaces;
using System.Linq.Expressions;

namespace SimpleQueue.Services
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected SimpleQueueDBContext _context;

        public RepositoryBase(SimpleQueueDBContext context)
        {
            _context = context;
        }

        public IQueryable<T> FindAll(bool trackChanges) => 
            !trackChanges ?
            _context.Set<T>()
            .AsNoTracking() :
            _context.Set<T>();

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges) => 
            !trackChanges ?
            _context.Set<T>()
           .Where(expression)
            .AsNoTracking() :
            _context.Set<T>()
            .Where(expression);

        public void Create(T entity) => _context.Set<T>().Add(entity);
        public void Update(T entity) => _context.Set<T>().Update(entity);
        public void Delete(T entity) => _context.Set<T>().Remove(entity);
    }
}
