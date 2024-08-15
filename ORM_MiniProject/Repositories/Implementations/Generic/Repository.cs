using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using ORM_MiniProject.Contexts;
using ORM_MiniProject.Repositories.Interfaces.Generic;
using System;
using System.Linq.Expressions;

namespace ORM_MiniProject.Repositories.Implementations.Generic
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        public Repository()
        {
            _context = new AppDbContext();
        }
        public async Task CreateAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public async Task<List<T>> GetAllAsync(params string[] includes)
        {
            var query = _context.Set<T>().AsQueryable();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            var result = await query.ToListAsync();
            return result;
        }

        public async Task<T?> GetAsync(Expression<Func<T, bool>> expression, params string[] includes)
        {
            var query =  _context.Set<T>().AsQueryable();
            foreach (var include in includes)
            { 
                  query = query.Include(include);
            }
            var result = await query.FirstOrDefaultAsync(expression);
            return result;
        }

        public async Task<List<T>> GetFilterAsync(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().Where(expression).ToListAsync();
        }

        public async Task<bool> IsExistAsync(Expression<Func<T, bool>> expression)
        {
            var result = await _context.Set<T>().AnyAsync(expression);
            return result;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

    }
}
