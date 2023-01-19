﻿using Microsoft.EntityFrameworkCore;
using SimpleQueue.Domain.Interfaces;
using System.Linq.Expressions;

namespace SimpleQueue.Data
{
    public abstract class EFRepositoryBase<T> : IEFRepositoryBase<T> where T : class
    {
        protected DbContext _context;

        public EFRepositoryBase(DbContext context)
        {
            _context = context;
        }

        public IQueryable<T> FindAll(bool trackChanges = true) => 
            !trackChanges ?
            _context.Set<T>()
            .AsNoTracking() :
            _context.Set<T>();

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = true) => 
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