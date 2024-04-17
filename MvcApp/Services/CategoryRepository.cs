
using Microsoft.EntityFrameworkCore;
using MvcApp.EFCore.Data;
using MvcApp.EFCore.Models;
using System.Linq.Expressions;

namespace MvcApp.Services
{
    public class CategoryRepository: IRepository<Category>
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Category entity)
        {
            await _context.Set<Category>().AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var record =await _context.Set<Category>().FindAsync(id);
            _context.Set<Category>().Remove(record);
        }
            
        public async Task<IEnumerable<Category>> GetAllAsync(string? includes = null)
        {
            return await _context.Set<Category>().ToListAsync();
        }

        public async Task<Category> GetAsync(Expression<Func<Category, bool>> predicate, string? includes = null)
        {
            IQueryable<Category> query = _context.Set<Category>();
            return await query.FirstOrDefaultAsync(predicate);
        }

        public async Task<Category> GetByIdAsync(int id, string? includes = null)
        {
            return await _context.Set<Category>().FindAsync(id);
        }

        public async Task UpdateAsync(int id, Category entity)
        {
            var record = await _context.Set<Category>().FindAsync(id);
            _context.Entry(record).CurrentValues.SetValues(entity);
        }
    }
}
