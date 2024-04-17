using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcApp.EFCore.Data;
using MvcApp.EFCore.Models;
using System.Linq.Expressions;

namespace MvcApp.Services
{
    public sealed class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly AppDbContext _context;

        public RepositoryBase(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(TEntity entity)
        {
            var record = entity;
            await _context.Set<TEntity>().AddAsync(record);
        }

        public async Task DeleteAsync(int id)
        {
            var record = await _context.Set<TEntity>().FindAsync(id);
            _context.Set<TEntity>().Remove(record);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(string? includes = null)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();
               
            if (!string.IsNullOrEmpty(includes))
            {
                foreach (var i in includes
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(i);
                }
            }
            return query.ToList();
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, string? includes = null)
        {
            return await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }

        public async Task<TEntity> GetByIdAsync(int id, string? includes = null)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (!string.IsNullOrEmpty(includes))
            {
                foreach (var i in includes
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(i);
                }
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(int id, TEntity entity)
        {
            var record = await GetByIdAsync(id);
            _context.Entry(record).CurrentValues.SetValues(entity);
        }
    }
}
