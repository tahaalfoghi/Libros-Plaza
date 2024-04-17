using Microsoft.EntityFrameworkCore;
using MvcApp.EFCore.Data;
using MvcApp.EFCore.Models;
using System.Linq.Expressions;

namespace MvcApp.Services
{
    public class ShoppingCartRepository:IRepository<ShoppingCart>
    {
        private readonly AppDbContext _context;

        public ShoppingCartRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(ShoppingCart entity)
        {
            await _context.Set<ShoppingCart>().AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var record = await _context.Set<ShoppingCart>().FindAsync(id);
            _context.Set<ShoppingCart>().Remove(record);
        }

        public async Task<IEnumerable<ShoppingCart>> GetAllAsync(string? includes = null)
        {
            IQueryable<ShoppingCart> query = _context.Set<ShoppingCart>();
            if (!string.IsNullOrEmpty(includes))
            {
                foreach (var i in includes
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(i);
                }
            }
            return await query.ToListAsync();
        }

        public async Task<ShoppingCart> GetAsync(Expression<Func<ShoppingCart, bool>> predicate, string? includes = null)
        {
            IQueryable<ShoppingCart> query = _context.Set<ShoppingCart>().Where(predicate);
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

        public async Task<ShoppingCart> GetByIdAsync(int id, string? includes = null)
        {
            IQueryable<ShoppingCart> query = _context.Set<ShoppingCart>();
            if (!string.IsNullOrEmpty(includes))
            {
                foreach (var item in includes
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(int id, ShoppingCart entity)
        {
            var record = await _context.Set<ShoppingCart>().FindAsync(id);
            _context.Entry(record).CurrentValues.SetValues(entity);
        }
    }
}
